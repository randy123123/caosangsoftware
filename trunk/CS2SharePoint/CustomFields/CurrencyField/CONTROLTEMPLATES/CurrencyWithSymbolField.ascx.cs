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

namespace Officience.IBNF.CurrencyField
{
    public partial class CurrencyWithSymbolField : SPFieldText
    {
        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                return base.FieldRenderingControl;
            }
        }

        #region constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="fieldName"></param>
        public CurrencyWithSymbolField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="typeName"></param>
        /// <param name="displayName"></param>
        public CurrencyWithSymbolField(SPFieldCollection fields, string typeName, string displayName)
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
            string _siteName = GetFieldThreadDataValue("SiteName", true);
            string _currencyListName = GetFieldThreadDataValue("CurrencyListName", true);
            string _currencySymbolColumn = GetFieldThreadDataValue("CurrencySymbolColumn", true);
            base.SetCustomProperty("SiteName", _siteName);
            base.SetCustomProperty("CurrencyListName", _currencyListName);
            base.SetCustomProperty("CurrencySymbolColumn", _currencySymbolColumn);
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
            Thread.FreeNamedDataSlot("SiteName");
            Thread.FreeNamedDataSlot("CurrencyListName");
            Thread.FreeNamedDataSlot("CurrencySymbolColumn");
        }
        #endregion

        #region SiteName property
        private string _siteName;
        public string SiteName
        {
            get
            {
                if (_siteName == null) _siteName = GetFieldThreadDataValue("SiteName", false);
                return (!string.IsNullOrEmpty(_siteName)) ? _siteName : null;
            }
            set
            {
                SetFieldThreadDataValue("SiteName", (!string.IsNullOrEmpty(value) ? value : ""));
            }
        }
        #endregion

        #region CurrencyListName property
        private string _currencyListName;
        public string CurrencyListName
        {
            get
            {
                if (_currencyListName == null) _currencyListName = GetFieldThreadDataValue("CurrencyListName", false);
                return (!string.IsNullOrEmpty(_currencyListName)) ? _currencyListName : null;
            }
            set
            {
                SetFieldThreadDataValue("CurrencyListName", (!string.IsNullOrEmpty(value) ? value : ""));
            }
        }
        #endregion

        #region CurrencySymbolColumn property
        private string _currencySymbolColumn;
        public string CurrencySymbolColumn
        {
            get
            {
                if (_currencySymbolColumn == null) _currencySymbolColumn = GetFieldThreadDataValue("CurrencySymbolColumn", false);
                return (!string.IsNullOrEmpty(_currencySymbolColumn)) ? _currencySymbolColumn : null;
            }
            set
            {
                SetFieldThreadDataValue("CurrencySymbolColumn", (!string.IsNullOrEmpty(value) ? value : ""));
            }
        }
        #endregion
    }
}
