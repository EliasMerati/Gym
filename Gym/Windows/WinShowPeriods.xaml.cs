using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataLayer;
using Stimulsoft.Report;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinShowPeriods.xaml
    /// </summary>
    public partial class WinShowPeriods : Window
    {
        public WinShowPeriods()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();
        public int id { get; set; }
        public int pid { get; set; }
        public string name { get; set; }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e) => Close();

        private void Window_Loaded(object sender, RoutedEventArgs e) => Dgvperiod.ItemsSource = db.Vw_Period.Where(k => k.peopleID == id).ToList();

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e) => Dgvperiod.ItemsSource = db.Vw_Period.Where(u => u.PeopleName.Contains(TxtFilter.Text) || u.Perioddate.Contains(TxtFilter.Text.Trim()) || u.PeriodDesc.Contains(TxtFilter.Text)).ToList();

        private void Show_click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                var Report = StiReport.CreateNewReport();

                var item = Dgvperiod.SelectedItem;
                if (item != null)
                {
                    pid = int.Parse((Dgvperiod.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);

                    Report["@id"] = pid;
                }

                Report.Load(path + @"/UserPeriod.mrt");
                Report.RenderWithWpf();
                Report.ShowWithWpf();
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
        }
    }
}
