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
    public partial class EmployeeQueries : QueriesExtender
    {
        protected internal async Task<ObservableCollection<ChartM.Record>> EvidenceCount()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    me = new MedicalModel();
                    await me.Database.Connection.OpenAsync();

                    List<object> users = Members.Count == 0 ? users = me.userdata.ToList().Where(u => me.priviledges.FirstOrDefault(p => p.IdP ==
                        me.accountdata.FirstOrDefault(a => a.IdAD == u.AccountDataIdUD).PriviledgesIdAD).IsDoctorP).Select(u =>
                        new { u.IdUD, u.NameUD, me.accountdata.FirstOrDefault(a => a.IdAD == u.AccountDataIdUD).RegistrateTimeAD }).ToList<object>() :

                        me.userdata.Where(u => Members.Any(m => m == u.IdUD)).ToList().Where(w => me.priviledges.FirstOrDefault(p => p.IdP ==
                        me.accountdata.FirstOrDefault(a => a.IdAD == w.AccountDataIdUD).PriviledgesIdAD).IsDoctorP).Select(u =>
                        new { u.IdUD, u.NameUD, me.accountdata.FirstOrDefault(a => a.IdAD == u.AccountDataIdUD).RegistrateTimeAD }).ToList<object>();

                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    foreach (object user in users)
                    {
                        int userId = (int)user.GetType().GetProperty("IdUD").GetValue(user);
                        List<evidencedata> evidences = me.evidencedata.Where(ex => ex.UserDataIdED == userId).ToList();

                        DateTime regDate = ((DateTime)user.GetType().GetProperty("RegistrateTimeAD").GetValue(user)).Date;
                        DateTime LocalStart = StartTime == null ? Correction(true, regDate) : Correction(true, (DateTime)StartTime).Date;
                        DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                            Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;

                        while (LocalStart.Date < LocalFinish.Date)
                        {
                            if (regDate.Date < NextStep(LocalStart).Date)
                                collection.Add(new ChartM.Record()
                            {
                                Id = users.IndexOf(user),
                                Name = user.GetType().GetProperty("NameUD").GetValue(user) as string,
                                Date = LocalStart,
                                Value1 = evidences.Count(b => b.DateTimeED.Date < NextStep(LocalStart))
                            });

                            LocalStart = NextStep(LocalStart);
                        }
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
    }
}