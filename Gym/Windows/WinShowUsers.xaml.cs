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
    /// Interaction logic for WinShowUsers.xaml
    /// </summary>
    public partial class WinShowUsers : Window
    {
        public WinShowUsers()
        {
            InitializeComponent();
        }

        private Gym_DBEntities db = new Gym_DBEntities();
        public int PID { get; set; }
        private void BindGrid()
        {
            DgvPeople.ItemsSource = db.People.OrderByDescending(k => k.PeopleName).ToList();
        }
        private void people_click(object sender, RoutedEventArgs e)
        {
            new Winusers().ShowDialog();
        }

        private void period_click(object sender, RoutedEventArgs e)
        {
            new WinShowPeriods().ShowDialog();
        }

        private void program_click(object sender, RoutedEventArgs e)
        {
            new WinShowProgram().ShowDialog();
        }

        private void edit_click(object sender, RoutedEventArgs e)
        {
            var f = db.People.Any();
            if (f)
            {
                object item = DgvPeople.SelectedItem;
                if (item != null)
                {
                    Winusers user = new Winusers();
                    int id = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    user.peopleID = id;
                    BindGrid();
                    user.ShowDialog();

                }
                else
                {
                    MessageBox.Show("لطفا کاربر را انتخاب کنید");
                }
            }
            else
            {
                return;
            }
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            var f = db.People.Any();
            if (f)
            {
                object item = DgvPeople.SelectedItem;
                if (item != null)
                {
                    string name = (DgvPeople.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                    int id = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    if (MessageBox.Show($"آیا از حذف {name} مطمئن هستید؟", "توجه", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        db.deletePeople(id);
                        db.SaveChanges();
                        BindGrid();
                    }

                }
                else
                {
                    MessageBox.Show("لطفا کاربری را انتخاب کنید");
                }
            }
            else
            {
                return;
            }

        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object item = DgvPeople.SelectedItem;
                if (item != null)
                {
                    Logs log = new Logs()
                    {
                        LogDateTimeIN = DateTime.Now.date() + "_" + $"{DateTime.Now:HH:mm:ss}",
                        PeopleID = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text)
                    };
                    db.Logs.Add(log);
                    db.SaveChanges();
                    check();
                }
            }
            catch
            {
                MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است، لطفا دوباره امتحان کنید");
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            object item = DgvPeople.SelectedItem;
            try
            {
                if (item != null)
                {
                    int id = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    string date = DateTime.Now.date() + "_" + $"{DateTime.Now:HH:mm:ss}";
                    db.ExitPersonDate(id, date);
                    db.SaveChanges();
                    check();
                }
                else if (item == null)
                {
                    MessageBox.Show("لطفا کاربر را انتخاب کنید");
                }

            }
            catch
            {
                MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است، لطفا دوباره امتحان کنید");
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindGrid();
            Btninsert.IsEnabled = false;
            BtnExit.IsEnabled = false;
            BtnCart.IsEnabled = false;
        }

        private void log_click(object sender, RoutedEventArgs e)
        {
            var f = db.People.Any();
            if (f)
            {
                object item = DgvPeople.SelectedItem;
                if (item != null)
                {
                    WinShowLogs logs = new WinShowLogs();
                    string name = (DgvPeople.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                    int id = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    logs.Name = name;
                    logs.ID = id;
                    logs.ShowDialog();
                    BindGrid();
                }
                else if (item == null)
                {
                    MessageBox.Show("لطفا کاربر را انتخاب کنید");
                }
            }
            else
            {
                return;
            }

        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgvPeople.ItemsSource = db.People.Where(u => u.PeopleAddress.Contains(TxtFilter.Text) || u.PeopleMellicode.Contains(TxtFilter.Text)
                                                                                                  || u.PeopleMobile.Contains(TxtFilter.Text) ||
                                                                                                  u.PeopleName.Contains(TxtFilter.Text)).ToList();
        }

        private void newprogram_click(object sender, RoutedEventArgs e)
        {
            var f = db.People.Any();
            if (f)
            {
                object item = DgvPeople.SelectedItem;
                try
                {
                    if (item != null)
                    {
                        db.insertprogramnew(int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text),
                                             DateTime.Now.date());
                        WinAddNewProgram winAddNewProgram = new WinAddNewProgram();
                        winAddNewProgram.peopleid = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                        winAddNewProgram.name = (DgvPeople.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                        winAddNewProgram.ShowDialog();
                    }
                    else if (item == null)
                    {
                        MessageBox.Show("لطفا کاربر را انتخاب کنید");
                    }

                }
                catch (Exception l)
                {
                    MessageBox.Show(l.Message);
                }
            }
            else
            {
                return;
            }


        }

        private void newperiod_click(object sender, RoutedEventArgs e)
        {
            var f = db.People.Any();
            if (f)
            {
                object item = DgvPeople.SelectedItem;
                try
                {
                    if (item != null)
                    {
                        db.InsertPeriod(int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text),
                        DateTime.Now.date());
                        WinAddPeriod winAdd = new WinAddPeriod();
                        winAdd.peopleid = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                        winAdd.name = (DgvPeople.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                        winAdd.ShowDialog();
                    }
                    else if (item == null)
                    {
                        MessageBox.Show("لطفا کاربر را انتخاب کنید");
                    }
                }
                catch (Exception l)
                {
                    MessageBox.Show(l.Message);
                }
            }
            else
            {
                return;
            }

        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                var Report = StiReport.CreateNewReport();

                var item = DgvPeople.SelectedItem;
                if (item != null)
                {
                    PID = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    Report["@id"] = PID;
                    Report.Load(path + @"/Kart.mrt");
                    Report.RenderWithWpf();
                    Report.ShowWithWpf();
                }
                else if (item == null)
                {
                    MessageBox.Show("لطفا کاربر را انتخاب کنید");
                }


            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
        }

        private void DgvPeople_SelectionChanged(object sender, SelectionChangedEventArgs e) => check();

        void check()
        {
            var f = db.People.Any();
            if (f)
            {
                var item = DgvPeople.SelectedItem;
                int LID = int.Parse((DgvPeople.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                if (item != null)
                {
                    var h = db.lastLogOut(LID).ToList();
                    if (h[0].LogDateTimeOut != null && h[0].LogDateTimeIN != null)
                    {
                        Btninsert.IsEnabled = true;
                        BtnExit.IsEnabled = false;
                    }
                    else if (h[0].LogDateTimeOut == null && h[0].LogDateTimeIN != null)
                    {
                        Btninsert.IsEnabled = false;
                        BtnExit.IsEnabled = true;
                    }
                    else
                    {
                        Btninsert.IsEnabled = false;
                        BtnExit.IsEnabled = false;
                        BtnCart.IsEnabled = false;
                    }
                    BtnCart.IsEnabled = true;
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }
        }

    }
}
