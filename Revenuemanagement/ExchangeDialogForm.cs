using System;
using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>
    /// Обмен валют с автоматическим расчётом. Курс всегда задаётся как
    /// «1 исходная валюта = X целевой валюты» и сохраняется для пары.
    /// </summary>
    public class ExchangeDialogForm : Form
    {
        private readonly FinanceManager _manager;
        private ComboBox cmbFrom;
        private ComboBox cmbTo;
        private Label lblRateHint;
        private TextBox txtRate;
        private TextBox txtAmount;
        private Label lblCalcGive;
        private Label lblCalcReceive;
        private Label lblBalances;
        private Button btnExchange;
        private Button btnCancel;

        public ExchangeDialogForm(FinanceManager manager)
        {
            _manager = manager;
            BuildUi();
            LoadSavedRate();
            Recalculate();
        }

        private void BuildUi()
        {
            Text = "Обмен валют";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(640, 600);
            Padding = new Padding(36);
            BackColor = UiHelper.BgMain;
            Font = UiHelper.BodyFont;

            var lblTitle = new Label
            {
                Text = "Умный обмен валют",
                Font = UiHelper.TitleFont,
                ForeColor = UiHelper.TextDark,
                Location = new Point(36, 28),
                AutoSize = true
            };
            var lblHint = new Label
            {
                Text = "Введите сумму и текущий курс. Итог появится сразу, а последний курс сохранится для выбранной пары валют.",
                Location = new Point(36, 66),
                Size = new Size(568, 42),
                ForeColor = UiHelper.TextMuted
            };

            var lblFrom = MakeLabel("Из валюты:", 36, 124);
            cmbFrom = new ComboBox { Location = new Point(36, 150), Size = new Size(264, 34) };
            UiHelper.FillCurrencyCombo(cmbFrom, CurrencyType.USD);

            var lblTo = MakeLabel("В валюту:", 340, 124);
            cmbTo = new ComboBox { Location = new Point(340, 150), Size = new Size(264, 34) };
            UiHelper.FillCurrencyCombo(cmbTo, CurrencyType.PRB);

            lblRateHint = MakeLabel("Текущий курс:", 36, 208);
            txtRate = new TextBox
            {
                Location = new Point(36, 234),
                Size = new Size(568, 32),
                Font = UiHelper.BodyFont
            };

            var lblAmount = MakeLabel("Сумма, которую отдаю:", 36, 290);
            txtAmount = new TextBox
            {
                Location = new Point(36, 316),
                Size = new Size(280, 32),
                Font = UiHelper.BodyFont
            };

            var resultPanel = new Panel
            {
                Location = new Point(36, 374),
                Size = new Size(568, 82),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(14)
            };
            lblCalcGive = new Label
            {
                Location = new Point(14, 12),
                Size = new Size(530, 24),
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = UiHelper.AccentRed
            };
            lblCalcReceive = new Label
            {
                Location = new Point(14, 42),
                Size = new Size(530, 24),
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = UiHelper.AccentGreen
            };
            resultPanel.Controls.Add(lblCalcGive);
            resultPanel.Controls.Add(lblCalcReceive);

            lblBalances = new Label
            {
                Location = new Point(36, 472),
                Size = new Size(568, 24),
                ForeColor = UiHelper.AccentBlue
            };
            RefreshBalancesLabel();

            btnExchange = new Button
            {
                Text = "Выполнить обмен",
                Size = new Size(300, 48),
                Location = new Point(36, 522),
                Enabled = false
            };
            UiHelper.StyleFlatButton(btnExchange, UiHelper.AccentGold, Color.White);
            btnExchange.TextAlign = ContentAlignment.MiddleCenter;
            btnExchange.Click += BtnExchange_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Size = new Size(180, 48),
                Location = new Point(424, 522)
            };
            UiHelper.StyleFlatButton(btnCancel, Color.FromArgb(200, 205, 215), UiHelper.TextDark);
            btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            cmbFrom.SelectedIndexChanged += CurrencyPairChanged;
            cmbTo.SelectedIndexChanged += CurrencyPairChanged;
            txtRate.TextChanged += (s, e) => Recalculate();
            txtAmount.TextChanged += (s, e) => Recalculate();

            Controls.AddRange(new Control[]
            {
                lblTitle, lblHint, lblFrom, cmbFrom, lblTo, cmbTo,
                lblRateHint, txtRate, lblAmount, txtAmount, resultPanel,
                lblBalances, btnExchange, btnCancel
            });
            AcceptButton = btnExchange;
            CancelButton = btnCancel;

            Theme.ApplyTo(this);
            UpdateRateHintText();
        }

        private Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = UiHelper.TextMuted
            };
        }

        private void CurrencyPairChanged(object sender, EventArgs e)
        {
            if (UiHelper.GetSelectedCurrency(cmbFrom) == UiHelper.GetSelectedCurrency(cmbTo))
            {
                cmbTo.SelectedIndex = (cmbTo.SelectedIndex + 1) % cmbTo.Items.Count;
                return;
            }

            LoadSavedRate();
            UpdateRateHintText();
            Recalculate();
        }

        private void LoadSavedRate()
        {
            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            decimal? saved = _manager.GetSavedExchangeRate(from, to);
            txtRate.Text = saved.HasValue ? saved.Value.ToString("0.####") : string.Empty;
        }

        private void UpdateRateHintText()
        {
            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            lblRateHint.Text = string.Format(
                "Текущий курс: 1 {0} = сколько {1}?",
                FinanceManager.CurrencyShort(from),
                FinanceManager.CurrencyShort(to));
        }

        private void Recalculate()
        {
            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            if (!UiHelper.TryParseAmount(txtRate.Text, out decimal rate) ||
                !UiHelper.TryParseAmount(txtAmount.Text, out decimal amount) ||
                from == to)
            {
                lblCalcGive.Text = "Списывается: —";
                lblCalcReceive.Text = "Будет получено: введите сумму и курс";
                btnExchange.Enabled = false;
                return;
            }

            decimal received = Math.Round(amount * rate, 2, MidpointRounding.AwayFromZero);
            lblCalcGive.Text = string.Format("Списывается: {0:N2} {1}", amount, FinanceManager.CurrencyShort(from));
            lblCalcReceive.Text = string.Format("Будет получено: {0:N2} {1}", received, FinanceManager.CurrencyShort(to));
            btnExchange.Enabled = received > 0;
        }

        private void RefreshBalancesLabel()
        {
            Balances balance = _manager.Balances;
            lblBalances.Text = string.Format(
                "На счетах:  PRB {0:N2}  |  RUB {1:N2}  |  USD {2:N2}  |  EUR {3:N2}",
                balance.PRB, balance.RUB, balance.USD, balance.EUR);
        }

        private void BtnExchange_Click(object sender, EventArgs e)
        {
            try
            {
                CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
                CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
                if (from == to)
                    throw new InvalidOperationException("Выберите разные валюты.");
                if (!UiHelper.TryParseAmount(txtRate.Text, out decimal rate))
                    throw new InvalidOperationException("Введите текущий курс больше нуля.");
                if (!UiHelper.TryParseAmount(txtAmount.Text, out decimal amount))
                    throw new InvalidOperationException("Введите сумму больше нуля.");

                decimal received = Math.Round(amount * rate, 2, MidpointRounding.AwayFromZero);
                if (received <= 0)
                    throw new InvalidOperationException("Не удалось рассчитать итоговую сумму.");

                _manager.SaveExchangeRate(from, to, rate);
                _manager.Exchange(from, amount, to, received);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }
    }
}
