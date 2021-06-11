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
    /// Interaction logic for WinChangePass.xaml
    /// </summary>
    public partial class WinChangePass : Window
    {
        public WinChangePass()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void reset()
        {
            TxtPasswordold.Password = string.Empty;
            TxtPasswordnew.Password = string.Empty;
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            if (TxtPasswordold.Password == "" || TxtPasswordnew.Password == "")
            {
                MessageBox.Show("لطفا فیلد ها را پر کنید");
                return;
            }


            try
            {

                /////////////////////////////////////////////////////////// کار با الگوریتم های رمزنگاری برای پسورد
                SHA256CryptoServiceProvider sha_OldPass = new SHA256CryptoServiceProvider();
                byte[] S1OLD;
                byte[] S2OLD;
                S1OLD = UTF8Encoding.UTF8.GetBytes(TxtPasswordold.Password.Trim());
                S2OLD = sha_OldPass.ComputeHash(S1OLD);
                string OldPass = BitConverter.ToString(S2OLD);
                ///////////////////////////////////////////////////////////

                var query = db.Login.ToList().Where(r => r.LoginName == Public.user &&
                                                                  r.LoginPassword == OldPass);
                var result = query.ToList();
                if (result.Count == 0)
                {
                    MessageBox.Show("رمز عبور قدیمی اشتباه است");
                }
                else if (result.Count == 1)
                {
                    /////////////////////////////////////////////////////////// کار با الگوریتم های رمزنگاری برای پسورد
                    SHA256CryptoServiceProvider sha_NewPass = new SHA256CryptoServiceProvider();
                    byte[] S1New;
                    byte[] S2New;
                    S1New = UTF8Encoding.UTF8.GetBytes(TxtPasswordnew.Password.Trim());
                    S2New = sha_NewPass.ComputeHash(S1New);
                    string NewPass = BitConverter.ToString(S2New);
                    ///////////////////////////////////////////////////////////// عملیات آپدیت پسورد جدید
                    var UpdatePass = db.Login.SingleOrDefault(u => u.LoginName == Public.user);

                    UpdatePass.LoginPassword = NewPass;
                    db.SaveChanges();
                    reset();

                    MessageBox.Show("عملیات با موفقیت انجام شد");
                }
                Close();

            }
            catch
            {
                MessageBox.Show("در ارتباط با پایگاه داده مشکلی بوجود آمده است ، لطفا مجددا تلاش کنید");
            }
        }

        private void TxtPasswordold_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtPasswordnew_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }
    }
}
