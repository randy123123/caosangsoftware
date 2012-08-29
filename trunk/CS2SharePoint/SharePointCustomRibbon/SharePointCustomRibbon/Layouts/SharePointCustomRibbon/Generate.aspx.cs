using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Specialized;
using CarlosAg.ExcelXmlWriter;
using ClosedXML.Excel;
using System.Web;
using System.IO;
using Microsoft.SharePoint.Administration;

namespace SharePointCustomRibbon.Layouts.SharePointCustomRibbon
{
    public partial class Generate : LayoutsPageBase
    {
        //SPWeb currentSPWeb;
        SPSite currentSPSite;

        public string list;
        public string view;
        public Guid listID;
        public const string DATE_FORMAT = "dd-MMM-yyyy";
        protected void Page_Load(object sender, EventArgs e)
        {
            //SPWeb rootWebSite = SPContext.Current.Site.RootWeb;
            //SPWeb currentSPWeb = SPContext.Current.Web;
            //currentSPContext = SPContext.Current;
            //currentSPWeb = currentSPContext.Web;

            if (Request.QueryString["list"] != null && Request.QueryString["view"] != null && Request.QueryString["url"] != null && Request.QueryString["office"] != null)
            {

                list = Request.QueryString["list"];
                view = Request.QueryString["view"];
                currentSPSite = new SPSite(Request.QueryString["url"]);

                listID = new Guid(list);

                if (Request.QueryString["office"].ToString() == "2003")
                    GenerateExcel2003(listID, view, currentSPSite);
                else if (Request.QueryString["office"].ToString() == "2010")
                    GenerateExcel2010(listID, view, currentSPSite);

            }
            else
            {

            }


            //Prehladanie celej site collection a najdenie listu
            //SPList currentList;

            //SPWebApplication webApplication = SPContext.Current.Site.WebApplication;
            //SPSiteCollection siteCollections = webApplication.Sites;

            //foreach (SPSite site in siteCollections)
            //{
            //    currentSPWeb = site.RootWeb;
            //    SPListCollection listCollection = currentSPWeb.Lists;
            //    foreach (SPList list in listCollection)
            //    {
            //        if (list.ID == listID)
            //        {
            //            currentList = list;
            //            break;
            //        }
            //    }
            //    site.Close();
            //}
        }


        private void GenerateExcel2010(Guid listID, string view, SPSite site)
        {

            SPWeb currentSPWeb = site.OpenWeb();

            SPList currentList = currentSPWeb.Lists[listID];
            SPView currentView = currentList.Views[view];
            SPQuery query = new SPQuery();
            query.ViewFields = currentView.ViewFields.SchemaXml;
            SPListItemCollection oItemCol = currentList.GetItems(query);// (currentView);
            SPViewFieldCollection collViewFields = currentView.ViewFields;
            StringCollection stringCol = currentView.ViewFields.ToStringCollection();

            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(currentList.Title);

            int row = 1;
            int column = 0;

            foreach (String viewField in stringCol)
            {
                SPField field = currentList.Fields.GetField(viewField);
                column++;
                worksheet.Cell(row, column).Value = field.Title;
            }

            int rowData = 1;
            int columData = 1;

            foreach (SPListItem item in oItemCol)
            {
                rowData++;
                foreach (String viewField in stringCol)
                {
                    SPField field = currentList.Fields.GetField(viewField);                    
                    if (item[field.Title] == null)
                        worksheet.Cell(rowData, columData).Value = "";
                    else if (field.Type == SPFieldType.DateTime)
                        worksheet.Cell(rowData, columData).Value = ((DateTime?)item[field.Title]).Value.ToString(DATE_FORMAT);
                    else if (field.Type == SPFieldType.Lookup)
                    {
                        SPFieldLookupValueCollection lkc = new SPFieldLookupValueCollection(item[field.Title].ToString());
                        string itemText = lkc[0].LookupValue;
                        for (int i = 1; i < lkc.Count; i++)
                            itemText = string.Format("; {0}", lkc[i].LookupValue);
                        worksheet.Cell(rowData, columData).Value = itemText;
                    }
                    else if (field.Type == SPFieldType.User)
                    {
                        SPFieldLookupValueCollection lkc = new SPFieldLookupValueCollection(item[field.Title].ToString());
                        string itemText = GetName(lkc[0].LookupValue);
                        for (int i = 1; i < lkc.Count; i++)
                            itemText = string.Format("; {0}", GetName(lkc[i].LookupValue));
                        worksheet.Cell(rowData, columData).Value = itemText;
                    }
                    else
                        worksheet.Cell(rowData, columData).Value = item[field.Title];

                    //if (field.FieldValueType.BaseType == typeof(System.DateTime))
                    //{
                    //    worksheet.Cell(rowData, columData).Style.DateFormat.Format = "yyyy-MM-dd";
                    //}

                    columData++;
                }

                var rngTable = worksheet.Range(rowData, 1, rowData, columData - 1);
                if (rowData % 2 == 0)
                    rngTable.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#DCE6F1")).Font.SetFontName("Calibri").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetBottomBorderColor(XLColor.FromHtml("#95B3D7"));
                else
                    rngTable.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#FFFFFF")).Font.SetFontName("Calibri").Font.SetFontSize(11).Border.SetBottomBorder(XLBorderStyleValues.Thin).Border.SetBottomBorderColor(XLColor.FromHtml("#95B3D7"));

                columData = 1;
            }

            var rngTitle = worksheet.Range(1, 1, row, column);
            rngTitle.Style.Font.Bold = true;
            rngTitle.Style.Font.FontName = "Calibri";
            rngTitle.Style.Font.FontSize = 11;
            rngTitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            rngTitle.Style.Font.FontColor = XLColor.White;
            rngTitle.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");

            var rngData = worksheet.Range(1, 1, rowData, column);
            var excelTable = rngData.CreateTable();

            worksheet.Columns().AdjustToContents();

            //Ulozenie suboru
            string outFileName = currentList.Title + "_[" + currentView.Title + "]_" + System.DateTime.Now.Date.ToShortDateString().Replace('/', '-') + ".xlsx";

            HttpResponse httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            httpResponse.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", outFileName));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.Flush();
            httpResponse.End();
        }

