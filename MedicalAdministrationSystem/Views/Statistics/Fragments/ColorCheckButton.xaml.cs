using MedicalAdministrationSystem.Models.Statistics;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalAdministrationSystem.Views.Statistics.Fragments
{
    public partial class ColorCheckButton : UserControl
    {
        public Brush Color { get; set; }
        private Action<int, bool> SearchForItem { get; set; }
        public ChartM.Legend Legend { get; set; }
        public ColorCheckButton(ChartM.Legend Legend, Action<int, bool> SearchForItem, Color color)
        {
            InitializeComponent();
            this.Legend = Legend;
            this.SearchForItem = SearchForItem;
            this.Color = new SolidColorBrush(color);
            this.DataContext = this;
        }
        private void MouseIn(object sender, System.Windows.Input.MouseEventArgs e)
        {
            rectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B9B9B9"));
        }

        private void MouseOut(object sender, System.Windows.Input.MouseEventArgs e)
        {
            rectangle.Fill = new SolidColorBrush(Colors.Transparent);
        }

        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            tickMark.Visibility = Legend.Visible ? Visibility.Hidden : Visibility.Visible;
            Legend.Visible = !Legend.Visible;
            SearchForItem(Legend.Id, Legend.Visible);
        }
    }
}
