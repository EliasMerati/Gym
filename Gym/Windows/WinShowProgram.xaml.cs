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
    /// Interaction logic for WinShowProgram.xaml
    /// </summary>
    public partial class WinShowProgram : Window
    {
        public WinShowProgram()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();
        public string name { get; set; }
        public int id { get; set; }
        public int PID { get; set; }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Show_click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                var Report = StiReport.CreateNewReport();

                var item = Dgvprogram.SelectedItem;
                if (item != null)
                {
                    PID = int.Parse((Dgvprogram.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    
                    Report["@id"] = PID;
                }
               
                Report.Load(path + @"/UserProgram.mrt");
                Report.RenderWithWpf();
                Report.ShowWithWpf();
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }

        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dgvprogram.ItemsSource = db.vw_newprogram.Where(u => u.PeopleName.Contains(TxtFilter.Text) || u.NewProgramDesc.Contains(TxtFilter.Text) || u.NewProgramDate.Contains(TxtFilter.Text)).ToList(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblname.Text = name;    
            Dgvprogram.ItemsSource = db.vw_newprogram.Where(l => l.PeopleID == id).ToList();
        }
    }
}
