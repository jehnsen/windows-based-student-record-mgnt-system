
namespace StudentRecordMgntSystem.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string PrnNumber { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Gender { get; set; }
        public string CivilStatus { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Citizenship { get; set; }
        public int Age { get; set; }
        public string Religion { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string ResAddressBrgy { get; set; }
        public string ResAddressMun { get; set; }
        public string ResAddressProvince { get; set; }
        public string ResAddressZipCode { get; set; }
        public string PerAddressBrgy { get; set; }
        public string PerAddressMun { get; set; }
        public string PerAddressProvince { get; set; }
        public string PerAddressZipCode { get; set; }
        public int Status { get; set; }
        public Student()
        {

        }

        public Student(int Id, string PrnNumber, string Firstname, string Middlename, string Lastname)
        {
            this.Id = Id;
            this.PrnNumber = PrnNumber;
            this.Firstname = Firstname;
            this.Middlename = Middlename;
            this.Lastname = Lastname;
        }
    }
}
