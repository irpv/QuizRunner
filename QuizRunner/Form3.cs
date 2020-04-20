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
        // Свойство формы, указывающее ращрешено ли её закрыться.
        public bool CanClose;

        // Свойство формы, указывающее были ли изменены данные, после открытия или создания.
        public bool Changed;

        // Структура для хранения интерфейсов пользовательских переменных.
        public struct UVariable
        {
            public string Name;
            public double Value;
            public TextBox NameInput;
            public NumericUpDown ValueInput;
            public Label Remove;
            public object AddButton;
        };

        // Массив интерфейсов пользовательских переменных.
        public UVariable[] GUserVariable = new UVariable[0];

        // Структура для хранения  интерфейсов строк статистики.
        public struct SLine
        {
            public TextBox Prefix;
            public TextBox Calc;
            public TextBox Postfix;
            public Label Remove;
        }

        // Массив интерфейсов строк статистики.
        public SLine[] GStatisticsLines = new SLine[0];

        // Структура интерфейса ответа на вопрос.
        public struct Answer
        {
            public TextBox AnswerIntput;
            public TextBox[] AnswerArguments;
            public Label Remove;
            public Label AddAnswerArgumets;
        };

        QuizRunner.Editor.Editor GEditor = new QuizRunner.Editor.Editor();

        public IfrCreator()
        {
            InitializeComponent();
        }

        private readonly SaveFileDialog GIsfdSaveDialog = new SaveFileDialog
        {
            Title = "Сохранить",
            FileName = "Test.qrtf",
            Filter = "QuizRunner Test File(*.qrtf)|*.qrtf|Все файлы|*.*"
        };

        private readonly OpenFileDialog GIofdOpenDialog = new OpenFileDialog
        {
            Title = "Открыть",
            FileName = "Test.qrtf",
            Filter = "QuizRunner Test File(*.qrtf)|*.qrtf|Все файлы|*.*"
        };

        private void IfrCreator_Load(object sender, EventArgs e)
        {
            if (Environment.OSVersion.Platform==PlatformID.Unix)
            {
                this.BackColor = Color.White;
            }

            // ТулТип для подсказок.
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

            // Кнопка сохранения в файл.
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
                Top = IpbSave.Top + IpbSave.Height + 15
            };
            IpbOpen.MouseEnter += MenuButtons_MouseEnter;
            IpbOpen.MouseLeave += MenuButtons_MouseLeave;
            IpbOpen.Click += IpbOpen_Click;
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

            /// Пользовательские переменные.
            #region
            // Панель переменных.
            var IpnUserVariable = new Panel
            {
                AutoScroll = true,
                Width = this.ClientSize.Width / 9 * 2,
                Height = this.ClientSize.Height,
                BackColor = Color.FromArgb(18, 136, 235),
                Left = this.ClientSize.Width - this.ClientSize.Width / 9 * 2,
                Parent = this
            };

            // Заголовок панели.
            var IlbUserVariableHeader = new Label
            {
                AutoSize = true,
                Text = "Переменные",
                Font = new Font("Verdana", 8, FontStyle.Bold),
                ForeColor = Color.White,
                Top = 5,
                Parent = IpnUserVariable
            };
            IlbUserVariableHeader.Left = IpnUserVariable.Width / 2 - IlbUserVariableHeader.Width / 2;

            // Кнопка добавления переменной.
            var IbtAddVariable = new Button
            {
                Text = "Добавить",
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(18, 136, 235),
                Cursor = System.Windows.Forms.Cursors.Hand,
                AutoSize = true,
                Parent = IpnUserVariable
            };
            IbtAddVariable.Left = IpnUserVariable.Width / 2 - IbtAddVariable.Width / 2;
            IbtAddVariable.Top = IlbUserVariableHeader.Top + IlbUserVariableHeader.Height + 42;
            IbtAddVariable.Click += IbtAddVariable_Click;
            #endregion

            /// Редактор тестов.
            #region
            // Контроллер вкладок
            var ItcQuizEditor = new TabControl
            {
                Left = IpnMenu.Width + 2,
                Width = this.ClientSize.Width - IpnMenu.Width - IpnUserVariable.Width - 2,
                Height = this.ClientSize.Height - 1,
                Parent = this
            };
            ItcQuizEditor.BringToFront();

            /// Главная страница.
            #region
            //  Страница для главной.
            var ItpHome = new TabPage
            {
                Text = "Главная",
                Parent = ItcQuizEditor
            };

            // Лейбл названия теста.
            var IlbTestName = new Label
            {
                AutoSize = true,
                Text = "Название теста",
                Font = new Font("Verdana", 25, FontStyle.Bold),
                ForeColor = Color.FromArgb(18, 136, 235),
                Top = 50,
                Parent = ItpHome
            };
            IlbTestName.Left = ItpHome.ClientSize.Width / 2 - IlbTestName.Width / 2;

            // Инпут названия теста.
            var ItbTestName = new TextBox
            {
                Width = ItpHome.ClientSize.Width - 40,
                Height = 25,
                Font = new Font("Verdana", 20, FontStyle.Bold),
                Top = IlbTestName.Top + IlbTestName.Height + 20,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = System.Windows.Forms.HorizontalAlignment.Center,
                Parent = ItpHome
            };
            ItbTestName.Left = ItpHome.ClientSize.Width / 2 - ItbTestName.Width / 2;
            ItbTestName.TextChanged += UnsavedText_TextChanged;

            // Лейбл описания теста.
            var IlbTestDescription = new Label
            {
                AutoSize = true,
                Text = "Описание теста",
                Font = new Font("Verdana", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(18, 136, 235),
                Top = ItbTestName.Top + ItbTestName.Height + 20,
                Parent = ItpHome
            };
            IlbTestDescription.Left = ItpHome.ClientSize.Width / 2 - IlbTestDescription.Width / 2;

            // Инпут описания теста.
            var IrtbTestDescription = new RichTextBox
            {
                Left = ItbTestName.Left,
                Top = IlbTestDescription.Top + IlbTestDescription.Height + 20,
                Width = ItbTestName.Width,
                Font = new Font("Verdana", 15, FontStyle.Bold),
                BorderStyle = BorderStyle.Fixed3D,
                Parent = ItpHome
            };
            IrtbTestDescription.Height = ItpHome.ClientSize.Height - IrtbTestDescription.Top - 20;
            IrtbTestDescription.TextChanged += UnsavedText_TextChanged;

            // Кнопка добавления страницы.
            var IlbAddTabPage = new Label
            {
                AutoSize = true,
                Text = "+",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.Green,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = ItpHome
            };
            IlbAddTabPage.MouseEnter += IlbAddTabPage_MouseEnter;
            IlbAddTabPage.MouseLeave += IlbAddTabPage_MouseLeave;
            IlbAddTabPage.Click += IlbAddTabPage_Click;
            IttCreatorToolTip.SetToolTip(IlbAddTabPage, "Добавить вопрос");
            #endregion 

            /// Страница статистики.
            #region
            // Страница для статистики.
            var ItpStatistics = new TabPage
            {
                Text = "Статистика",
                Parent = ItcQuizEditor
            };

            // Подсказка по использованию.
            var IlbHint = new Label
            {
                AutoSize = false,
                Top = 20,
                Width = ItpStatistics.ClientSize.Width - 40,
                Height = 80,
                Text = "Создайте строку для отображения статистики.\n" +
                    "Используйте: [имя переменной], что бы использовать переменную в расчётах.",
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(18, 136, 235),
                Parent = ItpStatistics
            };
            IlbHint.Left = ItpStatistics.ClientSize.Width / 2 - IlbHint.Width / 2;

            // Груп бокс для хранения строк статистики.
            var IgbStatisticsLines = new GroupBox
            {
                Text = "Cтроки статистики",
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 10, FontStyle.Bold),
                Width = ItpStatistics.ClientSize.Width - 40,
                Top = IlbHint.Top + IlbHint.Height + 30,
                Parent = ItpStatistics
            };
            IgbStatisticsLines.Left = ItpStatistics.ClientSize.Width / 2
                - IgbStatisticsLines.Width / 2;
            IgbStatisticsLines.Height = ItpStatistics.ClientSize.Height
                - IgbStatisticsLines.Top - 10;

            // Кнопка добавления строки статистики.
            var IlbAddStatisticsLine = new Label
            {
                AutoSize = true,
                Text = "+",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.Green,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = ItpStatistics,
                Left= IgbStatisticsLines.Left,
                Tag= new Panel
                {
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None,
                    Left = 15,
                    Top = 15,
                    Width = IgbStatisticsLines.ClientSize.Width - 30,
                    Height = IgbStatisticsLines.ClientSize.Height - 30,
                    Parent = IgbStatisticsLines
                }
            };
            IlbAddStatisticsLine.Top = IgbStatisticsLines.Top - IlbAddStatisticsLine.Height;
            IlbAddStatisticsLine.MouseEnter += IlbAddTabPage_MouseEnter;
            IlbAddStatisticsLine.MouseLeave += IlbAddTabPage_MouseLeave;
            IlbAddStatisticsLine.Click += IlbAddStatisticsLine_Click;
            IttCreatorToolTip.SetToolTip(IlbAddStatisticsLine, "Добавить строку статистики");

            // Подсказка форматирования: Префикс.
            var IlbStatisticPrefix = new Label
            {
                AutoSize = false,
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Text = "Префикс",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Height = 15,
                Width = new Panel
                {
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None,
                    Left = 15,
                    Top = 15,
                    Width = IgbStatisticsLines.ClientSize.Width - 30,
                    Height = IgbStatisticsLines.ClientSize.Height - 30,
                    Parent = IgbStatisticsLines
                }.ClientSize.Width / 10 * 2,
                Left = new Panel
                {
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None,
                    Left = 15,
                    Top = 15,
                    Width = IgbStatisticsLines.ClientSize.Width - 30,
                    Height = IgbStatisticsLines.ClientSize.Height - 30,
                    Parent = IgbStatisticsLines
                }.ClientSize.Width / 20 * 1,
                Top = 20,
                Parent = (Control)IlbAddStatisticsLine.Tag
            };

            // Подсказка форматирования: Вычисления.
            var IlbStatisticСalculations = new Label
            {
                AutoSize = false,
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Text = "Расчёты",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Height = 15,
                Width = new Panel
                {
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None,
                    Left = 15,
                    Top = 15,
                    Width = IgbStatisticsLines.ClientSize.Width - 30,
                    Height = IgbStatisticsLines.ClientSize.Height - 30,
                    Parent = IgbStatisticsLines
                }.ClientSize.Width / 10 * 2,
                Left = new Panel
                {
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None,
                    Left = 15,
                    Top = 15,
                    Width = IgbStatisticsLines.ClientSize.Width - 30,
                    Height = IgbStatisticsLines.ClientSize.Height - 30,
                    Parent = IgbStatisticsLines
                }.ClientSize.Width / 20 * 6,
                Top = 20,
                Parent = (Control)IlbAddStatisticsLine.Tag
            };

            // Подсказка форматирования: Постфикс.
            var IlbStatisticPostfix = new Label
            {
                AutoSize = false,
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Text = "Постфикс",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Height = 15,
                Width = new Panel
                {
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None,
                    Left = 15,
                    Top = 15,
                    Width = IgbStatisticsLines.ClientSize.Width - 30,
                    Height = IgbStatisticsLines.ClientSize.Height - 30,
                    Parent = IgbStatisticsLines
                }.ClientSize.Width / 10 * 2,
                Left = new Panel
                {
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None,
                    Left = 15,
                    Top = 15,
                    Width = IgbStatisticsLines.ClientSize.Width - 30,
                    Height = IgbStatisticsLines.ClientSize.Height - 30,
                    Parent = IgbStatisticsLines
                }.ClientSize.Width / 20 * 11,
                Top = 20,
                Parent = (Control)IlbAddStatisticsLine.Tag
            };
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
            if (Changed)
            {
                if (MessageBox.Show("Есть не сохранённые данные, при продолжении " +
                        "действия они будут потеряны.\nЖелаете продолжить?", "Выход", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation) == DialogResult.Yes)
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

        private void IpbBack_Click(object sender, EventArgs e)
        {
            if (Changed)
            {
                if (MessageBox.Show("Есть не сохранённые данные, при продолжении " +
                        "действия они будут потеряны.\nЖелаете продолжить?", "Выход в меню", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation) == DialogResult.Yes)
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

        private void IpbSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void IpbOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void IbtAddVariable_Click(object sender, EventArgs e)
        {
            var IbtAddVariable = (Button)sender;
            AddVariable(IbtAddVariable.Parent, sender);
        }

        // Установка статуса "Изменено" при изменении одного из текстовых полей.
        private void UnsavedText_TextChanged(object sender, EventArgs e)
        {
            Changed = true;
        }

        private void IlbAddTabPage_MouseEnter(object sender,EventArgs e)
        {
            ((Label)sender).ForeColor = Color.White;
            ((Label)sender).BackColor = Color.Green;
        }

        private void IlbAddTabPage_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Green;
            ((Label)sender).BackColor = Color.Transparent;
        }

        private void IlbAddTabPage_Click(object sender, EventArgs e)
        {
            CreateNewQuestionPage(((TabControl)this.Controls[0]).SelectedIndex + 1);
        }

        private void IlbAddStatisticsLine_Click(object sender,EventArgs e)
        {
            AddStatisticLine(((Label)sender).Tag);
        }
        #endregion

        /// <summary>
        /// Сохраняет тест в файл.
        /// </summary>
        private void Save()
        {
            if (GIsfdSaveDialog.ShowDialog()==DialogResult.OK)
            {
                //Тут должна быть функция сохранения.
                Changed = false;
            }
        }

        /// <summary>
        /// Открыввает тест из файла.
        /// </summary>
        private void Open()
        {
            if (Changed)
            {
                if (MessageBox.Show("Есть не сохранённые данные, при продолжении " +
                    "действия они будут потеряны.\nЖелаете продолжить?", "Открыть",MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation)==DialogResult.Yes)
                {
                    if (GIofdOpenDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Тут должна быть функция открытия.
                        Changed = false;
                    }
                }
            }
            else
            {
                if (GIofdOpenDialog.ShowDialog() == DialogResult.OK)
                {
                    //Тут должна быть функция открытия.
                    Changed = false;
                }
            }
        }

        /// <summary>
        /// Создаёт пользовательскую переменную.
        /// </summary>
        /// <param name="parent">Обьект на котором будет создана переменная.</param>
        /// <param name="sender">Обьект посылающий запрос.</param>
        private void AddVariable(object parent, object sender)
        {
            var TIttCreatorToolTip = new ToolTip();

            Array.Resize<UVariable>(ref GUserVariable, GUserVariable.Length + 1);
            Panel TIpnParentPanel = (Panel)parent;
            int TNow = GUserVariable.Length - 1;
            var TItbName = new TextBox
            {
                Text = "Имя " + (TNow).ToString(),
                Width = TIpnParentPanel.Width / 10 * 3,
                Left = TIpnParentPanel.Width / 10 * 1,
                Parent = TIpnParentPanel
            };
            TItbName.Tag = TItbName.Text;
            if (GUserVariable.Length==1)
            {
                TItbName.Top = 20;
            }
            else
            {
                TItbName.Top = GUserVariable[TNow - 1].NameInput.Top 
                    + GUserVariable[TNow - 1].NameInput.Height + 20;
            }
            TItbName.TextChanged += (s, e) =>
            {
                Changed = true;
                if (TItbName.Text != (TItbName.Tag.ToString()))
                {
                    // Функция получения значения по имени переменной (Name.Tag.ToString())
                    // Функция удаления переменной.
                    // Функция создания переменной.
                }
            };
            TIttCreatorToolTip.SetToolTip(TItbName, "Имя");
            GUserVariable[TNow].NameInput = TItbName;
            GUserVariable[TNow].Name = TItbName.Text;

            var TInudValue = new NumericUpDown
            {
                Width = TIpnParentPanel.Width / 10 * 2,
                Left = TIpnParentPanel.Width / 10 * 5,
                Top = TItbName.Top,
                ThousandsSeparator = true,
                Minimum = Convert.ToDecimal(Decimal.MinValue),
                DecimalPlaces = 1,
                Maximum = Convert.ToDecimal(Decimal.MaxValue),
                Tag = TNow,
                Parent = TIpnParentPanel
            };
            TInudValue.ValueChanged += (s, e) =>
            {
                Changed = true;
                // var Value = (NumericUpDown)sender; Что бы не мешался, пока нет функции.
                // Функция изменения значения переменной по имени 
                //(UserVariable[Convert.ToInt32(Value.Tag].Name)).
            };
            TIttCreatorToolTip.SetToolTip(TInudValue, "Значение");
            GUserVariable[TNow].ValueInput = TInudValue;
            GUserVariable[TNow].Value = Convert.ToDouble(TInudValue.Value);

            var TIlbRemoveV = new Label
            {
                AutoSize = false,
                Width = TIpnParentPanel.Width / 10 * 1,
                Height = TItbName.Height,
                Left = TIpnParentPanel.Width / 10 * 8,
                Top = TItbName.Top,
                ForeColor = Color.Red,
                Font = new Font("Verdana", 12, FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = TIpnParentPanel
            };
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                TIlbRemoveV.Text = "X";
            }
            else
            {
                TIlbRemoveV.Text = "❌";
            }
            TIlbRemoveV.Tag = TNow;
            TIlbRemoveV.Click += (s, e) =>
            {
                Changed = true;
                RemoveVariable((int)TIlbRemoveV.Tag);
            };
            TIttCreatorToolTip.SetToolTip(TIlbRemoveV, "Удалить переменную");
            GUserVariable[TNow].Remove = TIlbRemoveV;

            var TIbtAddButton = (Button)sender;
            TIbtAddButton.Top = TItbName.Top + TItbName.Height + 20;
            GUserVariable[TNow].AddButton = TIbtAddButton;

            // Функция проверки имени переменной.
            // Функция добавления переменной.
        }

        /// <summary>
        /// Удаляет пользовательскую переменную по индексу в массиве.
        /// </summary>
        /// <param name="index">Индекс.</param>
        private void RemoveVariable(int index)
        {
            // Функция удаления переменной.

            GUserVariable[index].NameInput.Dispose();
            GUserVariable[index].ValueInput.Dispose();
            GUserVariable[index].Remove.Dispose();

            if (index != GUserVariable.Length - 1)
            {
                for (var i = index; i < GUserVariable.Length - 1; i++)
                {
                    GUserVariable[i] = GUserVariable[i + 1];
                    GUserVariable[i].Remove.Tag = i;
                    GUserVariable[i].ValueInput.Tag = i;
                    if (i != 0)
                    {
                        GUserVariable[i].NameInput.Top = GUserVariable[i - 1].NameInput.Top +
                            GUserVariable[i - 1].NameInput.Height + 20;
                        GUserVariable[i].ValueInput.Top = GUserVariable[i - 1].NameInput.Top +
                            GUserVariable[i - 1].NameInput.Height + 20;
                        GUserVariable[i].Remove.Top = GUserVariable[i - 1].NameInput.Top +
                            GUserVariable[i - 1].NameInput.Height + 20;
                    }
                    else
                    {
                        GUserVariable[i].NameInput.Top = 20;
                        GUserVariable[i].ValueInput.Top = 20;
                        GUserVariable[i].Remove.Top = 20;
                    }
                }


                Array.Resize<UVariable>(ref GUserVariable, GUserVariable.Length - 1);
                var TIbtAddButton = (Button)GUserVariable[GUserVariable.Length - 1].AddButton;
                TIbtAddButton.Top = GUserVariable[GUserVariable.Length - 1].NameInput.Top +
                    GUserVariable[GUserVariable.Length - 1].NameInput.Height + 20;
            }
            else
            {
                if (GUserVariable.Length == 1)
                {
                    Array.Resize<UVariable>(ref GUserVariable, GUserVariable.Length - 1);
                }
                else
                {
                    Array.Resize<UVariable>(ref GUserVariable, GUserVariable.Length - 1);
                    var TIbtAddButton = (Button)GUserVariable[GUserVariable.Length - 1].AddButton;
                    TIbtAddButton.Top = GUserVariable[GUserVariable.Length - 1].NameInput.Top +
                        GUserVariable[GUserVariable.Length - 1].NameInput.Height + 20;
                }
            }
        }

        /// <summary>
        /// Cоздаёт строку статистики на указанной панели.
        /// </summary>
        /// <param name="sender">Панель</param>
        private void AddStatisticLine(object sender)
        {
            var TIttStatisticLine = new ToolTip();
            Array.Resize<SLine>(ref GStatisticsLines, GStatisticsLines.Length + 1);
            int TNow = GStatisticsLines.Length - 1;
            var TIpnParent = (Panel)sender;

            var TItbPrefix = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 1,
                Parent = TIpnParent
            };
            if (GStatisticsLines.Length==1)
            {
                TItbPrefix.Top = 40;
            }
            else
            {
                TItbPrefix.Top = GStatisticsLines[TNow - 1].Prefix.Top + 30;
            }
            TItbPrefix.TextChanged += UnsavedText_TextChanged;
            TIttStatisticLine.SetToolTip(TItbPrefix, "Префикс");
            GStatisticsLines[TNow].Prefix = TItbPrefix;

            var TItbCalc = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 6,
                Top = TItbPrefix.Top,
                Parent = TIpnParent
            };
            TItbCalc.TextChanged += UnsavedText_TextChanged;
            TIttStatisticLine.SetToolTip(TItbCalc, "Расчёты");
            GStatisticsLines[TNow].Calc = TItbCalc;

            var TItbPostfix = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 11,
                Top = TItbPrefix.Top,
                Parent = TIpnParent
            };
            TItbPostfix.TextChanged += UnsavedText_TextChanged;
            TIttStatisticLine.SetToolTip(TItbPostfix, "Постфикс");
            GStatisticsLines[TNow].Postfix = TItbPostfix;

            var TIlbRemoveSL = new Label
            {
                AutoSize = false,
                Width = TIpnParent.Width / 20 * 3,
                Height = TItbPrefix.Height,
                Left = TIpnParent.Width / 10 * 8,
                Top = TItbPrefix.Top,
                ForeColor = Color.Red,
                Font = new Font("Verdana", 12, FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = TIpnParent
            };
            if (Environment.OSVersion.Platform  == PlatformID.Unix)
            {
                TIlbRemoveSL.Text = "X";
            }
            else
            {
                TIlbRemoveSL.Text = "❌";
            }
            TIlbRemoveSL.Tag = TNow;
            TIlbRemoveSL.Click += (s, e) =>
            {
                Changed = true;
                RemoveStatisticLine(Convert.ToInt32(((Label)TIlbRemoveSL).Tag));
            };
            TIttStatisticLine.SetToolTip(TIlbRemoveSL, "Удалить строку");
            GStatisticsLines[TNow].Remove = TIlbRemoveSL;
        }

        /// <summary>
        /// Удаляет строку статистики по указанному индексу в массиве.
        /// </summary>
        /// <param name="index">Индекс.</param>
        private void RemoveStatisticLine(int index)
        {
            GStatisticsLines[index].Prefix.Dispose();
            GStatisticsLines[index].Calc.Dispose();
            GStatisticsLines[index].Postfix.Dispose();
            GStatisticsLines[index].Remove.Dispose();

            for (var i = index; i < GStatisticsLines.Length - 1; i++)
            {
                GStatisticsLines[i] = GStatisticsLines[i + 1];
                GStatisticsLines[i].Remove.Tag = i;
                
                if (i!=0)
                {
                    GStatisticsLines[i].Prefix.Top = GStatisticsLines[i - 1].Prefix.Top + 30;
                    GStatisticsLines[i].Calc.Top = GStatisticsLines[i].Prefix.Top;
                    GStatisticsLines[i].Postfix.Top = GStatisticsLines[i].Prefix.Top;
                    GStatisticsLines[i].Remove.Top = GStatisticsLines[i].Prefix.Top;
                }
                else
                {
                    GStatisticsLines[i].Prefix.Top = 40;
                    GStatisticsLines[i].Calc.Top = GStatisticsLines[i].Prefix.Top;
                    GStatisticsLines[i].Postfix.Top = GStatisticsLines[i].Prefix.Top;
                    GStatisticsLines[i].Remove.Top = GStatisticsLines[i].Prefix.Top;
                }
            }

            Array.Resize<SLine>(ref GStatisticsLines, GStatisticsLines.Length - 1);
        }


        /// <summary>
        /// Добавляет страницу теста на место по указанному индексу.
        /// </summary>
        /// <param name="index">Индекс</param>
        private void CreateNewQuestionPage(int index)
        {
            //ТулТип
            var TIttToolTip = new ToolTip();

            // Cтраница.
            var TItcTabController = (TabControl)this.Controls[0];
            var TItpQuestionPage = new TabPage
            {
                Width = TItcTabController.ClientSize.Width,
                Height = TItcTabController.ClientSize.Height,
                Tag = new Answer[0],
            };

            /// Остовные графические элементы страницы.
            #region
            // Инпут вопроса.
            var TIrtbQuestion = new RichTextBox
            {
                Width = TItpQuestionPage.Width - 40,
                Top = 50,
                Height = 200,
                Parent=TItpQuestionPage
            };
            TIrtbQuestion.Left = TItpQuestionPage.Width / 2
                - TIrtbQuestion.Width / 2;
            TIrtbQuestion.TextChanged += UnsavedText_TextChanged;
            // Лейбл вопроса.
            var ITlbQuestion = new Label
            {
                AutoSize = true,
                Text = "Вопрос",
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 15, FontStyle.Bold),
                Top = 20,
                Parent = TItpQuestionPage
            };
            ITlbQuestion.Left = TItpQuestionPage.Width / 2 - ITlbQuestion.Width / 2;

            // ГрупБокс для хранения типов ответов.
            var TIgbAnswerType = new GroupBox
            {
                Text="Тип ответов",
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Width = TIrtbQuestion.Width,
                Height = 80,
                Left = TIrtbQuestion.Left,
                Top = TIrtbQuestion.Top + TIrtbQuestion.Height + 20,
                Parent = TItpQuestionPage
            };

            // Тип ответа РадиоБатн.
            var TIrbRadioButtonType = new RadioButton
            {
                Text = "Один ответ",
                Font = new Font("Verdana", 10, FontStyle.Bold),
                AutoSize = true,
                Checked=true,
                Parent = TIgbAnswerType
            };
            TIrbRadioButtonType.Left = TIgbAnswerType.ClientSize.Width / 2 - TIrbRadioButtonType.Width - 40;
            TIrbRadioButtonType.Top = TIgbAnswerType.Height / 2 - TIrbRadioButtonType.Height / 2;
            TIrbRadioButtonType.CheckedChanged += (s, e) =>
            {
                Changed = true;
            };
            TIttToolTip.SetToolTip(TIrbRadioButtonType,
                "Позволяет выбрать только один ответ из предложенных");

            // Тип ответа Чекбокс.
            var TIrbCheckBoxType = new RadioButton
            {
                Text = "Несколько ответов",
                Font = new Font("Verdana", 10, FontStyle.Bold),
                Checked = false,
                AutoSize = true,
                Top=TIrbRadioButtonType.Top,
                Parent = TIgbAnswerType
            };
            TIrbCheckBoxType.Left = TIgbAnswerType.ClientSize.Width / 2 + 10;
            TIrbCheckBoxType.CheckedChanged += (s, e) =>
            {
                Changed = true;
            };
            TIttToolTip.SetToolTip(TIrbCheckBoxType,
                "Позволяет выбрать несколько ответов из предложенных");

            // ГрупБокс для хранения ответов.
            var TIgbAnswerBox = new GroupBox
            {
                Text = "Ответы",
                ForeColor = Color.FromArgb(18, 136, 235),
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Width = TIrtbQuestion.Width,
                Left = TIrtbQuestion.Left,
                Top = TIgbAnswerType.Top + TIgbAnswerType.Height + 30,
                Parent = TItpQuestionPage
            };
            TIgbAnswerBox.Height = TItpQuestionPage.Height - TIgbAnswerBox.Top - 20;

            // Панель для скролинга ответов.
            var TIpnScrollAnswerBox = new Panel
            {
                AutoScroll = true,
                Left = 15,
                Top = 15,
                Width = TIgbAnswerBox.Width - 30,
                Height = TIgbAnswerBox.Height - 30,
                Parent = TIgbAnswerBox
            };

            // Кнопка добавления варианта ответа.
            var TIlbAddAnswer = new Label
            {
                AutoSize = true,
                Text = "+",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.Green,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = TItpQuestionPage,
                Left = TIgbAnswerBox.Left,
                Tag = TIpnScrollAnswerBox
            };
            TIlbAddAnswer.Top = TIgbAnswerBox.Top - TIlbAddAnswer.Height;
            TIlbAddAnswer.MouseEnter += IlbAddTabPage_MouseEnter;
            TIlbAddAnswer.MouseLeave += IlbAddTabPage_MouseLeave;
            TIlbAddAnswer.Click += (s, e) =>
            {
                CreateNewAnswer(TIpnScrollAnswerBox);
            };
            TIttToolTip.SetToolTip(TIlbAddAnswer, "Добавить ответ на вопрос");

            // Кнопка создания новой вкладки.
            var TIlbAddNewTabPage = new Label
            {
                AutoSize = true,
                Text = "+",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.Green,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = TItpQuestionPage
            };
            TIlbAddNewTabPage.MouseEnter += IlbAddTabPage_MouseEnter;
            TIlbAddNewTabPage.MouseLeave += IlbAddTabPage_MouseLeave;
            TIlbAddNewTabPage.Click += IlbAddTabPage_Click;
            TIttToolTip.SetToolTip(TIlbAddNewTabPage, "Добавить вопрос");

            // Кнопка удаления текущей вкладки.
            var TIlbRemoveThisTabPage = new Label
            {
                AutoSize = true,
                Text = "x",
                Font = new Font("Verdana", 15, FontStyle.Bold),
                ForeColor = Color.Red,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Left = TIlbAddNewTabPage.Width,
                Parent = TItpQuestionPage
            };
            TIlbRemoveThisTabPage.MouseEnter += (s, e) =>
            {
                TIlbRemoveThisTabPage.BackColor = Color.Red;
                TIlbRemoveThisTabPage.ForeColor = Color.White;
            };
            TIlbRemoveThisTabPage.MouseLeave += (s, e) =>
            {
                TIlbRemoveThisTabPage.BackColor = Color.Transparent;
                TIlbRemoveThisTabPage.ForeColor = Color.Red;
            };
            TIlbRemoveThisTabPage.Click += (s, e) =>
            {
                if (MessageBox.Show("Вы действительно хотите удалить вопрос?","Удалить вопрос?",
                    MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    TItcTabController.TabPages.Remove(TItcTabController.SelectedTab);
                    TItcTabController.SelectedIndex = 0;
                    RenumberTabPages(TItcTabController, 1, TItcTabController.TabPages.Count - 2);
                }
            };
            TIttToolTip.SetToolTip(TIlbRemoveThisTabPage, "Удалить этот вопрос");
            #endregion

            /// Перемещение вкладки на нужную позицию.
            #region
            TItcTabController.TabPages.Insert(index, TItpQuestionPage);
            RenumberTabPages(TItcTabController, 1, TItcTabController.TabPages.Count - 2);
            #endregion

        }

        /// <summary>
        /// Устанавливает текст вкладок в соответствии с их положением.
        /// </summary>
        /// <param name="tabcontroller">Контейнер вкладок</param>
        /// <param name="start">Номер начала нумерации</param>
        /// <param name="end">Номер окончания нумерации</param>
        private void RenumberTabPages(TabControl tabcontroller, int start, int end)
        {
            for (var i = start; i <= end; i++)
            {
                tabcontroller.TabPages[i].Text = i.ToString();
            }
        }

        /// <summary>
        /// Создаёт строку ответа на указанной панели.
        /// </summary>
        /// <param name="sender">Панель</param>
        private void CreateNewAnswer(object sender)
        {
            //ТулТип
            var TIttToolTip = new ToolTip();

            var TIpnPanel = (Panel)sender;
            var TItpTabPage = (TabPage)TIpnPanel.Parent.Parent;
            var TAnswerArray = (Answer[])TItpTabPage.Tag;
            Array.Resize<Answer>(ref TAnswerArray, TAnswerArray.Length+1);

            /// Оснавные графические элементы ответа.
            #region
            // Инпут ответа.
            var TItbAnswerInput = new TextBox
            {
                Font = new Font("Verdana", 8, FontStyle.Bold),
                Width = TIpnPanel.Width - 80,
                Left = 5,
                Parent = TIpnPanel
            };
            if (TAnswerArray.Length!=1)
            {
                if (TAnswerArray[TAnswerArray.Length-2].AnswerArguments.Length!=0)
                {
                    TItbAnswerInput.Top = TAnswerArray[TAnswerArray.Length - 2].AnswerArguments
                        [TAnswerArray[TAnswerArray.Length - 2].AnswerArguments.Length - 1].Top+30;
                }
                else
                {
                    TItbAnswerInput.Top = TAnswerArray[TAnswerArray.Length - 2].AnswerIntput.Top + 30;
                }
            }
            else
            {
                TItbAnswerInput.Top = 20;
            }
            TItbAnswerInput.TextChanged += UnsavedText_TextChanged;
            TAnswerArray[TAnswerArray.Length - 1].AnswerIntput = TItbAnswerInput;

            // Кнопка удаления ответа.
            var TIlbRemoveA = new Label
            {
                AutoSize = false,
                Width = 20,
                Height = 20,
                Left = TItbAnswerInput.Left + TItbAnswerInput.Width + 10,
                Top = TItbAnswerInput.Top,
                ForeColor = Color.Red,
                Text = "x",
                Font = new Font("Verdana", 10, FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Tag = TAnswerArray.Length - 1,
                Parent = TIpnPanel
            };
            TAnswerArray[TAnswerArray.Length - 1].Remove = TIlbRemoveA;
            TIlbRemoveA.Click += (s, e) =>
            {
                if (MessageBox.Show("Вы действительно хотите удалить ответ и все его аргументы?",
                    "Удалить ответ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    RemoveAnswer((int)TIlbRemoveA.Tag);
                }
            };
            TIttToolTip.SetToolTip(TIlbRemoveA, "Удалить ответ");

            // Кнопка добавления аргумента.
            var TIlbAddAnswerArgumets = new Label
            {
                AutoSize = false,
                Width = 20,
                Height = 20,
                Left = TIlbRemoveA.Left + TIlbRemoveA.Width + 5,
                Top = TItbAnswerInput.Top,
                ForeColor = Color.Green,
                Text = "+",
                Font = new Font("Verdana", 10, FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Cursor = System.Windows.Forms.Cursors.Hand,
                Tag = TAnswerArray.Length - 1,
                Parent = TIpnPanel
            };
            TAnswerArray[TAnswerArray.Length - 1].AddAnswerArgumets = TIlbAddAnswerArgumets;
            TIlbAddAnswerArgumets.Click += (s, e) =>
            {
                MessageBox.Show("Будьте внимательны, не нужные строки аргументов нельзя удалить." +
                    "\nЕсли строка аргумента стала вам не нужна, оставьте её пустой.",
                    "Добавить аргумент.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CreateNewAnswerArgument((int)TIlbAddAnswerArgumets.Tag);
            };
            TIttToolTip.SetToolTip(TIlbAddAnswerArgumets, "Добавить аргумент к ответу");

            // Инициализация массива аргументов.
            TAnswerArray[TAnswerArray.Length - 1].AnswerArguments = new TextBox[0];
            #endregion

            // Сохранение массива ответов в страницу.
            TItpTabPage.Tag = TAnswerArray;
        }

        /// <summary>
        /// Улаляет интерфейс ответа по заданному индексу.
        /// </summary>
        /// <param name="index">Индекс</param>
        private void RemoveAnswer(int index)
        {
            var TItcTabController = (TabControl)this.Controls[0];
            var TITabPage = TItcTabController.SelectedTab;
            var TAnswerArray = (Answer[])TITabPage.Tag;

            TAnswerArray[0].AnswerIntput.Focus();
            TAnswerArray[index].AnswerIntput.Dispose();
            TAnswerArray[index].AddAnswerArgumets.Dispose();
            TAnswerArray[index].Remove.Dispose();
            if (TAnswerArray[index].AnswerArguments.Length!=0)
            {
                for (var i = 0; i < TAnswerArray[index].AnswerArguments.Length; i++)
                {
                    TAnswerArray[index].AnswerArguments[i].Dispose();
                }
            }

            /// Расстановка элементов по координатам.
            #region
            for (var i = index ; i < TAnswerArray.Length - 1; i++)
            {
                TAnswerArray[i] = TAnswerArray[i + 1];
                TAnswerArray[i].AddAnswerArgumets.Tag = i;
                TAnswerArray[i].Remove.Tag = i;

                // Расстановка строк ответов.
                if  (i!=0)
                {
                    if (TAnswerArray[i - 1].AnswerArguments.Length != 0)
                    {
                        TAnswerArray[i].AnswerIntput.Top = TAnswerArray[i - 1].AnswerArguments
                            [TAnswerArray[i - 1].AnswerArguments.Length - 1].Top +
                            TAnswerArray[i - 1].AnswerArguments[TAnswerArray[i - 1]
                            .AnswerArguments.Length - 1].Height+ 30;
                    }
                    else
                    {
                        TAnswerArray[i].AnswerIntput.Top = TAnswerArray[i - 1].AnswerIntput.Top
                            + TAnswerArray[i - 1].AnswerIntput.Height + 30;
                    }
                    TAnswerArray[i].AddAnswerArgumets.Top = TAnswerArray[i].AnswerIntput.Top;
                    TAnswerArray[i].Remove.Top = TAnswerArray[i].AnswerIntput.Top;
                }
                else
                {
                    TAnswerArray[i].AnswerIntput.Top = 20;
                    TAnswerArray[i].AddAnswerArgumets.Top = TAnswerArray[i].AnswerIntput.Top;
                    TAnswerArray[i].Remove.Top = TAnswerArray[i].AnswerIntput.Top;
                }

                // Расстановка аргументов.
                if (TAnswerArray[i].AnswerArguments.Length!=0)
                {
                    for (var j=0; j < TAnswerArray[i].AnswerArguments.Length; j++)
                    {
                        if (j!=0)
                        {
                            TAnswerArray[i].AnswerArguments[j].Top =
                                TAnswerArray[i].AnswerArguments[j - 1].Top +
                                TAnswerArray[i].AnswerArguments[j - 1].Height + 10;
                        }
                        else
                        {
                            TAnswerArray[i].AnswerArguments[j].Top =
                                TAnswerArray[i].AnswerIntput.Top + TAnswerArray[i].AnswerIntput.Height
                                + 10;
                        }
                    }
                }

            }
            #endregion

            Array.Resize<Answer>(ref TAnswerArray, TAnswerArray.Length - 1);


            TITabPage.Tag = TAnswerArray;

        }

        /// <summary>
        /// Создаёт интерфейс для аргумента по указанному индексу ответа.
        /// </summary>
        /// <param name="index">Индекс ответа</param>
        private void CreateNewAnswerArgument(int index)
        {
            var TItcTabController = (TabControl)this.Controls[0];
            var TItpTabPage = TItcTabController.SelectedTab;
            var TAnswerArray = (Answer[])TItpTabPage.Tag;
            var TAnswer = TAnswerArray[index];
            Array.Resize<TextBox>(ref TAnswer.AnswerArguments, TAnswer.AnswerArguments.Length + 1);

            /// Оснавные графические элементы аргументов
            #region
            // Инпут аргумента.
            var TItbNewAnswerArgument = new TextBox
            {
                Width = TAnswer.AnswerIntput.Width / 2,
                Left = TAnswer.AnswerIntput.Left + TAnswer.AnswerIntput.Width / 2,
                Parent = TAnswer.AnswerIntput.Parent
            };
            if (TAnswer.AnswerArguments.Length==1)
            {
                TItbNewAnswerArgument.Top = TAnswer.AnswerIntput.Top +
                    TAnswer.AnswerIntput.Height + 10;
            }
            else
            {
                TItbNewAnswerArgument.Top = TAnswer.AnswerArguments
                    [TAnswer.AnswerArguments.Length - 2].Top + 
                    TAnswer.AnswerArguments[TAnswer.AnswerArguments.Length-2].Height+10;
            }
            TAnswer.AnswerArguments[TAnswer.AnswerArguments.Length - 1]
                = TItbNewAnswerArgument;
            #endregion

            TAnswerArray[index] = TAnswer;

            /// Расстановка элементов по координатам.
            #region
            for (var i = index + 1; i <= TAnswerArray.Length-1; i++)
            {
                if (TAnswerArray[i - 1].AnswerArguments.Length != 0)
                {
                    TAnswerArray[i].AnswerIntput.Top = TAnswerArray[i - 1].AnswerArguments
                        [TAnswerArray[i - 1].AnswerArguments.Length - 1].Top + 30;
                    TAnswerArray[i].Remove.Top = TAnswerArray[i].AnswerIntput.Top;
                    TAnswerArray[i].AddAnswerArgumets.Top = TAnswerArray[i].AnswerIntput.Top;
                    if (TAnswerArray[i].AnswerArguments.Length!=0)
                    {
                        if (TAnswerArray[i].AnswerArguments.Length==1)
                        {
                            TAnswerArray[i].AnswerArguments[0].Top=TAnswerArray[i]
                                .AnswerIntput.Top + TAnswer.AnswerIntput.Height + 10;
                        }
                        else
                        {
                            TAnswerArray[i].AnswerArguments[0].Top = TAnswerArray[i]
                                .AnswerIntput.Top + TAnswer.AnswerIntput.Height + 10;
                            for (var j = 1; j<TAnswerArray[i].AnswerArguments.Length;j++)
                            {
                                TAnswerArray[i].AnswerArguments[j].Top =
                                    TAnswerArray[i].AnswerArguments[j - 1].Top
                                    + TAnswerArray[i].AnswerArguments[j - 1].Height + 10;
                            }
                        }
                    }
                }
                else
                {
                    TAnswerArray[i].AnswerIntput.Top = TAnswerArray[i - 1].AnswerIntput.Top + 30;
                    TAnswerArray[i].Remove.Top = TAnswerArray[i].AnswerIntput.Top;
                    TAnswerArray[i].AddAnswerArgumets.Top = TAnswerArray[i].AnswerIntput.Top;
                }
            }
            #endregion

            TItpTabPage.Tag = TAnswerArray;
        }
    }
}
