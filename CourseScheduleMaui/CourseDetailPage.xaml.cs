using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class CourseDetailPage : ContentPage
    {


        int selectedStatus;
        Course SelectedCourse;


        public CourseDetailPage(Course selectedCourse)
        {
            InitializeComponent();
            SelectedCourse = selectedCourse;
            courseNameEntry.Text = selectedCourse.CourseName;
            startDatePicker.Date = selectedCourse.StartDate;
            endDatePicker.Date = selectedCourse.EndDate;
            courseStatusPicker.SelectedIndex = selectedCourse.CourseStatus;
            courseDetailsEntry.Text = selectedCourse.Details;
            notesEntry.Text = selectedCourse.Notes;
            if (selectedCourse.NotificationsOn == true)
            {
                yes.IsChecked = true;
            }
            else
            {
                no.IsChecked = true;
            }

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Assessment>();
                var assessmentTable = conn.Table<Assessment>().ToList();
                var associatedAssessments = (from assessment in assessmentTable
                                             where assessment.CourseID == SelectedCourse.ID
                                             select assessment).ToList();
                assessListView.ItemsSource = associatedAssessments;
            }





        }

        private void updateCourseButton_Clicked(object sender, EventArgs e)
        {
            SelectedCourse.CourseName = courseNameEntry.Text;
            SelectedCourse.StartDate = startDatePicker.Date;
            SelectedCourse.EndDate = endDatePicker.Date;
            SelectedCourse.CourseStatus = courseStatusPicker.SelectedIndex;
            SelectedCourse.Details = courseDetailsEntry.Text;
            SelectedCourse.Notes = notesEntry.Text;
            if(yes.IsChecked == true)
            {
                SelectedCourse.NotificationsOn = true;
            }
            else
            {
                SelectedCourse.NotificationsOn = false;
            }

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Course>();
                conn.Update(SelectedCourse);

                Navigation.PushAsync(new AllCourses());

            }

        }

        private void deleteCourseButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Assessment>();
                var assessmentTable = conn.Table<Assessment>().ToList();
                var associatedAssessments = (from assessment in assessmentTable
                                             where assessment.CourseID == SelectedCourse.ID
                                             select assessment).ToList();
                foreach (Assessment assessment in associatedAssessments)
                {
                    conn.Delete(assessment);
                }

                conn.CreateTable<Course>();
                conn.Delete(SelectedCourse);



                Navigation.PushAsync(new MainPage());

            }
        }

        private void courseStatusPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker courseStatusPicker = (Picker)sender;
            selectedStatus = courseStatusPicker.SelectedIndex;
        }



        private void assessListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedAssessment = assessListView.SelectedItem as Assessment;

            if (selectedAssessment != null)
            {
                Navigation.PushAsync(new AssessmentDetailPage(selectedAssessment));
            }
        }

        private void sendNotesSMS_Clicked(object sender, EventArgs e)
        {
            if (notesEntry.Text != "")
            {
                Navigation.PushAsync(new SMSPage(notesEntry.Text, SelectedCourse.ID));
            }
        }
    }
}