using StudentRecordMgntSystem.Forms;
using System;
using System.Windows.Forms;

namespace StudentRecordMgntSystem
{
    public partial class mdiMain : Form
    {
        SubjectMasterlistForm _frmSubjectList = new SubjectMasterlistForm();
        StudentMasterlistForm _frmStudentMasterlist = new StudentMasterlistForm();
        
        SettingsForm settingsForm = new SettingsForm();

        private LoginForm loginForm;
      
        public mdiMain(LoginForm loginForm, string userFullname)
        {
            InitializeComponent();

            this.loginForm = loginForm;
            lblUser.Text = userFullname;
        }

        private bool isFormOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mdiMain_Load(object sender, EventArgs e)
        {
            // load the studentmasterlist from upon system initialization
            menuStudentMasterlsit.PerformClick();
        }

        private void menuStudentMasterlsit_Click(object sender, EventArgs e)
        {
            
            if (this.isFormOpened("StudentMasterlistForm"))
            {
                _frmStudentMasterlist.BringToFront();
            }
            else
            {
                if (_frmStudentMasterlist.IsDisposed)
                {
                    StudentMasterlistForm _frmStudentMasterlist = new StudentMasterlistForm();
                }
                _frmStudentMasterlist.MdiParent = this;
                _frmStudentMasterlist.Show();
            }
        }
        private void menuSubjectMasterlist_Click(object sender, EventArgs e)
        {
            
            if (this.isFormOpened("SubjectMasterlistForm"))
            {
                _frmSubjectList.BringToFront();
            }
            else
            {
                if (_frmSubjectList.IsDisposed)
                {
                    SubjectMasterlistForm _frmSubjectMasterlist = new SubjectMasterlistForm();
                }
                _frmSubjectList.MdiParent = this;
                _frmSubjectList.Show();
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            menuStudentMasterlsit.PerformClick();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            menuSubjectMasterlist.PerformClick();
        }

        private void menuDataEntryTool_Click(object sender, EventArgs e)
        {
            DataEntryForm _dataEntry = new DataEntryForm();
            _dataEntry.ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.MdiParent = this;
            reportForm.Show();
        }

        private void mdiMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.loginForm.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.ShowDialog();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
    }
}
