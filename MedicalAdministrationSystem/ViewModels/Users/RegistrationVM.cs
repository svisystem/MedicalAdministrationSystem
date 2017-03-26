using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Users
{
    public class RegistrationVM : VMExtender
    {
        protected internal RegistrationM RegistrationM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal RegistrationVM()
        {
            RegistrationM = new RegistrationM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteCompleted);
            Execute.RunWorkerAsync();
        }
        protected internal bool UserCheck(string user) => RegistrationM.ExistUsers.Any(l => l == user);
        private async void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    RegistrationM.ExistUsers = me.accountdata.Where(a => a.DeletedAD != true).Select(a => a.UserNameAD).ToList();
                    RegistrationM.PriviledgesList = me.priviledges.Select(a => a.NameP).ToList();
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            await Utilities.Loading.Hide();
            if (workingConn) RegistrationM.AcceptChanges();
            else ConnectionMessage();
        }
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    RegistrationM.PriviledgesID = me.priviledges.Where(b => b.NameP == RegistrationM.PriviledgesSelected).Select(b => b.IdP).Single();
                    accountdata ad = new accountdata();
                    ad.UserNameAD = RegistrationM.Username;
                    PasswordManager pwm = new PasswordManager();
                    RegistrationM.PassSalt = pwm.GetSaltString();
                    ad.PassSaltAD = RegistrationM.PassSalt;
                    ad.PasswordAD = pwm.GenerateHashWithSalt(RegistrationM.Password, RegistrationM.PassSalt);
                    ad.PriviledgesIdAD = RegistrationM.PriviledgesID;
                    ad.RegistrateTimeAD = DateTime.Now;
                    me.accountdata.Add(ad);
                    await me.SaveChangesAsync();
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void ExecuteCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                await Utilities.Loading.Hide();
                dialog = new Dialog(false, "Sikeres regisztráció", OkMethod, CancelMethod, true);
                dialog.content = new TextBlock("Sikeres regisztrációt követően a hozzásférését a rendszergazdának jóvá kell hagynia\n" +
                    "Szeretne átváltani a Bejelentkezés képernyőre?");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        private async void OkMethod()
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            await mbe.LoadItem(GlobalVM.StockLayout.usersTBI);
        }
        private async void CancelMethod()
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.modifier = false;
            await mbe.LoadItem(GlobalVM.StockLayout.usersTBI);
        }
        protected internal bool PriviledgeCheck(string selected)
        {
            return RegistrationM.PriviledgesList.Any(l => l == selected);
        }
        protected internal bool VMDirty()
        {
            return RegistrationM.IsChanged;
        }
    }
}