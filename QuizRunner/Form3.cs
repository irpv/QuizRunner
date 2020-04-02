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

        // Указывает, были ли изменены данные, после открытия или создания.
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
        public UVariable[] UserVariable = new UVariable[0];

        // Структура для хранения  интерфейсов строк статистики.
        public struct SLine
        {
            public TextBox Prefix;
            public TextBox Calc;
            public TextBox Postfix;
            public Label Remove;
        }

        // Массив интерфейсов строк статистики.
        public SLine[] StatisticsLines = new SLine[0];

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

        private readonly OpenFileDialog IofdOpenDialog = new OpenFileDialog
        {
            Title = "Открыть",
            FileName = "Test.qrtf",
            Filter = "QuizRunner Test File (*.qrtf)|*.qrtf"
        };

        private void IfrCreator_Load(object sender, EventArgs e)
        {
            // ТулТип для подсказок.
            var IttCreatorToolTip = new ToolTip();
            
            /// Меню.
            /// -----------------
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
            ///-----------------

            /// Пользовательские переменные.
            /// -----------------
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
            /// -----------------

            /// Редактор тестов.
            /// -----------------
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
            /// --------
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
            /// --------

            /// Страница статистики.
            /// --------
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
                TextAlign= System.Drawing.ContentAlignment.MiddleCenter,
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
                Parent = new Panel
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
                Parent = new Panel
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
                Parent = new Panel
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
            /// --------
            /// -----------------

        }

        /// События основных графических элементов.
        /// -----------------
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
        /// -----------------

        /// <summary>
        /// Сохраняет тест в файл.
        /// </summary>
        private void Save()
        {
            if (IsfdSaveDialog.ShowDialog()==DialogResult.OK)
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
                    if (IofdOpenDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Тут должна быть функция открытия.
                        Changed = false;
                    }
                }
            }
            else
            {
                if (IofdOpenDialog.ShowDialog() == DialogResult.OK)
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

            Array.Resize<UVariable>(ref UserVariable, UserVariable.Length + 1);
            Panel TIpnParentPanel = (Panel)parent;
            int TNow = UserVariable.Length - 1;
            var TItbName = new TextBox
            {
                Text = "Имя " + (TNow).ToString(),
                Width = TIpnParentPanel.Width / 10 * 3,
                Left = TIpnParentPanel.Width / 10 * 1,
                Parent = TIpnParentPanel
            };
            TItbName.Tag = TItbName.Text;
            if (UserVariable.Length==1)
            {
                TItbName.Top = 20;
            }
            else
            {
                TItbName.Top = UserVariable[TNow - 1].NameInput.Top 
                    + UserVariable[TNow - 1].NameInput.Height + 20;
            }
            TItbName.TextChanged += TItbName_TextChanged;
            TIttCreatorToolTip.SetToolTip(TItbName, "Имя");
            UserVariable[TNow].NameInput = TItbName;
            UserVariable[TNow].Name = TItbName.Text;

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
            TInudValue.ValueChanged += TInudValue_ValieChanged;
            TIttCreatorToolTip.SetToolTip(TInudValue, "Значение");
            UserVariable[TNow].ValueInput = TInudValue;
            UserVariable[TNow].Value = Convert.ToDouble(TInudValue.Value);

            var TIlbRemoveV = new Label
            {
                AutoSize = false,
                Width = TIpnParentPanel.Width / 10 * 1,
                Height = TItbName.Height,
                Left = TIpnParentPanel.Width / 10 * 8,
                Top = TItbName.Top,
                ForeColor = Color.Red,
                Text = "❌",
                Font = new Font("Verdana", 12, FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = TIpnParentPanel
            };
            TIlbRemoveV.Tag = TNow;
            TIlbRemoveV.Click += TIlbRemoveV_Click;
            TIttCreatorToolTip.SetToolTip(TIlbRemoveV, "Удалить переменную");
            UserVariable[TNow].Remove = TIlbRemoveV;

            var TIbtAddButton = (Button)sender;
            TIbtAddButton.Top = TItbName.Top + TItbName.Height + 20;
            UserVariable[TNow].AddButton = TIbtAddButton;

            // Функция проверки имени переменной.
            // Функция добавления переменной.
        }

        /// <summary>
        /// Удаляет пользовательскую переменную по индексу в массиве.
        /// </summary>
        /// <param name="Index">Индекс.</param>
        private void RemoveVariable(int Index)
        {
            // Функция удаления переменной.

            UserVariable[Index].NameInput.Dispose();
            UserVariable[Index].ValueInput.Dispose();
            UserVariable[Index].Remove.Dispose();

            if (Index != UserVariable.Length - 1)
            {
                for (var i = Index; i < UserVariable.Length - 1; i++)
                {
                    UserVariable[i] = UserVariable[i + 1];
                    UserVariable[i].Remove.Tag = i;
                    UserVariable[i].ValueInput.Tag = i;
                    if (i != 0)
                    {
                        UserVariable[i].NameInput.Top = UserVariable[i - 1].NameInput.Top +
                            UserVariable[i - 1].NameInput.Height + 20;
                        UserVariable[i].ValueInput.Top = UserVariable[i - 1].NameInput.Top +
                            UserVariable[i - 1].NameInput.Height + 20;
                        UserVariable[i].Remove.Top = UserVariable[i - 1].NameInput.Top +
                            UserVariable[i - 1].NameInput.Height + 20;
                    }
                    else
                    {
                        UserVariable[i].NameInput.Top = 20;
                        UserVariable[i].ValueInput.Top = 20;
                        UserVariable[i].Remove.Top = 20;
                    }
                }


                Array.Resize<UVariable>(ref UserVariable, UserVariable.Length - 1);
                var TIbtAddButton = (Button)UserVariable[UserVariable.Length - 1].AddButton;
                TIbtAddButton.Top = UserVariable[UserVariable.Length - 1].NameInput.Top +
                    UserVariable[UserVariable.Length - 1].NameInput.Height + 20;
            }
            else
            {
                if (UserVariable.Length == 1)
                {
                    Array.Resize<UVariable>(ref UserVariable, UserVariable.Length - 1);
                }
                else
                {
                    Array.Resize<UVariable>(ref UserVariable, UserVariable.Length - 1);
                    var TIbtAddButton = (Button)UserVariable[UserVariable.Length - 1].AddButton;
                    TIbtAddButton.Top = UserVariable[UserVariable.Length - 1].NameInput.Top +
                        UserVariable[UserVariable.Length - 1].NameInput.Height + 20;
                }
            }
        }

        /// События графических элементов пользовательскх переменных,
        /// созданных автоматически.
        /// -----------------
        private void TIlbRemoveV_Click(object sender, EventArgs e)
        {
            Changed = true;
            RemoveVariable((int)((Label)sender).Tag);
        }

        private void TItbName_TextChanged(object sender, EventArgs e)
        {
            Changed = true;
            if (((TextBox)sender).Text != ((TextBox)sender).Tag.ToString())
            {
                // Функция получения значения по имени переменной (Name.Tag.ToString())
                // Функция удаления переменной.
                // Функция создания переменной.
            }
        }

        private void TInudValue_ValieChanged(object sender, EventArgs e)
        {
            Changed = true;
            // var Value = (NumericUpDown)sender; Что бы не мешался, пока нет функции.
            // Функция изменения значения переменной по имени (UserVariable[Convert.ToInt32(Value.Tag].Name)).
        }
        /// -----------------

        /// <summary>
        /// Cоздаёт строку статистики на указанной панели.
        /// </summary>
        /// <param name="sender">Панель</param>
        private void AddStatisticLine(object sender)
        {
            var TIttStatisticLine = new ToolTip();
            Array.Resize<SLine>(ref StatisticsLines, StatisticsLines.Length + 1);
            int TNow = StatisticsLines.Length - 1;
            var TIpnParent = (Panel)sender;

            var TItbPrefix = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 1,
                Parent = TIpnParent
            };
            if (StatisticsLines.Length==1)
            {
                TItbPrefix.Top = 40;
            }
            else
            {
                TItbPrefix.Top = StatisticsLines[TNow - 1].Prefix.Top + 30;
            }
            TItbPrefix.TextChanged += UnsavedText_TextChanged;
            TIttStatisticLine.SetToolTip(TItbPrefix, "Префикс");
            StatisticsLines[TNow].Prefix = TItbPrefix;

            var TItbCalc = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 6,
                Top = TItbPrefix.Top,
                Parent = TIpnParent
            };
            TItbCalc.TextChanged += UnsavedText_TextChanged;
            TIttStatisticLine.SetToolTip(TItbCalc, "Расчёты");
            StatisticsLines[TNow].Calc = TItbCalc;

            var TItbPostfix = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 11,
                Top = TItbPrefix.Top,
                Parent = TIpnParent
            };
            TItbPostfix.TextChanged += UnsavedText_TextChanged;
            TIttStatisticLine.SetToolTip(TItbPostfix, "Постфикс");
            StatisticsLines[TNow].Postfix = TItbPostfix;

            var TIlbRemoveSL = new Label
            {
                AutoSize = false,
                Width = TIpnParent.Width / 20 * 3,
                Height = TItbPrefix.Height,
                Left = TIpnParent.Width / 10 * 8,
                Top = TItbPrefix.Top,
                ForeColor = Color.Red,
                Text = "❌",
                Font = new Font("Verdana", 12, FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = TIpnParent
            };
            TIlbRemoveSL.Click += TIlbRemoveSL_Click;
            TIlbRemoveSL.Tag = TNow;
            TIttStatisticLine.SetToolTip(TIlbRemoveSL, "Удалить строку");
            StatisticsLines[TNow].Remove = TIlbRemoveSL;
        }

        /// <summary>
        /// Удаляет строку статистики по указанному индексу в массиве.
        /// </summary>
        /// <param name="Index">Индекс.</param>
        private void RemoveStatisticLine(int Index)
        {
            StatisticsLines[Index].Prefix.Dispose();
            StatisticsLines[Index].Calc.Dispose();
            StatisticsLines[Index].Postfix.Dispose();
            StatisticsLines[Index].Remove.Dispose();

            for (var i = Index; i < StatisticsLines.Length - 1; i++)
            {
                StatisticsLines[i] = StatisticsLines[i + 1];
                StatisticsLines[i].Remove.Tag = i;
                
                if (i!=0)
                {
                    StatisticsLines[i].Prefix.Top = StatisticsLines[i - 1].Prefix.Top + 30;
                    StatisticsLines[i].Calc.Top = StatisticsLines[i].Prefix.Top;
                    StatisticsLines[i].Postfix.Top = StatisticsLines[i].Prefix.Top;
                    StatisticsLines[i].Remove.Top = StatisticsLines[i].Prefix.Top;
                }
                else
                {
                    StatisticsLines[i].Prefix.Top = 40;
                    StatisticsLines[i].Calc.Top = StatisticsLines[i].Prefix.Top;
                    StatisticsLines[i].Postfix.Top = StatisticsLines[i].Prefix.Top;
                    StatisticsLines[i].Remove.Top = StatisticsLines[i].Prefix.Top;
                }
            }

            Array.Resize<SLine>(ref StatisticsLines, StatisticsLines.Length - 1);
        }

        /// События графических элементов строк статистики,
        /// созданных автоматически.
        /// -----------------
        void TIlbRemoveSL_Click(object sender,EventArgs e)
        {
            Changed = true;
            RemoveStatisticLine(Convert.ToInt32(((Label)sender).Tag));
        }
        /// -----------------


        /// <summary>
        /// Добавляет страницу теста на место по указанному индексу.
        /// </summary>
        /// <param name="Index">Индекс</param>
        private void CreateNewQuestionPage(int Index)
        {
            // Cтраница.
            var TItcTabController = (TabControl)this.Controls[0];
            var TItpQuestionPage = new TabPage
            {
                Width = TItcTabController.ClientSize.Width,
                Height = TItcTabController.ClientSize.Height,
                Parent = TItcTabController
            };

            /// Остовные графические элементы страницы.
            /// -----------------
            // Инпут вопроса.
            var TIrtbQuestion = new RichTextBox
            {
                Width = TItpQuestionPage.Width - 40,
                Height = 250,
                Top = 50,
                Parent = TItpQuestionPage
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
                Text = "Радиобатн",
                Font = new Font("Verdana", 10, FontStyle.Bold),
                AutoSize = true,
                Checked=true,
                Parent = TIgbAnswerType
            };
            TIrbRadioButtonType.Left = TIgbAnswerType.ClientSize.Width / 2 - TIrbRadioButtonType.Width - 25;
            TIrbRadioButtonType.Top = TIgbAnswerType.Height / 2 - TIrbRadioButtonType.Height / 2;
            TIrbRadioButtonType.CheckedChanged += (s, e) =>
            {
                Changed = true;
            };

            // Тип ответа Чекбокс.
            var TIrbCheckBoxType = new RadioButton
            {
                Text = "Чекбокс",
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
                    RenumberTabPages(TItcTabController, 1, TItcTabController.TabPages.Count - 2);
                }
            };
            /// -----------------

            /// Перемещение вкладки на нужную позицию.
            /// -----------------
            if (TItcTabController.TabPages.Count != 3)
            {
                for (var i = TItcTabController.TabPages.Count - 1; i > Index; i--)
                {
                    TItcTabController.TabPages[i] = TItcTabController.TabPages[i - 1];
                }
                TItcTabController.TabPages[Index] = TItpQuestionPage;
            }
            else
            {
                TItcTabController.TabPages[2] = TItcTabController.TabPages[1];
                TItcTabController.TabPages[1] = TItpQuestionPage;
            }
            RenumberTabPages(TItcTabController, 1, TItcTabController.TabPages.Count - 2);
            /// -----------------

        }
        
        /// <summary>
        /// Устанавливает текст вкладок в соответствии с их положением.
        /// </summary>
        /// <param name="TabController">Контейнер вкладок</param>
        /// <param name="Start">Номер начала нумерации</param>
        /// <param name="End">Номер окончания нумерации</param>
        private void RenumberTabPages(TabControl TabController, int Start, int End)
        {
            for (var i = Start; i <= End; i++)
            {
                TabController.TabPages[i].Text = i.ToString();
            }
        }
    }
}
