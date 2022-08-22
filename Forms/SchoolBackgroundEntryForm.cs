using System;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class SchoolBackgroundEntryForm : Form
    {
        DbService dbService = new DbService();

        private bool isNew;
        private int studentId;
        private string sqlString;
        public SchoolBackgroundEntryForm()
        {
            InitializeComponent();
        }

        public SchoolBackgroundEntryForm(bool isNew, int studentId)
        {
            this.isNew = isNew;
            this.studentId = studentId;
            InitializeComponent();
        }

        private void frmSchoolBackgroundEntry_Load(object sender, EventArgs e)
        {
            txtSchoolName.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.isNew)
            {
                sqlString = "INSERT INTO tblstudent_educational_bg (`student_id`, `school_name`, `address`, `year_graduated`) " +
                              "VALUES ('"+ this.studentId + "', '" + txtSchoolName.Text + "', '" + txtAddress.Text + "', '" + txtYearGraduated.Text + "')";

            } else
            {
                sqlString = "UPDATE tblstudent_educational_bg SET `school_name`='" + txtSchoolName.Text + "', " +
                            "`address`='" + txtAddress.Text + "', `year_graduated`='" + txtYearGraduated.Text + "' WHERE student_id = '"+ this.studentId +"' ";

            }
            // save to database
            dbService.ExecuteQuery(sqlString);

            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
