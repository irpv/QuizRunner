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
        private readonly String[] GArgs;
        public IfrMain(string[] args)
        {
            InitializeComponent();
            GArgs = args;
        }

        private void IfrMain_Load(object sender, EventArgs e)
        {
            if (GArgs.Length > 0)
            {
                var ITestingPage = new IfrTesting();
                ITestingPage.Show();
                ITestingPage.Open(GArgs[0]);
            }
            else
            {
                var IStartPage = new IfrStartPage();
                IStartPage.Show();
            }
        }

        private void IfrMain_Activated(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
