using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using Microsoft.SharePoint.Navigation;

namespace Officience.SharePointHelper
{
    public class FVIntranet : FormFunction, IFormFunction
    {
        /*
        Note:   To add new function, only need copy code 'AddFunctions("Your functions").Click'
                and the press ' += ' and press TAB to past add your funtions, then type your 
                function name and press TAB to auto generate your function. After that you can
                write your code like my demo function.
        */
        public void DefineFunctions() //IFormFunctions
        {
            //AddFunctions("TestQueryByUser").Click += new EventHandler(TestQueryByUser);
            AddFunctions("[TN] Move document to new list").Click += new EventHandler(MoveDocumentToNewList);
            AddFunctions("[TN] Test Query").Click += new EventHandler(TestQuery);
        }

        void TestQuery(object sender, EventArgs e)
        {
            SPList list = List("WorkFlowConfig");
            SPQuery query = new SPQuery();
            query.Query = "<Where><Or><Eq><FieldRef Name='Read' /><Value Type='UserMulti'><UserID Type='Integer' /></Value></Eq><Membership Type='CurrentUserGroups'><FieldRef Name='Read'/></Membership></Or></Where>";
            SPListItemCollection items = list.GetItems(query);
            WriteLine(items.Count.ToString());
        }

        void MoveDocumentToNewList(object sender, EventArgs e)
        {
            OptionsDialog optionsDialog = new OptionsDialog();
            MoveFileOptions yourOptions = new MoveFileOptions();
            optionsDialog.Options.SelectedObject = yourOptions;
            if (optionsDialog.ShowDialog() == DialogResult.OK)
            {
                MoveDocumentToNewList(yourOptions);
            }
        }
        void MoveDocumentToNewList(MoveFileOptions moveFileOptions)
        {
            SPList docLibCurrent = List(moveFileOptions.From);
            SPList docLibNew = List(moveFileOptions.To);
            SPList listCommittee = Web.ParentWeb.Lists["Committee"];
            SPList listDeparment = Web.ParentWeb.Lists["Department"];
            SPList listDocumentTypes = Web.ParentWeb.Lists["Document Types"];
            SPList listJCIStandardChapters = Web.ParentWeb.Lists["JCI Standard Chapters"];
            Dictionary<string, string> contentTypeConvert = InitContentTypeConvert();
            Dictionary<int, string> committeeConvert = new Dictionary<int, string>();
            Dictionary<int, string> deparmentConvert = new Dictionary<int, string>();
            Dictionary<int, string> documentTypeConvert = new Dictionary<int, string>();
            Dictionary<int, string> jciStandardChapterConvert = new Dictionary<int, string>();
            RelatedDocumentConvert relatedDocumentConvert = new RelatedDocumentConvert();
            WriteLine("Delete file in current item");
            int itemCount = docLibNew.ItemCount - 1;
            ProgressBarInit("Delete file in current item", itemCount);
            for (int i = itemCount; i > 0; i--)
            {
                docLibNew.Items.Delete(i);
                ProgressBarNext();
            }
            WriteLine("Add file from docLibCurrent to docLibNew");
            //SPListItem currentFileItem = docLibCurrent.GetItemById(45);//1220);
            
            ProgressBarInit("Add file from docLibCurrent to docLibNew", docLibCurrent.ItemCount - 1);
            foreach (SPListItem currentFileItem in docLibCurrent.Items)
            {
                try
                {
                    SPFile currentFile = currentFileItem.File;
                    string fileName = currentFile.Name;
                    currentFile.CopyTo(docLibNew.RootFolder.Url + "/" + fileName, true);
                    SPListItem newFileItem = docLibNew.GetItems(GetQueryByFileName(fileName))[0];
                    //DMSubject	=>	Title
                    newFileItem["Title"] = currentFileItem["DMSubject"];

                    //isConvert	=>	IsConvert
                    bool isConvert = Common.ToBoolean(currentFileItem["isConvert"]);
                    newFileItem["IsConvert"] = isConvert;

                    //BelongTo  =>  ContentType
                    string belongTo = Common.ToString(currentFileItem["BelongTo"]);
                    string docIcon = Common.ToString(currentFileItem["DocIcon"]);

                    if (!string.IsNullOrEmpty(belongTo) && (docIcon.Equals("pdf") || isConvert == false))
                        newFileItem["ContentType"] = contentTypeConvert[belongTo.ToLower()];
                    else
                        newFileItem["ContentType"] = "Document";

                    //AllocatedCommittee	=>	AllocatedCommitteeLookup
                    string committee = Common.ToString(currentFileItem["AllocatedCommittee"]);
                    if (!String.IsNullOrEmpty(committee))
                        newFileItem["AllocatedCommitteeLookup"] = ConvertToLookupCollection(committeeConvert, listCommittee, committee);

                    //AllocatedDeparment	=>	AllocatedDeparmentLookup
                    string deparment = Common.ToString(currentFileItem["AllocatedDeparment"]);
                    if (!String.IsNullOrEmpty(deparment))
                        newFileItem["AllocatedDeparmentLookup"] = ConvertToLookupCollection(deparmentConvert, listDeparment, deparment);

                    //DocumentType	=>	DocumentTypeLookup
                    string documentType = Common.ToString(currentFileItem["DocumentType"]);
                    if (!String.IsNullOrEmpty(documentType))
                        newFileItem["DocumentTypeLookup"] = ConvertToLookupCollection(documentTypeConvert, listDocumentTypes, documentType);

                    //JCIStandardChapter	=>	JCIStandardChapterLookup
                    string jciStandardChapter = Common.ToString(currentFileItem["JCIStandardChapter"]);
                    if (!String.IsNullOrEmpty(jciStandardChapter))
                        newFileItem["JCIStandardChapterLookup"] = ConvertToLookupCollection(jciStandardChapterConvert, listJCIStandardChapters, jciStandardChapter);

                    //WhoNeedToKnow	=>	WhoNeedToKnowGroup <-- FV update
                    //newFileItem["RelatedDocuments"] = currentFileItem["RelatedDocuments"];
                    if (!String.IsNullOrEmpty(Common.ToString(currentFileItem["RelatedDocuments"])))
                        relatedDocumentConvert.Items.Add(new RelatedDocumentItem(newFileItem, currentFileItem));
                    newFileItem.SystemUpdate();
                    WriteLine("ADD: [{0}] '{1}'", newFileItem.ID, fileName);
                }
                catch (Exception ex)
                {
                    WriteLine("ERROR: [{0}] {1}", currentFileItem.ID, ex.Message);
                    WriteLine(ex.StackTrace);
                }
                ProgressBarNext();
            }
            WriteLine("Update related documents");
            ProgressBarInit("Update related documents", relatedDocumentConvert.Items.Count - 1);
            foreach (RelatedDocumentItem item in relatedDocumentConvert.Items)
            {
                SPListItem newFileItem = docLibNew.GetItemById(item.NewID);
                string log = relatedDocumentConvert.GetNewRelatedDocuments(item);
                if (String.IsNullOrEmpty(log))
                {
                    newFileItem["RelatedDocuments"] = item.NewRelatedDocuments;
                    newFileItem.SystemUpdate();
                    WriteLine("Update: [{0}] '{1}'", item.NewID, item.FileName);
                }
                else
                {
                    WriteLine("Error: [{0}] '{1}'", item.NewID, log);
                }
                ProgressBarNext();
            }
        }

