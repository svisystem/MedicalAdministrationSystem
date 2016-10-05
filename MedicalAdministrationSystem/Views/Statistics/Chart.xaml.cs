using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Graphs;
using System;
using System.Collections.ObjectModel;
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
        private void SetLayout(Type mainChart, Type assistChart, ObservableCollection<ChartM.Record> Data, ObservableCollection<ChartM.Record> AssistData, DateTime? Date)
        {
            if (mainChart != null)
            {
                mainContent.Content = Activator.CreateInstance(mainChart, Data);
                (mainContent.Content as MainChart).SelectedData = ChartVM.SelectSingleData;
            }
            else
            {

                lowerRowAssist.Height = GridLength.Auto;
            }
            continual = mainChart != null;
            assistContent.Content = Activator.CreateInstance(assistChart, AssistData);
            ChartVM.SetLegends((assistContent.Content as Pie).PieChart.Palette);
            ChartVM.SelectSingleData((DateTime)Date);
            CalculateContainerSizes();
            //else lowerAssistContent.Content = Activator.CreateInstance(assistChart, SelectedData);
        }

        private void CalculateContainerSizes()
        {
            if (continual != null)
            {
                listBox.MaxWidth = this.ActualWidth / 6;
                assistContent.Width = ((this.ActualWidth / 2) - legend.ActualWidth >= this.ActualHeight / Ratio()) ?
                    this.ActualHeight / Ratio() : (this.ActualWidth / 2) - legend.ActualWidth;
                assistContent.Height = this.ActualHeight / Ratio();
            }
        }
        private int Ratio() => (bool)continual ? 2 : 1;
        private void CalculateSize(object sender, SizeChangedEventArgs e) => CalculateContainerSizes();
        protected internal bool Dirty() => false;
    }
}