using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace StudentRecordMgntSystem.Forms
{
    public partial class LoginForm : Form
    {
        MySqlDataReader dr;
        DbService dbService = new DbService();
        HelperFunctions helperFunctions = new HelperFunctions();

        string userfullname;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (dbService.Open())
                {
                  
                    dr = dbService.ExecuteReader("SELECT CONCAT(firstname, ' ', lastname) as name FROM tbluseraccount WHERE username = '" + txtUsername.Text + "' AND password = sha1('" + txtPassword.Text + "') LIMIT 1");
                    
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            userfullname =  dr["name"].ToString().ToUpper();
                        }
                        txtUsername.Clear();
                        txtPassword.Clear();
                        this.Hide();
                        mdiMain _mainform = new mdiMain(this, userfullname);
                        _mainform.Show();
                    }
                    else
                    {
                        MessageBox.Show("User not found!", "User Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
            {
                SettingsForm _settings = new SettingsForm();
                _settings.ShowDialog();
            }
        }
    }
}
