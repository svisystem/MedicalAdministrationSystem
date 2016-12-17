using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class StackedBar : LowerAssistChart
    {
        public bool Continual { get; set; }
        public string MeasureUnit { get; set; }
        private bool MouseChecker = false;
        public StackedBar(ObservableCollection<ChartM.Record> Data, string Step)
        {
            this.Data = Data;
            this.DataContext = this;
            this.Continual = Data.GroupBy(d => d.Date.Date).Count() > 1;
            this.Argument = Continual ? "Date" : "Name";
            this.MeasureUnit = Step;
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
        private void CheckerDown(object sender, MouseButtonEventArgs e) => MouseChecker = Continual;
        private DateTime Nearest(DateTime value) =>
            VisualRange.OrderBy(vr => Greater(vr, value)).FirstOrDefault();
        private TimeSpan Greater(DateTime value1, DateTime value2) =>
            value1 > value2 ? value1 - value2 : value2 - value1;

        int MinIndex;
        int MaxIndex;
        int Lenght;
        protected internal override void SetVisualRange(bool fresh)
        {
            if (Continual && LoadDone && fresh)
            {
                Lenght = (int)this.ActualWidth / 100;

                int LocalMin = VisualRange.IndexOf(VisualRange.FirstOrDefault(d => Compare(
                    Nearest((DateTime)AxisX.ActualVisualRange.ActualMinValue), item2: d)));

                int LocalMax = LocalMin + Lenght >= VisualRange.Count - 1 ? VisualRange.Count - 1 : LocalMin + Lenght;

                if (MinIndex != LocalMin)
                {
                    MinIndex = LocalMin;
                    if (MinIndex + Lenght <= VisualRange.Count - 1)
                        AxisX.ActualVisualRange.MinValue = VisualRange[MinIndex];
                    else if (LocalMax - Lenght >= 0)
                        AxisX.ActualVisualRange.MaxValue = VisualRange[VisualRange.Count - 1];
                    else
                        AxisX.ActualVisualRange.MinValue = VisualRange[0];
                }
                else if (MaxIndex != LocalMax)
                {
                    MaxIndex = LocalMax;
                    if (LocalMin + Lenght <= VisualRange.Count - 1)
                        AxisX.ActualVisualRange.MaxValue = VisualRange[LocalMin + Lenght];
                    else if (MaxIndex - Lenght >= 0)
                        AxisX.ActualVisualRange.MinValue = VisualRange[MaxIndex - Lenght];
                    else
                        AxisX.ActualVisualRange.MaxValue = VisualRange[VisualRange.Count - 1];
                }
            }
        }
        private void BoundDataChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (XYSeries series in ChartControl.Diagram.Series)
                series.Brush = new SolidColorBrush(ChartControl.Palette[Convert.ToInt32(series.DisplayName)]);
        }
        protected internal new void SetDefaultView(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDone = true;
        }
    }
}
