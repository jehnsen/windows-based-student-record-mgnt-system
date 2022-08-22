using System;
using System.Data;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class SubjectSelectionForm : Form
    {
        DbService dbService = new DbService();
        private int studentId;
        private int courseId;

        string sqlQuery;
        public SubjectSelectionForm(int studentId, int courseId, string courseCode)
        {
            InitializeComponent();

            this.studentId = studentId;
            this.courseId = courseId;

            this.GetSubjects(txtSearch.Text);

            Text = courseCode + " - Select Subjects";
            txtSearch.Focus();
        }
       
        private void GetSubjects(string searchKey)
        {
            if (searchKey.Length > 0)
            {
                sqlQuery = "SELECT * FROM view_subject WHERE description LIKE '%" + searchKey + "%' LIMIT 50";
            }
            else
            {
                sqlQuery = "SELECT * FROM view_subject";
            }
            var result = dbService.GetDatatable(sqlQuery);

            lvSubjects.Items.Clear();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow dr = result.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["course_code"].ToString());
                listitem.SubItems.Add(dr["description"].ToString());
                listitem.SubItems.Add(dr["lecture_unit"].ToString());
                listitem.SubItems.Add(dr["lab_unit"].ToString());
                listitem.SubItems.Add(dr["instructor_fullname"].ToString());
                lvSubjects.Items.Add(listitem);

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            this.GetSubjects(txtSearch.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var _isExist = this.CheckDuplicateEntry(this.studentId, int.Parse(lvSubjects.SelectedItems[0].Text.ToString()));
            if (_isExist)
            {
                MessageBox.Show("Subject already selected!", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // if subject is not yet selected, add to the list
            sqlQuery = "INSERT INTO tblstudent_subjects ( `student_id`,`subject_id`,`final_grade`,`remarks`,`status`) " +
                       "VALUES ('"+ this.studentId + "', '"+ lvSubjects.SelectedItems[0].Text +"', 0.0, 0, 1)";

            dbService.ExecuteQuery(sqlQuery);
            StudentEnrollmentDetailsForm f = new StudentEnrollmentDetailsForm();
           
            this.Close();
        }

        private bool CheckDuplicateEntry(int studentId, int subjectId)
        {
            dbService.Open();
            var dr = dbService.ExecuteReader("SELECT * FROM tblstudent_subjects WHERE subject_id = '" + subjectId + "' AND student_id = '" + studentId + "'");
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    return true;
                }

            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvSubjects_DoubleClick(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            this.GetSubjects(txtSearch.Text);
        }
    }
}