        private Dictionary<string, string> InitContentTypeConvert()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("official documents", "Official Document");
            result.Add("legal documents", "Legal Document");
            result.Add("legal library", "Legal Library");
            return result;
        }

        private SPFieldLookupValueCollection ConvertToLookupCollection(Dictionary<int, string> dictionaryCacheItem, SPList lookupList, string lookupIds)
        {
            SPFieldLookupValueCollection result = new SPFieldLookupValueCollection();
            List<string> lookupIdsSplit = lookupIds.Split(new char[]{'|'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string id in lookupIdsSplit)
            {
                int lookupId = Common.ToInt(id);
                if (!dictionaryCacheItem.ContainsKey(lookupId))
                    dictionaryCacheItem.Add(lookupId, lookupList.GetItemById(lookupId).Title);
                result.Add(new SPFieldLookupValue(lookupId, dictionaryCacheItem[lookupId]));
            }
            return result;
        }

        private SPQuery GetQueryByFileName(string fileName)
        {
            SPQuery result = new SPQuery();
            result.Query = String.Format(@"<Where>
                                  <Eq>
                                     <FieldRef Name='FileLeafRef' />
                                     <Value Type='File'>{0}</Value>
                                  </Eq>
                               </Where>", fileName);
            return result;
        }

        void TestQueryByUser(object sender, EventArgs e)
        {
            string query =
               @"<Where>
                  <IsNotNull>
                     <FieldRef Name='ID' />
                  </IsNotNull>
               </Where>";
            using (SPSite site = new SPSite(Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList listDocuments = web.Lists["AllDocuments"];
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = query;
                    SPListItemCollection spListItemCol = listDocuments.GetItems(spQuery);
                    if (spListItemCol.Count != 0)
                    {
                        foreach (SPListItem item in spListItemCol)
                        {
                            WriteLine("ID = '{0}'; FileName = '{1}';", item.ID, item["FileLeafRef"]);
                        }
                    }
                    else
                    {
                        WriteLine("Item not found!");
                    }
                }
            }
			
            //using (SPWeb web = SPContext.Current.Site.OpenWeb("Docs"))
            //{
            //    string query =
            //       @"<Where>
            //      <IsNotNull>
            //         <FieldRef Name='ID' />
            //      </IsNotNull>
            //   </Where>";
            //    SPList listDocuments = web.Lists["AllDocuments"];
            //    SPQuery spQuery = new SPQuery();
            //    spQuery.Query = query;
            //    SPListItemCollection spListItemCol = listDocuments.GetItems(spQuery);
            //    if (spListItemCol.Count != 0)
            //    {
            //        foreach (SPListItem item in spListItemCol)
            //        {
            //            lblSeperator.Text += String.Format("ID = '{0}'; FileName = '{1}';", item.ID, item["FileLeafRef"]);
            //        }
            //    }
            //    else
            //    {
            //        lblSeperator.Text += "Item not found!";
            //    }
            //    lblSeperator.Visible = true;
            //}
        }

        #region Version 2007
        //public void FVIntranet_DefineFunctions()
        //{
        //    //Add your function here to contine...
        //    AddFunctions("[TRI] Copy DMSubject to DMSubjectTitle").Click += new EventHandler(CopyDMSubjectToDMSubjectTitle);
        //    AddFunctions("[TRI] Remove related to itself").Click += new EventHandler(RemoveRelatedToItself);
        //    AddFunctions("[TRI] Add char '|' to Reader for old data").Click += new EventHandler(AddStartAndEndForReader);
        //    AddFunctions("[TRI] Copy RelatedDocuments to ItemsRelated").Click += new EventHandler(CopyRelatedDocumentsToItemsRelated);
        //    AddFunctions("[TRI] Check error documents").Click += new EventHandler(CheckErrorDocuments);
        //    //AddFunctions("[TRI] Site map tree update").Click += new EventHandler(SiteMapTreeUpdate);
        //    AddFunctions("[TRI] Overwrite existing file").Click += new EventHandler(OverwriteExistingFile);
        //}
        //void OverwriteExistingFile(object sender, EventArgs e)
        //{            
        //    OptionsDialog optionsDialog = new OptionsDialog();
        //    OverwriteExistingFileOptions yourOptions = new OverwriteExistingFileOptions();
        //    //Map your class with form OptionsDialog
        //    optionsDialog.Options.SelectedObject = yourOptions;

        //    //ShowDialog
        //    if (optionsDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            if (File.Exists(yourOptions.UploadFile))
        //            {
        //                FileStream fStream = new FileStream(yourOptions.UploadFile, System.IO.FileMode.Open);
        //                byte[] fileContents = new byte[(int)fStream.Length];
        //                fStream.Read(fileContents, 0, (int)fStream.Length);
        //                fStream.Close();
        //                SPList docLib = List("Documents");
        //                SPListItem currentFileItem = docLib.GetItemById(yourOptions.ExistingFileID);
        //                SPFile currentFile = currentFileItem.File;
        //                string fileName = Path.GetFileName(yourOptions.UploadFile);
        //                currentFileItem["Title"] = fileName;
        //                currentFile.SaveBinary(fileContents);
        //                currentFile.MoveTo(docLib.RootFolder.Url + "/" + fileName, true);
        //                currentFile.Item["Name"] = fileName;
        //                currentFile.Update();
        //            }
        //            else
        //                throw new Exception("FILE_NOT_EXIST_CANOT_UPLOAD_TO_SHAREPOINT");

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}

        //#region Rebuild file name
        //public string RebuildFileName(string fileName, string docLibUrl)
        //{
        //    //Check special characters not allowed in SP filename
        //    if (SPSpecialCharacters.Any(c => fileName.Contains(c)))
        //    {
        //        foreach (char c in SPSpecialCharacters)
        //            fileName = fileName.Replace(c, '_');
        //    }
        //    //Check length of file name url cannot be longer than 128 characters
        //    int maxLength = 123 - docLibUrl.Length; //123 = 128 - 5 (5 is max length of ext of file name)
        //    if (fileName.Length > maxLength)
        //    {
        //        fileName = fileName.Remove(maxLength);
        //    }
        //    //Rules for the period character
        //    if (fileName.Contains('.'))
        //    {
        //        if (fileName.Contains("..")) fileName = fileName.Replace("..", "_");
        //        if (fileName.StartsWith(".")) fileName = fileName.Remove(0, 1);
        //        if (fileName.EndsWith(".")) fileName = fileName.Remove(fileName.Length - 2);
        //    }
        //    return fileName;
        //}

        //public static readonly char[] SPSpecialCharacters = new char[] { '~', '"', '#', '%', '&', '*', ':', '<', '>', '?', '/', '\\', '{', '|', '}' };
        //#endregion Rebuild file name

        //#region Site map tree update
        //void SiteMapTreeUpdate(object sender, EventArgs e)
        //{
        //    NodeContents(Web.Navigation.TopNavigationBar);
        //    NodeContents(Web.Navigation.GlobalNodes);
        //}

        //private void NodeContents(SPNavigationNodeCollection nodeCollection)
        //{
        //    foreach (SPNavigationNode node in nodeCollection)
        //    {
        //        if(node.Parent != null)
        //            WriteLine("Title = '{0}'; IsVisible = '{1}', Url= '{2}';", node.Title, node.IsVisible, node.Url);
        //        NodeContents(node.Children);
        //    }
        //}
        //#endregion

        //#region Check error documents
        //void CheckErrorDocuments(object sender, EventArgs e)
        //{
        //    SPList listDocuments = List("Documents");
        //    WriteLine("Start read list items");
        //    string queryFormat = "<Where><Eq><FieldRef Name='sourceDoc' /><Value Type='Text'>{0}</Value></Eq></Where>";
        //    string fileLocalFormat = Path.GetDirectoryName(Application.ExecutablePath);
        //    fileLocalFormat += String.Format(@"\Documents_{0}", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-ff"));
        //    Directory.CreateDirectory(fileLocalFormat);
        //    fileLocalFormat += @"\{0}";
        //    int i = 0;
        //    foreach (SPListItem item in listDocuments.Items)
        //    {
        //        try
        //        {
        //            if (String.IsNullOrEmpty(Common.ToString(item["BelongTo"])))
        //            {
        //                SPQuery spQuery = new SPQuery();
        //                spQuery.Query = String.Format(queryFormat, Common.ToString(item["FileLeafRef"]));
        //                SPListItemCollection spListItemCol = listDocuments.GetItems(spQuery);
        //                if (spListItemCol.Count == 0)
        //                {
        //                    WriteLine("ID = '{0}'; FileName = '{1}';", item.ID, item["FileLeafRef"]);
        //                    SPFile spFile = item.File;
        //                    byte[] bytes = spFile.OpenBinary();
        //                    FileStream fs = new FileStream(String.Format(fileLocalFormat, spFile.Name), FileMode.Create, FileAccess.ReadWrite);
        //                    BinaryWriter bw = new BinaryWriter(fs);
        //                    bw.Write(bytes);
        //                    bw.Close();
        //                    i++;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLine("ERROR: {0}", ex.Message);
        //            WriteLine(ex.StackTrace);
        //        }
        //    }
        //    WriteLine("FilesError.Count = '{0}'", i);
        //}
        //#endregion

        //#region Copy RelatedDocuments to ItemsRelated
        //void CopyRelatedDocumentsToItemsRelated(object sender, EventArgs e)
        //{
        //    SPList listDocuments = List("Documents");
        //    WriteLine("Start read list items");
        //    foreach (SPListItem item in listDocuments.Items)
        //    {
        //        try
        //        {
        //            string related = Common.ToString(item["RelatedDocuments"]);
        //            if (!String.IsNullOrEmpty(related))
        //            {
        //                WriteLine("item.AllRelatedDocuments = '{0}'", related);
        //                item["AllRelatedDocuments"] = related;
        //                item.SystemUpdate();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLine("ERROR: {0}", ex.Message);
        //            WriteLine(ex.StackTrace);
        //        }
        //    }
        //}
        //#endregion

        //#region Add char '|' to Reader for old data
        //void AddStartAndEndForReader(object sender, EventArgs e)
        //{
        //    SPList listDocuments = List("Documents");
        //    WriteLine("Start read list items");
        //    foreach (SPListItem item in listDocuments.Items)
        //    {
        //        try
        //        {
        //            bool update = false;
        //            string reader = Common.ToString(item["Reader"]);
        //            if (!String.IsNullOrEmpty(reader))
        //            {
        //                WriteLine("item.Reader = '{0}'", reader);
        //                if (!reader.StartsWith("|"))
        //                {
        //                    reader = String.Format("|{0}|", reader);
        //                    item["Reader"] = reader;
        //                    update = true;
        //                    WriteLine("item.Reader.Update = '{0}'", reader);
        //                }
        //            }

        //            string DocumentType = Common.ToString(item["DocumentType"]);
        //            if (!String.IsNullOrEmpty(DocumentType))
        //            {
        //                WriteLine("item.DocumentType = '{0}'", DocumentType);
        //                if (!DocumentType.StartsWith("|"))
        //                {
        //                    DocumentType = String.Format("|{0}|", DocumentType);
        //                    item["DocumentType"] = DocumentType;
        //                    update = true;
        //                    WriteLine("item.DocumentType.Update = '{0}'", DocumentType);
        //                }
        //            }

        //            string AllocatedDeparment = Common.ToString(item["AllocatedDeparment"]);
        //            if (!String.IsNullOrEmpty(AllocatedDeparment))
        //            {
        //                WriteLine("item.AllocatedDeparment = '{0}'", AllocatedDeparment);
        //                if (!AllocatedDeparment.StartsWith("|"))
        //                {
        //                    AllocatedDeparment = String.Format("|{0}|", AllocatedDeparment);
        //                    item["AllocatedDeparment"] = AllocatedDeparment;
        //                    update = true;
        //                    WriteLine("item.AllocatedDeparment.Update = '{0}'", AllocatedDeparment);
        //                }
        //            }

        //            if (update) item.SystemUpdate();
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLine("ERROR: {0}", ex.Message);
        //            WriteLine(ex.StackTrace);
        //        }
        //    }
        //}
        //#endregion

        //#region [TRI] Remove related to itself
        //void RemoveRelatedToItself(object sender, EventArgs e)
        //{
        //    SPList listDocuments = List("Documents");
        //    WriteLine("Start read list items");
        //    foreach (SPListItem item in listDocuments.Items)
        //    {
        //        try
        //        {
        //            if (item["RelatedDocuments"] != null)
        //            {
        //                WriteLine("item.ID = '{0}'", item.ID);
        //                SPFieldLookupValueCollection relatedDocuments = Common.ToLookupValueCollection(item["RelatedDocuments"]);
        //                bool existRelatedDocumentsToItself = relatedDocuments.Exists(rd => rd.LookupId == item.ID);
        //                WriteLine("existRelatedDocumentsToItself = '{0}'", existRelatedDocumentsToItself);
        //                if (existRelatedDocumentsToItself)
        //                {
        //                    WriteLine(" + item.RelatedDocuments.Before = '{0}'", relatedDocuments.ToString());
        //                    relatedDocuments.Remove(relatedDocuments.First(t => t.LookupId == item.ID));
        //                    WriteLine(" + item.RelatedDocuments.After = '{0}'", relatedDocuments.ToString());
        //                    item["RelatedDocuments"] = relatedDocuments;
        //                    item.SystemUpdate();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLine("ERROR: {0}", ex.Message);
        //            WriteLine(ex.StackTrace);
        //        }
        //    }
        //}
        //#endregion

        //#region [TRI] Copy DMSubject to DMSubjectTitle
        //void CopyDMSubjectToDMSubjectTitle(object sender, EventArgs e)
        //{
        //    OptionsDialog optionsDialog = new OptionsDialog();
        //    CopyDMSubjectOptions yourOptions = new CopyDMSubjectOptions();
        //    //Map your class with form OptionsDialog
        //    optionsDialog.Options.SelectedObject = yourOptions;

        //    //ShowDialog
        //    if (optionsDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        SPList listDocuments = List("Documents");
        //        WriteLine("Start copy data");
        //        string columnValue = yourOptions.ReuseValueDMSubjectTitle ? "DMSubjectTitle" : "DMSubject";
        //        foreach (SPListItem item in listDocuments.Items)
        //        {
        //            try
        //            {
        //                WriteLine("+ id = '{0}'", item.ID);
        //                string dmSubject = Common.ToString(item[columnValue]);
        //                if(yourOptions.OverwriteDMSubjectTitle) item["DMSubjectTitle"] = dmSubject;
        //                if(yourOptions.StrimByLength)
        //                    item["DMSubject"] = GetSubjectTitle(dmSubject, yourOptions.Length);
        //                else
        //                    item["DMSubject"] = LongTextNoBR(dmSubject, yourOptions.Length);
        //                item.SystemUpdate();
        //            }
        //            catch (Exception ex)
        //            {
        //                WriteLine("ERROR: {0}", ex.Message);
        //                WriteLine(ex.StackTrace);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        WriteLine("Cancel function");
        //    }
        //}
        //public string GetSubjectTitle(string value, int length)
        //{
        //    if (value.Length <= length) return value;
        //    value = value.Substring(0, length);
        //    value = value.Substring(0, value.LastIndexOf(' ')) + "...";
        //    return value;
        //}
        //public string LongTextNoBR(string value, float widthMax)
        //{
        //    FontFamily ff = new FontFamily("verdana");
        //    Font f = new Font(ff, 8, GraphicsUnit.Point);
        //    return LongTextNoBR(value, f, widthMax * 2);
        //}
        //public string LongTextNoBR(string text, Font f, float widthMax)
        //{
        //    if (text.Trim() == string.Empty)
        //        return string.Empty;

        //    Bitmap b = new Bitmap(1, 1);
        //    Graphics g = Graphics.FromImage(b);
        //    string newText = text.Replace("\r\n", " ");

        //    SizeF sf = g.MeasureString(newText, f);
        //    if (sf.Width <= widthMax)
        //        return newText;

        //    string[] arrayLetter = newText.Split(new char[] { ' ' });
        //    StringBuilder sb = new StringBuilder();
        //    float width = 0;
        //    for (int i = 0; i < arrayLetter.Length; i++)
        //    {
        //        if (arrayLetter[i] != string.Empty)
        //        {
        //            string s = arrayLetter[i] + " ";
        //            SizeF sf2 = g.MeasureString(s, f);
        //            width += sf2.Width;
        //            if (width < widthMax)
        //                sb.Append(s);
        //            else
        //                return sb.Append("...").ToString();
        //        }
        //    }
        //    return sb.ToString();
        //}
        //#endregion
        #endregion Version 2007
    }
    #region Others class
    public class RelatedDocumentConvert
    {
        public List<RelatedDocumentItem> Items { get; set; }
        public RelatedDocumentConvert() { Items = new List<RelatedDocumentItem>(); }

