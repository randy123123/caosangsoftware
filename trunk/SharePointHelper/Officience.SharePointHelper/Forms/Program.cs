using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Officience.SharePointHelper
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormSharePointHelper());
        }
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(String.Format("ERROR !!!\n\n [Message]\n{0} \n\n [StackTrace]\n{1}", e.Exception.Message, e.Exception.StackTrace));
        }
    }
}
