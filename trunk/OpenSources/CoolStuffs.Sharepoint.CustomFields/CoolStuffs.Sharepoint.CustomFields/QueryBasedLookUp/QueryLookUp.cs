using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using CoolStuffs.Sharepoint.CustomFields.QueryBasedLookUp.FieldControllers;

namespace CoolStuffs.Sharepoint.CustomFields.QueryBasedLookUp.Field
{
    public class QueryLookUp : SPFieldText
    {
        #region Constructors
        /// <summary>
        /// Overriden Constructor of SPFieldText
        /// </summary>
        /// <param name="fields"> collection of SPField</param>
        /// <param name="fieldName">Name of your field out of the collection</param>
         public QueryLookUp(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            this.init();
        }

        /// <summary>
        /// Overriden Constructor of SPFieldText
        /// </summary>
        /// <param name="fields">collection of SPField</param>
        /// <param name="typeName">Class Name as Field Controller</param>
        /// <param name="displayName"> display name of your field</param>
        public QueryLookUp(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            this.init();
        }

        #endregion

        #region private variables
        private string siteUrl;
        private string lookUpListName;
        private string lookUpDisplayColumnText;
        private string lookUpDisplayColumnValue;
        private string actualQuery;
        private string sqlQuery;
        private string badQueryFlag;
        private static Dictionary<int, Properties> staticPropertyList = new Dictionary<int, Properties>();
        #endregion

        #region Get/Set Custom Properties

