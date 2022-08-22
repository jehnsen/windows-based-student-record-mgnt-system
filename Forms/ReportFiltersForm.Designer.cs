namespace StudentRecordMgntSystem.Forms
{
    partial class ReportFiltersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportFiltersForm));
            this.cboCourse = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboSemester = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSchoolYear = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnApplyFilters = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboCourse
            // 
            this.cboCourse.FormattingEnabled = true;
            this.cboCourse.Items.AddRange(new object[] {
            "Father",
            "Mother",
            "Brother",
            "Sister",
            "Cousin",
            "Uncle",
            "Aunt",
            "Jowa"});
            this.cboCourse.Location = new System.Drawing.Point(101, 8);
            this.cboCourse.Name = "cboCourse";
            this.cboCourse.Size = new System.Drawing.Size(502, 24);
            this.cboCourse.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 36;
            this.label1.Text = "Course: ";
            // 
            // cboSemester
            // 
            this.cboSemester.FormattingEnabled = true;
            this.cboSemester.Items.AddRange(new object[] {
            "1st Semester",
            "2nd Semester"});
            this.cboSemester.Location = new System.Drawing.Point(101, 38);
            this.cboSemester.Name = "cboSemester";
            this.cboSemester.Size = new System.Drawing.Size(263, 24);
            this.cboSemester.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 38;
            this.label2.Text = "Semester: ";
            // 
            // cboSchoolYear
            // 
            this.cboSchoolYear.FormattingEnabled = true;
            this.cboSchoolYear.Items.AddRange(new object[] {
            "2021-2022",
            "2022-2023",
            "2023-2024"});
            this.cboSchoolYear.Location = new System.Drawing.Point(101, 68);
            this.cboSchoolYear.Name = "cboSchoolYear";
            this.cboSchoolYear.Size = new System.Drawing.Size(263, 24);
            this.cboSchoolYear.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 40;
            this.label3.Text = "School Year: ";
            // 
            // btnApplyFilters
            // 
            this.btnApplyFilters.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApplyFilters.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyFilters.Image")));
            this.btnApplyFilters.Location = new System.Drawing.Point(395, 41);
            this.btnApplyFilters.Name = "btnApplyFilters";
            this.btnApplyFilters.Size = new System.Drawing.Size(208, 53);
            this.btnApplyFilters.TabIndex = 42;
            this.btnApplyFilters.Text = "Apply Report Filter";
            this.btnApplyFilters.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApplyFilters.UseVisualStyleBackColor = true;
            this.btnApplyFilters.Click += new System.EventHandler(this.btnApplyFilters_Click);
            // 
            // frmReportFilters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 102);
            this.Controls.Add(this.btnApplyFilters);
            this.Controls.Add(this.cboSchoolYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboSemester);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboCourse);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmReportFilters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Filters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboCourse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSemester;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSchoolYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnApplyFilters;
    }
}