using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Patients;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Patients
{
    public class PatientListVM : VMExtender
    {
        public PatientListM PatientListM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker RefreshTable { get; set; }
        private BackgroundWorker Execute { get; set; }
        private BackgroundWorker EraseBackground { get; set; }
        private Action Loaded { get; set; }
        protected internal PatientListVM(Action Loaded)
        {
            this.Loaded = Loaded;
            PatientListM = new PatientListM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            RefreshTable = new BackgroundWorker();
            RefreshTable.DoWork += new DoWorkEventHandler(RefreshTableDoWork);
            RefreshTable.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RefreshTableComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();
                PatientListM.FullZipCodeList = me.zipcode_fx.ToList();
                PatientListM.FullSettlementList = me.settlement_fx.ToList();
                PatientListM.FullUsersList = me.userdata.Where(a => !me.accountdata.Where(b => b.IdAD == a.AccountDataIdUD).FirstOrDefault().DeletedAD).ToList()
                    .Where(n => me.priviledges_fx.Where(p => p.IdP == me.accountdata.Where(b => b.IdAD == n.AccountDataIdUD).FirstOrDefault().PriviledgesIdAD).FirstOrDefault().IsDoctorP)
                    .Select(a => new PatientListM.UserList
                    {
                        Id = a.IdUD,
                        Name = a.NameUD
                    }).ToList();
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                if (GlobalVM.GlobalM.AllSee)
                {
                    List<PatientListM.UserList> enabled = new List<PatientListM.UserList>(PatientListM.FullUsersList);
                    enabled.Insert(0, new PatientListM.UserList { Id = 0, Name = "Mindenki" });
                    PatientListM.UserList select = PatientListM.SelectedUser;
                    PatientListM.UserSelectionList = enabled;
                    if (select == null) PatientListM.SelectedUser = PatientListM.UserSelectionList.Where(a => a.Id == 0).Single();
                    else
                    {
                        try
                        {
                            PatientListM.SelectedUser = PatientListM.UserSelectionList.Where(a => a.Id == select.Id).Single();
                        }
                        catch
                        {
                            PatientListM.SelectedUser = PatientListM.UserSelectionList.Where(a => a.Id == 0).Single();
                        }
                    }
                }
                else
                {
                    PatientListM.SelectedUser = new PatientListM.UserList { Id = (int)GlobalVM.GlobalM.UserID };
                }
            }
            else ConnectionMessage();
        }

        List<PatientListM.Patient> temp;
        private void RefreshTableDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                if (!PatientListM.SelectedUser.Id.Equals(0))
                {
                    List<int> belongToCurr = me.belong_st.Where(a => a.IdUD == PatientListM.SelectedUser.Id).Select(a => a.IdPD).ToList();
                    temp = me.patientdata.Where(a => belongToCurr.Contains(a.IdPD)).Select(a => new PatientListM.Patient
                    {
                        Id = a.IdPD,
                        Name = a.NamePD,
                        BirthName = a.BirthNamePD,
                        BirthPlaceId = a.BirthPlacePD,
                        BirthDate = a.BirthDatePD,
                        TajNumber = a.TAJNumberPD,
                        ZipCodeId = a.ZipCodePD,
                        SettlementId = a.SettlementPD,
                        Address = a.AddressPD,
                        Belong = me.belong_st.Where(b => b.IdPD == a.IdPD).Select(b => b.IdUD).ToList()
                    }).ToList();
                }
                else if (GlobalVM.GlobalM.AllSee)
                    temp = me.patientdata.Select(a => new PatientListM.Patient
                    {
                        Id = a.IdPD,
                        Name = a.NamePD,
                        BirthName = a.BirthNamePD,
                        BirthPlaceId = a.BirthPlacePD,
                        BirthDate = a.BirthDatePD,
                        TajNumber = a.TAJNumberPD,
                        ZipCodeId = a.ZipCodePD,
                        SettlementId = a.SettlementPD,
                        Address = a.AddressPD,
                        Belong = me.belong_st.Where(b => b.IdPD == a.IdPD).Select(b => b.IdUD).ToList()
                    }).ToList();
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private void RefreshTableComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                BackgroundWorker LoadingList = new BackgroundWorker();
                LoadingList.DoWork += new DoWorkEventHandler(this.LoadingList);
                LoadingList.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingListComplete);
                LoadingList.RunWorkerAsync();
            }
            else ConnectionMessage();
        }
        private void LoadingList(object sender, DoWorkEventArgs e)
        {
            if (temp != null) foreach (PatientListM.Patient patient in temp)
                {
                    patient.BirthPlace = PatientListM.FullSettlementList.Where(b => b.IdS == patient.BirthPlaceId).Select(b => b.DataS).Single();
                    patient.ZipCode = PatientListM.FullZipCodeList.Where(b => b.IdZC == patient.ZipCodeId).Select(b => b.DataZC).Single();
                    patient.Settlement = PatientListM.FullSettlementList.Where(b => b.IdS == patient.SettlementId).Select(b => b.DataS).Single();
                    patient.BelongUsers = PatientListM.FullUsersList.Select(a => new PatientListM.UserList
                    {
                        Belong = a.Belong,
                        Id = a.Id,
                        Name = a.Name
                    }).ToList();
                }
        }
        private async void LoadingListComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (temp != null)
            {
                foreach (PatientListM.Patient patient in temp)
                    foreach (int Id in patient.Belong)
                        try
                        {
                            patient.BelongUsers.Where(a => a.Id == Id).Single().Belong = true;
                        }
                        catch { }
                PatientListM.PatientList = new ObservableCollection<PatientListM.Patient>(temp);
            }

            await Utilities.Loading.Hide();
            PatientListM.Erased.Clear();
            Loaded();

            foreach (PatientListM.Patient row in PatientListM.PatientList)
            {
                row.AcceptChanges();
                foreach (PatientListM.UserList user in row.BelongUsers)
                    user.AcceptChanges();
            }
        }

        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            BackgroundWorker Execute = new BackgroundWorker();
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
                if (PatientListM.Erased.Count != 0)
                {
                    foreach (int patient in PatientListM.Erased)
                        try
                        { //TODO
                            me.belong_st.RemoveRange(me.belong_st.Where(a => a.IdPD == patient));
                            me.examinationeachevidence_st.RemoveRange(me.examinationeachevidence_st.Where
                                (b => me.examinationdata.Where(a => a.PatientIdEX == patient).Select(a => a.IdEX).ToList().Any(c => c == b.IdEX)));
                            me.examinationdata.RemoveRange(me.examinationdata.Where(a => a.PatientIdEX == patient));
                            me.evidencedata.RemoveRange(me.evidencedata.Where(a => a.PatientIdED == patient));
                            me.scheduledata.RemoveRange(me.scheduledata.Where(a => a.PatientIdSD == patient));
                            me.patientdata.Remove(me.patientdata.Where(a => a.IdPD == patient).Single());
                        }
                        catch { }
                    me.SaveChanges();
                }
                foreach (PatientListM.Patient patient in PatientListM.PatientList)
                    if (!patient.BelongUsers.Where(a => a.Belong == true).Select(a => a.Id).ToList().SequenceEqual(patient.Belong))
                        foreach (PatientListM.UserList user in patient.BelongUsers)
                        {
                            belong_st temp = me.belong_st.Where(a => a.IdPD == patient.Id && a.IdUD == user.Id).FirstOrDefault();
                            if (user.Belong && temp == null)
                            {
                                me.belong_st.Add(new belong_st { IdPD = patient.Id, IdUD = user.Id });
                                patient.Belong.Add(user.Id);
                            }
                            if (!user.Belong && temp != null)
                            {
                                me.belong_st.Remove(me.belong_st.Where(a => a.IdPD == patient.Id && a.IdUD == user.Id).Single());
                                patient.Belong.Remove(user.Id);
                            }
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
        private async void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                PatientListM.Erased.Clear();
                await Utilities.Loading.Hide();
                foreach (object row in PatientListM.PatientList)
                    foreach (PatientListM.UserList user in (row as PatientListM.Patient).BelongUsers)
                        user.AcceptChanges();

                dialog = new Dialog(false, "Módosítások mentése", () => { });
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        bool fullRefresh;
        protected internal async void Question(bool fullRefreshGiven)
        {
            Action Ok;
            Action No;
            if (fullRefreshGiven)
            {
                await Utilities.Loading.Show();
                Ok = Loading.RunWorkerAsync;
                fullRefresh = fullRefreshGiven;
            }
            else Ok = RefreshTable.RunWorkerAsync;
            No = async () => await Utilities.Loading.Hide();
            if (VMDirty() && (fullRefreshGiven || (!fullRefreshGiven && !fullRefresh)))
            {
                dialog = new Dialog(true, "El nem menetett változások lehetnek az adott oldalon", Ok, No, true);
                dialog.content = new TextBlock("Amennyiben mentés nélkül frissíti a táblázatot, az Ön által végrehajtott változtatások nem kerülnek mentésre\n" +
                    "Biztosan frissíti a táblázatot?");
                dialog.Start();
            }
            else Ok();
            fullRefresh = fullRefreshGiven;
        }
        protected internal void Select(Action Selected)
        {
            SelectedPatient selectedPatient = new SelectedPatient(Selected);
            GlobalVM.StockLayout.headerContent.Content = selectedPatient;
            selectedPatient.Load(PatientListM.SelectedRow.Id, PatientListM.SelectedRow.Name);
        }
        protected internal void PatientEraseMethod()
        {
            dialog = new Dialog(true, "Páciens törlése", Erase, () => { }, true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott páciens összes adatát?\n" +
                "A páciens törlése csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private void Erase()
        {
            PatientListM.Erased.Add(PatientListM.SelectedRow.Id);
            PatientListM.PatientList.Remove(PatientListM.PatientList.Where(b => b.Id == PatientListM.Erased.Last()).Single());
        }
        protected internal bool VMDirty()
        {
            if (PatientListM.Erased.Count != 0) return true;
            if (PatientListM.PatientList.Any(p => p.IsChanged)) return true;
            return PatientListM.PatientList.Any(p => p.BelongUsers.Any(b => b.IsChanged));
        }
    }
}
