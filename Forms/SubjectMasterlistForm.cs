using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class SubjectMasterlistForm : Form
    {
        DbService dbService = new DbService();

        string queryString;
        public SubjectMasterlistForm()
        {
            InitializeComponent();

            this.GetSubjectList("", "");
        }

        public void GetSubjectList(string searchKey, string filter)
        {
            if(searchKey.Length > 0)
            {
                
                if (filter.Length > 0)
                {
                    string sqlField = "description";
                    switch (filter)
                    {
                        case "Course Code":
                            sqlField = "course_code";
                            break;
                        case "Instructor":
                            sqlField = "instructor_id";
                            break;
                        case "Semester":
                            sqlField = "semester";
                            break;
                        case "Unit":
                            sqlField = "unit";
                            break;
                        default:
                            break;
                    }
                    queryString = $"SELECT *, CASE status WHEN 1 THEN 'Active' ELSE 'InActive' END AS 'stat' FROM tblsubject WHERE {sqlField} LIKE '%" + searchKey + "%' LIMIT 50";
                } else
                {
                    queryString = "SELECT *, CASE status WHEN 1 THEN 'Active' ELSE 'InActive' END AS 'stat' FROM tblsubject WHERE description LIKE '%" + searchKey + "%' LIMIT 50";
                }
            } else
            {
                queryString = "SELECT *, CASE status WHEN 1 THEN 'Active' ELSE 'InActive' END AS 'stat' FROM tblsubject";
            }
            dbService.LoadGridviewData(queryString);
            //clear existing datagrid data
            dgrSubjects.Rows.Clear();
            dgrSubjects.AlternatingRowsDefaultCellStyle.BackColor = Color.LemonChiffon;
            if (dbService.dt.Rows.Count > 0)
            {
                for (int i = 0; i < dbService.dt.Rows.Count; i++)
                {
                    dgrSubjects.Rows.Add();
                    dgrSubjects.Rows[i].Cells[0].Value = dbService.dt.Rows[i]["id"].ToString();
                    dgrSubjects.Rows[i].Cells[1].Value = dbService.dt.Rows[i]["course_code"].ToString();
                    dgrSubjects.Rows[i].Cells[2].Value = dbService.dt.Rows[i]["description"].ToString();
                    dgrSubjects.Rows[i].Cells[3].Value = dbService.dt.Rows[i]["lecture_unit"].ToString();
                    dgrSubjects.Rows[i].Cells[4].Value = dbService.dt.Rows[i]["lab_unit"].ToString();
                    dgrSubjects.Rows[i].Cells[5].Value = dbService.dt.Rows[i]["semester"].ToString();
                    dgrSubjects.Rows[i].Cells[6].Value = dbService.dt.Rows[i]["year_level"].ToString();
                    dgrSubjects.Rows[i].Cells[7].Value = "Elon Musk"; // instructor
                    dgrSubjects.Rows[i].Cells[8].Value = dbService.dt.Rows[i]["stat"].ToString();
                }
            }
        }

        private void txtSearchbox_TextChanged(object sender, EventArgs e)
        {
            this.GetSubjectList(txtSearchbox.Text, cboSearchFilter.Text);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // add new subject
            using (SubjectEntryForm subjectEntry = new SubjectEntryForm(0, true) { })
            {
                if (subjectEntry.ShowDialog() == DialogResult.OK)
                {
                    this.GetSubjectList(txtSearchbox.Text, cboSearchFilter.Text);
                    dgrSubjects.Refresh();
                }
            }
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete this subject?",
                      "Delete Subject", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

                var result = dbService.ExecuteQuery("DELETE FROM tblsubject WHERE id = '" + dgrSubjects.SelectedRows[0].Cells[0].Value + "'");

                if (result > 0) MessageBox.Show("Subject successfully deleted.", "Delete Subject", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.GetSubjectList(txtSearchbox.Text, cboSearchFilter.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // update subject details
            int _id = int.Parse(dgrSubjects.SelectedRows[0].Cells[0].Value.ToString());
            SubjectEntryForm subjectEntry = new SubjectEntryForm(_id, false);
            subjectEntry.ShowDialog();
        }
        private void dgrSubjects_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void SubjectMasterlistForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void dgrSubjects_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton2.PerformClick();
        }
    }
}
