using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class StackedLine : ChartBase
    {
        public ObservableCollection<ChartM.Record> Data { get; set; }
        private DateTime? SelectedDate;
        private bool MouseChecker = false;
        public StackedLine(ObservableCollection<ChartM.Record> Data)
        {
            this.Data = Data;
            this.DataContext = this;
            InitializeComponent();
        }
        protected internal override void SetVisualRange()
        {
            if (Data.Count != 0)
            {
                Diagram.ActualAxisY.ActualWholeRange.MinValue = Data.OrderBy(d => d.Value1).FirstOrDefault().Value1 - 1;
                Diagram.ActualAxisY.ActualWholeRange.MaxValue = Data.OrderByDescending(d => d.Value1).FirstOrDefault().Value1 + 1;
            }
        }
        private void SetDefaultView(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Data.Count - (int)(this.ActualWidth / 100) > 0)
                AxisX.ActualVisualRange.MinValue = Data[Data.Count - (int)(this.ActualWidth / 100)].Date;
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

        private void CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            e.DrawOptions.Color = ChartControl.Palette[(e.SeriesPoint.Tag as ChartM.Record).Id];
        }

        private void CheckerLeave(object sender, MouseEventArgs e) => MouseChecker = false;

        private void CheckerDown(object sender, MouseButtonEventArgs e) => MouseChecker = true;
    }
}
