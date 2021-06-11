using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataLayer;
using Microsoft.Win32;

namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinAddUtility.xaml
    /// </summary>
    public partial class WinAddUtility : Window
    {
        public WinAddUtility()
        {
            InitializeComponent();

        }
        Gym_DBEntities db = new Gym_DBEntities();
        public int ID { get; set; }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void cleartextbox()
        {
            TxtFilename.Text = string.Empty;
            TxtTitle.Text = string.Empty;
            TxtFilepath.Text = string.Empty;
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            BtnSaveProgram.IsEnabled = false;
        }
        public void LoadData()
        {
            DgvProgram.ItemsSource = db.Vw_Program.Where(n => n.PeopleID == ID).ToList();
        }

        private void BtnSaveProgram_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var s = db.Program.Any(b => b.ProgramName == TxtFilename.Text);
                if (s)
                {
                    var m = db.Program.First(b => b.ProgramName == TxtFilename.Text);
                    if (TxtFilename.Text == m.ProgramName)
                    {
                        byte[] binarydata;
                        binarydata = m.ProgramBinary.ToArray();
                        int LenghtFile = m.ProgramBinary.Length;


                        FileInfo f_info = new FileInfo(TxtFilepath.Text);
                        int LenghtLocal = (int)f_info.Length;
                        if (LenghtFile > LenghtLocal || LenghtFile < LenghtLocal)
                        {
                            File.WriteAllBytes(TxtFilepath.Text, binarydata);
                            MessageBox.Show("فرایند ویرایش با موفقیت انجام شد");
                            LoadData();
                            cleartextbox();
                            return;
                        }
                        else if (LenghtFile == LenghtLocal)
                        {
                            MessageBox.Show("این فایل از قبل ذخیره شده است");
                            cleartextbox();
                            return;
                        }
                    }
                }
                else
                {

                    byte[] binarydata;
                    binarydata = File.ReadAllBytes(TxtFilepath.Text);
                    try
                    {
                        Program p = new Program()
                        {
                            PeopleID = ID,
                            ProgramBinary = binarydata,
                            ProgramName = TxtFilename.Text.Trim(),
                            ProgramTitle = TxtTitle.Text.Trim()
                        };
                        db.Program.Add(p);
                        db.SaveChanges();
                        MessageBox.Show("فایل با موفقیت ذخیره شد", "اطلاعیه", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                        LoadData();
                        cleartextbox();
                    }
                    catch
                    {
                        MessageBox.Show("در فرایند ذخیره ی فایل مشکلی بوجود آمده است", "مشکل در ذخیره", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception o)
            {
                MessageBox.Show(o.Message);
            }

        }


        private void DgvProgram_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                object item = DgvProgram.SelectedItem;


                if (item != null)
                {
                    int _id = int.Parse(((TextBlock)DgvProgram.SelectedCells[0].Column.GetCellContent(item)).Text);
                    var selected = db.Program.First(b => b.ProgramID == _id);
                    if (selected != null)
                    {
                        TxtFilename.Text = selected.ProgramName;
                        TxtTitle.Text = selected.ProgramTitle;
                    }
                }
            }
            catch (Exception n)
            {
                MessageBox.Show(n.Message);
            }
        }

        private void Delete_click(object sender, RoutedEventArgs e)
        {
            try
            {
                object item = DgvProgram.SelectedItem;
                if (item != null)
                {
                    int _id = int.Parse(((TextBlock)DgvProgram.SelectedCells[0].Column.GetCellContent(item)).Text);
                    string name = ((TextBlock)DgvProgram.SelectedCells[2].Column.GetCellContent(item)).Text;
                    var selected = db.Program.First(b => b.ProgramID == _id);
                    if (true)
                    {
                        if (MessageBox.Show($"آیا از حذف {name} مطمئن هستید؟", "توجه", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            db.Program.Remove(selected);
                            db.SaveChanges();
                            cleartextbox();
                            LoadData();
                        }
                    }
                }

            }
            catch
            {
                MessageBox.Show("در حذف اطلاعات مشکلی بوجود آمد");
            }
        }

        private void SaveinComputer_click(object sender, RoutedEventArgs e)
        {
            object item = DgvProgram.SelectedItem;
            if (item != null)
            {
                int _id = int.Parse(((TextBlock)DgvProgram.SelectedCells[0].Column.GetCellContent(item)).Text);
                var selected = db.Program.First(b => b.ProgramID == _id);
                byte[] Byte = selected.ProgramBinary.ToArray();

                SaveFileDialog savefd = new SaveFileDialog();
                savefd.FileName = TxtFilename.Text;

                Nullable<bool> result = savefd.ShowDialog();
                if (result == true)
                {
                    File.WriteAllBytes(savefd.FileName, Byte);
                    MessageBox.Show("فرایند ذخیره سازی با موفقیت انجام شد");
                    cleartextbox();
                    System.Diagnostics.Process.Start(savefd.FileName);
                }
            }
        }

        private void BtnSelectProgram_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == true)
            {
                TxtFilepath.Text = op.FileName;
                BtnSaveProgram.IsEnabled = true;
                //---------------------------
                TxtFilename.Text = op.SafeFileName;
            }
        }

    }
}
