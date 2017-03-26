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
    public partial class ServiceQueries : QueriesExtender
    {
        public ServiceQueries(List<int> Members, int Step, DateTime? Start = null, DateTime? Finish = null)
        {
            this.Members = Members;
            this.Step = Step;
            if (Start != null) this.StartTime = Start;
            if (Finish != null) this.FinishTime = Finish;
        }
        protected internal async Task<ObservableCollection<ChartM.Record>> ServicesPrices()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

                        List<ServicesForPrice> services = Members.Count == 0 ? me.servicesdata.Select(s =>
                            new ServicesForPrice { Id = s.IdTD, Name = s.NameTD, Deleted = s.DeletedTD }).ToList() :
                            me.servicesdata.Where(s => Members.Any(m => m == s.IdTD)).Select(s =>
                            new ServicesForPrice { Id = s.IdTD, Name = s.NameTD, Deleted = s.DeletedTD }).ToList();

                        DateTime LocalStart = StartTime == null ? Correction(true, me.pricesforeachservice.OrderBy(s => s.WhenChangedPFS).FirstOrDefault().WhenChangedPFS).Date :
                            Correction(true, (DateTime)StartTime).Date;
                        DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                            Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                        FinishTime = LocalFinish;

                        while (LocalStart.Date < ((DateTime)FinishTime).Date)
                        {
                            LocalFinish = NextStep(LocalStart);
                            foreach (ServicesForPrice item in services)
                            {
                                pricesforeachservice temp = me.pricesforeachservice.Where(pr => pr.ServiceDataIdPFS == item.Id && pr.WhenChangedPFS < LocalFinish).
                                    OrderByDescending(pr => pr.WhenChangedPFS).FirstOrDefault();
                                if (temp != null && (item.Deleted != null ? ((DateTime)item.Deleted).Date > LocalStart : true))
                                    collection.Add(new ChartM.Record()
                                    {
                                        Id = services.IndexOf(item),
                                        Name = item.Name,
                                        Date = LocalStart,
                                        Value1 = temp.PricePFS,
                                        Value2 = temp.PricePFS * temp.VatPFS / 100
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
        private class ServicesForPrice
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime? Deleted { get; set; }
        }
    }
}
