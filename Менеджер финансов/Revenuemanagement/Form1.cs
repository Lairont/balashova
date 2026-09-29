using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Revenuemanagement.Models;
using Revenuemanagement.Services;

namespace Revenuemanagement
{
    public partial class Form1 : Form
    {
        private FinanceManager _manager;
        private bool _gridReady;
        private List<HistoryRow> _filteredRows = new List<HistoryRow>();
        private Label _dayFilterLabel;
        private DateTimePicker _dayFilter;

        public Form1()
        {
            InitializeComponent();
            ConfigureExactDayFilter();
            cmbPeriod.SelectedIndex = 0;
            cmbTypeFilter.SelectedIndex = 0;
            panelMain.Resize += (s, e) => LayoutHistoryArea();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _manager = new FinanceManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось загрузить данные:\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _manager = new FinanceManager(new JsonDataStore("finance_data_new.json"));
            }

            Theme.Apply(_manager.IsDarkTheme);
            ApplyThemeToForm();
            LayoutHistoryArea();
            ConfigureGridWrap();
            RefreshDashboard();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { _manager?.Save(); }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Не удалось сохранить данные:\n" + ex.Message,
                    "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigureGridWrap()
        {
            gridHistory.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            gridHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridHistory.RowTemplate.Height = 34;
            gridHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        private void ApplyThemeToForm()
        {
            BackColor = Theme.BgMain;
            panelMain.BackColor = Theme.BgMain;
            panelSidebar.BackColor = Theme.BgSidebar;
            panelFooter.BackColor = Theme.FooterBack;
            panelFilter.BackColor = Theme.BgMain;
            panelStats.BackColor = Theme.BgMain;
            panelContent.BackColor = Theme.BgMain;
            panelPieHost.BackColor = Theme.BgMain;
            pieExpenses.BackColor = Theme.BgCard;

            lblAppTitle.ForeColor = Theme.TextOnSidebar;
            lblAppSubtitle.ForeColor = Color.FromArgb(180, 200, 220);
            lblWelcome.ForeColor = Theme.TextPrimary;
            lblHistoryTitle.ForeColor = Theme.TextPrimary;
            lblPeriod.ForeColor = Theme.TextMuted;
            lblTypeFilter.ForeColor = Theme.TextMuted;
            lblSearch.ForeColor = Theme.TextMuted;

            // ФИКС: Жестко прячем блок "Доходы по категориям", чтобы он не занимал место
            if (lblStatsTitle != null) lblStatsTitle.Visible = false;
            if (lblIncomeSummary != null) lblIncomeSummary.Visible = false;

            lblCopyright.ForeColor = Theme.TextMuted;
            lblDataPath.ForeColor = Theme.TextMuted;

            StyleSidebarButton(btnIncome, "➕  Добавить доход");
            StyleSidebarButton(btnExpense, "➖  Добавить расход");
            StyleSidebarButton(btnExchange, "💱  Обмен валют");
            StyleSidebarButton(btnDebts, "📒  Мои долги");
            StyleSidebarButton(btnEditBalance, "✏  Изменить баланс");
            StyleSidebarButton(btnTheme, Theme.IsDark ? "☀  Светлая тема" : "🌙  Тёмная тема");

            cardPrb.BackColor = Theme.BgCardPrb;
            cardRub.BackColor = Theme.BgCardRub;
            cardUsd.BackColor = Theme.BgCardUsd;
            cardEur.BackColor = Theme.BgCardEur;

            lblPrbTitle.ForeColor = Theme.AccentGreen;
            lblRubTitle.ForeColor = Theme.AccentBlue;
            lblUsdTitle.ForeColor = Theme.AccentGold;
            lblEurTitle.ForeColor = Theme.AccentPurple;
            lblPrbAmount.ForeColor = Theme.TextPrimary;
            lblRubAmount.ForeColor = Theme.TextPrimary;
            lblUsdAmount.ForeColor = Theme.TextPrimary;
            lblEurAmount.ForeColor = Theme.TextPrimary;
            lblPrbAmount.Font = UiHelper.CardAmountFont;
            lblRubAmount.Font = UiHelper.CardAmountFont;
            lblUsdAmount.Font = UiHelper.CardAmountFont;
            lblEurAmount.Font = UiHelper.CardAmountFont;
            lblWelcome.Font = UiHelper.TitleFont;

            gridHistory.EnableHeadersVisualStyles = false;
            gridHistory.BackgroundColor = Theme.BgGrid;
            gridHistory.DefaultCellStyle.BackColor = Theme.BgGrid;
            gridHistory.DefaultCellStyle.ForeColor = Theme.TextPrimary;
            gridHistory.DefaultCellStyle.SelectionBackColor = Theme.SelectionBack;
            gridHistory.DefaultCellStyle.SelectionForeColor = Theme.TextPrimary;
            gridHistory.DefaultCellStyle.Font = UiHelper.BodyFont;
            gridHistory.AlternatingRowsDefaultCellStyle.BackColor = Theme.BgGridAlt;
            gridHistory.AlternatingRowsDefaultCellStyle.ForeColor = Theme.TextPrimary;
            gridHistory.ColumnHeadersDefaultCellStyle.BackColor = Theme.BgHeader;
            gridHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold);
            gridHistory.ColumnHeadersHeight = 38;
            gridHistory.GridColor = Theme.IsDark ? Color.FromArgb(55, 60, 70) : Color.FromArgb(220, 225, 230);

            cmbPeriod.BackColor = Theme.BgCard;
            cmbPeriod.ForeColor = Theme.TextPrimary;
            cmbTypeFilter.BackColor = Theme.BgCard;
            cmbTypeFilter.ForeColor = Theme.TextPrimary;
            txtSearch.BackColor = Theme.BgCard;
            txtSearch.ForeColor = Theme.TextPrimary;
            _dayFilter.BackColor = Theme.BgCard;
            _dayFilter.ForeColor = Theme.TextPrimary;
            _dayFilterLabel.ForeColor = Theme.TextMuted;

            UiHelper.StyleFlatButton(btnDailyExpenses, Theme.AccentBlue, Color.White);
            btnDailyExpenses.TextAlign = ContentAlignment.MiddleCenter;
            UiHelper.StyleFlatButton(btnPrintExpenses, Theme.AccentGold, Color.White);
            btnPrintExpenses.TextAlign = ContentAlignment.MiddleCenter;
        }

