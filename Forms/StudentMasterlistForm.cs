using StudentRecordMgntSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class StudentMasterlistForm : Form
    {
        DbService service = new DbService();

        string sqlQuery;

        public StudentMasterlistForm()
        {
            InitializeComponent();

            cboSearchFilter.Text = "Course";
            cboYearLevel.Text = "First Year";

            this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);

            this.FillCourseComboboxData("select * from tblcourse", "description", "code");
        }
        private int GetSelectedId()
        {
            return int.Parse(dgrStudents.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // add new
            using (StudentMaintenanceForm form = new StudentMaintenanceForm(true, false, 0))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);
                    dgrStudents.Refresh();
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // update student
            StudentMaintenanceForm form = new StudentMaintenanceForm(false, false, this.GetSelectedId());
            form.ShowDialog();
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            // view
            StudentMaintenanceForm form = new StudentMaintenanceForm(false, true, this.GetSelectedId());
            form.ShowDialog();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // delete
            if (MessageBox.Show("Are you sure you want to delete this student?", 
                "Delete Student", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            var res = service.ExecuteQuery("DELETE FROM tblstudent WHERE id = '" + this.GetSelectedId() + "' ");
            if (res > 0)
            {
                MessageBox.Show("Student successfully deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);
                dgrStudents.Refresh();
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(dgrStudents.SelectedRows[0].Cells[8].Value.ToString()))
                {
                    MessageBox.Show("Student is not yet enrolled.\n\n Please enroll student first!", "Enroll", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // enrollment 
                Student student = new Student();

                student.Id = this.GetSelectedId();
                student.PrnNumber = dgrStudents.SelectedRows[0].Cells[1].Value.ToString();
                student.Firstname = dgrStudents.SelectedRows[0].Cells[2].Value.ToString();
                student.Middlename = dgrStudents.SelectedRows[0].Cells[3].Value.ToString();
                student.Lastname = dgrStudents.SelectedRows[0].Cells[4].Value.ToString();

                using (StudentEnrollmentDetailsForm _frm = new StudentEnrollmentDetailsForm(false, student))
                {
                    if (_frm.ShowDialog() == DialogResult.OK)
                    {
                        this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);
                        dgrStudents.Refresh();
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Please select student first!", "Select Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }
        private void FillCourseComboboxData(string query, string displayMember, string valueMember)
        {
            service.GetDatatable(query);
            cboCourseSelection.ComboBox.DataSource = service.dt;
            cboCourseSelection.ComboBox.DisplayMember = displayMember;
            cboCourseSelection.ComboBox.ValueMember = service.dt.Columns[0].ColumnName;
        }
        public void GetStudentList(string searchKey, string searchFilter)
        {
            switch (searchFilter)
            {
                case "Firstname":
                    searchFilter = "firstname";
                    break;
                case "Lastname":
                    searchFilter = "lastname";
                    break;
                case "ID Number":
                    searchFilter = "prn";
                    break;
            }

            if (txtSearchbox.Text.Length > 0)
            {
                if (searchFilter != "Course")
                {
                    sqlQuery = $"SELECT * FROM view_studentenrollment WHERE {searchFilter} LIKE '%" + txtSearchbox.Text + "%' ";
                }
                else
                {
                    sqlQuery = "SELECT * FROM view_studentenrollment WHERE prn LIKE '%" + txtSearchbox.Text + "%' ";
                }
            }
            else
            {
                sqlQuery = "SELECT * FROM view_studentenrollment WHERE course_description = '"+ cboCourseSelection.ComboBox.Text +"' AND year_level = '"+ cboYearLevel.ComboBox.Text +"' ";
            }
            if (searchFilter == "UnEnrolled")
            {
                sqlQuery = "SELECT * FROM view_studentenrollment WHERE enrollment_status IS NULL";
            }

            service.LoadGridviewData(sqlQuery);
            
            dgrStudents.Rows.Clear();
            dgrStudents.AlternatingRowsDefaultCellStyle.BackColor = Color.LemonChiffon;

            if (service.dt.Rows.Count > 0)
            {
                for (int i = 0; i < service.dt.Rows.Count; i++)
                {
                    dgrStudents.Rows.Add();
                    dgrStudents.Rows[i].Cells[0].Value = service.dt.Rows[i]["id"].ToString();
                    dgrStudents.Rows[i].Cells[1].Value = service.dt.Rows[i]["prn"].ToString();
                    dgrStudents.Rows[i].Cells[2].Value = service.dt.Rows[i]["firstname"].ToString();
                    dgrStudents.Rows[i].Cells[3].Value = service.dt.Rows[i]["middlename"].ToString();
                    dgrStudents.Rows[i].Cells[4].Value = service.dt.Rows[i]["lastname"].ToString();
                    dgrStudents.Rows[i].Cells[5].Value = service.dt.Rows[i]["course_description"].ToString();
                    dgrStudents.Rows[i].Cells[6].Value = service.dt.Rows[i]["year_level"].ToString();
                    dgrStudents.Rows[i].Cells[7].Value = service.dt.Rows[i]["semester"].ToString();
                    dgrStudents.Rows[i].Cells[8].Value = service.dt.Rows[i]["enrollment_status"].ToString();
                    if (dgrStudents.Rows[i].Cells[8].Value.ToString() == "Enrolled")
                    {
                        dgrStudents.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
        }

        private void frmStudentMasterlist_Activated(object sender, EventArgs e)
        {
            this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);
            dgrStudents.Refresh();
        }

        private void txtSearchbox_TextChanged(object sender, EventArgs e)
        {
            this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);
            dgrStudents.Refresh();
        }

        private void enterGradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // enter grades
            StudentSubjectsForm frm = new StudentSubjectsForm(this.GetSelectedId());
            frm.ShowDialog();
        }

        private void dgrStudents_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton7.PerformClick();
        }

        private void enrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // enroll student
            if (dgrStudents.SelectedRows[0].Cells[8].Value.ToString() == "Enrolled")
            {
                MessageBox.Show("Student is already enrolled for this School Year!", "Enrolled", MessageBoxButtons.OK ,MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Student student = new Student();

                student.Id = this.GetSelectedId();
                student.PrnNumber = dgrStudents.SelectedRows[0].Cells[1].Value.ToString();
                student.Firstname = dgrStudents.SelectedRows[0].Cells[2].Value.ToString();
                student.Middlename = dgrStudents.SelectedRows[0].Cells[3].Value.ToString();
                student.Lastname = dgrStudents.SelectedRows[0].Cells[4].Value.ToString();

                using (StudentEnrollmentDetailsForm _frm = new StudentEnrollmentDetailsForm(true, student))
                {
                    if (_frm.ShowDialog() == DialogResult.OK)
                    {
                        this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);
                        dgrStudents.Refresh();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select one student first!");
            }
        }

        private void cboCourseSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetStudentList("", "");
        }

        private void cboYearLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetStudentList("", "");
        }

        private void cboSearchFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSearchFilter.Text == "Course")
            {
                cboCourseSelection.Visible = true;
                cboYearLevel.Visible = true;
                txtSearchbox.Visible = false;
                txtSearchbox.Clear();
            } else
            {
                cboCourseSelection.Visible = false;
                cboYearLevel.Visible = false;
                txtSearchbox.Visible = true;
            }
            this.GetStudentList(txtSearchbox.Text, cboSearchFilter.Text);
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // view report
            ReportForm reportForm = new ReportForm(this.GetSelectedId());
            reportForm.MdiParent = this.ParentForm;
            reportForm.Generate("Student Grades");
            reportForm.Show();
        }

        private void viewGradesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4.PerformClick();
        }

        private void viewStudentProfileReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm(this.GetSelectedId());
            reportForm.MdiParent = this.ParentForm;
            reportForm.Generate("Student Profile");
            reportForm.Show();
        }

        private void markAsDroppedOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to drop this student?", "Drop", MessageBoxButtons.YesNo) == DialogResult.No) return;
            
            service.ExecuteQuery("UPDATE tblenrollment SET status = 2 WHERE student_id = '"+ this.GetSelectedId() + "' ");
            this.GetStudentList("", "");
        }
        private void frmStudentMasterlist_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}
