//----------------------------------------------------------------
//Code Art.
//
//文件描述:
//
//创 建 人: jianyi0115@163.com
//创建日期: 2008-1-14
//
//修订记录: 
//
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Reflection;

namespace CodeArt.SharePoint.PermissionEx
{
    public class EditControlListFieldIterator : Microsoft.SharePoint.WebControls.ListFieldIterator
    {
        protected override void CreateChildControls()
        {
            //if (this.ControlMode == SPControlMode.Display)
            //{
            //    base.CreateChildControls();
            //    return;
            //}

            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            if (currentUser != null && currentUser.IsSiteAdmin)
            {
                base.CreateChildControls();
                return;
            }

            ListFieldPermissionSetting listSetting = ListFieldPermissionSetting.GetListSetting(this.List);
            if (listSetting == null || listSetting.Count == 0)
            {
                base.CreateChildControls();
                return;
            }

            //base.CreateChildControls();
            this.Controls.Clear();
            if (this.ControlTemplate == null)
            {
                throw new ArgumentException("Could not find ListFieldIterator control template.");
            }

            Type t = typeof(TemplateContainer);

            PropertyInfo ControlModeProp = t.GetProperty("ControlMode", BindingFlags.Instance | BindingFlags.NonPublic);
            PropertyInfo FieldNameProp = t.GetProperty("FieldName", BindingFlags.Instance | BindingFlags.NonPublic);

            SPUser author = null;

            if (this.ControlMode == SPControlMode.New)
            {
                author = base.Web.CurrentUser;

                for (int i = 0; i < base.Fields.Count; i++)
                {
                    SPField field = base.Fields[i];

                    if (!this.IsFieldExcluded(field))
                    {
                        FieldPermission set = listSetting.GetByFieldName(field.InternalName);

                        if (set != null && !set.CanEdit(currentUser, author))
                        {
                            continue;
                        }

                        TemplateContainer child = new TemplateContainer();
                        this.Controls.Add(child);
                        FieldNameProp.SetValue(child, field.InternalName, null);
                        this.ControlTemplate.InstantiateIn(child);
                    }
                }

            }
            else
            {
                SPFieldUserValue authorFieldValue = new SPFieldUserValue(base.Web, "" + this.ListItem["Author"]);

                if (currentUser != null)
                {
                    author = authorFieldValue.User;
                }            

                for (int i = 0; i < base.Fields.Count; i++)
                {
                    SPField field = base.Fields[i];

                    if (!this.IsFieldExcluded(field))
                    {
                        SPControlMode thisMode = this.ControlMode;

                        FieldPermission set = listSetting.GetByFieldName(field.InternalName);

                        if (set != null )
                        {
                            if (!set.CanEdit(currentUser, author))
                            {
                                if (set.CanDisplay(currentUser, author))
                                    thisMode = SPControlMode.Display;
                                else
                                    continue;
                            }                                
                        }

                        TemplateContainer child = new TemplateContainer();
                        this.Controls.Add(child);
                        FieldNameProp.SetValue(child, field.InternalName, null);
                        ControlModeProp.SetValue(child, thisMode , null);
                        this.ControlTemplate.InstantiateIn(child);

                    }
                }

            }







        }


    }
}
