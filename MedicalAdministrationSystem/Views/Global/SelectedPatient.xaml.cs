using MedicalAdministrationSystem.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Global
{
    public partial class SelectedPatient : UserControl
    {
        protected internal SelectedPatientVM SelectedPatientVM { get; set; }
        public SelectedPatient(Action Selected)
        {
            SelectedPatientVM = new SelectedPatientVM(Selected, () => associatedExaminations.Focus());
            this.DataContext = SelectedPatientVM;
            InitializeComponent();
        }
        protected internal void Load(int id, string name)
        {
            SelectedPatientVM.Loaded(id, name);
        }
        protected internal async void Refresh(int Id, string Name)
        {
            if (AskId() == Id)
                await this.Dispatcher.BeginInvoke(new Action(() =>
                    SelectedPatientVM.Refresh(Name)), DispatcherPriority.Loaded);
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
        private void EraseItem(object sender, RoutedEventArgs e)
        {
            SelectedPatientVM.EraseItem();
        }
        private void EraseAll(object sender, RoutedEventArgs e)
        {
            SelectedPatientVM.EraseAll();
        }
    }
}
