using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels;
using MedicalAdministrationSystem.ViewModels.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Graphs;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics
{
    public partial class Chart : UserControl
    {
        protected internal ChartVM ChartVM { get; set; }
        private bool? continual = null;
        public Chart(ObservableCollection<StatisticsM.Step> Steps)
        {
            Start(Steps);
        }
        private async void Start(ObservableCollection<StatisticsM.Step> Steps)
        {
            await Loading.Show();
            ChartVM = new ChartVM(SetLayout, Steps);
            this.DataContext = ChartVM;
            InitializeComponent();
        }
        private async Task SetLayout(ObservableCollection<ChartM.Record> Data, ObservableCollection<ChartM.Record> AssistData,
            DateTime Date, string Step, ObservableCollection<ChartM.Legend> Legends)
        {
            await Task.Factory.StartNew(() =>
            {
                if (ChartVM.Continual())
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        CheckType(ChartVM.Main, mainContent, Data, Step, Legends);
                        (mainContent.Content as ChartBase).SelectedData = ChartVM.SelectSingleData;
                        CheckType(ChartVM.Secondary, lowerAssistContent, AssistData, Step, Legends);

                        ChartVM.SetVisualRange = (bool? main, bool? secondary) =>
                        {
                            (mainContent.Content as ChartBase).SetVisualRange(main == null ? ChartVM.TypeCheck(ChartVM.Main, typeof(StackedBar)) :
                                main == true ? ChartVM.TypeCheck(ChartVM.Main, typeof(StackedBar)) : false);

                            (lowerAssistContent.Content as ChartBase).SetVisualRange(secondary == null ?
                                ChartVM.TypeCheck(ChartVM.Secondary, typeof(Bar)) : secondary == true ? true : (bool)main);
                        };
                    }));
                else
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        CheckType(ChartVM.Secondary, mainContent, AssistData, Step, Legends);

                        lowerRowAssist.Height = GridLength.Auto;
                        pieField.Height = new GridLength(1, GridUnitType.Star);
                        ChartVM.SetVisualRange = (bool? main, bool? secondary) => (mainContent.Content as ChartBase).SetVisualRange(secondary == null ?
                                ChartVM.TypeCheck(ChartVM.Secondary, typeof(Bar)) : secondary == true ? true : (bool)main);
                    }));

                this.Dispatcher.Invoke(new Action(() => CheckType(typeof(Pie), mainContent, AssistData, Step, Legends)));
            }, CancellationToken.None).ContinueWith(async task =>
            {
                continual = ChartVM.Continual();
                await ChartVM.SetLegends(Date);
            });
        }
        private void CheckType(Type type, ContentControl socket, ObservableCollection<ChartM.Record> Data, string Step, ObservableCollection<ChartM.Legend> Legends)
        {
            if (ChartVM.TypeCheck(type, typeof(StackedBar)))
                socket.Content = Activator.CreateInstance(type, new object[] { Data, Step });
            else if (ChartVM.TypeCheck(type, typeof(Bar)))
                socket.Content = Activator.CreateInstance(type, new object[] { Data, Legends });
            else if (ChartVM.TypeCheck(type, typeof(Pie)))
                PieContent.Content = Activator.CreateInstance(type, new object[] { Data, ChartVM.TypeCheck(ChartVM.Main, typeof(MultiLine)) });
            else socket.Content = Activator.CreateInstance(type, Data);
        }
        private void CalculateContainerSizes()
        {
            if (continual != null)
            {
                listBox.MaxWidth = assistContent.ActualWidth / 3;
                PieContent.Width = ((this.ActualWidth / 2) - legend.ActualWidth >= this.ActualHeight / Ratio() - date.ActualHeight - 10) ?
                    this.ActualHeight / Ratio() - date.ActualHeight - 10 : (this.ActualWidth / 2) - legend.ActualWidth;
                if ((bool)continual)
                {
                    PieContent.Height = PieContent.Width;
                    (lowerAssistContent.Content as LowerAssistChart).Model.Width = PieContent.Width + legend.ActualWidth;
                    if (ChartVM.TypeCheck(ChartVM.Main, typeof(StackedBar)))
                        (mainContent.Content as LowerAssistChart).Model.Width = this.ActualWidth - (PieContent.Width + legend.ActualWidth);
                }
                else (mainContent.Content as LowerAssistChart).Model.Width = this.ActualWidth - (PieContent.Width + legend.ActualWidth);
            }
            if (ChartVM.SetVisualRange != null) ChartVM.SetVisualRange(null, true);
        }
        private int Ratio() => (bool)continual ? 2 : 1;
        private void CalculateSize(object sender, SizeChangedEventArgs e) => CalculateContainerSizes();
        protected internal bool Dirty() => false;
        private async void NewQuery(object sender, RoutedEventArgs e) => await new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.statisticsTBI);
        private void listBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged) CalculateContainerSizes();
        }
    }
}