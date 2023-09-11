using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class AllCourses : ContentPage
    {


        public AllCourses()
        {
            InitializeComponent();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                conn.CreateTable<Course>();
                var courses = conn.Table<Course>().ToList();
                coursesListView.ItemsSource = courses;
            }
        }

        private void addCourseButton_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new AddCoursePage());
        }

        private void coursesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Course selectedCourse = coursesListView.SelectedItem as Course;
            DisplayAlert("Test", selectedCourse.ID.ToString(), "Ok");
            if (selectedCourse != null)
            {
                Course course = selectedCourse as Course;
                Navigation.PushAsync(new CourseDetailPage(course));
            }



        }

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}