using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;

namespace CodeArt.SharePoint.PermissionEx
{
    class PeopleEditorEx : PeopleEditor
    {
        protected override int DefaultRows
        {
            get
            {
                return 1;
                // return base.DefaultRows;
            }
        }

        protected override bool DefaultPlaceButtonsUnderEntityEditor
        {
            get
            {
                return false;
                //return base.DefaultPlaceButtonsUnderEntityEditor;
            }
        }
    }
}
