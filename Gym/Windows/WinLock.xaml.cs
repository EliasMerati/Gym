using Gym.Files;
using Microsoft.Win32;
using System;
using System.Management;
using System.Windows;
using System.Windows.Input;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinLock.xaml
    /// </summary>
    public partial class WinLock : Window
    {
        public WinLock()
        {
            InitializeComponent();
        }

        public int g { get; set; }
        public string sum { get; set; }
        public int SL { get; set; }
        public string L1 { get; set; }
        public char c { get; set; }
        public int AI { get; set; }
        public Int64 j { get; set; }
        public Int64 s { get; set; }
        public string k { get; set; }
        public string w { get; set; }
        ServerConfig sc = new ServerConfig();
        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e) => Close();

        public void ahamase()
        {
            try
            {
                ManagementObjectSearcher searcher;
                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject obj in searcher.Get())
                {
                    try
                    {
                        sum = obj.Properties["SerialNumber"].Value.ToString();
                    }
                    catch (Exception m)
                    {
                        MessageBox.Show(m.Message);
                    }
                }


                txtserial.Text = sum;
                //-------------------------------جدا کردن ارقام شماره سریال دریافتی و افزودن به لیست باکس 1
                SL = txtserial.Text.Length;
                for (int i = 0; i < SL; i++)
                {
                    L1 = txtserial.Text.Substring(i, 1);
                    ls.Items.Add(L1);
                }
                //-------------------------------تبدیل به کد اسکی و افزودن به لیست باکس 2
                for (int i = 0; i < ls.Items.Count; i++)
                {
                    L1 = ls.Items[i].ToString();
                    c = char.Parse(L1);
                    AI = Convert.ToInt32(c);
                    la.Items.Add(AI);
                }
                //--------------------------Sum-----جمع کردن مقادیر لیست باکس 2 و نوشتن الگوریتم نهایی برای تولید شماره سریال نهایی
                for (int i = 0; i < la.Items.Count; i++)
                {
                    L1 = la.Items[i].ToString();
                    j = int.Parse(L1);
                    s += j;
                }
                txtserial.Text = (s * 191264).ToString();
            }
            catch
            {
                txtserial.Text = "24351399";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtLock.Text == "")
            {
                MessageBox.Show("لطفا شماره قفل دریافتی را وارد کنید", "توجه", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLock.Focus();
                return;
            }
            Int64 ii;
            Int64.TryParse(txtLock.Text, out ii);
            if (ii == 0)
            {
                MessageBox.Show("مقدار وارد شده نامعتبر است", "ورودی نامعتبر", MessageBoxButton.OK, MessageBoxImage.Error);
                txtLock.Text = string.Empty;
                txtLock.Focus();
                return;
            }
            string u = sc.IAmBook(txtserial.Text);
            if (txtLock.Text == u)
            {
                MessageBox.Show("نرم افزار با موفقیت فعالسازی شد", "تبریک", MessageBoxButton.OK, MessageBoxImage.Information);
                /////////////////////////////////////////////////////////////دستورات رجیستری
                RegistryKey Afra = Registry.CurrentUser.CreateSubKey("software\\Afra");
                string s = BCrypt.Net.BCrypt.HashString(txtLock.Text, 14);
                Afra.SetValue("GymSerial", s);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("عدد وارد شده صحیح نمی باشد", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                txtLock.Text = string.Empty;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtLock.Focus();
            ahamase();
            RegistryKey Afra = Registry.CurrentUser.CreateSubKey("software\\Afra");
            w = (string)Afra.GetValue("GymSerial");
            k = sc.IAmBook(txtserial.Text);
            if (w != null)
            {
                if (BCrypt.Net.BCrypt.Verify(k, w))
                {
                    DialogResult = true;
                }
            }
        }
    }
}
