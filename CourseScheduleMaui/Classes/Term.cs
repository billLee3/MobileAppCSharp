using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace CourseSchedule.Classes
{
    
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(100)]
        public string TermName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Term()
        {

        }

        public Term(string termName, DateTime startDate, DateTime endDate)
        {
            TermName = termName;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
