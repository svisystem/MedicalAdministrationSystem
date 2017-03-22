using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System.ComponentModel;
using System.Windows.Forms;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class ConnectionVM : VMExtender
    {
        public ConnectionM ConnectionM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal ConnectionVM()
        {
            ConnectionM = new ConnectionM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            ConnectionM.HostName = ConfigurationManager.ConfigurationManagerM.Server;
            ConnectionM.PortNumber = ConfigurationManager.ConfigurationManagerM.PortNumber;
            ConnectionM.DatabaseName = ConfigurationManager.ConfigurationManagerM.Database;
            ConnectionM.UserId = ConfigurationManager.ConfigurationManagerM.UserId;
            ConnectionM.Password = ConfigurationManager.ConfigurationManagerM.Password;
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            ConnectionM.AcceptChanges();
            await Utilities.Loading.Hide();
        }
        protected internal void ExecuteMethod()
        {
            dialog = new Dialog(true, "Módosítás megerősítése", OkMethod, () => { }, true);
            dialog.content = new TextBlock("Biztosan megváltoztatja a jelenlegi adatbázis beállításokat?");
            dialog.Start();
        }
        private async void OkMethod()
        {
            await Utilities.Loading.Show();
            Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteCompleted);
            Execute.RunWorkerAsync();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            ConfigurationManager.ConfigurationManagerM.Server = ConnectionM.HostName;
            ConfigurationManager.ConfigurationManagerM.PortNumber = ConnectionM.PortNumber;
            ConfigurationManager.ConfigurationManagerM.Database = ConnectionM.DatabaseName;
            ConfigurationManager.ConfigurationManagerM.UserId = ConnectionM.UserId;
            ConfigurationManager.ConfigurationManagerM.Password = ConnectionM.Password;
            ConfigurationManager.Save();
        }
        private void ExecuteCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dialog = new Dialog(false, "Sikeres adatómódosítás", Reload);
            dialog.content = new TextBlock("Sikeresen módosította az adatkapcsolat beállításait\n" +
                "A változtatások érvénybe lépéséhez most újraindítjuk az alkalmazást");
            dialog.Start();
        }
        private async void Reload()
        {
            await Utilities.Loading.Hide();
            Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
        protected internal bool VMDirty()
        {
            return ConnectionM.IsChanged;
        }
    }
}
