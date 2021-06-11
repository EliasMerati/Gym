using System.Windows;
using System.Windows.Input;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for Us.xaml
    /// </summary>
    public partial class Us : Window
    {
        public Us()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
