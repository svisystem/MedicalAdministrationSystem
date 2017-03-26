using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.MenuItem;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Users
{
    public class PassChangeVM : VMExtender
    {
        protected internal PassChangeM PassChangeM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private PasswordManager PasswordManager { get; set; }
        protected internal PassChangeVM()
        {
            PassChangeM = new PassChangeM();
            PasswordManager = new PasswordManager();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        protected internal void ExecuteMethod()
        {
            dialog = new Dialog(false, "Módosítás megerősítése", OkMethod, CanelMethod, true);
            dialog.content = new TextBlock("Biztosan megváltoztatja jelenlegi jelszavát?");
            dialog.Start();
        }
        private void CanelMethod() { }

        private async void OkMethod()
        {
            await Utilities.Loading.Show();
            Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteCompleted);
            Execute.RunWorkerAsync();
        }
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    accountdata ad = new accountdata();
                    ad = me.accountdata.Where(b => b.IdAD == GlobalVM.GlobalM.AccountID).Single();
                    PasswordManager pwm = new PasswordManager();
                    PassChangeM.NewPassSalt = pwm.GetSaltString();
                    ad.PassSaltAD = PassChangeM.NewPassSalt;
                    ad.PasswordAD = pwm.GenerateHashWithSalt(PassChangeM.NewPassword, PassChangeM.NewPassSalt);
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
            await Utilities.Loading.Hide();
            if (workingConn)
            {
                dialog = new Dialog(false, "Sikeres jelszómódosítás", Reload);
                dialog.content = new TextBlock("A jelszavát sikeresen megváltoztatta");
                dialog.Start();
            }
            else ConnectionMessage();
        }

        private void Reload()
        {
            UsersVM usersVM = new UsersVM();
            usersVM.PassChangeLoad();
        }

        private async void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    PassChangeM.RegPass = me.accountdata.Where(b => b.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.PasswordAD).Single();
                    PassChangeM.RegPassSalt = me.accountdata.Where(b => b.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.PassSaltAD).Single();
                    PassChangeM.UserName = me.accountdata.Where(b => b.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.UserNameAD).Single();
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
            if (workingConn) PassChangeM.AcceptChanges();
            else ConnectionMessage();
        }
        protected internal bool CurrentPassCompare(string value)
        {
            return PassChangeM.CurrentPassword.Equals(value);
        }
        protected internal bool UserNameCompare(string value)
        {
            return PassChangeM.UserName.Equals(value);
        }
        protected internal bool PasswordMatch()
        {
            return PasswordManager.IsPasswordMatch(PassChangeM.CurrentPassword, PassChangeM.RegPassSalt, PassChangeM.RegPass);
        }
        protected internal bool VMDirty()
        {
            return PassChangeM.IsChanged;
        }
    }
}