        /// <summary>
        /// Get/Set Site URL
        /// </summary>
        public string SiteUrl
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].siteUrl : this.siteUrl;
            }
            set
            {
                this.siteUrl = value;
            }
        }

        /// <summary>
        /// Get/Set Look Up List Name
        /// </summary>
        public string LookUpListName
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].lookUpListName : this.lookUpListName;
            }
            set
            {
                this.lookUpListName = value;
            }
        }

        /// <summary>
        /// Get/Set the display Column text
        /// </summary>
        public string LookUpDisplayColumnText
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].lookUpDisplayColumnText : this.lookUpDisplayColumnText;
            }
            set
            {
                this.lookUpDisplayColumnText = value;
            }
        }

        /// <summary>
        /// Get/Set the Display Column Value
        /// </summary>
        public string LookUpDisplayColumnValue
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].lookUpDisplayColumnValue : this.lookUpDisplayColumnValue;
            }
            set
            {
                this.lookUpDisplayColumnValue = value;
            }
        }

        /// <summary>
        /// Get/Set CAML query
        /// </summary>
        public string ActualQuery
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].actualQuery : this.actualQuery;
            }
            set
            {
                this.actualQuery = value;
            }
        }

        /// <summary>
        /// Get/Set sql query
        /// </summary>
        public string SQLQuery
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].sqlQuery : this.sqlQuery;
            }
            set
            {
                this.sqlQuery = value;
            }
        }

        /// <summary>
        /// Get/Set BadQueryFlag
        /// </summary>
        public string BadQueryFlag
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].badQueryFlag : this.badQueryFlag;
            }
            set
            {
                this.badQueryFlag = value;
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// SPFieldText overridden method,
        /// Gets the current set value of the field
        /// </summary>
        /// <param name="value">field value</param>
        /// <returns>field value in object</returns>
        public override object GetFieldValue(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            return value;
        }

        /// <summary>
        /// SPFieldtext overridden method,
        /// Called after OnSaveChange event of the FieldController 
        /// from edit column page
        /// </summary>
        /// <param name="op">SPFieldOptions</param>
        public override void OnAdded(SPAddFieldOptions op)
        {
            base.OnAdded(op);
            Update();
        }

        /// <summary>
        /// SPFieldText overridden Method,
        /// updates the column custom properties
        /// </summary>
        public override void Update()
        {
            if (!string.IsNullOrEmpty(this.SiteUrl))
            {
                this.SetCustomProperty("SiteUrl", this.SiteUrl);
            }

            if (!string.IsNullOrEmpty(this.LookUpListName))
            {
                this.SetCustomProperty("LookUpListName", this.LookUpListName);
            }

            if (!string.IsNullOrEmpty(this.LookUpDisplayColumnText))
            {
                this.SetCustomProperty("LookUpDisplayColumnText", this.LookUpDisplayColumnText);
            }

            if (!string.IsNullOrEmpty(this.LookUpDisplayColumnValue))
            {
                this.SetCustomProperty("LookUpDisplayColumnValue", this.LookUpDisplayColumnValue);
            }

            if (!string.IsNullOrEmpty(this.ActualQuery))
            {
                this.SetCustomProperty("ActualQuery", this.ActualQuery);
            }

            if(!string.IsNullOrEmpty(this.SQLQuery))
            {
                this.SetCustomProperty("SQLQuery", this.SQLQuery);
            }
            if (!string.IsNullOrEmpty(this.BadQueryFlag))
            {
                this.SetCustomProperty("BadQueryFlag", this.BadQueryFlag);
            }

            base.Update();
            if (staticPropertyList.ContainsKey(ContextId))
                staticPropertyList.Remove(ContextId);
        }

        /// <summary>
        /// Overriden BaseFieldControl method,
        /// returns the Rendering Control of the current column
        /// </summary>
        public override Microsoft.SharePoint.WebControls.BaseFieldControl FieldRenderingControl
        {
            get
            {
                Microsoft.SharePoint.WebControls.BaseFieldControl queryLookUpFieldControl = new QueryLookUpFieldControl();
                queryLookUpFieldControl.FieldName = InternalName;
                return queryLookUpFieldControl;
            }
        }

        /// <summary>
        /// Overriden initialization method
        /// </summary>
        private void init()
        {
            this.siteUrl = this.GetCustomProperty("SiteUrl") + string.Empty;
            this.lookUpListName = this.GetCustomProperty("LookUpListName") + string.Empty;
            this.lookUpDisplayColumnText = this.GetCustomProperty("LookUpDisplayColumnText") + string.Empty;
            this.lookUpDisplayColumnValue = this.GetCustomProperty("LookUpDisplayColumnValue") + string.Empty;
            this.actualQuery = this.GetCustomProperty("ActualQuery") + string.Empty;
            this.sqlQuery = this.GetCustomProperty("SQLQuery") + string.Empty;
            this.badQueryFlag = this.GetCustomProperty("BadQueryFlag") + string.Empty;
        }

        #endregion

        #region Custom Methods and classes

        /// <summary>
        /// sets the Dictionary with entered value in the Edit page
        /// </summary>
        /// <param name="propertyName">Name of the Property being saved</param>
        /// <param name="value">value of that property</param>
        public void UpdateMyCustomProperty(string propertyName, string value)
        {
            if (!staticPropertyList.ContainsKey(ContextId))
                staticPropertyList.Add(ContextId, new Properties());

            switch (propertyName)
            {
                case "SiteUrl":
                    staticPropertyList[ContextId].siteUrl = value;
                    break;
                case "LookUpListName":
                    staticPropertyList[ContextId].lookUpListName = value;
                    break;
                case "LookUpDisplayColumnText":
                    staticPropertyList[ContextId].lookUpDisplayColumnText = value;
                    break;

                case "LookUpDisplayColumnValue":
                    staticPropertyList[ContextId].lookUpDisplayColumnValue = value;
                    break;

                case "ActualQuery":
                    staticPropertyList[ContextId].actualQuery = value;
                    break;

                case "SQLQuery":
                    staticPropertyList[ContextId].sqlQuery = value;
                    break;

                case "BadQueryFlag":
                    staticPropertyList[ContextId].badQueryFlag = value;
                    break;
            }
        }

        /// <summary>
        /// Class to hold the static meta properties
        /// </summary>
        public class Properties
        {
            public string siteUrl;
            public string lookUpListName;
            public string lookUpDisplayColumnText;
            public string lookUpDisplayColumnValue;
            public string actualQuery;
            public string sqlQuery;
            public string badQueryFlag;
        }

        /// <summary>
        /// get context Id to be used to set a dictionary entry
        /// </summary>
        public int ContextId
        {
            get
            {
                return SPContext.Current.GetHashCode();
            }
        }

        #endregion
    }
}
