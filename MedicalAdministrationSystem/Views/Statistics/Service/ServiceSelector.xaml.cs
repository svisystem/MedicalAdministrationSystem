using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Service;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Service
{
    public partial class ServiceSelector : UserControl
    {
        protected internal ServiceSelectorVM ServiceSelectorVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public ServiceSelector(StatisticsM.Step Item, Action<int> DeleteItems)
        {
            Start();
            this.DeleteItems = DeleteItems;
            ServiceSelectorVM = new ServiceSelectorVM(Item);
            this.DataContext = ServiceSelectorVM;
            InitializeComponent();
        }
        private async void Start()
        {
            await Loading.Show();
        }
        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItems(ServiceSelectorVM.Item.Id);
        }

        private void Next(object sender, System.Windows.RoutedEventArgs e)
        {
            EnabledChange(false);
            ServiceSelectorVM.Execute();
        }
        protected internal void EnabledChange(bool enabled)
        {
            searchControl.IsEnabled = listBox.IsEnabled = next.IsEnabled = enabled;
        }
    }
}
