using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using System.Globalization;

namespace CSSoft.CS2SPCustomFields.AutoField
{
    public partial class AutoWithFormatField : SPFieldText
    {
        public const string DefaultFormat = "[Date(yyyyMMdd)]-[ItemCountInDate(000)]";
        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                Microsoft.SharePoint.WebControls.BaseFieldControl fieldControl = new AutoWithFormatFieldControl();
                fieldControl.FieldName = InternalName;
                return fieldControl;
                //return base.FieldRenderingControl;
            }
        }

        #region constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="fieldName"></param>
        public AutoWithFormatField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="typeName"></param>
        /// <param name="displayName"></param>
        public AutoWithFormatField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }
        #endregion

        #region OnAdded method
        /// <summary>
        /// Fires when a new filtered lookup field is added
        /// </summary>
        /// <param name="op"></param>
        public override void OnAdded(SPAddFieldOptions op)
        {
            base.OnAdded(op);
            Update();
        }
        #endregion

        #region Update method
        /// <summary>
        /// Updates the properties of the filtered lookup field
        /// </summary>
        public override void Update()
        {

            UpdateFieldProperties();
            base.Update();
            CleanUpThreadData();
        }
        #endregion

        #region UpdateFieldProperties method
        /// <summary>
        /// Updates custom properties of the filtered lookup field
        /// </summary>
        private void UpdateFieldProperties()
        {
            string _fieldFormat = GetFieldThreadDataValue("FieldFormat", true);
            base.SetCustomProperty("FieldFormat", _fieldFormat);
        }
        #endregion

        #region GetFieldThreadDataValue method
        private string GetFieldThreadDataValue(string propertyName, bool ignoreEmptyValue)
        {
            string _d = (string)Thread.GetData(Thread.GetNamedDataSlot(propertyName));
            if (string.IsNullOrEmpty(_d) && !ignoreEmptyValue)
            {
                _d = (string)base.GetCustomProperty(propertyName);
            }
            return _d;
        }

        private void SetFieldThreadDataValue(string propertyName, string value)
        {
            Thread.SetData(Thread.GetNamedDataSlot(propertyName), value);
        }
        #endregion

        #region CleanUpThreadData method
        private void CleanUpThreadData()
        {
            Thread.FreeNamedDataSlot("FieldFormat");
        }
        #endregion

        #region FieldFormat property
        private string _fieldFormat;
        public string FieldFormat
        {
            get
            {
                if (_fieldFormat == null) _fieldFormat = GetFieldThreadDataValue("FieldFormat", false);
                return (!string.IsNullOrEmpty(_fieldFormat)) ? _fieldFormat : DefaultFormat;
            }
            set
            {
                SetFieldThreadDataValue("FieldFormat", (!string.IsNullOrEmpty(value) ? value : DefaultFormat));
            }
        }
        #endregion
    }
}