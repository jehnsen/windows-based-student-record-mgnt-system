using MySql.Data.MySqlClient;
using StudentRecordMgntSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class StudentEnrollmentDetailsForm : Form
    {
        DbService dbService = new DbService();
        List<Subject> subjectList = new List<Subject>();
        Student student = new Student();

        MySqlDataReader dr;

        public int courseId;
        public string courseCode;
        public string courseDescription;
        public string firstname;
        public string middlename;
        public string lastname;

        private int studentId;
        private bool isNew = false;
        private string studentName;

        int recordId;
        string sqlQuery;
        public StudentEnrollmentDetailsForm()
        {
            InitializeComponent();
        }
        public StudentEnrollmentDetailsForm(bool isNew, Student student)
        {
            InitializeComponent();
            this.isNew = isNew;
            this.student = student;
            this.studentId = student.Id;

            txtStudentIdNo.Text = this.student.PrnNumber;
            txtFirstname.Text = this.student.Firstname;
            txtMiddlename.Text = this.student.Middlename;
            txtLastname.Text = this.student.Lastname;

            if (!isNew)
            {
                this.GetStudentSubjects();
                this.GetStudentEnrollmentInfo();
                this.Text = $"Update Enrollment Details - {this.student.Lastname}, {this.student.Firstname} {this.student.Middlename.Substring(0, 1)}.";
            } else
            {
                this.Text = $"Enroll Student - {this.student.Lastname}, {this.student.Firstname} {this.student.Middlename.Substring(0, 1)}.";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDescription.Text.Length == 0)
            {
                MessageBox.Show("Please select course!");
            }
            else if (cboSemester.Text.Length == 0 || cboYearLevel.Text.Length == 0)
            {
                MessageBox.Show("Please add semester and year level!");
            }
            else if (lvSubjects.Items.Count == 0)
            {
                MessageBox.Show("Please add student subject!");
            }
            else
            {
                int yearLevelIndex = cboYearLevel.SelectedIndex + 1;
                int semesterIndex = cboSemester.SelectedIndex + 1;

                if (this.isNew)
                {
                    sqlQuery = "INSERT INTO tblenrollment (`student_id`, `course_id`, `year_level`, `semester`, `date_enrolled`, `school_year`, `status`) " +
                               "VALUES ('" + this.studentId + "', '" + this.courseId + "', '" + yearLevelIndex + "', '" + semesterIndex + "', " +
                               "'" + dtpDateEnrolled.Value.Date.ToString("yyyy-MM-dd HH:mm") + "', '" + txtSchoolYear.Text + "', 1)";
                }
                else
                {
                    sqlQuery = "UPDATE tblenrollment SET course_id='" + this.courseId + "', year_level='" + cboYearLevel.Text + "', " +
                        "semester='" + semesterIndex + "', date_enrolled='" + dtpDateEnrolled.Value.Date.ToString("yyyy-MM-dd HH:mm") + "', school_year='" + txtSchoolYear.Text + "' " +
                        "WHERE student_id = '" + this.studentId + "'";
                }

                var result = dbService.ExecuteQuery(sqlQuery);
                if (result > 0)
                {
                    MessageBox.Show("Student successfully updated!", "Enrollment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewSubject_Click(object sender, EventArgs e)
        {
            SubjectSelectionForm _frm = new SubjectSelectionForm(this.studentId, this.courseId, txtCourseCode.Text);
            _frm.ShowDialog();
        }

        private void btnDeleteSubject_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($"Are you sure you want to remove this subject for this student? \n\n{this.studentName}", 
                        "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;
                
                dbService.ExecuteQuery("DELETE FROM tblstudent_subjects WHERE id = '" + recordId + "' ");
                
                this.GetStudentSubjects();
                lvSubjects.Refresh();

            }
             catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OKCancel);
            }

        }

        private void lvSubjects_Click(object sender, EventArgs e)
        {
            // recordId: referring to the id key in tblstudent_subjects table
            recordId = int.Parse(lvSubjects.SelectedItems[0].Text);
        }

        private void SetFormState()
        {
            if (this.isNew)
            {
                txtCourseCode.Clear();
                txtDescription.Clear();
                cboYearLevel.Text = "";
                cboSemester.Text = "";

            }
            txtCourseCode.Enabled = true;
            txtDescription.Enabled = true;
            cboYearLevel.Enabled = true;
            cboSemester.Enabled = true;
        }

        private void frmStudentSubjectEntry_Activated(object sender, EventArgs e)
        {
            this.GetStudentSubjects();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (CourseSelectionForm _csf = new CourseSelectionForm(this))
            {
                if (_csf.ShowDialog() == DialogResult.OK)
                {
                    txtCourseCode.Text = this.courseCode;
                    txtDescription.Text = this.courseDescription;
                }
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
                listitem.SubItems.Add(dr["lecture_unit"].ToString());
                listitem.SubItems.Add(dr["lab_unit"].ToString());
                listitem.SubItems.Add(dr["semester_description"].ToString());
                listitem.SubItems.Add(dr["instructor_fullname"].ToString());
                lvSubjects.Items.Add(listitem);
            }
        }

        private void GetStudentEnrollmentInfo()
        {
            try
            {
                if (dbService.Open())
                {
                    dr = dbService.ExecuteReader("SELECT * FROM view_studentenrollment WHERE id = '" + this.studentId + "'");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            this.courseId = int.Parse(dr["course_id"].ToString());
                            txtCourseCode.Text = dr["course_code"].ToString();
                            txtDescription.Text = dr["course_description"].ToString();
                            cboSemester.Text = dr["semester"].ToString();
                            cboYearLevel.Text = dr["year_level"].ToString();
                            txtSchoolYear.Text = dr["school_year"].ToString();
                            txtStatus.Text = dr["enrollment_status"].ToString();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
