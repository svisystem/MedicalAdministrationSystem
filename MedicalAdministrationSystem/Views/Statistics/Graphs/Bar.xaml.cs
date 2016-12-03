using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.ObjectModel;
using System.Linq;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class Bar : LowerAssistChart
    {
        public Bar(ObservableCollection<ChartM.Record> Data)
        {
            this.Data = Data;
            this.DataContext = this;
            this.Argument = "Id";
            InitializeComponent();
        }
        private void CustomDrawSeriesPoint(object sender, DevExpress.Xpf.Charts.CustomDrawSeriesPointEventArgs e)
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
                MinIndex = Data.IndexOf(Data.Where(d => Compare(AxisX.ActualVisualRange.ActualMinValue, d)).FirstOrDefault());
                MaxIndex = Data.IndexOf(Data.Where(d => Compare(AxisX.ActualVisualRange.ActualMaxValue, d)).FirstOrDefault());

                if (MinIndex + Lenght <= Data.Count - 1)
                {
                    AxisX.ActualVisualRange.MinValue = Data[MinIndex].GetType().GetProperty(Argument).GetValue(Data[MinIndex]);
                    AxisX.ActualVisualRange.MaxValue = Data[MinIndex + Lenght].GetType().GetProperty(Argument).GetValue(Data[MinIndex + Lenght]);
                }
                else if (MaxIndex - Lenght >= 0)
                {
                    AxisX.ActualVisualRange.MaxValue = Data[Data.Count - 1].GetType().GetProperty(Argument).GetValue(Data[Data.Count - 1]);
                    AxisX.ActualVisualRange.MinValue = Data[MaxIndex - Lenght].GetType().GetProperty(Argument).GetValue(Data[MaxIndex + Lenght]);
                }
                else
                {
                    AxisX.ActualVisualRange.MinValue = Data[0].GetType().GetProperty(Argument).GetValue(Data[0]);
                    AxisX.ActualVisualRange.MaxValue = Data[Data.Count - 1].GetType().GetProperty(Argument).GetValue(Data[Data.Count - 1]);
                }
            }
        }
        private bool LoadDone { get; set; } = false;
        private void SetView(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDone = true;
            if (Data.Count - (int)(this.ActualWidth / 100) > 0)
                AxisX.ActualVisualRange.MinValue = Data[Data.Count - (int)(this.ActualWidth / 100)].Date;
        }
    }
}