using System;
using System.Windows.Forms;
using System.Configuration;

namespace StudentRecordMgntSystem.Forms
{
    public partial class SettingsForm : Form
    {

        public SettingsForm()
        {
            InitializeComponent();

            txtServer.Text = ConfigurationManager.AppSettings.Get("server");
            txtDatabase.Text = ConfigurationManager.AppSettings.Get("database");
            txtUser.Text = ConfigurationManager.AppSettings.Get("user");
            txtPort.Text = ConfigurationManager.AppSettings.Get("port");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["server"].Value = txtServer.Text;
            config.AppSettings.Settings["database"].Value = txtDatabase.Text;
            config.AppSettings.Settings["user"].Value = txtUser.Text;
            config.AppSettings.Settings["port"].Value = txtPort.Text;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Database connection settings updated!", "Configuration File", 
                MessageBoxButtons.OK ,MessageBoxIcon.Information);

            Application.Exit();
        }
    }
}
