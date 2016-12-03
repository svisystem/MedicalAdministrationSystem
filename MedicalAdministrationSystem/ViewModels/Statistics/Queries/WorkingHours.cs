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
        public EmployeeQueries(List<int> Members, int Step, DateTime? Start = null, DateTime? Finish = null)
        {
            this.Members = Members;
            this.Step = Step;
            if (Start != null) this.StartTime = Start;
            if (Finish != null) this.FinishTime = Finish;
        }
        protected internal async Task<ObservableCollection<ChartM.Record>> WorkingHours()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    me = new medicalEntities();
                    await me.Database.Connection.OpenAsync();

                    List<object> users = Members.Count == 0 ? users = me.userdata.Select(u =>
                        new { u.IdUD, u.NameUD, me.accountdata.Where(a => a.IdAD == u.AccountDataIdUD).FirstOrDefault().RegistrateTimeAD }).ToList<object>() :
                        me.userdata.Where(u => Members.Any(m => m == u.IdUD)).Select(u =>
                        new { u.IdUD, u.NameUD, me.accountdata.Where(a => a.IdAD == u.AccountDataIdUD).FirstOrDefault().RegistrateTimeAD }).ToList<object>();

                    List<ChartM.Record> collection = new List<ChartM.Record>();
                    List<DateTime> days = new List<DateTime>();

                    foreach (object user in users)
                    {
                        int userId = (int)user.GetType().GetProperty("IdUD").GetValue(user);
                        List<usersschedule> schedule = me.usersschedule.Where(us => us.UserDataIdUS == userId).OrderBy(us => us.WhenCreateUS).ToList();

                        if (schedule.Count != 0)
                        {
                            DateTime regDate = (DateTime)user.GetType().GetProperty("RegistrateTimeAD").GetValue(user);
                            DateTime LocalStart = StartTime == null ? Correction(true, regDate) : Correction(true, (DateTime)StartTime);
                            DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime) :
                                Correction(false, DateTime.Now)) : Correction(false, (DateTime)FinishTime);

                            while (LocalStart.Date < LocalFinish.Date)
                            {
                                int hours = 0;

                                int day = 0;

                                while (day < EndStep(LocalStart) && LocalStart.AddDays(day).Date < ((FinishTime == null) ? DateTime.Now.Date : LocalFinish.Date))
                                {
                                    usersschedule actualDay = schedule.Where(s => s.WhenCreateUS <= LocalStart.AddDays(day) && s.DayOfWeekUS == DayOfWeek(LocalStart.AddDays(day))).OrderByDescending(s => s.WhenCreateUS).FirstOrDefault();
                                    if (actualDay != null)
                                        if (regDate.Date > LocalStart.AddDays(day).Date)
                                        {
                                            if (DayOfWeek(regDate) == actualDay.DayOfWeekUS) hours += (actualDay.FinishTimeUS).Hour - (actualDay.StartTimeUS).Hour;
                                        }
                                        else hours += (actualDay.FinishTimeUS).Hour - (actualDay.StartTimeUS).Hour;
                                    day++;
                                }

                                collection.Add(new ChartM.Record()
                                {
                                    Id = users.IndexOf(user),
                                    Name = user.GetType().GetProperty("NameUD").GetValue(user) as string,
                                    Date = LocalStart,
                                    Value1 = hours
                                });

                                LocalStart = NextStep(LocalStart);
                            }

                            LocalStart = StartTime == null ? Correction(true, regDate) : Correction(true, (DateTime)StartTime);

                            foreach (exceptedschedule es in me.exceptedschedule.Where(ex => ex.UserDataIdES == userId &&
                                ex.StartDateES < LocalFinish && ex.FinishDateED > LocalStart).OrderBy(ex => ex.IncludedES).ToList())
                            {
                                usersschedule tempUser = schedule.Where(sc => sc.DayOfWeekUS == DayOfWeek(es.StartDateES) && (sc.WhenCreateUS <= es.StartDateES ||
                                    sc.WhenCreateUS <= es.FinishDateED)).OrderByDescending(sc => sc.WhenCreateUS).FirstOrDefault();

                                int extendedHours = 0;
                                int currentHours;
                                
                                if (!es.IncludedES)
                                {
                                    if (tempUser != null)
                                    {
                                        days.Add(es.StartDateES);
                                        if (es.StartDateES.Date == es.FinishDateED.Date && LocalStart.Date <= es.StartDateES.Date && LocalFinish.Date > es.StartDateES.Date)
                                        {
                                            extendedHours = (es.StartDateES.Hour <= tempUser.StartTimeUS.Hour ? tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour :
                                                (tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour) - (es.StartDateES.Hour - tempUser.StartTimeUS.Hour)) +
                                                es.FinishDateED.Hour - tempUser.FinishTimeUS.Hour;

                                            currentHours = collection.OrderByDescending(t => t.Date).FirstOrDefault(t => t.Date.Date <= es.StartDateES.Date && t.Id == users.IndexOf(user)).Value1;

                                            collection.OrderByDescending(t => t.Date).FirstOrDefault(t => t.Date.Date <= es.StartDateES.Date && t.Id == users.IndexOf(user)).Value1 =
                                                currentHours >= extendedHours ? currentHours - extendedHours : 0;
                                        }
                                        else
                                        {
                                            if (es.StartDateES.Date < LocalStart.Date)
                                            {
                                                es.StartDateES = LocalStart;
                                                es.StartDateES.AddHours(-es.StartDateES.Hour);
                                            }

                                            bool finishCorrection = false;

                                            if (es.FinishDateED.Date > LocalFinish.Date)
                                            {
                                                finishCorrection = true;
                                                es.FinishDateED = LocalFinish;
                                                es.FinishDateED.AddHours(-es.FinishDateED.Hour);
                                            }

                                            days.Add(es.FinishDateED);

                                            extendedHours += es.StartDateES.Hour <= tempUser.StartTimeUS.Hour ? tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour :
                                                (tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour) - (es.StartDateES.Hour - tempUser.StartTimeUS.Hour);

                                            if (Step == 0 || Step == 1)
                                            {
                                                currentHours = collection.OrderByDescending(t => t.Date).FirstOrDefault(t => Compare(t.Date, es.StartDateES) && t.Id == users.IndexOf(user)).Value1;

                                                collection.OrderByDescending(t => t.Date).FirstOrDefault(t => Compare(t.Date, es.StartDateES) && t.Id == users.IndexOf(user)).Value1 =
                                                    currentHours >= extendedHours ? currentHours - extendedHours : 0;

                                                extendedHours = 0;
                                            }

                                            for (DateTime start = es.StartDateES.AddDays(1); start.Date < es.FinishDateED.Date; start = start.AddDays(1))
                                            {
                                                days.Add(start);
                                                tempUser = null;

                                                tempUser = schedule.Where(sc => sc.DayOfWeekUS == DayOfWeek(start) && sc.WhenCreateUS <= start).
                                                    OrderByDescending(sc => sc.WhenCreateUS).FirstOrDefault();

                                                if (tempUser != null) extendedHours += (tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour);

                                                if (CycleEnd(start))
                                                {
                                                    currentHours = collection.OrderByDescending(t => t.Date).FirstOrDefault(t => Compare(t.Date, start) && t.Id == users.IndexOf(user)).Value1;

                                                    collection.OrderByDescending(t => t.Date).FirstOrDefault(t => Compare(t.Date, start) && t.Id == users.IndexOf(user)).Value1 = 
                                                        currentHours >= extendedHours ? currentHours - extendedHours : 0;

                                                    extendedHours = 0;
                                                }
                                            }

                                            tempUser = null;
                                            tempUser = schedule.Where(sc => sc.DayOfWeekUS == DayOfWeek(es.FinishDateED) && sc.WhenCreateUS <= es.FinishDateED).
                                                OrderByDescending(sc => sc.WhenCreateUS).FirstOrDefault();

                                            if (tempUser != null) extendedHours += finishCorrection == true ? tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour :
                                                (es.FinishDateED.Hour >= tempUser.FinishTimeUS.Hour ? tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour :
                                                (es.FinishDateED.Hour <= tempUser.StartTimeUS.Hour ? 0 : tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour));

                                            currentHours = collection.OrderByDescending(t => t.Date).FirstOrDefault(t => Compare(t.Date, es.FinishDateED) && t.Id == users.IndexOf(user)).Value1;

                                            collection.OrderByDescending(t => t.Date).FirstOrDefault(t => Compare(t.Date, es.FinishDateED) && t.Id == users.IndexOf(user)).Value1 =
                                                currentHours >= extendedHours ? currentHours - extendedHours : 0;
                                        }
                                    }
                                }
                                else //if (es.Included)
                                {
                                    if (!days.Any(d => d.Date == es.StartDateES.Date) && tempUser != null)
                                        extendedHours = ((es.StartDateES.Hour >= tempUser.StartTimeUS.Hour ? 0 : tempUser.StartTimeUS.Hour - es.StartDateES.Hour) +
                                                (es.FinishDateED.Hour <= tempUser.FinishTimeUS.Hour ? 0 : es.FinishDateED.Hour - tempUser.FinishTimeUS.Hour));
                                    else extendedHours = es.FinishDateED.Hour - es.StartDateES.Hour;
                                    collection.Where(t => t.Date.Date <= es.StartDateES.Date).OrderByDescending(t => t.Date).FirstOrDefault().Value1 += extendedHours;
                                }
                            }
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
