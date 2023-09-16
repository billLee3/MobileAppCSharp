using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;
//using UIKit;

namespace CourseScheduleMaui
{
    public partial class AssessmentDetailPage : ContentPage
    {


        List<Assessment> AssociatedAssessments;
        Assessment SelectedAssessment;
        public AssessmentDetailPage(Assessment selectedAssessment)
        {
            InitializeComponent();

            SelectedAssessment = selectedAssessment;
            typeLabel.Text = selectedAssessment.Type + " Assessment";
            assessNameEntry.Text = SelectedAssessment.AssessmentName;
            assessDescEntry.Text = SelectedAssessment.Description;
            notes.Text = SelectedAssessment.Notes;
            startDatePicker.Date = SelectedAssessment.StartDate;
            dueDatePicker.Date = SelectedAssessment.EndDate;
            if (SelectedAssessment.NotificationsOn == true)
            {
                yes.IsChecked = true;
            }
            else
            {
                no.IsChecked = true;
            }


        }

        private void updateButton_Clicked(object sender, EventArgs e)
        {

            SelectedAssessment.AssessmentName = assessNameEntry.Text;
            SelectedAssessment.Description = assessDescEntry.Text;
            SelectedAssessment.Notes = notes.Text;
            SelectedAssessment.StartDate = startDatePicker.Date;
            SelectedAssessment.EndDate = dueDatePicker.Date;
            if (yes.IsChecked)
            {
                SelectedAssessment.NotificationsOn = true;
            }
            else
            {
                SelectedAssessment.NotificationsOn = false;
            }


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Assessment>();
                conn.Update(SelectedAssessment);

                Navigation.PushAsync(new AllAssessPage());
            }
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            int courseID;
            string type;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Assessment>();
                courseID = SelectedAssessment.CourseID;
                type = SelectedAssessment.Type;
                conn.Delete(SelectedAssessment);

                var assessmentTable = conn.Table<Assessment>().ToList();
                var associatedAssessments = (from assess in assessmentTable
                                             where assess.CourseID == courseID
                                             select assess).ToList();
                AssociatedAssessments = associatedAssessments;
            }
            int assessmentCount = AssociatedAssessments.Count();

            if (assessmentCount >= 2)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Course>();
                    var CourseTable = conn.Table<Course>().ToList();
                    if (CourseTable != null)
                    {
                        try
                        {
                            Course Course = (from course in CourseTable where course.ID == courseID select course as Course).First();
                            Navigation.PushAsync(new CourseDetailPage(Course));
                        }
                        catch (Exception ex)
                        {
                            Navigation.PushAsync(new MainPage());
                        }

                    }
                    else
                    {
                        Navigation.PushAsync(new MainPage());
                    }

                }

            }
            else
            {

                if (type == "Performance")
                {
                    Navigation.PushAsync(new AddAssessPage("Performance", courseID));
                }
                else if (type == "Objective")
                {
                    Navigation.PushAsync(new AddAssessPage("Objective", courseID));
                }
            }


        }

        private void sendNotes_Clicked(object sender, EventArgs e)
        {
            if (notes.Text != "")
            {
                Navigation.PushAsync(new SMSPage(notes.Text, SelectedAssessment.ID));
            }
        }


    }
}