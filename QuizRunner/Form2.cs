﻿using System;
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
        // Свойство формы, указывающее ращрешено ли её закрыться.
        public bool CanClose;

        public IfrStartPage()
        {
            InitializeComponent();
        }

        private void IfrStartPage_Load(object sender, EventArgs e)
        {

            this.BackColor = Color.White;

            var IttMainToolTip = new ToolTip();
            /// Заголовок программы.
            #region
            // Кнопка закрытия.
            var IlbExit = new Label
            {
                AutoSize = true,
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.Gray,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = this
            };
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                IlbExit.Text = "X";
            }
            else
            {
                IlbExit.Text = "❌";
            }
            IlbExit.Left = this.ClientSize.Width - IlbExit.Width;
            IlbExit.MouseEnter += IlbExit_MouseEnter;
            IlbExit.MouseLeave += IlbExit_MouseLeave;
            IlbExit.MouseClick += IlbExit_Click;
            
            // Вывод имени программы.
            var IlbTitle = new Label
            {
                AutoSize = true,
                Text = AppDomain.CurrentDomain.FriendlyName.Substring(0, 
                    AppDomain.CurrentDomain.FriendlyName.Length - 4),
                Font = new Font("Verdana", 15),
                ForeColor = Color.Gray,
                Parent=this
            };

            // Издание программы
            var IlbEdition = new Label
            {
                AutoSize = true,
                Font = new Font("Verdana", 12),
                ForeColor = Color.Gray,
                Left = IlbTitle.Left + IlbTitle.Width + 5,
                Parent = this
            };
            if (Environment.OSVersion.Platform.ToString().Contains("Win"))
            {
                IlbEdition.Text = "Windows Edition";
            }
            else
            {
                IlbEdition.Text = Environment.OSVersion.Platform.ToString() 
                    + " Edition";
            }
            IlbEdition.Top = IlbTitle.Top + IlbTitle.Height 
                - IlbEdition.Height;
            #endregion

            /// Оснавные элементы управления.
            #region
            // Кнопка перехода в редактор тестов.
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
            IpbCreatorButton.MouseEnter += MainButtons_MouseEnter;
            IpbCreatorButton.MouseLeave += MainButtons_MouseLeave;
            IpbCreatorButton.Click += IpbCreatorButton_Click;
            IttMainToolTip.SetToolTip(IpbCreatorButton, "Создать или отредактировать тест");

            // Кнопка перехода в прохождение теста.
            var IpbQuizButton = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.QuizPic,
                Width = this.Width / 5,
                Height = this.Width / 5,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = this
            };
            IpbQuizButton.Left = this.Width / 2 - IpbQuizButton.Width / 2;
            IpbQuizButton.Top = this.Height / 2 - IpbQuizButton.Height / 2;
            IpbQuizButton.MouseEnter += MainButtons_MouseEnter;
            IpbQuizButton.MouseLeave += MainButtons_MouseLeave;
            IpbQuizButton.Click += IpbQuizButton_Click;
            IttMainToolTip.SetToolTip(IpbQuizButton, "Пройти тест");

            // Кнопка перехода к просмотру результатов теста.
            var IpbResultButton = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.ResultPic,
                Width = this.Width / 5,
                Height = this.Width / 5,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = this
            };
            IpbResultButton.Left = this.Width / 2 + IpbResultButton.Width / 2 + 20;
            IpbResultButton.Top = this.Height / 2 - IpbResultButton.Height / 2;
            IpbResultButton.MouseEnter += MainButtons_MouseEnter;
            IpbResultButton.MouseLeave += MainButtons_MouseLeave;
            IpbResultButton.Click += IpbResultButton_Click;
            IttMainToolTip.SetToolTip(IpbResultButton, "Просмотреть результаты");
            #endregion

            /// Дополнительные элементы.
            #region
            // Вывод имени пользователя.
            var IlbUserName = new Label
            {
                AutoSize = true,
                Text = Environment.UserName,
                Font = new Font("Verdana", 25, FontStyle.Bold),
                ForeColor = Color.FromArgb(18, 136, 235),
                Parent = this
            };
            IlbUserName.Left = this.Width / 2 - IlbUserName.Width / 2;
            IlbUserName.Top = IpbQuizButton.Top - IlbUserName.Height - 15;

            // Вывод приветствия.
            var IlbWelcome = new Label
            {
                AutoSize = true,
                Text = "Добро пожаловать",
                Font = new Font("Verdana", 25),
                ForeColor = Color.FromArgb(18, 136, 235),
                Parent = this
            };
            IlbWelcome.Left = this.Width / 2 - IlbWelcome.Width / 2;
            IlbWelcome.Top = IlbUserName.Top - IlbWelcome.Height - 10;
            #endregion
        }

        /// События основных графических элементов.
        #region
        private void IfrStartPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
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

        }

        private void IlbExit_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Red;
            ((Label)sender).ForeColor = Color.White;
        }

        private void IlbExit_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Transparent;
            ((Label)sender).ForeColor = Color.Gray;
        }

        private void IlbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Вход курсора в область одной из кнопок.
        private void MainButtons_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Width += 20;
            ((PictureBox)sender).Height += 20;
            ((PictureBox)sender).Left -= 10;
            ((PictureBox)sender).Top -= 10;
        }

        //Покидание курсором области одной из кнопок.
        private void MainButtons_MouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).Width -= 20;
            ((PictureBox)sender).Height -= 20;
            ((PictureBox)sender).Left += 10;
            ((PictureBox)sender).Top += 10;
        }

        private void IpbCreatorButton_Click(object sender, EventArgs e)
        {
            this.CanClose = true;
            new IfrCreator().Show();
            this.Close();

        }

        private void IpbQuizButton_Click(object sender, EventArgs e)
        {
            this.CanClose = true;
            new IfrTesting().Show();
            this.Close();
        }

        private void IpbResultButton_Click(object sender, EventArgs e)
        {
            var TIsfdOpenDialog = new OpenFileDialog
            {
                Title = "Загрузить результаты",
                FileName = "Test.qrrf",
                Filter = "Результаты теста (*.qrrf)|*.qrrf|Все файлы (*.*)|*.*",
            };

            if (TIsfdOpenDialog.ShowDialog() == DialogResult.OK)
            {
                this.CanClose = true;
                new IfrResult(TIsfdOpenDialog.FileName).Show();
                this.Close();
            }
        }
        #endregion
    }
}
