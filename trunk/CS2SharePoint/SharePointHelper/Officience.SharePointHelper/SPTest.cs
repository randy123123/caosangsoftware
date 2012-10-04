using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using System.IO;
using System.ComponentModel;
using CSSoft;

namespace Officience.SharePointHelper
{
    public class SPTest : FormFunction//, IFormFunction
    {
        /*
        Note:   To add new function, only need copy code 'AddFunctions("Your functions").Click'
                and the press ' += ' and press TAB to past add your funtions, then type your 
                function name and press TAB to auto generate your function. After that you can
                write your code like my demo function.
        */

        public void DefineFunctions() //IFormFunctions
        {
            AddMenu("TEST").Click += new EventHandler(TEST);
        }
        void TEST(object sender, EventArgs e)
        {
            //SPListItem Item = List("Test").GetItemById(3);
            //CSSoft.CS2Regex.Substring("","","")
            //SPFieldMultiChoice spfChoice = (SPFieldMultiChoice)Item.Fields["MultiChoiceColumn"];
            OptionsDialog optionsDialog = new OptionsDialog();
            ReadFile yourOptions = new ReadFile();
            optionsDialog.Options.SelectedObject = yourOptions;
            if (optionsDialog.ShowDialog() == DialogResult.OK)
            {
                string fileText = File.ReadAllText(yourOptions.File);
                WriteLine(yourOptions.File);
                foreach (string staticName in CS2Regex.Substring(fileText, "StaticName=\"", "\""))
                {
                    WriteLine(staticName);
                }
            }
        }
    }
    public class ReadFile
    {
        [CategoryAttribute("FileName"), Description("Select your file")]
        public string File { get; set; }

        public ReadFile()    //Init default values
        {
            File = "";
        }
    }
}
