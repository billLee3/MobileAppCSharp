using SQLite;
using CourseSchedule.Classes;
using Plugin.LocalNotification;

namespace CourseScheduleMaui
{
    public partial class SMSPage : ContentPage
    {

        public static int associatedAssess;
        public static string Note;
        public SMSPage(string note, int assessmentID)
        {
            InitializeComponent();
            associatedAssess = assessmentID;
            Note = note;
            DisplayAlert("Test", note, "Ok");
        }

        private async void sendSMS_Clicked(object sender, EventArgs e)
        {
            //Gathered from Microsoft Docs on Xamarin Essentials. 
            string messageText = Note;
            string recipient = receipentPhone.Text;

            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });

                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Console.WriteLine("Unknown error: " + ex.Message);
            }

        }

        private void cancelButton_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new MainPage());
        }


    }
}