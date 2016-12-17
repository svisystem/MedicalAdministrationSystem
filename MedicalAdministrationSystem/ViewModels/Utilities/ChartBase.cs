using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public abstract class ChartBase : UserControl
    {
        public ObservableCollection<ChartM.Record> Data { get; set; }
        protected internal Func<DateTime, Task> SelectedData { get; set; }
        protected internal List<DateTime> VisualRange { get; set; }
        protected internal bool LoadDone { get; set; } = false;
        protected internal DateTime? SelectedDate;
        protected internal abstract void SetVisualRange(bool fresh);
        protected internal AxisX2D AxisXProperty { get; set; }

        protected internal void GetArgumentValue(object sender, CustomDrawCrosshairEventArgs e)
        {
            if (e.CrosshairElementGroups.Count > 0)
            {
                foreach (CrosshairElement element in e.CrosshairElementGroups[0].CrosshairElements)
                    element.LabelElement.Text =
                        Data.Single(d => d.Date.Date == element.SeriesPoint.DateTimeArgument.Date && d.Id ==
                        Convert.ToInt32(element.Series.DisplayName)).Name + ": " + element.SeriesPoint.Value;

                SelectedDate = e.CrosshairElementGroups[0].CrosshairElements[0].SeriesPoint.DateTimeArgument;
                e.CrosshairElementGroups[0].HeaderElement.Text = ((DateTime)SelectedDate).ToString("yyyy. MM. dd.", CultureInfo.CurrentCulture);
            }
            else SelectedDate = null;
        }

        protected internal void SetDefaultView(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadDone = true;
            if (VisualRange.Count - (int)(this.ActualWidth / 100) > 0)
                AxisXProperty.ActualVisualRange.MinValue = VisualRange[VisualRange.Count - (int)(this.ActualWidth / 100)];
            SetVisualRange(true);
        }
    }
}