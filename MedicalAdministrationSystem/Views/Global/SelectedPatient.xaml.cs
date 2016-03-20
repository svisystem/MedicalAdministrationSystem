using MedicalAdministrationSystem.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Global
{
    public partial class SelectedPatient : UserControl
    {
        protected internal SelectedPatientVM SelectedPatientVM { get; set; }
        public SelectedPatient()
        {
            SelectedPatientVM = new SelectedPatientVM();
            this.DataContext = SelectedPatientVM;
            InitializeComponent();
        }
        protected internal void Load(int id, string name)
        {
            SelectedPatientVM.Loaded(id, name);
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            SelectedPatientVM.Question();
        }
        private void Modify(object sender, RoutedEventArgs e)
        {
            SelectedPatientVM.PatientDetailsModify();
        }
        protected internal int AskId()
        {
            return SelectedPatientVM.Id();
        }
        protected internal void Dispose()
        {
            SelectedPatientVM.Dispose();
        }
    }
}
