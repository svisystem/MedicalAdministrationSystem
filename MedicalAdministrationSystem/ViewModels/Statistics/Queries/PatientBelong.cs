﻿using MedicalAdministrationSystem.DataAccess;
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
        protected internal async Task<ObservableCollection<ChartM.Record>> PatientBelong()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

                        List<object> users = Members.Count == 0 ? users = me.userdata.ToList().Where(u => !me.priviledges.FirstOrDefault(p => p.IdP ==
                            me.accountdata.FirstOrDefault(a => a.IdAD == u.AccountDataIdUD).PriviledgesIdAD).AllSeeP).Select(u =>
                            new { u.IdUD, u.NameUD, me.accountdata.FirstOrDefault(a => a.IdAD == u.AccountDataIdUD).RegistrateTimeAD }).ToList<object>() :

                            me.userdata.Where(u => Members.Any(m => m == u.IdUD)).ToList().Where(w => !me.priviledges.FirstOrDefault(p => p.IdP ==
                            me.accountdata.FirstOrDefault(a => a.IdAD == w.AccountDataIdUD).PriviledgesIdAD).AllSeeP).Select(u =>
                            new { u.IdUD, u.NameUD, me.accountdata.FirstOrDefault(a => a.IdAD == u.AccountDataIdUD).RegistrateTimeAD }).ToList<object>();

                        foreach (object user in users)
                        {
                            int userId = (int)user.GetType().GetProperty("IdUD").GetValue(user);
                            List<belong_st> belong = me.belong_st.Where(b => b.IdUD == userId).ToList();

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
                                        Value1 = belong.Count(b => b.WhenBelongBS.Date < NextStep(LocalStart))
                                    });

                                LocalStart = NextStep(LocalStart);
                            }
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