        private static void StyleSidebarButton(Button btn, string text)
        {
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Theme.IsDark
                ? Color.FromArgb(40, 50, 70)
                : Color.FromArgb(50, 80, 115);
            btn.BackColor = Color.Transparent;
            btn.ForeColor = Theme.TextOnSidebar;
            btn.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(12, 0, 4, 0);
            btn.Cursor = Cursors.Hand;
            btn.Height = 56;
        }

        private HistoryPeriod CurrentPeriod
        {
            get
            {
                switch (cmbPeriod.SelectedIndex)
                {
                    case 1: return HistoryPeriod.ThisMonth;
                    case 2: return HistoryPeriod.LastMonth;
                    default: return HistoryPeriod.AllTime;
                }
            }
        }

        private HistoryTypeFilter CurrentTypeFilter
        {
            get
            {
                switch (cmbTypeFilter.SelectedIndex)
                {
                    case 1: return HistoryTypeFilter.Income;
                    case 2: return HistoryTypeFilter.Expense;
                    case 3: return HistoryTypeFilter.Debts;
                    case 4: return HistoryTypeFilter.Exchange;
                    default: return HistoryTypeFilter.All;
                }
            }
        }

        private string PeriodTitle
        {
            get
            {
                if (ExactDay.HasValue)
                    return "За " + ExactDay.Value.ToString("dd.MM.yyyy");
                switch (cmbPeriod.SelectedIndex)
                {
                    case 1: return "Этот месяц";
                    case 2: return "Прошлый месяц";
                    default: return "За всё время";
                }
            }
        }

