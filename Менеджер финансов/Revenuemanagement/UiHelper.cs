using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Models;

namespace Revenuemanagement
{
    public static class UiHelper
    {
        public static readonly Font TitleFont = new Font("Segoe UI Semibold", 16f, FontStyle.Bold);
        public static readonly Font HeaderFont = new Font("Segoe UI Semibold", 13f, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 12f);
        public static readonly Font BigButtonFont = new Font("Segoe UI Semibold", 12f, FontStyle.Bold);
        public static readonly Font CardAmountFont = new Font("Segoe UI Semibold", 20f, FontStyle.Bold);

        // Совместимость со старыми ссылками
        public static Color BgMain => Theme.BgMain;
        public static Color BgSidebar => Theme.BgSidebar;
        public static Color AccentGreen => Theme.AccentGreen;
        public static Color AccentRed => Theme.AccentRed;
        public static Color AccentBlue => Theme.AccentBlue;
        public static Color AccentGold => Theme.AccentGold;
        public static Color TextDark => Theme.TextPrimary;
        public static Color TextMuted => Theme.TextMuted;

        public static void StyleFlatButton(Button btn, Color back, Color fore)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.Font = BigButtonFont;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(16, 0, 8, 0);
        }

        public static void FillCurrencyCombo(ComboBox combo, CurrencyType selected = CurrencyType.PRB)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Font = BodyFont;
            combo.Items.Clear();
            combo.Items.Add(new CurrencyItem(CurrencyType.PRB, "Рубли ПМР (PRB)"));
            combo.Items.Add(new CurrencyItem(CurrencyType.RUB, "Рубли РФ (RUB)"));
            combo.Items.Add(new CurrencyItem(CurrencyType.USD, "Доллары США (USD)"));
            combo.Items.Add(new CurrencyItem(CurrencyType.EUR, "Евро (EUR)"));
            combo.DisplayMember = "Display";
            combo.ValueMember = "Value";
            combo.SelectedIndex = selected <= CurrencyType.EUR ? (int)selected : 0;
        }

        public static CurrencyType GetSelectedCurrency(ComboBox combo)
        {
            var item = combo.SelectedItem as CurrencyItem;
            return item != null ? item.Value : CurrencyType.PRB;
        }

        public static bool TryParseAmount(string text, out decimal amount)
        {
            amount = 0m;
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim().Replace(',', '.');
            return decimal.TryParse(
                text,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out amount) && amount > 0;
        }

        public static void ShowError(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInfo(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private sealed class CurrencyItem
        {
            public CurrencyItem(CurrencyType value, string display)
            {
                Value = value;
                Display = display;
            }

            public CurrencyType Value { get; }
            public string Display { get; }
            public override string ToString() => Display;
        }
    }
}