        public string GetNewRelatedDocuments(RelatedDocumentItem item)
        {
            string log = "";
            foreach (SPFieldLookupValue lookupItem in item.OldRelatedDocuments)
            {
                try
                {
                    item.NewRelatedDocuments.Add(new SPFieldLookupValue(Items.First(i => i.OldId == lookupItem.LookupId).NewID, lookupItem.LookupValue));
                }
                catch
                {
                    log += String.Format("[{0}#{1}];", lookupItem.LookupId, lookupItem.LookupValue);
                }
            }
            return log;
        }
    }
    public class RelatedDocumentItem
    {
        public int NewID { get; set; }
        public int OldId { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public SPFieldLookupValueCollection NewRelatedDocuments { get; set; }
        public SPFieldLookupValueCollection OldRelatedDocuments { get; set; }
        public RelatedDocumentItem() { NewRelatedDocuments = new SPFieldLookupValueCollection(); OldRelatedDocuments = new SPFieldLookupValueCollection(); }
        public RelatedDocumentItem(SPListItem newItem, SPListItem oldItem)
        {
            //newItem["RelatedDocuments"] = oldItem["RelatedDocuments"];
            NewID = newItem.ID;
            OldId = oldItem.ID;
            Title = newItem.Title;
            FileName = Common.ToString(newItem["FileLeafRef"]);
            NewRelatedDocuments = new SPFieldLookupValueCollection(); //calc after
            OldRelatedDocuments = Common.ToLookupValueCollection(oldItem["RelatedDocuments"]);
        }
    }
    public class MoveFileOptions
    {
        [CategoryAttribute("Config"), Description("From")]
        public string From { get; set; }

