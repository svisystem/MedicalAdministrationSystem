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
    public partial class FinanceQueries : QueriesExtender
    {
        protected internal async Task<ObservableCollection<ChartM.Record>> TotalIncome()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    List<ChartM.Record> collection = new List<ChartM.Record>();

                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();

                        List<Income> incomes = me.billing.Join(me.currentpricesforeachbill_st, b => b.IdB, cp => cp.IdB, (b, cp) => new { b.DateTimeB, cp.IdPFS }).
                            Join(me.pricesforeachservice, q => q.IdPFS, pr => pr.IdPFS, (q, pr) => new { q.DateTimeB, pr.PricePFS, pr.VatPFS }).Select(w =>
                            new Income() { Price = w.PricePFS, Vat = w.VatPFS, When = w.DateTimeB }).ToList();

                        DateTime LocalStart = StartTime == null ? Correction(true, incomes.OrderBy(i => i.When).FirstOrDefault().When).Date :
                            Correction(true, (DateTime)StartTime).Date;
                        DateTime LocalFinish = FinishTime == null ? (StartTime != null ? Correction(false, (DateTime)StartTime).Date :
                            Correction(false, DateTime.Now).Date) : Correction(false, (DateTime)FinishTime).Date;
                        FinishTime = LocalFinish;

                        while (LocalStart.Date < ((DateTime)FinishTime).Date)
                        {
                            LocalFinish = NextStep(LocalStart);
                            int tempPrice = 0;
                            int tempVat = 0;
                            foreach (Income income in incomes.Where(i => i.When > LocalStart && i.When < LocalFinish).ToList())
                            {
                                tempPrice += income.Price;
                                tempVat += income.Price * income.Vat / 100;
                            }
                            collection.Add(new ChartM.Record()
                            {
                                Id = 0,
                                Name = "Bruttó ár",
                                Date = LocalStart,
                                Value1 = tempPrice
                            });
                            collection.Add(new ChartM.Record()
                            {
                                Id = 1,
                                Name = "Áfa értéke",
                                Date = LocalStart,
                                Value1 = tempVat
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
        private class Income
        {
            public DateTime When { get; set; }
            public int Price { get; set; }
            public int Vat { get; set; }

        }
    }
}
