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

        }

        private void IfrMain_Activated(object sender, EventArgs e)
        {
            this.Hide();
            if (GArgs.Length > 0)
            {
                string TFile = GArgs[0].Substring(GArgs[0].LastIndexOf('.') + 1);
                if (TFile == "qrrf")
                {
                    var IResultPage = new IfrResult(GArgs[0]);
                    IResultPage.Show();
                }
                else
                {
                    var ITestingPage = new IfrTesting();
                    ITestingPage.Show();
                    ITestingPage.Open(GArgs[0]);
                }
            }
            else
            {
                var IStartPage = new IfrStartPage();
                IStartPage.Show();
            }
        }
    }
}
