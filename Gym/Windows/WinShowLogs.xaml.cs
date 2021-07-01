using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataLayer;
using Telerik.Windows.Data;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinShowLogs.xaml
    /// </summary>
    public partial class WinShowLogs : Window
    {
        public WinShowLogs()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();

        public new string Name { get; set; }
        public int ID { get; set; }

        private void showimage()/////////////////////////////////////////////////  ایجاد تصویر از دیتابیس در هنگام لود صفحه اصلی
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {
                var queryList = db.People.Where(y => y.PeopleID == ID).ToList();
                if (queryList[0].PeopleLogo != null)
                {
                    byte[] imagearray = (byte[])queryList[0].PeopleLogo;
                    MemoryStream strim = new MemoryStream();
                    strim.Write(imagearray, 0, imagearray.Length);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(strim);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();

                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms;
                    bi.EndInit();
                    imgUser.Source = bi;
                }
                else
                {
                    return;
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var h = db.Logs.Where(m => m.PeopleID == ID).ToIList();
            if (h.Count != 0)
            {
                try
                {

                    var s = db.People.Where(y => y.PeopleID == ID).ToList();
                    if (s[0].PeopleCreditor != 0 || s[0].PeopleDeptor != 0)
                    {
                        lbldeptor.Content = (s[0].PeopleDeptor - s[0].PeopleCreditor).ToString("n0");
                    }
                    lbldeptor.Foreground = Brushes.Crimson;
                    lblName.Content = Name;
                    showlog();
                    showimage();
                }
                catch
                {
                    MessageBox.Show("در واکشی اطلاعات مشکلی به وجود آمده");
                }
            }
            else
            {
                MessageBox.Show("ورود خروجی برای این کاربر ثبت نشده است");
            }

        }

        private void showlog() => DgvLogs.ItemsSource = db.vw_log.Where(y => y.PeopleID == ID).OrderByDescending(y => y.LogDateTimeIN).ToList();
        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();



        private void Btnshowprograms_Click(object sender, RoutedEventArgs e)
        {
            WinShowProgram winShowProgram = new WinShowProgram();
            winShowProgram.id = ID;
            winShowProgram.name = Name;
            winShowProgram.ShowDialog();
        }

        private void Btnshowperiods_Click(object sender, RoutedEventArgs e)
        {
            WinShowPeriods winShowPeriod = new WinShowPeriods();
            winShowPeriod.name = Name;
            winShowPeriod.id = ID;
            winShowPeriod.ShowDialog();
        }

        private void Btnpey_Click(object sender, RoutedEventArgs e)
        {
            WinAddPeyment win = new WinAddPeyment();
            win.cash = int.Parse(lbldeptor.Content.ToString());
            win.id = ID;
            win.ShowDialog();
        }

        private void BtnShowpayment_Click(object sender, RoutedEventArgs e)
        {
            WinShowPeyment winShowPeyment = new WinShowPeyment();
            winShowPeyment.Id = ID;
            winShowPeyment.Name = Name;
            winShowPeyment.ShowDialog();
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e) => DgvLogs.ItemsSource = db.Logs.Where(u => u.LogDateTimeIN.Contains(TxtFilter.Text)
                                                                                                                      || u.LogDateTimeOut.Contains(TxtFilter.Text)).Where(i => i.PeopleID == ID).ToList();

        private void BtnShahrieh_Click(object sender, RoutedEventArgs e)
        {
            db.insertshahriehdate(ID, DateTime.Now.date(), DateTime.Now.date2());
            MessageBox.Show("تاریخ شهریه جدید ثبت شد");
        }

        private void BtnProgramFile_Click(object sender, RoutedEventArgs e)
        {
            WinAddUtility win = new WinAddUtility();
            win.ID = ID;
            win.ShowDialog();

        }

        private void lblname_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
