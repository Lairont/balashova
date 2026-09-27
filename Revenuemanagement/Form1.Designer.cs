namespace Revenuemanagement
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnTheme = new System.Windows.Forms.Button();
            this.btnEditBalance = new System.Windows.Forms.Button();
            this.btnDebts = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnExpense = new System.Windows.Forms.Button();
            this.btnIncome = new System.Windows.Forms.Button();
            this.lblAppSubtitle = new System.Windows.Forms.Label();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblDataPath = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelPieHost = new System.Windows.Forms.Panel();
            this.pieExpenses = new Revenuemanagement.ExpensePiePanel();
            this.gridHistory = new System.Windows.Forms.DataGridView();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.btnPrintExpenses = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cmbTypeFilter = new System.Windows.Forms.ComboBox();
            this.lblTypeFilter = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.panelStats = new System.Windows.Forms.Panel();
            this.btnDailyExpenses = new System.Windows.Forms.Button();
            this.lblIncomeSummary = new System.Windows.Forms.Label();
            this.lblStatsTitle = new System.Windows.Forms.Label();
            this.cardEur = new System.Windows.Forms.Panel();
            this.lblEurAmount = new System.Windows.Forms.Label();
            this.lblEurTitle = new System.Windows.Forms.Label();
            this.cardUsd = new System.Windows.Forms.Panel();
            this.lblUsdAmount = new System.Windows.Forms.Label();
            this.lblUsdTitle = new System.Windows.Forms.Label();
            this.cardRub = new System.Windows.Forms.Panel();
            this.lblRubAmount = new System.Windows.Forms.Label();
            this.lblRubTitle = new System.Windows.Forms.Label();
            this.cardPrb = new System.Windows.Forms.Panel();
            this.lblPrbAmount = new System.Windows.Forms.Label();
            this.lblPrbTitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.ctxHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSidebar.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelPieHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.cardEur.SuspendLayout();
            this.cardUsd.SuspendLayout();
            this.cardRub.SuspendLayout();
            this.cardPrb.SuspendLayout();
            this.ctxHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.Controls.Add(this.btnTheme);
            this.panelSidebar.Controls.Add(this.btnEditBalance);
            this.panelSidebar.Controls.Add(this.btnDebts);
            this.panelSidebar.Controls.Add(this.btnExchange);
            this.panelSidebar.Controls.Add(this.btnExpense);
            this.panelSidebar.Controls.Add(this.btnIncome);
            this.panelSidebar.Controls.Add(this.lblAppSubtitle);
            this.panelSidebar.Controls.Add(this.lblAppTitle);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(200, 700);
            this.panelSidebar.TabIndex = 0;
            // 
            // btnTheme
            // 
            this.btnTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTheme.Location = new System.Drawing.Point(12, 630);
            this.btnTheme.Name = "btnTheme";
            this.btnTheme.Size = new System.Drawing.Size(176, 48);
            this.btnTheme.TabIndex = 7;
            this.btnTheme.Text = "Тёмная тема";
            this.btnTheme.Click += new System.EventHandler(this.btnTheme_Click);
            // 
            // btnEditBalance
            // 
            this.btnEditBalance.Location = new System.Drawing.Point(12, 370);
            this.btnEditBalance.Name = "btnEditBalance";
            this.btnEditBalance.Size = new System.Drawing.Size(176, 56);
            this.btnEditBalance.TabIndex = 6;
            this.btnEditBalance.Text = "Изменить баланс";
            this.btnEditBalance.Click += new System.EventHandler(this.btnEditBalance_Click);
            // 
            // btnDebts
            // 
            this.btnDebts.Location = new System.Drawing.Point(12, 300);
            this.btnDebts.Name = "btnDebts";
            this.btnDebts.Size = new System.Drawing.Size(176, 56);
            this.btnDebts.TabIndex = 5;
            this.btnDebts.Text = "Мои долги";
            this.btnDebts.Click += new System.EventHandler(this.btnDebts_Click);
            // 
            // btnExchange
            // 
            this.btnExchange.Location = new System.Drawing.Point(12, 230);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(176, 56);
            this.btnExchange.TabIndex = 4;
            this.btnExchange.Text = "Обмен валют";
            this.btnExchange.Click += new System.EventHandler(this.btnExchange_Click);
            // 
            // btnExpense
            // 
            this.btnExpense.Location = new System.Drawing.Point(12, 160);
            this.btnExpense.Name = "btnExpense";
            this.btnExpense.Size = new System.Drawing.Size(176, 56);
            this.btnExpense.TabIndex = 3;
            this.btnExpense.Text = "Добавить расход";
            this.btnExpense.Click += new System.EventHandler(this.btnExpense_Click);
            // 
            // btnIncome
            // 
            this.btnIncome.Location = new System.Drawing.Point(12, 90);
            this.btnIncome.Name = "btnIncome";
            this.btnIncome.Size = new System.Drawing.Size(176, 56);
            this.btnIncome.TabIndex = 2;
            this.btnIncome.Text = "Добавить доход";
            this.btnIncome.Click += new System.EventHandler(this.btnIncome_Click);
            // 
            // lblAppSubtitle
            // 
            this.lblAppSubtitle.AutoSize = true;
            this.lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAppSubtitle.Location = new System.Drawing.Point(18, 52);
            this.lblAppSubtitle.Name = "lblAppSubtitle";
            this.lblAppSubtitle.Size = new System.Drawing.Size(170, 19);
            this.lblAppSubtitle.TabIndex = 1;
            this.lblAppSubtitle.Text = "Личные финансы Юлии";
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.Location = new System.Drawing.Point(16, 18);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(149, 30);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "💰 Кошелёк";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelContent);
            this.panelMain.Controls.Add(this.panelFilter);
            this.panelMain.Controls.Add(this.panelStats);
            this.panelMain.Controls.Add(this.cardEur);
            this.panelMain.Controls.Add(this.cardUsd);
            this.panelMain.Controls.Add(this.cardRub);
            this.panelMain.Controls.Add(this.cardPrb);
            this.panelMain.Controls.Add(this.lblWelcome);
            this.panelMain.Controls.Add(this.panelFooter);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(200, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(920, 700);
            this.panelMain.TabIndex = 1;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.lblCopyright);
            this.panelFooter.Controls.Add(this.lblDataPath);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 660);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(860, 40);
            this.panelFooter.TabIndex = 10;
            // 
            // lblCopyright
            // 
            this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCopyright.Location = new System.Drawing.Point(0, 0);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(860, 22);
            this.lblCopyright.TabIndex = 0;
            this.lblCopyright.Text = "Банчу.Н&Белько.В© 2026";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDataPath
            // 
            this.lblDataPath.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDataPath.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblDataPath.Location = new System.Drawing.Point(0, 22);
            this.lblDataPath.Name = "lblDataPath";
            this.lblDataPath.Size = new System.Drawing.Size(860, 18);
            this.lblDataPath.TabIndex = 1;
            this.lblDataPath.Text = "Файл данных:";
            this.lblDataPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelContent
            // 
            this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // Важно: сначала Fill (таблица), потом Right (график) —
            // при Dock верхний по z-order контрол стыкуется первым.
            this.panelContent.Controls.Add(this.gridHistory);
            this.panelContent.Controls.Add(this.panelPieHost);
            this.panelContent.Location = new System.Drawing.Point(20, 318);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(880, 332);
            this.panelContent.TabIndex = 9;
            // 
            // panelPieHost — отдельная правая колонка, таблица Fill не залезает под неё
            // 
            this.panelPieHost.Controls.Add(this.pieExpenses);
            this.panelPieHost.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelPieHost.Location = new System.Drawing.Point(640, 0);
            this.panelPieHost.Name = "panelPieHost";
            this.panelPieHost.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.panelPieHost.Size = new System.Drawing.Size(240, 332);
            this.panelPieHost.TabIndex = 1;
            // 
            // pieExpenses
            // 
            this.pieExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieExpenses.Location = new System.Drawing.Point(12, 0);
            this.pieExpenses.Name = "pieExpenses";
            this.pieExpenses.Size = new System.Drawing.Size(228, 332);
            this.pieExpenses.TabIndex = 0;
            // 
            // gridHistory
            // 
            this.gridHistory.AllowUserToAddRows = false;
            this.gridHistory.AllowUserToDeleteRows = false;
            this.gridHistory.BackgroundColor = System.Drawing.Color.White;
            this.gridHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridHistory.ContextMenuStrip = this.ctxHistory;
            this.gridHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridHistory.Location = new System.Drawing.Point(0, 0);
            this.gridHistory.MultiSelect = false;
            this.gridHistory.Name = "gridHistory";
            this.gridHistory.ReadOnly = true;
            this.gridHistory.RowHeadersVisible = false;
            this.gridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridHistory.Size = new System.Drawing.Size(580, 332);
            this.gridHistory.TabIndex = 0;
            this.gridHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridHistory_CellContentClick);
            this.gridHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridHistory_CellFormatting);
            // 
            // panelFilter
            // 
            this.panelFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFilter.Controls.Add(this.btnPrintExpenses);
            this.panelFilter.Controls.Add(this.txtSearch);
            this.panelFilter.Controls.Add(this.lblSearch);
            this.panelFilter.Controls.Add(this.cmbTypeFilter);
            this.panelFilter.Controls.Add(this.lblTypeFilter);
            this.panelFilter.Controls.Add(this.cmbPeriod);
            this.panelFilter.Controls.Add(this.lblPeriod);
            this.panelFilter.Controls.Add(this.lblHistoryTitle);
            this.panelFilter.Location = new System.Drawing.Point(20, 242);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(880, 68);
            this.panelFilter.TabIndex = 8;
            // 
            // btnPrintExpenses
            // 
            this.btnPrintExpenses.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrintExpenses.Location = new System.Drawing.Point(680, 34);
            this.btnPrintExpenses.Name = "btnPrintExpenses";
            this.btnPrintExpenses.Size = new System.Drawing.Size(140, 28);
            this.btnPrintExpenses.TabIndex = 7;
            this.btnPrintExpenses.Text = "🖨  Печать расходов";
            this.btnPrintExpenses.UseVisualStyleBackColor = true;
            this.btnPrintExpenses.Click += new System.EventHandler(this.btnPrintExpenses_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearch.Location = new System.Drawing.Point(420, 34);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(250, 27);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSearch.Location = new System.Drawing.Point(350, 38);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(52, 20);
            this.lblSearch.TabIndex = 5;
            this.lblSearch.Text = "Поиск:";
            // 
            // cmbTypeFilter
            // 
            this.cmbTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeFilter.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbTypeFilter.FormattingEnabled = true;
            this.cmbTypeFilter.Items.AddRange(new object[] {
            "Все",
            "Доходы",
            "Расходы",
            "Долги",
            "Обмен валют"});
            this.cmbTypeFilter.Location = new System.Drawing.Point(170, 34);
            this.cmbTypeFilter.Name = "cmbTypeFilter";
            this.cmbTypeFilter.Size = new System.Drawing.Size(160, 28);
            this.cmbTypeFilter.TabIndex = 4;
            this.cmbTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cmbTypeFilter_SelectedIndexChanged);
            // 
            // lblTypeFilter
            // 
            this.lblTypeFilter.AutoSize = true;
            this.lblTypeFilter.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTypeFilter.Location = new System.Drawing.Point(0, 38);
            this.lblTypeFilter.Name = "lblTypeFilter";
            this.lblTypeFilter.Size = new System.Drawing.Size(72, 20);
            this.lblTypeFilter.TabIndex = 3;
            this.lblTypeFilter.Text = "Показать:";
            // 
            // panelStats
            // 
            this.panelStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStats.Controls.Add(this.btnDailyExpenses);
            this.panelStats.Controls.Add(this.lblIncomeSummary);
            this.panelStats.Controls.Add(this.lblStatsTitle);
            this.panelStats.Location = new System.Drawing.Point(20, 182);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(880, 52);
            this.panelStats.TabIndex = 11;
            // 
            // btnDailyExpenses
            // 
            this.btnDailyExpenses.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnDailyExpenses.Location = new System.Drawing.Point(640, 10);
            this.btnDailyExpenses.Name = "btnDailyExpenses";
            this.btnDailyExpenses.Size = new System.Drawing.Size(180, 32);
            this.btnDailyExpenses.TabIndex = 4;
            this.btnDailyExpenses.Text = "📅  Расходы по дням";
            this.btnDailyExpenses.UseVisualStyleBackColor = true;
            this.btnDailyExpenses.Click += new System.EventHandler(this.btnDailyExpenses_Click);
            // 
            // lblIncomeSummary
            // 
            this.lblIncomeSummary.AutoEllipsis = true;
            this.lblIncomeSummary.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIncomeSummary.Location = new System.Drawing.Point(0, 28);
            this.lblIncomeSummary.Name = "lblIncomeSummary";
            this.lblIncomeSummary.Size = new System.Drawing.Size(620, 20);
            this.lblIncomeSummary.TabIndex = 1;
            this.lblIncomeSummary.Text = "Доходы по категориям появятся после первой операции.";
            // 
            // lblStatsTitle
            // 
            this.lblStatsTitle.AutoSize = true;
            this.lblStatsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatsTitle.Location = new System.Drawing.Point(0, 4);
            this.lblStatsTitle.Name = "lblStatsTitle";
            this.lblStatsTitle.Size = new System.Drawing.Size(145, 19);
            this.lblStatsTitle.TabIndex = 0;
            this.lblStatsTitle.Text = "Доходы по категориям:";
            // 
            // cardEur
            // 
            this.cardEur.Controls.Add(this.lblEurAmount);
            this.cardEur.Controls.Add(this.lblEurTitle);
            this.cardEur.Location = new System.Drawing.Point(620, 64);
            this.cardEur.Name = "cardEur";
            this.cardEur.Size = new System.Drawing.Size(190, 110);
            this.cardEur.TabIndex = 12;
            // 
            // lblEurAmount
            // 
            this.lblEurAmount.AutoSize = true;
            this.lblEurAmount.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblEurAmount.Location = new System.Drawing.Point(16, 48);
            this.lblEurAmount.Name = "lblEurAmount";
            this.lblEurAmount.Size = new System.Drawing.Size(74, 37);
            this.lblEurAmount.TabIndex = 1;
            this.lblEurAmount.Text = "0.00";
            // 
            // lblEurTitle
            // 
            this.lblEurTitle.AutoSize = true;
            this.lblEurTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblEurTitle.Location = new System.Drawing.Point(16, 16);
            this.lblEurTitle.Name = "lblEurTitle";
            this.lblEurTitle.Size = new System.Drawing.Size(103, 21);
            this.lblEurTitle.TabIndex = 0;
            this.lblEurTitle.Text = "Евро (EUR €)";
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Items.AddRange(new object[] {
            "За всё время",
            "Этот месяц",
            "Прошлый месяц"});
            this.cmbPeriod.Location = new System.Drawing.Point(280, 4);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(200, 28);
            this.cmbPeriod.TabIndex = 2;
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPeriod.Location = new System.Drawing.Point(210, 8);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(64, 20);
            this.lblPeriod.TabIndex = 1;
            this.lblPeriod.Text = "Период:";
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.AutoSize = true;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblHistoryTitle.Location = new System.Drawing.Point(0, 6);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Size = new System.Drawing.Size(185, 25);
            this.lblHistoryTitle.TabIndex = 0;
            this.lblHistoryTitle.Text = "История операций";
            // 
            // cardUsd
            // 
            this.cardUsd.Controls.Add(this.lblUsdAmount);
            this.cardUsd.Controls.Add(this.lblUsdTitle);
            this.cardUsd.Location = new System.Drawing.Point(420, 64);
            this.cardUsd.Name = "cardUsd";
            this.cardUsd.Size = new System.Drawing.Size(190, 110);
            this.cardUsd.TabIndex = 3;
            // 
            // lblUsdAmount
            // 
            this.lblUsdAmount.AutoSize = true;
            this.lblUsdAmount.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblUsdAmount.Location = new System.Drawing.Point(16, 48);
            this.lblUsdAmount.Name = "lblUsdAmount";
            this.lblUsdAmount.Size = new System.Drawing.Size(74, 37);
            this.lblUsdAmount.TabIndex = 1;
            this.lblUsdAmount.Text = "0.00";
            // 
            // lblUsdTitle
            // 
            this.lblUsdTitle.AutoSize = true;
            this.lblUsdTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblUsdTitle.Location = new System.Drawing.Point(16, 16);
            this.lblUsdTitle.Name = "lblUsdTitle";
            this.lblUsdTitle.Size = new System.Drawing.Size(152, 21);
            this.lblUsdTitle.TabIndex = 0;
            this.lblUsdTitle.Text = "Доллары США ($)";
            // 
            // cardRub
            // 
            this.cardRub.Controls.Add(this.lblRubAmount);
            this.cardRub.Controls.Add(this.lblRubTitle);
            this.cardRub.Location = new System.Drawing.Point(220, 64);
            this.cardRub.Name = "cardRub";
            this.cardRub.Size = new System.Drawing.Size(190, 110);
            this.cardRub.TabIndex = 2;
            // 
            // lblRubAmount
            // 
            this.lblRubAmount.AutoSize = true;
            this.lblRubAmount.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblRubAmount.Location = new System.Drawing.Point(16, 48);
            this.lblRubAmount.Name = "lblRubAmount";
            this.lblRubAmount.Size = new System.Drawing.Size(74, 37);
            this.lblRubAmount.TabIndex = 1;
            this.lblRubAmount.Text = "0.00";
            // 
            // lblRubTitle
            // 
            this.lblRubTitle.AutoSize = true;
            this.lblRubTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblRubTitle.Location = new System.Drawing.Point(16, 16);
            this.lblRubTitle.Name = "lblRubTitle";
            this.lblRubTitle.Size = new System.Drawing.Size(124, 21);
            this.lblRubTitle.TabIndex = 0;
            this.lblRubTitle.Text = "Рубли РФ (RUB)";
            // 
            // cardPrb
            // 
            this.cardPrb.Controls.Add(this.lblPrbAmount);
            this.cardPrb.Controls.Add(this.lblPrbTitle);
            this.cardPrb.Location = new System.Drawing.Point(20, 64);
            this.cardPrb.Name = "cardPrb";
            this.cardPrb.Size = new System.Drawing.Size(190, 110);
            this.cardPrb.TabIndex = 1;
            // 
            // lblPrbAmount
            // 
            this.lblPrbAmount.AutoSize = true;
            this.lblPrbAmount.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblPrbAmount.Location = new System.Drawing.Point(16, 48);
            this.lblPrbAmount.Name = "lblPrbAmount";
            this.lblPrbAmount.Size = new System.Drawing.Size(74, 37);
            this.lblPrbAmount.TabIndex = 1;
            this.lblPrbAmount.Text = "0.00";
            // 
            // lblPrbTitle
            // 
            this.lblPrbTitle.AutoSize = true;
            this.lblPrbTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblPrbTitle.Location = new System.Drawing.Point(16, 16);
            this.lblPrbTitle.Name = "lblPrbTitle";
            this.lblPrbTitle.Size = new System.Drawing.Size(133, 21);
            this.lblPrbTitle.TabIndex = 0;
            this.lblPrbTitle.Text = "Рубли ПМР (PRB)";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(20, 18);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(352, 30);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Привет, Юлия! Вот ваши финансы";
            // 
            // ctxHistory
            // 
            this.ctxHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEdit,
            this.menuDelete});
            this.ctxHistory.Name = "ctxHistory";
            this.ctxHistory.Size = new System.Drawing.Size(180, 48);
            // 
            // menuEdit
            // 
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(179, 22);
            this.menuEdit.Text = "✏ Изменить...";
            this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(179, 22);
            this.menuDelete.Text = "🗑 Удалить";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 700);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSidebar);
            this.MinimumSize = new System.Drawing.Size(1000, 640);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Кошелёк Юлии — учёт финансов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelSidebar.ResumeLayout(false);
            this.panelSidebar.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelPieHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).EndInit();
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelStats.PerformLayout();
            this.cardEur.ResumeLayout(false);
            this.cardEur.PerformLayout();
            this.cardUsd.ResumeLayout(false);
            this.cardUsd.PerformLayout();
            this.cardRub.ResumeLayout(false);
            this.cardRub.PerformLayout();
            this.cardPrb.ResumeLayout(false);
            this.cardPrb.PerformLayout();
            this.ctxHistory.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Button btnTheme;
        private System.Windows.Forms.Button btnEditBalance;
        private System.Windows.Forms.Button btnDebts;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Button btnExpense;
        private System.Windows.Forms.Button btnIncome;
        private System.Windows.Forms.Label lblAppSubtitle;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblDataPath;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelPieHost;
        private ExpensePiePanel pieExpenses;
        private System.Windows.Forms.DataGridView gridHistory;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Button btnPrintExpenses;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cmbTypeFilter;
        private System.Windows.Forms.Label lblTypeFilter;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Button btnDailyExpenses;
        private System.Windows.Forms.Label lblIncomeSummary;
        private System.Windows.Forms.Label lblStatsTitle;
        private System.Windows.Forms.Panel cardEur;
        private System.Windows.Forms.Label lblEurAmount;
        private System.Windows.Forms.Label lblEurTitle;
        private System.Windows.Forms.Panel cardUsd;
        private System.Windows.Forms.Label lblUsdAmount;
        private System.Windows.Forms.Label lblUsdTitle;
        private System.Windows.Forms.Panel cardRub;
        private System.Windows.Forms.Label lblRubAmount;
        private System.Windows.Forms.Label lblRubTitle;
        private System.Windows.Forms.Panel cardPrb;
        private System.Windows.Forms.Label lblPrbAmount;
        private System.Windows.Forms.Label lblPrbTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.ContextMenuStrip ctxHistory;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
    }
}
