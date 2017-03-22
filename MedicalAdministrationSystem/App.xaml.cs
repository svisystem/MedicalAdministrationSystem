using DevExpress.Xpf.Core;
using MedicalAdministrationSystem.ViewModels;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = Theme.MetropolisLightName;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            GlobalVM.StartUp();

            CultureInfo culture = CultureInfo.CreateSpecificCulture("hu-HU");

            //// The following line provides localization for the application's user interface. 
            //Thread.CurrentThread.CurrentUICulture = culture;

            //// The following line provides localization for data formats. 
            //Thread.CurrentThread.CurrentCulture = culture;

            // Set this culture as the default culture for all threads in this application. 
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) =>
            Log.WriteException(e.Exception);
    }
}