        [CategoryAttribute("Config"), Description("To")]
        public string To { get; set; }

        public MoveFileOptions()    //Init default values
        {
            From = "Documents";
            To = "AllDocuments";
        }
    }
    #region Version 2007
    //public class OverwriteExistingFileOptions
    //{
    //    [CategoryAttribute("Config"), Description("Upload File")]
    //    public string UploadFile { get; set; }

    //    [CategoryAttribute("Config"), Description("Existing File ID")]
    //    public int ExistingFileID { get; set; }

    //    public OverwriteExistingFileOptions()    //Init default values
    //    {

    //    }
    //}

    //public class CopyDMSubjectOptions
    //{
    //    [CategoryAttribute("Config"), Description("Overwrite DMSubjectTitle"), DefaultValue(true)]
    //    public bool OverwriteDMSubjectTitle { get; set; }

    //    [CategoryAttribute("Config"), Description("WidthMax or NumberChars value"), DefaultValue(248)]
    //    public int Length { get; set; }

    //    [CategoryAttribute("Option"), Description("Re-Use Value DMSubjectTitle"), DefaultValue(false)]
    //    public bool ReuseValueDMSubjectTitle { get; set; }

    //    [CategoryAttribute("Option"), Description("Strim by length"), DefaultValue(false)]
    //    public bool StrimByLength { get; set; }

    //    public CopyDMSubjectOptions()    //Init default values
    //    {
    //        OverwriteDMSubjectTitle = true;
    //        Length = 248; //320
    //        ReuseValueDMSubjectTitle = StrimByLength = false;
    //    }
    //}
    #endregion Version 2007
    #endregion
}
