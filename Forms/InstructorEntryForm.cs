using System;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class InstructorEntryForm : Form
    {
        DbService dbService = new DbService();
        private int instructorId;
        private bool isNew;
        string sqlString;

        public InstructorEntryForm(bool isNew, int instructorId)
        {
            InitializeComponent();

            this.isNew = isNew;
            this.instructorId = instructorId;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Text = cboRank.SelectedIndex.ToString();
            //if (this.instructorId == 0)
            //{
            //    sqlString = "INSERT INTO tblinstructor (`firstname`, `middlename`, `lastname`, `position_level`, `status`) " +
            //                  "VALUES ('" + txtFirstname.Text + "', '" + txtMiddlename.Text + "', '" + txtLastname.Text + "', '"+ cboRank.Text +"' 1)";

            //}
            //else
            //{
            //    sqlString = "UPDATE tblinstructor SET firstname='" + txtFirstname.Text + "', " +
            //                "middlename='" + txtMiddlename.Text + "', lastname='" + txtLastname.Text + "', " +
            //                "position_level='"+ cboRank.Text +"' WHERE id = '" + this.instructorId + "' ";

            //}
            // save to database
            dbService.ExecuteQuery(sqlString);

            this.Close();

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
