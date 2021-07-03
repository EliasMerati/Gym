using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataLayer;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinAddPeriod.xaml
    /// </summary>
    public partial class WinAddPeriod : Window
    {
        public WinAddPeriod()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();
        public int id { get; set; }
        public int peopleid { get; set; }
        public string name { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblname.Text = name;
            TxtproductName.Focus();
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void showItemsInDatagrid() => DgvPeriodItems.ItemsSource = db.PeriodItems.Where(k => k.PeriodID == id).ToList();
        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var query = db.Database.SqlQuery<Period>("select top 1* from Period order by PeriodID desc").ToList();
                id = query[0].PeriodID;
                try
                {
                    db.InsertPeriodItem(id,TxtproductName.Text.Trim(),Txtcount.Text.Trim(),int.Parse(TxtFee.Text.Trim()));
                    db.SaveChanges();
                    showItemsInDatagrid();
                    if (DgvPeriodItems.Items != null)
                    {
                        BtnCloseProgram.IsEnabled = true;
                    }
                    else
                    {
                        BtnCloseProgram.IsEnabled = false;
                    }

                    clear();
                    TxtproductName.Focus();
                    ts.Complete();
                }
                catch (Exception n)
                {
                    MessageBox.Show(n.Message);
                }
            }
        }

        void clear()
        {
            Txtcount.Text = string.Empty;
            TxtFee.Text = string.Empty;
            TxtproductName.Text = string.Empty;
        }

        private void BtnCloseProgram_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("آیا از ثبت نهایی دوره اطمینان دارید؟", "توجه", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    db.ClosePeriod(id, TxtDescription.Text.Trim());
                    db.SaveChanges();
                    MessageBox.Show("دوره با موفقیت در سیستم ثبت شد");
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است، لطفا دوباره امتحان کنید");
            }
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            object item = DgvPeriodItems.SelectedItem;
            try
            {
                if (item != null)
                {
                    if (MessageBox.Show("آیا از حذف این قسمت اطمینان دارید؟", "توجه", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        int itemid = int.Parse((DgvPeriodItems.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                        var b = db.PeriodItems.Where(m => m.PeriodID == id && m.PeriodItemID == itemid).SingleOrDefault();
                        db.PeriodItems.Remove(b);
                        db.SaveChanges();
                        showItemsInDatagrid();

                    }
                }
                else
                {
                    MessageBox.Show(" لطفا برای حذف موردی را انتخاب کنید");
                }

            }
            catch
            {
                MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است، لطفا دوباره امتحان کنید");
            }
        }

        private void TxtFee_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[آ-ی a-z A-Z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void TxtproductName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[0-9]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtcount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[A-Z a-z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void BtnPay_Click(object sender, RoutedEventArgs e) => new WinAddPeyment().ShowDialog();
    }
}
