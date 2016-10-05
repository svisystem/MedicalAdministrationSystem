using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using MedicalAdministrationSystem.Views.Statistics.Graphs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Statistics
{
    public class ChartVM : VMExtender
    {
        public ChartM ChartM { get; set; }
        private Action<Type, Type, ObservableCollection<ChartM.Record>, ObservableCollection<ChartM.Record>, DateTime?> SetLayout { get; set; }
        private ObservableCollection<StatisticsM.Step> GetData { get; set; }
        protected internal ChartVM(Action<Type, Type, ObservableCollection<ChartM.Record>, ObservableCollection<ChartM.Record>, DateTime?> SetLayout, ObservableCollection<StatisticsM.Step> GetData)
        {
            ChartM = new ChartM();
            this.SetLayout = SetLayout;
            this.GetData = GetData;
            Start();
        }
        private async void Start()
        {
            if (GetData[0].Answer as int? == 0)
            {
                if (GetData[2].Answer as int? == 0)
                {

                }
                else if (GetData[2].Answer as int? == 1)
                {

                }
                else if (GetData[2].Answer as int? == 2)
                {

                }
                else //if (GetData[2].Answer as int? == 3)
                {
                    if ((GetData[3].Answer as List<object>)[0] as int? == 0)
                    {

                    }
                    else if ((GetData[3].Answer as List<object>)[0] as int? == 1)
                    {

                    }
                    else //if ((GetData[3].Answer as List<object>)[0] as int? == 2)
                    {

                        if ((GetData[3].Answer as List<object>)[1] as int? == 0)
                        {
                            if (GetData[1].Answer.GetType() == typeof(int))
                            {

                            }
                            else
                            {

                            }
                        }
                        else if ((GetData[3].Answer as List<object>)[1] as int? == 1)
                        {
                            if (GetData[1].Answer.GetType() == typeof(int))
                            {
                                ChartM.FullRecords = await new Queries.Employee.WorkingHours().Week();
                                CreateView(typeof(Pie), typeof(Line));
                            }
                            else //if (GetData[1].Answer.GetType() == typeof(List<int>))
                            {
                                ChartM.FullRecords = await new Queries.Employee.WorkingHours().Week((List<int>)GetData[1].Answer);
                                CreateView(typeof(Pie), typeof(Line));
                            }
                        }
                        else if ((GetData[3].Answer as List<object>)[1] as int? == 2)
                        {

                        }
                        else //if ((GetData[3].Answer as List<object>)[1] as int? == 3)
                        {

                        }
                    }
                }
            }
            else if (GetData[0].Answer as int? == 1)
            {

            }
            else if (GetData[0].Answer as int? == 2)
            {

            }
            else //if (GetData[0].Answer as int? == 3)
            {

            }
            await Loading.Hide();
        }
        private void CreateView(Type secondary, Type main)
        {
            if (ChartM.FullRecords.GroupBy(r => r.Date.Date).Count() > 1)
                SetLayout(main, secondary, ChartM.FullRecords, ChartM.SingleRecord, ChartM.FullRecords.OrderByDescending(r => r.Date).FirstOrDefault().Date.Date);
            else
                SetLayout(null, secondary, ChartM.FullRecords, ChartM.SingleRecord, null);
        }

        protected internal void SelectSingleData(DateTime When)
        {
            ChartM.SingleRecord.Clear();
            foreach (ChartM.Record item in new List<ChartM.Record>(ChartM.FullRecords.Where(r => r.Date.Date == When.Date)))
                if (ChartM.Legends.Where(l => l.Id == item.Id).Single().Visible) ChartM.SingleRecord.Add(item);
        }

        protected internal void SetLegends(DevExpress.Xpf.Charts.Palette Palette)
        {
            foreach (int item in ChartM.FullRecords.GroupBy(r => r.Id).Select(r => r.Key).ToList())
            {
                ChartM.Legends.Add(new ChartM.Legend()
                {
                    Id = item,
                    Name = ChartM.FullRecords.Where(f => f.Id == item).FirstOrDefault().Name,
                    Visible = true
                });
                ChartM.LegendsContainer.Add(new ColorCheckButton(ChartM.Legends.Last(), SearchForItem, Palette[ChartM.LegendsContainer.Count]));
            }
        }
        private void SearchForItem(int Id, bool Visible)
        {
            //TODO
        }
        private void WorkingHoursWeek(DateTime Start)
        {

        }
        private void WorkingHoursWeek(DateTime Start, DateTime End)
        {

        }
    }
}
