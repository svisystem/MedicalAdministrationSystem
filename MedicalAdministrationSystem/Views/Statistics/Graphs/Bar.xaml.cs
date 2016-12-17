using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class Bar : LowerAssistChart
    {
        private ObservableCollection<ChartM.Legend> Legends { get; set; }
        public Bar(ObservableCollection<ChartM.Record> Data, ObservableCollection<ChartM.Legend> Legends)
        {
            this.Data = Data;
            this.DataContext = this;
            this.Argument = "Id";
            this.Legends = Legends;
            InitializeComponent();
            AxisXProperty = AxisX;
        }
        int MinIndex;
        int MaxIndex;
        int Lenght;
        protected internal override void SetVisualRange(bool fresh)
        {
            if (LoadDone && fresh)
            {
                Lenght = (int)this.ActualWidth / 100;

                if (Lenght < Legends.Count)
                {
                    int LocalMin = Legends.IndexOf(Legends.Single(l => Compare(AxisX.ActualVisualRange.ActualMinValue, l)));
                    int LocalMax = LocalMin + Lenght >= Legends.Count - 1 ? Legends.Count - 1 : LocalMin + Lenght;

                    if (MinIndex != LocalMin)
                    {
                        MinIndex = LocalMin;
                        if (MinIndex + Lenght <= Legends.Count - 1)
                            AxisX.ActualVisualRange.MinValue = Legends[MinIndex].Id;
                        else if (LocalMax - Lenght >= 0)
                            AxisX.ActualVisualRange.MaxValue = Legends[Legends.Count - 1].Id;
                        else
                            AxisX.ActualVisualRange.MinValue = Legends[0].Id;
                    }
                    else if (MaxIndex != LocalMax)
                    {
                        MaxIndex = LocalMax;
                        if (LocalMin + Lenght <= Legends.Count - 1)
                            AxisX.ActualVisualRange.MaxValue = Legends[LocalMin + Lenght].Id;
                        else if (MaxIndex - Lenght >= 0)
                            AxisX.ActualVisualRange.MinValue = Legends[MaxIndex - Lenght].Id;
                        else
                            AxisX.ActualVisualRange.MaxValue = Legends[Legends.Count - 1].Id;
                    }
                }
                else
                {
                    AxisX.ActualVisualRange.MinValue = Legends.OrderBy(d => d.Id).FirstOrDefault().Id;
                    AxisX.ActualVisualRange.MaxValue = Legends.OrderByDescending(d => d.Id).FirstOrDefault().Id;
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