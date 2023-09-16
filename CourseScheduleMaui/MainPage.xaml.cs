using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        bool emptyArray = true;
        List<NotificationRequest> requests = new List<NotificationRequest>();
        List<String> detailEntries = new List<String>();
        bool courseNotificationNeeded = false;
        bool assessNotificationNeeded = false;
        string details = string.Empty;
        string title = string.Empty;
        public MainPage()
        {
            InitializeComponent();
        }

        
        protected override async void OnAppearing()
        {
            base.OnAppearing();


            //CODE
            //PLEASE UNCOMMENT THE LINE BELOW TO GENERATE DATA ON THE OPENING OF THE APP. 
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

                

                conn.CreateTable<Course>();
                List<Course> allCourses = conn.Query<Course>("SELECT * FROM Course");
                courseNotifications(allCourses);
                    
                foreach (Assessment a in allAssessments)
                {
                    if(a.NotificationsOn == true)
                    {
                        if (a.StartDate == DateTime.Today)
                        {
                            assessNotificationNeeded = true;
                            string text = "Assessment: " + a.AssessmentName + " for course ID " + a.CourseID + " is due today!";
                            var request = new NotificationRequest
                            {
                                Title = "Assessment Notification",
                                Description = a.AssessmentName + " for course ID " + a.CourseID + " is due today!",
                                BadgeNumber = 39,

                            };
                            detailEntries.Add(text);
                            //await LocalNotificationCenter.Current.Show(request);
                            //Code retrieved from https://github.com/edsnider/LocalNotificationsPlugin
                            //CrossLocalNotifications.Current.Show("Assessment Notification ", a.AssessmentName + " for course ID " + a.CourseID + " is due today!", 100);
                        }
                        if (a.EndDate == DateTime.Today)
                        {
                            assessNotificationNeeded = true;
                            string text = "Assessment: " + a.AssessmentName + " for course ID " + a.CourseID + " starts today!";
                            detailEntries.Add(text);
                        }
                    }
                        
                }

                foreach(string entry in detailEntries)
                {
                    string text = $"{entry} \n";
                    details = details + text;
                }

                if (courseNotificationNeeded == true && assessNotificationNeeded == true)
                {
                    title = "Course and Assessment Notification";
                }else if (courseNotificationNeeded == true && assessNotificationNeeded == false)
                {
                    title = "Course Notification";
                }else if (courseNotificationNeeded == false && assessNotificationNeeded == true)
                {
                    title = "Assessment Notification";
                }

                if (title != string.Empty && details != string.Empty)
                {
                    var request = new NotificationRequest
                    {
                        Title = title,
                        Description = details,
                    };
                    await LocalNotificationCenter.Current.Show(request);
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
            Navigation.PushAsync(new AddTermPage());
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


                Course course = new Course("Mobile Development", DateTime.Today, DateTime.Today.AddDays(30), 1, "Course about Xamarin", "Consistent practice helps", term.ID, "anika", "5551234567", "anika.patel@strimeuniversity.edu", false);

                conn.CreateTable<Course>();
                conn.Insert(course);

                Assessment oA = new Assessment("Objective", "Test", "A test on Xamarin", "You got this!", DateTime.Today, DateTime.Today.AddDays(30), false, course.ID);
                Assessment pA = new Assessment("Performance", "Course Schedule App", "An application to track student schedules", "You got this!", DateTime.Today, DateTime.Today.AddDays(30), false, course.ID);

                conn.CreateTable<Assessment>();
                conn.Insert(oA);
                conn.Insert(pA);

            }
        }

        private void createData_Clicked(object sender, EventArgs e)
        {
            createDataSet();
            Navigation.PushAsync(new MainPage());
        }

       public async void courseNotifications(List<Course> allCourses)
        {
            foreach (Course c in allCourses)
            {
                if (c.NotificationsOn == true)
                {
                    if (c.StartDate == DateTime.Today && c.EndDate == DateTime.Today)
                    {
                        string text = "Course: " + c.CourseName + " begins today and ends today";
                        courseNotificationNeeded = true;
                        var request = new NotificationRequest
                        {
                            Title = "Start and End Course Notification",
                            Description = c.CourseName + " begins today and ends today!",
                            BadgeNumber = 41,

                        };
                        detailEntries.Add(text);
                        await LocalNotificationCenter.Current.Show(request);
                        //CrossLocalNotifications.Current.Show("Course Notification", c.CourseName + " begins today!", 101);
                    }
                    else if (c.EndDate == DateTime.Today && c.StartDate != DateTime.Today)
                    {
                        string text = "Course: " + c.CourseName + " ends today";
                        courseNotificationNeeded = true;
                        var request = new NotificationRequest
                        {
                            Title = "End Course Notification",
                            Description = c.CourseName + " ends today!",
                            BadgeNumber = 42,

                        };
                        detailEntries.Add(text);
                        //await LocalNotificationCenter.Current.Show(request);
                        //CrossLocalNotifications.Current.Show("Course Notification", c.CourseName + " ends today!", 102);
                    }
                    else if (c.StartDate == DateTime.Today && c.EndDate != DateTime.Today)
                    {
                        string text = "Course: " + c.CourseName + " starts today!";
                        courseNotificationNeeded = true;
                        var request = new NotificationRequest
                        {
                            Title = "Start Course Notification",
                            Description = c.CourseName + "starts today!",
                            BadgeNumber = 43,


                        };
                        detailEntries.Add(text);
                        //await LocalNotificationCenter.Current.Show(request);
                    }
                }    
                   
            }
        }
        

       
    }
}