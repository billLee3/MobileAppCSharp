using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseScheduleMaui.Classes
{
    public class Constants
    {
        public const string DatabaseFileName = "MauiCourseSchedule.db3";

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);

    }
}
