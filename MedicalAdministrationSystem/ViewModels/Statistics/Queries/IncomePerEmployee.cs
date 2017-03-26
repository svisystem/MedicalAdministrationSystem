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
        public FinanceQueries(List<int> Members, int Step, DateTime? Start = null, DateTime? Finish = null)
        {
            this.Members = Members;
            this.Step = Step;
            if (Start != null) this.StartTime = Start;
            if (Finish != null) this.FinishTime = Finish;
        }
        protected internal async Task<ObservableCollection<ChartM.Record>> IncomePerEmployee()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

                        DateTime LocalStart = StartTime == null ? Correction(true, me.billing.OrderBy(b => b.DateTimeB).FirstOrDefault().DateTimeB).Date :
                            Correction(true, (DateTime)StartTime).Date;
                        DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                            Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                        FinishTime = LocalFinish;

                        List<Service> services = me.billing.Where(b => b.DateTimeB > LocalStart && b.DateTimeB < FinishTime).
                            Join(me.currentpricesforeachbill_st, b => b.IdB, c => c.IdB, (b, c) => new { c.IdPFS, b.DateTimeB, b.UserIdB }).
                            Join(me.pricesforeachservice, q => q.IdPFS, pr => pr.IdPFS, (q, pr) => new { pr.ServiceDataIdPFS, pr.PricePFS, pr.VatPFS, q.DateTimeB, q.UserIdB }).
                            Select(w => new Service() { Date = w.DateTimeB, Id = w.ServiceDataIdPFS, Price = w.PricePFS, Vat = w.VatPFS, UserId = w.UserIdB }).ToList();

                        List<User> users = Members.Count == 0 ? me.userdata.Select(u => new User() { Id = u.IdUD, Name = u.NameUD }).ToList() :
                        me.userdata.Where(u => Members.Any(m => m == u.IdUD)).Select(u => new User() { Id = u.IdUD, Name = u.NameUD }).ToList();

                        while (LocalStart.Date < ((DateTime)FinishTime).Date)
                        {
                            LocalFinish = NextStep(LocalStart);
                            foreach (User id in users)
                            {
                                List<Service> temp = services.Where(s => s.UserId == id.Id && s.Date.Date > LocalStart && s.Date.Date < LocalFinish).ToList();
                                if (temp.Count != 0)
                                    collection.Add(new ChartM.Record()
                                    {
                                        Id = users.IndexOf(id),
                                        Name = id.Name,
                                        Date = LocalStart,
                                        Value1 = temp.Sum(t => t.Price),
                                        Value2 = temp.Sum(t => t.Price * t.Vat / 100)
                                    });
                                else
                                    collection.Add(new ChartM.Record()
                                    {
                                        Id = users.IndexOf(id),
                                        Name = id.Name,
                                        Date = LocalStart,
                                        Value1 = 0,
                                        Value2 = 0
                                    });
                            }
                            LocalStart = NextStep(LocalStart);
                        }
                    }
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
                return task.Result;
            });
        }

        private class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
