using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Revenuemanagement.Models;

namespace Revenuemanagement
{
    /// <summary>Печать расходов в виде кассовой квитанции.</summary>
    public static class ExpensePrintHelper
    {
        private const int ReceiptWidthChars = 42;

        public static void ShowPreview(IWin32Window owner, IList<HistoryRow> rows, string periodTitle)
        {
            if (rows == null || rows.Count == 0)
            {
                UiHelper.ShowError(owner, "Нет расходов для печати в текущем списке.");
                return;
            }

            var receipt = BuildReceipt(rows, periodTitle);
            int lineIndex = 0;

            var doc = new PrintDocument();
            doc.DocumentName = "Квитанция расходов";

            doc.PrintPage += (s, e) =>
            {
                float y = e.MarginBounds.Top;
                float centerX = e.MarginBounds.Left + e.MarginBounds.Width / 2f;

                using (var fontTitle = new Font("Courier New", 12f, FontStyle.Bold))
                using (var fontBold = new Font("Courier New", 9f, FontStyle.Bold))
                using (var fontBody = new Font("Courier New", 9f, FontStyle.Regular))
                {
                    while (lineIndex < receipt.Count)
                    {
                        ReceiptLine line = receipt[lineIndex];
                        Font f = line.Bold ? fontBold : (line.Title ? fontTitle : fontBody);
                        float h = f.GetHeight(e.Graphics) + 2f;

                        if (y + h > e.MarginBounds.Bottom)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        if (line.Centered)
                        {
                            SizeF size = e.Graphics.MeasureString(line.Text, f);
                            e.Graphics.DrawString(line.Text, f, Brushes.Black, centerX - size.Width / 2f, y);
                        }
                        else
                        {
                            e.Graphics.DrawString(line.Text, f, Brushes.Black, e.MarginBounds.Left, y);
                        }

                        y += h;
                        lineIndex++;
                    }
                }

                e.HasMorePages = false;
            };

            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = doc;
                preview.Width = 480;
                preview.Height = 720;
                preview.ShowDialog(owner);
            }
        }

        private static List<ReceiptLine> BuildReceipt(IList<HistoryRow> rows, string periodTitle)
        {
            var lines = new List<ReceiptLine>();
            string dash = new string('-', ReceiptWidthChars);

            lines.Add(new ReceiptLine { Text = "КВИТАНЦИЯ ПО РАСХОДАМ", Title = true, Centered = true });
            lines.Add(new ReceiptLine { Text = periodTitle, Centered = true });
            lines.Add(new ReceiptLine { Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm"), Centered = true });
            lines.Add(new ReceiptLine { Text = dash });

            lines.Add(new ReceiptLine
            {
                Text = PadColumns("ДАТА", "КАТЕГОРИЯ", "СУММА"),
                Bold = true
            });
            lines.Add(new ReceiptLine { Text = dash });

            var totalsByCurrency = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            foreach (var r in rows)
            {
                string date = r.Дата ?? "";
                if (date.Length > 10)
                    date = date.Substring(0, 10);

                string cat = Truncate(r.Категория ?? "", 14);
                string sum = (r.Сумма ?? "0") + " " + ShortCurrency(r.Валюта);
                lines.Add(new ReceiptLine { Text = PadColumns(date, cat, sum) });

                if (decimal.TryParse((r.Сумма ?? "").Replace(" ", ""), out decimal amt))
                {
                    string cur = ShortCurrency(r.Валюта);
                    if (!totalsByCurrency.ContainsKey(cur))
                        totalsByCurrency[cur] = 0m;
                    totalsByCurrency[cur] += amt;
                }
            }

            lines.Add(new ReceiptLine { Text = dash });
            string total = string.Join(" | ", totalsByCurrency
                .OrderBy(k => k.Key)
                .Select(k => string.Format("{0:N2} {1}", k.Value, k.Key)));
            lines.Add(new ReceiptLine { Text = "ИТОГО: " + total, Bold = true });

            return lines;
        }

        private static string PadColumns(string col1, string col2, string col3)
        {
            return Fit(col1, 10) + Fit(col2, 16) + Fit(col3, 16, alignRight: true);
        }

        private static string Fit(string text, int width, bool alignRight = false)
        {
            text = text ?? "";
            if (text.Length > width)
                text = text.Substring(0, width);
            return alignRight ? text.PadLeft(width) : text.PadRight(width);
        }

        private static string Truncate(string text, int max)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= max)
                return text ?? "";
            return text.Substring(0, max - 1) + "…";
        }

        private static string ShortCurrency(string display)
        {
            if (string.IsNullOrEmpty(display))
                return "";
            if (display.StartsWith("RUB", StringComparison.OrdinalIgnoreCase)) return "RUB";
            if (display.StartsWith("PRB", StringComparison.OrdinalIgnoreCase)) return "PRB";
            if (display.StartsWith("USD", StringComparison.OrdinalIgnoreCase)) return "USD";
            if (display.StartsWith("EUR", StringComparison.OrdinalIgnoreCase)) return "EUR";
            return display;
        }

        private sealed class ReceiptLine
        {
            public string Text { get; set; }
            public bool Bold { get; set; }
            public bool Title { get; set; }
            public bool Centered { get; set; }
        }
    }
}
