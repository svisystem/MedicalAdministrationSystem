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
            Start();
            this.DeleteItems = DeleteItems;
            FinanceQuestionVM = new FinanceQuestionVM(Item);
            this.DataContext = FinanceQuestionVM;
            InitializeComponent();
        }
        private async void Start()
        {
            await Loading.Show();
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
