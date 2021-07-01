using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataLayer;
using Microsoft.Win32;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinAddGym.xaml
    /// </summary>
    public partial class WinAddGym : Window
    {
        public WinAddGym()
        {
            InitializeComponent();
        }

        private Gym_DBEntities db = new Gym_DBEntities();
        private string strname, imagename;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var b = db.Gym.ToList();
            var g = db.Gym.Any();
            if (g)
            {
                try
                {
                    TxtName.Text = b[0].GymName;
                    TxtAddress.Text = b[0].GymAddress;
                    TxtInsta.Text = b[0].GymInstagram;
                    TxtPasswordsms.Password = b[0].GymsmsPass;
                    Txtmanager.Text = b[0].GymManager;
                    Txtsender.Text = b[0].Gymsmssender;
                    Txttel.Text = b[0].GymTel;
                    Txttelegram.Text = b[0].GymTelegram;
                    Txtusernamesms.Text = b[0].Gymsmsusername;
                    Txtwebsite.Text = b[0].GymWebsite;
                    Txtweekend.Text = b[0].GymWeekend;
                    Txtwhatsup.Text = b[0].GymWhatsup;
                    Txtwork.Text = b[0].GymWork;
                    if (b[0].GymLogo != null)
                    {
                        ShowImage();
                    }
                }
                catch
                {
                    MessageBox.Show("در ویرایش اطلاعات مشکلی بوجود آمده است، لطفا مجددا تلاش نمایید");
                }
            }

        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void ShowImage()
        {
            /////////////////////////////////////////////////  ایجاد تصویر از دیتابیس در هنگام ویرایش
            var q = db.Gym.ToList();
            if (q[0].GymLogo != null)
            {
                byte[] imagearray = q[0].GymLogo;
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
                imglogo.Source = bi;

            }
        }
        private void imglogo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ///////////////////گرفتن عکس از هارد کامپیوتر و آماده سازی برای ذخیره
                FileDialog fldlg = new OpenFileDialog();
                fldlg.Filter = " Image File (*.jpg;*.bmp;*.gif;*.jpeg)|*.jpg;*.bmp;*.gif;*.jpeg";
                fldlg.ShowDialog();
                if (new FileInfo(fldlg.FileName).Length <= 100000) ///////////// محدودیت حجم عکس انتخابی
                {
                    {
                        strname = fldlg.SafeFileName;
                        imagename = fldlg.FileName;
                        if (imagename != null)
                        {
                            ImageSourceConverter isc = new ImageSourceConverter();
                            imglogo.SetValue(Image.SourceProperty, isc.ConvertFromString(imagename));
                        }

                    }
                }
                else
                {
                    MessageBox.Show("حجم عکس شما از 100 کیلوبایت بیشتر است");
                }

                fldlg = null;
            }
            catch
            {
                MessageBox.Show("در ثبت تصویر مشکلی به وجود آمده");
            }
        }

        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            using (Gym_DBEntities db = new Gym_DBEntities())
            {
                var n = db.Gym.Any();
                try
                {
                    if (n)
                    {
                        if (imagename != null)
                        {
                            FileStream fs = new FileStream(imagename, FileMode.Open, FileAccess.Read);
                            byte[] imgbytearr = new byte[fs.Length];
                            fs.Read(imgbytearr, 0, Convert.ToInt32(fs.Length));
                            fs.Close();
                            //////////////////////////////////////////////////////////////////////////////
                            db.UpdateGym1(TxtName.Text.Trim(), Txtmanager.Text.Trim(), TxtAddress.Text.Trim(),
                                Txttel.Text.Trim(),
                                Txtwebsite.Text.Trim(), TxtInsta.Text.Trim(), Txttelegram.Text.Trim(),
                                Txtwhatsup.Text.Trim(),
                                Txtwork.Text.Trim(), Txtusernamesms.Text.Trim(), TxtPasswordsms.Password.Trim(),
                                Txtsender.Text.Trim(),
                                Txtweekend.Text.Trim(), imgbytearr);
                            db.SaveChanges();
                            MessageBox.Show("عملیات ویرایش با موفقیت انجام شد");
                        }
                        else if (imagename == null)
                        {
                            db.UpdateGym2(TxtName.Text.Trim(), Txtmanager.Text.Trim(), TxtAddress.Text.Trim(),
                                Txttel.Text.Trim(),
                                Txtwebsite.Text.Trim(), TxtInsta.Text.Trim(), Txttelegram.Text.Trim(),
                                Txtwhatsup.Text.Trim(),
                                Txtwork.Text.Trim(), Txtusernamesms.Text.Trim(), TxtPasswordsms.Password.Trim(),
                                Txtsender.Text.Trim(),
                                Txtweekend.Text.Trim());
                            db.SaveChanges();
                            MessageBox.Show("عملیات ویرایش با موفقیت انجام شد");
                        }
                    }
                    else if (n == false)
                    {
                        if (imagename != null)
                        {
                            FileStream fs = new FileStream(imagename, FileMode.Open, FileAccess.Read);
                            byte[] imgbytearr = new byte[fs.Length];
                            fs.Read(imgbytearr, 0, Convert.ToInt32(fs.Length));
                            fs.Close();
                            //////////////////////////////////////////////////////////////////////////////
                            db.InsertGym1(TxtName.Text.Trim(), Txtmanager.Text.Trim(), TxtAddress.Text.Trim(),
                                Txttel.Text.Trim(),
                                Txtwebsite.Text.Trim(), TxtInsta.Text.Trim(), Txttelegram.Text.Trim(),
                                Txtwhatsup.Text.Trim(),
                                Txtwork.Text.Trim(), Txtusernamesms.Text.Trim(), TxtPasswordsms.Password.Trim(),
                                Txtsender.Text.Trim(),
                                Txtweekend.Text.Trim(), imgbytearr);
                            db.SaveChanges();
                            MessageBox.Show("عملیات ثبت با موفقیت انجام شد");
                        }
                        else if (imagename == null)
                        {
                            db.InsertGym1(TxtName.Text.Trim(), Txtmanager.Text.Trim(), TxtAddress.Text.Trim(),
                                Txttel.Text.Trim(),
                                Txtwebsite.Text.Trim(), TxtInsta.Text.Trim(), Txttelegram.Text.Trim(),
                                Txtwhatsup.Text.Trim(),
                                Txtwork.Text.Trim(), Txtusernamesms.Text.Trim(), TxtPasswordsms.Password.Trim(),
                                Txtsender.Text.Trim(),
                                Txtweekend.Text.Trim(),null);
                            db.SaveChanges();
                            MessageBox.Show("عملیات ثبت با موفقیت انجام شد");
                        }
                    }
                }
                catch(Exception d)
                {
                    MessageBox.Show(d.Message);
                }

            }
        }
        private void TxtName_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[0-9]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtmanager_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[0-9]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txttel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtwork_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtweekend_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtwebsite_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtusernamesms_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtPasswordsms_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtsender_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtreciew_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }
    }
}
