using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using Microsoft.SharePoint.Navigation;
using CSSoft;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;



namespace Officience.SharePointHelper
{
    public static class TechToolsPath
    {
        public const string BidFileTemplate = @"C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\techtools\Templates\Bid Support.xls";

        public static string TempPath { get { return Path.Combine(Path.GetTempPath(), "TechTools"); } }//String.Format(@"{0}TechTools\", Path.GetTempPath()); } }
        public static string UserData { get { return Path.Combine(TempPath, "UserData"); } }
    }

    public class Regions
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Technology> Technology { get; set; }
        public Regions() { Technology = new List<Technology>(); }
    }
    public class Technology
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class YourClassStruct : FormFunction, IFormFunction
    {
        /*
        Note:   To add new function, only need copy code 'AddFunctions("Your functions").Click'
                and the press ' += ' and press TAB to past add your funtions, then type your 
                function name and press TAB to auto generate your function. After that you can
                write your code like my demo function.
        */
        public void DefineFunctions() //IFormFunctions
        {
            //AddMenu("[your-class-struct] Test").Click += new EventHandler(Test);
            //AddMenu("[your-class-struct][1.0] Dynamic add menu").Click += new EventHandler(DynamicAddMenu);
            //AddMenu("Fillter Init for IBNF").Click += new EventHandler(FillterInitForIBNF);
            //AddMenu("Change Date Format").Click += new EventHandler(ChangeDateFormat);
            //AddMenu("Test").Click += new EventHandler(Test);
            //AddMenu("UpdateData").Click += new EventHandler(UpdateData);
            //AddMenu("Update GMap Latitude and Longitude").Click +=new EventHandler(UpdateGMapValues);
            //AddMenu("Debug Code").Click += new EventHandler(DebugCode);
            //AddMenu("Change request #9916").Click += new EventHandler(Fix9916);
            AddMenu("Update GMap Latitude and Longitude").Click += new EventHandler(Update_TIMDI_GMapValues);
        }

        void Fix9916(object sender, EventArgs e)
        {
            foreach (SPListItem item in List("AllDocuments").Items)
            {
                item["RenewalDate"] = null;
                item.SystemUpdate();
                //DateTime? approvalDate = CS2Convert.ToDateTime(item["ApprovalDate"]);
                //if (approvalDate != null)
                //{
                //    DateTime renewalDate = approvalDate.Value.AddYears(3).AddMonths(-2); //Review date = Approval date + 3 years - 2 months
                //    WriteLine("{0}: ApprovalDate['{1}'] => RenewalDate['{2}']", item.Title, approvalDate.Value, renewalDate);
                //    item["RenewalDate"] = renewalDate;
                //    item.SystemUpdate();
                //}
            }
        }

        /// <summary>
        /// Debugs the code.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void DebugCode(object sender, EventArgs e)
        {
            string path = @"C:\Users\Administrator\Desktop\test.htm";
            HTMLToPdf(File.ReadAllText(path), Path.Combine(Path.GetDirectoryName(path), "test.pdf"));

            //File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(path), "test.pdf"), "");

            //string MonthFormat = "MMM-yyyy";
            //string value = DateTime.Today.ToString(MonthFormat);
            //DateTime reValue = DateTime.Parse(value);

            //List<char> FileNameSpecialChars = null;
            //if (FileNameSpecialChars == null)
            //{
            //    FileNameSpecialChars = new List<char>();
            //    FileNameSpecialChars.AddRange(System.IO.Path.GetInvalidFileNameChars());
            //    FileNameSpecialChars.AddRange(System.IO.Path.GetInvalidPathChars());
            //    FileNameSpecialChars.Distinct();
            //}

            //string g = "";
            //int i = 0;
            //int n = 50;
            //List<string> group = new List<string>();
            //for (int t = 0; t < n; t++)
            //{
            //    g += String.Format("{0},", t);
            //    if (i++ % 5 == 4)
            //    {
            //        group.Add(g);
            //        g = "";
            //    }
            //}
            //if (i % 5 > 0)
            //{
            //    group.Add(g);
            //}
        }

        public void HTMLToPdf(string HTML, string FileOutput)
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(FileOutput, FileMode.Create));
            document.Open();
            HTMLWorker hw = new HTMLWorker(document);
            hw.Parse(new StringReader(HTML));
            document.Close();
        }

        void Update_TIMDI_GMapValues(object sender, EventArgs e)
        {
            SPQuery query = new SPQuery();
            query.Query = "<Where><And><Or><IsNull><FieldRef Name='Latitude' /></IsNull><Eq><FieldRef Name='Latitude' /><Value Type='Number'>0</Value></Eq></Or><Or><IsNull><FieldRef Name='Longitude' /></IsNull><Eq><FieldRef Name='Longitude' /><Value Type='Number'>0</Value></Eq></Or></And></Where>";
            foreach (SPListItem item in List("SITE CORE SATIN").GetItems(query))//.Items)//
            {
                //string country = item.Title;
                string address = String.Format("{0}, {1}, {2}", CS2Convert.ToString(item["Street"]), CS2Convert.ToString(item["City"]), CS2Convert.ToString(item["Country"]));
                double latitude = 0;
                double longitude = 0;
                GetLocation(address, ref latitude, ref longitude);

                if (longitude != 0 && longitude != 0)
                {
                    item["Latitude"] = latitude;
                    item["Longitude"] = longitude;
                    item.SystemUpdate(false);
                }
            }
        }

        void UpdateGMapValues(object sender, EventArgs e)
        {
            SPQuery query = new SPQuery();
            query.Query = "<Where><And><Or><IsNull><FieldRef Name='Latitude' /></IsNull><Eq><FieldRef Name='Latitude' /><Value Type='Number'>0</Value></Eq></Or><Or><IsNull><FieldRef Name='Longitude' /></IsNull><Eq><FieldRef Name='Longitude' /><Value Type='Number'>0</Value></Eq></Or></And></Where>";
            foreach (SPListItem item in List("Access Reference").Items)//.GetItems(query))//
            {
                string country = item.Title;
                double latitude = 0;
                double longitude = 0;
                GetLocation(country, ref latitude, ref longitude);

                if (longitude != 0 && longitude != 0)
                {
                    item["Latitude"] = latitude;
                    item["Longitude"] = longitude;
                    item.SystemUpdate(false);
                }
            }
        }

        private void GetLocation(string address, ref double latitude, ref double longitude)
        {
            GetLocation(address, ref latitude, ref longitude, 0);
        }
        private void GetLocation(string address, ref double latitude, ref double longitude, int retry)
        {
            string geocoderUri = string.Format(@"http://maps.google.com/maps/api/geocode/xml?sensor=false&address={0}", address.Trim());
            XmlDocument doc = new XmlDocument();
            doc.Load(geocoderUri);
            XmlNode node = doc.SelectSingleNode("//lat");
            try
            {
                if (doc.SelectSingleNode(@"//status").InnerText == "OK")
                {
                    latitude = Double.Parse(doc.SelectSingleNode(@"//lat").InnerText);
                    longitude = Double.Parse(doc.SelectSingleNode(@"//lng").InnerText);
                    WriteLine("Address['{0}'] = [{1},{2}]", address, latitude, longitude);
                }
                else
                {
                    if (retry < 2)
                    {
                        Thread.Sleep(200);
                        GetLocation(address, ref latitude, ref longitude, ++retry);
                    }
                    else if (address.Contains('('))
                    {
                        GetLocation(address.Substring(0, address.LastIndexOf('(') - 1).Trim(), ref latitude, ref longitude);
                    }
                    else if (address.Contains(','))
                    {
                        GetLocation(address.Substring(0, address.LastIndexOf(',') - 1).Trim(), ref latitude, ref longitude);
                    }
                }
            }
            catch { }
        }

        void UpdateData(object sender, EventArgs e)
        {
            List<Regions> Regions = new List<Regions>();
            List<Technology> Technology = new List<Technology>();
            WriteLine("Read Technology");
            SPList ListTechnology = List("Technology");
            foreach (SPListItem item in ListTechnology.Items)
            {
                Technology.Add(new Technology { Id = item.ID, Title = item.Title });
            }

            WriteLine("Read Regions");
            SPList ListRegions = List("Regions");
            foreach (SPListItem item in ListRegions.Items)
            {
                Regions region = new Regions { Id = item.ID, Title = item.Title };
                SPFieldLookupValueCollection technology = CS2Convert.ToLookupValueCollection(item["Technology"]);
                if (technology != null)
                    foreach (SPFieldLookupValue lk in technology)
                        region.Technology.Add(new Technology() { Id = lk.LookupId, Title = lk.LookupValue });
                Regions.Add(region);
            }

            foreach (SPListItem item in List("Access Reference").Items)
            {
                string region = CS2Convert.ToString(item["Region"]).Trim();
                string techs = CS2Convert.ToString(item["TechAvailable"]).Trim();

                Regions itemRegion = Regions.FirstOrDefault(r => r.Title == region);
                SPListItem addRegion;
                if (itemRegion == null)
                {
                    addRegion = ListRegions.AddItem();
                    addRegion["Title"] = region;
                    addRegion.SystemUpdate(false);
                    itemRegion = new Regions() { Id = addRegion.ID, Title = addRegion.Title };
                    Regions.Add(itemRegion);
                }
                else
                {
                    addRegion = ListRegions.GetItemById(itemRegion.Id);
                }

                if (!String.IsNullOrEmpty(techs))
                {
                    foreach (string t in techs.Split(','))
                    {
                        string tech = t.Trim();
                        Technology itemTechnology = Technology.FirstOrDefault(i => i.Title == tech);
                        if (itemTechnology == null)
                        {
                            SPListItem addTechnology = ListTechnology.AddItem();
                            addTechnology["Title"] = tech;
                            addTechnology.SystemUpdate(false);
                            itemTechnology = new Technology() { Id = addTechnology.ID, Title = tech };
                            Technology.Add(itemTechnology);
                        }
                        if(!itemRegion.Technology.Any(tk => tk.Id == itemTechnology.Id))
                            itemRegion.Technology.Add(itemTechnology);
                    }
                }
                SPFieldLookupValueCollection lkTechnology = new SPFieldLookupValueCollection();
                foreach (Technology tech in itemRegion.Technology)
                {
                    lkTechnology.Add(new SPFieldLookupValue(tech.Id, tech.Title));
                }
                if (lkTechnology.Count > 0)
                {
                    addRegion["Technology"] = lkTechnology;
                    addRegion.SystemUpdate(false);
                }
            }
        }

        void Test(object sender, EventArgs e)
        {
            string f = "=IF(D5=\"\",\" \",A4+1, A44, BA42, D5, A$4, $A4, $A$4)";
            List<string> s = new List<string>(Matches(f, @"\$?(?:\bXF[A-D]|X[A-E][A-Z]|[A-W][A-Z]{2}|[A-Z]{2}|[A-Z])\$?(?:104857[0-6]|10485[0-6]\d|1048[0-4]\d{2}|104[0-7]\d{3}|10[0-3]\d{4}|[1-9]\d{1,5}|[1-9])d?\b"));
            int c = s.Count;

            f = "-a-b-c-d-";
            s = new List<string>(Substring(f,"-","-"));
            c = s.Count;

            s = new List<string>(Substring2(f,"-","-"));
            c = s.Count;
        }

        public IEnumerable<string> Substring(string inputText, string startWithText, string endWithText)
        {
            string regularExpressionPattern = String.Format(@"{0}(.*?){1}", startWithText, endWithText);
            return Matches(inputText, regularExpressionPattern);
        } 

        public IEnumerable<string> Substring2(string inputText, string startWithText, string endWithText)
        {
            string regularExpressionPattern = String.Format(@"{0}(.*?){1}", startWithText, endWithText);
            Regex re = new Regex(regularExpressionPattern);
            foreach (Match m in re.Matches(inputText))
                yield return m.Groups[1].Value;
        } 
        public IEnumerable<string> Matches(string inputText, string regularExpressionPattern)
        {
            Regex re = new Regex(regularExpressionPattern);
            foreach (Match m in re.Matches(inputText))
                yield return m.Value;
        }
        //void ChangeDateFormat(object sender, EventArgs e)
        //{
        //    CultureAndRegionInfoBuilder carib = new CultureAndRegionInfoBuilder("nb-NO", CultureAndRegionModifiers.Replacement);
        //    carib.LoadDataFromCultureInfo(new CultureInfo("lv-LV"));
        //    carib.LoadDataFromRegionInfo(new RegionInfo("lv"));
        //    carib.GregorianDateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";
        //    carib.Register();
        //    CultureInfo ci = new CultureInfo("nb-NO");
        //    Web.Locale = ci;
        //    Web.Update();
        //}

        //void DynamicAddMenu(object sender, EventArgs e)
        //{
        //    DisableMenu(((ToolStripItem)sender).Text); //Disable click parent menu again
        //    AddMenu("[your-class-struct][1.1] Menu 1").Click +=new EventHandler(MenuClick);
        //    AddMenu("[your-class-struct][1.2] Menu 2").Click +=new EventHandler(MenuClick);
        //    AddMenu("[your-class-struct][1.3] Menu 3").Click +=new EventHandler(MenuClick);
        //}
        //
        //void MenuClick(object sender, EventArgs e)
        //{
        //    WriteLine("Menu item '{0}' clicked.", ((ToolStripItem)sender).Text);
        //}


        void FillterInitForIBNF(object sender, EventArgs e)
        {
            int munberItem = List("Order").ItemCount + List("Circuit").ItemCount;

            ProgressBarInit("Demo function with progressBar", munberItem);

            UpdateOrderList();
            UpdateCircuitList();

            ProgressBarClear();
        }

        private void UpdateOrderList()
        {
            WriteLine("UpdateOrderList()");
            SPList orderList = List("Order");
            SPListItemCollection orderItems = orderList.Items;
            foreach (SPListItem order in orderItems)
            {
                WriteLine("Order[{0}]: '{1}'", order.ID, order.Title);
                ProgressBarNext();

                orderList = List("Order");

                SPFieldLookupValue fieldLookupValue = null;
                string countryA = string.Empty;
                string countryB = string.Empty;
                string serviceProvider = string.Empty;
                string customerName = string.Empty;
                string ftProduct = string.Empty;

                fieldLookupValue = CS2Convert.ToLookupValue(order[new Guid(OrderFields.CountryA)]);
                if (fieldLookupValue != null)
                {
                    countryA = fieldLookupValue.LookupValue;
                    order[new Guid(OrderFields.OrderCountryA)] = countryA;
                }

                fieldLookupValue = CS2Convert.ToLookupValue(order[new Guid(OrderFields.CountryB)]);
                if (fieldLookupValue != null)
                {
                    countryB = fieldLookupValue.LookupValue;
                    order[new Guid(OrderFields.OrderCountryB)] = countryB;
                }

                fieldLookupValue = CS2Convert.ToLookupValue(order[new Guid(OrderFields.Supplier)]);
                if (fieldLookupValue != null)
                {
                    serviceProvider = fieldLookupValue.LookupValue;
                    order[new Guid(OrderFields.OrderServiceProvider)] = serviceProvider;
                }

                fieldLookupValue = CS2Convert.ToLookupValue(order[new Guid(OrderFields.EndCustomerName)]);
                if (fieldLookupValue != null)
                {
                    customerName = fieldLookupValue.LookupValue;
                    order[new Guid(OrderFields.OtherEndCustomerName)] = customerName;
                }

                fieldLookupValue = CS2Convert.ToLookupValue(order[new Guid(OrderFields.FTProduct)]);
                if (fieldLookupValue != null)
                {
                    ftProduct = fieldLookupValue.LookupValue;
                    order["MappingField:FTproduct"] = ftProduct;
                }

                order[new Guid(OrderFields.OtherPlaceThrough)] = order[new Guid(OrderFields.Orderchannel)];
                order[new Guid(OrderFields.OrderNetwork)] = order[new Guid(OrderFields.Network)];
                order["OrderUsage"] = order[new Guid(OrderFields.Usage)];

                SPFieldMultiChoice fieldMultiChoice = (SPFieldMultiChoice)orderList.Fields.GetFieldByInternalName("FilterByCountry");
                if (!fieldMultiChoice.Choices.Contains(countryA) && !string.IsNullOrEmpty(countryA))
                    fieldMultiChoice.Choices.Add(countryA);
                if (!fieldMultiChoice.Choices.Contains(countryB) && !string.IsNullOrEmpty(countryB))
                    fieldMultiChoice.Choices.Add(countryB);
                fieldMultiChoice.Update();
                order["FilterByCountry"] = countryA + ";#" + countryB;

                SPFieldChoice fieldChoice = (SPFieldChoice)orderList.Fields.GetFieldByInternalName("FilterByCustomerName");
                if (!fieldChoice.Choices.Contains(customerName) && !string.IsNullOrEmpty(customerName))
                    fieldChoice.Choices.Add(customerName);
                fieldChoice.Update();
                order["FilterByCustomerName"] = customerName;

                fieldChoice = (SPFieldChoice)orderList.Fields.GetFieldByInternalName("FilterByFTProduct");
                if (!fieldChoice.Choices.Contains(ftProduct) && !string.IsNullOrEmpty(ftProduct))
                    fieldChoice.Choices.Add(ftProduct);
                fieldChoice.Update();
                order["FilterByFTProduct"] = ftProduct;

                fieldChoice = (SPFieldChoice)orderList.Fields.GetFieldByInternalName("FilterByServiceProvider");
                if (!fieldChoice.Choices.Contains(serviceProvider) && !string.IsNullOrEmpty(serviceProvider))
                    fieldChoice.Choices.Add(serviceProvider);
                fieldChoice.Update();
                order["FilterByServiceProvider"] = serviceProvider;

                order.SystemUpdate();
            }
        }

        private void UpdateCircuitList()
        {
            WriteLine("UpdateCircuitList()");
            SPList orderList = List("Order");
            SPList circuitList = List("Circuit");
            SPListItemCollection circuitItems = circuitList.Items;
            foreach (SPListItem circuit in circuitItems)
            {
                WriteLine("UpdateCircuit = '{0}' '{1}'", circuit.ID, circuit.Title);
                ProgressBarNext();
                try
                {
                    //Get Order Item link to Circuit
                    SPFieldLookupValue ftReference = CS2Convert.ToLookupValue(circuit[new Guid(CircuitFields.FTReference)]);
                    if (!string.IsNullOrEmpty(ftReference.LookupValue))
                    {
                        SPListItem order = orderList.GetItemById(ftReference.LookupId);
                        
                        circuitList = List("Circuit");

                        string countryA = CS2Convert.ToLookupValue(order[new Guid(OrderFields.CountryA)]) != null ? CS2Convert.ToLookupValue(order[new Guid(OrderFields.CountryA)]).LookupValue : "";
                        string countryB = CS2Convert.ToLookupValue(order[new Guid(OrderFields.CountryB)]) != null ? CS2Convert.ToLookupValue(order[new Guid(OrderFields.CountryB)]).LookupValue : "";
                        SPFieldMultiChoice fieldMultiChoice = (SPFieldMultiChoice)circuitList.Fields.GetFieldByInternalName("FilterByCountry");
                        if (!fieldMultiChoice.Choices.Contains(countryA) && !string.IsNullOrEmpty(countryA))
                            fieldMultiChoice.Choices.Add(countryA);
                        if (!fieldMultiChoice.Choices.Contains(countryB) && !string.IsNullOrEmpty(countryB))
                            fieldMultiChoice.Choices.Add(countryB);
                        fieldMultiChoice.Update();
                        circuit["FilterByCountry"] = countryA + ";#" + countryB;

                        string customerName = CS2Convert.ToLookupValue(order[new Guid(OrderFields.EndCustomerName)]) != null ? CS2Convert.ToLookupValue(order[new Guid(OrderFields.EndCustomerName)]).LookupValue : "";
                        SPFieldChoice fieldChoice = (SPFieldChoice)circuitList.Fields.GetFieldByInternalName("FilterByCustomerName");
                        if (!fieldChoice.Choices.Contains(customerName) && !string.IsNullOrEmpty(customerName))
                            fieldChoice.Choices.Add(customerName);
                        fieldChoice.Update();
                        circuit["FilterByCustomerName"] = customerName;

                        string ftProduct = CS2Convert.ToLookupValue(order[new Guid(OrderFields.FTProduct)]) != null ? CS2Convert.ToLookupValue(order[new Guid(OrderFields.FTProduct)]).LookupValue : "";
                        fieldChoice = (SPFieldChoice)circuitList.Fields.GetFieldByInternalName("FilterByFTProduct");
                        if (!fieldChoice.Choices.Contains(ftProduct) && !string.IsNullOrEmpty(ftProduct))
                            fieldChoice.Choices.Add(ftProduct);
                        fieldChoice.Update();
                        circuit["FilterByFTProduct"] = ftProduct;

                        string serviceProvider = CS2Convert.ToLookupValue(order[new Guid(OrderFields.Supplier)]) != null ? CS2Convert.ToLookupValue(order[new Guid(OrderFields.Supplier)]).LookupValue : "";
                        fieldChoice = (SPFieldChoice)circuitList.Fields.GetFieldByInternalName("FilterByServiceProvider");
                        if (!fieldChoice.Choices.Contains(serviceProvider) && !string.IsNullOrEmpty(serviceProvider))
                            fieldChoice.Choices.Add(serviceProvider);
                        fieldChoice.Update();
                        circuit["FilterByServiceProvider"] = serviceProvider;

                        circuit.SystemUpdate();
                    }
                }
                catch (Exception)
                { }
            }
        }
    }
    public static class OrderFields
    {
        #region Item Base Content Type

        //The following Guids can be found in the ctypewss.xml file locatedin the 
        //%programfiles"\common files\microsoft shared\web server extensions\12\TEMPLATE\FEATURES\cytpes folder.
        public const string Id = "1D22EA11-1E32-424E-89AB-9FEDBADB6CE1";
        public const string Title = "FA564E0F-0C70-4AB9-B863-0177E6DDD247";

        #endregion

        public const string RequestType = "a8b1f2ee-f588-4771-b53e-cfc11afd567c";
        public const string Segment = "5c8b6b50-b331-4d32-84db-ad248aa83774";
        public const string EquantReference = "ccfb23ec-6225-46c7-a918-ef129847cfd6";
        public const string PresalesID = "91b9475c-0661-4a5e-af28-8ee8c48f6954";
        public const string RequestSubmittedDate = "04e5be85-3fb2-4d18-a016-056ba1a63bc4";
        public const string EndCustomerName = "1f9716d3-d346-4be1-a926-ee92fa1bbade";
        public const string CustomerRequestedDate = "95e63cdc-66bf-460d-9b6c-4949bbf7644f";
        public const string ProjectManager = "e47dcdb2-273a-4708-9930-1d695ce92555"; //ProjectManagerUser
        public const string ProjectManagerEmail = "6290b75a-5d69-4fe2-9462-c45518772d94";
        public const string ProjectManagerContactNumber = "b1c02be1-9b2b-4c99-864f-f75d53e1b7c5";
        public const string StopBillingDate = "92c65281-525c-48b4-b53d-6e83139b1a82";
        public const string RequestedStopBillingDate = "211be847-6da0-4711-97e3-f00ec504718a";
        public const string InitialOrderReference = "ffead789-57b6-4eff-8f23-d0581e4e3587";
        public const string SupplierInitialReference = "184d83f3-432b-4b55-833b-4ff84fc44ee0";
        public const string InServiceDate = "5207761f-e52a-4db9-bde8-8c97dae92a25";
        public const string SupplierTechnicalReference = "26849385-5d80-4f11-9dfa-8008af75c04b";
        public const string EquantInitialReference = "016b2a04-11ad-40b7-b69a-010ed6a8bad1";
        public const string InitialOrderTerm = "ec406093-4b5d-4e40-848d-9c9ed51e0d16";
        public const string Purchaseditem = "d86e77ce-210a-4440-be12-048ffe2feebe";
        public const string Requestor = "783d8f9d-1c08-4df3-a165-85c381e3cf59"; //RequestorUser
        public const string RequestedEntity = "35c46b5a-5a02-4c96-807a-9a019007e339";
        public const string Network = "6adb4685-419f-4867-9c19-3042f9f7a301";
        public const string Usage = "deabe58f-c718-4f5d-af9e-00f27db00aba";
        public const string FTProduct = "ee08f226-3ccd-407c-8ee1-9bb9dab2aa41";
        public const string SiteIDA = "f0ce58c9-09b5-4cd9-a2f2-46a617e8f486";
        public const string SiteIDB = "6eda894a-7be8-4234-bc58-2402be2fca2b";
        public const string CustomerDefinedCode = "d9921741-1810-483f-9de5-bfb840fa3307";
        public const string NodeA = "4aaf68f4-a1c2-48f7-898c-23437737c092";
        public const string AddressA = "ba8953cc-c9bd-41ad-9f8c-947d44092747";
        public const string PostalCodeA = "fdccc954-f7eb-4be0-9744-8be27dd9d3cd";
        public const string CityA = "8478c2ab-1074-413e-8da3-14e5c1af43d9";
        public const string ProvinceStateA = "5d401fd2-1580-4664-bce0-37e386433980";
        public const string BuildingFloorA = "91b88039-aa3c-49d7-ad96-9aa5fb1df176";
        public const string RoomSuiteA = "35c9a328-e53b-4af9-b0b4-deebb882ed3f";
        public const string CountryA = "6567bb15-d766-47d2-b6d5-8418dec6beb1";
        public const string SiteContactNameA = "d1bc4dd6-366a-4f46-a5c2-d5fccea5bae1";
        public const string SiteContactPhoneA = "3264242c-efb0-41aa-a665-526b3f631551";
        public const string SiteContactEmailA = "fa6e4a22-9163-4ec6-a2c3-4fb7f23e1c7a";
        public const string WorkingRowLineupA = "acce6b40-eead-4547-abbc-a0158483181b";
        public const string WorkingCabinetBayRackA = "172db09b-50fc-411b-88a8-0e37f48c025e";
        public const string WorkingShelfTrayPanelA = "3bdbecaf-015e-4718-8bd0-447233297edc";
        public const string WorkingJackPortPositionA = "dfd068bb-6c49-474e-9dc0-3067108e9094";
        public const string ProtocolA = "3094da7c-5531-4b7b-805e-ae5c4fc65fec";
        public const string InterfaceTypeA = "58e312d3-0273-41e5-a398-3cd7f66ce417";
        public const string ConnectorTypeA = "89e185e0-cbd2-4570-a778-705edafac4e7";
        public const string HandoffProtectionA = "d8006622-56c6-44b6-8faa-c12b0b4b99e0";
        public const string ProtectionSchemeA = "d61690d0-eb0b-4db9-bfc1-bec04ea6d989";
        public const string ExtendedDemarcA = "b3a58dc2-42a2-45b2-9cd2-b6a6df9c5119";
        public const string LOACFA = "e3d231a8-210f-454e-bd86-b983da0917ad";
        public const string ProvidedByA = "50b60258-63e5-4be3-8733-35954965ae37";
        public const string NodeB = "b2b8ee14-adda-4a91-86eb-a1c4184e104c";
        public const string AddressB = "449c9651-c77c-405e-85e0-db848e059efb";
        public const string PostalCodeB = "43a5c69b-01c7-4379-8f69-5fb7a7675584";
        public const string CityB = "e0b4d8de-1d7e-4da2-bd39-8ff2afad48c1";
        public const string ProvinceStateB = "a36a90ac-d7c2-4b74-8c0c-a8cf23ed66df";
        public const string BuildingFloorB = "ccb9ec9c-7753-4b35-923f-ec3cfe3edd96";
        public const string RoomSuiteB = "6bcdf3aa-51b1-4235-90ca-e16544492714";
        public const string SiteContactNameB = "1afb6c72-1b59-4674-8e5e-37cf54e41703";
        public const string SiteContactPhoneB = "ba179134-3173-402d-b425-a41ccd3f8001";
        public const string SiteContactEmailB = "ae3fed7c-45ff-4d09-95b3-9967ef7d086d";
        public const string WorkingRowLineupB = "fd87239d-f250-42d4-be8e-41f6d1c76686";
        public const string WorkingCabinetBayRackB = "e57ee788-bc00-4f13-b123-4588d25b7023";
        public const string WorkingShelfTrayPanelB = "c006652a-0f1c-467f-901a-8635104543d4";
        public const string WorkingJackPortPositionB = "9e498ef2-2900-4a9e-8047-708bd5d4d8e0";
        public const string ProtocolB = "da407db5-e687-4a57-8ca0-432a1824bc2f";
        public const string InterfaceTypeB = "03201965-5fe7-4ea5-8007-ce213547267c";
        public const string ConnectorTypeB = "3b26cd7e-1a52-4516-9fe7-a49f462641d5";
        public const string HandoffProtectionB = "67190e4d-210e-4c23-a408-8e701b5a7ed0";
        public const string ProtectionSchemeB = "d82ec845-fe4a-4d7a-9b7a-90fb35ca5cc8";
        public const string ExtendedDemarcB = "05a70430-4c47-4593-833c-2a318f9b7c58";
        public const string LOACFB = "5c37cefc-135e-4dba-b421-21da6738af36";
        public const string ProvidedByB = "3711e255-32b5-4fdd-b547-981e4231ab41";
        public const string CountryB = "4e757ab4-c926-4387-bf3a-b01fe49afd5b";
        public const string CodeArticle = "1b6fc3f5-1087-4773-9400-79c5849b8c17";
        public const string CodeOperation = "8e7e0502-27a8-428f-bed1-5fd4fe6d0160";
        public const string CRD = "9b65d32b-c860-491c-9ca7-a1746a7f3337";
        public const string SesameIC01code = "431ebb16-63b6-4a2e-b574-256262b00af7";
        public const string GLcode = "3e3490f8-f042-4e46-865b-e83ebda404c6";
        public const string Mgtcode = "644ebb35-c828-4d38-a028-e367a0d9d41c";
        public const string ExpenseType = "ad01cc3e-0fa7-4d1e-9f79-e20d5a608098";
        public const string PriceFrom = "b3f38f2a-dc41-4f0f-ab17-77cd1c063fb8";
        public const string OtherPriceFrom = "ba2401ad-aeea-4c82-9ece-7586cb878c1d";
        public const string BlacklistedProvider = "d7d4baed-5215-4255-ae56-c79963c53b8f";
        public const string ServiceProvidersDeliveryInterval = "fdb64613-2373-4cc2-be4f-879fe25386a6";
        public const string WebALCRef = "d3ce9d7a-aba3-458a-a1fd-48b8cf1ad124";
        public const string SDsRequiredDeliveryDatef = "18130a27-8783-45ad-a68f-06a3676cccbb";
        /// <summary>
        /// Cost Periodicity
        /// </summary>
        public const string RecurringCost = "3b96802f-1230-4379-8bbf-7508353c0631";
        public const string Duration = "81aa7335-824d-4a1f-9b94-b4c91af9b74f";
        public const string OrderPurpose = "e59793d8-083d-4170-9347-151830b0a46b";
        public const string CircuitQuantity = "708f3f18-3c6e-4e62-b5a5-67bbe7c94865";
        public const string ContractType = "22286066-6ba0-4a38-9dd8-ad04be63a6b8";
        public const string CircuitType = "1661ff34-5609-49e4-b91e-f727daceb244";
        public const string ServiceRate = "fa0edea7-9637-43de-9875-f00eb0ea1b70";
        public const string OtherServiceRate = "cfe530b8-517f-4863-982a-1eb235043dee";
        public const string CircuitConfiguration = "4541415c-15fd-44cb-9667-210628b58615";
        public const string CableAssignmentLetterAttached = "0cebce3f-240c-44ba-ad4c-68f62280188d";
        public const string ServiceProtocol = "19a2f387-9711-444e-b535-d48debf11602";
        public const string OtherServiceProtocol = "b42950cf-6a4f-4052-b0ce-bb3cd1d17746";
        public const string PurposeofOverallServiceBeingProvided = "a0826086-e686-4377-8a51-4f66b0d2204d";
        public const string SupplierTestingRequirements = "d598aa12-ff31-4a3c-bda5-7cc8e3404cfa";
        public const string T1E1LineCoding = "1ec65da9-d7a5-4427-9e80-2b25fda8cc82";
        public const string DS3Framing = "15dc0a63-bf83-4775-bf4b-b9307bb58753";
        public const string Securisation = "7cc2cab8-6fc8-41ad-9691-5df5e5c35294";
        public const string Restorationoption = "b71a8420-15ac-4f58-bd8e-0148fe8b252c";
        public const string RoutingDiversity = "bb6d2543-bbae-4098-950a-8178a0adbbf0";
        public const string CablingDiversity = "4840056b-6a08-414c-95c7-e8b82368da9e";
        public const string CableName = "2dba1d48-b85e-490a-8ff8-4212ed3c9ff9";
        public const string EntireRouteDescription = "0ea7503e-c4f7-42c2-a930-7833b0eee505";
        /// <summary>
        /// Service Provider - Carrier
        /// </summary>
        public const string Supplier = "c0bb704e-5844-4c49-ba46-32a5f81bb3fe";
        public const string LocalCurrency = "ccdbdf57-e7dd-4da0-a8a5-af27f39a559e";
        public const string MRCLCY = "9f650d39-4a65-4089-adaa-110296b441a1";
        /// <summary>
        /// Non Recurring Cost
        /// </summary>
        public const string NRCLCY = "5fc173b2-55e8-4e68-b853-eec163c7b95a";
        public const string RequestCreationDate = "06d970e3-8864-4515-9297-dc64ee26ac04";
        public const string RequestedDeliveryDate = "9634c516-0552-4304-8f2f-f21455e7eeac";
        public const string Requirements = "129eb168-4105-438d-ad41-4fc58dc7d535";
        public const string Otherrequirement = "925d7bc4-8aee-4f38-82bd-32ddae63c479";
        public const string EquipementQuantity = "dd0d7d84-949b-4395-bc4a-747e2bea895c";
        public const string Equipmentweight = "bfe23c65-b913-4d34-905a-3e55f4b80fa2";
        public const string Equipmentheight = "e4e4e5a1-7ad9-4534-ab71-889bf6c427da";
        public const string Equipmentwidth = "c1874222-8dc8-438b-bc6c-0ba2028ad82a";
        public const string Equipmentdepth = "6b69a57f-a748-4d1d-95e1-693c1ef8f115";
        public const string PowerSupply = "dfe55b97-d08a-41ed-b85f-05b8291a6c10";
        public const string OtherSupply = "da93ab06-4080-4ef9-993a-fc6922d7a77d";
        public const string Breakersvalue = "b23ed890-0612-4af6-b544-0c085c2ae568";
        public const string Redundancy = "34c136d9-476c-4655-8e11-0a5f4959a0bf";
        public const string ForcastedPowerConsumption = "70e5dedc-b767-4708-b1d5-7d637a19c75e";
        public const string Validationstatus = "41a55a69-4c41-4ed1-967e-1234a73ac9cc";
        public const string Orderchannel = "8a20d6f6-c6fa-4e6f-97f4-fedc840cb1c7";
        public const string Validationstatus1 = "c7b58aa4-43bf-47f3-b6bd-ab68dbc0350a";
        public const string LOISOrderReference = "f724ae11-06d8-406f-8c6d-ff484dec032b";
        public const string USID = "f397ec51-0aa0-4537-b3b4-8b2c09b4f836";
        public const string POreference = "bfa75294-9211-45fd-a7b9-e9300af600aa";
        public const string PRreference = "9d6e8f61-e8e4-4fb8-b4b7-00b04b02013c";
        public const string Orderdate = "739d1a75-2325-4003-8ffd-2c7e1d63f9b2";
        public const string RequestComment = "ee560f25-6c87-401a-990e-51b11d6d81e7";
        public const string EquantOrderReference = "2d7fc60a-5109-4280-8114-6300ce6c831e";
        public const string EquantDisconnectionReference = "d824f1f0-80f2-4b80-859c-d91a41e712ea";
        public const string HousingComment = "b50af197-ccd4-4f29-8af7-ce167c9eb3f3";
        public const string CODEP = "108dd602-9d73-43d2-9088-dc4ff3a6df5e";
        public const string Address2A = "323c45f9-95be-49a4-ae8d-ffcf8dbb6c12";
        public const string Address2B = "c4e2ddf3-44e5-4496-a74b-2292bc68594b";
        public const string QuoteRefOrOther = "b1bada68-73e0-46b4-b2d9-e87f8f945ce5";
        public const string CustomerVSP = "9af5c98c-ed7e-499c-8b49-a2f5d1fe3a17";
        public const string TacitRenewal = "bf66005a-18f0-407a-97d7-f3a3722bab89";
        public const string ProtectRowLineupA = "4709762f-b1a2-4310-bbfb-5c4e2d7c88d2";
        public const string ProtectRowLineupB = "01c06ff5-ef2a-4481-8a9b-3b7999a2e828";
        public const string ProtectCabinetBayRackA = "73bdf39f-8ab6-4701-810c-e7c5e1eaea9c";
        public const string ProtectCabinetBayRackB = "e4600b00-e2f6-4432-9748-6aef60da16cf";
        public const string ProtectShelfTrayPanelA = "fc643ce5-4d83-4b49-b83e-4fd76a7bd111";
        public const string ProtectShelfTrayPanelB = "61830306-2e00-485e-8e5a-c5b273ce4c3d";
        public const string ProtectJackPortPositionA = "3ea6e4c7-719b-4c4a-bb5f-0d6bb2ff6a15";
        public const string ProtectJackPortPositionB = "23a2c5d3-4ecc-487d-b7b2-11fa0b237e2c";
        public const string TechnicalCommentsA = "018b9899-6705-4ecf-afbc-552e889b7128";
        public const string TechnicalCommentsB = "c93c12df-5422-4a3e-86b2-2a5bf10ae572";
        public const string ProjectCode = "{7100237c-3f67-49ad-8a90-d211649fc7bf}";
        public const string SiteIDALatitude = "c8779cf5-e1ac-4120-a8b0-d4f4c4177ee7";
        public const string SiteIDALongitude = "8f5fc892-2cb0-4654-9c5b-58e72be4d1d9";
        public const string SiteIDBLatitude = "2e669603-c5d1-4f98-a1c5-d3fa9cf272cd";
        public const string SiteIDBLongitude = "8443894b-ecf4-43b9-b752-da246dcc263e";
        public const string OrderCountryA = "d7736763-13a6-499e-9d6e-b71b5d587267";
        public const string OrderCountryB = "9c061a9a-fdc0-4922-a609-6343b62e57cf";
        public const string OrderNetwork = "a1229bae-0dc4-495f-bd6b-46c9638fec80";
        public const string OrderServiceProvider = "555dfd19-1b18-47a6-82d2-235e0205c3f2";
        public const string OtherEndCustomerName = "b5f22458-cc87-444f-a9d0-3ec77bfbce4f";
        public const string OtherPlaceThrough = "10056d93-9ad9-49eb-adbb-69e97e066058";
        //public const string OrderUsage = "c3e329fe-45d2-4f9a-bcf0-84726a0ff30e";
        public const string Package = "1a466497-29f8-4bc8-b48b-376b9b494769";
    }

    public static class CircuitFields
    {
        #region Item Base Content Type

        //The following Guids can be found in the ctypewss.xml file locatedin the 
        //%programfiles"\common files\microsoft shared\web server extensions\12\TEMPLATE\FEATURES\cytpes folder.
        public const string Id = "1D22EA11-1E32-424E-89AB-9FEDBADB6CE1";
        public const string Title = "FA564E0F-0C70-4AB9-B863-0177E6DDD247";

        #endregion

        public const string FTReference = "133E62C1-C83A-4BF8-B1C4-B9F6DEECE366";
        public const string CircuitID = "fa564e0f-0c70-4ab9-b863-0177e6ddd247";
        public const string OtherCircuitID = "443636D7-5DC3-4C1C-BB26-565E0E20252A";
        public const string Deliverydate = "3C45B195-DAF4-4A5F-A685-2EE5B9A64EB6";
        public const string TechnicalStartDate = "9464CBFD-715F-440E-BAAC-67582A84EF69";
        public const string StartBillingDate = "9F705B9F-2180-4435-BDF5-81294B28DD2F";
        public const string CircuitCurrency = "68de9d30-a983-4577-b8bd-25af8d7b4fa4";
        public const string MRC = "a30eecc0-02a0-409c-98ee-d915ba6eb60c";
        public const string NRC = "7b04ce3b-62cd-4abe-80bb-14221ea0f943";
        public const string TechnicalStopdate = "2C8AA040-63A0-4BC9-BA09-5BFD76138546";
        public const string StopBillingDate = "A2446EBF-34C4-44B4-8F7D-F8CABFDC32A7";
        public const string EUI = "B787CEF5-4D59-4C18-8243-DA8F8A337776";
        public const string FTSecondaryReference = "620a6a24-c7ba-49ac-bc05-c442afd12ea6";
        public const string ITUDesignation = "ff8c5a3a-f36e-4075-8817-3aa5271f2155";
        public const string TechnicalInformationA = "7e870374-cf0f-4e9b-a7cf-3e4b43bcf344";
        public const string TechnicalInformationB = "0cacb9b7-a8cd-4c2b-882e-d6601134a622";
        public const string CostTypology = "CostTypology";
        public const string YRC = "YRC";

        //Order //
        public const string OrderNodeA = "3f83cf47-c91d-4143-88ec-7da4b72393a0";
        public const string OrderAddressA = "0520fd40-6998-42fd-b5b5-ef9efb2243d0";
        public const string OrderCityA = "f56d5c9a-2a43-4b2e-80db-3a276a6e21e9";
        public const string OrderNodeB = "50b26505-3089-47ba-8727-bff9f2d37d78";
        public const string OrderAddressB = "7738d3ae-123c-4a94-a692-2c92ea2fc989";
        public const string OrderCityB = "159442bf-24b1-49f8-838e-4a751ad38d91";
        public const string OrderCodeArticle = "f0ffdd26-fa47-4765-a195-89c61a6b03b8";
        public const string OrderCodeOperation = "6b977aa3-a185-4e84-8ea0-abab6338991e";
        public const string OrderCRD = "eb104e0a-b11b-4e12-b92d-c14f384be574";
        public const string OrderSesameIC01code = "7d7a67c6-10db-4847-9904-b2fa4a4d947b";
        public const string OrderGLcode = "6c16ae10-1ba2-4523-afd1-7f80db3225ec";
        public const string OrderMgtcode = "b22639f1-9857-4b15-9717-b41ef9b248f4";
        public const string OrderITUDesignation = "ff8c5a3a-f36e-4075-8817-3aa5271f2155";
        public const string OrderEntireRouteDescription = "3d6513c6-54ce-4698-b5cb-bef18e2a23c6";
        public const string OrderLOISOrderReference = "f8db9614-5f81-48da-9855-549240a3abbe";
        public const string OrderUSID = "eac4347f-f61b-4fa4-89f6-b6843f3f8f7b";
        public const string OrderPOreference = "82e7d9f5-14b5-44b6-ad08-250286449d6f";
        public const string OrderPRreference = "721ebc20-f722-4681-8521-d4547a63786f";

        //DEPOT //
        public const string CircuitID_DEPOT = "832FC439-C10B-4D42-88D5-EAAA6C2BFFA7";
        public const string Carrier = "7EBE23E2-8A7E-4124-96D6-66604C36E456";
        public const string Currency_DEPOT = "58E6E73E-AF3D-4B6E-8FD3-6FE5EC4A492C";
        public const string LocalMRC = "73869F10-75FF-43EE-901D-8783440D7B0F";
        public const string MRCEUR = "B0436AAF-E646-4BB2-A1AA-81BEEC8DAF1E";
        public const string SumNRC = "6DC15F0B-4624-4F7F-AD00-B8468F784DD6";
        public const string SumNRCEUR = "45B14C16-9166-4B69-B38D-13DDF40C098C";
        public const string TIVID = "CD6BB3DB-139A-4DA9-BD0C-5A5A601345AA";
        public const string MgmtCode = "3FAB9DEA-34B5-4C4D-BCF8-E4AA167AAB7C";
        public const string MgmtCodeDescription = "0832A59D-FA00-4E8C-876E-EE816C1073E6";
        public const string MgmtCodeClass = "07EF518B-EA0E-4329-B900-44A049551E07";
        public const string GLAccountCode = "500D4509-0006-461E-9E6A-8A4AF6488CB6";
        public const string GLAccountService = "5DF85D22-BBC7-47D2-8383-89D9E330019E";
        public const string GLAccountDescription = "180CA883-89D1-4E41-B936-2C627C77F273";
        public const string IC01 = "75B3703A-8C0A-42B5-834B-69130E391148";
        public const string Customer = "2841B768-BDD4-444E-8279-5D31E9244D20";
        public const string Catnum = "158FB279-FAF6-4EF6-BB7C-0AB06DE319AD";
        public const string Category = "A9635B0E-5FD3-4785-9164-AA96C132A77A";
        public const string CatnumSince = "1E51FCE2-773D-4DC0-9896-417D27144294";
        public const string BEndCustomer = "20F135FF-8917-4BA6-A60A-73260CDDD66E";
        public const string Country = "99ACA430-96F0-4D33-9EC2-E23673551B38";
        public const string City = "0E41FA42-B33F-41E4-9EA7-B91BB219827C";
        public const string BEndAddress = "2B0A1057-F7A9-4B87-A6B8-15A7BCD6837C";
        public const string State = "65364A66-C82F-42AA-8E34-C9FFD0D292B9";
        public const string PostalCode = "0D1E0E59-6A54-481B-9572-7FADCF6CCD6A";
        public const string ReportingCountry = "3F1874C9-9C3B-4956-9197-C987D1D0ABB9";
        public const string CDC = "3C488FB9-FAA2-48B8-8AE9-8D2C858B26E9";

        // public const string SESOriginalOrderRef = "7BEE13E6-B392-4309-A7FB-698C7A596137";
        public const string SESOriginalOrderRef = "9b77a88e-7be2-4f98-b6a4-f85332b6ef7f";
        public const string CircuitSpeed = "7560190D-D0B8-494C-9AC1-87E015284F05";
        // public const string SESInitialQuoteID = "E2741C81-1C75-41E1-B160-5405631635B1";
        public const string SESInitialQuoteID = "5381f0e3-ca6c-448c-a4fd-1eab9bd4551b";
        public const string CarrierCircuitInstallDate = "8D0F7355-C807-4A52-A623-60F4BB3F5DA6";
        public const string SESCAVDate = "119C2DAB-1873-4C7C-A13D-97CACFDA18C0";

        public const string OperDiscoDate = "03d06d87-3072-489f-bd15-cb4eeda10529";
        public const string FinancialStopDate = "fcc670d6-1379-4721-a725-98673b5f5b68";
        public const string BBBMatch = "832678e7-7dd6-4b2a-94c1-9471fd86725f";
        public const string USID = "74d7ec00-f88a-47ce-8e77-cd44206062fa";
        public const string URN = "349cb7cc-d495-4197-8361-e33cb0904011";
        public const string OrderRef = "6887b588-99fd-41d6-90c0-c1467cb967b0";
        public const string NetworkBy = "91cdea83-6e2a-4355-a589-40fd16ab5ad3";
        public const string ECMSPool = "43c62e37-d45f-43bb-917b-777ace9e5482";
        public const string ECMSCircuitUsage = "d98db474-c670-4bfe-9379-d8be50193129";
        public const string ECMSStatus = "7a353b05-261f-48d2-972d-e94818f2b161";
        public const string ECMSSubStatus = "223953fb-d8f6-45f4-9656-9a0dc958e4ff";
        public const string Ban = "75751395-5318-45fa-ae03-e9dc15858676";
        public const string FirstSeenInCirp = "82bf92d5-b6b2-41bd-b82f-da720dc861d7";
        public const string LastSeenInCirp = "d9c952e6-0b13-4146-ae4f-a9fc094c1b96";
        public const string ExtractDate = "2eba7243-f7cf-47fa-9634-25dae58f8c60";

        //public const string OperDiscoDate = "900B2F8F-C259-42CB-AE2F-6483F6F01438";
        //public const string FinancialStopDate = "C31FE6FB-8F63-48B7-ADB5-E0D1BC20B6C5";
        //public const string BBBMatch = "9103527C-EB9F-4AB9-98EC-4497CD86F394";
        //public const string USID = "4BCC0C05-2106-4B28-861A-92BF621BF14C";
        //public const string URN = "7910081D-AB0F-41A0-9F2B-914AB311A5C1";
        //public const string OrderRef = "792F606A-3F7B-4453-A705-10C4ACC56671";
        //public const string NetworkBy = "FCE5A2B2-230E-4169-9B04-84B0108DC581";
        //public const string ECMSPool = "88517D82-11EF-4E40-8452-9EA1302FD133";
        //public const string ECMSCircuitUsage = "772F4604-D668-4FE5-BE48-4828F088C37B";
        //public const string ECMSStatus = "AA9669E4-34CE-4E00-82C7-5109CDC75BC9";
        //public const string ECMSSubStatus = "1E4CC13D-59F5-41D8-AE2D-8A74E42EB539";
        //public const string Ban = "D0948A9A-C00C-4996-9DA8-E2AAB6CA2C46";
        //public const string FirstSeenInCirp = "917794F7-B218-4E1B-8576-D7023A8E5BA7";
        //public const string LastSeenInCirp = "38F4D66B-89C9-4544-B9C5-7D8CD7C85D35";
        //public const string ExtractDate = "B786AFDF-66D2-45A2-9128-82A42AED2C29";
        public const string SynchronisationStatus = "68000388-42ce-4107-8f1d-6986b9169af4";
        public const string Alerts = "f6c9c4d9-aaa9-4d74-b52a-c5395fdace22";
    }
}
