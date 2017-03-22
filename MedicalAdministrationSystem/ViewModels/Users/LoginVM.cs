using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        protected internal LoginVM()
        {
            LoginM = new LoginM();
            Start();
        }
        protected internal async void Start()
        {
            await Task.Run(() =>
            {
                try
                {
                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        me.Database.Connection.Open();
                        LoginM.ExistUsers = me.accountdata.Where(a => a.DeletedAD != true).Select(a => a.UserNameAD).ToList();
                        me.Database.Connection.Close();
                        workingConn = true;
                    }
                    if (ConfigurationManager.ConfigurationManagerM.FacilityId != string.Empty)
                        GlobalVM.GlobalM.CompanyId = Convert.ToInt32(ConfigurationManager.ConfigurationManagerM.FacilityId);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                }
                finally
                {
                    if (!workingConn)
                        LoginM.ExistUsers = new List<string> { ConfigurationManager.ConfigurationManagerM.SecurityUsername };
                }
            }, CancellationToken.None).ContinueWith(async task =>
            {
                LoginM.AcceptChanges();
                if (!workingConn)
                {
                    dialog = new Dialog(true, "Nem sikerült elérni az adatbázist", async () => await Utilities.Loading.Hide());
                    dialog.content = new TextBlock("Adatbáziskapcsolat nélkül nem lehet megfelelően használni az alkalmazást\n" +
                        "Első használat alkalmával be kell konfigurálni az adatbázis kapcsolatot\n" +
                        "Kérjük jelezze a problémát a rendszergazdának");
                    dialog.Start();
                    (GlobalVM.StockLayout.verticalMenu.Children[0] as StockVerticalMenuItem).IsEnabledTrigger = false;
                }
                else await Utilities.Loading.Hide();
            }, TaskScheduler.FromCurrentSynchronizationContext());
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
                dialog = new Dialog(true, "Nincs ilyen felhasználó", () => { });
                dialog.content = new TextBlock("Kérjük ellenőrizze a beírt adatokat");
                dialog.Start();
            }
        }
        protected internal bool UserCheck(string user) => LoginM.ExistUsers.Any(u => u == user);
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            if (workingConn)
            {
                try
                {
                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();
                        GlobalVM.GlobalM.AccountID = await me.accountdata.Where(a => a.UserNameAD == LoginM.Username && a.DeletedAD != true).Select(a => a.IdAD).SingleAsync();
                        try
                        {
                            GlobalVM.GlobalM.UserID = await me.userdata.Where(a => a.AccountDataIdUD == GlobalVM.GlobalM.AccountID).Select(a => a.IdUD).SingleAsync();
                        }
                        catch
                        {
                            GlobalVM.GlobalM.UserID = null;
                        }

                        LoginM.RegPass = await me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.PasswordAD).SingleAsync();
                        LoginM.RegPassSalt = await me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.PassSaltAD).SingleAsync();
                        me.Database.Connection.Close();
                        workingConn = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                }
            }
            else
            {
                LoginM.RegPass = ConfigurationManager.ConfigurationManagerM.SecurityPassword;
                LoginM.RegPassSalt = ConfigurationManager.ConfigurationManagerM.SecurityPasswordSalt;

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
                dialog = new Dialog(true, "Sikertelen belépés", async () =>
                {
                    GlobalVM.GlobalM.AccountID = null;
                    await Utilities.Loading.Hide();
                });
                dialog.content = new TextBlock("Nem egyeznek meg a beírt adatok\n" +
                    "Kérjük ellenőrizze le őket");
                dialog.Start();
            }
        }
        private async void SecurityEnter()
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.SingleChange(GlobalVM.StockLayout.usersTBI, Visibility.Collapsed);
            mbe.SingleChange(GlobalVM.StockLayout.settingsTBI, Visibility.Visible);
            mbe.SingleChange(GlobalVM.StockLayout.logoutTBI, Visibility.Visible);
            GlobalVM.GlobalM.Secure = true;
            await mbe.LoadItem(GlobalVM.StockLayout.settingsTBI);
            await Utilities.Loading.Hide();
        }
        private async void bgw1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    LoginM.Verified = await me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Select(a => a.VerifiedByAdminAD).SingleAsync();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
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
                    dialog = new Dialog(false, "Sikertelen belépés", async () => await Utilities.Loading.Hide());
                    dialog.content = new TextBlock("A beírt adatok megfelelőek\n" +
                        "Viszont amíg a rendszergazda nem hagyja jóvá regisztrációját nem tudja használni a programot\n" +
                        "Kérjük jelezze a rendszergazdának");
                    dialog.Start();
                }
            }
            else ConnectionMessage();

        }
        priviledges pr;
        private async void bgw2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    GlobalVM.GlobalM.PriviledgeID = await me.accountdata.Where(b => b.IdAD == GlobalVM.GlobalM.AccountID).Select(b => b.PriviledgesIdAD).SingleAsync();
                    pr = await me.priviledges.Where(b => b.IdP == GlobalVM.GlobalM.PriviledgeID).SingleAsync();
                    GlobalVM.GlobalM.AllSee = pr.AllSeeP;
                    GlobalVM.GlobalM.JustImportDocuments = pr.JustImportDocumentsP;
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private void bgw2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                dialog = new Dialog(false, "Sikeres belépés", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("Üdvözöljük felhasználóink között");
                dialog.Start();
                new MenuButtonsEnabled(pr).LoadFirst();
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty() => LoginM.IsChanged;
    }
}
