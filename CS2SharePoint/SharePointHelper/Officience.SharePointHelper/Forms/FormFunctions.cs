using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;

namespace Officience.SharePointHelper
{
    public interface IFormFunction
    {
        void SetForm(FormSharePointHelper form);
        void DefineFunctions();
    }
    public class FormFunction
    {
        public FormSharePointHelper Form { get; set; }
        public FormFunction() { }
        public void SetForm(FormSharePointHelper form) { Form = form; }

        public SPWeb Web { get { return Form.Web; } }

        public ToolStripItem AddFunctions(string Text)
        {
            return Form.AddFunctions(Text);
        }
        public void WriteLine(string fomat, params object[] args)
        {
            Form.WriteLine(fomat, args);
        }
        public void WriteLine(string text)
        {
            Form.WriteLine(text);
        }
        public SPList List(string listName)
        {
            return Form.List(listName);
        }
        public void ProgressBarInit(int value)
        {
            Form.ProgressBarInit(value);
        }
        public void ProgressBarInit(string lable, int value)
        {
            Form.ProgressBarInit(lable, value);
        }
        public void ProgressBarNext()
        {
            Form.ProgressBarNext();
        }
    }
}
