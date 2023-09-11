using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class TermDetailPage : ContentPage
    {
        Term SelectedTerm;
        List<Course> AssociatedCourses;


        public TermDetailPage(Term selectedTerm)
        {
            InitializeComponent();
            SelectedTerm = selectedTerm;
            termNameEntry.Text = selectedTerm.TermName;
            startDate.Date = selectedTerm.StartDate;
            endDate.Date = selectedTerm.EndDate;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Course>();
                var courseTable = conn.Table<Course>().ToList();
                var associatedCourses = (from course in courseTable
                                         where course.TermID == selectedTerm.ID
                                         select course).ToList();
                AssociatedCourses = associatedCourses;

            }

            associatedCoursesListView.ItemsSource = AssociatedCourses;
        }

        private void updateButton_Clicked(object sender, EventArgs e)
        {
            SelectedTerm.TermName = termNameEntry.Text;
            SelectedTerm.StartDate = startDate.Date;
            SelectedTerm.EndDate = endDate.Date;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Term>();
                int rows = conn.Update(SelectedTerm);

                if (rows > 0)
                {
                    DisplayAlert("Success", "Term successfully updated.", "Exit");
                    Navigation.PushAsync(new MainPage());
                }
                else
                {
                    DisplayAlert("Error", "Term wasn't successfully updated. Ensure all entries are filled", "Exit");
                }
            }
        }

        private void deleteButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Course>();

                var courseTable = conn.Table<Course>().ToList();
                var associatedCourses = (from course in courseTable
                                         where course.TermID == SelectedTerm.ID
                                         select course).ToList();
                foreach (Course course in associatedCourses)
                {
                    conn.CreateTable<Assessment>();
                    var assessmentTable = conn.Table<Assessment>().ToList();
                    var associatedAssessment = (from assess in assessmentTable
                                                where assess.CourseID == course.ID
                                                select assess).ToList();
                    foreach (Assessment assessment in associatedAssessment)
                    {
                        conn.Delete(assessment);
                    }
                    conn.Delete(course);
                }

                conn.CreateTable<Term>();
                conn.Delete(SelectedTerm);
                Navigation.PushAsync(new MainPage());

            }
        }

        private void associatedCoursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCourse = associatedCoursesListView.SelectedItem as Course;

            if (selectedCourse != null)
            {
                Navigation.PushAsync(new CourseDetailPage(selectedCourse));
            }
        }

        private void addCourse_Clicked(object sender, EventArgs e)
        {

            if (AssociatedCourses.Count < 6)
            {
                Navigation.PushAsync(new AddCoursePage(SelectedTerm.ID));
            }
            else
            {
                DisplayAlert("Warning", "You can only have 6 courses per term. ", "Ok");
            }

        }


    }
}