
namespace ScreenerWFP
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EntryTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.MainSearchTxt = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ScreenerDropdown = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ShowCurrentVisitorsChk = new System.Windows.Forms.CheckBox();
            this.AddEntryPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AddEssentialVisitorBtn = new System.Windows.Forms.Button();
            this.AddEssentialCaregiverBtn = new System.Windows.Forms.Button();
            this.AddEmployeeBtn = new System.Windows.Forms.Button();
            this.AddESPBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.EntryTable = new System.Windows.Forms.DataGridView();
            this.panel2.SuspendLayout();
            this.AddEntryPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EntryTable)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryTypeComboBox
            // 
            this.EntryTypeComboBox.CausesValidation = false;
            this.EntryTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EntryTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.EntryTypeComboBox.FormattingEnabled = true;
            this.EntryTypeComboBox.Items.AddRange(new object[] {
            "All",
            "Employees",
            "Essential Service Providers",
            "Essential Caregivers",
            "Essential Visitors"});
            this.EntryTypeComboBox.Location = new System.Drawing.Point(86, 4);
            this.EntryTypeComboBox.Name = "EntryTypeComboBox";
            this.EntryTypeComboBox.Size = new System.Drawing.Size(280, 33);
            this.EntryTypeComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Show:";
            // 
            // SearchBtn
            // 
            this.SearchBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchBtn.Location = new System.Drawing.Point(86, 43);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(280, 34);
            this.SearchBtn.TabIndex = 3;
            this.SearchBtn.Text = "Display";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // MainSearchTxt
            // 
            this.MainSearchTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainSearchTxt.Location = new System.Drawing.Point(753, 4);
            this.MainSearchTxt.Name = "MainSearchTxt";
            this.MainSearchTxt.Size = new System.Drawing.Size(257, 30);
            this.MainSearchTxt.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.ScreenerDropdown);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.ShowCurrentVisitorsChk);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.EntryTypeComboBox);
            this.panel2.Controls.Add(this.SearchBtn);
            this.panel2.Controls.Add(this.MainSearchTxt);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1902, 80);
            this.panel2.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1570, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 25);
            this.label3.TabIndex = 19;
            this.label3.Text = "Screener:";
            // 
            // ScreenerDropdown
            // 
            this.ScreenerDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScreenerDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScreenerDropdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScreenerDropdown.FormattingEnabled = true;
            this.ScreenerDropdown.Items.AddRange(new object[] {
            "Jack Murray",
            "Isabelle O\'Meara",
            "Kaitlyn Adair",
            "Grace Faulds",
            "Nolan Something"});
            this.ScreenerDropdown.Location = new System.Drawing.Point(1674, 7);
            this.ScreenerDropdown.Name = "ScreenerDropdown";
            this.ScreenerDropdown.Size = new System.Drawing.Size(221, 33);
            this.ScreenerDropdown.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(753, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(257, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Advanced Search (New window)";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSearch.Location = new System.Drawing.Point(1017, 4);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 33);
            this.BtnSearch.TabIndex = 16;
            this.BtnSearch.Text = "Go";
            this.BtnSearch.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(672, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 25);
            this.label2.TabIndex = 15;
            this.label2.Text = "Search";
            // 
            // ShowCurrentVisitorsChk
            // 
            this.ShowCurrentVisitorsChk.AutoSize = true;
            this.ShowCurrentVisitorsChk.Checked = true;
            this.ShowCurrentVisitorsChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowCurrentVisitorsChk.Location = new System.Drawing.Point(373, 4);
            this.ShowCurrentVisitorsChk.Name = "ShowCurrentVisitorsChk";
            this.ShowCurrentVisitorsChk.Size = new System.Drawing.Size(200, 21);
            this.ShowCurrentVisitorsChk.TabIndex = 14;
            this.ShowCurrentVisitorsChk.Text = "Show only currently visiting";
            this.ShowCurrentVisitorsChk.UseVisualStyleBackColor = true;
            // 
            // AddEntryPanel
            // 
            this.AddEntryPanel.Controls.Add(this.AddEssentialVisitorBtn);
            this.AddEntryPanel.Controls.Add(this.AddEssentialCaregiverBtn);
            this.AddEntryPanel.Controls.Add(this.AddEmployeeBtn);
            this.AddEntryPanel.Controls.Add(this.AddESPBtn);
            this.AddEntryPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AddEntryPanel.Location = new System.Drawing.Point(0, 875);
            this.AddEntryPanel.Name = "AddEntryPanel";
            this.AddEntryPanel.Size = new System.Drawing.Size(1902, 78);
            this.AddEntryPanel.TabIndex = 32;
            // 
            // AddEssentialVisitorBtn
            // 
            this.AddEssentialVisitorBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AddEssentialVisitorBtn.AutoSize = true;
            this.AddEssentialVisitorBtn.Location = new System.Drawing.Point(1, 1);
            this.AddEssentialVisitorBtn.Margin = new System.Windows.Forms.Padding(1);
            this.AddEssentialVisitorBtn.Name = "AddEssentialVisitorBtn";
            this.AddEssentialVisitorBtn.Size = new System.Drawing.Size(488, 76);
            this.AddEssentialVisitorBtn.TabIndex = 0;
            this.AddEssentialVisitorBtn.Text = "NEW ESSENTIAL VISITOR";
            this.AddEssentialVisitorBtn.UseVisualStyleBackColor = true;
            this.AddEssentialVisitorBtn.Click += new System.EventHandler(this.AddEntryBtn_Click);
            // 
            // AddEssentialCaregiverBtn
            // 
            this.AddEssentialCaregiverBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AddEssentialCaregiverBtn.AutoSize = true;
            this.AddEssentialCaregiverBtn.Location = new System.Drawing.Point(491, 1);
            this.AddEssentialCaregiverBtn.Margin = new System.Windows.Forms.Padding(1);
            this.AddEssentialCaregiverBtn.Name = "AddEssentialCaregiverBtn";
            this.AddEssentialCaregiverBtn.Size = new System.Drawing.Size(476, 76);
            this.AddEssentialCaregiverBtn.TabIndex = 2;
            this.AddEssentialCaregiverBtn.Text = "NEW ESSENTIAL CAREGIVER";
            this.AddEssentialCaregiverBtn.UseVisualStyleBackColor = true;
            this.AddEssentialCaregiverBtn.Click += new System.EventHandler(this.AddEntryBtn_Click);
            // 
            // AddEmployeeBtn
            // 
            this.AddEmployeeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AddEmployeeBtn.AutoSize = true;
            this.AddEmployeeBtn.Location = new System.Drawing.Point(969, 1);
            this.AddEmployeeBtn.Margin = new System.Windows.Forms.Padding(1);
            this.AddEmployeeBtn.Name = "AddEmployeeBtn";
            this.AddEmployeeBtn.Size = new System.Drawing.Size(436, 77);
            this.AddEmployeeBtn.TabIndex = 1;
            this.AddEmployeeBtn.Text = "NEW EMPLOYEE";
            this.AddEmployeeBtn.UseVisualStyleBackColor = true;
            this.AddEmployeeBtn.Click += new System.EventHandler(this.AddEntryBtn_Click);
            // 
            // AddESPBtn
            // 
            this.AddESPBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AddESPBtn.AutoSize = true;
            this.AddESPBtn.Location = new System.Drawing.Point(1407, 1);
            this.AddESPBtn.Margin = new System.Windows.Forms.Padding(1);
            this.AddESPBtn.Name = "AddESPBtn";
            this.AddESPBtn.Size = new System.Drawing.Size(488, 77);
            this.AddESPBtn.TabIndex = 3;
            this.AddESPBtn.Text = "NEW ESSENTIAL SERVICE PROVIDER";
            this.AddESPBtn.UseVisualStyleBackColor = true;
            this.AddESPBtn.Click += new System.EventHandler(this.AddEntryBtn_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.EntryTable);
            this.panel1.Controls.Add(this.AddEntryPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1902, 953);
            this.panel1.TabIndex = 31;
            // 
            // EntryTable
            // 
            this.EntryTable.AllowUserToOrderColumns = true;
            this.EntryTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.EntryTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EntryTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntryTable.Location = new System.Drawing.Point(0, 0);
            this.EntryTable.Name = "EntryTable";
            this.EntryTable.RowHeadersWidth = 4;
            this.EntryTable.RowTemplate.Height = 24;
            this.EntryTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EntryTable.Size = new System.Drawing.Size(1902, 875);
            this.EntryTable.TabIndex = 33;
            this.EntryTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LeaveBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "MainForm";
            this.Text = "Screener Helper";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.AddEntryPanel.ResumeLayout(false);
            this.AddEntryPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EntryTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox EntryTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.TextBox MainSearchTxt;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ShowCurrentVisitorsChk;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox ScreenerDropdown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel AddEntryPanel;
        private System.Windows.Forms.Button AddEssentialVisitorBtn;
        private System.Windows.Forms.Button AddEssentialCaregiverBtn;
        private System.Windows.Forms.Button AddEmployeeBtn;
        private System.Windows.Forms.Button AddESPBtn;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.DataGridView EntryTable;
    }
}

