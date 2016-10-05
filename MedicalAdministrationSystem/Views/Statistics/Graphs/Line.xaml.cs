using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class Line : MainChart
    {
        public ObservableCollection<ChartM.Record> Data { get; set; }
        private DateTime? SelectedDate;
        public Line(ObservableCollection<ChartM.Record> Data)
        {
            this.Data = Data;
            this.DataContext = this;
            InitializeComponent();
        }
        private void SetDefaultView(object sender, System.Windows.RoutedEventArgs e)
        {
            AxisX.ActualVisualRange.MinValue = Data[Data.Count - (int)(this.ActualWidth / 100)].Date;
            Diagram.ActualAxisY.ActualWholeRange.MinValue = Data.OrderBy(d => d.Value1).FirstOrDefault().Value1 - 1;
        }

        private void GetArgumentValue(object sender, CustomDrawCrosshairEventArgs e)
        {
            if (e.CrosshairElementGroups.Count > 0) SelectedDate = e.CrosshairElementGroups[0].CrosshairElements[0].SeriesPoint.DateTimeArgument;
            else SelectedDate = null;
        }

        private void SetSelectedValues(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ChartControl).CalcHitInfo(e.GetPosition(sender as ChartControl)).InDiagram) SelectedData((DateTime)SelectedDate);
        }
    }
}
