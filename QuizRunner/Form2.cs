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
    public partial class IfrStartPage : Form
    {
        // Указывает, разрешено ли форме закрыться.
        public bool CanClose;

        public IfrStartPage()
        {
            InitializeComponent();
        }

        private void IfrStartPage_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;

            var IttMainToolTip = new ToolTip();

            var IlbExit = new Label
            {
                AutoSize = true,
                Text = "❌",
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.Gray,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = this
            };
            IlbExit.Left = this.ClientSize.Width - IlbExit.Width;
            IlbExit.MouseEnter += IlbExit_MouseEnter;
            IlbExit.MouseLeave += IlbExit_MouseLeave;
            IlbExit.MouseClick += IlbExit_Click;

            var IlbTitle = new Label
            {
                AutoSize = true,
                Text = AppDomain.CurrentDomain.FriendlyName.Substring(0, 
                    AppDomain.CurrentDomain.FriendlyName.Length - 4),
                Font = new Font("Verdana", 15),
                ForeColor = Color.Gray,
                Parent=this
            };

            var IpbCreatorButton = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.CreatorPic,
                Width = this.Width / 5,
                Height = this.Width / 5,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = this
            };
            IpbCreatorButton.Left = this.Width / 2 - IpbCreatorButton.Width 
                - IpbCreatorButton.Width/2 - 20;
            IpbCreatorButton.Top = this.Height / 2 - IpbCreatorButton.Height / 2;
            IpbCreatorButton.MouseEnter += IpbCreatorButton_MouseEnter;
            IpbCreatorButton.MouseLeave += IpbCreatorButton_MouseLeave;
            IttMainToolTip.SetToolTip(IpbCreatorButton, "Создать или отредактировать тест");
        }

        private void IfrStartPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CanClose)
            {
                Application.Exit();
            }
            else
            {
                this.Dispose();
            }
        }

        private void IlbExit_MouseEnter(object sender, EventArgs e)
        {
            var IlbExit = (Label)sender;
            IlbExit.BackColor = Color.Red;
            IlbExit.ForeColor = Color.White;
        }

        private void IlbExit_MouseLeave(object sender, EventArgs e)
        {
            var IlbExit = (Label)sender;
            IlbExit.BackColor = Color.Transparent;
            IlbExit.ForeColor = Color.Gray;
        }

        private void IlbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void IpbCreatorButton_MouseEnter(object sender, EventArgs e)
        {
            var IpbCreatorButton = (PictureBox)sender;
            IpbCreatorButton.Width += 20;
            IpbCreatorButton.Height += 20;
            IpbCreatorButton.Left -= 10;
            IpbCreatorButton.Top -= 10;
        }

        private void IpbCreatorButton_MouseLeave(object sender, EventArgs e)
        {
            var IpbCreatorButton = (PictureBox)sender;
            IpbCreatorButton.Width -= 20;
            IpbCreatorButton.Height -= 20;
            IpbCreatorButton.Left += 10;
            IpbCreatorButton.Top += 10;
        }

    }
}
