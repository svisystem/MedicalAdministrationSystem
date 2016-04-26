using DevExpress.Xpf.Core;
using MedicalAdministrationSystem.ViewModels;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            GlobalVM.StartUp();
            Task.Factory.StartNew(() =>
            Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    ApplicationThemeHelper.UpdateApplicationThemeName()))
            , CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            CultureInfo culture = CultureInfo.CreateSpecificCulture("hu-HU");

            //// The following line provides localization for the application's user interface. 
            //Thread.CurrentThread.CurrentUICulture = culture;

            //// The following line provides localization for data formats. 
            //Thread.CurrentThread.CurrentCulture = culture;

            // Set this culture as the default culture for all threads in this application. 
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
