using System.Linq;
using System.Windows;
using System.Windows.Input;
using DataLayer;
using Gym.Utilitys;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinSms.xaml
    /// </summary>
    public partial class WinSms : Window
    {
        public WinSms()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e) => Close();

        private void BtnSendSms_Click(object sender, RoutedEventArgs e)
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {
                SmsSender sms = new SmsSender();
                var people = db.People.ToList();
                string text = TxtSmsText.ToString();
                foreach (var person in people)
                {
                    sms.SendMessage(person.PeopleMobile,text);
                }
            }
        }
    }
}
