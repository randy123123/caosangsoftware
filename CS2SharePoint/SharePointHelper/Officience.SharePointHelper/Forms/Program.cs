using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Officience.SharePointHelper
{
    static class Program
    {
        private static FormSharePointHelper formSharePointHelper;
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formSharePointHelper = new FormSharePointHelper();
            Application.Run(formSharePointHelper);
        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            formSharePointHelper.WriteLine("ERROR: {0}", e.Exception.Message);
            formSharePointHelper.WriteLine(e.Exception.StackTrace);
            formSharePointHelper.EndFuntion();
        }
    }
}
