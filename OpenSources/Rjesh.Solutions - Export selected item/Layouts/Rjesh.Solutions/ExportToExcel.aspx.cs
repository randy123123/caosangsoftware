using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Rjesh.Solutions
{
    public partial class ExportToExcel : LayoutsPageBase
    {
        HttpResponse exportResponse;
        protected void Page_Init(object sender, EventArgs e)
        {
            exportResponse = Response;
            // Get view and list guids from the request.
            Guid listGUID = new Guid(Request["ListGuid"].ToString());
            Guid viewGUID = new Guid(Request["ViewGuid"].ToString());

            // Get the list item ids in csv format.
            String[] listItemsID = Request["IDDict"].ToString().Split(new Char[] { ',' });

            ExportSelectedItemsToExcel(listGUID, viewGUID, listItemsID);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ExportSelectedItemsToExcel(Guid listID, Guid listViewID, String[] listIDsCSV)
        {
            try
            {
                SPList list = SPContext.Current.Web.Lists[listID];
                SPView listView = list.Views[listViewID];

                HtmlTable exportListTable = new HtmlTable();
                // Set the table's formatting-related properties.
                exportListTable.Border = 1;
                exportListTable.CellPadding = 3;
                exportListTable.CellSpacing = 3;

                // Start adding content to the table.
                HtmlTableRow htmlrow;
                HtmlTableCell htmlcell;

                // Add header row in HTML table
                htmlrow = new HtmlTableRow();
                SPViewFieldCollection viewHeaderFields = listView.ViewFields;
                for (int index = 0; index < viewHeaderFields.Count; index++)
                {
                    foreach (SPField field in listView.ParentList.Fields)
                    {
                        if (field.InternalName == viewHeaderFields[index])
                        {
                            if (!field.Hidden)
                            {
                                htmlcell = new HtmlTableCell();
                                htmlcell.BgColor = "#0099FF";
                                htmlcell.InnerHtml = field.Title.ToString();
                                htmlrow.Cells.Add(htmlcell);
                            }
                            break;
                        }
                    }
                }
                exportListTable.Rows.Add(htmlrow);

                // Add rows in HTML table based on the fields in view.
                foreach (String id in listIDsCSV)
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        htmlrow = new HtmlTableRow();

                        SPListItem item = list.GetItemById(Convert.ToInt32(id));
                        SPViewFieldCollection viewFields = listView.ViewFields;
                        for (int i = 0; i < viewFields.Count; i++)
                        {
                            foreach (SPField field in listView.ParentList.Fields)
                            {
                                if (field.InternalName == viewFields[i])
                                {
                                    if (!field.Hidden)
                                    {
                                        htmlcell = new HtmlTableCell();
                                        if (item[field.InternalName]!= null)
                                        {
                                            htmlcell.InnerHtml = item[field.InternalName].ToString();
                                        }
                                        else
                                        {
                                            htmlcell.InnerHtml = String.Empty;
                                        }
                                        htmlrow.Cells.Add(htmlcell);
                                    }
                                    break;
                                }
                            }
                        }
                        exportListTable.Rows.Add(htmlrow);
                    }
                }

                // Write the HTML table contents to response as excel file
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        exportListTable.RenderControl(htw);
                        exportResponse.Clear();
                        exportResponse.ContentType = "application/vnd.ms-excel";
                        exportResponse.AddHeader("content-disposition", string.Format("attachment; filename={0}", list.Title + ".xls"));
                        exportResponse.Cache.SetCacheability(HttpCacheability.NoCache);
                        exportResponse.Write(sw.ToString());
                        exportResponse.End();
                    }
                }
            }

            catch (Exception ex)
            {
                //exportResponse.Write("Some error occured during export :" + ex.Message.ToString());
                // Logging to be enabled here
            }

        }
    }
}
