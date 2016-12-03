using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using MedicalAdministrationSystem.Views.Statistics.Graphs;
using MedicalAdministrationSystem.ViewModels.Statistics.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAdministrationSystem.ViewModels.Statistics
{
    public class ChartVM : VMExtender
    {
        public ChartM ChartM { get; set; }
        private Action<Type, Type, ObservableCollection<ChartM.Record>, ObservableCollection<ChartM.Record>, DateTime, string> SetLayout { get; set; }
        protected internal Action SetVisualRange { get; set; }
        private ObservableCollection<StatisticsM.Step> GetData { get; set; }
        private DateTime SelectedDate
        {
            get
            {
                return _localSelectedDate;
            }
            set
            {
                ChartM.SelectedDate = value.ToString("yyyy. MMMM d. (dddd)", new CultureInfo("hu-HU"));
                _localSelectedDate = value;
            }
        }
        private DateTime _localSelectedDate;
        private Type Main { get; set; }
        private Type Secondary { get; set; }
        protected internal ChartVM(Action<Type, Type, ObservableCollection<ChartM.Record>, ObservableCollection<ChartM.Record>, DateTime, string> SetLayout,
            ObservableCollection<StatisticsM.Step> GetData)
        {
            ChartM = new ChartM();
            this.SetLayout = SetLayout;
            this.GetData = GetData;
            Start();
        }
        private async void Start()
        {
            await DuplicateQueriedData();
            CreateView(Main, Secondary);
            await Loading.Hide();
        }
        private async Task DuplicateQueriedData()
        {
            ChartM.FullRecords = await SelectQuery();
            foreach (ChartM.Record item in ChartM.FullRecords) ChartM.ViewRecords.Add(item);
        }
        private async Task<ObservableCollection<ChartM.Record>> SelectQuery()
        {
            if ((int)GetData[0].Answer == 0)
            {
                EmployeeQueries employeeQueries = null;
                if ((int)GetData[2].Answer == 0)
                {

                }
                else if ((int)GetData[2].Answer == 1)
                {

                }
                else if ((int)GetData[2].Answer == 2)
                {
                    Main = typeof(StackedBar);
                    Secondary = typeof(Bar);
                    ChartM.MainText = "Adott alkalmazottak mennyi munkaórát dolgoztak";

                    if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                    {
                        employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                            0,
                            (DateTime)((List<object>)GetData[3].Answer)[1]);
                        ChartM.LesserText = "Adott időpontban";
                    }

                    else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                    {
                        employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                            (int)((List<object>)GetData[3].Answer)[1],
                            (DateTime)((List<object>)GetData[3].Answer)[2],
                            (DateTime)((List<object>)GetData[3].Answer)[3]);
                        ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                    }

                    else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                    {
                        employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                            (int)((List<object>)GetData[3].Answer)[1]);
                        ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                    }
                    ChartM.Step = employeeQueries.GetStepInString();
                    return await employeeQueries.WorkingHours();
                }
                else //if ((int)GetData[2].Answer == 3)
                {
                    Main = typeof(StackedBar);
                    Secondary = typeof(Bar);
                    ChartM.MainText = "Adott alkalmazottak mennyi munkaórát dolgoztak";

                    if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                    {
                        employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                            0,
                            (DateTime)((List<object>)GetData[3].Answer)[1]);
                        ChartM.LesserText = "Adott időpontban";
                    }

                    else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                    {
                        employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                            (int)((List<object>)GetData[3].Answer)[1],
                            (DateTime)((List<object>)GetData[3].Answer)[2],
                            (DateTime)((List<object>)GetData[3].Answer)[3]);
                        ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                    }

                    else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                    {
                        employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                            (int)((List<object>)GetData[3].Answer)[1]);
                        ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
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
            return null;
        }
        private void CreateView(Type main, Type secondary)
        {
            SetLayout((ChartM.FullRecords.GroupBy(r => r.Date.Date).Count() > 1 ? main : null), secondary, 
                ChartM.ViewRecords, ChartM.SingleRecord, ChartM.FullRecords.OrderByDescending(r => r.Date).FirstOrDefault().Date.Date, ChartM.Step);
        }

        protected internal void SelectSingleData(DateTime When)
        {
            SelectedDate = When;
            ChartM.SingleRecord.Clear();
            foreach (ChartM.Record item in ChartM.FullRecords.Where(r => r.Date.Date == When.Date).ToList())
                if (ChartM.Legends.Where(l => l.Id == item.Id).Single().Visible)
                    ChartM.SingleRecord.Add(item);
            SetVisualRange();
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
            if (Visible)
            {
                foreach (ChartM.Record item in ChartM.FullRecords.Where(fr => fr.Id == Id).ToList()) ChartM.ViewRecords.Add(item);
                ChartM.SingleRecord.Add(ChartM.ViewRecords.Where(vr => vr.Id == Id && vr.Date.Date == SelectedDate.Date).Single());
            }
            else //if (!Visible)
            {
                ChartM.SingleRecord.Remove(ChartM.ViewRecords.Where(vr => vr.Id == Id && vr.Date.Date == SelectedDate.Date).Single());
                foreach (ChartM.Record item in ChartM.ViewRecords.Where(fr => fr.Id == Id).ToList()) ChartM.ViewRecords.Remove(item);
            }
            SetVisualRange();
        }
        private string StepMeaning(int Step)
        {
            if (Step == 1) return "napi bontásban";
            if (Step == 2) return "heti bontásban";
            if (Step == 3) return "havi bontásban";
            if (Step == 4) return "éves bontásban";
            return null;
        }
    }
}
