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

namespace MedicalAdministrationSystem.ViewModels.Statistics.Queries.Employee
{
    partial class WorkingHours : VMExtender
    {
        //Weekly part
        protected internal async Task<ObservableCollection<ChartM.Record>> Week(List<int> members = null, DateTime? Start = null, DateTime? Finish = null)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    me = new medicalEntities();
                    await me.Database.Connection.OpenAsync();

                    List<object> users = members == null ? users = me.userdata.Select(u =>
                        new { u.IdUD, u.NameUD, me.accountdata.Where(a => a.IdAD == u.AccountDataIdUD).FirstOrDefault().RegistrateTimeAD }).ToList<object>() :
                        me.userdata.Where(u => members.Any(m => m == u.IdUD)).Select(u =>
                        new { u.IdUD, u.NameUD, me.accountdata.Where(a => a.IdAD == u.AccountDataIdUD).FirstOrDefault().RegistrateTimeAD }).ToList<object>();

                    List<ChartM.Record> collection = new List<ChartM.Record>();
                    List<DateTime> days = new List<DateTime>();

                    foreach (object user in users)
                    {
                        int userId = (int)user.GetType().GetProperty("IdUD").GetValue(user);
                        List<usersschedule> schedule = me.usersschedule.Where(us => us.UserDataIdUS == userId).OrderBy(us => us.WhenCreateUS).ToList();
                        if (schedule.Count != 0)
                        {
                            DateTime StartDate = schedule.FirstOrDefault().WhenCreateUS.AddDays(-(DayOfWeek(schedule.FirstOrDefault().WhenCreateUS) - 1));

                            while (StartDate.Date < DateTime.Now.Date)
                            {
                                int hours = 0;

                                int i = 1;
                                int nextWeek = 8 - i;
                                while (i < 8 && StartDate.AddDays(i - (8 - nextWeek)).Date < DateTime.Now.Date)
                                {
                                    usersschedule actualDay = schedule.Where(s => s.WhenCreateUS <= StartDate.AddDays(i - 1) && s.DayOfWeekUS == i).OrderByDescending(s => s.WhenCreateUS).FirstOrDefault();
                                    if (actualDay != null) hours += (actualDay.FinishTimeUS).Hour - (actualDay.StartTimeUS).Hour;
                                    i++;
                                }

                                collection.Add(new ChartM.Record()
                                {
                                    Id = userId,
                                    Name = user.GetType().GetProperty("NameUD").GetValue(user) as string,
                                    Date = StartDate,
                                    Value1 = hours
                                });
                                StartDate = StartDate.AddDays(nextWeek);
                            }
                            foreach (exceptedschedule es in me.exceptedschedule.Where(ex => ex.UserDataIdES == userId).OrderBy(ex => ex.IncludedES).ToList())
                            {
                                usersschedule tempUser = schedule.Where(sc => sc.DayOfWeekUS == DayOfWeek(es.StartDateES) && (sc.WhenCreateUS <= es.StartDateES ||
                                    sc.WhenCreateUS <= es.FinishDateED)).OrderByDescending(sc => sc.WhenCreateUS).FirstOrDefault();

                                int extendedHours = 0;


                                if (!es.IncludedES)
                                {
                                    if (tempUser != null)
                                    {
                                        days.Add(es.StartDateES);
                                        if (es.StartDateES.Date == es.FinishDateED.Date)
                                        {

                                            extendedHours = (es.StartDateES.Hour <= tempUser.StartTimeUS.Hour ? tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour :
                                                (tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour) - (es.StartDateES.Hour - tempUser.StartTimeUS.Hour)) +
                                                es.FinishDateED.Hour - tempUser.FinishTimeUS.Hour;
                                            collection.Where(t => t.Date.Date <= es.StartDateES.Date && t.Id == userId).
                                                OrderByDescending(t => t.Date).FirstOrDefault().Value1 -= extendedHours;
                                        }
                                        else
                                        {
                                            days.Add(es.FinishDateED);
                                            extendedHours += es.StartDateES.Hour <= tempUser.StartTimeUS.Hour ? tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour :
                                                (tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour) - (es.StartDateES.Hour - tempUser.StartTimeUS.Hour);
                                            for (DateTime start = es.StartDateES.AddDays(1); start.Date < es.FinishDateED.Date; start = start.AddDays(1))
                                            {
                                                days.Add(start);
                                                tempUser = null;
                                                tempUser = schedule.Where(sc => sc.DayOfWeekUS == DayOfWeek(start) && sc.WhenCreateUS <= start).
                                                    OrderByDescending(sc => sc.WhenCreateUS).FirstOrDefault();
                                                if (tempUser != null) extendedHours += (tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour);
                                                if (DayOfWeek(start) == 7)
                                                {
                                                    collection.Where(t => t.Date <= start.Date && t.Date.AddDays(7) >= start.Date && t.Id == userId).Single().Value1 -= extendedHours;
                                                    extendedHours = 0;
                                                }
                                            }
                                            tempUser = null;
                                            tempUser = schedule.Where(sc => sc.DayOfWeekUS == DayOfWeek(es.FinishDateED) && sc.WhenCreateUS <= es.FinishDateED).
                                                OrderByDescending(sc => sc.WhenCreateUS).FirstOrDefault();
                                            if (tempUser != null) extendedHours += es.FinishDateED.Hour <= tempUser.StartTimeUS.Hour ? (tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour) -
                                                (tempUser.StartTimeUS.Hour - es.FinishDateED.Hour) : tempUser.FinishTimeUS.Hour - tempUser.StartTimeUS.Hour;
                                            collection.Where(t => t.Date <= es.FinishDateED.Date && t.Date.AddDays(7) >= es.FinishDateED.Date && t.Id == userId).Single().Value1 -= extendedHours;
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

                    //fill empty spaces

                    DateTime firstDate = collection.OrderBy(c => c.Date).FirstOrDefault().Date;
                    DateTime lastDate = collection.OrderByDescending(c => c.Date).FirstOrDefault().Date;

                    foreach (object user in users)
                        for (DateTime period = firstDate; period.Date <= lastDate.Date; period = period.AddDays(7))
                            if (!collection.Any(c => c.Date.Date == period.Date && c.Id == (int)user.GetType().GetProperty("IdUD").GetValue(user)))
                                collection.Add(new ChartM.Record()
                                {
                                    Id = (int)user.GetType().GetProperty("IdUD").GetValue(user),
                                    Name = user.GetType().GetProperty("NameUD").GetValue(user) as string,
                                    Date = period,
                                    Value1 = 0
                                });

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
