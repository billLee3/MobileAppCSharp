using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace CourseSchedule.Classes
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(100)]
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CourseStatus { get; set; }
        public string Details { get; set; }
        public string Notes { get; set; }
        [MaxLength(100)]
        
        public int TermID { get; set; }
        public string InstructorName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }



        public Course()
        {
        }

        public Course(string courseName, DateTime startDate, DateTime endDate, int courseStatus, string details, string notes, int termID, string instructorName, string phoneNum, string email)
        {
            CourseName = courseName;
            StartDate = startDate;
            EndDate = endDate;
            CourseStatus = courseStatus;
            Details = details;
            Notes = notes;
            TermID = termID;
            InstructorName = instructorName;
            PhoneNumber = phoneNum;
            Email = email;
        }
    }
}
