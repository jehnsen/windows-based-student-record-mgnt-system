using System;
using System.Data;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class StudentSubjectsForm : Form
    {
        DbService dbService = new DbService();
        private int studentId;
        public StudentSubjectsForm(int studentId)
        {
            InitializeComponent();

            this.studentId = studentId;

            this.GetStudentSubjects();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddGrades_Click(object sender, EventArgs e)
        {
            try
            {
                int _id = int.Parse(lvSubjects.SelectedItems[0].Text.ToString());
                string subjectName = lvSubjects.SelectedItems[0].SubItems[2].Text.ToString();
                using (GradeEntryForm frm = new GradeEntryForm(_id, subjectName))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.GetStudentSubjects();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Select subject first!", "Subject", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelGrades_Click(object sender, EventArgs e)
        {
            try
            {
                dbService.ExecuteQuery("UPDATE tblstudent_subjects SET final_grade = 0.0, remarks = 0 WHERE id = '" + int.Parse(lvSubjects.SelectedItems[0].Text.ToString()) + "' ");
                this.GetStudentSubjects();
            }
            catch (Exception)
            {
                MessageBox.Show("Select subject first!", "Subject", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GetStudentSubjects()
        {
            var result = dbService.GetDatatable("SELECT * FROM view_studentsubjects WHERE student_id = '" + this.studentId + "'");
            lvSubjects.Items.Clear();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow dr = result.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString()); // id from tblstudent_subject, this is not the subject Id
                listitem.SubItems.Add(dr["course_code"].ToString());
                listitem.SubItems.Add(dr["description"].ToString());
                listitem.SubItems.Add(dr["semester_description"].ToString());
                listitem.SubItems.Add(dr["final_grade"].ToString());
                listitem.SubItems.Add(dr["remarks_description"].ToString());
                listitem.SubItems.Add(dr["instructor_fullname"].ToString());
                lvSubjects.Items.Add(listitem);
            }
        }
    }
}
