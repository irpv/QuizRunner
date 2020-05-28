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
                BackColor = Color.FromArgb(240, 240, 240),
                Width = this.Width - IpnMenu.Width,
                Height = this.Height,
                Left = IpnMenu.Width,
                Top = 0,
                AutoScroll = true,
                Parent = this
            };

            /// Загрузка тестов.
            #region

            var IlbName = new Label
            {
                AutoSize = false,
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 25, FontStyle.Bold),
                Text = SR.ReadLine(),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Width = IpnMain.Width,
                Height = 45,
                Top = 5,
                Parent = IpnMain
            };



            var IrtbDescription = new RichTextBox
            {
                BackColor = Color.White,
                Width = IpnMain.Width - 40,
                Height = 150,
                BorderStyle = BorderStyle.None,
                Font = new Font("Verdana", 20, FontStyle.Bold),
                SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center,
                ReadOnly = true,
                Left = 20,
                Top = 55,
                Enabled = false,
                Parent = IpnMain
            };

            var TNumber = Convert.ToInt32(SR.ReadLine());

            for (var i = 0; i < TNumber; i++)
            {
                IrtbDescription.Text += SR.ReadLine() + "\n";
            }

            TNumber = Convert.ToInt32(SR.ReadLine());

            var TTop = 210;

            for (var i = 0; i < TNumber; i++)
            {
                var TIrtbQuestion = new RichTextBox
                {
                    BackColor = Color.White,
                    Width = IpnMain.Width - 60,
                    Height = 100,
                    BorderStyle = BorderStyle.None,
                    Font = new Font("Verdana", 15, FontStyle.Bold),
                    SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center,
                    ReadOnly = true,
                    Left = 30,
                    Top = TTop,
                    Enabled = false,
                    Parent = IpnMain
                };

                var TAnswerNumber = Convert.ToInt32(SR.ReadLine());

                for (var j = 0; j < TAnswerNumber; j++)
                {
                    TIrtbQuestion.Text += SR.ReadLine() + "\n";
                }

                var TIpnAnswer = new Panel
                {
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.FromArgb(240, 240, 240),
                    Width = IpnMain.Width - 60,
                    Height = 150,
                    Left = 30,
                    Top = TTop + 105,
                    AutoScroll = true,
                    Parent = IpnMain
                };

                if (Convert.ToBoolean(SR.ReadLine()))
                {
                    TAnswerNumber = Convert.ToInt32(SR.ReadLine());
                    for (var j = 0; j < TAnswerNumber; j++)
                    {
                        var TIrbAnswer = new RadioButton
                        {
                            AutoSize = true,
                            Text = SR.ReadLine(),
                            Checked = Convert.ToBoolean(SR.ReadLine()),
                            Left = 10,
                            Top = 30 * j,
                            Enabled = false,
                            Parent = TIpnAnswer
                        };
                    }
                }

                TTop += 260;
            }

            TNumber = Convert.ToInt32(SR.ReadLine());

            var IrtbStatistic = new RichTextBox
            {
                BackColor = Color.White,
                Width = IpnMain.Width - 40,
                Height = 150,
                BorderStyle = BorderStyle.None,
                Font = new Font("Verdana", 20, FontStyle.Bold),
                SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center,
                ReadOnly = true,
                Left = 20,
                Top = TTop + 5,
                Enabled = false,
                Parent = IpnMain
            };

            for (var i = 0; i < TNumber; i++)
            {
                IrtbStatistic.Text += SR.ReadLine() + "\n";
            }

            #endregion

            SR.Close();
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
