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

        public struct UVariable
        {
            public string Name;
            public double Value;
            public TextBox NameInput;
            public NumericUpDown ValueInput;
            public Label Remove;
            public object AddButton;
        };

        public UVariable [] UserVariable = new UVariable[0];

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
                Width = this.ClientSize.Width / 9*2,
                Height = this.ClientSize.Height,
                BackColor = Color.FromArgb(18, 136, 235),
                Left = this.ClientSize.Width - this.ClientSize.Width / 9*2,
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
                Cursor=System.Windows.Forms.Cursors.Hand,
                AutoSize = true,
                Parent=IpnUserVariable
            };
            IbtAddVariable.Left = IpnUserVariable.Width / 2 - IbtAddVariable.Width / 2;
            IbtAddVariable.Top = IlbUserVariableHeader.Top + IlbUserVariableHeader.Height + 42;
            IbtAddVariable.Click += IbtAddVariable_Click;

            var ItcQuizEditor = new TabControl
            {
                Left = IpnMenu.Width+2,
                Width = this.ClientSize.Width - IpnMenu.Width - IpnUserVariable.Width-2,
                Height = this.ClientSize.Height-1,
                Parent = this
            };

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

        private void IpbOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void IbtAddVariable_Click(object sender, EventArgs e)
        {
            var IbtAddVariable = (Button)sender;
            AddVariable(IbtAddVariable.Parent, sender);
        }

        /// <summary>
        /// Сохраняет тест в файл.
        /// </summary>
        private void Save()
        {
            if (IsfdSaveDialog.ShowDialog()==DialogResult.OK)
            {
                //Тут должна быть функция сохранения.
            }
        }

        /// <summary>
        /// Открыввает тест из файла.
        /// </summary>
        private void Open()
        {
            if (IofdOpenDialog.ShowDialog()==DialogResult.OK)
            {
                //Тут должна быть функция открытия.
            }
        }

        /// <summary>
        /// Создаёт пользовательскую переменную.
        /// </summary>
        /// <param name="parent">Обьект на котором будет создана переменная.</param>
        /// <param name="sender">Обьект посылающий запрос.</param>
        private void AddVariable(object parent, object sender)
        {
            var IttCreatorToolTip = new ToolTip();

            Array.Resize<UVariable>(ref UserVariable, UserVariable.Length + 1);
            Panel ParentPanel = (Panel)parent;
            int Now = UserVariable.Length - 1;
            var Name = new TextBox
            {
                Text = "Имя " + (Now).ToString(),
                Width = ParentPanel.Width / 10 * 3,
                Left = ParentPanel.Width / 10 * 1,
                Parent = ParentPanel
            };
            Name.Tag = Name.Text;
            if (UserVariable.Length==1)
            {
                Name.Top = 20;
            }
            else
            {
                Name.Top = UserVariable[Now - 1].NameInput.Top 
                    + UserVariable[Now - 1].NameInput.Height + 20;
            }
            Name.TextChanged += NameInput_TextChanged;
            IttCreatorToolTip.SetToolTip(Name, "Имя");
            UserVariable[Now].NameInput = Name;
            UserVariable[Now].Name = Name.Text;

            var Value = new NumericUpDown
            {
                Width = ParentPanel.Width / 10 * 2,
                Left = ParentPanel.Width / 10 * 5,
                Top = Name.Top,
                ThousandsSeparator = true,
                Minimum = Convert.ToDecimal(Decimal.MinValue),
                DecimalPlaces = 1,
                Maximum = Convert.ToDecimal(Decimal.MaxValue),
                Tag = Now,
                Parent = ParentPanel
            };
            Value.ValueChanged += ValueInput_ValieChanged;
            IttCreatorToolTip.SetToolTip(Value, "Значение");
            UserVariable[Now].ValueInput = Value;
            UserVariable[Now].Value = Convert.ToDouble(Value.Value);

            var Remove = new Label
            {
                AutoSize = false,
                Width = ParentPanel.Width / 10 * 1,
                Height = Name.Height,
                Left = ParentPanel.Width / 10 * 8,
                Top = Name.Top,
                ForeColor = Color.Red,
                Text = "❌",
                Font = new Font("Verdana", 12, FontStyle.Bold),
                Cursor = System.Windows.Forms.Cursors.Hand,
                Parent = ParentPanel
            };
            Remove.Tag = Now;
            Remove.Click += Remove_Click;
            IttCreatorToolTip.SetToolTip(Remove, "Удалить переменную");
            UserVariable[Now].Remove = Remove;

            var AddButton = (Button)sender;
            AddButton.Top = Name.Top + Name.Height + 20;
            UserVariable[Now].AddButton = AddButton;

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
                var AddButton = (Button)UserVariable[UserVariable.Length - 1].AddButton;
                AddButton.Top = UserVariable[UserVariable.Length - 1].NameInput.Top +
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
                    var AddButton = (Button)UserVariable[UserVariable.Length - 1].AddButton;
                    AddButton.Top = UserVariable[UserVariable.Length - 1].NameInput.Top +
                        UserVariable[UserVariable.Length - 1].NameInput.Height + 20;
                }
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var Remove = (Label)sender;
            RemoveVariable((int)Remove.Tag);
        }

        private void NameInput_TextChanged(object sender, EventArgs e)
        {
            var Name = (TextBox)sender;
            if (Name.Text != Name.Tag.ToString())
            {
                // Функция получения значения по имени переменной (Name.Tag.ToString())
                // Функция удаления переменной.
                // Функция создания переменной.
            }
        }

        private void ValueInput_ValieChanged(object sender, EventArgs e)
        {
            // var Value = (NumericUpDown)sender; Что бы не мешался, пока нет функции.
            // Функция изменения значения переменной по имени (UserVariable[Convert.ToInt32(Value.Tag].Name)).
        }
    }
}
