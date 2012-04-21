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
            AddFunctions("[your-class-struct] Test").Click += new EventHandler(Test);
            AddFunctions("[your-class-struct][1.0] Dynamic add menu").Click += new EventHandler(DynamicAddMenu);
        }

        void Test(object sender, EventArgs e)
        {            
            WriteLine("test");
        }

        void DynamicAddMenu(object sender, EventArgs e)
        {
            DisableFunctions(((ToolStripItem)sender).Text); //Disable click parent menu again
            AddFunctions("[your-class-struct][1.1] Menu 1").Click +=new EventHandler(MenuClick);
            AddFunctions("[your-class-struct][1.2] Menu 2").Click +=new EventHandler(MenuClick);
            AddFunctions("[your-class-struct][1.3] Menu 3").Click +=new EventHandler(MenuClick);
        }

        void MenuClick(object sender, EventArgs e)
        {
            WriteLine("Menu item '{0}' clicked.", ((ToolStripItem)sender).Text);
        }
    }
}
