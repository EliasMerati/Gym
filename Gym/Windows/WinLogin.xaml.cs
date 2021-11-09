using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DataLayer;
using Gym.Utilitys;
using Microsoft.Win32;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinLogin.xaml
    /// </summary>
    public partial class WinLogin : Window
    {
        public WinLogin()
        {
            InitializeComponent();
            Timer();
            WinLock winLock = new WinLock();
            Hide();
            winLock.ShowDialog();
            if (winLock.DialogResult == true)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("با تشکر از حسن انتخاب شما", "پیام تشکر", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(0);
            }
        }

        private void timer_Tick(object sender, EventArgs e) => Lblhour.Content = DateTime.Now.ToString("HH:mm:ss");

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e) => Environment.Exit(0);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {
                var g = db.People.Any();
                if (g)
                {
                    var d = db.vw_shahrieh.ToList();

                    foreach (var people in d)
                    {

                        SmsSender sms = new SmsSender();
                        if (people.ShahriehOUT == DateTime.Now.date())
                        {
                            sms.SendMessage(people.PeopleMobile, "ورزشکار گرامی ، تاریخ یک ماهه ی شهریه ی شما تمام شده است. لطفا جهت تمدید شهریه به باشگاه مراجعه کنید... با تشکر");
                            MessageBox.Show($"تاریخ شهریه ی {people.PeopleName} به پایان رسیده است");
                        }
                    }
                }

                var qv = db.Login.Any();
                try
                {
                    if (qv == false)
                    {
                        MessageBox.Show("لطفا نام کاربری و پسورد خود را از طریق لینک پایین ثبت کنید و دوباره وارد شوید ");
                        lbllogin.Visibility = Visibility.Visible;
                        Btninsert.IsEnabled = false;
                        TxtPassword.IsEnabled = false;
                        TxtUsername.IsEnabled = false;
                    }
                    else if (qv)
                    {

                        lbllogin.Visibility = Visibility.Hidden;
                        Btninsert.IsEnabled = true;
                        TxtPassword.IsEnabled = true;
                        TxtUsername.IsEnabled = true;
                    }

                }
                catch
                {
                    MessageBox.Show("لطفا نام کاربری و پسورد خود را از طریق لینک پایین ثبت کنید و دوباره وارد شوید ");
                    lbllogin.Visibility = Visibility.Visible;
                    Btninsert.IsEnabled = false;
                    TxtPassword.IsEnabled = false;
                    TxtUsername.IsEnabled = false;
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////
            LblDate.Content = DateTime.Now.date();
            /////////////////////////////////////////////////////////////دستورات رجیستری
            RegistryKey usernamekey = Registry.CurrentUser.CreateSubKey("software\\Gym");
            TxtUsername.Text = (string)usernamekey.GetValue("usernameregister");
            ////////////////////////////////////////////////////فکوس روی فیلد پسورد
            TxtPassword.Focus();

        }

        private void Timer()
        {
            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Tick += new EventHandler(timer_Tick);
            tmr.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            tmr.Start();
        }

        private void img_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {

                var g = db.Login.ToList();

                if (g.Count != 0)
                {

                    ///////////////////////////////////////////////////////// کار با الگوریتم های رمزنگاری برای پسورد
                    SHA256CryptoServiceProvider sha2 = new SHA256CryptoServiceProvider();
                    byte[] S1;
                    byte[] S2;
                    S1 = UTF8Encoding.UTF8.GetBytes(TxtPassword.Password.Trim());
                    S2 = sha2.ComputeHash(S1);
                    string F = BitConverter.ToString(S2);

                    /////////////////////////////////////////////////////////////////////////// چک کردن یوزر نیم و پسورد با دیتابیس
                    if (TxtPassword.Password != null)
                    {
                        var query = from u in db.Login
                                    where u.LoginUserName == TxtUsername.Text.Trim()
                                    select u;
                        var r = query.ToList();

                        if (r[0].LoginPassword == F)
                        {
                            if (r.Count == 1)
                            {
                                RegistryKey usernamekey = Registry.CurrentUser.CreateSubKey("software\\Gym");
                                try
                                {
                                    usernamekey.SetValue("usernameregister", TxtUsername.Text.Trim());
                                    ////////////////////////////////////////////////////////////////////////////// داینامیک کردن متغیر های عمومی در کل برنامه
                                    Public.user = g[0].LoginName;
                                }
                                catch
                                {
                                    MessageBox.Show("در سیستم ورود مشکلی پیش آمده است");
                                }
                                finally
                                {
                                    usernamekey.Close();
                                }

                                Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("رمز وارد شده اشتباه است ، لطفا مجددا تلاش کنید");
                            TxtPassword.Password = string.Empty;
                            TxtPassword.Focus();
                        }

                    }
                }
                else
                {
                    try
                    {
                        if (TxtUsername.Text == "admin" && TxtPassword.Password == "admin")
                        {
                            Public.user = "کاربر پیش فرض";
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("با حروف کوچک وارد کنید و زبان کیبورد هم انگلیسی باشد");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("با حروف کوچک وارد کنید و زبان کیبورد هم انگلیسی باشد");
                    }
                }


            }
        }

        private void TxtUsername_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void lbllogin_MouseDown(object sender, MouseButtonEventArgs e) => new WinAddLogin().ShowDialog();

    }

}
