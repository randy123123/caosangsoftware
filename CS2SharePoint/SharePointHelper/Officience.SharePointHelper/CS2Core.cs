using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using Microsoft.SharePoint.Navigation;
using CSSoft;

namespace Officience.SharePointHelper
{
    public class CS2Core : FormFunction, IFormFunction
    {
        /*
        Note:   To add new function, only need copy code 'AddFunctions("Your functions").Click'
                and the press ' += ' and press TAB to past add your funtions, then type your 
                function name and press TAB to auto generate your function. After that you can
                write your code like my demo function.
        */
        public void DefineFunctions() //IFormFunctions
        {
            AddFunctions("[CS2Core] Read CS2SPUsers.dat").Click += new EventHandler(ReadCS2SPUsers);
        }

        void ReadCS2SPUsers(object sender, EventArgs e)
        {
            OptionsDialog optionsDialog = new OptionsDialog();
            CS2SPUsersData yourOptions = new CS2SPUsersData();
            //Map your class with form OptionsDialog
            optionsDialog.Options.SelectedObject = yourOptions;

            //ShowDialog
            if (optionsDialog.ShowDialog() == DialogResult.OK)
            {
                string[] lines = System.IO.File.ReadAllLines(yourOptions.FileName);
                foreach (string line in lines)
                {
                    WriteLine(line);
                    WriteLine(CS2Secret.DecryptString(line));
                }
            }

        }
    }
    public class CS2SPUsersData
    {
        [CategoryAttribute("File Name"), Description("Seleted file name")]
        [System.ComponentModel.Editor(typeof(System.Windows.Forms.Design.FileNameEditor),typeof(System.Drawing.Design.UITypeEditor))]
        public string FileName { get; set; }

        public CS2SPUsersData()
        {
            FileName = @"C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\CSSoft\Log\CS2SPUsers.dat";
        }
    }
}
