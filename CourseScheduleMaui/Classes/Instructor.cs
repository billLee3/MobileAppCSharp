using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CourseSchedule.Classes
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement] 
        public int ID {  get; set; }

        public string InstructorName { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
       
        
        public Instructor() { }

        public Instructor(string instructorName,  string phoneNumber, string email)
        {
            InstructorName = instructorName;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
