using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizRunner
{
    public partial class IfrMain : Form
    {
        public IfrMain()
        {
            InitializeComponent();
        }

        private void IfrMain_Load(object sender, EventArgs e)
        {
            IfrStartPage IStartPage = new IfrStartPage();
            IStartPage.Show();
        }

        private void IfrMain_Activated(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
