using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Diagnostics;


namespace RibbonManager.RibbonManager
{
    [ToolboxItemAttribute(false)]
    public class RibbonManager : WebPart
    { 
        //******* Pending tasks *******
        //private const string MENU_CONTROL_UPLOAD = "";
        //private const string MENU_ID_UPLOADDOCUMENT = "";
        //private const string MENU_ID_UPLOADMULTIPLE = "";  
        //private const string MENU_ID_EXPLORER = "";
        //private const string MENU_ID_OUTLOOK = "";     
        //private const string MENU_ID_DATABASE = "";

        #region WebPart properties

        private const string MENU_CONTROL_LIST = "Ribbon.ListItem";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'List' Tab"), Category("Element")]
        public bool ShowListMenu
        {
            get { return ShowList; }
            set { ShowList = value; }
        }

        private const string MENU_CONTROL_ACTIONS = "Ribbon.ListItem.Actions";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Actions' Menu"), Category("Actions")]
        public bool ShowActionsMenu
        {
            get { return ShowActions; }
            set { ShowActions = value; }
        }
        private const string MENU_CONTROL_SETTINGS = "Ribbon.List.Settings";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Settings' Menu"), Category("Settings")]
        public bool ShowSettingsMenu
        {
            get { return this.ShowItemSettings; }
            set { this.ShowItemSettings = value; }
        }

        private const string MENU_ID_NEW = "Ribbon.ListItem.New.NewListItem";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'New' Menu"), Category("Element")]
        public bool ShowNewMenu
        {
            get { return ShowNew; }
            set { ShowNew = value; }
        }

        private const string MENU_ID_NEWFOLDER = "Ribbon.ListItem.New.NewFolder";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'New Folder' Menu"), Category("Element")]
        public bool ShowNewFolderMenu
        {
            get { return Showfolder; }
            set { Showfolder = value; }
        }

        private const string MENU_ID_EDITITEM = "Ribbon.ListItem.Manage.EditProperties";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Edit' Menu"), Category("Element")]
        public bool ShowEditMenu
        {
            get { return this.ShowEdititem; }
            set { this.ShowEdititem = value; }
        }

        private const string MENU_ID_VERSIONS = "Ribbon.ListItem.Manage.ViewVersions";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Versions' Menu"), Category("Element")]
        public bool ShowVersionsMenu
        {
            get { return this.ShowVersions; }
            set { this.ShowVersions = value; }
        }

        private const string MENU_ID_ITEMPERMISSIONS = "Ribbon.ListItem.Manage.ManagePermissions";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Permissions' Menu"), Category("Element")]
        public bool ShowPermissionsMenu
        {
            get { return this.ShowItemPermissions; }
            set { this.ShowItemPermissions = value; }
        }

        private const string MENU_ID_DELETE = "Ribbon.ListItem.Manage.Delete";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Delete' Menu"), Category("Element")]
        public bool ShowDeleteMenu
        {
            get { return this.ShowDelete; }
            set { this.ShowDelete = value; }
        }

        private const string MENU_ID_ATTACHFILE = "Ribbon.ListItem.Actions.AttachFile";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Attach' Menu"), Category("Element")]
        public bool ShowAttachMenu
        {
            get { return this.ShowAttachfile; }
            set { this.ShowAttachfile = value; }
        }

        private const string MENU_ID_ALERT = "Ribbon.ListItem.Share.AlertMe.Menu.ManageAlerts.ManageAlerts";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Alerts' Menu"), Category("Element")]
        public bool ShowAlertMenu
        {
            get { return this.ShowAlert; }
            set { this.ShowAlert = value; }
        }

        private const string MENU_ID_WORKFLOW = "Ribbon.ListItem.Workflow.ViewWorkflows";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Workflow' Menu"), Category("Element")]
        public bool ShowWorkflowMenu
        {
            get { return this.ShowWorkflow; }
            set { this.ShowWorkflow = value; }
        }

        private const string MENU_ID_DEFAULTVIEW = "Ribbon.List.ViewFormat.Standard";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Default View' Menu"), Category("List")]
        public bool ShowDefaultViewMenu
        {
            get { return this.ShowDefaultview; }
            set { this.ShowDefaultview = value; }
        }

        private const string MENU_ID_DATASHEET = "Ribbon.List.ViewFormat.Datasheet";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'DataSheet' Menu"), Category("List")]
        public bool ShowDataSheetMenu
        {
            get { return this.ShowDatasheet; }
            set { this.ShowDatasheet = value; }
        }

