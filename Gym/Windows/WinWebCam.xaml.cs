using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Win32;


namespace Gym.Windows
{
    /// <summary>
    /// Interaction logic for WinWebCam.xaml
    /// </summary>
    public partial class WinWebCam : Window
    {
        public WinWebCam()
        {
            InitializeComponent();
        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice camera;
        public string strname { get; set; }
        public string imagename { get; set; }
        public int MyProperty { get; set; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in webcam)
            {
                CmbWebcam.Items.Add(device.Name);
            }
            CmbWebcam.SelectedIndex = 0;
            // start webcam
            try
            {
                camera = new VideoCaptureDevice(webcam[CmbWebcam.SelectedIndex].MonikerString);
                camera.NewFrame += new NewFrameEventHandler(cam_frame);
                camera.Start();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "ErroR");
            }
            /////////////////////////////////////////////////////////////////
        }

        void cam_frame(object sender, NewFrameEventArgs eventArgs)
        {
            System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();

            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            bi.Freeze();
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                ImgCapture.Source = bi;
            }));
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        
        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => Close();

        private void BtnSnapshot_Click(object sender, RoutedEventArgs e)
        {
            //System.Drawing.Image img = (Bitmap)ImgCapture.Source.Clone();

            //MemoryStream ms = new MemoryStream();
            //img.Save(ms, ImageFormat.Bmp);
            //ms.Seek(0, SeekOrigin.Begin);
            //BitmapImage bi = new BitmapImage();
            //bi.BeginInit();
            //bi.StreamSource = ms;
            //bi.EndInit();

            //bi.Freeze();
            //Dispatcher.BeginInvoke(new ThreadStart(delegate
            //{
            //    ImgCapture.Source = bi;
            //}));
        }
    }
}
