using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class UserAdministrateVM : VMExtender
    {
        public UserAdministrateMDataSet UsersMDataSet { get; set; }
        public UserAdministrateMViewElements UsersMViewElements { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private BackgroundWorker NewPassThread { get; set; }
        private ObservableCollection<UserAdministrateMViewElements.UserRow> temp { get; set; }
        private NewPass newPass { get; set; }
        private Action Loaded { get; set; }
        protected internal UserAdministrateVM(Action Loaded)
        {
            this.Loaded = Loaded;
            UsersMDataSet = new UserAdministrateMDataSet();
            UsersMViewElements = new UserAdministrateMViewElements();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        protected internal async void Refresh()
        {
            await Utilities.Loading.Show();
            new FormChecking(Loading.RunWorkerAsync, delegate { }, true);
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();
                UsersMViewElements.UserDatas = me.accountdata.Where(a => a.DeletedAD == false).ToList();

                UsersMDataSet.PriviledgesList = me.priviledges_fx.ToList();
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
            UsersMViewElements.Priviledges = UsersMDataSet.PriviledgesList.Select(a => a.NameP).ToList();
            UsersMViewElements.Users.Clear();
            for (int i = 0; i < UsersMViewElements.UserDatas.Count; i++)
            {
                if (!UsersMViewElements.UserDatas[i].DeletedAD)
                {
                    UsersMViewElements.Users.Add(new UserAdministrateMViewElements.UserRow
                    {
                        Id = UsersMViewElements.UserDatas[i].IdAD,
                        RegistrationDate = UsersMViewElements.UserDatas[i].RegistrateTimeAD,
                        UserName = UsersMViewElements.UserDatas[i].UserNameAD,
                        Priviledge = UsersMDataSet.PriviledgesList.Where(a => a.IdP == UsersMViewElements.UserDatas[i].PriviledgesIdAD).Select(a => a.NameP).Single(),
                        Verified = UsersMViewElements.UserDatas[i].VerifiedByAdminAD,
                        PassModified = false,
                        Enabled = true });
                }
            }
        }
        private void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                FindMyself();
                foreach (object row in UsersMViewElements.Users)
                    (row as UserAdministrateMViewElements.UserRow).AcceptChanges();
                UsersMViewElements.AcceptChanges();
                Loaded();
            }
            else ConnectionMessage();
        }
        private void FindMyself()
        {
            UsersMViewElements.Users.Where(m => m.Id == GlobalVM.GlobalM.AccountID).Single().Enabled = false;
        }
        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteComplete);
            Execute.RunWorkerAsync();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();
                if (UsersMViewElements.Erased.Count != 0)
                {
                    foreach (int user in UsersMViewElements.Erased)
                        try
                        {
                            me.accountdata.Where(a => a.IdAD == user).Single().DeletedAD = true;
                        }
                        catch { }
                }
                for (int i = 0; i < UsersMViewElements.UserDatas.Count; i++)
                {
                    int temp = UsersMViewElements.UserDatas[i].IdAD;
                    try
                    {
                        accountdata ac = new accountdata();
                        ac = me.accountdata.Where(a => a.IdAD == temp).Single();
                        if (!UsersMViewElements.UserDatas[i].PasswordAD.Equals(ac.PasswordAD))
                        {
                            ac.PasswordAD = UsersMViewElements.UserDatas[i].PasswordAD;
                            ac.PassSaltAD = UsersMViewElements.UserDatas[i].PassSaltAD;
                            me.SaveChanges();
                        }
                        if (UsersMViewElements.Users[i].Priviledge != UsersMDataSet.PriviledgesList.Where(a => a.IdP == ac.PriviledgesIdAD).Select(a => a.NameP).Single())
                        {
                            string currpriv = UsersMViewElements.Users[i].Priviledge;
                            ac.PriviledgesIdAD = UsersMDataSet.PriviledgesList.Where(a => a.NameP == currpriv).Select(a => a.IdP).Single();
                            me.SaveChanges();
                        }
                        if (!UsersMViewElements.Users[i].Verified.Equals(ac.VerifiedByAdminAD))
                        {
                            ac.VerifiedByAdminAD = UsersMViewElements.Users[i].Verified;
                            me.SaveChanges();
                        }
                    }
                    catch { }
                }
                me.SaveChanges();
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                UsersMViewElements.Erased.Clear();
                foreach (object row in UsersMViewElements.Users)
                    (row as UserAdministrateMViewElements.UserRow).AcceptChanges();
                UsersMViewElements.AcceptChanges();

                dialog = new Dialog(false, "Módosítások mentése", async delegate { await Utilities.Loading.Hide(); });
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty()
        {
            if (UsersMViewElements.Erased.Count != 0) return true;
            return UsersMViewElements.Users.Any(u => u.IsChanged);
        }
        protected internal void NewPassMethod()
        {
            dialog = new Dialog(false, "Jelszó megváltoztatása", OkMethod, delegate { }, false);
            newPass = new NewPass(dialog.YesButton(), UsersMDataSet.SelectedRow.UserName);
            dialog.content = newPass;
            dialog.Start();
        }
        private void OkMethod()
        {
            dialog = new Dialog(false, "Jelszó megváltoztatása", NewPassStart);
            dialog.content = new TextBlock("A felhasználó jelszavát sikeresen módosítottuk\n" +
                "A változtatás véglegesítéséhez nyomja meg a \"Változtatások mentése\" gombot");
            dialog.Start();
        }
        private void NewPassStart()
        {
            NewPassThread = new BackgroundWorker();
            NewPassThread.DoWork += new DoWorkEventHandler(NewPassThreadDoWork);
            NewPassThread.RunWorkerAsync();
        }
        private void NewPassThreadDoWork(object sender, DoWorkEventArgs e)
        {
            PasswordManager pwm = new PasswordManager();
            UsersMViewElements.UserDatas.Where(b => b.UserNameAD == UsersMDataSet.SelectedRow.UserName).Single().PassSaltAD = pwm.GetSaltString();
            UsersMViewElements.UserDatas.Where(b => b.UserNameAD == UsersMDataSet.SelectedRow.UserName).Single().PasswordAD = pwm.GenerateHashWithSalt(newPass.NewPassVM.Pass(),
                UsersMViewElements.UserDatas.Where(b => b.UserNameAD == UsersMDataSet.SelectedRow.UserName).Select(b => b.PassSaltAD).Single());
            UsersMViewElements.Users.Where(b => b.UserName == UsersMDataSet.SelectedRow.UserName).Single().PassModified = true;
        }
        protected internal void UserEraseMethod()
        {
            dialog = new Dialog(true, "Felhasználó törlése", Erase, delegate { }, true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott felhasználói fiókot?\n" +
              "A felhasználói fiók törlése csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private void Erase()
        {
            UsersMViewElements.Erased.Add(UsersMViewElements.UserDatas.Where(a => a.IdAD == UsersMDataSet.SelectedRow.Id).Select(a => a.IdAD).Single());
            UsersMViewElements.UserDatas.Remove(UsersMViewElements.UserDatas.Where(b => b.IdAD == UsersMViewElements.Erased.Last()).Single());
            UsersMViewElements.Users.Remove(UsersMViewElements.Users.Where(b => b.UserName == UsersMDataSet.SelectedRow.UserName).Single());
            Loaded();
        }
    }
}
