using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels;
using MedicalAdministrationSystem.ViewModels.Patients;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Patients
{
    public partial class PatientList : UserControl
    {
        protected internal PatientListVM PatientListVM { get; set; }
        protected internal Action Selected { get; set; }
        public PatientList(Action Selected)
        {
            this.Selected = Selected;
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            PatientListVM = new PatientListVM(view_Loaded);
            this.DataContext = PatientListVM;
            InitializeComponent();
            if (!GlobalVM.GlobalM.AllSee) selectedUser.Visibility = Visibility.Hidden;
        }
        protected internal bool Dirty()
        {
            return PatientListVM.VMDirty();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            PatientListVM.ExecuteMethod();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            PatientListVM.Question(true);
        }
        private void selectedUser_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            PatientListVM.Question(false);
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(delegate
             {
                 view.BestFitColumns();
             }), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
        private void check_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (sender as CheckEdit).IsChecked = !(sender as CheckEdit).IsChecked;
        }
        private void erase(object sender, RoutedEventArgs e)
        {
            PatientListVM.PatientEraseMethod();
        }
        private void select(object sender, RoutedEventArgs e)
        {
            PatientListVM.Select(Selected);
            Selected();
        }
    }
}
