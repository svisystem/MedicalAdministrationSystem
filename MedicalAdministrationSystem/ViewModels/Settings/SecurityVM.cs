using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.ComponentModel;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class SecurityVM : VMExtender
    {
        public SecurityM SecurityM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private PasswordManager PasswordManager { get; set; }
        private Action SecurityLoad { get; set; }
        protected internal SecurityVM(Action SecurityLoad)
        {
            this.SecurityLoad = SecurityLoad;
            SecurityM = new SecurityM();
            PasswordManager = new PasswordManager();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            SecurityM.RegSecurityUser = ConfigurationManager.ConfigurationManagerM.SecurityUsername;
            SecurityM.RegSecurityPass = ConfigurationManager.ConfigurationManagerM.SecurityPassword;
            SecurityM.RegSecurityPassSalt = ConfigurationManager.ConfigurationManagerM.SecurityPasswordSalt;
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            SecurityM.AcceptChanges();
            await Utilities.Loading.Hide();
            await Utilities.Loading.Hide();
        }
        protected internal void ExecuteMethod()
        {
            dialog = new Dialog(true, "Módosítás megerősítése", OkMethod, () => { }, true);
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
            if (!string.IsNullOrEmpty(SecurityM.NewSecurityUser))
                ConfigurationManager.ConfigurationManagerM.SecurityUsername = SecurityM.NewSecurityUser;
            if (!string.IsNullOrEmpty(SecurityM.NewSecurityPass))
            {
                ConfigurationManager.ConfigurationManagerM.SecurityPasswordSalt = PasswordManager.GetSaltString();
                ConfigurationManager.ConfigurationManagerM.SecurityPassword = PasswordManager.GenerateHashWithSalt
                    (SecurityM.NewSecurityPass, ConfigurationManager.ConfigurationManagerM.SecurityPasswordSalt);
            }
            ConfigurationManager.Save();
        }
        private void ExecuteCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dialog = new Dialog(false, "Sikeres adatómódosítás", SecurityLoad);
            dialog.content = new TextBlock("Sikeresen módosította a biztonsági profilt");
            dialog.Start();
        }
        protected internal bool RegSecurityUserCompare(string value) => SecurityM.RegSecurityUser.Equals(value);
        protected internal bool PasswordMatch(string pass) =>
            PasswordManager.IsPasswordMatch(pass, SecurityM.RegSecurityPassSalt, SecurityM.RegSecurityPass);
        protected internal bool VMDirty() => SecurityM.IsChanged;
    }
}
