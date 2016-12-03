using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class StackedBar : LowerAssistChart
    {
        public bool Continual { get; set; }
        public string MeasureUnit { get; set; }
        public string SeriesDataMember { get; set; }
        private List<DateTime> VisualRange { get; set; }
        private DateTime? SelectedDate;
        private bool MouseChecker = false;
        public StackedBar(ObservableCollection<ChartM.Record> Data, string Step)
        {
            this.Data = Data;
            this.DataContext = this;
            this.Continual = Data.GroupBy(d => d.Date.Date).Count() > 1;
            SetMembers();
            this.MeasureUnit = Step;
            this.VisualRange = Data.GroupBy(d => d.Date.Date).OrderBy(d => d.Key).Select(d => d.Key).ToList();
            InitializeComponent();
        }
        private void SetMembers()
        {
            Argument = Continual ? "Date" : "Id";
            SeriesDataMember = Continual ? "Id" : "Date";
        }
        private void CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            e.DrawOptions.Color = ChartControl.Palette[(e.SeriesPoint.Tag as ChartM.Record).Id];
        }
        int MinIndex;
        int MaxIndex;
        int Lenght;
        private void SetBarWidth(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (e.WidthChanged) SetVisualRange();
        }
        protected internal override void SetVisualRange()
        {
            if (Data.Count > 0 && LoadDone)
            {
                Lenght = (int)this.ActualWidth / 100;
                MinIndex = VisualRange.IndexOf(VisualRange.Where(d => Compare(AxisX.ActualVisualRange.ActualMinValue, item2: d)).FirstOrDefault());
                MaxIndex = VisualRange.IndexOf(VisualRange.Where(d => Compare(AxisX.ActualVisualRange.ActualMaxValue, item2: d)).FirstOrDefault());

                if (MinIndex + Lenght <= VisualRange.Count - 1)
                {
                    AxisX.ActualVisualRange.MinValue = VisualRange[MinIndex];
                    AxisX.ActualVisualRange.MaxValue = VisualRange[MinIndex + Lenght];
                }
                else if (MaxIndex - Lenght >= 0)
                {
                    AxisX.ActualVisualRange.MaxValue = VisualRange[VisualRange.Count - 1];
                    AxisX.ActualVisualRange.MinValue = VisualRange[MaxIndex - Lenght];
                }
                else
                {
                    AxisX.ActualVisualRange.MinValue = VisualRange[0];
                    AxisX.ActualVisualRange.MaxValue = VisualRange[VisualRange.Count - 1];
                }
            }
        }
        private bool LoadDone { get; set; } = false;
        private void SetView(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDone = true;
            if (VisualRange.Count - (int)(this.ActualWidth / 100) > 0)
                AxisX.ActualVisualRange.MinValue = VisualRange[VisualRange.Count - (int)(this.ActualWidth / 100)].Date;
        }
        private void GetArgumentValue(object sender, CustomDrawCrosshairEventArgs e)
        {
            if (e.CrosshairElementGroups.Count > 0) SelectedDate = e.CrosshairElementGroups[0].CrosshairElements[0].SeriesPoint.DateTimeArgument;
            else SelectedDate = null;
        }

        private void SetSelectedValues(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ChartControl).CalcHitInfo(e.GetPosition(sender as ChartControl)).InDiagram && SelectedDate != null && MouseChecker)
            {
                MouseChecker = false;
                SelectedData((DateTime)SelectedDate);
            }
        }
        private void CheckerLeave(object sender, MouseEventArgs e) => MouseChecker = false;

        private void CheckerDown(object sender, MouseButtonEventArgs e) => MouseChecker = true;
    }
}
