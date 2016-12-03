using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels;
using MedicalAdministrationSystem.ViewModels.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Graphs;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics
{
    public partial class Chart : UserControl
    {
        protected internal ChartVM ChartVM { get; set; }
        private bool? continual = null;
        public Chart(ObservableCollection<StatisticsM.Step> Data)
        {
            Start();
            ChartVM = new ChartVM(SetLayout, Data);
            this.DataContext = ChartVM;
            InitializeComponent();
        }
        private async void Start() => await Loading.Show();
        private void SetLayout(Type mainChart, Type assistChart, ObservableCollection<ChartM.Record> Data, ObservableCollection<ChartM.Record> AssistData, DateTime Date, string Step)
        {
            continual = mainChart != null;

            if (mainChart != null)
            {
                CheckType(mainChart, mainContent, Data, Step);

                (mainContent.Content as ChartBase).SelectedData = ChartVM.SelectSingleData;

                CheckType(assistChart, lowerAssistContent, AssistData, Step);

                ChartVM.SetVisualRange = () =>
                {
                    (mainContent.Content as ChartBase).SetVisualRange();
                    (lowerAssistContent.Content as ChartBase).SetVisualRange();
                };
            }
            else
            {
                CheckType(assistChart, mainContent, AssistData, Step);

                lowerRowAssist.Height = GridLength.Auto;
                pieField.Height = new GridLength(1, GridUnitType.Star);
                ChartVM.SetVisualRange = (mainContent.Content as ChartBase).SetVisualRange;
            }

            PieContent.Content = Activator.CreateInstance(typeof(Pie), AssistData);
            ChartVM.SetLegends((PieContent.Content as Pie).ChartControl.Palette);

            ChartVM.SelectSingleData(Date);
        }

        private void CheckType(Type type, ContentControl socket, ObservableCollection<ChartM.Record> Data, string Step)
        {
            if (type == typeof(StackedBar))
                socket.Content = Activator.CreateInstance(type, BindingFlags.CreateInstance | BindingFlags.Public |
                    BindingFlags.Instance | BindingFlags.OptionalParamBinding, null, new object[] { Data, Step }, CultureInfo.CurrentCulture);

            else socket.Content = Activator.CreateInstance(type, Data);
        }
        private void CalculateContainerSizes()
        {
            if (continual != null)
            {
                listBox.MaxWidth = this.ActualWidth / 6;
                PieContent.Width = ((this.ActualWidth / 2) - legend.ActualWidth >= this.ActualHeight / Ratio() - date.ActualHeight - 10) ?
                    this.ActualHeight / Ratio() - date.ActualHeight - 10 : (this.ActualWidth / 2) - legend.ActualWidth;
                if ((bool)continual)
                {
                    PieContent.Height = PieContent.Width;
                    (lowerAssistContent.Content as LowerAssistChart).Model.Width = PieContent.Width + legend.ActualWidth;
                    if (mainContent.Content is StackedBar)
                        (mainContent.Content as LowerAssistChart).Model.Width = this.ActualWidth - (PieContent.Width + legend.ActualWidth);
                }
                else (mainContent.Content as LowerAssistChart).Model.Width = this.ActualWidth - (PieContent.Width + legend.ActualWidth);

            }
        }
        private int Ratio() => (bool)continual ? 2 : 1;
        private void CalculateSize(object sender, SizeChangedEventArgs e) => CalculateContainerSizes();
        protected internal bool Dirty() => false;
        private void listBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (continual != null) CalculateContainerSizes();
        }
        private void NewQuery(object sender, RoutedEventArgs e) => new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.statisticsTBI);
    }
}