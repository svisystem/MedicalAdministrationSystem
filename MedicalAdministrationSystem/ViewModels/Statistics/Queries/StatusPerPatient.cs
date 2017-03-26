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
        protected internal async Task<ObservableCollection<ChartM.Record>> StatusPerPatient()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

                        List<object> patients = Members.Count == 0 ? me.patientdata.Join(me.scheduleperson_st, p => p.IdPD, sc => sc.ExistedIdSP, (p, sc) =>
                        new { sc.IdSP, p.NamePD, p.CreatedPD }).ToList<object>() :
                        me.scheduleperson_st.Join(me.patientdata.Where(p => Members.Any(m => m == p.IdPD)).
                        Select(p => new { p.IdPD, p.NamePD, p.CreatedPD }),
                        sc => sc.ExistedIdSP, p => p.IdPD, (sc, p) => new { sc.IdSP, p.NamePD, p.CreatedPD }).ToList<object>();
                        List<int> patientsId = patients.Select(p => (int)p.GetType().GetProperty("IdSP").GetValue(p)).ToList();

                        DateTime LocalStart = StartTime == null ? Correction(true, me.scheduledata.OrderBy(sc => sc.StartSD).FirstOrDefault().StartSD).Date :
                            Correction(true, (DateTime)StartTime).Date;
                        DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                            Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                        FinishTime = LocalFinish;

                        while (LocalStart.Date < ((DateTime)FinishTime).Date)
                        {
                            LocalFinish = NextStep(LocalStart);
                            List<scheduledata> schedule = me.scheduledata.Where(sc => patientsId.Contains(sc.PatientIdSD) &&
                                sc.StartSD > LocalStart && sc.FinishSD < LocalFinish).ToList();
                            foreach (object element in patients)
                                if ((DateTime)element.GetType().GetProperty("CreatedPD").GetValue(element) < NextStep(LocalStart))
                                    collection.Add(new ChartM.Record()
                                    {
                                        Id = patients.IndexOf(element),
                                        Name = (string)element.GetType().GetProperty("NamePD").GetValue(element),
                                        Date = LocalStart,
                                        Value1 = schedule.Count(sc => sc.PatientIdSD == (int)element.GetType().GetProperty("IdSP").GetValue(element))
                                    });
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