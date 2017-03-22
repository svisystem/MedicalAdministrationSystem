using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Patients;
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
        private bool newForm { get; set; }
        protected internal bool From { get; set; }
        private patientdata selected { get; set; }
        private int? selectedId { get; set; }
        protected internal PatientDetailsVM(bool newForm, string Name = null, string Taj = null, int? Id = null)
        {
            if (!newForm && Id == null) selectedId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).AskId();
            this.newForm = newForm;
            PatientDetailsMViewElements = new PatientDetailsMViewElements();
            if (Id != null)
            {
                selectedId = Id;
                PatientDetailsMViewElements.UserName = Name;
                PatientDetailsMViewElements.TajNumber = Taj;
            }
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
            if (!newForm)
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
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    me.Database.Connection.Open();
                    if (!newForm) selected = me.patientdata.Where(a => a.IdPD == selectedId).Single();
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
                    if (newForm) selected.CreatedPD = DateTime.Now;
                    if (PatientDetailsMViewElements.Notes != null) selected.NotesPD = PatientDetailsMViewElements.Notes;
                    if (newForm)
                    {
                        me.patientdata.Add(selected);
                        me.SaveChanges();

                        if (me.priviledges.Single(p => p.IdP == GlobalVM.GlobalM.PriviledgeID).IsDoctorP)
                            me.belong_st.Add(new belong_st()
                            {
                                IdUD = (int)GlobalVM.GlobalM.UserID,
                                IdPD = selected.IdPD,
                                WhenBelongBS = DateTime.Now
                            });
                        if (newForm && selectedId != null)
                        {
                            scheduleperson_st sp = me.scheduleperson_st.Single(spd => spd.IdSP == me.scheduledata.
                                FirstOrDefault(sd => sd.IdSD == selectedId).PatientIdSD);
                            me.newperson.Remove(me.newperson.Single(np => np.IdNP == sp.NewPersonIdSP));
                            sp.NewPersonIdSP = null;
                            sp.ExistedIdSP = selected.IdPD;
                            me.scheduledata.Where(sd => sd.IdSD == selectedId).Single().StillNotVisitedSD = false;
                        }
                    }
                    me.SaveChanges();
                    me.Database.Connection.Close();
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
                if (!newForm) (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).Refresh((int)selectedId, PatientDetailsMViewElements.UserName);
                string temp;

                if (newForm) temp = "rögzítettük";
                else temp = "módosítottuk";
                dialog = new Dialog(false, "Páciens adatok", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("Sikeresen " + temp + " az adatokat.");
                dialog.Start();
                await new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.patientsTBI);
            }
            else ConnectionMessage();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    me.Database.Connection.Open();
                    PatientDetailsMDataSet.FullGenderList = me.gender_fx.ToList();
                    PatientDetailsMDataSet.FullZipCodeList = me.zipcode_fx.ToList();
                    PatientDetailsMDataSet.FullSettlementList = me.settlement_fx.ToList();
                    PatientDetailsMDataSet.SettlementZipSwitch = me.settlementzipcode_st.ToList();
                    if (!newForm) selected = me.patientdata.Where(a => a.IdPD == selectedId).Single();
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

                if (!newForm)
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

                if (!newForm) PatientDetailsMViewElements.AcceptChanges();
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
                    if (!ZipCodeSearch.IsBusy) ZipCodeSearch.RunWorkerAsync();
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
