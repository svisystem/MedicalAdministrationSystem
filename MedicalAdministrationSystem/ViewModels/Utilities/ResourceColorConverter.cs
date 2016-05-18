using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public class ResourceColorConverter : MarkupExtension, IValueConverter
    {
        static ResourceColorConverter instance = new ResourceColorConverter();
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Resource resource = (Resource)value;
            Color color = resource.GetColor();
            
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));

            return brush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}