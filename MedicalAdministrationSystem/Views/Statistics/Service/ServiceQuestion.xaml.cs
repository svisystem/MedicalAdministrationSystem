using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Service;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Service
{
    public partial class ServiceQuestion : UserControl
    {
        protected internal ServiceQuestionVM ServiceQuestionVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public ServiceQuestion(StatisticsM.Step Item, Action<int> DeleteItems, bool? All)
        {
            Start(Item, DeleteItems, All);
        }
        private async void Start(StatisticsM.Step Item, Action<int> DeleteItems, bool? All)
        {
            await Loading.Show();
            this.DeleteItems = DeleteItems;
            ServiceQuestionVM = new ServiceQuestionVM(Item, All);
            this.DataContext = ServiceQuestionVM;
            InitializeComponent();
        }
        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItems(ServiceQuestionVM.Item.Id);
        }
        protected internal void EnabledChange(bool enabled)
        {
            ServiceQuestionVM.EnabledChange(enabled);
        }
    }
}
