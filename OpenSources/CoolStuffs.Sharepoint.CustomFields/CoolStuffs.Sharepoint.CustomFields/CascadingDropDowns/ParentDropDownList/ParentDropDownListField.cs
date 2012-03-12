using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.FieldControllers;

namespace CoolStuffs.Sharepoint.CustomFields.CascadingDropDowns.Field
{
    public class ParentDropDownListField : SPFieldText
  {
        #region Constructors

        public ParentDropDownListField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            this.init();

        }

        public ParentDropDownListField(Microsoft.SharePoint.SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            this.init();
        }



        
        #endregion

        private static Dictionary<int, Properties> staticPropertyList = new Dictionary<int, Properties>();

        private string parentSiteUrl;

        public string ParentSiteUrl
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ParentSiteUrl : parentSiteUrl;
            }
            set { parentSiteUrl = value; }
        }

        private string parentListName;

        public string ParentListName
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ParentListName : parentListName;
            }
            set { parentListName = value; }
        }

        private string parentListTextField;

        public string ParentListTextField
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ParentListTextField : parentListTextField;
            }
            set { parentListTextField = value; }
        }

        private string parentListValueField;

        public string ParentListValueField
        {
            get
            {
                return staticPropertyList.ContainsKey(ContextId) ? staticPropertyList[ContextId].ParentListValueField : parentListValueField;
            }
            set { parentListValueField = value; }
        }

        private void init()
        {
            this.parentSiteUrl = this.GetCustomProperty("ParentSiteUrl") + string.Empty;
            this.parentListName = this.GetCustomProperty("ParentListName") + string.Empty;
            this.parentListTextField = this.GetCustomProperty("ParentListTextField") + string.Empty;
            this.parentListValueField = this.GetCustomProperty("ParentListValueField") + string.Empty;
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
                case "ParentSiteUrl":
                    staticPropertyList[ContextId].ParentSiteUrl = value;
                    break;
                case "ParentListName":
                    staticPropertyList[ContextId].ParentListName = value;
                    break;
                case "ParentListTextField":
                    staticPropertyList[ContextId].ParentListTextField = value;
                    break;
                case "ParentListValueField":
                    staticPropertyList[ContextId].ParentListValueField = value;
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

            if (this.ParentSiteUrl != null)
            {
                this.SetCustomProperty("ParentSiteUrl", this.ParentSiteUrl);
            }

            if (this.ParentListName != null)
            {
                this.SetCustomProperty("ParentListName", this.ParentListName);
            }

            if (this.ParentListTextField != null)
            {
                this.SetCustomProperty("ParentListTextField", this.ParentListTextField);
            }

            if (this.ParentListValueField != null)
            {
                this.SetCustomProperty("ParentListValueField", this.ParentListValueField);
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
                Microsoft.SharePoint.WebControls.BaseFieldControl parentDropDownFieldControl = new ParentDropDownListFieldControl();
                parentDropDownFieldControl.FieldName = InternalName;
                return parentDropDownFieldControl;
            }
        }

        public class Properties
        {
            public string ParentSiteUrl;
            public string ParentListName;
            public string ParentListTextField;
            public string ParentListValueField;
        }
    }
}
