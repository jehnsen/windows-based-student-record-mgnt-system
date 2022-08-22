using System;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class ReportFiltersForm : Form
    {
        private ReportForm report;
        DbService dbService = new DbService();
        public ReportFiltersForm(ReportForm report)
        {
            InitializeComponent();

            this.report = report;
            this.GetCourseList();
        }

        private void GetCourseList()
        {
            dbService.GetDatatable("SELECT description FROM tblcourse");

            cboCourse.DataSource = dbService.dt;
            cboCourse.DisplayMember = "description";
            cboCourse.ValueMember = dbService.dt.Columns[0].ColumnName;

        }

        private void btnApplyFilters_Click(object sender, EventArgs e)
        {
            report.course = cboCourse.Text;
            report.semester = cboSemester.Text;
            report.schoolyear = cboSchoolYear.Text;
            this.Close();
        }
    }
}
