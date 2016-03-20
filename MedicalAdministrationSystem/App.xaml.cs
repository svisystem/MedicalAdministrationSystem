using DevExpress.Xpf.Core;
using MedicalAdministrationSystem.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem
{
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            GlobalVM.StartUp();
            await Task.Factory.StartNew(() =>
            Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate
                {
                    ApplicationThemeHelper.UpdateApplicationThemeName();
                }))
            , CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
