using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizRunner
{
    public partial class IfrResult : Form
    {
        private readonly string GPath;

        public IfrResult(string arg)
        {
            InitializeComponent();
            GPath = arg;
        }

        private void IfrResult_Load(object sender, EventArgs e)
        {
            StreamReader SR = new StreamReader(GPath);
            var IttCreatorToolTip = new ToolTip();

            /// Боковое меню.
            #region
            // Панель меню.
            var IpnMenu = new Panel
            {
                BackColor = Color.FromArgb(18, 136, 235),
                Width = 60,
                Height = this.ClientSize.Height,
                Parent = this
            };

            // Кнопка закрытия программы.
            var IlbExit = new Label
            {
                AutoSize = false,
                Width = IpnMenu.Width,
                Height = IpnMenu.Width,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new Font("Verdana", 25, FontStyle.Bold),
                ForeColor = Color.White,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = IpnMenu,
                Top = IpnMenu.Height - IpnMenu.Width
            };
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                IlbExit.Text = "X";
            }
            else
            {
                IlbExit.Text = "❌";
            }
            IlbExit.MouseEnter += IlbExit_MouseEnter;
            IlbExit.MouseLeave += IlbExit_MouseLeave;
            IlbExit.Click += IlbExit_Click;
            IttCreatorToolTip.SetToolTip(IlbExit, "Закрыть " + AppDomain.CurrentDomain.FriendlyName.Substring(0,
                    AppDomain.CurrentDomain.FriendlyName.Length - 4));

            // Кнопка выхода в меню.
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
            #endregion

            var IpnMain = new Panel
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Width = this.Width - IpnMenu.Width,
                Height = this.Height,
                Left = IpnMenu.Width,
                Top = 0,
                Parent = this
            };
        }

        /// События основных графических элементов.
        #region
        // Вход курсора в область одной из кнопок меню.
        private void MenuButtons_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Width += 4;
            ((PictureBox)sender).Height += 4;
            ((PictureBox)sender).Left -= 2;
            ((PictureBox)sender).Top -= 2;
        }

        // Покидание курсором области одной из кнопок меню.
        private void MenuButtons_MouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).Width -= 4;
            ((PictureBox)sender).Height -= 4;
            ((PictureBox)sender).Left += 2;
            ((PictureBox)sender).Top += 2;
        }

        private void IlbExit_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Verdana", 30, FontStyle.Bold);
            ((Label)sender).BackColor = Color.Red;
        }

        private void IlbExit_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Verdana", 25, FontStyle.Bold);
            ((Label)sender).BackColor = Color.Transparent;
        }

        private void IlbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void IpbBack_Click(object sender, EventArgs e)
        {
            new IfrStartPage().Show();
            Close();
        }

        #endregion
    }
}
