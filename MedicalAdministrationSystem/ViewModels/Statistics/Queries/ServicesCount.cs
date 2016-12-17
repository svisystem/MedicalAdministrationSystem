using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics.Queries
{
    public partial class ServiceQueries : QueriesExtender
    {
        protected internal async Task<ObservableCollection<ChartM.Record>> ServicesCount()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    me = new MedicalModel();
                    await me.Database.Connection.OpenAsync();

                    List<ServicesForCount> services = Members.Count == 0 ? me.servicesdata.Select(s => new ServicesForCount { Id = s.IdTD, Deleted = s.DeletedTD }).ToList():
                        me.servicesdata.Where(s => Members.Any(m => m == s.IdTD)).Select(s => new ServicesForCount { Id = s.IdTD, Deleted = s.DeletedTD }).ToList();

                    foreach (ServicesForCount service in services)
                        service.Created = me.pricesforeachservice.OrderBy(pr => pr.WhenChangedPFS).Where(pr => pr.ServiceDataIdPFS == service.Id).FirstOrDefault().WhenChangedPFS;

                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    DateTime LocalStart = StartTime == null ? Correction(true, services.OrderBy(s => s.Created).FirstOrDefault().Created).Date :
                        Correction(true, (DateTime)StartTime).Date;
                    DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                        Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                    FinishTime = LocalFinish;

                    while (LocalStart.Date < ((DateTime)FinishTime).Date)
                    {
                        LocalFinish = NextStep(LocalStart);

                        collection.Add(new ChartM.Record()
                        {
                            Id = 0,
                            Name = "Szolgáltatások mennyisége",
                            Date = LocalStart,
                            Value1 = services.Count(s => s.Created < LocalFinish && (s.Deleted != null ? s.Deleted > LocalStart : true))
                        });
                        LocalStart = NextStep(LocalStart);
                    }
                    me.Database.Connection.Close();
                    workingConn = true;
                    return new ObservableCollection<ChartM.Record>(collection);
                }
                catch
                {
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
        private class ServicesForCount
        {
            public int Id { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Deleted { get; set; }
        }
    }
}