        private void RefreshDashboard()
        {
            if (_manager == null)
                return;

            var b = _manager.Balances;
            lblPrbAmount.Text = b.PRB.ToString("N2");
            lblRubAmount.Text = b.RUB.ToString("N2");
            lblUsdAmount.Text = b.USD.ToString("N2");
            lblEurAmount.Text = b.EUR.ToString("N2");
            lblWelcome.Text = "Привет, Юлия! Вот ваши финансы";
            lblDataPath.Text = "Файл данных: " + _manager.DataFilePath;
            lblCopyright.Text = "Банчу.Н&Белько.В© 2026";

            // ФИКС: Убрали вызов RefreshIncomeStats(), так как блок скрыт
            BindHistoryGrid();
            pieExpenses.SetData(_manager.GetExpenseTotalsByCategory(EffectivePeriod, ExactDay));
        }

        private void RefreshIncomeStats()
        {
            var categories = _manager.GetIncomeTotalsByCategory(EffectivePeriod, ExactDay);
            lblIncomeSummary.Text = categories.Count == 0
                ? "Доходы по категориям появятся после первой операции."
                : string.Join("  |  ", categories.Select(item => item.Category + ": " + item.CurrencyHint));
        }

        private void BindHistoryGrid()
        {
            IEnumerable<Transaction> query = _manager.GetTransactions(EffectivePeriod, ExactDay);

            switch (CurrentTypeFilter)
            {
                case HistoryTypeFilter.Income:
                    query = query.Where(t => t.Type == TransactionType.Income);
                    break;
                case HistoryTypeFilter.Expense:
                    query = query.Where(t => t.Type == TransactionType.Expense);
                    break;
                case HistoryTypeFilter.Debts:
                    query = query.Where(t =>
                        t.Type == TransactionType.DebtCreated ||
                        t.Type == TransactionType.DebtPaid);
                    break;
                case HistoryTypeFilter.Exchange:
                    query = query.Where(t => t.Type == TransactionType.Exchange);
                    break;
            }

            string search = (txtSearch.Text ?? string.Empty).Trim();
            if (search.Length > 0)
            {
                query = query.Where(t =>
                    ContainsIgnoreCase(t.Category, search) ||
                    ContainsIgnoreCase(t.Note, search) ||
                    ContainsIgnoreCase(t.Shop, search) ||
                    ContainsIgnoreCase(t.CategoryDisplay, search));
            }

            _filteredRows = query
                .Select(t => new HistoryRow
                {
                    Id = t.Id,
                    Type = t.Type,
                    Дата = t.Date.ToString("dd.MM.yyyy HH:mm"),
                    Тип = t.TypeDisplay,
                    Категория = t.CategoryDisplay,
                    Магазин = t.Shop ?? string.Empty,
                    Сумма = t.Amount > 0 ? t.Amount.ToString("N2") : "—",
                    Валюта = t.Amount > 0 ? t.CurrencyDisplay : "",
                    Заметка = t.Note
                })
                .ToList();

            _gridReady = false;
            gridHistory.DataSource = null;
            gridHistory.Columns.Clear();
            gridHistory.DataSource = _filteredRows;

            if (gridHistory.Columns["Id"] != null)
                gridHistory.Columns["Id"].Visible = false;
            if (gridHistory.Columns["Type"] != null)
                gridHistory.Columns["Type"].Visible = false;
            if (gridHistory.Columns["Магазин"] != null)
                gridHistory.Columns["Магазин"].Visible = false;

            ApplyGridColumnLayout();

            if (gridHistory.Columns["colDelete"] == null)
            {
                var colDel = new DataGridViewButtonColumn
                {
                    Name = "colDelete",
                    HeaderText = "",
                    Text = "🗑",
                    UseColumnTextForButtonValue = true,
                    Width = 48,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    FlatStyle = FlatStyle.Flat
                };
                gridHistory.Columns.Add(colDel);
            }

            if (gridHistory.Columns["Сумма"] != null)
                gridHistory.Columns["Сумма"].DefaultCellStyle.Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold);

            _gridReady = true;
        }

        private void ApplyGridColumnLayout()
        {
            foreach (DataGridViewColumn col in gridHistory.Columns)
            {
                if (col.Name == "Заметка")
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    col.MinimumWidth = 120;
                }
                else if (col.Name != "colDelete" && col.Visible)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }

