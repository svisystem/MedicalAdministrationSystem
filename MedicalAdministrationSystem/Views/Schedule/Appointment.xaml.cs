using DevExpress.Xpf.Editors;
using MahApps.Metro.Controls.Dialogs;
using MedicalAdministrationSystem.Models.Schedule;
using MedicalAdministrationSystem.ViewModels;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace MedicalAdministrationSystem.Views.Schedule
{
    public partial class Appointment : ViewExtender
    {
        private TimeSpan AppointmentLenght;
        private CustomDialog CustomDialog = new CustomDialog();
        private AppointmentValid appointmentValid { get; set; }
        public ObservableCollection<ScheduleM.Patient> Patients { get; set; }
        public Appointment()
        {
            EditorLocalizer.Active = new Localizer();
            appointmentValid = new AppointmentValid();
            InitializeComponent();
            validatorClass = appointmentValid;
            button = btnOk;
            ConnectValidators();
            grid.Width = GlobalVM.MainWindow.ActualWidth;
            GlobalVM.MainWindow.ShowMetroDialogAsync(CustomDialog);
            GlobalVM.MainWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
        }
        private void ConnectValidators()
        {
            patientName.Validate += patientName_Validate;
            tajNumber.Validate += tajNumber_Validate;
            startDateTime.Validate += startDateTime_Validate;
            endDateTime.Validate += endDateTime_Validate;
        }
        private void patientName_Validate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false);
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A páciens nevét nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else
            {
                if ((bool)visited.IsChecked)
                {
                    Match match = Regex.Match(e.Value as string, @"(([A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]{1,4})\.?\ )*[A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]+(-[A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]+)*(\ [A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]+)+");
                    if (match.Success && match.Length == (e.Value as string).Length)
                    {
                        e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                        validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true);
                    }
                    else e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                }
                else
                {
                    if (Patients.Any(p => p.Name == e.Value as string))
                    {
                        e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                        validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true);
                    }
                    else e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                    if ((sender as ComboBoxEdit).SelectedIndex != -1) tajNumber.EditValue = Patients[(sender as ComboBoxEdit).SelectedIndex].TajNumber;
                    else tajNumber.EditValue = null;
                }
            }
            ForceBinding(sender, e);
        }
        private void tajNumber_Validate(object sender, ValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A TAJ szám kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else
            {
                if (Patients.Any(p => p.TajNumber == e.Value as string) && (bool)visited.IsChecked)
                    e.SetError("A mező tartalmának egyedinel kell lennie", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true);
                }
            }
            ForceBinding(sender, e);
        }
        private void startDateTime_Validate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false);
            if (e.Value == null)
            {
                e.SetError("A mezőt nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                (sender as DateEdit).EditValue = e.Value;
            }
            else if (e.Value as DateTime? < DateTime.Now)
                e.SetError("Nem vehetünk fel időpontot a múltba", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true);
                if (appointmentValid.endDateTime) endDateTime.EditValue = (DateTime)(sender as DateEdit).EditValue + AppointmentLenght;
                else endDateTime.DoValidate();
            }
            button.IsEnabled = (validatorClass as FormValidate).Validate(validatorClass);
        }
        private void endDateTime_Validate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false);
            if (e.Value == null)
            {
                e.SetError("A mezőt nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                (sender as DateEdit).EditValue = e.Value;
            }
            else if (e.Value as DateTime? < (DateTime)startDateTime.EditValue)
                e.SetError("Nem lehet az időpont vége korábban mint a kezdete", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true);
                AppointmentLenght = (DateTime)endDateTime.EditValue - (DateTime)startDateTime.EditValue;
            }
            button.IsEnabled = (validatorClass as FormValidate).Validate(validatorClass);
        }
        private void CloseButton(object sender, System.Windows.RoutedEventArgs e)
        {
            GlobalVM.MainWindow.HideMetroDialogAsync(CustomDialog);
            GlobalVM.MainWindow.ResizeMode = System.Windows.ResizeMode.CanResize;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            visited.IsChecked = !(bool)visited.IsChecked;
            patientName.AutoComplete = nameDropDown.IsEnabled = !(bool)visited.IsChecked;
            tajNumber.IsEnabled = (bool)visited.IsChecked;
            patientName.DoValidate();
            tajNumber.DoValidate();
        }
        private void visited_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as CheckEdit).IsChecked = ((sender as CheckEdit).IsChecked == null) ? false : (sender as CheckEdit).IsChecked;
            if ((sender as CheckEdit).IsChecked == true) patientName.AutoComplete = nameDropDown.IsEnabled = false;
            tajNumber.IsEnabled = (bool)visited.IsChecked;
        }
        private class AppointmentValid : FormValidate
        {
            public bool patientName { get; set; }
            public bool tajNumber { get; set; }
            public bool startDateTime { get; set; }
            public bool endDateTime { get; set; }
        }

        private void ViewExtender_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Patients = (this.DataContext as dynamic).Patients;
            patientName.ItemsSource = Patients.Select(p => p.Name).ToList();
            AppointmentLenght = (DateTime)endDateTime.EditValue - (DateTime)startDateTime.EditValue;
        }

        private void patientNameErase(object sender, System.Windows.RoutedEventArgs e)
        {
            patientName.Clear();
            if (!(bool)visited.IsChecked) tajNumber.Clear();
        }
    }
}
