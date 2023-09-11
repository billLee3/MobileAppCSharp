using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //createDataSet();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Term>();
                var terms = conn.Table<Term>().ToList();
                termListView.ItemsSource = terms;
            }


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                conn.CreateTable<Assessment>();
                List<Assessment> allAssessments = conn.Query<Assessment>("SELECT * FROM Assessment");

                foreach (Assessment a in allAssessments)
                {
                    if (a.DueDate == DateTime.Today)
                    {

                        var request = new NotificationRequest
                        {
                            Title = "Assessment Notification",
                            Description = a.AssessmentName + " for course ID " + a.CourseID + " is due today!",
                            BadgeNumber = 42
                        };

                        LocalNotificationCenter.Current.Show(request);
                        //Code retrieved from https://github.com/edsnider/LocalNotificationsPlugin
                        //CrossLocalNotifications.Current.Show("Assessment Notification ", a.AssessmentName + " for course ID " + a.CourseID + " is due today!", 100);

                    }
                }

                conn.CreateTable<Course>();
                List<Course> allCourses = conn.Query<Course>("SELECT * FROM Course");
                foreach (Course c in allCourses)
                {
                    if (c.StartDate == DateTime.Today)
                    {
                        var request = new NotificationRequest
                        {
                            Title = "Course Notification",
                            Description = c.CourseName + " begins today!",
                            BadgeNumber = 42
                        };
                        LocalNotificationCenter.Current.Show(request);
                        //CrossLocalNotifications.Current.Show("Course Notification", c.CourseName + " begins today!", 101);
                    }
                    if (c.EndDate == DateTime.Today)
                    {
                        var request = new NotificationRequest
                        {
                            Title = "Course Notification",
                            Description = c.CourseName + " ends today!",
                            BadgeNumber = 42
                        };
                        LocalNotificationCenter.Current.Show(request);
                        //CrossLocalNotifications.Current.Show("Course Notification", c.CourseName + " ends today!", 102);
                    }
                }

            }
        }

        

        

        private void assessButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AllAssessPage());
        }

        private void termListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedTerm = termListView.SelectedItem as Term;

            if (selectedTerm != null)
            {
                Navigation.PushAsync(new TermDetailPage(selectedTerm));
            }
        }

        private void addTermButton_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new AddTermPage());
        }

        private void allCourses_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new AllCourses());
        }

        private void allAssess_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AllAssessPage());
        }

        private void createDataSet()
        {
            Term term = new Term("Fall", DateTime.Today, DateTime.Today.AddDays(30));

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Term>();
                conn.Insert(term);


                Course course = new Course("Mobile Development", DateTime.Today, DateTime.Today.AddDays(30), 1, "Course about Xamarin", "Consistent practice helps", term.ID, "anika", "5551234567", "anika.patel@strimeuniversity.edu");

                conn.CreateTable<Course>();
                conn.Insert(course);

                Assessment oA = new Assessment("Objective", "Test", "A test on Xamarin", "You got this!", DateTime.Today.AddDays(30), course.ID);
                Assessment pA = new Assessment("Performance", "Course Schedule App", "An application to track student schedules", "You got this!", DateTime.Today.AddDays(30), course.ID);

                conn.CreateTable<Assessment>();
                conn.Insert(oA);
                conn.Insert(pA);

            }
        }
    }
}