using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAdministrationSystem.ViewModels.Users
{
    public class DetailsModifyVM : VMExtender
    {
        public DetailsModifyMViewElements DetailsModifyMViewElements { get; set; }
        public DetailsModifyMDataSet DetailsModifyMDataSet { get; set; }
        private BackgroundWorker loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal BackgroundWorker ZipCodeSearch { get; set; }
        protected internal BackgroundWorker SettlementSearch { get; set; }
        protected internal bool nonexist { get; set; }
        private userdata ud { get; set; }
        private bool fromPatient { get; set; } = false;
        protected internal DetailsModifyVM()
        {
            Start();
        }
        protected internal DetailsModifyVM(bool fromPatient)
        {
            this.fromPatient = fromPatient;
            Start();
        }
        private void Start()
        {
            DetailsModifyMViewElements = new DetailsModifyMViewElements();
            DetailsModifyMDataSet = new DetailsModifyMDataSet();
            ZipCodeSearch = new BackgroundWorker();
            ZipCodeSearch.DoWork += new DoWorkEventHandler(ZipCodeDoWork);
            SettlementSearch = new BackgroundWorker();
            SettlementSearch.DoWork += new DoWorkEventHandler(SettlementDoWork);
            loading = new BackgroundWorker();
            loading.DoWork += new DoWorkEventHandler(LoadingModel);
            loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            loading.RunWorkerAsync();
        }
        protected internal void ExecuteMethod()
        {
            if (!nonexist)
            {
                dialog = new Dialog(false, "Módosítás megerősítése", OkMethod, () => { }, true);
                dialog.content = new TextBlock("Biztosan megváltoztatja jelenlegi adatait?");
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
                using (me = new MedicalModel())
                {
                    me.Database.Connection.Open();
                    if (!nonexist) ud = me.userdata.Where(b => b.AccountDataIdUD == DetailsModifyMDataSet.UserID).Single();
                    ud.AccountDataIdUD = DetailsModifyMDataSet.UserID;
                    ud.NameUD = DetailsModifyMViewElements.UserName;
                    if (DetailsModifyMViewElements.BirthName != null) ud.BirthNameUD = DetailsModifyMViewElements.BirthName;
                    ud.JobTitleUD = DetailsModifyMViewElements.JobTitle;
                    if (!DetailsModifyMViewElements.SealNumber.Equals(0)) ud.SealNumberUD = DetailsModifyMViewElements.SealNumber;
                    ud.TAJNumberUD = DetailsModifyMViewElements.TajNumber;
                    if (!DetailsModifyMViewElements.TaxNumber.Equals(0)) ud.TAXNumberUD = DetailsModifyMViewElements.TaxNumber;
                    ud.GenderUD = DetailsModifyMDataSet.FullGenderList.Where(a => a.DataG == DetailsModifyMViewElements.GenderSelected).Select(a => a.IdG).Single();
                    ud.MotherNameUD = DetailsModifyMViewElements.MotherName;
                    ud.BirthDateUD = Convert.ToDateTime(DetailsModifyMViewElements.BirthDate);
                    ud.BirthPlaceUD = DetailsModifyMDataSet.FullSettlementList.Where(a => a.DataS == DetailsModifyMViewElements.BirthPlaceSelected).Select(a => a.IdS).Single();
                    if (!DetailsModifyMViewElements.ZipCodeSelected.Equals(0)) ud.ZipCodeUD = DetailsModifyMDataSet.FullZipCodeList.Where(a => a.DataZC == DetailsModifyMViewElements.ZipCodeSelected).Select(a => a.IdZC).Single();
                    if (DetailsModifyMViewElements.SettlementSelected != null) ud.SettlementUD = DetailsModifyMDataSet.FullSettlementList.Where(a => a.DataS == DetailsModifyMViewElements.SettlementSelected).Select(a => a.IdS).Single();
                    if (DetailsModifyMViewElements.Address != null) ud.AddressUD = DetailsModifyMViewElements.Address;
                    if (DetailsModifyMViewElements.Phone != null) ud.PhoneUD = DetailsModifyMViewElements.Phone;
                    if (DetailsModifyMViewElements.JobPhone != null) ud.JobPhoneUD = DetailsModifyMViewElements.JobPhone;
                    if (DetailsModifyMViewElements.Email != null) ud.EmailUD = DetailsModifyMViewElements.Email;
                    if (nonexist)
                    {
                        me.userdata.Add(ud);
                        me.SaveChanges();
                        GlobalVM.GlobalM.UserID = me.userdata.Where(a => a.AccountDataIdUD == GlobalVM.GlobalM.AccountID).Select(a => a.IdUD).Single();
                    }
                    me.SaveChanges();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
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

                if (nonexist) temp = "rögzítettük";
                else temp = "módosítottuk";

                dialog = new Dialog(false, "Felhasználói adatok", () => { });
                dialog.content = new TextBlock("Sikeresen " + temp + " adatait.");
                dialog.Start();

                DetailsModifyMViewElements.AcceptChanges();
                nonexist = false;
            }
            else ConnectionMessage();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            ud = new userdata();
            try
            {
                using (me = new MedicalModel())
                {
                    me.Database.Connection.Open();
                    DetailsModifyMDataSet.UserID = (int)GlobalVM.GlobalM.AccountID;
                    if (!GlobalVM.GlobalM.UserID.Equals(null))
                    {
                        ud = me.userdata.Where(b => b.IdUD == GlobalVM.GlobalM.UserID).Single();
                        nonexist = false;
                    }
                    else nonexist = true;
                    DetailsModifyMDataSet.FullGenderList = me.gender_fx.ToList();
                    DetailsModifyMDataSet.FullZipCodeList = me.zipcode_fx.ToList();
                    DetailsModifyMDataSet.FullSettlementList = me.settlement_fx.ToList();
                    DetailsModifyMDataSet.SettlementZipSwitch = me.settlementzipcode_st.ToList();
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
                DetailsModifyMDataSet.ViewGenderList = DetailsModifyMDataSet.FullGenderList.Select(a => a.DataG).ToList();
                DetailsModifyMDataSet.ViewZipCodeList = DetailsModifyMDataSet.FullZipCodeList.Select(a => a.DataZC).ToList();
                DetailsModifyMDataSet.ViewSettlementList = DetailsModifyMDataSet.FullSettlementList.Select(a => a.DataS).ToList();
                DetailsModifyMDataSet.ViewBirthPlaceList = DetailsModifyMDataSet.ViewSettlementList;

                if (!nonexist)
                {
                    DetailsModifyMViewElements.UserName = ud.NameUD;
                    DetailsModifyMViewElements.BirthName = ud.BirthNameUD;
                    DetailsModifyMViewElements.JobTitle = ud.JobTitleUD;
                    DetailsModifyMViewElements.SealNumber = ud.SealNumberUD;
                    DetailsModifyMViewElements.TajNumber = ud.TAJNumberUD;
                    DetailsModifyMViewElements.TaxNumber = ud.TAXNumberUD;
                    DetailsModifyMViewElements.GenderSelected = DetailsModifyMDataSet.FullGenderList.Where(a => a.IdG == ud.GenderUD).Select(a => a.DataG).Single();
                    DetailsModifyMViewElements.MotherName = ud.MotherNameUD;
                    DetailsModifyMViewElements.BirthPlaceSelected = DetailsModifyMDataSet.FullSettlementList.Where(a => a.IdS == ud.BirthPlaceUD).Select(a => a.DataS).Single();
                    DetailsModifyMViewElements.BirthDate = ud.BirthDateUD;
                    try
                    {
                        DetailsModifyMViewElements.ZipCodeSelected = DetailsModifyMDataSet.FullZipCodeList.Where(a => a.IdZC == ud.ZipCodeUD).Select(a => a.DataZC).Single();
                    }
                    catch
                    {
                        DetailsModifyMViewElements.ZipCodeSelected = null;
                    }
                    try
                    {
                        DetailsModifyMViewElements.SettlementSelected = DetailsModifyMDataSet.FullSettlementList.Where(a => a.IdS == ud.SettlementUD).Select(a => a.DataS).Single();
                    }
                    catch
                    {
                        DetailsModifyMViewElements.SettlementSelected = null;
                    }
                    DetailsModifyMViewElements.Address = ud.AddressUD;
                    DetailsModifyMViewElements.Phone = ud.PhoneUD;
                    DetailsModifyMViewElements.JobPhone = ud.JobPhoneUD;
                    DetailsModifyMViewElements.Email = ud.EmailUD;
                }

                DetailsModifyMViewElements.AcceptChanges();

                if (nonexist && !fromPatient)
                {
                    dialog = new Dialog(true, "Felhasználói adatok", () => { });
                    dialog.content = new TextBlock("Még nem töltötte ki a felhasználói adatait\n" +
                        "Kérjük töltse ki a megfelelő adatokat");
                    dialog.Start();
                }
            }
            else ConnectionMessage();
            await Loading.Hide();
        }
        private async void ZipCodeDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
            {
                int id = DetailsModifyMDataSet.FullSettlementList.Where(c => c.DataS == settle).Select(c => c.IdS).Single();
                List<int> temp = (DetailsModifyMDataSet.SettlementZipSwitch.Where(a => a.IdS == id).Select(a => a.IdZC)).ToList();
                DetailsModifyMDataSet.ViewZipCodeList = DetailsModifyMDataSet.FullZipCodeList.Where(b => temp.Any(a => a == b.IdZC)).Select(b => b.DataZC).ToList();
            });
        }
        private async void SettlementDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
             {
                 int id = DetailsModifyMDataSet.FullZipCodeList.Where(c => c.DataZC == zip).Select(c => c.IdZC).Single();
                 List<int> temp = (DetailsModifyMDataSet.SettlementZipSwitch.Where(a => a.IdZC == id).Select(a => a.IdS)).ToList();
                 DetailsModifyMDataSet.ViewSettlementList = DetailsModifyMDataSet.FullSettlementList.Where(b => temp.Any(a => a == b.IdS)).Select(b => b.DataS).ToList();
             });
        }
        string settle;
        int zip;
        protected internal void ItemSourceSearcher(string who, string what)
        {
            if (who == "zipCode")
            {
                if (string.IsNullOrEmpty(what))
                    DetailsModifyMDataSet.ViewSettlementList = DetailsModifyMDataSet.FullSettlementList.Select(a => a.DataS).ToList();
                else if (what.Equals("false"))
                    DetailsModifyMDataSet.ViewSettlementList = null;
                else
                {
                    this.zip = Convert.ToInt32(what);
                    if (!SettlementSearch.IsBusy) SettlementSearch.RunWorkerAsync();
                }
            }
            else if (who == "settlement")
                if (string.IsNullOrEmpty(what))
                    DetailsModifyMDataSet.ViewZipCodeList = DetailsModifyMDataSet.FullZipCodeList.Select(a => a.DataZC).ToList();
                else if (what.Equals("false"))
                    DetailsModifyMDataSet.ViewZipCodeList = null;
                else
                {
                    this.settle = what;
                    if (!ZipCodeSearch.IsBusy) ZipCodeSearch.RunWorkerAsync();
                }
        }
        protected internal bool ListChecker(string selected, Type type)
        {
            if (type.Equals(typeof(settlement_fx)))
                return DetailsModifyMDataSet.FullSettlementList.Any(s => s.DataS == selected);
            else if (type.Equals(typeof(gender_fx)))
                return DetailsModifyMDataSet.FullGenderList.Any(g => g.DataG == selected);
            else if (type.Equals(typeof(zipcode_fx)))
                return DetailsModifyMDataSet.FullZipCodeList.Any(z => z.DataZC == Convert.ToInt32(selected));
            return false;
        }
        protected internal bool VMDirty()
        {
            return DetailsModifyMViewElements.IsChanged;
        }
    }
}
