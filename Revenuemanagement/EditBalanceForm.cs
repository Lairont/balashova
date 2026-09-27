using System;
using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>Диалог ручной правки балансов по всем валютам.</summary>
    public class EditBalanceForm : Form
    {
        private readonly FinanceManager _manager;
        private TextBox txtPrb;
        private TextBox txtRub;
        private TextBox txtUsd;
        private TextBox txtEur;

        public EditBalanceForm(FinanceManager manager)
        {
            _manager = manager;
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Изменить баланс";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(400, 340);
            BackColor = Theme.BgMain;
            Font = UiHelper.BodyFont;

            var lbl = new Label
            {
                Text = "✏  Исправьте суммы на счетах",
                Font = UiHelper.TitleFont,
                ForeColor = Theme.TextPrimary,
                Location = new Point(20, 16),
                AutoSize = true
            };

            var hint = new Label
            {
                Text = "Если случайно ввели неверную сумму —\nпросто укажите правильный остаток.",
                Location = new Point(20, 55),
                Size = new Size(360, 40),
                ForeColor = Theme.TextMuted
            };

            txtPrb = MakeField("PRB (ПМР)", 105, _manager.Balances.PRB);
            txtRub = MakeField("RUB (РФ)", 145, _manager.Balances.RUB);
            txtUsd = MakeField("USD ($)", 185, _manager.Balances.USD);
            txtEur = MakeField("EUR (€)", 225, _manager.Balances.EUR);

            var btnOk = new Button
            {
                Text = "✔  Сохранить",
                Size = new Size(180, 44),
                Location = new Point(20, 280)
            };
            UiHelper.StyleFlatButton(btnOk, Theme.AccentGreen, Color.White);
            btnOk.TextAlign = ContentAlignment.MiddleCenter;
            btnOk.Click += BtnOk_Click;

            var btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(120, 44),
                Location = new Point(220, 280)
            };
            UiHelper.StyleFlatButton(btnCancel, Color.FromArgb(180, 185, 195), Theme.TextPrimary);
            btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            Controls.Add(lbl);
            Controls.Add(hint);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Theme.ApplyTo(this);
        }

        private TextBox MakeField(string caption, int top, decimal value)
        {
            var tip = new Label
            {
                Text = caption,
                Location = new Point(20, top),
                AutoSize = true,
                ForeColor = Theme.TextMuted
            };
            var box = new TextBox
            {
                Location = new Point(160, top - 2),
                Size = new Size(180, 28),
                Font = UiHelper.BodyFont,
                Text = value.ToString("0.##")
            };
            Controls.Add(tip);
            Controls.Add(box);
            return box;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TryParseNonNegative(txtPrb.Text, out decimal prb) ||
                    !TryParseNonNegative(txtRub.Text, out decimal rub) ||
                    !TryParseNonNegative(txtUsd.Text, out decimal usd) ||
                    !TryParseNonNegative(txtEur.Text, out decimal eur))
                {
                    UiHelper.ShowError(this, "Введите числа ≥ 0 для всех валют.");
                    return;
                }

                _manager.SetBalances(prb, rub, usd, eur);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }

        private static bool TryParseNonNegative(string text, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(text))
                return false;
            text = text.Trim().Replace(',', '.');
            return decimal.TryParse(
                       text,
                       System.Globalization.NumberStyles.Number,
                       System.Globalization.CultureInfo.InvariantCulture,
                       out value)
                   && value >= 0;
        }
    }
}
