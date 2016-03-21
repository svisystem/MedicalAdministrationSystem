using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace MedicalAdministrationSystem.ViewModels.Users
{
    public class LoginVM : VMExtender
    {
        protected internal LoginM LoginM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private BackgroundWorker bgw1 { get; set; }
        private BackgroundWorker bgw2 { get; set; }
        private BackgroundWorker offlinebgw { get; set; }
        private medicalEntities me { get; set; }
        private Configuration config { get; set; }
        protected internal LoginVM()
        {
            LoginM = new LoginM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    LoginM.ExistUsers = me.accountdata.Where(a => a.DeletedAD != true).Select(a => a.UserNameAD).ToList();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
            finally
            {
                if (!workingConn)
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    LoginM.ExistUsers = new List<string> { config.AppSettings.Settings["securityUserName"].Value };
                }
            }
        }
        private void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            LoginM.AcceptChanges();
            if (!workingConn)
            {
                dialog = new Dialog(true, "Nem sikerült elérni az adatbázist", Dummy);
                dialog.content = new TextBlock("Adatbáziskapcsolat nélkül nem lehet megfelelően használni az alkalmazást\n" +
                    "Első használat alkalmával be kell konfigurálni az adatbázis kapcsolatot\n" +
                    "Kérjük jelezze a problémát a rendszergazdának");
                dialog.Start();
                (GlobalVM.StockLayout.verticalMenu.Children[0] as StockVerticalMenuItem).IsEnabledTrigger = false;
            }
            Utilities.Loading.Hide();
        }
        protected internal async void ExecuteMethod()
        {
            if (UserCheck(LoginM.Username))
            {
                await Utilities.Loading.Show();
                Execute = new BackgroundWorker();
                Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
                Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteCompleted);
                Execute.RunWorkerAsync();
            }
            else
            {
                dialog = new Dialog(true, "Nincs ilyen felhasználó", Dummy);
                dialog.content = new TextBlock("Kérjük ellenőrizze a beírt adatokat");
                dialog.Start();
            }
        }
        protected internal bool UserCheck(string user)
        {
            return LoginM.ExistUsers.Any(u => u == user);
        }
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            if (workingConn)
            {
                try
                {
                    using (me = new medicalEntities())
                    {
                        await me.Database.Connection.OpenAsync();
                        GlobalVM.GlobalM.AccountID = await me.accountdata.Where(a => a.UserNameAD == LoginM.Username && a.DeletedAD != true).Select(a => a.IdAD).SingleAsync();
                        try
                        {
                            GlobalVM.GlobalM.UserID = await me.userdata.Where(a => a.AccountDataIdUD == GlobalVM.GlobalM.AccountID).Select(a => a.IdUD).SingleAsync();
                        }
                        catch
                        {
                            GlobalVM.GlobalM.UserID = 0;
                        }

                        LoginM.RegPass = await me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.PasswordAD).SingleAsync();
                        LoginM.RegPassSalt = await me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.PassSaltAD).SingleAsync();
                        me.Database.Connection.Close();
                        workingConn = true;
                    }
                }
                catch
                {
                    workingConn = false;
                }
            }
            else
            {
                LoginM.RegPass = config.AppSettings.Settings["securityPassword"].Value;
                LoginM.RegPassSalt = config.AppSettings.Settings["securityPasswordSalt"].Value;

            }
        }
        private void ExecuteCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PasswordManager pwm = new PasswordManager();
            if (!workingConn && pwm.IsPasswordMatch(LoginM.Password, LoginM.RegPassSalt, LoginM.RegPass))
            {
                dialog = new Dialog(false, "Sikeres belépés", SecurityEnter);
                dialog.content = new TextBlock("Sikeresen belépett a biztonsági profillal");
                dialog.Start();
            }
            else if (pwm.IsPasswordMatch(LoginM.Password, LoginM.RegPassSalt, LoginM.RegPass))
            {
                bgw1 = new BackgroundWorker();
                bgw1.DoWork += new DoWorkEventHandler(bgw1_DoWork);
                bgw1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw1_RunWorkerCompleted);
                bgw1.RunWorkerAsync();
            }
            else
            {
                dialog = new Dialog(true, "Sikertelen belépés", Utilities.Loading.Hide);
                dialog.content = new TextBlock("Nem egyeznek meg a beírt adatok\n" +
                    "Kérjük ellenőrizze le őket");
                dialog.Start();
            }
        }
        private void SecurityEnter()
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.SingleChange(GlobalVM.StockLayout.usersTBI, Visibility.Collapsed);
            mbe.SingleChange(GlobalVM.StockLayout.settingsTBI, Visibility.Visible);
            mbe.SingleChange(GlobalVM.StockLayout.logoutTBI, Visibility.Visible);
            GlobalVM.GlobalM.Secure = true;
            mbe.LoadItem(GlobalVM.StockLayout.settingsTBI);
            Utilities.Loading.Hide();
        }
        private async void bgw1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    await me.Database.Connection.OpenAsync();
                    LoginM.Verified = await me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.VerifiedByAdminAD).SingleAsync();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
        }
        private void bgw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                if (LoginM.Verified && workingConn)
                {
                    bgw2 = new BackgroundWorker();
                    bgw2.DoWork += new DoWorkEventHandler(bgw2_DoWork);
                    bgw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw2_RunWorkerCompleted);
                    bgw2.RunWorkerAsync();
                }
                else
                {
                    dialog = new Dialog(false, "Sikertelen belépés", Utilities.Loading.Hide);
                    dialog.content = new TextBlock("A beírt adatok megfelelőek\n" +
                        "Viszont amíg a rendszergazda nem hagyja jóvá regisztrációját nem tudja használni a programot\n" +
                        "Kérjük jelezze a rendszergazdának");
                    dialog.Start();
                }
            }
            else ConnectionMessage();

        }
        priviledges_fx pr;
        private async void bgw2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    await me.Database.Connection.OpenAsync();
                    GlobalVM.GlobalM.PriviledgeID = await me.accountdata.Where(b => b.IdAD == GlobalVM.GlobalM.AccountID).Select(b => b.PriviledgesIdAD).SingleAsync();
                    pr = await me.priviledges_fx.Where(b => b.IdP == GlobalVM.GlobalM.PriviledgeID).SingleAsync();
                    GlobalVM.GlobalM.AllSee = pr.AllSeeP;
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
        }
        private void bgw2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                dialog = new Dialog(false, "Sikeres belépés", Load);
                dialog.content = new TextBlock("Üdvözöljük felhasználóink között");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        private void Load()
        {
            GlobalVM.GlobalM.AccountName = LoginM.Username;
            MenuButtonsEnabled mbe = new MenuButtonsEnabled(pr);
            mbe.LoadItem(GlobalVM.StockLayout.usersTBI);
            Utilities.Loading.Hide();
        }
        protected internal bool VMDirty()
        {
            return LoginM.IsChanged;
        }
        private void Dummy() { }
    }
}
