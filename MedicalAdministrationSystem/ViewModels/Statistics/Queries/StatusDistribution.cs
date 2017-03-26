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
        public PatientQueries(List<int> Members, int Step, DateTime? Start = null, DateTime? Finish = null)
        {
            this.Members = Members;
            this.Step = Step;
            if (Start != null) this.StartTime = Start;
            if (Finish != null) this.FinishTime = Finish;
        }
        protected internal async Task<ObservableCollection<ChartM.Record>> StatusDistibution()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

                        List<int> patients = Members.Count == 0 ? patients = me.scheduleperson_st.Select(st => st.IdSP).ToList() :
                            me.scheduleperson_st.Where(sc => Members.Any(m => m == sc.ExistedIdSP)).Select(u => u.IdSP).ToList();
                        List<status_fx> statuses = me.status_fx.ToList();

                        DateTime LocalStart = StartTime == null ? Correction(true, me.scheduledata.OrderBy(sc => sc.StartSD).FirstOrDefault().StartSD).Date :
                            Correction(true, (DateTime)StartTime).Date;
                        DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                            Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                        FinishTime = LocalFinish;

                        while (LocalStart.Date < ((DateTime)FinishTime).Date)
                        {
                            LocalFinish = NextStep(LocalStart);
                            List<scheduledata> schedule = me.scheduledata.Where(sc => patients.Contains(sc.PatientIdSD) &&
                                sc.StartSD > LocalStart && sc.FinishSD < LocalFinish).ToList();
                            foreach (status_fx element in statuses)
                                collection.Add(new ChartM.Record()
                                {
                                    Id = statuses.IndexOf(element),
                                    Name = element.DataS,
                                    Date = LocalStart,
                                    Value1 = schedule.Count(sc => sc.StatusSD == element.IdS)
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
