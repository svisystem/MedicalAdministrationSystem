using MahApps.Metro.Controls.Dialogs;
using MedicalAdministrationSystem.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Schedule
{
    public partial class AppointmentBase : UserControl
    {
        private CustomDialog CustomDialog { get; set; }
        public AppointmentBase()
        {
            InitializeComponent();
            CustomDialog = new CustomDialog();
            CustomDialog.Content = new Appointment(CloseMethod);
            GlobalVM.MainWindow.Focusable = false;
            GlobalVM.MainWindow.ShowMetroDialogAsync(CustomDialog);
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CustomDialog.DataContext = e.NewValue;
        }
        private void CloseMethod()
        {
            GlobalVM.MainWindow.Focusable = true;
            GlobalVM.MainWindow.HideMetroDialogAsync(CustomDialog);
        }
    }
}
