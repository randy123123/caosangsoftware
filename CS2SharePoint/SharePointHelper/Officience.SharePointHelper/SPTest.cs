using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;
using System.IO;
using System.ComponentModel;

namespace Officience.SharePointHelper
{
    public class SPTest : FormFunction, IFormFunction
    {
        /*
        Note:   To add new function, only need copy code 'AddFunctions("Your functions").Click'
                and the press ' += ' and press TAB to past add your funtions, then type your 
                function name and press TAB to auto generate your function. After that you can
                write your code like my demo function.
        */

        public void DefineFunctions() //IFormFunctions
        {
            AddMenu("Get SPFieldMultiChoice").Click += new EventHandler(GetSPFieldMultiChoice);
        }
        void GetSPFieldMultiChoice(object sender, EventArgs e)
        {
            SPListItem Item = List("Test").GetItemById(3);
            //SPFieldMultiChoice spfChoice = (SPFieldMultiChoice)Item.Fields["MultiChoiceColumn"];
        }
    }
}
