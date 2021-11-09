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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinGymServices.xaml
    /// </summary>
    public partial class WinGymServices : Window
    {
        public WinGymServices()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void TxtService_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void TxtServicePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
