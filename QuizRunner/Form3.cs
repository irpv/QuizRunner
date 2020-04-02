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

        //Указывает, были ли изменены данные, после открытия или создания.
        public bool Changed;

        public struct UVariable
        {
            public string Name;
            public double Value;
            public TextBox NameInput;
            public NumericUpDown ValueInput;
            public Label Remove;
            public object AddButton;
        };

        public UVariable[] UserVariable = new UVariable[0];

        public struct SLine
        {
            public TextBox Prefix;
            public TextBox Calc;
            public TextBox Postfix;
            public Label Remove;
        }

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
            var IttCreatorToolTip = new ToolTip();

            var IpnMenu = new Panel
            {
                BackColor = Color.FromArgb(18, 136, 235),
                Width = 60,
                Height = this.ClientSize.Height,
                Parent = this
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
            IpbOpen.Click += IpbOpen_Click;
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

            var IpnUserVariable = new Panel
            {
                AutoScroll = true,
                Width = this.ClientSize.Width / 9 * 2,
                Height = this.ClientSize.Height,
                BackColor = Color.FromArgb(18, 136, 235),
                Left = this.ClientSize.Width - this.ClientSize.Width / 9 * 2,
                Parent = this
            };

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

            var ItcQuizEditor = new TabControl
            {
                Left = IpnMenu.Width + 2,
                Width = this.ClientSize.Width - IpnMenu.Width - IpnUserVariable.Width - 2,
                Height = this.ClientSize.Height - 1,
                Parent = this
            };
            ItcQuizEditor.SendToBack();

            var ItpHome = new TabPage
            {
                Text = "Главная",
                Parent = ItcQuizEditor
            };

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

            var ItpStatistics = new TabPage
            {
                Text = "Статистика",
                Parent = ItcQuizEditor
            };

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

        }

        private void MenuButtons_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Width += 4;
            ((PictureBox)sender).Height += 4;
            ((PictureBox)sender).Left -= 2;
            ((PictureBox)sender).Top -= 2;
        }

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

        private void IlbAddStatisticsLine_Click(object sender,EventArgs e)
        {
            AddStatisticLine(((Label)sender).Tag);
        }

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
            TItbName.TextChanged += NameInput_TextChanged;
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
            TInudValue.ValueChanged += ValueInput_ValieChanged;
            TIttCreatorToolTip.SetToolTip(TInudValue, "Значение");
            UserVariable[TNow].ValueInput = TInudValue;
            UserVariable[TNow].Value = Convert.ToDouble(TInudValue.Value);

            var TIlbRemove = new Label
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
            TIlbRemove.Tag = TNow;
            TIlbRemove.Click += RemoveVar_Click;
            TIttCreatorToolTip.SetToolTip(TIlbRemove, "Удалить переменную");
            UserVariable[TNow].Remove = TIlbRemove;

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

        private void RemoveVar_Click(object sender, EventArgs e)
        {
            Changed = true;
            RemoveVariable((int)((Label)sender).Tag);
        }

        private void NameInput_TextChanged(object sender, EventArgs e)
        {
            Changed = true;
            if (((TextBox)sender).Text != ((TextBox)sender).Tag.ToString())
            {
                // Функция получения значения по имени переменной (Name.Tag.ToString())
                // Функция удаления переменной.
                // Функция создания переменной.
            }
        }

        private void ValueInput_ValieChanged(object sender, EventArgs e)
        {
            Changed = true;
            // var Value = (NumericUpDown)sender; Что бы не мешался, пока нет функции.
            // Функция изменения значения переменной по имени (UserVariable[Convert.ToInt32(Value.Tag].Name)).
        }

        /// <summary>
        /// Cоздаёт строку статистики на указанной панели.
        /// </summary>
        /// <param name="sender">Панель</param>
        private void AddStatisticLine(object sender)
        {
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
            StatisticsLines[TNow].Prefix = TItbPrefix;

            var TItbCalc = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 6,
                Top = TItbPrefix.Top,
                Parent = TIpnParent
            };
            TItbCalc.TextChanged += UnsavedText_TextChanged;
            StatisticsLines[TNow].Calc = TItbCalc;

            var TItbPostfix = new TextBox
            {
                Width = TIpnParent.Width / 10 * 2,
                Left = TIpnParent.Width / 20 * 11,
                Top = TItbPrefix.Top,
                Parent = TIpnParent
            };
            TItbPostfix.TextChanged += UnsavedText_TextChanged;
            StatisticsLines[TNow].Postfix = TItbPostfix;

            var TIlbRemove = new Label
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
            TIlbRemove.Click += RemoveLine_Click;
            TIlbRemove.Tag = TNow;
            StatisticsLines[TNow].Remove = TIlbRemove;
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

        void RemoveLine_Click(object sender,EventArgs e)
        {
            Changed = true;
            RemoveStatisticLine(Convert.ToInt32(((Label)sender).Tag));
        }
    }
}