        private void GenerateExcel2003(Guid listID, string view, SPSite site)
        {

            SPWeb currentSPWeb = site.OpenWeb();

            //Context.Response.Write("<script type=\"text/ecmascript\" language=\"ecmascript\">SP.UI.Notify.addNotification('Hello World!', true);</script>");
            SPList currentList = currentSPWeb.Lists[listID];
            SPView currentView = currentList.Views[view];
            SPQuery query = new SPQuery();
            query.ViewFields = currentView.ViewFields.SchemaXml;
            SPListItemCollection oItemCol = currentList.GetItems(query);// (currentView);
            SPViewFieldCollection collViewFields = currentView.ViewFields;
            StringCollection stringCol = currentView.ViewFields.ToStringCollection();

            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets.Add(currentList.Title);

            //Definovanie stylu pre Stlpce
            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 11;
            style.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style.Font.Color = "White";
            style.Interior.Color = "#4F81BD";
            style.Interior.Pattern = StyleInteriorPattern.Solid;

            //Pridanie Stlpcov
            WorksheetRow rows = sheet.Table.Rows.Add();
            foreach (String viewField in stringCol)
            {
                SPField field = currentList.Fields.GetField(viewField);
                WorksheetCell cell = rows.Cells.Add(field.Title);
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                cell.StyleID = "HeaderStyle";
            }


            //Pridanie hodnot
            foreach (SPListItem item in oItemCol)
            {
                rows = sheet.Table.Rows.Add();
                foreach (String viewField in stringCol)
                {
                    SPField field = currentList.Fields.GetField(viewField);
                    if (item[field.Title] == null)
                        rows.Cells.Add("");
                    else if (field.Type == SPFieldType.DateTime)
                        rows.Cells.Add(new WorksheetCell(((DateTime?)item[field.Title]).Value.ToString(DATE_FORMAT), DataType.String));
                    else if (field.Type == SPFieldType.Lookup)
                    {
                        SPFieldLookupValueCollection lkc = new SPFieldLookupValueCollection(item[field.Title].ToString());
                        string itemText = lkc[0].LookupValue;
                        for (int i = 1; i < lkc.Count; i++)
                            itemText = string.Format("; {0}", lkc[i].LookupValue);
                        rows.Cells.Add(new WorksheetCell(itemText, DataType.String));   
                    }
                    else if (field.Type == SPFieldType.User)
                    {
                        SPFieldLookupValueCollection lkc = new SPFieldLookupValueCollection(item[field.Title].ToString());
                        string itemText = GetName(lkc[0].LookupValue);
                        for (int i = 1; i < lkc.Count; i++)
                            itemText = string.Format("; {0}", GetName(lkc[i].LookupValue));
                        rows.Cells.Add(new WorksheetCell(itemText, DataType.String));   
                    }
                    else
                        rows.Cells.Add(new WorksheetCell(item[field.Title].ToString(), DataType.String));                    
                }
            }

            //Ulozenie suboru
            string outFileName = currentList.Title + "_[" + currentView.Title + "]_" + System.DateTime.Now.Date.ToShortDateString().Replace('/', '-') + ".xls";

            HttpResponse httpResponse = Response;
            httpResponse.Clear();


            httpResponse.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", outFileName));
            httpResponse.ContentType = "Application/x-msexcel";

            using (MemoryStream memoryStream = new MemoryStream())
            {
                book.Save(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.Flush();
            httpResponse.End();
        }


        public string GetName(string value)
        {
            if (value.Contains("|"))
                return value.Substring(value.LastIndexOf('|') + 1);
            else if (value.Contains("\\"))
                return value.Substring(value.LastIndexOf('\\') + 1);
            else if (value.Contains("#"))
                return value.Substring(value.LastIndexOf('#') + 1);
            else return value;
        }
    }
}
