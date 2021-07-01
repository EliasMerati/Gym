using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataLayer;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinShowPeyment.xaml
    /// </summary>
    public partial class WinShowPeyment : Window
    {
        public WinShowPeyment()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();
        public int Id { get; set; }
        public new string Name { get; set; }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblname.Text = Name;
            Dgvpeyment.ItemsSource = db.vw_Peyment.Where(h => h.PeopleID == Id ).OrderByDescending(k => k.Datetime).ToList();
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e) => Dgvpeyment.ItemsSource = db.Payment.Where(u => u.Datetime.Contains(TxtFilter.Text) || u.PaymentSave.Contains(TxtFilter.Text)
                                                                                                                            || u.PaymentStatus.Contains(TxtFilter.Text)).Where(i => i.PeopleID == Id).ToList();
    }
}
