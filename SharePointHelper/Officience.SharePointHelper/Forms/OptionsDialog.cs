using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Officience.SharePointHelper
{
    public partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();
        }

        public PropertyGrid Options
        {
            get { return gridProperties; }
            set { gridProperties = value; }
        }
    }
}
