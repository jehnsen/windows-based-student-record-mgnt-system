using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace StudentRecordMgntSystem
{
    class DbService
    {
        MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataAdapter da;
        public DataTable dt;
        int result;

        static string server = ConfigurationManager.AppSettings.Get("server"),
                      database = ConfigurationManager.AppSettings.Get("database"),
                      user = ConfigurationManager.AppSettings.Get("user"),
                      password = ConfigurationManager.AppSettings.Get("port");

        static string connectionString = $"Server={server};Database={database};User ID={user};Password={password}";
     
        public bool Open()
        {
            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
                return true;
            }
            catch (Exception er)
            {
                MessageBox.Show("Connection Error ! " + er.Message, "Information");
            }
            return false;
        }
        public void Close()
        {
            conn.Close();
            conn.Dispose();
        }
        public DataSet ExecuteDataSet(string sql, string dataTableName)
        {
            try
            {
                if (this.Open())
                {
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    //da.Fill(ds, "result");
                    da.Fill(ds, dataTableName);
                    return ds;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return null;
        }
        public MySqlDataReader ExecuteReader(string sql)
        {
            try
            {
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                int affected;
                MySqlTransaction mytransaction = conn.BeginTransaction();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                affected = cmd.ExecuteNonQuery();
                mytransaction.Commit();
                return affected;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return -1;
        }
        public int ExecuteQuery(string sql)
        {
            try
            {
                var isConnectionOpen = this.Open();

                if (isConnectionOpen)
                {
                    result = this.ExecuteNonQuery(sql);
                    if (result > 0)
                    {
                        return result;
                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return -1;
        }
        public DataTable GetDatatable(string sql)
        {
            try
            {
                var isConnectionOpen = this.Open();
                //check if db connection status is open
                if (isConnectionOpen)
                {
                    cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    da = new MySqlDataAdapter();
                    da.SelectCommand = cmd;
                    dt = new DataTable();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //close the connection
                this.Close();
            }
            return dt;
        }
        public void LoadGridviewData(string sql)
        {
            try
            {

                if (this.Open())
                {
                    cmd = new MySqlCommand();
                    da = new MySqlDataAdapter();
                    dt = new DataTable();

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    //dgv.DataSource = dt;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                da.Dispose();
                this.Close();
            }

        }

    }
}
