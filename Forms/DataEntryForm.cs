using StudentRecordMgntSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class DataEntryForm : Form
    {
        DbService dbService = new DbService();
        HelperFunctions helper = new HelperFunctions();
        List<Instructor> instructorList = new List<Instructor>();

        int result;
        public DataEntryForm()
        {
            InitializeComponent();

            this.GetInstructors();
            this.GetUsers();
            this.GetCourses();
            this.GetDepartments();
            this.GetShoolYear();
        }

        private void GetInstructors()
        {
            var _instructors = dbService.GetDatatable("SELECT *, CONCAT(firstname, ' ' , lastname) AS 'fullname' FROM tblinstructor");

            lvInstructors.Items.Clear();
            for (int i = 0; i < _instructors.Rows.Count; i++)
            {
                DataRow dr = _instructors.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["fullname"].ToString());
                listitem.SubItems.Add(dr["position_level"].ToString());
                lvInstructors.Items.Add(listitem);

                Instructor instructor = new Instructor();
                instructor.Id = int.Parse(dr["id"].ToString());
                instructor.Firstname = dr["firstname"].ToString();
                instructor.Middlename = dr["middlename"].ToString();
                instructor.Lastname = dr["lastname"].ToString();
                instructor.PositionLevel = dr["position_level"].ToString();
               
                instructorList.Add(instructor);
            }
            instructorList.ForEach(i => Console.WriteLine(i.Id));
        }

        private void GetUsers()
        {
            var _users = dbService.GetDatatable("SELECT *, CONCAT(firstname, ' ' , lastname) AS 'fullname' FROM tbluseraccount");

            lvUsers.Items.Clear();
            for (int i = 0; i < _users.Rows.Count; i++)
            {
                DataRow dr = _users.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["fullname"].ToString());
                listitem.SubItems.Add(dr["user_type"].ToString());
                lvUsers.Items.Add(listitem);
            }

        }
        private void GetCourses()
        {
            var _courses = dbService.GetDatatable("SELECT * FROM tblcourse");

            lvCourses.Items.Clear();
            for (int i = 0; i < _courses.Rows.Count; i++)
            {
                DataRow dr = _courses.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["code"].ToString());
                listitem.SubItems.Add(dr["description"].ToString());
                lvCourses.Items.Add(listitem);
            }

        }

        private void GetDepartments()
        {
            var _departments = dbService.GetDatatable("SELECT * FROM tbldepartment");

            lvDepartments.Items.Clear();
            for (int i = 0; i < _departments.Rows.Count; i++)
            {
                DataRow dr = _departments.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["department_name"].ToString());
                listitem.SubItems.Add(dr["building_no"].ToString());
                listitem.SubItems.Add(dr["chairperson"].ToString());
                lvDepartments.Items.Add(listitem);
            }

        }

        private void GetShoolYear()
        {
            var _sy = dbService.GetDatatable("SELECT * FROM tblschool_year");

            lvSY.Items.Clear();
            for (int i = 0; i < _sy.Rows.Count; i++)
            {
                DataRow dr = _sy.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["school_year"].ToString());
                lvSY.Items.Add(listitem);
            }

        }

        private void btnAddInstructor_Click(object sender, EventArgs e)
        {
            txtFirstname.Clear();
            txtMiddlename.Clear();
            txtLastname.Clear();
            cboPosition.Text = "";
            txtFirstname.Enabled = true;
            txtMiddlename.Enabled = true;
            txtLastname.Enabled = true;
            cboPosition.Enabled = true;
            txtFirstname.Focus();
        }

        private void btnRemoveInstructor_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this instructor?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;

            dbService.ExecuteQuery("DELETE FROM tblinstructor WHERE id = '" + lvInstructors.SelectedItems[0].Text + "' ");

            this.GetInstructors();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int _position = cboPosition.SelectedIndex + 1;
            var _result = dbService.ExecuteQuery("INSERT INTO tblinstructor (`firstname`,`middlename`,`lastname`,`position_level`, `status`) " +
               "VALUES ('" + txtFirstname.Text + "', '" + txtMiddlename.Text + "', '" + txtLastname.Text + "', '" + _position + "', 1)");
            if(result > 0)
            {
                MessageBox.Show("Instructor successfully created!", "Add Instructor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            this.GetInstructors();
            txtFirstname.Clear();
            txtMiddlename.Clear();
            txtLastname.Clear();
            cboPosition.Text = "";
            txtFirstname.Enabled = false;
            txtMiddlename.Enabled = false;
            txtLastname.Enabled = false;
            cboPosition.Enabled = false;
        }

        private void lvInstructors_Click(object sender, EventArgs e)
        {
            var selectedInstructor = this.instructorList.Find(i => i.Id == int.Parse(lvInstructors.SelectedItems[0].Text.ToString()));

            txtFirstname.Text = selectedInstructor.Firstname;
            txtMiddlename.Text = selectedInstructor.Middlename;
            txtLastname.Text = selectedInstructor.Lastname;
            cboPosition.Text = selectedInstructor.PositionLevel;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            this.ClearUserFields();

            txtUserFirstname.Enabled = true;
            txtUserLastname.Enabled = true;
            txtUsername.Enabled = true;
            txtPassword.Enabled = true;
            cboUserType.Enabled = true;
            txtUserFirstname.Focus();
        }

        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this user?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dbService.ExecuteQuery("DELETE FROM tbluseraccount WHERE id = '" + int.Parse(lvUsers.SelectedItems[0].Text.ToString()) + "' ");
                this.GetUsers();
            } else
            {
                return;
            }   
        }

        private void ClearUserFields()
        {
            txtUserFirstname.Clear();
            txtUserLastname.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            cboUserType.Text = "";

            txtUserFirstname.Enabled = false;
            txtUserLastname.Enabled = false;
            txtUsername.Enabled = false;
            txtPassword.Enabled = false;
            cboUserType.Enabled = false;
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                var _userTypeIndex = cboUserType.SelectedIndex + 1;

                result = dbService.ExecuteQuery("INSERT INTO tbluseraccount (`firstname`,`lastname`,`username`,`password`,`user_type`,`status`) " +
                                "VALUES ('" + txtUserFirstname.Text + "', '" + txtUserLastname.Text + "', '" + txtUsername.Text + "', " +
                                "sha1('" + txtPassword.Text + "'), '" + _userTypeIndex + "', 1)");

                if (result > 0) MessageBox.Show("User successfully created!", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.GetUsers();
                this.ClearUserFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OKCancel);
            }
        }

        private void btnAddNewCourse_Click(object sender, EventArgs e)
        {
            txtCourseCode.Enabled = true;
            txtCourseDescription.Enabled = true;
            txtCourseCode.Focus();
        }

        private void btnRemoveCourse_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this course?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;

            result = dbService.ExecuteQuery("DELETE FROM tblcourse WHERE id = '"+ lvCourses.SelectedItems[0].Text +"'");

            if (result > 0) MessageBox.Show("Course successfully deleted!", "Delete Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.GetCourses();
        }
        private void btnSaveCourse_Click(object sender, EventArgs e)
        {
            result = dbService.ExecuteQuery("INSERT INTO tblcourse (`code`,`description`, `status`) VALUES ('" + txtCourseCode.Text + "', '" + txtCourseDescription.Text + "', 1)");

            if (result > 0) MessageBox.Show("Course successfully created!", "Add New Course", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtCourseCode.Clear();
            txtCourseDescription.Clear();
            txtCourseCode.Enabled = false;
            txtCourseDescription.Enabled = false;
            this.GetCourses();
        }

        private void btnAddDepartment_Click(object sender, EventArgs e)
        {
            txtDepartmentName.Enabled = true;
            txtBldgNo.Enabled = true;
            txtChairperson.Enabled = true;
            txtDepartmentName.Focus();
        }

        private void btnRemoveDepartment_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this department?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;

            result = dbService.ExecuteQuery("DELETE FROM tbldepartment WHERE id = '"+ lvDepartments.SelectedItems[0].Text +"' ");

            if (result > 0) MessageBox.Show("Department successfully deleted!", "Delete Department", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.GetDepartments();
        }

        private void btnSaveDepartment_Click(object sender, EventArgs e)
        {
            result = dbService.ExecuteQuery("INSERT INTO tbldepartment (`department_name`, `building_no`,`chairperson`) " +
                                            "VALUES ('" + txtDepartmentName.Text + "', '" + txtBldgNo.Text + "', '" + txtChairperson.Text + "')");

            if (result > 0) MessageBox.Show("Department successfully created!", "Add New Department", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtDepartmentName.Clear();
            txtBldgNo.Clear();
            txtChairperson.Clear();
            txtDepartmentName.Enabled = false;
            txtBldgNo.Enabled = false;
            txtChairperson.Enabled = false;
            this.GetDepartments();
        }

        private void btnSaveSy_Click(object sender, EventArgs e)
        {
            result = dbService.ExecuteQuery("INSERT INTO tblschool_year (`school_year`) VALUES ('" + txtSy.Text + "')");

            if (result > 0) MessageBox.Show("School Year successfully created!", "Add New School Year", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtSy.Clear();
            txtSy.Enabled = false;
            this.GetShoolYear();
        }

        private void btnAddSY_Click(object sender, EventArgs e)
        {
            txtSy.Enabled = true;
            txtSy.Focus();
        }

        private void btnRemoveSY_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this school year?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No) return;

            result = dbService.ExecuteQuery("DELETE FROM tblschool_year WHERE school_year = '"+ lvSY.SelectedItems[0].Text +"' ");

            if (result > 0) MessageBox.Show("School Year successfully deleted!", "Delete School Year", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtSy.Clear();
            txtSy.Enabled = false;
            this.GetShoolYear();
        }
    }
}
