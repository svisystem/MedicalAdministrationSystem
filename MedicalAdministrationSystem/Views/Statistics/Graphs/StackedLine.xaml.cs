using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class StackedLine : ChartBase
    {
        private bool MouseChecker = false;
        public StackedLine(ObservableCollection<ChartM.Record> Data)
        {
            this.Data = Data;
            this.DataContext = this;
            this.VisualRange = Data.GroupBy(d => d.Date.Date).OrderBy(d => d.Key).Select(d => d.Key).ToList();
            InitializeComponent();
            AxisXProperty = AxisX;
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
        protected internal override void SetVisualRange(bool fresh)
        {
            if (LoadDone && fresh)
            {
                Diagram.ActualAxisY.ActualWholeRange.MinValue = Data.OrderBy(d => d.Value1).FirstOrDefault().Value1 - 1;
                Diagram.ActualAxisY.ActualWholeRange.MaxValue = Data.OrderByDescending(d => d.Value1).FirstOrDefault().Value1 + 1;
            }
        }

        private void BoundDataChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (XYSeries series in ChartControl.Diagram.Series)
                series.Brush = new SolidColorBrush(ChartControl.Palette[Convert.ToInt32(series.DisplayName)]);
        }
    }
}
