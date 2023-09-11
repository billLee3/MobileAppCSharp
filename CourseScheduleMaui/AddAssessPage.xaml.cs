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
            Assessment assessment = new Assessment(type, assessNameEntry.Text, assessDescEntry.Text, notesEntry.Text, dueDatePicker.Date, courseID);

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Assessment>();
                conn.Insert(assessment);
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