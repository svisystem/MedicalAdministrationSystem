using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Patients;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Patients
{
    public partial class PatientDetails : ViewExtender
    {
        protected internal PatientDetailsVM PatientDetailsVM { get; set; }
        private PatientDetailsValid patientDetailsValid { get; set; }
        protected internal bool newView { get; private set; }
        public PatientDetails(bool newView)
        {
            Start(newView);
        }
        private async void Start(bool newView)
        {
            await Loading.Show();
            this.newView = newView;
            EditorLocalizer.Active = new Localizer();
            patientDetailsValid = new PatientDetailsValid();
            PatientDetailsVM = new PatientDetailsVM(newView);
            this.DataContext = PatientDetailsVM;
            InitializeComponent();
            validatorClass = patientDetailsValid;
            button = modify;
            ConnectValidators();
            if (newView) modify.Content = "Páciens felvétele";
        }
        private void ConnectValidators()
        {
            userName.Validate += UserNameValidate;
            birthName.Validate += BirthNameValidate;
            gender.Validate += gender_Validate;
            motherName.Validate += MaskedNotNullValidateForString;
            birthPlace.Validate += birthPlace_Validate;
            birthDate.Validate += birthDate_Validate;
            tajNumber.Validate += MaskedNotNullValidateForString;
            taxNumber.Validate += MaskedNullEnabledValidateForNumber;
            zipCode.Validate += zipCode_Validate;
            settlement.Validate += settlement_Validate;
            address.Validate += NonMaskedNotNullValidateForString;
            phone.Validate += MaskedNullEnabledValidateForNumber;
            mobilePhone.Validate += MaskedNullEnabledValidateForNumber;
            eMail.Validate += MaskedNullEnabledValidateForString;
            billingName.Validate += UserNameValidate;
            billingZipCode.Validate += zipCode_Validate;
            billingSettlement.Validate += settlement_Validate;
            billingAddress.Validate += NonMaskedNotNullValidateForString;
        }
        private void gender_Validate(object sender, ValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (!PatientDetailsVM.ListChecker(e.Value.ToString(), typeof(gender_fx)))
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                patientDetailsValid.gender = true;
            }
            ForceBinding(sender, e);
        }
        private void birthPlace_Validate(object sender, ValidationEventArgs e)
        {
            patientDetailsValid.birthPlace = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (!PatientDetailsVM.ListChecker(e.Value.ToString(), typeof(settlement_fx)))
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                patientDetailsValid.birthPlace = true;
            }
            ForceBinding(sender, e);
        }
        private void birthDate_Validate(object sender, ValidationEventArgs e)
        {
            patientDetailsValid.birthDate = false;
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                (sender as DateEdit).EditValue = e.Value;
            }
            else if (e.Value as DateTime? < new DateTime(1900, 1, 1))
                e.SetError("A mező tartalma a megadott értékeken kívülre mutat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (e.Value as DateTime? > DateTime.Now)
                e.SetError("Nem vehetünk fel egy személyt aki még meg se született", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                patientDetailsValid.birthDate = true;
            }
            button.IsEnabled = (validatorClass as FormValidate).Validate(validatorClass);
        }
        private void birthDate_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }
        private void zipCode_Validate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false, null);
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                PatientDetailsVM.ItemSourceSearcher("zipCode", null);
            }
            else if (!e.Value.ToString().Where(char.IsNumber).Count().Equals(e.Value.ToString().Length))
            {
                e.SetError("A mező csak számokat tartalmazhat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                PatientDetailsVM.ItemSourceSearcher("zipCode", "false");
            }
            else if (!PatientDetailsVM.ListChecker(e.Value.ToString(), typeof(zipcode_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                PatientDetailsVM.ItemSourceSearcher("zipCode", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                PatientDetailsVM.From = (sender as ComboBoxEdit).Name == "zipCode";
                PatientDetailsVM.ItemSourceSearcher("zipCode", e.Value.ToString());
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            }
            ForceBinding(sender, e);
        }
        private void zipCodeErase(object sender, RoutedEventArgs e)
        {
            zipCode.Clear();
            PatientDetailsVM.ItemSourceSearcher("zipCode", null);
        }
        private void settlement_Validate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false, null);
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                PatientDetailsVM.ItemSourceSearcher("settlement", null);
            }
            else if (!PatientDetailsVM.ListChecker(e.Value.ToString(), typeof(settlement_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                PatientDetailsVM.ItemSourceSearcher("settlement", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                PatientDetailsVM.From = (sender as ComboBoxEdit).Name == "settlement";
                PatientDetailsVM.ItemSourceSearcher("settlement", e.Value.ToString());
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            }
            ForceBinding(sender, e);
        }
        private void settlementErase(object sender, RoutedEventArgs e)
        {
            settlement.Clear();
            PatientDetailsVM.ItemSourceSearcher("settlement", null);
        }
        private void billingZipCodeErase(object sender, RoutedEventArgs e)
        {
            billingZipCode.Clear();
            PatientDetailsVM.ItemSourceSearcher("zipCode", null);
        }
        private void billingSettlementErase(object sender, RoutedEventArgs e)
        {
            billingSettlement.Clear();
            PatientDetailsVM.ItemSourceSearcher("settlement", null);
        }
        private void Compare(object sender, RoutedEventArgs e)
        {
            if (billingName.EditValue != null ||
                billingZipCode.EditValue != null ||
                billingSettlement.EditValue != null ||
                billingAddress.EditValue != null)
            {
                Dialog dialog = new Dialog(true, "Adatok módosítása", PatientDetailsVM.Copy, () => { }, true);
                dialog.content = new Dialogs.TextBlock("Biztosan felülírja a számlázási adatokat?\n" +
                    "A korábbi adatok elvesznek");
                dialog.Start();
            }
            else PatientDetailsVM.Copy();
        }
        protected internal bool Dirty()
        {
            return PatientDetailsVM.VMDirty();
        }
        private void ModifyExecute(object sender, RoutedEventArgs e)
        {
            PatientDetailsVM.ExecuteMethod();
        }
        private void ModifyWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ModifyExecute(this, new RoutedEventArgs());
        }
        private class PatientDetailsValid : FormValidate
        {
            public bool userName { get; set; }
            public bool birthName { get; set; }
            public bool gender { get; set; }
            public bool motherName { get; set; }
            public bool birthPlace { get; set; }
            public bool birthDate { get; set; }
            public bool tajNumber { get; set; }
            public bool taxNumber { get; set; }
            public bool zipCode { get; set; }
            public bool settlement { get; set; }
            public bool address { get; set; }
            public bool phone { get; set; }
            public bool mobilePhone { get; set; }
            public bool eMail { get; set; }
            public bool billingName { get; set; }
            public bool billingZipCode { get; set; }
            public bool billingSettlement { get; set; }
            public bool billingAddress { get; set; }
        }
    }
}
