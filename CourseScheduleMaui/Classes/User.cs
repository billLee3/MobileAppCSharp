using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSchedule.Classes
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }

        
        public int TermID { get; set; }

        public User() { }
        public User(string email, string password) { 
            Email = email;
            Password = password;
        }
    }
}
