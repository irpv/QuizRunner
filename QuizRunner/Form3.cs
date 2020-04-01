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
    public partial class IfrCreator : Form
    {
        // Указывает, разрешено ли форме закрыться.
        public bool CanClose;

        public IfrCreator()
        {
            InitializeComponent();
        }

        private void IfrCreator_Load(object sender, EventArgs e)
        {
            var IpnMenu = new Panel
            {
                BackColor = Color.FromArgb(18, 136, 235),
                Width = 60,
                Height = this.ClientSize.Height,
                Parent =this
            };

            var IpbSave = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.SavePic,
                Width = 50,
                Height = 50,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = IpnMenu,
                Left = 5,
                Top = 5
            };
            IpbSave.MouseEnter += MenuButtons_MouseEnter;
            IpbSave.MouseLeave += MenuButtons_MouseLeave;

        }

        void MenuButtons_MouseEnter(object sender, EventArgs e)
        {
            var Button = (PictureBox)sender;
            Button.Width += 4;
            Button.Height += 4;
            Button.Left -= 2;
            Button.Top -= 2;
        }

        void MenuButtons_MouseLeave(object sender, EventArgs e)
        {
            var Button = (PictureBox)sender;
            Button.Width -= 4;
            Button.Height -= 4;
            Button.Left += 2;
            Button.Top += 2;
        }
    }
}
