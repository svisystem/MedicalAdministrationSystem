using MedicalAdministrationSystem.Views.Global;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public abstract class VMExtender
    {
        protected internal StockVerticalMenuItem currentItem { get; set; }
        protected internal StockVerticalMenuItem earlierItem { get; set; }
        protected internal Dialog dialog { get; set; }
        protected internal void ConnectionMessage()
        {
            dialog = new Dialog(true, "Nem sikerült elérni az adatbázist", Application.Current.Shutdown);
            dialog.content = new Views.Dialogs.TextBlock("Adatbáziskapcsolat nélkül nem lehet megfelelően használni az alkalmazást\n" +
                "Kérjük jelezze a problémát a rendszergazdának\nAz alkalmazás most bezárul");
            dialog.Start();
        }
        protected internal async void ViewLoad(Func<UserControl> method, StockVerticalMenuItem button)
        {
            await Task.Factory.StartNew(() =>
            {
                return method();

            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(task =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate { GlobalVM.StockLayout.actualContent.Content = task.Result; }));
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            });
            button.button_Click(button.button, new RoutedEventArgs(Button.ClickEvent));
            currentItem = button;
            Loading.Hide();
        }
    }
}