        private const string MENU_ID_NEWROW = "Ribbon.List.Datasheet.NewRow";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'New Row' Menu"), Category("List")]
        public bool ShowNewRowMenu
        {
            get { return this.ShowNewrow; }
            set { this.ShowNewrow = value; }
        }

        private const string MENU_ID_CREATEVIEW = "Ribbon.List.CustomViews.CreateView";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Create View' Menu"), Category("List")]
        public bool ShowCreateViewMenu
        {
            get { return this.ShowCreateview; }
            set { this.ShowCreateview = value; }
        }

        private const string MENU_ID_VIEWPROPERTIES = "Ribbon.ListItem.Manage.ViewProperties";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'View Properties' Menu"), Category("List")]
        public bool ShowViewPropertiesMenu
        {
            get { return this.ShowViewproperties; }
            set { this.ShowViewproperties = value; }
        }

        private const string MENU_ID_MODIFYVIEW = "Ribbon.List.CustomViews.ModifyView";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Modify View' Menu"), Category("List")]
        public bool ShowModifyViewMenu
        {
            get { return this.ShowModifyview; }
            set { this.ShowModifyview = value; }
        }


        private const string MENU_ID_SELECTVIEW = "Ribbon.List.CustomViews.DisplayView";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Select View' Menu"), Category("List")]
        public bool ShowSelectViewMenu
        {
            get { return this.ShowSelectView; }
            set { this.ShowSelectView = value; }
        }


        private const string MENU_ID_ADDCOLUMN = "Ribbon.List.CustomViews.CreateColumn";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Add Column' Menu"), Category("List")]
        public bool ShowAddColumnMenu
        {
            get { return this.ShowAddcolumn; }
            set { this.ShowAddcolumn = value; }
        }

        private const string MENU_ID_NAVIGATEUP = "Ribbon.List.CustomViews.NavigateUp";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Navigate Up' Menu"), Category("List")]
        public bool ShowNavigateUpMenu
        {
            get { return this.ShowNavigateup; }
            set { this.ShowNavigateup = value; }
        }

        private const string MENU_ID_CURRENTVIEW = "Ribbon.List.CustomViews.CurrentView";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Current View' Menu"), Category("List")]
        public bool ShowCurrenteViewMenu
        {
            get { return this.ShowCurrentview; }
            set { this.ShowCurrentview = value; }
        }

        private const string MENU_ID_EMAILLIBRARYLINK = "Ribbon.List.Share.EmailLibraryLink";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Send email library link' Menu"), Category("List")]
        public bool ShowEmailLibraryLinkMenu
        {
            get { return this.ShowEmaillibrarylink; }
            set { this.ShowEmaillibrarylink = value; }
        }

        private const string MENU_ID_ALERTME = "Ribbon.List.Share.AlertMe";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Alert Me' Menu"), Category("List")]
        public bool ShowAlertMeMenu
        {
            get { return this.ShowAlertme; }
            set { this.ShowAlertme = value; }
        }

        private const string MENU_ID_RSS = "Ribbon.List.Share.ViewRSSFeed";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Source RSS' Menu"), Category("List")]
        public bool ShowRssMenu
        {
            get { return this.ShowRss; }
            set { this.ShowRss = value; }
        }

        private const string MENU_ID_EXPORTSPREADSHEET = "Ribbon.List.Actions.ExportToSpreadsheet";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Export To Spreadsheet' Menu"), Category("List")]
        public bool ShowExportExcelMenu
        {
            get { return this.ShowExportToSpreadsheet; }
            set { this.ShowExportToSpreadsheet = value; }
        }

        private const string MENU_ID_OUTLOOK = "Ribbon.List.Actions.ConnectToClient";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Connect to Outlook' Menu"), Category("List")]
        public bool ShowOutlookMenu
        {
            get { return this.ShowOutlook; }
            set { this.ShowOutlook = value; }
        }

        private const string MENU_ID_WORKSPACE = "Ribbon.List.Actions.TakeOfflineToClient";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Export to WorkSpace' Menu"), Category("List")]
        public bool ShowWorkSpaceMenu
        {
            get { return this.ShowWorkspace; }
            set { this.ShowWorkspace = value; }
        }

        private const string MENU_ID_CUSTOMIZE = "Ribbon.List.CustomizeList";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Customize List' Menu"), Category("List")]
        public bool ShowCustomizeMenu
        {
            get { return this.ShowCustomize; }
            set { this.ShowCustomize = value; }
        }

        private const string MENU_ID_SETTINGS = "Ribbon.List.Settings.ListSettings";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Settings' Menu"), Category("List")]
        public bool ShowListSettingsMenu
        {
            get { return this.ShowListSettings; }
            set { this.ShowListSettings = value; }
        }

