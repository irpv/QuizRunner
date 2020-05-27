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
            public RichTextBox IrtbQuestion;
            public bool Type;
            public RadioButton[] RadioButtonList;
            public CheckBox[] CheckBoxeList;
            public GroupBox IgbAnswerGroupBox;
            public Panel IpnAnswerPanel;
            public Button IbtNext;
            public Button IbtBack;
        }

        // Структура интерффейсов теста.
        private struct Test
        {
            public Label IlbTestName;
            public RichTextBox IrtbDescription;
            public Question[] QuestionList;
            public Button IbtStart;
            public Label[] StatisticsLines;
            public Dictionary<string, Double> UserVariables;
            public int NowQuestion;
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
                AutoScroll = true,
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

            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
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
            }
            else
            {
                var IbtOpen = new Button
                {
                    Text = "Выбрать файл...",
                    BackColor = Color.FromArgb(18, 136, 235),
                    Font = new Font("Verdana", 25, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Cursor = System.Windows.Forms.Cursors.Hand,
                    AutoSize = true,
                    Parent = IpnMain
                };
                IbtOpen.Left = IpnMain.Width / 2 - IbtOpen.Width / 2;
                IbtOpen.Top = IpnMain.Height / 2 - IbtOpen.Height / 2;
                IbtOpen.Click += IpbOpen_Click;
            }
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
            if (InProcess)
            {
                if (MessageBox.Show("Тест в процессе прохождения!" +
                    "\nЕсли вы продолжите это действие, вы потеряете все результаты." +
                    "\nЖелаете продолжиь?", "Закрыть программу?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    CanClose = true;
                    Application.Exit();
                }
            }
            else
            {
                CanClose = true;
                Application.Exit();
            }
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
            if (InProcess)
            {
                if (MessageBox.Show("Тест в процессе прохождения!" +
                    "\nЕсли вы продолжите это действие, вы потеряете все результаты." +
                    "\nЖелаете продолжиь?", "Выти в меню?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    CanClose = true;
                    new IfrStartPage().Show();
                    Close();
                }
            }
            else
            {
                CanClose = true;
                new IfrStartPage().Show();
                Close();
            }
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
                        "\nЖелаете продолжиь?", "Открыть файл?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (GIofdOpenDialog.ShowDialog() == DialogResult.OK)
                        {
                            this.LoadingProcess = true;
                            GEditor = new QuizRunner.Editor.Editor();
                            GEditor.Open(GIofdOpenDialog.FileName);
                            this.Controls[0].Controls[0].Visible = false;
                            LoadTest(ref GTest, GEditor);
                            this.InProcess = false;
                            this.LoadingProcess = false;
                        }
                    }
                }
                else
                {
                    if (GIofdOpenDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.LoadingProcess = true;
                        GEditor = new QuizRunner.Editor.Editor();
                        GEditor.Open(GIofdOpenDialog.FileName);
                        this.Controls[0].Controls[0].Visible = false;
                        LoadTest(ref GTest, GEditor);
                        this.InProcess = false;
                        this.LoadingProcess = false;
                    }
                }

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
        public void Open(string path)
        {
            try
            {
                if (InProcess)
                {
                    if (MessageBox.Show("Тест в процессе прохождения!" +
                        "\nЕсли вы продолжите это действие, вы потеряете все результаты." +
                        "\nЖелаете продолжиь?", "Открыть файл?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        this.LoadingProcess = true;
                        GEditor = new QuizRunner.Editor.Editor();
                        GEditor.Open(path);
                        this.Controls[0].Controls[0].Visible = false;
                        LoadTest(ref GTest, GEditor);
                        this.InProcess = false;
                        this.LoadingProcess = false;
                    }
                }
                else
                {
                    this.LoadingProcess = true;
                    GEditor = new QuizRunner.Editor.Editor();
                    GEditor.Open(path);
                    this.Controls[0].Controls[0].Visible = false;
                    LoadTest(ref GTest, GEditor);
                    this.InProcess = false;
                    this.LoadingProcess = false;
                }

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
        /// Загружает интерфейсы тестирования.
        /// </summary>
        /// <param name="test">интерфейсы тестирования</param>
        /// <param name="editor">тест</param>
        private void LoadTest(ref Test test,QuizRunner.Editor.Editor editor)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                Task.Run(() =>
                {
                    LoadingScreen TIfrLoadScreen =
                        new LoadingScreen("Секундочку, тест открывается.", this);
                    TIfrLoadScreen.ShowDialog();
                });
                Thread.Sleep(2000);
                this.Hide();
            }

            /// Основные элементы
            #region
            this.Controls[0].Dispose();
            var TIpnMain = new Panel
            {
                AutoScroll = true,
                BackColor = Color.White,
                Left = this.Controls[0].Width,
                Top = 0,
                Width = this.Width - this.Controls[0].Width,
                Height = this.Height,
                Parent = this
            };
            TIpnMain.BringToFront();

            test = new Test();

            test.UserVariables = editor.ListOfVariables;

            // Название теста
            test.IlbTestName = new Label
            {
                AutoSize = true,
                Text = editor.GetName(),
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 25, FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Parent = TIpnMain
            };
            test.IlbTestName.Left = TIpnMain.Width / 2 - test.IlbTestName.Width / 2;

            // Описание теста
            test.IrtbDescription = new RichTextBox
            {
                BackColor = Color.White,
                Width = TIpnMain.Width - 40,
                Height = TIpnMain.Height - test.IlbTestName.Height - 60,
                BorderStyle = BorderStyle.None,
                Font = new Font("Verdana", 20, FontStyle.Bold),
                SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center,
                ReadOnly = true,
                Parent = TIpnMain
            };
            test.IrtbDescription.Left = TIpnMain.Width / 2 - test.IrtbDescription.Width / 2;
            test.IrtbDescription.Top = test.IlbTestName.Height + 20;
            for (var i = 0; i < editor.GetDescription().Length; i++)
            {
                test.IrtbDescription.AppendText(editor.GetDescription()[i] + "\n");
            }
            test.IrtbDescription.LinkClicked += (s, e) =>
            {
                if (Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    System.Diagnostics.Process.Start(e.LinkText);
                }
                else
                {
                    if (MessageBox.Show("Скопировать ссылку в буфер обмена?",
                        "Копировать ссылку?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    {
                        Clipboard.SetText(e.LinkText);
                    }
                }
            };

            // Кнопка для старта тестирования
            test.IbtStart = new Button
            {
                Text = "Начать тест",
                BackColor = Color.FromArgb(18, 136, 235),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = System.Windows.Forms.Cursors.Hand,
                AutoSize = true,
                Top = test.IrtbDescription.Top + test.IrtbDescription.Height + 10,
                Parent = TIpnMain
            };
            test.IbtStart.Left = TIpnMain.Width / 2 - test.IbtStart.Width / 2;

            // Панель для лейбла вопроса.
            var IpnQuestionPanel = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Width = TIpnMain.Width,
                Height = TIpnMain.Height / 3 - 20,
                Visible = false,
                Parent = TIpnMain
            };


            // Нажатие на кнопку старта
            test.IbtStart.Click += (s, e) =>
            {
                GTest.IlbTestName.Visible = false;
                GTest.IrtbDescription.Visible = false;
                GTest.IbtStart.Visible = false;
                IpnQuestionPanel.Visible = true;
                //IgbAnswer.Visible = true;
                MoveNext(0,0, GTest);
                this.InProcess = true;
            };

            // Текстбокс для перехвата курсора.
            var ItbHideCursor = new TextBox
            {
                Left = -100,
                Top = -100,
                Parent = TIpnMain
            };
            #endregion

            /// Вопросы
            #region
            test.QuestionList = new Question[0];
            for (var i = 0; i < editor.NumberOfQuestion(); i++)
            {
                Array.Resize<Question>(ref test.QuestionList, test.QuestionList.Length + 1);

                // Лейбл вопроса.
                test.QuestionList[i].IrtbQuestion = new RichTextBox
                {
                    Top = 5,
                    BackColor = Color.White,
                    Height = IpnQuestionPanel.Height - 10,
                    Width = TIpnMain.Width - 40,
                    Left = 20,
                    Font = new Font("Verdana", 15, FontStyle.Bold),
                    SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center,
                    ReadOnly = true,
                    ContextMenu = new ContextMenu(),
                    ShortcutsEnabled = false,
                    BorderStyle = BorderStyle.None,
                    Parent = IpnQuestionPanel
                };
                test.QuestionList[i].IrtbQuestion.LinkClicked += (s, e) =>
                {
                    if (Environment.OSVersion.Platform != PlatformID.Unix)
                    {
                        System.Diagnostics.Process.Start(e.LinkText);
                    }
                    else
                    {
                        if (MessageBox.Show("Скопировать ссылку в буфер обмена?",
                            "Копировать ссылку?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            Clipboard.SetText(e.LinkText);
                        }
                    }
                };
                for (var j = 0; j < editor.GetQuestionText(i).Length; j++)
                {
                    test.QuestionList[i].IrtbQuestion.
                        AppendText(editor.GetQuestionText(i)[j] + "\n");
                }
                test.QuestionList[i].IrtbQuestion.Left = IpnQuestionPanel.Width / 2 -
                    test.QuestionList[i].IrtbQuestion.Width / 2;
                test.QuestionList[i].IrtbQuestion.GotFocus += (s, e) =>
                {
                    if (Environment.OSVersion.Platform != PlatformID.Unix)
                    {
                        ItbHideCursor.Focus();
                        ItbHideCursor.Select(0, -1);
                    }
                    else
                    {
                        ItbHideCursor.Focus();
                        ItbHideCursor.Select(0, 1);
                    }
                };

                // Тип ответа
                test.QuestionList[i].Type = editor.GetAnswerType(i);

                test.QuestionList[i].IgbAnswerGroupBox = new GroupBox
                {
                    Text = "Ответы",
                    Font = new Font("Verdana", 8, FontStyle.Bold),
                    Width = TIpnMain.Width - 40,
                    Height = TIpnMain.Height - IpnQuestionPanel.Height - 50,
                    Left = 20,
                    Top = IpnQuestionPanel.Height + 20,
                    Visible = false,
                    Parent = TIpnMain
                };

                test.QuestionList[i].IpnAnswerPanel = new Panel
                {
                    AutoScroll = true,
                    Left = 15,
                    Top = 15,
                    Width = test.QuestionList[i].IgbAnswerGroupBox.Width - 30,
                    Height = test.QuestionList[i].IgbAnswerGroupBox.Height - 30,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Parent = test.QuestionList[i].IgbAnswerGroupBox
                };

                // Ответы
                if (test.QuestionList[i].Type)
                {
                    // Радиобаттаны
                    if (editor.NumberOfAnswers(i) > 0)
                    {
                        test.QuestionList[i].RadioButtonList = new RadioButton[1];
                        test.QuestionList[i].RadioButtonList[0] = new RadioButton
                        {
                            AutoSize = true,
                            Left = 2,
                            Top = 2,
                            Font = new Font("Verdana", 10, FontStyle.Bold),
                            Text = editor.GetAnswerText(i, 0),
                            Tag = editor.GetAnswerArgument(i, 0),
                            Visible = false,
                            Parent = test.QuestionList[i].IpnAnswerPanel
                        };
                        for (var j = 1; j < editor.NumberOfAnswers(i); j++)
                        {
                            Array.Resize<RadioButton>(ref test.QuestionList[i].RadioButtonList,
                                test.QuestionList[i].RadioButtonList.Length + 1);
                            test.QuestionList[i].RadioButtonList[j] = new RadioButton
                            {
                                AutoSize = true,
                                Left = 2,
                                Top = test.QuestionList[i].RadioButtonList[j - 1].Top
                                    + test.QuestionList[i].RadioButtonList[j - 1].Height + 4,
                                Font = new Font("Verdana", 10, FontStyle.Bold),
                                Text = editor.GetAnswerText(i, j),
                                Tag = editor.GetAnswerArgument(i, j),
                                Visible = false, 
                                Parent = test.QuestionList[i].IpnAnswerPanel
                            };
                        }
                    }
                }
                else
                {
                    // Чекбоксы
                    if (editor.NumberOfAnswers(i) > 0)
                    {
                        test.QuestionList[i].CheckBoxeList = new CheckBox[1];
                        test.QuestionList[i].CheckBoxeList[0] = new CheckBox
                        {
                            AutoSize = true,
                            Left = 2,
                            Top = 2,
                            Font = new Font("Verdana", 10, FontStyle.Bold),
                            Text = editor.GetAnswerText(i, 0),
                            Tag = editor.GetAnswerArgument(i, 0),
                            Visible = false,
                            Parent = test.QuestionList[i].IpnAnswerPanel
                        };
                        for (var j = 1; j < editor.NumberOfAnswers(i); j++)
                        {
                            Array.Resize<CheckBox>(ref test.QuestionList[i].CheckBoxeList,
                                test.QuestionList[i].CheckBoxeList.Length + 1);
                            test.QuestionList[i].CheckBoxeList[j] = new CheckBox
                            {
                                AutoSize = true,
                                Left = 2,
                                Top = test.QuestionList[i].CheckBoxeList[j - 1].Top
                                    + test.QuestionList[i].CheckBoxeList[j - 1].Height + 4,
                                Font = new Font("Verdana", 10, FontStyle.Bold),
                                Text = editor.GetAnswerText(i, j),
                                Tag = editor.GetAnswerArgument(i, j),
                                Visible = false,
                                Parent = test.QuestionList[i].IpnAnswerPanel
                            };

                        }
                    }
                }

                // Кнопка перехода.
                if (i != editor.NumberOfQuestion() - 1)
                {
                    test.QuestionList[i].IbtNext = new Button
                    {
                        Text = "Следующий вопрос",
                        BackColor = Color.FromArgb(18, 136, 235),
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Cursor = System.Windows.Forms.Cursors.Hand,
                        AutoSize = true,
                        Top = test.QuestionList[i].IgbAnswerGroupBox.Top + test.QuestionList[i].IgbAnswerGroupBox.Height + 2,
                        Visible = false,
                        Parent = TIpnMain
                    };
                    test.QuestionList[i].IbtNext.Left = TIpnMain.Width / 2
                        - test.QuestionList[i].IbtNext.Width / 2;
                    // Лямбда кнопки.
                    test.QuestionList[i].IbtNext.Click += (s, e) =>
                    {
                        MoveNext(GTest.NowQuestion, GTest.NowQuestion + 1, GTest);
                    };
                }
                else
                {
                    test.QuestionList[i].IbtNext = new Button
                    {
                        Text = "Завершить тест",
                        BackColor = Color.FromArgb(18, 136, 235),
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Cursor = System.Windows.Forms.Cursors.Hand,
                        AutoSize = true,
                        Top = test.QuestionList[i].IgbAnswerGroupBox.Top + test.QuestionList[i].IgbAnswerGroupBox.Height + 2,
                        Visible = false,
                        Parent = TIpnMain
                    };
                    test.QuestionList[i].IbtNext.Left = TIpnMain.Width / 2
                        - test.QuestionList[i].IbtNext.Width / 2;
                    // Лямбда кнопки.
                    test.QuestionList[i].IbtNext.Click += (s, e) =>
                    {
                        if (MessageBox.Show("Вы действительно готовы завершить тест?", "Завершить тест", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        { 
                            MoveNext(GTest.NowQuestion, GTest.NowQuestion + 1, GTest);
                        }
                    };
                }

                // Кнопка возвращения.
                test.QuestionList[i].IbtBack = new Button
                {
                    Text = "Назад",
                    BackColor = Color.FromArgb(18, 136, 235),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Cursor = System.Windows.Forms.Cursors.Hand,
                    AutoSize = true,
                    Top = test.QuestionList[i].IbtNext.Top,
                    Visible = false,
                    Parent = TIpnMain
                };
                test.QuestionList[i].IbtBack.Left = test.QuestionList[i].IbtNext.Left - test.QuestionList[i].IbtBack.Width - 20;
                // Лямбда кнопки.
                test.QuestionList[i].IbtBack.Click += (s, e) =>
                {
                    MoveNext(GTest.NowQuestion, GTest.NowQuestion - 1, GTest);
                };
            }
            #endregion

            /// Строки статистики.
            #region
            if (editor.NumberOfStatLine() > 0)
            {
                test.StatisticsLines = new Label[1];
                test.StatisticsLines[0] = new Label
                {
                    AutoSize = true,
                    Font = new Font("Verdana", 15, FontStyle.Bold),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Visible = false,
                    Parent = TIpnMain
                };

                for (var i = 1; i < editor.NumberOfStatLine(); i++)
                {
                    Array.Resize<Label>(ref test.StatisticsLines, test.StatisticsLines.Length + 1);
                    test.StatisticsLines[i] = new Label
                    {
                        Top = TIpnMain.Controls[TIpnMain.Controls.Count - 1].Top +
                            TIpnMain.Controls[TIpnMain.Controls.Count - 1].Height + 10,
                        AutoSize = true,
                        Font = new Font("Verdana", 15, FontStyle.Bold),
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                        Visible = false,
                        Parent = TIpnMain
                    };
                }
            }
            #endregion

            this.Show();
            ItbHideCursor.Focus();
            Thread.Sleep(500);
        }

        /// <summary>
        /// Выключает интерфейс предидущей страницы, и включает новый.
        /// </summary>
        /// <param name="index">номер новой страницы</param>
        /// <param name="test">интерфейсы тестов</param>
        private void MoveNext(int oldindex, int index, Test test)
        {
            if ((oldindex >= 0) && (index < test.QuestionList.Length))
            {
                // Выключаем старые интерфейсы.
                test.QuestionList[oldindex].IrtbQuestion.Visible = false;
                test.QuestionList[oldindex].IbtNext.Visible = false;
                test.QuestionList[oldindex].IgbAnswerGroupBox.Visible = false;
                test.QuestionList[oldindex].IbtBack.Visible = false;
                if (test.QuestionList[oldindex].Type)
                {
                    for (var i = 0; i < test.QuestionList[oldindex].RadioButtonList.Length; i++)
                    {
                        test.QuestionList[oldindex].RadioButtonList[i].Visible = false;
                    }
                }
                else
                {
                    for (var i = 0; i < test.QuestionList[oldindex].CheckBoxeList.Length; i++)
                    {
                        test.QuestionList[oldindex].CheckBoxeList[i].Visible = false;
                    }
                }

                // Включаем новые интерфейсы.

                test.QuestionList[index].IrtbQuestion.Visible = true;
                test.QuestionList[index].IbtNext.Visible = true;
                test.QuestionList[index].IgbAnswerGroupBox.Visible = true;
                if (index != 0)
                {
                    test.QuestionList[index].IbtBack.Visible = true;
                }
                if (test.QuestionList[index].Type)
                {
                    for (var i = 0; i < test.QuestionList[index].RadioButtonList.Length; i++)
                    {
                        test.QuestionList[index].RadioButtonList[i].Visible = true;
                    }
                }
                else
                {
                    for (var i = 0; i < test.QuestionList[index].CheckBoxeList.Length; i++)
                    {
                        test.QuestionList[index].CheckBoxeList[i].Visible = true;
                    }
                }
                GTest.NowQuestion = index;
            }
            else if (oldindex < 0)
            {
                // Включаем новые интерфейсы.

                test.QuestionList[index].IrtbQuestion.Visible = true;
                test.QuestionList[index].IbtNext.Visible = true;
                test.QuestionList[index].IgbAnswerGroupBox.Visible = false;
                if (index != 0)
                {
                    test.QuestionList[index].IbtBack.Visible = true;
                }
                if (test.QuestionList[index].Type)
                {
                    for (var i = 0; i < test.QuestionList[index].RadioButtonList.Length; i++)
                    {
                        test.QuestionList[index].RadioButtonList[i].Visible = true;
                    }
                }
                else
                {
                    for (var i = 0; i < test.QuestionList[index].CheckBoxeList.Length; i++)
                    {
                        test.QuestionList[index].CheckBoxeList[i].Visible = true;
                    }
                }
                GTest.NowQuestion = index;
            }
            else
            {
                // Выключаем старые интерфейсы.
                test.QuestionList[oldindex].IrtbQuestion.Visible = false;
                test.QuestionList[oldindex].IbtNext.Visible = false;
                test.QuestionList[oldindex].IgbAnswerGroupBox.Visible = false;
                test.QuestionList[oldindex].IgbAnswerGroupBox.Visible = false;
                if (test.QuestionList[oldindex].Type)
                {
                    for (var i = 0; i < test.QuestionList[oldindex].RadioButtonList.Length; i++)
                    {
                        test.QuestionList[oldindex].RadioButtonList[i].Visible = false;
                    }
                }
                else
                {
                    for (var i = 0; i < test.QuestionList[oldindex].CheckBoxeList.Length; i++)
                    {
                        test.QuestionList[oldindex].CheckBoxeList[i].Visible = false;
                    }
                }

                // Выключаем все визаульные элементы главной панели.
                for (var i = 0; i < this.Controls[0].Controls.Count; i++)
                {
                    this.Controls[0].Controls[i].Visible = false;
                }

                

                // Подсчитываем все аргументы. 

                for (var i = 0; i < GTest.QuestionList.Length; i++)
                {
                    if (GTest.QuestionList[i].Type)
                    {
                        for (var k = 0; k < GTest.QuestionList[i]
                        .RadioButtonList.Length; k++)
                        {
                            if (GTest.QuestionList[i].RadioButtonList[k].Checked)
                            {
                                var TArgumentArray =
                                    (String[])GTest.QuestionList[i].RadioButtonList[k].Tag;

                                for (var l = 0; l < TArgumentArray.Length; l++)
                                {
                                    QuizRunner.Testing.Testing TTesting = new QuizRunner.Testing.Testing();
                                    TArgumentArray[l] = TTesting.SimplifyArg(TArgumentArray[l]);
                                    GTest.UserVariables[TTesting.GetArgumentName(TArgumentArray[l])]
                                        = Convert.ToDouble(TTesting.GetCompute(TArgumentArray[l], GTest.UserVariables));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var k = 0; k < GTest.QuestionList[i]
                        .CheckBoxeList.Length; k++)
                        {
                            if (GTest.QuestionList[i].CheckBoxeList[k].Checked)
                            {
                                var TArgumentArray =
                                    (String[])GTest.QuestionList[i].CheckBoxeList[k].Tag;
                                for (var l = 0; l < TArgumentArray.Length; l++)
                                {
                                    QuizRunner.Testing.Testing TTesting = new QuizRunner.Testing.Testing();
                                    TArgumentArray[l] = TTesting.SimplifyArg(TArgumentArray[l]);
                                    GTest.UserVariables[TTesting.GetArgumentName(TArgumentArray[l])]
                                        = Convert.ToDouble(TTesting.GetCompute(TArgumentArray[l], GTest.UserVariables));
                                }
                            }
                        }
                    }
                }

                // Лейбл завершения теста.
                var IlbTestEnd = new Label
                {
                    AutoSize = true,
                    Text = "Тест завершен",
                    ForeColor = Color.FromArgb(18, 136, 235),
                    Font = new Font("Verdana", 25, FontStyle.Bold),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Parent = this.Controls[0]
                };
                IlbTestEnd.Left = this.Controls[0].Width / 2 - IlbTestEnd.Width / 2;


                // Заполняем строки статистики
                for (var i = 0; i < test.StatisticsLines.Length; i++)
                {
                    test.StatisticsLines[i].Text = $"{GEditor.GetStatPrefix(i)}  " +
                        $"{String.Format("{0:0.00}", new QuizRunner.Testing.Testing().GetCompute(GEditor.GetStatCalculate(i), GTest.UserVariables))}" +
                        $"{GEditor.GetStatPostfix(i)}";
                    test.StatisticsLines[i].Left = test.StatisticsLines[i].Parent.Width / 2
                        - test.StatisticsLines[i].Width / 2;
                    if (i == 0)
                    {
                        test.StatisticsLines[i].Top = IlbTestEnd.Height + 25;
                    }
                    else
                    {
                        test.StatisticsLines[i].Top = test.StatisticsLines[i - 1].Top
                            + test.StatisticsLines[i - 1].Height + 10;
                    }
                }

                // Включаем строки статистики.
                for (var i = 0; i < test.StatisticsLines.Length; i++)
                {
                    test.StatisticsLines[i].Visible = true;
                }
                GTest.NowQuestion = index ;

                var IbtSaveResult = new Button
                {
                    Text = "Сохранить результаты",
                    BackColor = Color.FromArgb(18, 136, 235),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Cursor = System.Windows.Forms.Cursors.Hand,
                    AutoSize = true,
                    Top = test.StatisticsLines[test.StatisticsLines.Length - 1].Top
                       + test.StatisticsLines[test.StatisticsLines.Length - 1].Height + 10,
                    Parent = this.Controls[0]
                };
                IbtSaveResult.Left = IbtSaveResult.Parent.Width / 2 - IbtSaveResult.Width / 2;
            }
        }
    }
}
