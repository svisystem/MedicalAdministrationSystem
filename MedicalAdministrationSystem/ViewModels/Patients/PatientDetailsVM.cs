using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Patients;
using MedicalAdministrationSystem.ViewModels.MenuItem;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAdministrationSystem.ViewModels.Patients
{
    public class PatientDetailsVM : VMExtender
    {
        public PatientDetailsMViewElements PatientDetailsMViewElements { get; set; }
        public PatientDetailsMDataSet PatientDetailsMDataSet { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal BackgroundWorker ZipCodeSearch { get; set; }
        protected internal BackgroundWorker SettlementSearch { get; set; }
        private bool newform { get; set; }
        protected internal bool From { get; set; }
        private patientdata selected { get; set; }
        private int selectedId { get; set; }
        protected internal PatientDetailsVM(bool newform)
        {
            this.newform = newform;
            if (!newform) selectedId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).AskId();
            PatientDetailsMViewElements = new PatientDetailsMViewElements();
            PatientDetailsMDataSet = new PatientDetailsMDataSet();
            ZipCodeSearch = new BackgroundWorker();
            ZipCodeSearch.DoWork += new DoWorkEventHandler(ZipCodeDoWork);
            SettlementSearch = new BackgroundWorker();
            SettlementSearch.DoWork += new DoWorkEventHandler(SettlementDoWork);
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        protected internal async void ExecuteMethod()
        {
            if (!newform)
            {
                await Utilities.Loading.Show();
                dialog = new Dialog(false, "Módosítás megerősítése", OkMethod, () => { }, true);
                dialog.content = new TextBlock("Biztosan megváltoztatja a páciens adatait?");
                dialog.Start();
            }
            else OkMethod();
        }
        private void OkMethod()
        {
            Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteCompleted);
            Execute.RunWorkerAsync();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    if (!newform) selected = me.patientdata.Where(a => a.IdPD == selectedId).Single();
                    else selected = new patientdata();
                    selected.NamePD = PatientDetailsMViewElements.UserName;
                    if (PatientDetailsMViewElements.BirthName != null) selected.BirthNamePD = PatientDetailsMViewElements.BirthName;
                    selected.GenderPD = PatientDetailsMDataSet.FullGenderList.Where(a => a.DataG == PatientDetailsMViewElements.GenderSelected).Select(a => a.IdG).Single();
                    selected.MotherNamePD = PatientDetailsMViewElements.MotherName;
                    selected.BirthPlacePD = PatientDetailsMDataSet.FullSettlementList.Where(a => a.DataS == PatientDetailsMViewElements.BirthPlaceSelected).Select(a => a.IdS).Single();
                    selected.BirthDatePD = (DateTime)PatientDetailsMViewElements.BirthDate;
                    selected.TAJNumberPD = PatientDetailsMViewElements.TajNumber;
                    if (!PatientDetailsMViewElements.TaxNumber.Equals(0)) selected.TAXNumberPD = PatientDetailsMViewElements.TaxNumber;
                    selected.ZipCodePD = PatientDetailsMDataSet.FullZipCodeList.Where(a => a.DataZC == PatientDetailsMViewElements.ZipCodeSelected).Select(a => a.IdZC).Single();
                    selected.SettlementPD = PatientDetailsMDataSet.FullSettlementList.Where(a => a.DataS == PatientDetailsMViewElements.SettlementSelected).Select(a => a.IdS).Single();
                    selected.AddressPD = PatientDetailsMViewElements.Address;
                    if (PatientDetailsMViewElements.Phone != null) selected.PhonePD = PatientDetailsMViewElements.Phone;
                    if (PatientDetailsMViewElements.MobilePhone != null) selected.MobilePhonePD = PatientDetailsMViewElements.MobilePhone;
                    if (PatientDetailsMViewElements.Email != null) selected.EmailPD = PatientDetailsMViewElements.Email;
                    selected.BillingNamePD = PatientDetailsMViewElements.BillingName;
                    selected.BillingZipCodePD = PatientDetailsMDataSet.FullZipCodeList.Where(a => a.DataZC == PatientDetailsMViewElements.BillingZipCode).Select(a => a.IdZC).Single();
                    selected.BillingSettlementPD = PatientDetailsMDataSet.FullSettlementList.Where(a => a.DataS == PatientDetailsMViewElements.BillingSettlement).Select(a => a.IdS).Single();
                    selected.BillingAddressPD = PatientDetailsMViewElements.BillingAddress;
                    if (PatientDetailsMViewElements.Notes != null) selected.NotesPD = PatientDetailsMViewElements.Notes;
                    if (newform)
                    {
                        me.patientdata.Add(selected);
                        me.SaveChanges();
                        int id = me.patientdata.Where(a => a.NamePD == selected.NamePD).Select(a => a.IdPD).Single();
                        belong_st bt = new belong_st();
                        bt.IdUD = (int)GlobalVM.GlobalM.UserID;
                        bt.IdPD = id;
                        me.belong_st.Add(bt);
                    }
                    me.SaveChanges();
                    me.Database.Connection.Close();
                }
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private void ExecuteCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                string temp;

                if (newform) temp = "rögzítettük";
                else temp = "módosítottuk";


                PatientDetailsMViewElements.AcceptChanges();
                dialog = new Dialog(false, "Páciens adatok", Reload);
                dialog.content = new TextBlock("Sikeresen " + temp + " az adatokat.");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        private async void Reload()
        {
            if (newform)
            {
                PatientsVM PatientsVM = new PatientsVM();
                PatientsVM.NewPatientLoad();
            }
            await Utilities.Loading.Hide();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    PatientDetailsMDataSet.FullGenderList = me.gender_fx.ToList();
                    PatientDetailsMDataSet.FullZipCodeList = me.zipcode_fx.ToList();
                    PatientDetailsMDataSet.FullSettlementList = me.settlement_fx.ToList();
                    PatientDetailsMDataSet.SettlementZipSwitch = me.settlementzipcode_st.ToList();
                    if (!newform) selected = me.patientdata.Where(a => a.IdPD == selectedId).Single();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                PatientDetailsMDataSet.ViewGenderList = PatientDetailsMDataSet.FullGenderList.Select(a => a.DataG).ToList();
                PatientDetailsMDataSet.ViewZipCodeList = PatientDetailsMDataSet.FullZipCodeList.Select(a => a.DataZC).ToList();
                PatientDetailsMDataSet.ViewBillingZipCodeList = PatientDetailsMDataSet.ViewZipCodeList;
                PatientDetailsMDataSet.ViewSettlementList = PatientDetailsMDataSet.FullSettlementList.Select(a => a.DataS).ToList();
                PatientDetailsMDataSet.ViewBirthPlaceList = PatientDetailsMDataSet.ViewSettlementList;
                PatientDetailsMDataSet.ViewBillingSettlementList = PatientDetailsMDataSet.ViewSettlementList;

                if (!newform)
                {
                    PatientDetailsMViewElements.UserName = selected.NamePD;
                    try
                    {
                        PatientDetailsMViewElements.BirthName = selected.BirthNamePD;
                    }
                    catch
                    {
                        PatientDetailsMViewElements.BirthName = null;
                    }
                    PatientDetailsMViewElements.GenderSelected = PatientDetailsMDataSet.FullGenderList.Where(a => a.IdG == selected.GenderPD).Select(a => a.DataG).Single();
                    PatientDetailsMViewElements.MotherName = selected.MotherNamePD;
                    PatientDetailsMViewElements.BirthPlaceSelected = PatientDetailsMDataSet.FullSettlementList.Where(a => a.IdS == selected.BirthPlacePD).Select(a => a.DataS).Single();
                    PatientDetailsMViewElements.BirthDate = selected.BirthDatePD;
                    PatientDetailsMViewElements.TajNumber = selected.TAJNumberPD;
                    PatientDetailsMViewElements.TaxNumber = selected.TAXNumberPD;
                    PatientDetailsMViewElements.ZipCodeSelected = PatientDetailsMDataSet.FullZipCodeList.Where(a => a.IdZC == selected.ZipCodePD).Select(a => a.DataZC).Single();
                    PatientDetailsMViewElements.SettlementSelected = PatientDetailsMDataSet.FullSettlementList.Where(a => a.IdS == selected.SettlementPD).Select(a => a.DataS).Single();
                    PatientDetailsMViewElements.Address = selected.AddressPD;
                    try
                    {
                        PatientDetailsMViewElements.Phone = selected.PhonePD;
                    }
                    catch
                    {
                        PatientDetailsMViewElements.Phone = null;
                    }
                    try
                    {
                        PatientDetailsMViewElements.MobilePhone = selected.MobilePhonePD;
                    }
                    catch
                    {
                        PatientDetailsMViewElements.MobilePhone = null;
                    }
                    try
                    {
                        PatientDetailsMViewElements.Email = selected.EmailPD;
                    }
                    catch
                    {
                        PatientDetailsMViewElements.Email = null;
                    }
                    PatientDetailsMViewElements.BillingName = selected.BillingNamePD;
                    PatientDetailsMViewElements.BillingZipCode = PatientDetailsMDataSet.FullZipCodeList.Where(a => a.IdZC == selected.BillingZipCodePD).Select(a => a.DataZC).Single();
                    PatientDetailsMViewElements.BillingSettlement = PatientDetailsMDataSet.FullSettlementList.Where(a => a.IdS == selected.BillingSettlementPD).Select(a => a.DataS).Single();
                    PatientDetailsMViewElements.BillingAddress = selected.BillingAddressPD;
                    try
                    {
                        PatientDetailsMViewElements.Notes = selected.NotesPD;
                    }
                    catch
                    {
                        PatientDetailsMViewElements.Notes = null;
                    }
                }
                await Utilities.Loading.Hide();
                PatientDetailsMViewElements.AcceptChanges();
            }
            else ConnectionMessage();
        }
        protected internal void Copy()
        {
            PatientDetailsMViewElements.BillingName = PatientDetailsMViewElements.UserName;
            PatientDetailsMViewElements.BillingZipCode = PatientDetailsMViewElements.ZipCodeSelected;
            PatientDetailsMViewElements.BillingSettlement = PatientDetailsMViewElements.SettlementSelected;
            PatientDetailsMViewElements.BillingAddress = PatientDetailsMViewElements.Address;
        }
        private async void ZipCodeDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
            {
                int id = PatientDetailsMDataSet.FullSettlementList.Where(c => c.DataS == settle).Select(c => c.IdS).Single();
                List<int> temp = (PatientDetailsMDataSet.SettlementZipSwitch.Where(a => a.IdS == id).Select(a => a.IdZC)).ToList();
                List<int> temp2 = PatientDetailsMDataSet.FullZipCodeList.Where(b => temp.Any(a => a == b.IdZC)).Select(b => b.DataZC).ToList();
                if (From) PatientDetailsMDataSet.ViewZipCodeList = temp2;
                else PatientDetailsMDataSet.ViewBillingZipCodeList = temp2;
            });
        }

        private async void SettlementDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
            {
                int id = PatientDetailsMDataSet.FullZipCodeList.Where(c => c.DataZC == zip).Select(c => c.IdZC).Single();
                List<int> temp = (PatientDetailsMDataSet.SettlementZipSwitch.Where(a => a.IdZC == id).Select(a => a.IdS)).ToList();
                List<string> temp2 = PatientDetailsMDataSet.FullSettlementList.Where(b => temp.Any(a => a == b.IdS)).Select(b => b.DataS).ToList();
                if (From) PatientDetailsMDataSet.ViewSettlementList = temp2;
                else PatientDetailsMDataSet.ViewBillingSettlementList = temp2;
            });
        }
        string settle;
        int zip;
        protected internal void ItemSourceSearcher(string who, string what)
        {
            if (who == "zipCode")
            {
                if (string.IsNullOrEmpty(what))
                    PatientDetailsMDataSet.ViewSettlementList = PatientDetailsMDataSet.FullSettlementList.Select(a => a.DataS).ToList();
                else if (what.Equals("false"))
                    PatientDetailsMDataSet.ViewSettlementList = null;
                else
                {
                    this.zip = Convert.ToInt32(what);
                    if (!SettlementSearch.IsBusy) SettlementSearch.RunWorkerAsync();
                }
            }
            else if (who == "settlement")
                if (string.IsNullOrEmpty(what))
                    PatientDetailsMDataSet.ViewZipCodeList = PatientDetailsMDataSet.FullZipCodeList.Select(a => a.DataZC).ToList();
                else if (what.Equals("false"))
                    PatientDetailsMDataSet.ViewZipCodeList = null;
                else
                {
                    this.settle = what;
                    if (!SettlementSearch.IsBusy) ZipCodeSearch.RunWorkerAsync();
                }
        }
        protected internal bool ListChecker(string selected, Type type)
        {
            if (type.Equals(typeof(settlement_fx)))
                return PatientDetailsMDataSet.FullSettlementList.Any(s => s.DataS == selected);
            else if (type.Equals(typeof(gender_fx)))
                return PatientDetailsMDataSet.FullGenderList.Any(g => g.DataG == selected);
            else if (type.Equals(typeof(zipcode_fx)))
                return PatientDetailsMDataSet.FullZipCodeList.Any(z => z.DataZC == Convert.ToInt32(selected));
            return false;
        }
        protected internal bool VMDirty()
        {
            return PatientDetailsMViewElements.IsChanged;
        }
    }
}
