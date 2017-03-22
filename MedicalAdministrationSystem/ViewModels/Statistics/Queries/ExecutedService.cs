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
        protected internal async Task<ObservableCollection<ChartM.Record>> ExecutedService()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    me = new MedicalModel(ConfigurationManager.Connect());
                    await me.Database.Connection.OpenAsync();

                    List<object> services = Members.Count == 0 ? me.servicesdata.Select(s => new { s.IdTD, s.NameTD }).ToList<object>() :
                        me.servicesdata.Where(s => Members.Any(m => m == s.IdTD)).Select(s => new { s.IdTD, s.NameTD }).ToList<object>();

                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    DateTime LocalStart = StartTime == null ? Correction(true, me.examinationdata.OrderBy(e => e.DateTimeEX).FirstOrDefault().DateTimeEX.Date) :
                        Correction(true, (DateTime)StartTime).Date;
                    DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                        Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                    FinishTime = LocalFinish;

                    while (LocalStart.Date < ((DateTime)FinishTime).Date)
                    {
                        LocalFinish = NextStep(LocalStart);

                        List<int> examinations = me.examinationdata.Where(e => e.DateTimeEX > LocalStart && e.DateTimeEX < LocalFinish).Select(e => e.ServiceIdEX).ToList();

                        foreach (object service in services)
                        {
                            collection.Add(new ChartM.Record()
                            {
                                Id = services.IndexOf(service),
                                Name = (string)service.GetType().GetProperty("NameTD").GetValue(service),
                                Date = LocalStart,
                                Value1 = examinations.Count(e => e == (int)service.GetType().GetProperty("IdTD").GetValue(service))
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
    }
}