        private const string MENU_ID_PERMISSIONS = "Ribbon.List.Settings.ListPermissions";


        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Permissions' Menu"), Category("List")]
        public bool ShowListPermissionsMenu
        {
            get { return this.ShowListPermissions; }
            set { this.ShowListPermissions = value; }
        }

        private const string MENU_ID_WORKFLOWS = "Ribbon.List.Settings.ManageWorkflows";



        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
        WebDisplayName("Show 'Workflows' Menu"), Category("List")]
        public bool ShowWorkFlowsMenu
        {
            get { return this.ShowWorkflows; }
            set { this.ShowWorkflows = value; }
        }
        #endregion
                
        #region Private properties

        private bool ShowList = true;
        private bool ShowActions = true;
        private bool ShowItemSettings = true;
        private bool ShowNew = true;
        private bool Showfolder = true;
        private bool ShowEdititem = true;
        private bool ShowVersions = true;
        private bool ShowItemPermissions = true;
        private bool ShowDelete = true;
        private bool ShowAttachfile = true;
        private bool ShowAlert = true;
        private bool ShowWorkflow = true;
        private bool ShowDefaultview = true;
        private bool ShowDatasheet = true;
        private bool ShowNewrow = true;
        private bool ShowCreateview = true;
        private bool ShowViewproperties = true;
        private bool ShowModifyview = true;
        private bool ShowAddcolumn = true;
        private bool ShowNavigateup = true;
        private bool ShowCurrentview = true;
        private bool ShowEmaillibrarylink = true;
        private bool ShowAlertme = true;
        private bool ShowRss = true;
        private bool ShowExportToSpreadsheet = true;
        private bool ShowOutlook = true;
        private bool ShowWorkspace = true;
        private bool ShowCustomize = true;
        private bool ShowListSettings = true;
        private bool ShowListPermissions = true;
        private bool ShowWorkflows = true;
        private bool ShowSelectView = true;

        #endregion
                
        /// <summary>
        /// Pre render webpart HTML event handler
        /// </summary>
        /// <param name="e">Internal parameters</param>
        protected override void OnPreRender(EventArgs e)
        {
            ExamineControls();
        }
        
        /// <summary>
        /// Method that render and disable buttons and features of a ribbon into the page where the webpart is put
        /// </summary>
        private void ExamineControls()
        {
            SPRibbon ribbon = SPRibbon.GetCurrent(this.Page);


            if (ribbon != null)
            {

                if (!this.ShowList)
                {
                    try
                    {
                        ribbon.TrimById(MENU_CONTROL_LIST);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowActions)
                {
                    try
                    {
                        ribbon.TrimById(MENU_CONTROL_ACTIONS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowItemSettings)
                {
                    try
                    {
                        ribbon.TrimById(MENU_CONTROL_SETTINGS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowNew)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_NEW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }

                if (!this.Showfolder)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_NEWFOLDER);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowEdititem)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_EDITITEM);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowVersions)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_VERSIONS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowItemPermissions)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_ITEMPERMISSIONS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowDelete)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_DELETE);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowAttachfile)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_ATTACHFILE);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowAlert)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_ALERT);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowWorkflow)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_WORKFLOW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }



                if (!this.ShowWorkflow)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_WORKFLOW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowDatasheet)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_DATASHEET);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowNewrow)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_NEWROW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowCreateview)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_CREATEVIEW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowViewproperties)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_VIEWPROPERTIES);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowModifyview)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_MODIFYVIEW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowAddcolumn)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_ADDCOLUMN);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowNavigateup)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_NAVIGATEUP);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }

                if (!this.ShowCurrenteViewMenu)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_CURRENTVIEW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }

                if (!this.ShowEmaillibrarylink)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_EMAILLIBRARYLINK);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowAlertme)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_ALERTME);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowRss)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_RSS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowExportToSpreadsheet)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_EXPORTSPREADSHEET);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowOutlook)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_OUTLOOK);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowWorkspace)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_WORKSPACE);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowCustomize)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_CUSTOMIZE);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }

                if (!this.ShowListSettings)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_SETTINGS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowListPermissions)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_PERMISSIONS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowWorkflows)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_SETTINGS);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }


                if (!this.ShowSelectView)
                {
                    try
                    {
                        ribbon.TrimById(MENU_ID_SELECTVIEW);
                    }
                    catch (Exception ex)
                    {
                        PortalLog.LogString(ex.StackTrace);
                    }
                }
                


            }

        }
        
    }
}
