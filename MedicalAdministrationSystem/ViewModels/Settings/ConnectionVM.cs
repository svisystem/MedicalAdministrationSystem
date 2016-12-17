﻿using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class ConnectionVM : VMExtender
    {
        public ConnectionM ConnectionM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private Configuration config { get; set; }
        protected internal ConnectionVM()
        {
            ConnectionM = new ConnectionM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            string temp = config.ConnectionStrings.ConnectionStrings["MedicalDb"].ConnectionString;
            string[] splitted = temp.Split(new char[] { ';', '=' });
            ConnectionM.HostName = splitted[3];
            ConnectionM.PortNumber = splitted[5];
            ConnectionM.DatabaseName = splitted[11];
            ConnectionM.UserId = splitted[7];
            ConnectionM.Password = splitted[9];
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
            config.ConnectionStrings.ConnectionStrings["MedicalDb"].ConnectionString =
                "\"persistsecurityinfo=True;server=" + ConnectionM.HostName + ";port=" + ConnectionM.PortNumber + ";user id=" + ConnectionM.UserId + 
                ";password=" + ConnectionM.Password + ";database=" + ConnectionM.DatabaseName + "\"";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
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
