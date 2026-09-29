using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// Просторный диалог добавления дохода или расхода.
    public class TransactionDialogForm : Form
    {
        private readonly FinanceManager _manager;
        private readonly bool _isIncome;

        private ComboBox cmbCategory;
        private ComboBox cmbShop;
        private TextBox txtAmount;
        private ComboBox cmbCurrency;
        private TextBox txtNote;
        private Control shopSection;

        public TransactionDialogForm(FinanceManager manager, bool isIncome)
        {
            _manager = manager;
            _isIncome = isIncome;
            BuildUi();
        }

        private void BuildUi()
        {
            Text = _isIncome ? "Добавить доход" : "Добавить расход";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(820, _isIncome ? 680 : 760);
            MinimumSize = Size;
            Padding = new Padding(42, 36, 42, 36);
            BackColor = UiHelper.BgMain;
            Font = UiHelper.BodyFont;

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var title = new Label
            {
                Text = _isIncome ? "Новый доход" : "Новый расход",
                AutoSize = true,
                Font = UiHelper.TitleFont,
                ForeColor = UiHelper.TextDark,
                Margin = new Padding(0, 0, 0, 10)
            };
            var hint = new Label
            {
                Text = "Заполните сумму, валюту и категорию. Категорию можно добавить кнопкой +.",
                AutoSize = true,
                ForeColor = UiHelper.TextMuted,
                Margin = new Padding(0, 0, 0, 20)
            };

            cmbCategory = new ComboBox
            {
                Width = 570,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            ReloadCategories();
            cmbCategory.SelectedIndexChanged += (s, e) => UpdateShopVisibility();

            var categoryInput = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0),
                // ФИКС ОБРЕЗАНИЯ: Даем блоку 5 пикселей запаса снизу
                Padding = new Padding(0, 0, 0, 5)
            };
            var addCategory = new Button
            {
                Text = "+",
                // ФИКС КНОПКИ: Полноценная высота 36px, чтобы не была сплюснутой
                Size = new Size(48, 36),
                Margin = new Padding(14, 0, 0, 0),
                Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold)
            };
            UiHelper.StyleFlatButton(addCategory, Theme.AccentBlue, Color.White);
            addCategory.TextAlign = ContentAlignment.MiddleCenter;
            addCategory.Padding = Padding.Empty;
            addCategory.Click += BtnAddCategory_Click;

            categoryInput.Controls.Add(cmbCategory);
            categoryInput.Controls.Add(addCategory);

            cmbShop = new ComboBox
            {
                Width = 632,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiHelper.BodyFont
            };
            cmbShop.Items.AddRange(GroceryShops.All);
            if (cmbShop.Items.Count > 0)
                cmbShop.SelectedIndex = 0;

            txtAmount = new TextBox { Width = 300, Font = UiHelper.BodyFont };
            cmbCurrency = new ComboBox
            {
                Width = 312,
                Font = UiHelper.BodyFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            UiHelper.FillCurrencyCombo(cmbCurrency);

            var amountSection = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 18),
                // ФИКС ОБРЕЗАНИЯ ВАЛЮТЫ: Даем блоку 5 пикселей запаса снизу
                Padding = new Padding(0, 0, 0, 5)
            };
            var amountWrap = CreateFieldSection("Сумма", txtAmount, 0);
            amountWrap.Margin = new Padding(0, 0, 20, 0);
            amountSection.Controls.Add(amountWrap);
            amountSection.Controls.Add(CreateFieldSection("Валюта", cmbCurrency, 0));

            txtNote = new TextBox
            {
                Width = 632,
                Height = 82,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = UiHelper.BodyFont
            };

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Width = 632,
                Margin = new Padding(0, 20, 0, 0)
            };
            var cancel = new Button { Text = "Отмена", Size = new Size(190, 48), Margin = new Padding(12, 0, 0, 0) };
            UiHelper.StyleFlatButton(cancel, Color.FromArgb(200, 205, 215), UiHelper.TextDark);
            cancel.TextAlign = ContentAlignment.MiddleCenter;
            cancel.Padding = Padding.Empty;
            cancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            var save = new Button
            {
                Text = _isIncome ? "Сохранить доход" : "Сохранить расход",
                Size = new Size(290, 48)
            };
            UiHelper.StyleFlatButton(save, _isIncome ? UiHelper.AccentGreen : UiHelper.AccentRed, Color.White);
            save.TextAlign = ContentAlignment.MiddleCenter;
            save.Padding = Padding.Empty;
            save.Click += BtnSave_Click;

            buttons.Controls.Add(cancel);
            buttons.Controls.Add(save);

            shopSection = CreateFieldSection("Магазин для категории «Продукты»", cmbShop, 18);

            layout.Controls.Add(title);
            layout.Controls.Add(hint);
            layout.Controls.Add(CreateFieldSection("Категория", categoryInput, 18));
            layout.Controls.Add(shopSection);
            layout.Controls.Add(amountSection);
            layout.Controls.Add(CreateFieldSection("Заметка (необязательно)", txtNote, 0));
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            AcceptButton = save;
            CancelButton = cancel;
            Theme.ApplyTo(this);
            UpdateShopVisibility();
        }

        private Control CreateFieldSection(string labelText, Control input, int bottomMargin)
        {
            var section = new Panel
            {
                Margin = new Padding(0, 0, 0, bottomMargin)
            };

            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                ForeColor = UiHelper.TextMuted,
                Location = new Point(0, 0)
            };

            input.Location = new Point(0, label.PreferredHeight + 15);

            section.Controls.Add(label);
            section.Controls.Add(input);

            section.Width = input.Width > label.PreferredWidth ? input.Width : label.PreferredWidth;

            // ФИКС ОБРЕЗАНИЯ ДЛЯ ОСТАЛЬНЫХ ПОЛЕЙ: Добавляем +5 пикселей к высоте панели
            section.Height = input.Location.Y + input.Height + 5;

            return section;
        }
        private void ReloadCategories()
        {
            string previous = cmbCategory.Text;
            cmbCategory.Items.Clear();
            var categories = _isIncome ? _manager.GetIncomeCategories() : _manager.GetExpenseCategories();
            cmbCategory.Items.AddRange(categories.Cast<object>().ToArray());
            if (!string.IsNullOrWhiteSpace(previous) && cmbCategory.Items.Contains(previous))
                cmbCategory.SelectedItem = previous;
            else if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            using (var dialog = new SimpleInputDialog(
                "Новая категория",
                "Введите название категории (например, «Маникюр»):"))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    string category = _isIncome
                        ? _manager.AddIncomeCategory(dialog.InputText)
                        : _manager.AddExpenseCategory(dialog.InputText);
                    ReloadCategories();
                    cmbCategory.SelectedItem = category;
                }
                catch (Exception exception)
                {
                    UiHelper.ShowError(this, exception.Message);
                }
            }
        }

        private void UpdateShopVisibility()
        {
            bool visible = !_isIncome && string.Equals(cmbCategory.Text, "Продукты", StringComparison.OrdinalIgnoreCase);
            shopSection.Visible = visible;
            PerformLayout();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCategory.SelectedItem == null)
                    throw new InvalidOperationException("Добавьте и выберите категорию.");
                if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
                    throw new InvalidOperationException("Введите сумму больше нуля. Можно использовать точку или запятую.");

                string category = cmbCategory.SelectedItem.ToString();
                CurrencyType currency = UiHelper.GetSelectedCurrency(cmbCurrency);
                string note = txtNote.Text.Trim();
                string shop = shopSection.Visible ? cmbShop.Text : string.Empty;
                if (_isIncome)
                    _manager.AddIncome(category, amount, currency, note);
                else
                    _manager.AddExpense(category, amount, currency, note, shop);
                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                UiHelper.ShowError(this, exception.Message);
            }
        }
    }

    /// <summary>Небольшой диалог ввода новой пользовательской категории.</summary>
    public class SimpleInputDialog : Form
    {
        private readonly TextBox _textBox;

        public string InputText => _textBox.Text.Trim();

        public SimpleInputDialog(string title, string prompt, string defaultValue = "")
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(620, 270);
            Padding = new Padding(34);
            Font = UiHelper.BodyFont;

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 1, RowCount = 3 };
            var caption = new Label { Text = prompt, AutoSize = false, Height = 36, Dock = DockStyle.Top, ForeColor = Theme.TextMuted, Margin = new Padding(0, 0, 0, 10) };
            _textBox = new TextBox { Dock = DockStyle.Top, Font = UiHelper.BodyFont, Text = defaultValue ?? string.Empty, Margin = new Padding(0, 0, 0, 20) };
            var buttons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Top };
            var cancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Size = new Size(150, 42), Margin = new Padding(12, 0, 0, 0) };
            UiHelper.StyleFlatButton(cancel, Color.FromArgb(200, 205, 215), Theme.TextPrimary);
            cancel.TextAlign = ContentAlignment.MiddleCenter;
            cancel.Padding = Padding.Empty;
            var accept = new Button { Text = "Добавить", Size = new Size(150, 42) };
            UiHelper.StyleFlatButton(accept, Theme.AccentGreen, Color.White);
            accept.TextAlign = ContentAlignment.MiddleCenter;
            accept.Padding = Padding.Empty;
            accept.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_textBox.Text))
                {
                    UiHelper.ShowError(this, "Введите название.");
                    return;
                }
                DialogResult = DialogResult.OK;
            };
            buttons.Controls.Add(cancel);
            buttons.Controls.Add(accept);
            layout.Controls.Add(caption, 0, 0);
            layout.Controls.Add(_textBox, 0, 1);
            layout.Controls.Add(buttons, 0, 2);
            Controls.Add(layout);
            AcceptButton = accept;
            CancelButton = cancel;
            Theme.ApplyTo(this);
        }
    }
}
