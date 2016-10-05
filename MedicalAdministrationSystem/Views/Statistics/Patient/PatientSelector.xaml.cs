using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Patient;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Patient
{
    public partial class PatientSelector : UserControl
    {
        protected internal PatientSelectorVM PatientSelectorVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public PatientSelector(StatisticsM.Step Item, Action<int> DeleteItems)
        {
            Start();
            this.DeleteItems = DeleteItems;
            PatientSelectorVM = new PatientSelectorVM(Item);
            this.DataContext = PatientSelectorVM;
            InitializeComponent();
        }
        private async void Start()
        {
            await Loading.Show();
        }
        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItems(PatientSelectorVM.Item.Id);
        }

        private void Next(object sender, System.Windows.RoutedEventArgs e)
        {
            EnabledChange(false);
            PatientSelectorVM.Execute();
        }
        protected internal void EnabledChange(bool enabled)
        {
            searchControl.IsEnabled = listBox.IsEnabled = next.IsEnabled = enabled;
        }
    }
}
