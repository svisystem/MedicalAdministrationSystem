using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Employee;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Employee
{

    public partial class EmployeeQuestion : UserControl
    {
        protected internal EmployeeQuestionVM EmployeeQuestionVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public EmployeeQuestion(StatisticsM.Step Item, Action<int> DeleteItems, bool? All)
        {
            Start(Item, DeleteItems, All);
        }
        private async void Start(StatisticsM.Step Item, Action<int> DeleteItems, bool? All)
        {
            await Loading.Show();
            this.DeleteItems = DeleteItems;
            EmployeeQuestionVM = new EmployeeQuestionVM(Item, All);
            this.DataContext = EmployeeQuestionVM;
            InitializeComponent();
        }
        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItems(EmployeeQuestionVM.Item.Id);
        }
        protected internal void EnabledChange(bool enabled)
        {
            EmployeeQuestionVM.EnabledChange(enabled);
        }
    }
}
