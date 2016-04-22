using System;
using System.ComponentModel;
using System.Configuration;
using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class SecurityVM : VMExtender
    {
        public SecurityM SecurityM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private PasswordManager PasswordManager { get; set; }
        private Configuration config { get; set; }
        private Action SecurityLoad { get; set; }
        protected internal SecurityVM(Action SecurityLoad)
        {
            this.SecurityLoad = SecurityLoad;
            SecurityM = new SecurityM();
            PasswordManager = new PasswordManager();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            SecurityM.RegSecurityUser = config.AppSettings.Settings["securityUserName"].Value;
            SecurityM.RegSecurityPass = config.AppSettings.Settings["securityPassword"].Value;
            SecurityM.RegSecurityPassSalt = config.AppSettings.Settings["securityPasswordSalt"].Value;
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            SecurityM.AcceptChanges();
            await Utilities.Loading.Hide();
            await Utilities.Loading.Hide();
        }
        protected internal void ExecuteMethod()
        {
            dialog = new Dialog(true, "Módosítás megerősítése", OkMethod, delegate { }, true);
            dialog.content = new TextBlock("Biztosan megváltoztatja a biztonsági profil beállításait?");
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
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (SecurityM.NewSecurityUser.Length != 0)
            {
                config.AppSettings.Settings["securityUserName"].Value = SecurityM.NewSecurityUser;
            }
            if (SecurityM.NewSecurityPass.Length != 0)
            {
                config.AppSettings.Settings["securityPasswordSalt"].Value = PasswordManager.GetSaltString();
                config.AppSettings.Settings["securityPassword"].Value = PasswordManager.GenerateHashWithSalt
                    (SecurityM.NewSecurityPass, config.AppSettings.Settings["securityPasswordSalt"].Value);
            }
            config.Save(ConfigurationSaveMode.Modified);
        }
        private void ExecuteCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dialog = new Dialog(false, "Sikeres adatómódosítás", SecurityLoad);
            dialog.content = new TextBlock("Sikeresen módosított a biztonsági profilt");
            dialog.Start();
        }
        protected internal bool RegSecurityUserCompare(string value)
        {
            return SecurityM.RegSecurityUser.Equals(value);
        }
        protected internal bool PasswordMatch(string pass)
        {
            return PasswordManager.IsPasswordMatch(pass, SecurityM.RegSecurityPassSalt, SecurityM.RegSecurityPass);
        }
        protected internal bool VMDirty()
        {
            return SecurityM.IsChanged;
        }
    }
}
