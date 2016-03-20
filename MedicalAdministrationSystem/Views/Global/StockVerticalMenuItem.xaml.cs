using MedicalAdministrationSystem.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalAdministrationSystem.Views.Global
{
    public partial class StockVerticalMenuItem : UserControl
    {
        public StockVerticalMenuItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        protected internal void button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GlobalVM.StockLayout.verticalMenu.Children.Count; i++)
            {
                var svmi = (StockVerticalMenuItem)GlobalVM.StockLayout.verticalMenu.Children[i];
                if (this == GlobalVM.StockLayout.verticalMenu.Children[i])
                {
                    svmi.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));
                    svmi.button.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    svmi.Background = new SolidColorBrush(Colors.Transparent);
                    if (svmi.button.IsEnabled) svmi.button.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }
        public bool IsEnabledTrigger
        {
            get { return button.IsEnabled; }
            set
            {
                if (!value) button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));
                button.IsEnabled = value;
            }
        }
    }
}
