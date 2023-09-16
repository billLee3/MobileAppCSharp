using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSchedule.Classes
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Type { get; set; }
        public string AssessmentName { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool NotificationsOn { get; set; }
        public int CourseID{ get; set; }

        public Assessment()
        {

        }

        public Assessment(string type, string assessmentName, string description, string notes, DateTime startDate, DateTime endDate, bool notifications, int courseID)
        {
            Type = type;
            AssessmentName = assessmentName;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Notes = notes;
            NotificationsOn = notifications;
            CourseID = courseID;
        }

    }
}
