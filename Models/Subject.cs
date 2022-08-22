
namespace StudentRecordMgntSystem.Models
{
    class Subject
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public int LectureUnit { get; set; }
        public int LabUnit { get; set; }
        public int Semester { get; set; }
        public string YearLevel { get; set; }
        public int InstructorId { get; set; }
        public int Status { get; set; }

        public Subject()
        {

        }
        public Subject(int Id, string CourseCode, string Description, int LectureUnit, 
            int LabUnit, int Semester, string YearLevel, int InstructorId, int Status)
        {
            this.Id = Id;
            this.CourseCode = CourseCode;
            this.Description = Description;
            this.LectureUnit = LectureUnit;
            this.LabUnit = LabUnit;
            this.Semester = Semester;
            this.YearLevel = YearLevel;
            this.InstructorId = InstructorId;
            this.Status = Status;
        }

        public Subject(int Id, string CourseCode, string Description, int LectureUnit, int LabUnit, int InstructorId)
        {
            this.Id = Id;
            this.CourseCode = CourseCode;
            this.Description = Description;
            this.LectureUnit = LectureUnit;
            this.LabUnit = LabUnit;
            this.InstructorId = InstructorId;
         
        }

        public Subject(int Id, string CourseCode, string Description)
        {
            this.Id = Id;
            this.CourseCode = CourseCode;
            this.Description = Description;
        }
    }
}
