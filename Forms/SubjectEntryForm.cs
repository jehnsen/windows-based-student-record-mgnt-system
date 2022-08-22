using MySql.Data.MySqlClient;
using StudentRecordMgntSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class SubjectEntryForm : Form
    {

        DbService dbService = new DbService();
        Subject subject = new Subject();
        MySqlDataReader dr;
        List<Instructor> instructorsArray = new List<Instructor>();

        private int subjectId;
        private bool isNew;
        string sqlQuery;
        int queryStatus;
        
        public SubjectEntryForm()
        {
            InitializeComponent();
        }
        public SubjectEntryForm(int subjectId, bool isNew)
        {
            InitializeComponent();
            this.isNew = isNew;
            this.subjectId = subjectId;

            // get instructor list 
            this.FillComboboxData("select id, concat(firstname, ' ', lastname) as 'instructor_name' from tblinstructor", "instructor_name", "id");

            if (!isNew) this.GetSubjectDetails();

            Text = isNew ? "Add New Subject" : "Update Subject";
        }

        private void GetSubjectDetails()
        {
            try
            {
                if (dbService.Open())
                {
                    dr = dbService.ExecuteReader("SELECT s.*, CONCAT(i.firstname, ' ',i.lastname) AS 'name' FROM tblsubject s " +
                                                "INNER JOIN tblinstructor i ON s.instructor_id = i.id WHERE s.id = '" + this.subjectId + "'");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            txtCourseCode.Text = dr["course_code"].ToString();
                            txtDescription.Text = dr["description"].ToString();
                            txtLectureUnit.Value = (int)dr["lecture_unit"];
                            txtLabUnit.Text = dr["lab_unit"].ToString();
                            cboSemester.Text = dr["semester"].ToString();
                            cboYear.Text = dr["year_level"].ToString();
                            cboInstructor.Text = dr["name"].ToString();
                        }

                    }
                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // get the instructor id
                var _selectedInstructor = instructorsArray.Find(i => i.Firstname == cboInstructor.Text.ToString());

                if (this.isNew)
                {
                    sqlQuery = "INSERT INTO tblsubject (`course_code`, `description`, `lecture_unit`, `lab_unit`, `semester`, `year_level`, `instructor_id`, `status`) " +
                                "VALUES ('" + txtCourseCode.Text + "', '" + txtDescription.Text + "', '" + txtLectureUnit.Value + "', '" + txtLabUnit.Text + "', " +
                                "'" + cboSemester.Text + "', '" + cboYear.Text + "', '" + _selectedInstructor.Id + "', 1)";
                }
                else
                {
                    sqlQuery = "UPDATE tblsubject SET course_code='" + txtCourseCode.Text + "', description='" + txtDescription.Text + "', " + 
                                "lecture_unit='" + txtLectureUnit.Value + "', lab_unit='" + txtLabUnit.Text + "', semester='" + cboSemester.Text + "', " +
                                "year_level='" + cboYear.Text + "', instructor_id='" + _selectedInstructor.Id + "' WHERE id = '"+ this.subjectId +"' ";
                }
                queryStatus = dbService.ExecuteQuery(sqlQuery);
                if (queryStatus > 0)
                {
                    MessageBox.Show("Subject successfully saved to database!", "Add/Edit Subject", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Something went wrong while saving to database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.Close();
        }

        private void FillComboboxData(string query, string displayMember, string valueMember)
        {
            dbService.GetDatatable(query);

            cboInstructor.DataSource = dbService.dt;
            cboInstructor.DisplayMember = displayMember;
            cboInstructor.ValueMember = dbService.dt.Columns[0].ColumnName;

            Instructor instructor = new Instructor();

            for (int i = 0; i < dbService.dt.Rows.Count; i++)
            {
                DataRow dr = dbService.dt.Rows[i];
                instructor.Id = (int)dr["id"];
                instructor.Firstname = dr["instructor_name"].ToString();
                instructorsArray.Add(new Instructor(instructor.Id, instructor.Firstname, "bezos", "musk", "instructor"));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
