using System;
using System.Windows;
using System.Windows.Threading;
using Gym.Windows;

namespace Gym
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void startup_Login(object sender, StartupEventArgs e)
        {
            new WinLogin().ShowDialog();
            new MainWindow().ShowDialog();
        }
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
        //    DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        //    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        //}

        //void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        //{
        //    e.Handled = true;
        //}

        //void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //    var isTerminating = e.IsTerminating;
        //}
    }
}
