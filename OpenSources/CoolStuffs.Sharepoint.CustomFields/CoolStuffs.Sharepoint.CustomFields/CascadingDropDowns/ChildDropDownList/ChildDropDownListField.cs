using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.FieldControllers;
using Microsoft.SharePoint.Search;
using Microsoft.SharePoint.Search.Query;

namespace CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.Field
{
    public class ChildDropDownListField:SPFieldText
    {
        #region Constructors

        public ChildDropDownListField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            this.init();

        }

        public ChildDropDownListField(Microsoft.SharePoint.SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            this.init();
        }



        
        #endregion

        private static Dictionary<int, Properties> staticPropertyList = new Dictionary<int, Properties>();

        private string childSiteUrl;

        public string ChildSiteUrl
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ChildSiteUrl : childSiteUrl;
            }
            set { childSiteUrl = value; }
        }

        private string childListName;

        public string ChildListName
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ChildListName : childListName;
            }
            set { childListName = value; }
        }

        private string childListTextField;

        public string ChildListTextField
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ChildListTextField : childListTextField;
            }
            set { childListTextField = value; }
        }

        private string childListValueField;

        public string ChildListValueField
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ChildListValueField : childListValueField;
            }
            set { childListValueField = value; }
        }

        private string childJoinField;

        public string ChildJoinField
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ChildJoinField : childJoinField;
            }
            set { childJoinField = value; }
        }


      
        private void init()
        {
            this.childSiteUrl = this.GetCustomProperty("ChildSiteUrl") + string.Empty;
            this.childListName = this.GetCustomProperty("ChildListName") + string.Empty;
            this.childListTextField = this.GetCustomProperty("ChildListTextField") + string.Empty;
            this.childListValueField = this.GetCustomProperty("ChildListValueField") + string.Empty;
            this.childJoinField = this.GetCustomProperty("ChildJoinField") + string.Empty;
        }
	
        /// <summary>

        /// Here we can apply formating to our number that will show up on the edit page.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override object GetFieldValue(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            return value;
        }

        public override void OnAdded(SPAddFieldOptions op)
        {
            base.OnAdded(op);
            Update();
        }

        public void UpdateMyCustomProperty(string propertyName, string value)
        {
            if (!staticPropertyList.ContainsKey(ContextId))
                staticPropertyList.Add(ContextId, new Properties());
    
            switch (propertyName)
            {
                case "ChildSiteUrl":
                    staticPropertyList[ContextId].ChildSiteUrl = value;
                    break;
                case "ChildListName":
                    staticPropertyList[ContextId].ChildListName = value;
                    break;
                case "ChildListTextField":
                    staticPropertyList[ContextId].ChildListTextField = value;
                    break;
                case "ChildListValueField":
                    staticPropertyList[ContextId].ChildListValueField = value;
                    break;
                case "ChildJoinField":
                    staticPropertyList[ContextId].ChildJoinField = value;
                    break;
            }
            
        }

        public int ContextId
        {
            get
            {
                return SPContext.Current.GetHashCode();
            }
        }


        public override void Update()
        {
            //this.SetCustomProperty("ChildSiteUrl", this.ChildSiteUrl);
            //this.SetCustomProperty("ChildListName", this.ChildListName);
            //this.SetCustomProperty("ChildListTextField", this.childListTextField);
            //this.SetCustomProperty("ChildListValueField", this.ChildListValueField);
            //this.SetCustomProperty("ChildJoinField", this.ChildJoinField);
            //base.Update();

            if (this.ChildSiteUrl != null)
            {
                this.SetCustomProperty("ChildSiteUrl", this.ChildSiteUrl);
            }

            if (this.ChildListName != null)
            {
                this.SetCustomProperty("ChildListName", this.ChildListName);
            }

            if (this.ChildListTextField != null)
            {
                this.SetCustomProperty("ChildListTextField", this.ChildListTextField);
            }

            if (this.ChildListValueField != null)
            {
                this.SetCustomProperty("ChildListValueField", this.ChildListValueField);
            }

            if (this.ChildJoinField != null)
            {
                this.SetCustomProperty("ChildJoinField", this.ChildJoinField);
            }

            base.Update();
            //once the field has been updated we can remove it from the static (temporary) placeholder
            if (staticPropertyList.ContainsKey(ContextId))
                staticPropertyList.Remove(ContextId);
        }


        public override Microsoft.SharePoint.WebControls.BaseFieldControl FieldRenderingControl
        {
            get
            {
                Microsoft.SharePoint.WebControls.BaseFieldControl childDropDownFieldControl = new ChildDropDownListFieldControl();
                childDropDownFieldControl.FieldName = InternalName;
                return childDropDownFieldControl;
            }
        }

        public class Properties
        {
            public string ChildSiteUrl;
            public string ChildListName;
            public string ChildListTextField;
            public string ChildListValueField;
            public string ChildJoinField;

        }
    }
}