        private static bool ContainsIgnoreCase(string haystack, string needle)
        {
            if (string.IsNullOrEmpty(haystack) || string.IsNullOrEmpty(needle))
                return false;
            return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private Guid? GetSelectedTransactionId()
        {
            if (gridHistory.CurrentRow == null || gridHistory.CurrentRow.Index < 0)
                return null;
            if (gridHistory.Columns["Id"] == null)
                return null;
            var val = gridHistory.CurrentRow.Cells["Id"].Value;
            if (val is Guid g) return g;
            return null;
        }

        private void gridHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!_gridReady || e.RowIndex < 0 || gridHistory.Columns[e.ColumnIndex].Name != "Сумма")
                return;

            var typeObj = gridHistory.Rows[e.RowIndex].Cells["Type"].Value;
            if (!(typeObj is TransactionType type))
                return;

            if (type == TransactionType.Income)
            {
                e.CellStyle.ForeColor = Theme.AccentGreen;
            }
            else if (type == TransactionType.Expense)
            {
                e.CellStyle.ForeColor = Theme.AccentRed;
            }
            else if (type == TransactionType.Exchange)
            {
                e.CellStyle.ForeColor = Theme.AccentGold;
            }
            else if (type == TransactionType.DebtPaid)
            {
                var note = Convert.ToString(gridHistory.Rows[e.RowIndex].Cells["Заметка"].Value) ?? "";
                e.CellStyle.ForeColor = note.IndexOf("Получено", StringComparison.OrdinalIgnoreCase) >= 0
                    ? Theme.AccentGreen
                    : Theme.AccentRed;
            }
            else
            {
                e.CellStyle.ForeColor = Theme.TextPrimary;
            }
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        }

        private void gridHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || gridHistory.Columns[e.ColumnIndex].Name != "colDelete")
                return;
            gridHistory.Rows[e.RowIndex].Selected = true;
            DeleteSelected();
        }

        private void menuDelete_Click(object sender, EventArgs e) => DeleteSelected();

        private void menuEdit_Click(object sender, EventArgs e) => EditSelected();

        private void DeleteSelected()
        {
            var id = GetSelectedTransactionId();
            if (id == null)
            {
                UiHelper.ShowError(this, "Выберите запись в таблице.");
                return;
            }

            if (MessageBox.Show(this,
                    "Удалить эту запись?\nБаланс будет пересчитан (где возможно).",
                    "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _manager.DeleteTransaction(id.Value);
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                UiHelper.ShowError(this, ex.Message);
            }
        }

        private void EditSelected()
        {
            var id = GetSelectedTransactionId();
            if (id == null)
            {
                UiHelper.ShowError(this, "Выберите запись в таблице.");
                return;
            }

            var tx = _manager.GetTransaction(id.Value);
            if (tx == null) return;

            if (tx.Type != TransactionType.Income && tx.Type != TransactionType.Expense)
            {
                UiHelper.ShowError(this,
                    "Редактировать можно доход или расход.\nДля обмена и долгов используйте удаление.");
                return;
            }

            using (var dlg = new EditTransactionForm(_manager, tx))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_manager != null)
                RefreshDashboard();
        }

        private void cmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_manager != null)
                BindHistoryGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_manager != null)
                BindHistoryGrid();
        }

        private void btnDailyExpenses_Click(object sender, EventArgs e)
        {
            var days = _manager.GetExpenseTotalsByDay(EffectivePeriod, ExactDay);
            using (var dlg = new DailyExpensesForm(days, "Расходы по дням — " + PeriodTitle))
                dlg.ShowDialog(this);
        }

        private DateTime? ExactDay => _dayFilter != null && _dayFilter.Checked ? _dayFilter.Value.Date : (DateTime?)null;

        private HistoryPeriod EffectivePeriod => ExactDay.HasValue ? HistoryPeriod.AllTime : CurrentPeriod;

        private void ConfigureExactDayFilter()
        {
            _dayFilterLabel = new Label
            {
                Text = "День:",
                AutoSize = true,
                Font = new Font("Segoe UI", 11F)
            };
            _dayFilter = new DateTimePicker
            {
                Name = "dtpExactDay",
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false,
                Width = 140, // ИСПРАВЛЕНО: Делаем календарь компактным
                Font = new Font("Segoe UI", 11F)
            };
            _dayFilter.ValueChanged += ExactDayValueChanged;
            _dayFilter.MouseUp += ExactDayFilterChanged;
            _dayFilter.KeyUp += ExactDayFilterChanged;
            panelFilter.Controls.Add(_dayFilterLabel);
            panelFilter.Controls.Add(_dayFilter);
        }


        private void ExactDayValueChanged(object sender, EventArgs e)
        {
            ExactDayFilterChanged(sender, e);
        }

        private void ExactDayFilterChanged(object sender, EventArgs e)
        {
            if (_manager != null)
                RefreshDashboard();
        }

        private void LayoutHistoryArea()
        {
            if (panelFilter == null || cmbPeriod == null || _dayFilter == null) return;

            int width = panelFilter.ClientSize.Width;

            // --- ВЕРХНИЙ РЯД ---
            cmbPeriod.Width = 185;
            _dayFilterLabel.Location = new Point(cmbPeriod.Right + 30, cmbPeriod.Top + 4);
            _dayFilter.Location = new Point(_dayFilterLabel.Right + 10, cmbPeriod.Top);

            // ФИКС КНОПКИ: Ставим кнопку "Расходы по дням" жестко справа
            btnDailyExpenses.Size = new Size(160, 36);
            btnDailyExpenses.Location = new Point(300, 240);

            // --- НИЖНИЙ РЯД ---
            lblTypeFilter.Location = new Point(0, 74);
            cmbTypeFilter.Location = new Point(105, 70);
            cmbTypeFilter.Width = 185;

            lblSearch.Location = new Point(cmbPeriod.Right + 30, 74);
            txtSearch.Location = new Point(_dayFilter.Left, 70);

            btnPrintExpenses.Size = new Size(160, 36);

            // Адаптация под узкий экран
            bool compact = width < 820;
            if (compact)
            {
                panelFilter.Height = 150;
                txtSearch.Width = Math.Max(120, width - txtSearch.Left - 10);

                // Кнопки уходят на третий ряд вниз
                btnDailyExpenses.Location = new Point(0, 110);
                btnPrintExpenses.Location = new Point(170, 110);
            }
            else
            {
                panelFilter.Height = 110;
                // Кнопка печати ставится прямо под кнопкой "Расходы по дням"
                btnPrintExpenses.Location = new Point(width - btnPrintExpenses.Width, 70);
                txtSearch.Width = Math.Max(150, btnPrintExpenses.Left - txtSearch.Left - 20);
            }

            panelContent.Top = panelFilter.Bottom + 10;
            panelContent.Height = Math.Max(120, panelFooter.Top - panelContent.Top - 10);
        }
        private void btnPrintExpenses_Click(object sender, EventArgs e)
        {
            var expenses = _filteredRows
                .Where(r => r.Type == TransactionType.Expense)
                .ToList();
            ExpensePrintHelper.ShowPreview(this, expenses, PeriodTitle);
        }

        private void btnTheme_Click(object sender, EventArgs e)
        {
            _manager.IsDarkTheme = !_manager.IsDarkTheme;
            Theme.Apply(_manager.IsDarkTheme);
            ApplyThemeToForm();
            RefreshDashboard();
        }

        private void btnEditBalance_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditBalanceForm(_manager))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void btnIncome_Click(object sender, EventArgs e)
        {
            using (var dlg = new TransactionDialogForm(_manager, true))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void btnExpense_Click(object sender, EventArgs e)
        {
            using (var dlg = new TransactionDialogForm(_manager, false))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    RefreshDashboard();
            }
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            using (var dlg = new ExchangeDialogForm(_manager))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshDashboard();
                    UiHelper.ShowInfo(this, "Обмен выполнен. Балансы обновлены.");
                }
            }
        }

        private void btnDebts_Click(object sender, EventArgs e)
        {
            using (var dlg = new DebtsForm(_manager))
            {
                dlg.ShowDialog(this);
                RefreshDashboard();
            }
        }
    }
}
