using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class AddTermPage : ContentPage
    {
        public AddTermPage()
        {
            InitializeComponent();
        }

        private async void addTermButton_Clicked(object sender, EventArgs e)
        {
            Term term = new Term(TermNameEntry.Text, startDatePicker.Date, endDatePicker.Date);
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Term>();
                conn.Insert(term);
            }


            await Navigation.PushAsync(new AddCoursePage(term.ID));
        }

    }
}