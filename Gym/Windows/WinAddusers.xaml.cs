using System;
using System.IO;
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
    /// Interaction logic for Winusers.xaml
    /// </summary>
    public partial class Winusers : Window
    {
        public Winusers()
        {
            InitializeComponent();
        }

        Gym_DBEntities db = new Gym_DBEntities();

        public int peopleID = 0;
        public string strname { get; set; }
        public string imagename { get; set; }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Cmbtype.SelectedIndex = 0;
            Cmbtype2.SelectedIndex = 0;
            if (peopleID != 0)
            {
                var people = db.People.Find(peopleID);
                TxtAddress.Text = people.PeopleAddress;
                Txtcreditor.Text = people.PeopleCreditor.ToString();
                Txtdeptor.Text = people.PeopleDeptor.ToString();
                TxtMellicode.Text = people.PeopleMellicode;
                TxtName.Text = people.PeopleName;
                TxtTel.Text = people.PeopleMobile;
                Cmbtype.Text = people.PeopleType;
                Txtdeptor.IsEnabled = false;
                Txtcreditor.IsEnabled = false;
                ShowImage();
            }
        }
        public void resetimage()
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"\Resources\avatar.png", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            imgperson.Source = bi;
        }

        private void imgperson_MouseDown(object sender, MouseButtonEventArgs e)
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
                            imgperson.SetValue(Image.SourceProperty, isc.ConvertFromString(imagename));
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

        private void ShowImage()
        {
            /////////////////////////////////////////////////  ایجاد تصویر از دیتابیس در هنگام ویرایش
            var query = db.People.Find(peopleID);
            if (query.PeopleLogo != null)
            {
                byte[] imagearray = query.PeopleLogo;
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
                imgperson.Source = bi;

            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void TxtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[0-9]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtTel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[a-zA-Zآ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtMellicode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[a-zA-Zآ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtDeptor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[a-zA-Zآ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtCreditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[a-zA-Zآ-ی]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (peopleID == 0)
                {
                    if (imagename != null)
                    {
                        FileStream fs = new FileStream(imagename, FileMode.Open, FileAccess.Read);
                        byte[] imgbytearr = new byte[fs.Length];
                        fs.Read(imgbytearr, 0, Convert.ToInt32(fs.Length));
                        fs.Close();
                        //////////////////////////////////////////////////////////////////////////////
                        db.InsertPeople(TxtName.Text.Trim(), TxtTel.Text.Trim(), TxtAddress.Text.Trim(),
                            TxtMellicode.Text.Trim(),
                            Cmbtype.Text, Cmbtype2.Text, int.Parse(Txtdeptor.Text.Trim()), int.Parse(Txtcreditor.Text.Trim()),
                            imgbytearr);

                    }
                    else if (imagename == null)
                    {
                        db.InsertPeople2(TxtName.Text.Trim(), TxtTel.Text.Trim(), TxtAddress.Text.Trim(),
                            TxtMellicode.Text.Trim(),
                            Cmbtype.Text, Cmbtype2.Text, int.Parse(Txtdeptor.Text.Trim()), int.Parse(Txtcreditor.Text.Trim())
                            );
                    }

                    db.SaveChanges();
                    MessageBox.Show("ثبت اطلاعات انجام شد");
                    TxtName.Focus();
                }
                else if (peopleID != 0)
                {
                    if (imagename != null)
                    {
                        FileStream fs = new FileStream(imagename, FileMode.Open, FileAccess.Read);
                        byte[] imgbytearr = new byte[fs.Length];
                        fs.Read(imgbytearr, 0, Convert.ToInt32(fs.Length));
                        fs.Close();
                        ///////////////////////////////////////////////////////////////////////////////
                        db.UpdatePeople1(peopleID, TxtName.Text.Trim(), TxtTel.Text.Trim(), TxtAddress.Text.Trim(),
                            TxtMellicode.Text.Trim(),
                            Cmbtype.Text, Cmbtype2.Text,
                            imgbytearr);
                    }
                    else if (imagename == null)
                    {
                        db.UpdatePeople2(peopleID, TxtName.Text.Trim(), TxtTel.Text.Trim(), TxtAddress.Text.Trim(),
                            TxtMellicode.Text.Trim(),
                            Cmbtype.Text, Cmbtype2.Text
                            );
                    }
                    db.SaveChanges();
                    MessageBox.Show("ویرایش اطلاعات انجام شد");
                    
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است، لطفا مجددا تلاش نمایید");
            }
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}