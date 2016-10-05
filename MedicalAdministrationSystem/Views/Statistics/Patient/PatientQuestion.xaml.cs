using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Patient;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Patient
{
    public partial class PatientQuestion : UserControl
    {
        protected internal PatientQuestionVM PatientQuestionVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public PatientQuestion(StatisticsM.Step Item, Action<int> DeleteItems, bool? All)
        {
            Start();
            this.DeleteItems = DeleteItems;
            PatientQuestionVM = new PatientQuestionVM(Item, All);
            this.DataContext = PatientQuestionVM;
            InitializeComponent();
        }
        private async void Start()
        {
            await Loading.Show();
        }
        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteItems(PatientQuestionVM.Item.Id);
        }
        protected internal void EnabledChange(bool enabled)
        {
            PatientQuestionVM.EnabledChange(enabled);
        }
    }
}
