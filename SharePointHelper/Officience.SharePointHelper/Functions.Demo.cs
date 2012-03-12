﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using System.IO;
using System.ComponentModel;

namespace Officience.SharePointHelper
{
    public partial class FormSharePointHelper
    {
        /*
        Note:   To add new function, only need copy code 'AddFunctions("Your functions").Click'
                and the press ' += ' and press TAB to past add your funtions, then type your 
                function name and press TAB to auto generate your function. After that you can
                write your code like my demo function.
        */
        public void Demo_DefineFunctions()
        {
            AddFunctions("Demo").Click += new EventHandler(Demo);
            AddFunctions("Demo Dialog Properties").Click += new EventHandler(DemoDiaglogProperties);
            AddFunctions("Demo progressBar").Click += new EventHandler(DemoProgressBar);
        }

        void Demo(object sender, EventArgs e)
        {
            //Your Code here...
            //[Demo]            
            try
            {
                //SPList listOrderForm = List("Order Form");
                WriteLine("Web.Title = '{0}'", Web.Title);
                WriteLine("Web.Url = '{0}'", Web.Url);
                foreach (SPList list in Web.Lists)
                {
                    WriteLine(" + {0}", list.Title);
                }
            }
            catch (Exception ex)
            {
                WriteLine("ERROR: {0}", ex.Message);
                WriteLine(ex.StackTrace);
            }
        }

        void DemoDiaglogProperties(object sender, EventArgs e)
        {
            OptionsDialog optionsDialog = new OptionsDialog();
            YourOptions yourOptions = new YourOptions();
            //Map your class with form OptionsDialog
            optionsDialog.Options.SelectedObject = yourOptions;

            //ShowDialog
            if (optionsDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(String.Format("yourOptions = ['{0}','{1}']", yourOptions.Value1, yourOptions.Value2));
            }
            else
            {
                MessageBox.Show("You click button Cancel in form OptionsDialog");
            }
        }

        void DemoProgressBar(object sender, EventArgs e)
        {
            int itemCount = 100;
            ProgressBarInit("Demo progressBar", itemCount - 1);
            for (int i = 0; i < itemCount; i++)
            {
                System.Threading.Thread.Sleep(300);
                ProgressBarNext();
            }
        }
    }

    public class YourOptions
    {
        [CategoryAttribute("Group1"), Description("Your description for property Value1")]
        public string Value1 { get; set; }

        [CategoryAttribute("Group2"), Description("Your description for property Value2; if value equal DefaultValue, that value not bold"), DefaultValue(false)]
        public bool Value2 { get; set; }
                
        public YourOptions()    //Init default values
        {
            Value1 = "Defaut value1";
            Value2 = false;
        }
    }
}
