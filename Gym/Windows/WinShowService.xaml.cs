using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using DataLayer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinShowService.xaml
    /// </summary>
    public partial class WinShowService : Window
    {
        public WinShowService()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();
        public int Id { get; set; }
        public string Name { get; set; }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e) => Close();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblname.Text = Name;
            
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
