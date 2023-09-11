using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;
using System.Text.RegularExpressions;

namespace CourseScheduleMaui
{
    public partial class AddCoursePage : ContentPage
    {


        int selectedStatus;
        int termID;
        public AddCoursePage()
        {
            InitializeComponent();
        }

        public AddCoursePage(int termID)
        {
            InitializeComponent();
            this.termID = termID;
        }

        private void addCourseButton_Clicked(object sender, EventArgs e)
        {

            if (IsValidInstructor(instructorEmailEntry.Text))
            {
                Course course = new Course(courseNameEntry.Text, startDatePicker.Date, endDatePicker.Date, selectedStatus, detailsEntry.Text, notesEntry.Text, termID, instructorEntry.Text, instructorPhoneEntry.Text, instructorEmailEntry.Text);

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Course>();
                    conn.Insert(course);

                }

                Navigation.PushAsync(new AddAssessPage("Objective", course.ID));
            }


        }



        private void courseStatusPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker courseStatusPicker = (Picker)sender;
            selectedStatus = courseStatusPicker.SelectedIndex;

        }

        //Reference from https://mailtrap.io/blog/validate-email-address-c/
        private bool IsValidInstructor(string email)
        {
            if (instructorEntry.Text == "")
            {
                DisplayAlert("Error", "Provide an instructor name", "Ok");
                return false;
            }
            if (instructorPhoneEntry.Text == "")
            {
                DisplayAlert("Error", "Provide an instructor phone number", "Ok");
                return false;
            }
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            bool validEmail = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);

            if (validEmail)
            {
                return true;
            }
            else
            {
                DisplayAlert("Error", "Provide a valid email address", "Ok");
                return false;
            }
        }


    }
}