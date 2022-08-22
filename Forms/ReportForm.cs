using System;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace StudentRecordMgntSystem.Forms
{
    public partial class ReportForm : Form
    {
        DbService dbService = new DbService();
        ReportDocument reportdoc = new ReportDocument();

        public string course, semester, schoolyear;
        private int studentId;
        string query;

        public ReportForm()
        {
            InitializeComponent();
        }
        public ReportForm(int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
        }
        public ReportForm(string course, string semester, string schoolyear)
        {
            InitializeComponent();
            this.course = course;
            this.schoolyear = schoolyear;
            this.semester = semester;
        }

        private void LoadReport(string sqlQuery, string reportname, string dtName)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string strReportPath = appPath.Replace("bin\\Release\\", "Reports\\") + reportname + ".rpt";
                
                var dsData = dbService.ExecuteDataSet(sqlQuery, dtName);
                reportdoc.Load(strReportPath);
                reportdoc.SetDataSource(dsData);

                // set report parameter values
                if (reportname == "rptStudentMasterList")
                {
                    reportdoc.SetParameterValue("@Course", this.course);
                    reportdoc.SetParameterValue("@YearLevel", this.schoolyear);
                    reportdoc.SetParameterValue("@Semester", this.semester);
                }
  
                // load the crystalreport document
                crystalReportViewer1.ReportSource = reportdoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            this.Generate(cboSearchFilter.Text);
        }

        public void Generate(string report)
        {
            switch (report)
            {
                case "Student Masterlist":

                    using (ReportFiltersForm _filter = new ReportFiltersForm(this))
                    {
                        if (_filter.ShowDialog() == DialogResult.OK)
                        {
                            query = "SELECT * FROM view_studentenrollment WHERE course_description = '" + this.course + "' " +
                                "AND semester = '" + this.semester + "' AND school_year = '" + this.schoolyear + "'";

                            this.LoadReport(query, "rptStudentMasterList", "dtStudentEnrollment");
                        }
                    }
                    break;

                case "Student Profile":
                    query = "SELECT * FROM view_studentprofile WHERE id = '" + this.studentId + "'";
                    this.LoadReport(query, "rptStudentProfile", "dtStudentProfile");
                    break;

                case "Student Grades":
                    query = "SELECT * FROM view_studentprofile WHERE id = '" + this.studentId + "'";
                    this.LoadReport(query, "rptStudentGrades", "dtStudentProfile");
                    break;

                default:
                    break;
            }
        }
    }
}
