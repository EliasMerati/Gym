using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using DataLayer;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinAddLogin.xaml
    /// </summary>
    public partial class WinAddLogin : Window
    {
        public WinAddLogin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {
                try
                {
                    var d = db.Login.ToList();
                    if (d.Count > 0)
                    {
                        TxtAdmin.Text = d[0].LoginName;
                        TxtUserName.Text = d[0].LoginUserName;
                        TxtPassword.IsEnabled = false;
                    }
                }
                catch
                {
                    MessageBox.Show("در ویرایش اطلاعات مشکلی به وجود آمده");
                }
            }
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private bool Check()
        {
            if (TxtAdmin.Text == "")
            {
                MessageBox.Show("لطفا نام ادمین را وارد کنید ");
            }

            else if (TxtUserName.Text == "")
            {
                MessageBox.Show("لطفا نام کاربری وارد کنید ");
            }

            return true;
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {
                var query = db.Login.Any();
                if (query == false)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Close();
                }
            }  
        }

        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {
                var c = db.Login.Any();
                if (Check())
                {
                    try
                    {
                        if (c == false)
                        {
                            /////////////////////////////////////////////////////////// کار با الگوریتم های رمزنگاری برای پسورد
                            SHA256CryptoServiceProvider sha2 = new SHA256CryptoServiceProvider();
                            byte[] S1;
                            byte[] S2;
                            S1 = UTF8Encoding.UTF8.GetBytes(TxtPassword.Password.Trim());
                            S2 = sha2.ComputeHash(S1);
                            string F = BitConverter.ToString(S2);
                            /////////////////////////////////////////////////////////////////////////
                            db.InsertLogin(TxtUserName.Text.Trim(), F, TxtAdmin.Text.Trim());
                           
                            if (MessageBox.Show($"لطفا برنامه را دوباره اجرا کنید و با نام کاربری و پسورد جدید وارد شوید", "توجه", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                            {
                                Environment.Exit(0);
                                
                            }
                        }
                        else if (c)
                        {
                            db.UpdateLogin(TxtUserName.Text.Trim(), TxtAdmin.Text.Trim());
                        }
                        db.SaveChanges();
                    }
                    catch
                    {
                        MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است ، لطفا دوباره تلاش کنید");
                    }
                }
            }

        }

        private void TxtUserName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }
    }
}
