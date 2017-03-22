using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics.Queries
{
    public partial class FinanceQueries : QueriesExtender
    {
        protected internal async Task<ObservableCollection<ChartM.Record>> IncomePerService()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    me = new MedicalModel(ConfigurationManager.Connect());
                    await me.Database.Connection.OpenAsync();

                    DateTime LocalStart = StartTime == null ? Correction(true, me.billing.OrderBy(b => b.DateTimeB).FirstOrDefault().DateTimeB).Date :
                        Correction(true, (DateTime)StartTime).Date;
                    DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                        Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                    FinishTime = LocalFinish;

                    List<Service> IdList = Members.Count == 0 ? me.servicesdata.Select(sd => new Service() { Id = sd.IdTD, Name = sd.NameTD }).ToList() :
                    me.servicesdata.Where(sd => Members.Any(m => m == sd.IdTD)).Select(sd => new Service() { Id = sd.IdTD, Name = sd.NameTD }).ToList();

                    List<Service> services = me.billing.Where(b => b.DateTimeB > LocalStart && b.DateTimeB < FinishTime).
                        Join(me.currentpricesforeachbill_st, b => b.IdB, c => c.IdB, (b, c) => new { c.IdPFS, b.DateTimeB }).
                        Join(me.pricesforeachservice, q => q.IdPFS, pr => pr.IdPFS, (q, pr) => new { pr.ServiceDataIdPFS, pr.PricePFS, pr.VatPFS, q.DateTimeB }).
                        Select(w => new Service() { Date = w.DateTimeB, Id = w.ServiceDataIdPFS, Price = w.PricePFS, Vat = w.VatPFS }).ToList();

                    for (int i = 0; i < services.Count; i++)
                        if (IdList.Any(id => id.Id == services[i].Id))
                            services[i].Name = IdList.Single(id => id.Id == services[i].Id).Name;
                        else
                        {
                            services.Remove(services[i]);
                            i--;
                        }

                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    while (LocalStart.Date < ((DateTime)FinishTime).Date)
                    {
                        LocalFinish = NextStep(LocalStart);
                        foreach (Service id in IdList)
                        {
                            List<Service> temp = services.Where(s => s.Id == id.Id && s.Date.Date > LocalStart && s.Date.Date < LocalFinish).ToList();
                            if (temp.Count != 0)
                                collection.Add(new ChartM.Record()
                                {
                                    Id = IdList.IndexOf(id),
                                    Name = temp[0].Name,
                                    Date = LocalStart,
                                    Value1 = temp[0].Price * temp.Count,
                                    Value2 = temp[0].Price * temp[0].Vat / 100 * temp.Count
                                });
                            else
                                collection.Add(new ChartM.Record()
                                {
                                    Id = IdList.IndexOf(id),
                                    Name = services.FirstOrDefault(s => s.Id == id.Id).Name,
                                    Date = LocalStart,
                                    Value1 = 0,
                                    Value2 = 0
                                });
                        }
                        LocalStart = NextStep(LocalStart);
                    }
                    me.Database.Connection.Close();
                    workingConn = true;
                    return new ObservableCollection<ChartM.Record>(collection);
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
                else return task.Result;
                return null;
            });
        }
        private class Service
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            public int Vat { get; set; }
            public int? UserId { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
