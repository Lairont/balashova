using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;

namespace Revenuemanagement
{
    /// <summary>Простая круговая диаграмма расходов по категориям (без внешних библиотек).</summary>
    public class ExpensePiePanel : Panel
    {
        private List<CategoryTotal> _data = new List<CategoryTotal>();
        private readonly string[] _palette =
        {
            "#2EA078", "#4682B4", "#D4A032", "#D25555",
            "#7B68EE", "#20B2AA", "#CD853F", "#708090"
        };

        public ExpensePiePanel()
        {
            // Полная двойная буферизация — убирает полосы и «хвосты» при ресайзе
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Opaque, true);
            DoubleBuffered = true;
            ResizeRedraw = true;
            UpdateStyles();
        }

        public void SetData(IEnumerable<CategoryTotal> data)
        {
            _data = (data ?? Enumerable.Empty<CategoryTotal>()).ToList();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Сами очищаем фон в OnPaint — иначе WinForms оставляет артефакты
            e.Graphics.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Не вызываем base.OnPaint — рисуем кадр целиком сами
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            using (var titleFont = new Font("Segoe UI Semibold", 11f, FontStyle.Bold))
            using (var smallFont = new Font("Segoe UI", 9f))
            using (var brushTitle = new SolidBrush(Theme.TextPrimary))
            using (var brushMuted = new SolidBrush(Theme.TextMuted))
            {
                g.DrawString("Расходы по категориям", titleFont, brushTitle, 8, 8);

                if (_data.Count == 0 || _data.Sum(x => x.Total) <= 0)
                {
                    g.DrawString("Пока нет расходов\nза выбранный период", smallFont, brushMuted, 8, 40);
                    return;
                }

                decimal sum = _data.Sum(x => x.Total);
                int size = Math.Min(Width - 24, Height - 120);
                if (size < 60) size = 60;
                var rect = new Rectangle(12, 36, size, size);

                float start = -90f;
                for (int i = 0; i < _data.Count; i++)
                {
                    float sweep = (float)((double)_data[i].Total / (double)sum * 360.0);
                    using (var br = new SolidBrush(ColorTranslator.FromHtml(_palette[i % _palette.Length])))
                    {
                        if (sweep > 0.1f)
                            g.FillPie(br, rect, start, sweep);
                    }
                    start += sweep;
                }

                using (var pen = new Pen(BackColor, 2))
                    g.DrawEllipse(pen, rect);

                int legendY = rect.Bottom + 12;
                for (int i = 0; i < _data.Count && legendY < Height - 8; i++)
                {
                    var c = ColorTranslator.FromHtml(_palette[i % _palette.Length]);
                    using (var br = new SolidBrush(c))
                        g.FillRectangle(br, 12, legendY + 2, 12, 12);

                    string label = string.Format("{0}: {1:N0}", _data[i].Category, _data[i].Total);
                    if (label.Length > 28) label = label.Substring(0, 26) + "…";
                    g.DrawString(label, smallFont, brushTitle, 30, legendY);
                    legendY += 18;
                }
            }
        }
    }
}
