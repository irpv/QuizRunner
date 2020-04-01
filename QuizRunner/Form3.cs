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

        private readonly SaveFileDialog IsfdSaveDialog = new SaveFileDialog
        {
            Title = "Сохранить",
            FileName = "Test.qrtf",
            Filter = "QuizRunner Test File (*.qrtf)|*.qrtf"
        };

        private void IfrCreator_Load(object sender, EventArgs e)
        {
            var IttCreatorToolTip = new ToolTip();

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
            IpbSave.Click += IpbSave_Click;
            IttCreatorToolTip.SetToolTip(IpbSave, "Сохранить...");

            var IpbOpen = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.OpenPic,
                Width = 50,
                Height = 50,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = IpnMenu,
                Left = 5,
                Top = IpbSave.Top + IpbSave.Height + 15
            };
            IpbOpen.MouseEnter += MenuButtons_MouseEnter;
            IpbOpen.MouseLeave += MenuButtons_MouseLeave;
            IttCreatorToolTip.SetToolTip(IpbOpen, "Открыть...");

            var IlbExit = new Label
            {
                AutoSize = false,
                Width = IpnMenu.Width,
                Height = IpnMenu.Width,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = "❌",
                Font = new Font("Verdana", 25, FontStyle.Bold),
                ForeColor = Color.White,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = IpnMenu,
                Top = IpnMenu.Height - IpnMenu.Width
            };
            IlbExit.MouseEnter += IlbExit_MouseEnter;
            IlbExit.MouseLeave += IlbExit_MouseLeave;
            IlbExit.Click += IlbExit_Click;
            IttCreatorToolTip.SetToolTip(IlbExit, "Закрыть " + AppDomain.CurrentDomain.FriendlyName.Substring(0,
                    AppDomain.CurrentDomain.FriendlyName.Length - 4));

            var IpbBack = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.BackPic,
                Width = 50,
                Height = 50,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = IpnMenu,
                Left = 5,
            };
            IpbBack.Top = IlbExit.Top - IpbBack.Height - 15;
            IpbBack.MouseEnter += MenuButtons_MouseEnter;
            IpbBack.MouseLeave += MenuButtons_MouseLeave;
            IpbBack.Click += IpbBack_Click;
            IttCreatorToolTip.SetToolTip(IpbBack, "Вернуться в меню");

        }

        private void MenuButtons_MouseEnter(object sender, EventArgs e)
        {
            var Button = (PictureBox)sender;
            Button.Width += 4;
            Button.Height += 4;
            Button.Left -= 2;
            Button.Top -= 2;
        }

        private void MenuButtons_MouseLeave(object sender, EventArgs e)
        {
            var Button = (PictureBox)sender;
            Button.Width -= 4;
            Button.Height -= 4;
            Button.Left += 2;
            Button.Top += 2;
        }

        private void IlbExit_MouseEnter(object sender, EventArgs e)
        {
            var IlbExit = (Label)sender;
            IlbExit.Font = new Font("Verdana", 30, FontStyle.Bold);
            IlbExit.BackColor = Color.Red;
        }

        private void IlbExit_MouseLeave(object sender, EventArgs e)
        {
            var IlbExit = (Label)sender;
            IlbExit.Font = new Font("Verdana", 25, FontStyle.Bold);
            IlbExit.BackColor = Color.Transparent;
        }

        private void IlbExit_Click(object sender, EventArgs e)
        {
            CanClose = true;
            Application.Exit();
        }

        private void IpbBack_Click(object sender, EventArgs e)
        {
            CanClose = true;
            var IStartPage = new IfrStartPage();
            IStartPage.Show();
            this.Close();
        }

        private void IpbSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (IsfdSaveDialog.ShowDialog()==DialogResult.OK)
            {
                //Тут должна быть функция сохранения.
            }
        }
    }
}
