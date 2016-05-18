using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Billing
{
    public class BillsVM : VMExtender
    {
        public BillsM BillsM { get; set; }
        protected internal BackgroundWorker Loading { get; set; }
        private Action Loaded { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        protected internal BillsVM(Action Loaded)
        {
            this.Loaded = Loaded;
            if (GlobalVM.StockLayout.headerContent.Visibility != System.Windows.Visibility.Collapsed)
                SelectedPatient = GlobalVM.StockLayout.headerContent.Content as SelectedPatient;
            BillsM = new BillsM();
            BillsM.PatientId = SelectedPatient != null ? SelectedPatient.SelectedPatientVM.SelectedPatientM.Id : 0;
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            BillsM.Bills.Clear();
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                if (BillsM.PatientId == 0)
                {
                    foreach (BillsM.Bill bill in me.billing.ToList().Select(b => new BillsM.Bill()
                    {
                        Id = b.IdB,
                        Personal = b.CompanyIdToB == null,
                        DoctorName = me.userdata.Where(u => u.IdUD == b.UserIdB).FirstOrDefault().NameUD,
                        Patient = me.patientdata.Where(p => p.IdPD == b.PatientIdB).FirstOrDefault().NamePD,
                        Code = b.CodeB,
                        BillingName = b.CompanyIdToB == null ?
                        me.patientdata.Where(p => p.IdPD == b.PatientIdB).FirstOrDefault().NamePD :
                        me.companydata.Where(c => c.IdCD == b.CompanyIdToB).FirstOrDefault().NameCD,
                        DateTime = b.DateTimeB
                    }))
                        BillsM.Bills.Add(bill);
                }
                else
                {
                    foreach (BillsM.Bill bill in me.billing.Where(b => b.PatientIdB == BillsM.PatientId).ToList().Select(b => new BillsM.Bill()
                    {
                        Id = b.IdB,
                        Personal = b.CompanyIdToB == null,
                        DoctorName = me.userdata.Where(u => u.IdUD == b.UserIdB).FirstOrDefault().NameUD,
                        Patient = me.patientdata.Where(p => p.IdPD == b.PatientIdB).FirstOrDefault().NamePD,
                        Code = b.CodeB,
                        BillingName = b.CompanyIdToB == null ?
                        me.patientdata.Where(p => p.IdPD == b.PatientIdB).FirstOrDefault().NamePD :
                        me.companydata.Where(c => c.IdCD == b.CompanyIdToB).FirstOrDefault().NameCD,
                        DateTime = b.DateTimeB
                    }))
                        BillsM.Bills.Add(bill);
                }
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
            if (workingConn) Loaded();
            else ConnectionMessage();
        }
        protected internal void View()
        {
            new MenuButtonsEnabled()
            {
                ID = BillsM.SelectedBill.Id
            }.LoadItem(GlobalVM.StockLayout.billingTBI);
        }
    }
}
