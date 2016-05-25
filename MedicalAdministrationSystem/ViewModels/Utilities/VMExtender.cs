using MedicalAdministrationSystem.DataAccess;
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
        protected internal medicalEntities me { get; set; }
        protected internal Dialog dialog { get; set; }
        protected internal bool workingConn { get; set; }
        protected internal void ConnectionMessage()
        {
            dialog = new Dialog(true, "Nem sikerült elérni az adatbázist", Application.Current.Shutdown);
            dialog.content = new Views.Dialogs.TextBlock("Adatbáziskapcsolat nélkül nem lehet megfelelően használni az alkalmazást\n" +
                "Kérjük jelezze a problémát a rendszergazdának\nAz alkalmazás most bezárul");
            dialog.Start();
        }
        protected internal async Task ViewLoad(Func<UserControl> method, StockVerticalMenuItem button)
        {
            await Task.Factory.StartNew(method, 
            CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(task =>
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                {
                    GlobalVM.StockLayout.actualContent.Content = task.Result;
                    button.button_Click(button.button, new RoutedEventArgs(Button.ClickEvent));
                    await Loading.Hide();
                }));
            });
            currentItem = button;
        }

        protected internal async void Check(StockVerticalMenuItem select, Action OK, Action No)
        {
            earlierItem = currentItem;
            if (!currentItem.Equals(select))
            {
                await Loading.Show();
                currentItem = select;
                new FormChecking(OK, No, true);
            }
        }
        protected internal void Back()
        {
            currentItem = earlierItem;
            currentItem.button_Click(currentItem.button, new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
