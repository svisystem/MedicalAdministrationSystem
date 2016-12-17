using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Billing
{
    public class CreateBillVM : VMExtender
    {
        public CreateBillM CreateBillM { get; set; }
        private Action Loaded { get; set; }
        private BackgroundWorker Loading { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        protected internal CreateBillVM(Action Loaded)
        {
            this.Loaded = Loaded;
            Start();
        }
        private async void Start()
        {
            CreateBillM = new CreateBillM();
            SelectedPatient = GlobalVM.StockLayout.headerContent.Content as SelectedPatient;
            CreateBillM.Code = await new Codes().Generate((int)GlobalVM.GlobalM.UserID, SelectedPatient.AskId());
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new MedicalModel();
                me.Database.Connection.Open();

                foreach (CreateBillM.Company item in me.companydata.ToList().
                    Select(c => new CreateBillM.Company()
                    {
                        Id = c.IdCD,
                        Name = c.NameCD
                    }))
                {
                    if (item.Id != GlobalVM.GlobalM.CompanyId)
                    {
                        CreateBillM.Companies.Add(item);
                        CreateBillM.CompaniesView.Add(item.Name);
                    }
                }

                foreach (CreateBillM.Service item in me.servicesdata.ToList().
                    Select(s => new CreateBillM.Service()
                    {
                        Id = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == s.IdTD).OrderByDescending(pfs => pfs.IdPFS).FirstOrDefault().IdPFS,
                        Name = s.NameTD,
                        Vat = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == s.IdTD).OrderByDescending(pfs => pfs.IdPFS).FirstOrDefault().VatPFS,
                        Price = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == s.IdTD).OrderByDescending(pfs => pfs.IdPFS).FirstOrDefault().PricePFS
                    }))
                    CreateBillM.Services.Add(item);

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
            if (!workingConn) ConnectionMessage();
            else Loaded();
        }
        protected internal void AddBillingItem()
        {
            if (!CreateBillM.PrintList.Any(p => p.Id == CreateBillM.SelectedService.Id))
            {
                CreateBillM.PrintList.Add(new CreateBillM.PrintItem()
                {
                    Id = CreateBillM.SelectedService.Id,
                    Name = CreateBillM.SelectedService.Name,
                    Quantity = 1,
                    QuantityPrice = CreateBillM.SelectedService.Price - (CreateBillM.SelectedService.Price * CreateBillM.SelectedService.Vat / 100),
                    PriceWithoutVat = CreateBillM.SelectedService.Price - (CreateBillM.SelectedService.Price * CreateBillM.SelectedService.Vat / 100),
                    Vat = CreateBillM.SelectedService.Vat,
                    VatPrice = CreateBillM.SelectedService.Price * CreateBillM.SelectedService.Vat / 100,
                    PriceWithVat = CreateBillM.SelectedService.Price,
                    Price = CreateBillM.SelectedService.Price
                });
                Price();
            }
            else
            {
                dialog = new Dialog(true, "Már szerepel a tétel", () => { });
                dialog.content = new TextBlock("Ezt a tételt már felvettük a listába");
                dialog.Start();
            }
        }
        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            dialog = new Dialog(false, "Megerősítés", () =>
            {
                BackgroundWorker Execute = new BackgroundWorker();
                Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
                Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteComplete);
                Execute.RunWorkerAsync();
            }, async () => await Utilities.Loading.Hide(), true);
            dialog.content = new TextBlock("Biztos elkészíti a számlát?\n\n" +
                "Későbbi módosításra nem lesz már lehetőség");
            dialog.Start();
        }
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            int PriceWithoutVat = 0;
            int Vat = 0;
            int PatientId = SelectedPatient.AskId();

            foreach (CreateBillM.PrintItem item in CreateBillM.PrintList)
            {
                PriceWithoutVat += item.PriceWithoutVat;
                Vat += item.VatPrice;
            }

            try
            {
                me = new MedicalModel();
                me.Database.Connection.Open();

                CreateBillM.CompanyData from = me.companydata.Where(cd => cd.IdCD == GlobalVM.GlobalM.CompanyId).Select(
                    cd => new CreateBillM.CompanyData()
                    {
                        Name = cd.NameCD,
                        ZipCode = me.zipcode_fx.Where(z => z.IdZC == cd.ZipCodeCD).FirstOrDefault().DataZC,
                        Settlement = me.settlement_fx.Where(s => s.IdS == cd.SettlementCD).FirstOrDefault().DataS,
                        Address = cd.AddressCD,
                        TaxNumber = cd.TAXNumberCD,
                        RegistrationNumber = cd.RegistrationNumberCD,
                        InvoiceNumber = cd.InvoiceNumberCD,
                        Phone = cd.PhoneCD,
                        Email = cd.EmailCD,
                        WebPage = cd.WebPageCD
                    }).Single();

                CreateBillM.CompanyData to;
                if (CreateBillM.From)
                    to = me.patientdata.Where(p => p.IdPD == PatientId).Select(
                        p => new CreateBillM.CompanyData()
                        {
                            Name = p.BillingNamePD,
                            ZipCode = me.zipcode_fx.Where(z => z.IdZC == p.BillingZipCodePD).FirstOrDefault().DataZC,
                            Settlement = me.settlement_fx.Where(s => s.IdS == p.BillingSettlementPD).FirstOrDefault().DataS,
                            Address = p.AddressPD
                        }).Single();
                else
                {
                    int Id = CreateBillM.Companies.Where(cs => cs.Name == CreateBillM.SelectedCompany).Single().Id;
                    companydata cd = me.companydata.Where(c => c.IdCD == Id).Single();
                    to = new CreateBillM.CompanyData()
                    {
                        Name = cd.NameCD,
                        ZipCode = me.zipcode_fx.Where(z => z.IdZC == cd.ZipCodeCD).FirstOrDefault().DataZC,
                        Settlement = me.settlement_fx.Where(s => s.IdS == cd.SettlementCD).FirstOrDefault().DataS,
                        Address = cd.AddressCD,
                        TaxNumber = cd.TAXNumberCD,
                        RegistrationNumber = cd.RegistrationNumberCD,
                        InvoiceNumber = cd.InvoiceNumberCD,
                        Phone = cd.PhoneCD,
                        Email = cd.EmailCD,
                        WebPage = cd.WebPageCD
                    };
                }

                billing b = new billing()
                {
                    PatientIdB = PatientId,
                    UserIdB = (int)GlobalVM.GlobalM.UserID,
                    CompanyIdFromB = (int)GlobalVM.GlobalM.CompanyId,
                    CompanyIdToB = !CreateBillM.From ? CreateBillM.Companies.Where(c => c.Name == CreateBillM.SelectedCompany).Single().Id : (int?)null,
                    CodeB = CreateBillM.Code,
                    DateTimeB = DateTime.Now,
                    BillB = new DocumentGenerator().Billing(CreateBillM.Code, from, to, CreateBillM.PrintList, PriceWithoutVat, Vat, CreateBillM.Price).ToArray()
                };

                me.billing.Add(b);

                await me.SaveChangesAsync();

                billId = b.IdB;

                foreach (CreateBillM.PrintItem item in CreateBillM.PrintList)
                    me.currentpricesforeachbill_st.Add(new currentpricesforeachbill_st()
                    {
                        IdB = billId,
                        IdPFS = item.Id
                    });

                await me.SaveChangesAsync();

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        int billId;
        private void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                dialog = new Dialog(false, "Számla", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("Sikeresen elkészítettük a számlát");
                dialog.Start();

                new MenuButtonsEnabled()
                {
                    Id = billId
                }.LoadItem(GlobalVM.StockLayout.billingTBI);
            }
            else ConnectionMessage();
        }
        protected internal void Price()
        {
            CreateBillM.Price = 0;
            if (BillingCount())
            {
                foreach (CreateBillM.PrintItem item in CreateBillM.PrintList)
                    CreateBillM.Price = CreateBillM.Price + item.PriceWithVat;
            }
        }
        protected internal void Erase()
        {
            CreateBillM.PrintList.Remove(CreateBillM.SelectedPrintItem);
        }
        protected internal bool CompanyCheck(string selected)
        {
            return CreateBillM.CompaniesView.Any(c => c == selected);
        }
        protected internal bool BillingCount()
        {
            return CreateBillM.PrintList.Count() == 0 ? false : true;
        }
        protected internal void From(bool from)
        {
            CreateBillM.From = from;
        }
        protected internal void ChangeValue(bool increment)
        {
            if ((!increment && CreateBillM.SelectedPrintItem.Quantity > 1) || increment)
            {
                CreateBillM.SelectedPrintItem.Quantity += increment ? 1 : -1;

                CreateBillM.SelectedPrintItem.PriceWithoutVat = CreateBillM.SelectedPrintItem.QuantityPrice * CreateBillM.SelectedPrintItem.Quantity;
                CreateBillM.SelectedPrintItem.VatPrice = CreateBillM.SelectedPrintItem.Price * CreateBillM.SelectedPrintItem.Vat / 100 * CreateBillM.SelectedPrintItem.Quantity;
                CreateBillM.SelectedPrintItem.PriceWithVat = CreateBillM.SelectedPrintItem.Price * CreateBillM.SelectedPrintItem.Quantity;
            }
            Price();
        }
    }
}
