using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataLayer;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinAddNewProgram.xaml
    /// </summary>
    public partial class WinAddNewProgram : Window
    {
        public WinAddNewProgram()
        {
            InitializeComponent();
        }
        Gym_DBEntities db = new Gym_DBEntities();
        public int id { get; set; }
        public int peopleid { get; set; }
        public string name { get; set; }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e) => Close();

        private void showItemsInDatagrid() => DgvProgramItems.ItemsSource = db.NewProgramItem.Where(k => k.NewProgramID == id).ToList();
        private void Btninsert_Click(object sender, RoutedEventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var query = db.Database.SqlQuery<NewProgram>("select top 1* from NewProgram order by NewProgramID desc").ToList();
                id = query[0].NewProgramID;
                try
                {
                    db.insertProgram(id, Txtitem.Text.Trim(), Txtcount.Text.Trim());
                    db.SaveChanges();
                    showItemsInDatagrid();
                    if (DgvProgramItems.Items != null)
                    {
                        BtnCloseProgram.IsEnabled = true;
                    }
                    else
                    {
                        BtnCloseProgram.IsEnabled = false;
                    }
                    Txtcount.Text = string.Empty;
                    Txtitem.Text = string.Empty;
                    Txtitem.Focus();
                    ts.Complete();
                }
                catch (Exception n)
                {
                    MessageBox.Show(n.Message);
                }
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnCloseProgram.IsEnabled = false;
            lblname.Text = name;
            Txtitem.Focus();
        }

        private void Txtitem_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[A-Z a-z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void Txtcount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[A-Z a-z]");
            e.Handled = Regex.IsMatch(e.Text);
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            object item = DgvProgramItems.SelectedItem;
            try
            {
                if (item != null)
                {
                    if (MessageBox.Show("آیا از حذف این قسمت اطمینان دارید؟", "توجه", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {

                        int itemid = int.Parse((DgvProgramItems.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                        var b = db.NewProgramItem.Where(m => m.NewProgramID == id && m.NewProgramItemID == itemid).SingleOrDefault();
                        db.NewProgramItem.Remove(b);
                        db.SaveChanges();
                        showItemsInDatagrid();

                    }
                }
                else
                {
                    MessageBox.Show("لطفا برای حذف موردی را انتخاب کنید");
                }


            }
            catch
            {
                MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است، لطفا دوباره امتحان کنید");
            }
        }

        private void BtnCloseProgram_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("آیا از ثبت نهایی برنامه اطمینان دارید؟", "توجه", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    db.closetheprogram(id, TxtDescription.Text.Trim());
                    db.SaveChanges();
                    MessageBox.Show("برنامه با موفقیت در سیستم ثبت شد");       
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("در ثبت اطلاعات مشکلی بوجود آمده است، لطفا دوباره امتحان کنید");
            }
        }

        private void TxtDescription_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Regex = new Regex("[A-Z a-z]");
            e.Handled = Regex.IsMatch(e.Text);
        }
    }
}
