using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>
    /// Окно списка долгов: добавить, срок погашения и погасить.
    /// </summary>
    public class DebtsForm : Form
    {
        private readonly FinanceManager _manager;

        private DataGridView grid;
        private TextBox txtPerson;
        private TextBox txtAmount;
        private ComboBox cmbCurrency;
        private ComboBox cmbDirection;
        private DateTimePicker dtpDue;
        private CheckBox chkHasDue;
        private Button btnAdd;
        private Button btnPay;
        private Button btnClose;
        private Panel panelAdd;
        private Label lblTitle;

        public DebtsForm(FinanceManager manager)
        {
            _manager = manager;
            BuildUi();
            Theme.ApplyTo(this);
            // Акцентные кнопки после ApplyTo
            UiHelper.StyleFlatButton(btnAdd, Theme.AccentBlue, Color.White);
            btnAdd.TextAlign = ContentAlignment.MiddleCenter;
            UiHelper.StyleFlatButton(btnPay, Theme.AccentGreen, Color.White);
            btnPay.TextAlign = ContentAlignment.MiddleCenter;
            RefreshGrid();
        }

        private void BuildUi()
        {
            Text = "Мои долги";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(840, 640);
            Font = UiHelper.BodyFont;

            lblTitle = new Label
            {
                Text = "📒  Долги Юлии",
                Font = UiHelper.TitleFont,
                Location = new Point(20, 16),
                AutoSize = true
            };

            grid = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(800, 290),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = UiHelper.BodyFont,
                RowTemplate = { Height = 34 },
                EnableHeadersVisualStyles = false
            };
            grid.ColumnHeadersHeight = 40;
            grid.CellFormatting += Grid_CellFormatting;

            panelAdd = new Panel
            {
                Location = new Point(20, 365),
                Size = new Size(800, 175)
            };

            var lblNew = new Label
            {
                Text = "Новый долг",
                Font = UiHelper.HeaderFont,
                Location = new Point(12, 5),
                AutoSize = true,
                Name = "lblNew"
            };

            // Идеальное выравнивание полей по всей ширине в 800 пикселей
            var tipPerson = new Label { Text = "Имя", Location = new Point(12, 40), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = UiHelper.TextMuted };
            txtPerson = new TextBox { Location = new Point(12, 60), Size = new Size(200, 28), Font = UiHelper.BodyFont };

            var tipAmount = new Label { Text = "Сумма", Location = new Point(230, 40), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = UiHelper.TextMuted };
            txtAmount = new TextBox { Location = new Point(230, 60), Size = new Size(140, 28), Font = UiHelper.BodyFont };

            var tipCur = new Label { Text = "Валюта", Location = new Point(390, 40), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = UiHelper.TextMuted };
            cmbCurrency = new ComboBox
            {
                Location = new Point(390, 60),
                Size = new Size(160, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            UiHelper.FillCurrencyCombo(cmbCurrency);

            var tipDir = new Label { Text = "Кто кому", Location = new Point(570, 40), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = UiHelper.TextMuted };
            cmbDirection = new ComboBox
            {
                Location = new Point(570, 60),
                Size = new Size(218, 28), // Растягиваем ровно до правого края (570 + 218 = 788)
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            cmbDirection.Items.Add("Мне должны");
            cmbDirection.Items.Add("Я должна");
            cmbDirection.SelectedIndex = 0;

            chkHasDue = new CheckBox
            {
                Text = "Погасить до:",
                Location = new Point(12, 120),
                AutoSize = true,
                Checked = true,
                Font = UiHelper.BodyFont
            };
            chkHasDue.CheckedChanged += (s, e) => dtpDue.Enabled = chkHasDue.Checked;

            dtpDue = new DateTimePicker
            {
                Location = new Point(170, 116),
                Size = new Size(160, 28),
                Format = DateTimePickerFormat.Short,
                Font = UiHelper.BodyFont,
                Value = DateTime.Today.AddDays(14)
            };

            btnAdd = new Button
            {
                Text = "+ Добавить долг",
                Location = new Point(570, 110), // ФИКС: Кнопка стоит ровно под комбобоксом "Кто кому"
                Size = new Size(218, 42),       // И идеально совпадает с ним по ширине
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold) // Увеличен шрифт
            };
            UiHelper.StyleFlatButton(btnAdd, Theme.AccentBlue, Color.White);
            btnAdd.TextAlign = ContentAlignment.MiddleCenter;
            btnAdd.Click += BtnAdd_Click;

            panelAdd.Controls.AddRange(new Control[]
            {
                lblNew, tipPerson, txtPerson, tipAmount, txtAmount,
                tipCur, cmbCurrency, tipDir, cmbDirection,
                chkHasDue, dtpDue, btnAdd
            });

            btnPay = new Button
            {
                Text = "✔  Погасить выбранный долг",
                Location = new Point(20, 565),
                Size = new Size(340, 50),
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold) // Увеличен шрифт
            };
            UiHelper.StyleFlatButton(btnPay, Theme.AccentGreen, Color.White);
            btnPay.TextAlign = ContentAlignment.MiddleCenter;
            btnPay.Click += BtnPay_Click;

            btnClose = new Button
            {
                Text = "Закрыть",
                Location = new Point(640, 565), // ФИКС: Идеально выравниваем по правому краю таблицы (640+180=820 + 20px отступ = 840)
                Size = new Size(180, 50),
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold) // Увеличен шрифт
            };
            UiHelper.StyleFlatButton(btnClose, Color.FromArgb(180, 185, 195), Theme.TextPrimary);
            btnClose.TextAlign = ContentAlignment.MiddleCenter;
            btnClose.Click += (s, e) => Close();

            Controls.Add(lblTitle);
            Controls.Add(grid);
            Controls.Add(panelAdd);
            Controls.Add(btnPay);
            Controls.Add(btnClose);
        }
        private void RefreshGrid()
        {
            var rows = _manager.GetAllDebts()
                .OrderBy(d => d.IsPaid)
                .ThenBy(d => d.DueDate ?? DateTime.MaxValue)
                .ThenByDescending(d => d.CreatedDate)
                .Select(d => new
                {
                    d.Id,
                    Человек = d.PersonName,
                    Направление = d.DirectionDisplay,
                    Сумма = d.Amount,
                    Валюта = d.CurrencyDisplay,
                    Погасить_до = d.DueDateDisplay,
                    Статус = d.IsOverdue ? "Просрочен!" : d.StatusDisplay,
                    d.IsOverdue,
                    Дата = d.CreatedDate.ToString("dd.MM.yyyy")
                })
                .ToList();

            grid.DataSource = null;
            grid.DataSource = rows;
            if (grid.Columns["Id"] != null)
                grid.Columns["Id"].Visible = false;
            if (grid.Columns["IsOverdue"] != null)
                grid.Columns["IsOverdue"].Visible = false;
            if (grid.Columns["Погасить_до"] != null)
                grid.Columns["Погасить_до"].HeaderText = "Погасить до";
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || grid.Columns["IsOverdue"] == null)
                return;

            var overdue = grid.Rows[e.RowIndex].Cells["IsOverdue"].Value;
            if (overdue is bool b && b)
            {
                e.CellStyle.ForeColor = Theme.AccentRed;
                if (grid.Columns[e.ColumnIndex].Name == "Статус" || grid.Columns[e.ColumnIndex].Name == "Погасить_до")
                    e.CellStyle.Font = new Font(grid.Font, FontStyle.Bold);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
                {
                    UiHelper.ShowError(this, "Введите сумму больше нуля.");
                    return;
                }

                DateTime? due = chkHasDue.Checked ? dtpDue.Value.Date : (DateTime?)null;
                bool isOwedToMe = cmbDirection.SelectedIndex == 0;

                _manager.AddDebt(
                    txtPerson.Text,
                    amount,
                    UiHelper.GetSelectedCurrency(cmbCurrency),
                    isOwedToMe,
                    due);

                txtPerson.Clear();
                txtAmount.Clear();
                RefreshGrid();
                UiHelper.ShowInfo(this, "Долг добавлен.");
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }

        private void BtnPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid.CurrentRow == null)
                {
                    UiHelper.ShowError(this, "Выберите долг в таблице.");
                    return;
                }

                var idObj = grid.CurrentRow.Cells["Id"].Value;
                if (idObj == null) return;

                var id = (Guid)idObj;
                var debt = _manager.GetAllDebts().FirstOrDefault(d => d.Id == id);
                if (debt == null) return;

                if (debt.IsPaid)
                {
                    UiHelper.ShowError(this, "Этот долг уже погашен.");
                    return;
                }

                string dueInfo = debt.DueDate.HasValue
                    ? "\nСрок: " + debt.DueDate.Value.ToString("dd.MM.yyyy")
                    : "";

                string ask = debt.IsOwedToMe
                    ? string.Format("Получить {0:N2} {1} от «{2}» на баланс?{3}", debt.Amount, debt.CurrencyDisplay, debt.PersonName, dueInfo)
                    : string.Format("Отдать {0:N2} {1} человеку «{2}» с баланса?{3}", debt.Amount, debt.CurrencyDisplay, debt.PersonName, dueInfo);

                if (MessageBox.Show(this, ask, "Погашение долга", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                _manager.PayDebt(id);
                RefreshGrid();
                UiHelper.ShowInfo(this, "Долг погашен. Баланс обновлён.");
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }
    }
}
