using StudentRecordMgntSystem.Models;
using System;
using System.Data;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class CourseSelectionForm : Form
    {
        DbService dbService = new DbService();

        private StudentEnrollmentDetailsForm _parentForm;
        public CourseSelectionForm(StudentEnrollmentDetailsForm _f)
        {
            InitializeComponent();
            this._parentForm = _f;

            this.GetCourses(txtSearch.Text);
        }

        private void GetCourses(string searchKey)
        {
            string sqlQuery;
            if (searchKey.Length > 0)
            {
                sqlQuery = "SELECT * FROM tblcourse WHERE description LIKE '%" + searchKey + "%' OR code LIKE '%" + searchKey + "%' LIMIT 50";            }
            else
            {
                sqlQuery = "SELECT * FROM tblcourse";
            }
            var result = dbService.GetDatatable(sqlQuery);

            lvCourses.Items.Clear();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow dr = result.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["code"].ToString());
                listitem.SubItems.Add(dr["description"].ToString());
                lvCourses.Items.Add(listitem);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            this.GetCourses(txtSearch.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _parentForm.courseId = int.Parse(lvCourses.SelectedItems[0].Text.ToString());
            _parentForm.courseCode = lvCourses.SelectedItems[0].SubItems[1].Text.ToString();
            _parentForm.courseDescription = lvCourses.SelectedItems[0].SubItems[2].Text.ToString();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvCourses_DoubleClick(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }
    }
}
