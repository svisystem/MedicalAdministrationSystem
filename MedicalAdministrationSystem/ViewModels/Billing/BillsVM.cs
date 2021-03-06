﻿using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Billing
{
    public class BillsVM : VMExtender
    {
        public BillsM BillsM { get; set; }
        private Action Loaded { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        protected internal BillsVM(Action Loaded)
        {
            this.Loaded = Loaded;
            if (GlobalVM.StockLayout.headerContent.Visibility != Visibility.Collapsed)
                SelectedPatient = GlobalVM.StockLayout.headerContent.Content as SelectedPatient;
            BillsM = new BillsM();
            BillsM.PatientId = SelectedPatient != null ? SelectedPatient.SelectedPatientVM.SelectedPatientM.Id : 0;
            Start();
        }
        protected internal async void Start()
        {
            await Utilities.Loading.Show();
            BillsM.Bills = await LoadDataSet();
            await Utilities.Loading.Hide();
        }
        private async Task<ObservableCollection<BillsM.Bill>> LoadDataSet()
        {
            return await Task.Run(async () =>
            {
                ObservableCollection<BillsM.Bill> collection = new ObservableCollection<BillsM.Bill>();
                try
                {
                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

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
                                collection.Add(bill);
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
                                collection.Add(bill);
                        }
                    }
                    workingConn = true;
                    return collection;
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                    return null;
                }
            }, CancellationToken.None).ContinueWith(task =>
            {
                if (!workingConn) Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => ConnectionMessage()));
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => Loaded()));
                    return task.Result;
                }
                return null;
            });
        }
        protected internal async void View()
        {
            await new MenuButtonsEnabled()
            {
                Id = BillsM.SelectedBill.Id
            }.LoadItem(GlobalVM.StockLayout.billingTBI);
        }
    }
}
