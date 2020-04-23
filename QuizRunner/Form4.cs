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
    public partial class IfrTesting : Form
    {
        // Свойство формы, указывающее ращрешено ли её закрыться.
        public bool CanClose;

        // Свойство формы, указывающее были ли изменены данные, после открытия или создания.
        public bool Changed;

        // Свойство формы, указывающее, проходит ли на ней какая либо загрузка в данный момент.
        public bool LoadingProcess;

        // Свойство форму, указывающее, проходит ли сейчас тест.
        public bool InProcess;

        public IfrTesting()
        {
            InitializeComponent();
        }

        // Структура интерфейсов вопроса
        private struct Question
        {
            public Label IlbQuestion;
            public bool Type;
            public RadioButton[] RadioButtonList;
            public CheckBox[] CheckBoxeList;
            public Button IbtNext;
        }

        // Структура интерффейсов теста.
        private struct Test
        {
            public Label IlbTestName;
            public RichTextBox IrtbDescription;
            public Question[] QuestionList;
            public Button IbtStart;
        }

        // Переменная для теста.
        private Test GTest;

        private readonly OpenFileDialog GIofdOpenDialog = new OpenFileDialog
        {
            Title = "Открыть",
            FileName = "Test.qrtf",
            Filter = "QuizRunner Test File(*.qrtf)|*.qrtf|Все файлы|*.*"
        };

        private  QuizRunner.Editor.Editor GEditor = new QuizRunner.Editor.Editor();

        private void IfrTesting_Load(object sender, EventArgs e)
        {
            var IttCreatorToolTip = new ToolTip();

            /// Меню.
            #region
            // Панель меню.
            var IpnMenu = new Panel
            {
                BackColor = Color.FromArgb(18, 136, 235),
                Width = 60,
                Height = this.ClientSize.Height,
                Parent = this
            };


            // Кнопка открытия из файла.
            var IpbOpen = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Properties.Resources.OpenPic,
                Width = 50,
                Height = 50,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = IpnMenu,
                Left = 5,
                Top = 5
            };
            IpbOpen.MouseEnter += MenuButtons_MouseEnter;
            IpbOpen.MouseLeave += MenuButtons_MouseLeave;
            IpbOpen.MouseClick += IpbOpen_Click;
            //IpbOpen.Click += IpbOpen_Click;
            IttCreatorToolTip.SetToolTip(IpbOpen, "Открыть...");

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

            /// Основные элементы.
            #region
            // Главная панель.
            var IpnMain = new Panel
            {
                BackColor = Color.White,
                Left = IpnMenu.Width,
                Top = 0,
                Width = this.Width - IpnMenu.Width,
                Height = this.Height,
                Parent = this
            };
            IpnMain.BringToFront();

            /// Открытие теста
            #region
            var IlbDrop = new Label
            {
                AutoSize = false,
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Перетащите файл сюда.",
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 10, FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                AllowDrop = true,
                Width = IpnMain.Width / 2,
                Height = IpnMain.Height / 5,
                Parent = IpnMain
            };
            IlbDrop.Left = IpnMain.Width / 2 - IlbDrop.Width / 2;
            IlbDrop.Top = IpnMain.Height / 2 - IlbDrop.Height / 2;
            IlbDrop.DragEnter += IlbDrop_DragEnter;
            IlbDrop.DragLeave += IlbDrop_DragLeave;
            IlbDrop.DragDrop += IlbDrop_DragDrop;

            var IbtOpen = new Button
            {
                AutoSize = false,
                Width = IlbDrop.Width / 3,
                Height = IlbDrop.Height / 5,
                Text = "Выбрать файл...",
                FlatStyle = FlatStyle.System,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = IlbDrop
            };
            IbtOpen.Left = IlbDrop.Width / 2 - IbtOpen.Width / 2;
            IbtOpen.Top = IlbDrop.Height / 2 + 25;
            IbtOpen.Click += IpbOpen_Click;
            #endregion
            #endregion
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
            CanClose = true;
            Application.Exit();
        }

        private void IlbDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                ((Label)sender).BorderStyle = BorderStyle.Fixed3D;
                ((Label)sender).ForeColor = Color.Green;
                e.Effect = DragDropEffects.All;
            }
        }

        private void IlbDrop_DragLeave(object sender, EventArgs e)
        {
            ((Label)sender).BorderStyle = BorderStyle.FixedSingle;
            ((Label)sender).ForeColor = Color.FromArgb(18, 136, 235);
        }

        private void IlbDrop_DragDrop(object sender, DragEventArgs e)
        {
            var TPath = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            Open(TPath[0]);
            ((Label)sender).BorderStyle = BorderStyle.FixedSingle;
            ((Label)sender).ForeColor = Color.FromArgb(18, 136, 235);
        }

        private void IpbOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void IpbBack_Click(object sender, EventArgs e)
        {
            CanClose = true;
            new IfrStartPage().Show();
            Close();
        }

        #endregion

        /// <summary>
        /// Открывает файл, с помощью диалого открытия файла.
        /// </summary>
        private void Open()
        {
            try
            {
                if (InProcess)
                {
                    if (MessageBox.Show("Тест в процессе прохождения!" +
                        "\nЕсли вы продолжите это действие, вы потеряете все результаты." +
                        "\nЖелаете продолжиь?", "Открыть файл",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (GIofdOpenDialog.ShowDialog() == DialogResult.OK)
                        {
                            GEditor = new QuizRunner.Editor.Editor();
                            GEditor.Open(GIofdOpenDialog.FileName);
                        }
                    }
                }
                else
                {
                    if (GIofdOpenDialog.ShowDialog() == DialogResult.OK)
                    {
                        GEditor = new QuizRunner.Editor.Editor();
                        GEditor.Open(GIofdOpenDialog.FileName);
                    }
                }

                this.Controls[0].Controls[0].Visible = false;
                LoadTest(ref GTest, GEditor);
            }
            catch (System.FormatException)
            {
                this.LoadingProcess = false;
                this.Show();
                MessageBox.Show("Файл имеет неверный формат.", "Ошибка при открытии!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.IO.IOException)
            {
                this.LoadingProcess = false;
                this.Show();
                MessageBox.Show("Не удалось получить доступ к файлу.", "Ошибка при открытии!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                this.LoadingProcess = false;
                this.Show();
                MessageBox.Show("Не удалось открыть файл.\n" + e.Message, "Ошибка при открытии!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Открывает файл, по указанному пути.
        /// </summary>
        /// <param name="path">путь</param>
        private void Open(string path)
        {
            try
            {
                if (InProcess)
                {
                    if (MessageBox.Show("Тест в процессе прохождения!" +
                        "\nЕсли вы продолжите это действие, вы потеряете все результаты." +
                        "\nЖелаете продолжиь?", "Открыть файл",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        GEditor = new QuizRunner.Editor.Editor();
                        GEditor.Open(path);
                    }
                }
                else
                {
                    GEditor = new QuizRunner.Editor.Editor();
                    GEditor.Open(path);
                }

                this.Controls[0].Controls[0].Visible = false;
                LoadTest(ref GTest, GEditor);
            }
            catch (System.FormatException)
            {
                this.LoadingProcess = false;
                this.Show();
                MessageBox.Show("Файл имеет неверный формат.", "Ошибка при открытии!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.IO.IOException)
            {
                this.LoadingProcess = false;
                this.Show();
                MessageBox.Show("Не удалось получить доступ к файлу.", "Ошибка при открытии!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                this.LoadingProcess = false;
                this.Show();
                MessageBox.Show("Не удалось открыть файл.\n" + e.Message, "Ошибка при открытии!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTest(ref Test test,QuizRunner.Editor.Editor editor)
        {
            test = new Test();

            // Название теста
            test.IlbTestName = new Label
            {
                AutoSize = true,
                Text = editor.GetName(),
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 25, FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Parent = this.Controls[0]
            };
            test.IlbTestName.Left = this.Controls[0].Width / 2 - test.IlbTestName.Width / 2;

            // Описание теста
            test.IrtbDescription = new RichTextBox
            {
                Width = this.Controls[0].Width - 40,
                Height = this.Controls[0].Height - test.IlbTestName.Height - 60,
                BorderStyle = BorderStyle.None,
                Font = new Font("Verdana", 20, FontStyle.Bold),
                SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center,
                ReadOnly = true,
                Parent = this.Controls[0]
            };
            test.IrtbDescription.Left = this.Controls[0].Width / 2 - test.IrtbDescription.Width / 2;
            test.IrtbDescription.Top = test.IlbTestName.Height + 20;
            for (var i = 0; i < editor.GetDescription().Length; i++)
            {
                test.IrtbDescription.AppendText(editor.GetDescription()[i] + "\n");
            }
            test.IrtbDescription.LinkClicked += (s, e) =>
            {
                System.Diagnostics.Process.Start(e.LinkText);
            };


        }
    }
}
