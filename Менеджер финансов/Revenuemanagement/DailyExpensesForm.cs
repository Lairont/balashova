using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Revenuemanagement.Models;

namespace Revenuemanagement
{
    /// <summary>Расходы по дням за выбранный период.</summary>
    public class DailyExpensesForm : Form
    {
        public DailyExpensesForm(IReadOnlyList<DayExpenseTotal> days, string periodTitle)
        {
            Text = "Расходы по дням";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            ClientSize = new Size(520, 420);
            Font = UiHelper.BodyFont;

            var lbl = new Label
            {
                Text = "📅  " + periodTitle,
                Font = UiHelper.HeaderFont,
                Location = new Point(16, 12),
                AutoSize = true
            };

            var list = new ListBox
            {
                Location = new Point(16, 48),
                Size = new Size(488, 320),
                Font = UiHelper.BodyFont,
                IntegralHeight = false
            };

            if (days == null || days.Count == 0)
                list.Items.Add("За этот период расходов нет.");
            else
            {
                foreach (var d in days)
                    list.Items.Add(d.Summary);
            }

            var btnClose = new Button
            {
                Text = "Закрыть",
                Size = new Size(120, 40),
                Location = new Point(384, 372)
            };
            UiHelper.StyleFlatButton(btnClose, Color.FromArgb(200, 205, 215), Theme.TextPrimary);
            btnClose.TextAlign = ContentAlignment.MiddleCenter;
            btnClose.Click += (s, e) => Close();

            Controls.Add(lbl);
            Controls.Add(list);
            Controls.Add(btnClose);

            Theme.ApplyTo(this);
        }
    }
}
