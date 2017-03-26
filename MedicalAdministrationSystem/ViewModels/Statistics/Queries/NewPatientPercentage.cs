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
    public partial class PatientQueries : QueriesExtender
    {
        protected internal async Task<ObservableCollection<ChartM.Record>> NewPatientPercentage()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

                        List<DateTime> patients = Members.Count == 0 ? me.patientdata.Select(p => p.CreatedPD).ToList() :
                        me.patientdata.Where(p => Members.Any(m => m == p.IdPD)).Select(p => p.CreatedPD).ToList();

                        DateTime LocalStart = StartTime == null ? Correction(true, patients.OrderBy(p => p).FirstOrDefault().Date) :
                            Correction(true, (DateTime)StartTime).Date;
                        DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                            Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                        FinishTime = LocalFinish;

                        int counter = 0;

                        while (LocalStart.Date < ((DateTime)FinishTime).Date)
                        {
                            LocalFinish = NextStep(LocalStart);
                            if (patients.Any(p => p < LocalFinish))
                            {
                                collection.Add(new ChartM.Record()
                                {
                                    Id = 0,
                                    Name = "Meglévő",
                                    Date = LocalStart,
                                    Value1 = counter
                                });
                                int newPatientsCount = patients.Count(p => p < LocalFinish.Date && p > LocalStart.Date);
                                counter += newPatientsCount;
                                collection.Add(new ChartM.Record()
                                {
                                    Id = 1,
                                    Name = "Új",
                                    Date = LocalStart,
                                    Value1 = newPatientsCount
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
    }
}