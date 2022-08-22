
namespace StudentRecordMgntSystem.Models
{
    class Instructor
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string PositionLevel { get; set; }
        public Instructor()
        {

        }

        public Instructor(int Id, string Firstname, string Middlename, string Lastname, string PositionLevel)
        {
            this.Id = Id;
            this.Firstname = Firstname;
            this.Middlename = Middlename;
            this.Lastname = Lastname;
            this.PositionLevel = PositionLevel;
        }
    }
}
