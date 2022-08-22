using System;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class FamilyBackgroundEntryForm : Form
    {

        DbService dbService = new DbService();
        private bool isNew;
        int studentId;
        string sqlString;

        public FamilyBackgroundEntryForm(bool isNew, int studentId)
        {
            InitializeComponent();
            this.isNew = isNew;
            this.studentId = studentId;
            txtFirstname.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.isNew)
            {
                sqlString = "INSERT INTO tblstudent_family_member (`student_id`, `firstname`, `middlename`, `lastname`, `address`, `occupation`,`contact_no`, `relationship`) " +
                        "VALUES ('" + this.studentId + "', '" + txtFirstname.Text + "', '" + txtMiddlename.Text + "', '" + txtLastname.Text + "','" + txtAddress.Text + "', '" + txtOccupation.Text + "', " +
                        "'" + txtContactNo.Text + "', '" + cboRelationship.Text + "')";
            }

            dbService.ExecuteQuery(sqlString);

            this.Close();
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
