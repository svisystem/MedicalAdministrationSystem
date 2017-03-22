using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Finance;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Finance
{
    public partial class FinanceQuestion : UserControl
    {
        protected internal FinanceQuestionVM FinanceQuestionVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public FinanceQuestion(StatisticsM.Step Item, Action<int> DeleteItems)
        {
            Start(Item, DeleteItems);
        }
        private async void Start(StatisticsM.Step Item, Action<int> DeleteItems)
        {
            await Loading.Show();
            this.DeleteItems = DeleteItems;
            FinanceQuestionVM = new FinanceQuestionVM(Item);
            this.DataContext = FinanceQuestionVM;
            InitializeComponent();
        }
        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItems(FinanceQuestionVM.Item.Id);
        }
        protected internal void EnabledChange(bool enabled)
        {
            FinanceQuestionVM.EnabledChange(enabled);
        }
    }
}
