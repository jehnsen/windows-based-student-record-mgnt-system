using MySql.Data.MySqlClient;
using StudentRecordMgntSystem.Models;
using System;
using System.Data;

namespace StudentRecordMgntSystem
{
    class StudentService
    {
        DbService dbService = new DbService();
        HelperFunctions helperFunctions = new HelperFunctions();

        public DataTable dt;
        MySqlDataReader dr;

        public string GeneratePrnNumber(string newPrn)
        {
            string lastId = this.GetLastId();
 
            switch (lastId.Length)
            {
                case 2:
                    newPrn = $"PRN-0000{lastId}";
                    break;
                case 3:
                    newPrn = $"PRN-000{lastId}";
                    break;
                case 4:
                    newPrn = $"PRN-00{lastId}";
                    break;
                case 5:
                    newPrn = $"PRN-0{lastId}";
                    break;
                case 6:
                    newPrn = $"PRN-{lastId}";
                    break;
                default:
                    newPrn = $"PRN-00000{lastId}";
                    break;
            }
            return newPrn;
        }

        public Student GetStudentData(int studentId)
        {
            try
            {
                Student student = new Student();
                if (dbService.Open())
                {
                    dr = dbService.ExecuteReader("select * from tblstudent where id = '" + studentId + "'");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            student.Id = (int)dr["id"];
                            student.PrnNumber = dr["prn"].ToString();
                            student.Firstname = dr["firstname"].ToString();
                            student.Middlename = dr["middlename"].ToString();
                            student.Lastname = dr["lastname"].ToString();
                            student.BirthDate = dr["birthdate"].ToString();
                            student.BirthPlace = dr["birthplace"].ToString();
                            student.Age = int.Parse(dr["age"].ToString());
                            student.Gender = dr["gender"].ToString();
                            student.CivilStatus = dr["civilstatus"].ToString();
                            student.Religion = dr["religion"].ToString();
                            student.Citizenship = dr["citizenship"].ToString();
                            student.Height = dr["height"].ToString();
                            student.Weight = dr["weight"].ToString();
                            student.ContactNumber = dr["contact_no"].ToString();
                            student.EmailAddress = dr["email_address"].ToString();
                            student.ResAddressBrgy = dr["res_address_brgy"].ToString();
                            student.ResAddressMun = dr["res_address_mun"].ToString();
                            student.ResAddressProvince = dr["res_address_prov"].ToString();
                            student.ResAddressZipCode = dr["res_address_zipcode"].ToString();
                            student.PerAddressBrgy = dr["per_address_brgy"].ToString();
                            student.PerAddressMun = dr["per_address_mun"].ToString();
                            student.PerAddressProvince = dr["per_address_prov"].ToString();
                            student.PerAddressZipCode = dr["per_address_zipcode"].ToString();
                            student.Status = int.Parse(dr["status"].ToString());
                        }
                       
                        return student;
                    }
                    else
                    {
                        return null;
                    }
                }
                return student;
            }
            finally
            {
                dbService.Close();
            }
            
        }

        public string GetLastId()
        {
            try
            {
                //check if database connection is open
                if (dbService.Open())
                {
                    string lastId = null;
                    string query = "select id + 1 as 'id' from tblstudent ORDER BY id DESC LIMIT 1";
                    dr = dbService.ExecuteReader(query);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lastId = dr["id"].ToString();
                        }
                        return lastId;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            finally
            {
                dbService.Close();
            }
        }
        public string GetPhotoUrlPath(int studentId)
        {
            try
            {
                string path="";
                if (dbService.Open())
                {
                    dr = dbService.ExecuteReader("SELECT * FROM tblstudent_photos WHERE student_id = '" + studentId + "' ORDER BY id DESC LIMIT 1");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            path = dr["path"].ToString();
                            Console.WriteLine(path);
                        }
                        return path;
                    }
                    else
                    {
                        return "";
                    }
                }
                return path;
            }
            finally
            {
                dbService.Close();
            }
        }

    }
}
