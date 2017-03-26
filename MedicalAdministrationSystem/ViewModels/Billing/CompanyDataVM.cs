using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAdministrationSystem.ViewModels.Billing
{
    public class CompanyDataVM : VMExtender
    {
        public CompanyDataM CompanyDataM { get; set; }
        private BackgroundWorker LoadingFixMembers { get; set; }
        private BackgroundWorker LoadingList { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal BackgroundWorker ZipCodeSearch { get; set; }
        protected internal BackgroundWorker SettlementSearch { get; set; }
        private Action firstRow { get; set; }
        public bool Valid { get; set; }
        protected internal CompanyDataVM(Action firstRow)
        {
            this.firstRow = firstRow;
            CompanyDataM = new CompanyDataM();
            ZipCodeSearch = new BackgroundWorker();
            ZipCodeSearch.DoWork += ZipCodeDoWork;
            SettlementSearch = new BackgroundWorker();
            SettlementSearch.DoWork += SettlementDoWork;
            LoadingFixMembers = new BackgroundWorker();
            LoadingFixMembers.DoWork += LoadingFixMembersModel;
            LoadingFixMembers.RunWorkerCompleted += LoadingFixMembersComplete;
            LoadingList = new BackgroundWorker();
            LoadingList.DoWork += LoadingListModel;
            LoadingList.RunWorkerCompleted += LoadingListComplete;
            LoadingFixMembers.RunWorkerAsync();
        }
        private async void LoadingFixMembersModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();

                    CompanyDataM.FullZipCodeList = me.zipcode_fx.ToList();
                    CompanyDataM.FullSettlementList = me.settlement_fx.ToList();
                    CompanyDataM.SettlementZipSwitch = me.settlementzipcode_st.ToList();
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private void LoadingFixMembersComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                CompanyDataM.ViewZipCodeList = CompanyDataM.FullZipCodeList.Select(a => a.DataZC).ToList();
                CompanyDataM.ViewSettlementList = CompanyDataM.FullSettlementList.Select(a => a.DataS).ToList();
                LoadingList.RunWorkerAsync();
            }
            else ConnectionMessage();
        }
        private async void LoadingListModel(object sender, DoWorkEventArgs e)
        {
            CompanyDataM.Companies.Clear();
            CompanyDataM.Erased.Clear();
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    foreach (CompanyDataM.Company item in me.companydata.ToList()
                        .Select(a => new CompanyDataM.Company
                        {
                            ID = a.IdCD,
                            Name = a.NameCD,
                            ZipCodeId = a.ZipCodeCD,
                            ViewZipCode = CompanyDataM.FullZipCodeList.Where(fzcl => fzcl.IdZC == a.ZipCodeCD).FirstOrDefault().DataZC,
                            SettlementId = a.SettlementCD,
                            ViewSettlement = CompanyDataM.FullSettlementList.Where(fsl => fsl.IdS == a.SettlementCD).FirstOrDefault().DataS,
                            Address = a.AddressCD,
                            TaxNumber = a.TAXNumberCD,
                            RegistrationNumber = a.RegistrationNumberCD,
                            InvoiceNumber = a.InvoiceNumberCD,
                            Phone = a.PhoneCD,
                            Email = a.EmailCD,
                            WebPage = a.WebPageCD,
                        }))
                        CompanyDataM.Companies.Add(item);
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void LoadingListComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                foreach (CompanyDataM.Company item in CompanyDataM.Companies) item.AcceptChanges();
                CompanyDataM.SelectedCompany = CompanyDataM.Companies[0];
                CompanyDataM.AcceptChanges();
                modified = false;
                firstRow();

            }
            else ConnectionMessage();

            await Loading.Hide();
        }
        bool refresh;
        protected internal async void Refresh()
        {
            await Loading.Show();
            new FormChecking(() => { refresh = true; LoadingList.RunWorkerAsync(); }, () => { }, true);
        }
        bool modified;
        int count = 0;
        private void NotValidMessage()
        {
            if (count == 0)
            {
                dialog = new Dialog(true, "Nem megfelelő adatok", () => count--);
                dialog.content = new TextBlock("Nem megfelelő adatot tartalmaz az oldal");
                dialog.Start();
                count++;
            }
        }
        protected internal void NewLine(Action lastRow)
        {
            if (Validate())
            {
                CompanyDataM.Companies.Add(new CompanyDataM.Company());
                lastRow();
            }
            else NotValidMessage();
        }
        bool fromInvalidRow;
        protected internal bool Validate()
        {
            if (CompanyDataM.SelectedCompany == null) return true;
            if (eraseable) { eraseable = false; return true; }
            if (refresh) { refresh = false; return true; }
            else
            {
                if (!Valid)
                {
                    fromInvalidRow = true;
                    NotValidMessage();
                }
                else fromInvalidRow = false;
            }
            return Valid;
        }
        protected internal void EraseMethod()
        {
            if (!fromInvalidRow)
            {
                dialog = new Dialog(true, "Cégadat törlése", async () =>
                {
                    await Loading.Show();
                    BackgroundWorker Erase = new BackgroundWorker();
                    Erase.DoWork += new DoWorkEventHandler(EraseDoWork);
                    Erase.RunWorkerCompleted += new RunWorkerCompletedEventHandler(EraseComplete);
                    Erase.RunWorkerAsync();
                }, () => { }, true);
                dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott Céghez tartozó összes adatot?\n" +
                    "A művelet csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
                dialog.Start();
            }
            else fromInvalidRow = false;
        }
        private bool eraseable;
        private async void EraseDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();

                    if (CompanyDataM.SelectedCompany.ID == null) eraseable = true;
                    else eraseable = !(me.billing.Any(b => b.CompanyIdFromB == CompanyDataM.SelectedCompany.ID ||
                            b.CompanyIdToB == CompanyDataM.SelectedCompany.ID) ||
                            me.examinationdata.Any(ex => ex.CompanyIdEX == CompanyDataM.SelectedCompany.ID) ||
                            me.evidencedata.Any(ex => ex.CompanyIdED == CompanyDataM.SelectedCompany.ID) ||
                            me.importedexaminationdata.Any(ex => ex.CompanyIdIEX == CompanyDataM.SelectedCompany.ID) ||
                            me.importedevidencedata.Any(ex => ex.CompanyIdIED == CompanyDataM.SelectedCompany.ID));
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void EraseComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                if (eraseable)
                {
                    if (CompanyDataM.SelectedCompany.ID != null) CompanyDataM.Erased.Add((int)CompanyDataM.SelectedCompany.ID);
                    CompanyDataM.Companies.Remove(CompanyDataM.SelectedCompany);
                    modified = true;

                    await Loading.Hide();
                }
                else
                {
                    dialog = new Dialog(true, "Nem lehet törölni", async () => await Loading.Hide());
                    dialog.content = new TextBlock("A törölni kívánt Cég adatait már használták a rendszerben, ezért annak törlésére nincs mód.");
                    dialog.Start();
                }
            }
            else ConnectionMessage();
        }
        protected internal async void Save()
        {
            await Loading.Show();
            BackgroundWorker Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteComplete);
            Execute.RunWorkerAsync();
        }
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    if (CompanyDataM.Erased.Count != 0)
                    {
                        foreach (int company in CompanyDataM.Erased)
                            try
                            {
                                me.companydata.Remove(me.companydata.Where(a => a.IdCD == company).FirstOrDefault());
                            }
                            catch { }
                        await me.SaveChangesAsync();
                    }
                    foreach (CompanyDataM.Company company in CompanyDataM.Companies)
                    {
                        if (company.ID == null)
                        {
                            me.companydata.Add(new companydata()
                            {
                                NameCD = company.Name,
                                ZipCodeCD = company.ZipCodeId,
                                SettlementCD = company.SettlementId,
                                AddressCD = company.Address,
                                TAXNumberCD = company.TaxNumber,
                                RegistrationNumberCD = company.RegistrationNumber,
                                InvoiceNumberCD = company.InvoiceNumber,
                                PhoneCD = company.Phone,
                                EmailCD = company.Email,
                                WebPageCD = company.WebPage
                            });
                        }
                        else if (company.IsChanged)
                        {
                            companydata cd = me.companydata.Where(c => c.IdCD == company.ID).Single();
                            cd.NameCD = company.Name;
                            cd.ZipCodeCD = company.ZipCodeId;
                            cd.SettlementCD = company.SettlementId;
                            cd.AddressCD = company.Address;
                            cd.TAXNumberCD = company.TaxNumber;
                            cd.RegistrationNumberCD = company.RegistrationNumber;
                            cd.InvoiceNumberCD = company.InvoiceNumber;
                            cd.PhoneCD = company.Phone;
                            cd.EmailCD = company.Email;
                            cd.WebPageCD = company.WebPage;
                        }
                        await me.SaveChangesAsync();
                    }
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                CompanyDataM.Erased.Clear();
                foreach (CompanyDataM.Company row in CompanyDataM.Companies)
                    row.AcceptChanges();
                modified = false;
                dialog = new Dialog(false, "Módosítások mentése", async () => await Loading.Hide());
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        private async void ZipCodeDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
            {
                CompanyDataM.SelectedCompany.SettlementId = CompanyDataM.FullSettlementList.Where(c => c.DataS == settle).Select(c => c.IdS).Single();
                List<int> temp = (CompanyDataM.SettlementZipSwitch.Where(a => a.IdS == CompanyDataM.SelectedCompany.SettlementId).Select(a => a.IdZC)).ToList();
                CompanyDataM.ViewZipCodeList = CompanyDataM.FullZipCodeList.Where(b => temp.Any(a => a == b.IdZC)).Select(b => b.DataZC).ToList();
            });
        }
        private async void SettlementDoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Run(() =>
            {
                CompanyDataM.SelectedCompany.ZipCodeId = CompanyDataM.FullZipCodeList.Where(c => c.DataZC == zip).Select(c => c.IdZC).Single();
                List<int> temp = (CompanyDataM.SettlementZipSwitch.Where(a => a.IdZC == CompanyDataM.SelectedCompany.ZipCodeId).Select(a => a.IdS)).ToList();
                CompanyDataM.ViewSettlementList = CompanyDataM.FullSettlementList.Where(b => temp.Any(a => a == b.IdS)).Select(b => b.DataS).ToList();
            });
        }
        string settle;
        int zip;
        protected internal void ItemSourceSearcher(string who, string what)
        {
            if (who == "zipCode")
            {
                if (string.IsNullOrEmpty(what))
                    CompanyDataM.ViewSettlementList = CompanyDataM.FullSettlementList.Select(a => a.DataS).ToList();
                else if (what.Equals("false"))
                    CompanyDataM.ViewSettlementList = null;
                else
                {
                    this.zip = Convert.ToInt32(what);
                    if (!SettlementSearch.IsBusy) SettlementSearch.RunWorkerAsync();
                }
            }
            else if (who == "settlement")
                if (string.IsNullOrEmpty(what))
                    CompanyDataM.ViewZipCodeList = CompanyDataM.FullZipCodeList.Select(a => a.DataZC).ToList();
                else if (what.Equals("false"))
                    CompanyDataM.ViewZipCodeList = null;
                else
                {
                    this.settle = what;
                    if (!ZipCodeSearch.IsBusy) ZipCodeSearch.RunWorkerAsync();
                }
        }
        protected internal bool ListChecker(string selected, Type type)
        {
            if (type.Equals(typeof(settlement_fx)))
                return CompanyDataM.FullSettlementList.Any(s => s.DataS == selected);
            else if (type.Equals(typeof(zipcode_fx)))
                return CompanyDataM.FullZipCodeList.Any(z => z.DataZC == Convert.ToInt32(selected));
            return false;
        }
        protected internal bool VMDirty()
        {
            return modified ? true : CompanyDataM.Companies.Any(item => item.IsChanged);
        }
    }
}
