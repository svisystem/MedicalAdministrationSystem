using MedicalAdministrationSystem.Models.Statistics;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public abstract class LowerAssistChart : ChartBase
    {
        public string Argument { get; set; }
        public LowerAssistChartModel Model { get; set; } = new LowerAssistChartModel();
        public ObservableCollection<ChartM.Record> Data { get; set; }

        public class LowerAssistChartModel : NotifyPropertyChanged
        {
            private double _Width;
            public double Width
            {
                get
                {
                    return _Width;
                }
                set
                {
                    if (_Width == value) return;
                    _Width = value;
                    OnPropertyChanged("Width");
                }
            }
        }

        protected internal bool Compare(object value, ChartM.Record item1 = null, DateTime? item2 = null)
        {
            if (item1 != null)
                if (item1.GetType().GetProperty(Argument).PropertyType == typeof(DateTime))
                    return ((DateTime)item1.GetType().GetProperty(Argument).GetValue(item1)).Date == ((DateTime)value).Date;
                else
                    return (int)item1.GetType().GetProperty(Argument).GetValue(item1) == Convert.ToInt32(value);
            else return ((DateTime)item2).Date == ((DateTime)value).Date;
        }
    }
}
