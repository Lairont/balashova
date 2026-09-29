using System;
using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>Редактирование суммы и категории дохода/расхода.</summary>
    public class EditTransactionForm : Form
    {
        private readonly FinanceManager _manager;
        private readonly Transaction _tx;
        private TextBox txtCategory;
        private ComboBox cmbShop;
        private Label lblShop;
        private TextBox txtAmount;
        private ComboBox cmbCurrency;

        public EditTransactionForm(FinanceManager manager, Transaction tx)
        {
            _manager = manager;
            _tx = tx;
            BuildUi();
        }

        private void BuildUi()
        {
            bool isExpense = _tx.Type == TransactionType.Expense;
            Text = "Изменить запись";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(420, isExpense ? 320 : 280);
            BackColor = Theme.BgMain;
            Font = UiHelper.BodyFont;

            var lbl = new Label
            {
                Text = _tx.Type == TransactionType.Income ? "✏  Изменить доход" : "✏  Изменить расход",
                Font = UiHelper.TitleFont,
                ForeColor = Theme.TextPrimary,
                Location = new Point(20, 16),
                AutoSize = true
            };

            var tipCat = new Label { Text = "Категория", Location = new Point(20, 60), AutoSize = true, ForeColor = Theme.TextMuted };
            txtCategory = new TextBox
            {
                Location = new Point(20, 82),
                Size = new Size(370, 28),
                Text = _tx.Category,
                Font = UiHelper.BodyFont
            };
            txtCategory.TextChanged += (s, e) => UpdateShopVisibility();

            lblShop = new Label
            {
                Text = "Магазин",
                Location = new Point(20, 118),
                AutoSize = true,
                ForeColor = Theme.TextMuted,
                Visible = false
            };
            cmbShop = new ComboBox
            {
                Location = new Point(20, 140),
                Size = new Size(370, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false
            };
            cmbShop.Items.AddRange(GroceryShops.All);
            if (!string.IsNullOrWhiteSpace(_tx.Shop))
                cmbShop.Text = _tx.Shop;
            else
                cmbShop.SelectedIndex = 0;

            int amtTop = isExpense ? 178 : 120;
            var tipAmt = new Label { Text = "Сумма", Location = new Point(20, amtTop), AutoSize = true, ForeColor = Theme.TextMuted };
            txtAmount = new TextBox
            {
                Location = new Point(20, amtTop + 22),
                Size = new Size(160, 28),
                Text = _tx.Amount.ToString("0.##"),
                Font = UiHelper.BodyFont
            };

            var tipCur = new Label { Text = "Валюта", Location = new Point(200, amtTop), AutoSize = true, ForeColor = Theme.TextMuted };
            cmbCurrency = new ComboBox { Location = new Point(200, amtTop + 22), Size = new Size(190, 28) };
            UiHelper.FillCurrencyCombo(cmbCurrency, _tx.Currency);

            int btnTop = amtTop + 68;
            var btnOk = new Button
            {
                Text = "✔  Сохранить",
                Size = new Size(180, 44),
                Location = new Point(20, btnTop)
            };
            UiHelper.StyleFlatButton(btnOk, Theme.AccentGreen, Color.White);
            btnOk.TextAlign = ContentAlignment.MiddleCenter;
            btnOk.Click += BtnOk_Click;

            var btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(120, 44),
                Location = new Point(220, btnTop)
            };
            UiHelper.StyleFlatButton(btnCancel, Color.FromArgb(180, 185, 195), Theme.TextPrimary);
            btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[]
            {
                lbl, tipCat, txtCategory, lblShop, cmbShop,
                tipAmt, txtAmount, tipCur, cmbCurrency, btnOk, btnCancel
            });
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Theme.ApplyTo(this);
            UpdateShopVisibility();
        }

        private void UpdateShopVisibility()
        {
            bool show = _tx.Type == TransactionType.Expense &&
                        string.Equals(txtCategory.Text.Trim(), "Продукты", StringComparison.OrdinalIgnoreCase);
            lblShop.Visible = show;
            cmbShop.Visible = show;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
                {
                    UiHelper.ShowError(this, "Введите сумму больше нуля.");
                    return;
                }

                string shop = cmbShop.Visible ? cmbShop.Text : string.Empty;
                _manager.EditTransaction(
                    _tx.Id,
                    txtCategory.Text,
                    amount,
                    UiHelper.GetSelectedCurrency(cmbCurrency),
                    shop);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }
    }
}
