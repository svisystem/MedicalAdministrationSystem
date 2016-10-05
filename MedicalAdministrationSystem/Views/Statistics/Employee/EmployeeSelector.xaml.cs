using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Employee;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Employee
{
    public partial class EmployeeSelector : UserControl
    {
        protected internal EmployeeSelectorVM EmployeeSelectorVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public EmployeeSelector(StatisticsM.Step Item, Action<int> DeleteItems)
        {
            Start();
            this.DeleteItems = DeleteItems;
            EmployeeSelectorVM = new EmployeeSelectorVM(Item);
            this.DataContext = EmployeeSelectorVM;
            InitializeComponent();
        }
        private async void Start()
        {
            await Loading.Show();
        }

        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItems(EmployeeSelectorVM.Item.Id);
        }

        private void Next(object sender, System.Windows.RoutedEventArgs e)
        {
            EnabledChange(false);
            EmployeeSelectorVM.Execute();
        }
        protected internal void EnabledChange(bool enabled)
        {
            searchControl.IsEnabled = listBox.IsEnabled = next.IsEnabled = enabled;
        }
    }
}
