using CourseScheduleMaui.Classes;

namespace CourseScheduleMaui
{
    public partial class App : Application
    {
        public static string DatabaseLocation = Constants.DatabasePath;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        
    }
}