using System;
using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    /// <summary>
    /// Просторный конвертер. Для пары с USD/EUR курс всегда указан за одну
    /// единицу USD/EUR: например, 1 USD = 16.35 PRB.
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
        private bool _changingCurrencies;

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
            ClientSize = new Size(820, 780);
            MinimumSize = Size;
            Padding = new Padding(42, 36, 42, 36);
            BackColor = UiHelper.BgMain;
            Font = UiHelper.BodyFont;

            // Главный контейнер (строго друг под другом)
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
                Text = "Умный обмен валют",
                AutoSize = true,
                Font = UiHelper.TitleFont,
                ForeColor = UiHelper.TextDark,
                Margin = new Padding(0, 0, 0, 10)
            };
            var hint = new Label
            {
                Text = "Выберите валюты, затем укажите сумму списания и курс. Результат рассчитывается автоматически, а курс сохранится для следующего обмена этой пары.",
                AutoSize = true,
                ForeColor = UiHelper.TextMuted,
                Margin = new Padding(0, 0, 0, 20)
            };

            // Блок выбора валют (Из валюты -> В валюту)
            cmbFrom = new ComboBox { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Font = UiHelper.BodyFont };
            UiHelper.FillCurrencyCombo(cmbFrom, CurrencyType.PRB);

            cmbTo = new ComboBox { Width = 312, DropDownStyle = ComboBoxStyle.DropDownList, Font = UiHelper.BodyFont };
            UiHelper.FillCurrencyCombo(cmbTo, CurrencyType.USD);

            var currencies = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 18),
                Padding = new Padding(0, 0, 0, 5) // Запас от обрезки снизу
            };
            var fromWrap = CreateFieldSection("Из валюты", cmbFrom, 0);
            fromWrap.Margin = new Padding(0, 0, 20, 0); // Отступ между блоками
            currencies.Controls.Add(fromWrap);
            currencies.Controls.Add(CreateFieldSection("В валюту", cmbTo, 0));

            // Блок ввода курса (собираем вручную, так как Label обновляется)
            lblRateHint = new Label
            {
                AutoSize = true,
                ForeColor = UiHelper.TextMuted,
                Location = new Point(0, 0) // Текст строго сверху
            };
            txtRate = new TextBox { Width = 632, Font = UiHelper.BodyFont };
            txtRate.Location = new Point(0, 28); // Поле строго под текстом

            var rateSection = new Panel { Margin = new Padding(0, 0, 0, 18) };
            rateSection.Controls.Add(lblRateHint);
            rateSection.Controls.Add(txtRate);
            rateSection.Width = 635;
            rateSection.Height = txtRate.Location.Y + txtRate.Height + 5; // Идеальная высота

            // Блок ввода суммы
            txtAmount = new TextBox { Width = 632, Font = UiHelper.BodyFont };
            var amountSection = CreateFieldSection("Сумма, которую отдаю", txtAmount, 18);

            // Панель результата
            var resultPanel = new Panel
            {
                Width = 632,
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18),
                Margin = new Padding(0, 0, 0, 18)
            };
            lblCalcReceive = new Label
            {
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = UiHelper.AccentGreen
            };
            lblCalcGive = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = UiHelper.AccentRed
            };
            resultPanel.Controls.Add(lblCalcReceive);
            resultPanel.Controls.Add(lblCalcGive);

            // Балансы счетов
            lblBalances = new Label
            {
                AutoSize = true,
                ForeColor = UiHelper.AccentBlue,
                Margin = new Padding(0, 0, 0, 18)
            };
            RefreshBalancesLabel();

            // Кнопки
            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Width = 632, // Ширина под размер полей
                Margin = new Padding(0, 20, 0, 0)
            };
            var cancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Size = new Size(190, 48), Margin = new Padding(14, 0, 0, 0) };
            UiHelper.StyleFlatButton(cancel, Color.FromArgb(200, 205, 215), UiHelper.TextDark);
            cancel.TextAlign = ContentAlignment.MiddleCenter;
            cancel.Padding = Padding.Empty;

            btnExchange = new Button { Text = "Выполнить обмен", Size = new Size(330, 48), Enabled = false };
            UiHelper.StyleFlatButton(btnExchange, UiHelper.AccentGold, Color.White);
            btnExchange.TextAlign = ContentAlignment.MiddleCenter;
            btnExchange.Padding = Padding.Empty;
            btnExchange.Click += BtnExchange_Click;

            buttons.Controls.Add(cancel);
            buttons.Controls.Add(btnExchange);

            // Собираем всё в главное окно
            layout.Controls.Add(title);
            layout.Controls.Add(hint);
            layout.Controls.Add(currencies);
            layout.Controls.Add(rateSection);
            layout.Controls.Add(amountSection);
            layout.Controls.Add(resultPanel);
            layout.Controls.Add(lblBalances);
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            AcceptButton = btnExchange;
            CancelButton = cancel;

            cmbFrom.SelectedIndexChanged += CurrencyPairChanged;
            cmbTo.SelectedIndexChanged += CurrencyPairChanged;
            txtRate.TextChanged += (s, e) => Recalculate();
            txtAmount.TextChanged += (s, e) => Recalculate();

            Theme.ApplyTo(this);
            UpdateRateHintText();
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
                Location = new Point(0, 0) // Текст строго сверху
            };

            // Поле ввода жестко отступает от верхнего края на 28 пикселей
            input.Location = new Point(0, 28);

            section.Controls.Add(label);
            section.Controls.Add(input);

            section.Width = input.Width > label.PreferredWidth ? input.Width : label.PreferredWidth;

            // Фикс обрезания: добавляем +5 пикселей к итоговой высоте блока
            section.Height = input.Location.Y + input.Height + 5;

            return section;
        }
        private void CurrencyPairChanged(object sender, EventArgs e)
        {
            if (_changingCurrencies)
                return;

            if (UiHelper.GetSelectedCurrency(cmbFrom) == UiHelper.GetSelectedCurrency(cmbTo))
            {
                _changingCurrencies = true;
                cmbTo.SelectedIndex = (cmbTo.SelectedIndex + 1) % cmbTo.Items.Count;
                _changingCurrencies = false;
            }

            LoadSavedRate();
            UpdateRateHintText();
            Recalculate();
        }

        private void GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency)
        {
            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            if (IsHardCurrency(from) && IsLocalCurrency(to))
            {
                baseCurrency = from;
                quoteCurrency = to;
            }
            else if (IsHardCurrency(to) && IsLocalCurrency(from))
            {
                baseCurrency = to;
                quoteCurrency = from;
            }
            else
            {
                baseCurrency = from;
                quoteCurrency = to;
            }
        }

        private static bool IsHardCurrency(CurrencyType currency)
        {
            return currency == CurrencyType.USD || currency == CurrencyType.EUR;
        }

        private static bool IsLocalCurrency(CurrencyType currency)
        {
            return currency == CurrencyType.PRB || currency == CurrencyType.RUB;
        }

        private void LoadSavedRate()
        {
            GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
            decimal? savedRate = _manager.GetSavedExchangeRate(baseCurrency, quoteCurrency);
            txtRate.Text = savedRate.HasValue ? savedRate.Value.ToString("0.####") : string.Empty;
        }

        private void UpdateRateHintText()
        {
            GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
            lblRateHint.Text = string.Format(
                "Текущий курс: 1 {0} = сколько {1}?",
                FinanceManager.CurrencyShort(baseCurrency),
                FinanceManager.CurrencyShort(quoteCurrency));
        }

        private bool TryCalculate(out decimal sourceAmount, out decimal receivedAmount)
        {
            sourceAmount = 0m;
            receivedAmount = 0m;
            if (!UiHelper.TryParseAmount(txtAmount.Text, out sourceAmount) ||
                !UiHelper.TryParseAmount(txtRate.Text, out decimal rate))
                return false;

            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            if (from == to)
                return false;

            GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
            if (from == baseCurrency && to == quoteCurrency)
                receivedAmount = sourceAmount * rate;
            else if (from == quoteCurrency && to == baseCurrency)
                receivedAmount = sourceAmount / rate;
            else
                return false;

            receivedAmount = Math.Round(receivedAmount, 2, MidpointRounding.AwayFromZero);
            return receivedAmount > 0;
        }

        private void Recalculate()
        {
            CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
            CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
            if (!TryCalculate(out decimal sourceAmount, out decimal receivedAmount))
            {
                lblCalcGive.Text = "Отдаете: —";
                lblCalcReceive.Text = "Будет получено: введите сумму и курс";
                btnExchange.Enabled = false;
                return;
            }

            lblCalcGive.Text = string.Format("Отдаете: {0:N2} {1}", sourceAmount, FinanceManager.CurrencyShort(from));
            lblCalcReceive.Text = string.Format("Будет получено: {0:N2} {1}", receivedAmount, FinanceManager.CurrencyShort(to));
            btnExchange.Enabled = true;
        }

        private void RefreshBalancesLabel()
        {
            Balances balances = _manager.Balances;
            lblBalances.Text = string.Format(
                "На счетах: PRB {0:N2}   |   RUB {1:N2}   |   USD {2:N2}   |   EUR {3:N2}",
                balances.PRB, balances.RUB, balances.USD, balances.EUR);
        }

        private void BtnExchange_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TryCalculate(out decimal sourceAmount, out decimal receivedAmount))
                    throw new InvalidOperationException("Введите корректную сумму и текущий курс.");

                CurrencyType from = UiHelper.GetSelectedCurrency(cmbFrom);
                CurrencyType to = UiHelper.GetSelectedCurrency(cmbTo);
                GetRateCurrencies(out CurrencyType baseCurrency, out CurrencyType quoteCurrency);
                if (!UiHelper.TryParseAmount(txtRate.Text, out decimal rate))
                    throw new InvalidOperationException("Курс должен быть больше нуля.");

                _manager.SaveExchangeRate(baseCurrency, quoteCurrency, rate);
                _manager.Exchange(from, sourceAmount, to, receivedAmount);
                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                UiHelper.ShowError(this, exception.Message);
            }
        }
    }
}
