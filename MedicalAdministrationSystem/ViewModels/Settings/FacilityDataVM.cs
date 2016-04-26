using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System.Threading.Tasks;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class FacilityDataVM : VMExtender
    {
        public FacilityDataMViewElements FacilityDataMViewElements { get; set; }
        public FacilityDataMDataSet FacilityDataMDataSet { get; set; }
        private BackgroundWorker Loading { get; set; }
        protected internal BackgroundWorker Refresh { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal BackgroundWorker ZipCodeSearch { get; set; }
        protected internal BackgroundWorker SettlementSearch { get; set; }
        private Configuration config { get; set; }
        private companydata cd { get; set; }
        protected internal FacilityDataVM()
        {
            FacilityDataMViewElements = new FacilityDataMViewElements();
            FacilityDataMDataSet = new FacilityDataMDataSet();
            ZipCodeSearch = new BackgroundWorker();
            ZipCodeSearch.DoWork += new DoWorkEventHandler(ZipCodeDoWork);
            SettlementSearch = new BackgroundWorker();
            SettlementSearch.DoWork += new DoWorkEventHandler(SettlementDoWork);
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Refresh = new BackgroundWorker();
            Refresh.DoWork += new DoWorkEventHandler(RefreshModel);
            Refresh.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RefreshModelComplete);
            Loading.RunWorkerAsync();
        }
        List<FacilityDataMDataSet.Company> temp;
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    FacilityDataMDataSet.FullZipCodeList = me.zipcode_fx.ToList();
                    FacilityDataMDataSet.FullSettlementList = me.settlement_fx.ToList();
                    FacilityDataMDataSet.SettlementZipSwitch = me.settlementzipcode_st.ToList();
                    temp = me.companydata
                        .Select(a => new FacilityDataMDataSet.Company
                        {
                            ID = a.IdCD,
                            Name = a.NameCD
                        }).ToList();
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
                FacilityDataMDataSet.ViewZipCodeList = FacilityDataMDataSet.FullZipCodeList.Select(a => a.DataZC).ToList();
                FacilityDataMDataSet.ViewSettlementList = FacilityDataMDataSet.FullSettlementList.Select(a => a.DataS).ToList();
                temp.Insert(0, new FacilityDataMDataSet.Company
                {
                    ID = 0,
                    Name = "Új Intézmény felvétele"
                });
                FacilityDataMDataSet.Companies = temp;
                FacilityDataMViewElements.AcceptChanges();
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings["facilityId"].Value != "")
                    FacilityDataMDataSet.SelectedCompany = FacilityDataMDataSet.Companies.Where(a => a.ID == Convert.ToInt32(config.AppSettings.Settings["facilityId"].Value)).Single();
                else FacilityDataMDataSet.SelectedCompany = FacilityDataMDataSet.Companies.Where(a => a.ID == 0).Single();
            }
            else ConnectionMessage();

            await Utilities.Loading.Hide();
        }
        private void RefreshModel(object sender, DoWorkEventArgs e)
        {
            if (!FacilityDataMDataSet.SelectedCompany.ID.Equals(0))
            {
                try
                {
                    using (me = new medicalEntities())
                    {
                        me.Database.Connection.Open();
                        cd = me.companydata.Where(b => b.IdCD == FacilityDataMDataSet.SelectedCompany.ID).Single();
                        me.Database.Connection.Close();
                        workingConn = true;
                    }
                }
                catch
                {
                    workingConn = false;
                }
            }
        }
        private void RefreshModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                if (!FacilityDataMDataSet.SelectedCompany.ID.Equals(0))
                {
                    FacilityDataMViewElements.Name = cd.NameCD;
                    FacilityDataMViewElements.ZipCode = FacilityDataMDataSet.FullZipCodeList.Where(a => a.IdZC == cd.ZipCodeCD).Select(a => a.DataZC).Single();
                    FacilityDataMViewElements.Settlement = FacilityDataMDataSet.FullSettlementList.Where(a => a.IdS == cd.SettlementCD).Select(a => a.DataS).Single();
                    FacilityDataMViewElements.Address = cd.AddressCD;
                    FacilityDataMViewElements.TaxNumber = cd.TAXNumberCD;
                    try
                    {
                        FacilityDataMViewElements.RegistrationNumber = cd.RegistrationNumberCD;
                    }
                    catch
                    {
                        FacilityDataMViewElements.RegistrationNumber = null;
                    }
                    try
                    {
                        FacilityDataMViewElements.InvoiceNumber = cd.InvoiceNumberCD;
                    }
                    catch
                    {
                        FacilityDataMViewElements.InvoiceNumber = null;
                    }
                    try
                    {
                        FacilityDataMViewElements.Phone = cd.PhoneCD;
                    }
                    catch
                    {
                        FacilityDataMViewElements.Phone = null;
                    }
                    try
                    {
                        FacilityDataMViewElements.Email = cd.EmailCD;
                    }
                    catch
                    {
                        FacilityDataMViewElements.Email = null;
                    }
                    try
                    {
                        FacilityDataMViewElements.WebPage = cd.WebPageCD;
                    }
                    catch
                    {
                        FacilityDataMViewElements.WebPage = null;
                    }
                }
                else
                {
                    FacilityDataMViewElements.Name = null;
                    FacilityDataMViewElements.ZipCode = null;
                    FacilityDataMViewElements.Settlement = null;
                    FacilityDataMViewElements.Address = null;
                    FacilityDataMViewElements.TaxNumber = null;
                    FacilityDataMViewElements.RegistrationNumber = null;
                    FacilityDataMViewElements.InvoiceNumber = null;
                    FacilityDataMViewElements.Phone = null;
                    FacilityDataMViewElements.Email = null;
                    FacilityDataMViewElements.WebPage = null;
                }
                FacilityDataMViewElements.AcceptChanges();
            }
            else ConnectionMessage();
        }
        protected internal void ExecuteMethod()
        {
            string text;
            if (!FacilityDataMDataSet.SelectedCompany.ID.Equals(0))
                text = "Biztosan megváltoztatja a kiválasztott intézmény adatait";
            else text = "Valóban elmenti a megadott adatokat új intézményként";
            dialog = new Dialog(false, "Intézmény adatok mentése", OkMethod, () => { }, true);
            dialog.content = new TextBlock(text + " és alkalmazza jelenlegi intézmény adatokkén?");
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
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    if (!FacilityDataMDataSet.SelectedCompany.ID.Equals(0)) cd = me.companydata.Where(b => b.IdCD == FacilityDataMDataSet.SelectedCompany.ID).Single();
                    else cd = new companydata();
                    cd.NameCD = FacilityDataMViewElements.Name;
                    cd.ZipCodeCD = FacilityDataMDataSet.FullZipCodeList.Where(a => a.DataZC == FacilityDataMViewElements.ZipCode).Select(a => a.IdZC).Single();
                    cd.SettlementCD = FacilityDataMDataSet.FullSettlementList.Where(a => a.DataS == FacilityDataMViewElements.Settlement).Select(a => a.IdS).Single();
                    cd.AddressCD = FacilityDataMViewElements.Address;
                    cd.TAXNumberCD = FacilityDataMViewElements.TaxNumber;
                    if (FacilityDataMViewElements.RegistrationNumber != null) cd.RegistrationNumberCD = FacilityDataMViewElements.RegistrationNumber;
                    if (FacilityDataMViewElements.InvoiceNumber != null) cd.InvoiceNumberCD = FacilityDataMViewElements.InvoiceNumber;
                    if (FacilityDataMViewElements.Phone != null) cd.PhoneCD = FacilityDataMViewElements.Phone;
                    if (FacilityDataMViewElements.Email != null) cd.EmailCD = FacilityDataMViewElements.Email;
                    if (FacilityDataMViewElements.WebPage != null) cd.WebPageCD = FacilityDataMViewElements.WebPage;
                    if (FacilityDataMDataSet.SelectedCompany.ID.Equals(0)) me.companydata.Add(cd);
                    me.SaveChanges();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
            int newValue = 0;
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    newValue = me.companydata.Where(a => a.NameCD == FacilityDataMViewElements.Name).Select(a => a.IdCD).Single();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
                if (FacilityDataMDataSet.SelectedCompany.ID.Equals(0))
                {
                    config.AppSettings.Settings["facilityId"].Value = newValue.ToString();
                    GlobalVM.GlobalM.CompanyId = newValue;
                }
                else
                {
                    config.AppSettings.Settings["facilityId"].Value = FacilityDataMDataSet.SelectedCompany.ID.ToString();
                    GlobalVM.GlobalM.CompanyId = FacilityDataMDataSet.SelectedCompany.ID;
                }
                config.Save(ConfigurationSaveMode.Modified);
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

                if (FacilityDataMDataSet.SelectedCompany.ID.Equals(0))
                {
                    Loading.RunWorkerAsync();
                    temp = "rögzítettük";
                }
                else temp = "módosítottuk";

                dialog = new Dialog(false, "Intézmény adatai", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("Sikeresen " + temp + " az adatokat.");
                dialog.Start();

                FacilityDataMViewElements.AcceptChanges();
            }
            else ConnectionMessage();
        }
        bool fullRefresh;
        protected internal async void Question(bool fullRefreshGiven)
        {
            Action func;
            Action func2;
            if (fullRefreshGiven)
            {
                await Utilities.Loading.Show();
                func = Loading.RunWorkerAsync;
                fullRefresh = fullRefreshGiven;
            }
            else func = Refresh.RunWorkerAsync;
            func2 = async () => await Utilities.Loading.Hide();
            if (VMDirty() && (fullRefreshGiven || (!fullRefreshGiven && !fullRefresh)))
            {
                dialog = new Dialog(true, "El nem menetett változások lehetnek az adott oldalon", func, func2, true);
                dialog.content = new TextBlock("Amennyiben mentés nélkül frissíti az adatokat, az Ön által végrehajtott változtatások nem kerülnek mentésre\n" +
                    "Biztosan frissíti a táblázatot?");
                dialog.Start();
            }
            else func();
            fullRefresh = fullRefreshGiven;
        }
        private async void ZipCodeDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
            {
                int id = FacilityDataMDataSet.FullSettlementList.Where(c => c.DataS == what).Select(c => c.IdS).Single();
                List<int> temp = (FacilityDataMDataSet.SettlementZipSwitch.Where(a => a.IdS == id).Select(a => a.IdZC)).ToList();
                FacilityDataMDataSet.ViewZipCodeList = FacilityDataMDataSet.FullZipCodeList.Where(b => temp.Any(a => a == b.IdZC)).Select(b => b.DataZC).ToList();
            });
        }
        private async void SettlementDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
            {
                int zip = Convert.ToInt32(what);
                int id = FacilityDataMDataSet.FullZipCodeList.Where(c => c.DataZC == zip).Select(c => c.IdZC).Single();
                List<int> temp = (FacilityDataMDataSet.SettlementZipSwitch.Where(a => a.IdZC == id).Select(a => a.IdS)).ToList();
                FacilityDataMDataSet.ViewSettlementList = FacilityDataMDataSet.FullSettlementList.Where(b => temp.Any(a => a == b.IdS)).Select(b => b.DataS).ToList();
            });
        }
        string what;
        protected internal void ItemSourceSearcher(string who, string what)
        {
            if (who == "zipCode")
            {
                if (string.IsNullOrEmpty(what))
                    FacilityDataMDataSet.ViewSettlementList = FacilityDataMDataSet.FullSettlementList.Select(a => a.DataS).ToList();
                else if (what.Equals("false"))
                    FacilityDataMDataSet.ViewSettlementList = null;
                else
                {
                    this.what = what;
                    if (!SettlementSearch.IsBusy) SettlementSearch.RunWorkerAsync();
                }
            }
            else if (who == "settlement")
                if (string.IsNullOrEmpty(what))
                    FacilityDataMDataSet.ViewZipCodeList = FacilityDataMDataSet.FullZipCodeList.Select(a => a.DataZC).ToList();
                else if (what.Equals("false"))
                    FacilityDataMDataSet.ViewZipCodeList = null;
                else
                {
                    this.what = what;
                    if (!SettlementSearch.IsBusy) ZipCodeSearch.RunWorkerAsync();
                }
        }
        protected internal bool ListChecker(string selected, Type type)
        {
            if (type.Equals(typeof(settlement_fx)))
                return FacilityDataMDataSet.FullSettlementList.Any(s => s.DataS == selected);
            else if (type.Equals(typeof(zipcode_fx)))
                return FacilityDataMDataSet.FullZipCodeList.Any(z => z.DataZC == Convert.ToInt32(selected));
            return false;
        }
        protected internal bool VMDirty()
        {
            return FacilityDataMViewElements.IsChanged;
        }
    }
}
