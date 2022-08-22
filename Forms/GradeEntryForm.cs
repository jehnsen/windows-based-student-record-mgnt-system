using System;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class GradeEntryForm : Form
    {
        DbService dbService = new DbService();

        private int recordId;

        public GradeEntryForm(int recordId, string subjectName)
        {
            InitializeComponent();

            this.recordId = recordId;
            lbkSubject.Text = subjectName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           dbService.ExecuteQuery("UPDATE tblstudent_subjects SET final_grade = '"+ txtGrade.Text +"', remarks='"+ cboMark.SelectedIndex +"' WHERE id = '"+ this.recordId + "' ");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
