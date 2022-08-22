using StudentRecordMgntSystem.Models;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class StudentMaintenanceForm : Form
    {
        private int studentId = 0;
        private string path;
        private string sqlString;
        private bool isNew = true;
        private bool isViewOnly = false;

        DbService dbService = new DbService();
        StudentService studentService = new StudentService();
        Student student = new Student();
        HelperFunctions helperFunctions = new HelperFunctions();

        public StudentMaintenanceForm(bool isNew, bool isViewOnly, int studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            this.isNew = isNew;

            if (this.isNew)
            {
                this.Text = "Student Entry - Add New Student";
                lblPrnNumber.Text = studentService.GeneratePrnNumber(studentService.GetLastId());
            } else
            {
                this.isViewOnly = isViewOnly;
                this.student = studentService.GetStudentData(this.studentId);
                this.FillStudentPersonalInfo();

                // load student data
                this.GetStudentEducationalInfo();
                this.GetStudentFamilyMembers();
                this.GetImage();

                var _mode = isViewOnly ? "View Personal Data" : "Update Personal Data";
                this.Text = $"{_mode} - {student.Lastname}, {student.Firstname} {student.Middlename.Substring(0, 1)}.";
            }
            this.SetFormState();
        }

        private void SetFormState()
        {
            // if view only, disable all input fields
            if (this.isViewOnly)
            {
                txtFirstname.Enabled = false;
                txtMiddlename.Enabled = false;
                txtLastname.Enabled = false;
                dtpBirthdate.Enabled = false;
                txtBirthPlace.Enabled = false;
                txtAge.Enabled = false;
                txtCitizenship.Enabled = false;
                txtContactNo.Enabled = false;
                txtEmailAddress.Enabled = false;
                txtHeight.Enabled = false;
                txtWeight.Enabled = false;
                cboReligion.Enabled = false;
                txtResAddBrgy.Enabled = false;
                txtResAddMun.Enabled = false;
                txtResAddProv.Enabled = false;
                txtResAddZip.Enabled = false;
                txtPerAddBrgy.Enabled = false;
                txtPerAddMun.Enabled = false;
                txtPerAddProv.Enabled = false;
                txtPerAddZip.Enabled = false;
                cboCivilStatus.Enabled = false;
                cboGender.Enabled = false;
                btnAddFamily.Enabled = false;
                btnRemoveFamily.Enabled = false;
                btnAddNewSchoolBackground.Enabled = false;
                btnDeleteSchoolBackground.Enabled = false;
                btnBrowsePhoto.Visible = false;
            }
            // if new entry, reset all fields
            if (this.isNew)
            {
                txtFirstname.Clear();
                txtMiddlename.Clear();
                txtLastname.Clear();
                txtBirthPlace.Clear();
                txtAge.Clear();
                txtCitizenship.Clear();
                txtContactNo.Clear();
                txtEmailAddress.Clear();
                txtHeight.Clear();
                txtWeight.Clear();
                txtResAddBrgy.Clear();
                txtResAddMun.Clear();
                txtResAddProv.Clear();
                txtResAddZip.Clear();
                txtPerAddBrgy.Clear();
                txtPerAddMun.Clear();
                txtPerAddProv.Clear();
                txtPerAddZip.Clear();
                cboCivilStatus.Text = "";
                cboGender.Text = "";
                cboReligion.Text = "";
                txtFirstname.Focus();
            }
        }

        // this event use for updating data everytime there's a change in the db
        private void frmStudentMaintenance_Activated(object sender, EventArgs e)
        {
            lvEducBackground.Items.Clear();
            this.GetStudentEducationalInfo();
            lvEducBackground.Refresh();

            lvFamilyMembers.Items.Clear();
            this.GetStudentFamilyMembers();
            lvFamilyMembers.Refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isValidEmail = helperFunctions.ValidateEmail(txtEmailAddress.Text);

            // if email is not a valid email address then terminate the function
            if (!isValidEmail)
            {
                MessageBox.Show("Not a valid email address");
                return;
            }

            this.SaveStudentPersonalInformation();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveStudentPersonalInformation()
        {
            // update other entries related to the student
            // get the last student id in the database
            string newStudentId = studentService.GetLastId();
            
            if (helperFunctions.IsEmpty(newStudentId))
            {
                MessageBox.Show("Something went wrong! Please check network connection and try again.");
            } else
            {
                var _height = txtHeight.Text.Replace("'", "''");
            
                if (this.isNew)
                {
                    var _genderIndex = cboGender.SelectedIndex + 1;
                    var _civilStatusIndex = cboCivilStatus.SelectedIndex + 1;
                    
                    // create new student
                    sqlString = "INSERT INTO tblStudent (`prn`, `firstname`, `middlename`, `lastname`, `contact_no`, `birthdate`, `gender`, `civilstatus`, `birthplace`, `weight`, `height`, " +
                        "`res_address_brgy`, `res_address_mun`, `res_address_prov`, `res_address_zipcode`, `per_address_brgy`, `per_address_mun`, `per_address_prov`, `per_address_zipcode`, " +
                        "`citizenship`, `religion`,`email_address`,`age`, `status`) " +
                        "VALUES ('"+ lblPrnNumber.Text + "', '" + txtFirstname.Text + "', '" + txtMiddlename.Text + "', '" + txtLastname.Text + "', '" + txtContactNo.Text + "', '" + dtpBirthdate.Text + "', " +
                        "'" + _genderIndex + "', '" + _civilStatusIndex + "', '" + txtBirthPlace.Text + "', '" + txtWeight.Text + "', '" + _height + "', '" + txtResAddBrgy.Text + "', " +
                        "'" + txtResAddMun.Text + "', '" + txtResAddProv.Text + "', '" + txtResAddZip.Text + "', '" + txtPerAddBrgy.Text + "', '" + txtPerAddMun.Text + "', '" + txtPerAddProv.Text + "', " +
                        "'" + txtPerAddZip.Text + "', '" + txtCitizenship.Text + "', '" + cboReligion.Text + "','" + txtEmailAddress.Text + "','" + txtAge.Text + "', 1)";

                    // call the service
                    var newStudent = dbService.ExecuteQuery(sqlString);
                    if (newStudent > 0)
                    {
                        MessageBox.Show("Student successfully added to database!", "New Student", 
                                             MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // update all entries w/ studentid = 0 (identifier for new entries)
                    dbService.ExecuteQuery("UPDATE tblstudent_educational_bg SET student_id = '" + newStudentId + "', status = 1 WHERE student_id = 0 AND status = 0");
                    dbService.ExecuteQuery("UPDATE tblstudent_family_member SET student_id = '" + newStudentId + "', status = 1 WHERE student_id = 0 AND status = 0");
                    // insert student photo
                    if (!helperFunctions.IsEmpty(this.path))
                    {
                        this.InsertStudentPhoto(int.Parse(newStudentId));
                    }

                } 
                else
                {
                    sqlString = "UPDATE tblStudent SET firstname='" + txtFirstname.Text + "', middlename='" + txtMiddlename.Text + "', lastname='" + txtLastname.Text + "', " +
                        "contact_no='" + txtContactNo.Text + "', birthdate='" + dtpBirthdate.Text + "', gender='" + cboGender.Text + "', civilstatus='" + cboCivilStatus.Text + "', " +
                        "birthplace='" + txtBirthPlace.Text + "', weight='" + txtWeight.Text + "', height='" + _height + "', res_address_brgy='" + txtResAddBrgy.Text + "', " +
                        "res_address_mun='" + txtResAddMun.Text + "', res_address_prov='" + txtResAddProv.Text + "', res_address_zipcode='" + txtResAddZip.Text + "', " +
                        "per_address_brgy='" + txtPerAddBrgy.Text + "', per_address_mun='" + txtPerAddMun.Text + "', per_address_prov='" + txtPerAddProv.Text + "', " +
                        "per_address_zipcode='" + txtPerAddZip.Text + "', citizenship='" + txtCitizenship.Text + "', religion='"+ cboReligion.Text +"', " +
                        "email_address='"+ txtEmailAddress.Text +"', age='"+ txtAge.Text +"' WHERE id = '"+ this.studentId +"' ";

                    // call the service
                    var result = dbService.ExecuteQuery(sqlString);
                    if (result > 0)
                    {
                        MessageBox.Show("Student successfully updated!", "Update Student", 
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // update student photo
                    if (!helperFunctions.IsEmpty(this.path))
                    {
                        dbService.ExecuteQuery("UPDATE tblstudent_photos SET path = '" + this.path.Replace(@"\", @"\\") + "' WHERE student_id = '"+ this.studentId + "'");
                    }

                }   
            }
            
        }

        private void GetImage()
        {
            this.path = studentService.GetPhotoUrlPath(this.studentId);
            
            if (!helperFunctions.IsEmpty(this.path))
            {
                Image image;
                image = Image.FromFile($@"{this.path}");
                picStudentPic.Image = image;
            }   
        }

        private void FillStudentPersonalInfo()
        {
            txtFirstname.Text = student.Firstname;
            txtMiddlename.Text = student.Middlename;
            txtLastname.Text = student.Lastname;
            dtpBirthdate.Text = student.BirthDate;
            txtBirthPlace.Text = student.BirthPlace;
            txtAge.Text = student.Age.ToString();
            cboGender.Text = student.Gender;
            cboCivilStatus.Text = student.CivilStatus;
            cboReligion.Text = student.Religion;
            txtCitizenship.Text = student.Citizenship;
            txtHeight.Text = student.Height;
            txtWeight.Text = student.Weight;
            txtContactNo.Text = student.ContactNumber;
            txtEmailAddress.Text = student.EmailAddress;
            txtResAddBrgy.Text = student.ResAddressBrgy;
            txtResAddMun.Text = student.ResAddressMun;
            txtResAddProv.Text = student.ResAddressProvince;
            txtResAddZip.Text = student.ResAddressZipCode;
            txtPerAddBrgy.Text = student.PerAddressBrgy;
            txtPerAddMun.Text = student.PerAddressMun;
            txtPerAddProv.Text = student.PerAddressProvince;
            txtPerAddZip.Text = student.PerAddressZipCode;
            
        }

        private void GetStudentEducationalInfo()
        {
            var result = dbService.GetDatatable("SELECT * FROM tblstudent_educational_bg WHERE student_id = '"+ this.studentId +"'");
            lvEducBackground.Items.Clear();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow dr = result.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["school_name"].ToString());
                listitem.SubItems.Add(dr["address"].ToString());
                listitem.SubItems.Add(dr["year_graduated"].ToString());
                lvEducBackground.Items.Add(listitem);
            }
        }

        private void GetStudentFamilyMembers()
        {
            
            if (!this.isNew)
            {
                sqlString = "SELECT * FROM tblstudent_family_member WHERE student_id = '" + this.studentId + "'";
            } else
            {
                sqlString = "SELECT * FROM tblstudent_family_member WHERE student_id = 0 and status = 0";
            }

            var familymembers = dbService.GetDatatable(sqlString);
            lvFamilyMembers.Items.Clear();
            for (int i = 0; i < familymembers.Rows.Count; i++)
            {
                DataRow dr = familymembers.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["id"].ToString());
                listitem.SubItems.Add(dr["firstname"].ToString());
                listitem.SubItems.Add(dr["middlename"].ToString());
                listitem.SubItems.Add(dr["lastname"].ToString());
                listitem.SubItems.Add(dr["relationship"].ToString());
                listitem.SubItems.Add(dr["address"].ToString());
                listitem.SubItems.Add(dr["contact_no"].ToString());
                lvFamilyMembers.Items.Add(listitem);
            }
        }

        private void btnBrowsePhoto_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select image to be upload.";
            // valid image formats
            openFileDialog1.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            // FilterIndex property represents the index of the filter currently selected in the file dialog box.
            openFileDialog1.FilterIndex = 1;
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialog1.CheckFileExists)
                    {
                        this.path = System.IO.Path.GetFullPath(openFileDialog1.FileName); //.Replace(@"\", @"\\");
                        picStudentPic.Image = new Bitmap(openFileDialog1.FileName);
                        picStudentPic.SizeMode = PictureBoxSizeMode.StretchImage;
                       
                    }
                }
                else
                {
                    MessageBox.Show("Please select a photo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void InsertStudentPhoto(int studentId)
        {
            // insert the new photo
            dbService.ExecuteQuery("insert into tblstudent_photos (student_id, path, status) values ('"+ studentId + "', '"+ this.path.Replace(@"\", @"\\") + "', 1) ");
        }
        private void btnAddNewSchoolBackground_Click(object sender, EventArgs e)
        {
            SchoolBackgroundEntryForm _frm = new SchoolBackgroundEntryForm(true, studentId);
            _frm.ShowDialog();
        }
        private void btnDeleteSchoolBackground_Click(object sender, EventArgs e)
        {
            dbService.ExecuteQuery("DELETE FROM tblstudent_educational_bg WHERE id = '" + lvEducBackground.SelectedItems[0].Text + "'");
            this.GetStudentEducationalInfo();
        }
        private void btnAddFamily_Click(object sender, EventArgs e)
        {
            FamilyBackgroundEntryForm _frm = new FamilyBackgroundEntryForm(true, this.studentId);
            _frm.ShowDialog();
        }

        private void btnRemoveFamily_Click(object sender, EventArgs e)
        {
            dbService.ExecuteQuery("DELETE FROM tblstudent_family_member WHERE id = '"+ lvFamilyMembers.SelectedItems[0].Text  +"' ");
            this.GetStudentFamilyMembers();
        }

    }
}
