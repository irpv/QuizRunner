using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace QuizRunner
{
    public partial class LoadingScreen : Form
    {
        string GMessage;
        IfrCreator GIfrCreator;
        public LoadingScreen(string message, IfrCreator sender)
        {
            InitializeComponent();
            GMessage = message;
            GIfrCreator = sender;
        }

        private void LoadingScreen_Load(object sender, EventArgs e)
        {
            var IpbLoading = new PictureBox
            {
                Width = this.Width / 5,
                Height = this.Width / 5,
                Image = Properties.Resources.Loading,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Parent = this
            };
            IpbLoading.Left = this.Width / 2 - IpbLoading.Width / 2;
            IpbLoading.Top = this.Height / 2 - IpbLoading.Height / 2 - 20;

            var IlbLoading = new Label
            {
                AutoSize = true,
                Text = GMessage,
                Font = new Font("Verdana", 25, FontStyle.Bold),
                ForeColor = Color.FromArgb(18, 136, 235),
                Parent = this
            };
            IlbLoading.Left = this.Width / 2 - IlbLoading.Width / 2;
            IlbLoading.Top = IpbLoading.Top + IpbLoading.Height + 20;

            var ItmTimer = new System.Windows.Forms.Timer();
            ItmTimer.Tick += ItmTimer_Tick;
            ItmTimer.Start();


        }

        private void ItmTimer_Tick(object sender, EventArgs e)
        {
            if (GIfrCreator.LoadingProcess == false)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void LoadingScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
