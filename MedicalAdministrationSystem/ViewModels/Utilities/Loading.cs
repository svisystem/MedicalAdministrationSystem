using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    static class Loading
    {
        private static MetroWindow metroWindow = GlobalVM.MainWindow;
        private static Views.Global.Loading loading = new Views.Global.Loading();
        private static int counter = 0;
        private static bool showed = false;
        static async internal Task Show()
        {
            counter++;
            if (counter.Equals(1))
            {
                GlobalVM.MainWindow.Focusable = false;
                loading.Loaded += Loaded;
                await metroWindow.ShowMetroDialogAsync(loading);
            }
        }
        static async internal Task Hide()
        {
            if (counter > 0) counter--;
            if (counter.Equals(0) && showed)
            {
                GlobalVM.MainWindow.Focusable = true;
                await metroWindow.HideMetroDialogAsync(loading);
                showed = false;
            }
        }
        private static async void Loaded(object sender, RoutedEventArgs e)
        {
            showed = true;
            if (counter.Equals(0))
            {
                GlobalVM.MainWindow.Focusable = true;
                await metroWindow.HideMetroDialogAsync(loading);
                showed = false;
            }
            loading.Loaded -= Loaded;
        }
    }
}
