using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;

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
            if (counter.Equals(1) && !showed)
            {
                metroWindow.Focusable = false;
                loading.Loaded += Loaded;
                await metroWindow.ShowMetroDialogAsync(loading);
            }
        }
        private static async void Loaded(object sender, RoutedEventArgs e)
        {
            if (counter.Equals(0))
            {
                metroWindow.Focusable = true;
                loading.Unloaded += Unloaded;
                await metroWindow.HideMetroDialogAsync(loading);
            }
            else showed = true;
            loading.Loaded -= Loaded;
        }
        static async internal Task Hide()
        {
            if (counter > 0) counter--;
            if (counter.Equals(0) && showed)
            {
                metroWindow.Focusable = true;
                loading.Unloaded += Unloaded;
                await metroWindow.HideMetroDialogAsync(loading);
            }
        }
        private static async void Unloaded(object sender, RoutedEventArgs e)
        {
            if (counter != 0)
            {
                metroWindow.Focusable = false;
                loading.Loaded += Loaded;
                await metroWindow.ShowMetroDialogAsync(loading);
            }
            else showed = false;
            loading.Unloaded -= Unloaded;
        }
    }
}
