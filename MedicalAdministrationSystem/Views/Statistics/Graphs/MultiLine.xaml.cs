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
    public partial class MultiLine : ChartBase
    {
        private bool MouseChecker = false;
        public MultiLine(ObservableCollection<ChartM.Record> Data)
        {
            this.Data = Data;
            this.DataContext = this;
            this.VisualRange = Data.GroupBy(d => d.Date.Date).OrderBy(d => d.Key).Select(d => d.Key).ToList();
            InitializeComponent();
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
                LineDiagram.ActualAxisY.ActualWholeRange.MinValue = Data.OrderBy(d => d.Value1).FirstOrDefault().Value1 - 1;
                LineDiagram.ActualAxisY.ActualWholeRange.MaxValue = Data.OrderByDescending(d => d.Value1).FirstOrDefault().Value1 + 1;
                StackedLineDiagram.ActualAxisY.ActualWholeRange.MinValue = Data.OrderBy(d => d.Value2).FirstOrDefault().Value2 - 1;
                StackedLineDiagram.ActualAxisY.ActualWholeRange.MaxValue = Data.OrderByDescending(d => d.Value2).FirstOrDefault().Value2 + 1;
            }
        }

        private void BoundDataChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ChartControl1 != null)
                foreach (XYSeries series in ChartControl1.Diagram.Series)
                    series.Brush = new SolidColorBrush(ChartControl1.Palette[Convert.ToInt32(series.DisplayName)]);
            if (ChartControl2 != null)
                foreach (XYSeries series in ChartControl2.Diagram.Series)
                    series.Brush = new SolidColorBrush(ChartControl2.Palette[Convert.ToInt32(series.DisplayName)]);
        }
        protected internal new void SetDefaultView(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDone = true;
            if (VisualRange.Count - (int)(this.ActualWidth / 100) > 0)
            {
                LineAxisX.ActualVisualRange.MinValue = VisualRange[VisualRange.Count - (int)(this.ActualWidth / 100)];
                StackedLineAxisX.ActualVisualRange.MinValue = VisualRange[VisualRange.Count - (int)(this.ActualWidth / 100)];
            }
            SetVisualRange(true);
        }

        private void Zoom(object sender, XYDiagram2DZoomEventArgs e)
        {
            if ((string)sender.GetType().GetProperty("Name").GetValue(sender) == "LineDiagram")
            {
                StackedLineAxisX.ActualVisualRange.MinValue = e.NewXRange.MinValue;
                StackedLineAxisX.ActualVisualRange.MaxValue = e.NewXRange.MaxValue;
            }
            else
            {
                LineAxisX.ActualVisualRange.MinValue = e.NewXRange.MinValue;
                LineAxisX.ActualVisualRange.MaxValue = e.NewXRange.MaxValue;
            }
        }

        private void Scroll(object sender, XYDiagram2DScrollEventArgs e)
        {
            if ((string)sender.GetType().GetProperty("Name").GetValue(sender) == "LineDiagram")
            {
                StackedLineAxisX.ActualVisualRange.MinValue = e.NewXRange.MinValue;
                StackedLineAxisX.ActualVisualRange.MaxValue = e.NewXRange.MaxValue;
            }
            else
            {
                LineAxisX.ActualVisualRange.MinValue = e.NewXRange.MinValue;
                LineAxisX.ActualVisualRange.MaxValue = e.NewXRange.MaxValue;
            }
        }
    }
}
