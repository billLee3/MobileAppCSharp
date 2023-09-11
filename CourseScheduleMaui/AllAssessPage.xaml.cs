using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class AllAssessPage : ContentPage
    {


        public AllAssessPage()
        {
            InitializeComponent();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Assessment>();
                var assessments = conn.Table<Assessment>().ToList();
                assessListView.ItemsSource = assessments;
            }

        }

        private void addAssessmentButton_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new AddAssessPage());
        }

        private void assessListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedAssess = assessListView.SelectedItem as Assessment;

            if (selectedAssess != null)
            {
                Navigation.PushAsync(new AssessmentDetailPage(selectedAssess));
            }
        }

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}