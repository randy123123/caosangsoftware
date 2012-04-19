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

namespace Officience.SharePointHelper
{
    public class YourClassStruct : FormFunction, IFormFunction
    {
        /*
        Note:   To add new function, only need copy code 'AddFunctions("Your functions").Click'
                and the press ' += ' and press TAB to past add your funtions, then type your 
                function name and press TAB to auto generate your function. After that you can
                write your code like my demo function.
        */
        public void DefineFunctions() //IFormFunctions
        {
            AddFunctions("Test").Click += new EventHandler(Test);
        }

        void Test(object sender, EventArgs e)
        {            
            WriteLine("test");
        }
    }
}
