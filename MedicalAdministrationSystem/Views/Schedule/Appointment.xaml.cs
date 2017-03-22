using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models.Schedule;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Schedule
{
    public partial class Appointment : ViewExtender
    {
        private TimeSpan AppointmentLenght;
        private Action CloseMethod { get; set; }
        private AppointmentValid appointmentValid { get; set; }
        public ObservableCollection<ScheduleM.Patient> Patients { get; set; }
        public Appointment(Action CloseMethod)
        {
            this.CloseMethod = CloseMethod;
            EditorLocalizer.Active = new Localizer();
            appointmentValid = new AppointmentValid();
            InitializeComponent();
            validatorClass = appointmentValid;
            button = btnOk;
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
            if (!(bool)visited.IsChecked)
            {
                if (e.Value == null || e.Value as string == "")
                {
                    e.SetError("A páciens nevét nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                    tajNumber.EditValue = null;
                }
                else if (e.Value.GetType() != typeof(Person))
                {
                    e.SetError("Nincs ilyen felhasználó", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                    tajNumber.EditValue = null;
                }
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true);
                    tajNumber.EditValue = Patients.Where(p => p.Id == (e.Value as Person).Id).Single().TajNumber;
                    (this.DataContext as OwnAppointmentFormViewModel).Subject = (e.Value as Person).Name;
                }
            }
            else
            {
                if (e.Value == null || e.Value as string == "") e.SetError("A páciens nevét nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                else
                {
                    string currentName = e.Value.GetType() == typeof(Person) ? (e.Value as Person).Name : e.Value as string;
                    Match match = Regex.Match(currentName,
                        @"(([A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]{1,4})\.?\ )*[A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]+(-[A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]+)*(\ [A-ZÁÉÍÓÖŐÚÜŰ]{1}[a-záéíóöőúüű]+)+");
                    if (match.Success && match.Length == currentName.Length)
                    {
                        e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                        (this.DataContext as OwnAppointmentFormViewModel).Subject = currentName;
                        validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true);
                    }
                    else e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                }
            }
            ForceBinding(sender, e);
        }
        private void tajNumber_Validate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false);
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
            else if (e.Value as DateTime? < DateTime.Now && !notes.IsReadOnly)
                e.SetError("Nem vehetünk fel időpontot a múltba", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, !notes.IsReadOnly);
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
            else if (e.Value as DateTime? <= (DateTime)startDateTime.EditValue)
                e.SetError("Nem lehet az időpont vége kisebb vagy egyenlő a kezdetével", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
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
            if ((sender as Button).Name == "btnOk")
                (this.DataContext as OwnAppointmentFormViewModel).RegistrateEnabled((bool)visited.IsChecked);
            else (this.DataContext as OwnAppointmentFormViewModel).CollectionGetChanges(false);
            CloseMethod();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            visited.IsChecked = !(bool)visited.IsChecked;
            patientName.AutoComplete = nameDropDown.IsEnabled = !(bool)visited.IsChecked;
            if ((bool)visited.IsChecked) tajNumber.Clear();
            tajNumber.IsReadOnly = !(bool)visited.IsChecked;
            patientName.DoValidate();
            tajNumber.DoValidate();
        }
        private class AppointmentValid : FormValidate
        {
            public bool patientName { get; set; }
            public bool tajNumber { get; set; }
            public bool startDateTime { get; set; }
            public bool endDateTime { get; set; }
        }
        private class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
        private void patientNameErase(object sender, System.Windows.RoutedEventArgs e)
        {
            patientName.Clear();
            patientName.DoValidate();
            if (!(bool)visited.IsChecked) tajNumber.Clear();
        }

        private void ViewExtender_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Patients = (this.DataContext as OwnAppointmentFormViewModel).Patients;
            patientName.ItemsSource = new ObservableCollection<Person>(Patients.Select(p => new Person() { Id = p.Id, Name = p.Name }));
            AppointmentLenght = (DateTime)endDateTime.EditValue - (DateTime)startDateTime.EditValue;
            ConnectValidators();
            if ((DateTime)startDateTime.EditValue < DateTime.Now)
            {
                patientName.IsReadOnly = tajNumber.IsReadOnly = startDateTime.IsReadOnly = endDateTime.IsReadOnly = doctors.IsReadOnly = notes.IsReadOnly = true;
                change.IsEnabled = cancel.IsEnabled = false;
            }

            visited.IsChecked = (visited.IsChecked == null) ? false : visited.IsChecked;
            if (visited.IsChecked == true)
            {
                patientName.AutoComplete = nameDropDown.IsEnabled = false;
                patientName.EditValue = (this.DataContext as OwnAppointmentFormViewModel).Subject;
            }
            else if (!string.IsNullOrEmpty((this.DataContext as OwnAppointmentFormViewModel).Subject as string))
                patientName.SelectedItem = (patientName.ItemsSource as ObservableCollection<Person>).
                    Where(pn => pn.Id == Patients.Where(p => p.TajNumber ==
                    (this.DataContext as OwnAppointmentFormViewModel).CustomFields["PatientTajNumber"] as string).Single().Id).Single();
            if ((DateTime)startDateTime.EditValue > DateTime.Now) tajNumber.IsReadOnly = !(bool)visited.IsChecked;
        }
    }
}
