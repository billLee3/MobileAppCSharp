using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class AddAssessPage : ContentPage
    {


        string type;
        int courseID;
        List<Assessment> AssociatedAssessments;
        public AddAssessPage()
        {
            InitializeComponent();
        }

        public AddAssessPage(string type, int courseID)
        {
            InitializeComponent();
            this.type = type;
            this.courseID = courseID;
            AssessmentLabel.Text = "Add " + type + " Assessment Form";

        }

        private void addAssessButton_Clicked(object sender, EventArgs e)
        {
            Assessment finalAssessment;
            if (yes.IsChecked)
            {
                Assessment assessment = new Assessment(type, assessNameEntry.Text, assessDescEntry.Text, notesEntry.Text, startDatePicker.Date, dueDatePicker.Date, true, courseID);
                finalAssessment = assessment;
            }
            else
            {
                Assessment assessment = new Assessment(type, assessNameEntry.Text, assessDescEntry.Text, notesEntry.Text, startDatePicker.Date, dueDatePicker.Date, false, courseID);
                finalAssessment = assessment;
            }
            

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Assessment>();
                conn.Insert(finalAssessment);
                var assessmentTable = conn.Table<Assessment>().ToList();
                var associatedAssessments = (from assess in assessmentTable
                                             where assess.CourseID == courseID
                                             select assess).ToList();
                AssociatedAssessments = associatedAssessments;
            }
            int assessmentCount = AssociatedAssessments.Count();
            if (assessmentCount >= 2)
            {
                Navigation.PushAsync(new MainPage());
            }
            else
            {

                if (type == "Perfomance")
                {
                    Navigation.PushAsync(new AddAssessPage("Objective", courseID));
                }
                else if (type == "Objective")
                {
                    Navigation.PushAsync(new AddAssessPage("Performance", courseID));
                }
            }


        }


    }
}