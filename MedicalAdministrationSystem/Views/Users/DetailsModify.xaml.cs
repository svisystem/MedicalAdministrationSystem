using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Users
{
    public partial class DetailsModify : ViewExtender
    {
        protected internal DetailsModifyVM DetailsModifyVM { get; set; }
        private DetailsModifyValid detailsModifyValid { get; set; }

        public DetailsModify()
        {
            Start(null);
        }
        public DetailsModify(bool fromPatient)
        {
            Start(fromPatient);
        }
        private void Start(bool? fromPatient)
        {
            Loading.Show();
            EditorLocalizer.Active = new Localizer();
            detailsModifyValid = new DetailsModifyValid();
            if (fromPatient == null) DetailsModifyVM = new DetailsModifyVM();
            else if ((bool)fromPatient) DetailsModifyVM = new DetailsModifyVM((bool)fromPatient);
            this.DataContext = DetailsModifyVM;
            InitializeComponent();
            validatorClass = detailsModifyValid;
            button = detailsModify;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            userName.Validate += UserNameValidate;
            birthName.Validate += BirthNameValidate;
            jobTitle.Validate += MaskedNotNullValidateForString;
            sealNumber.Validate += sealNumber_Validate;
            tajNumber.Validate += MaskedNotNullValidateForString;
            taxNumber.Validate += MaskedNullEnabledValidateForNumber;
            gender.Validate += gender_Validate;
            motherName.Validate += MaskedNullEnabledValidateForString;
            birthPlace.Validate += birthPlace_Validate;
            birthDate.Validate += birthDate_Validate;
            zipCode.Validate += zipCode_Validate;
            settlement.Validate += settlement_Validate;
            address.Validate += NonMaskedNullEnabledValidateForString;
            phone.Validate += MaskedNullEnabledValidateForNumber;
            jobPhone.Validate += MaskedNullEnabledValidateForNumber;
            eMail.Validate += MaskedNullEnabledValidateForString;
        }

        private void sealNumber_Validate(object sender, ValidationEventArgs e)
        {
            detailsModifyValid.sealNumber = true;
            if (e.Value == null) e.SetError("A mező kitöltése csak orvosok számára kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            ForceBinding(sender, e);
        }
        private void gender_Validate(object sender, ValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (!DetailsModifyVM.ListChecker(e.Value.ToString(), typeof(gender_fx)))
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                detailsModifyValid.gender = true;
            }
            ForceBinding(sender, e);
        }
        private void birthPlace_Validate(object sender, ValidationEventArgs e)
        {
            detailsModifyValid.birthPlace = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (!DetailsModifyVM.ListChecker(e.Value.ToString(), typeof(settlement_fx)))
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                detailsModifyValid.birthPlace = true;
            }
            ForceBinding(sender, e);
        }
        private void birthDate_Validate(object sender, ValidationEventArgs e)
        {
            detailsModifyValid.birthDate = false;
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                (sender as DateEdit).EditValue = e.Value;
            }
            else if (e.Value as DateTime? < new DateTime(1900, 1, 1))
                e.SetError("A mező tartalma a megadott értékeken kívülre mutat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (e.Value as DateTime? > DateTime.Now.AddYears(-18))
                e.SetError("Csak a 18. életévüket betöltöttek dolgozhatnak", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                detailsModifyValid.birthDate = true;
            }
            button.IsEnabled = (validatorClass as FormValidate).Validate(validatorClass);
        }
        private void birthDate_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }
        private void zipCode_Validate(object sender, ValidationEventArgs e)
        {
            detailsModifyValid.zipCode = true;
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése nem kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                DetailsModifyVM.ItemSourceSearcher("zipCode", null);
            }
            else if (!e.Value.ToString().Where(char.IsNumber).Count().Equals(e.Value.ToString().Length))
            {
                e.SetError("A mező csak számokat tartalmazhat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                detailsModifyValid.zipCode = false;
                DetailsModifyVM.ItemSourceSearcher("zipCode", "false");
            }
            else if (!DetailsModifyVM.ListChecker(e.Value.ToString(), typeof(zipcode_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                detailsModifyValid.zipCode = false;
                DetailsModifyVM.ItemSourceSearcher("zipCode", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                DetailsModifyVM.ItemSourceSearcher("zipCode", e.Value.ToString());
            }
            ForceBinding(sender, e);
        }
        private void zipCodeErase(object sender, RoutedEventArgs e)
        {
            zipCode.Clear();
            DetailsModifyVM.ItemSourceSearcher("zipCode", null);
        }
        private void settlement_Validate(object sender, ValidationEventArgs e)
        {
            detailsModifyValid.settlement = true;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("A mező kitöltése nem kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                DetailsModifyVM.ItemSourceSearcher("settlement", null);
            }
            else if (!DetailsModifyVM.ListChecker(e.Value.ToString(), typeof(settlement_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                detailsModifyValid.settlement = false;
                DetailsModifyVM.ItemSourceSearcher("settlement", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                DetailsModifyVM.ItemSourceSearcher("settlement", e.Value.ToString());

            }
            ForceBinding(sender, e);
        }
        private void settlementErase(object sender, RoutedEventArgs e)
        {
            settlement.Clear();
            DetailsModifyVM.ItemSourceSearcher("settlement", null);
        }
        private void DetailsModifyExecute(object sender, RoutedEventArgs e)
        {
            DetailsModifyVM.ExecuteMethod();
        }
        private void DetailsModifyWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) DetailsModifyExecute(this, new RoutedEventArgs());
        }
        protected internal bool Dirty()
        {
            return DetailsModifyVM.VMDirty();
        }
        private class DetailsModifyValid : FormValidate
        {
            public bool userName { get; set; }
            public bool birthName { get; set; }
            public bool jobTitle { get; set; }
            public bool sealNumber { get; set; }
            public bool tajNumber { get; set; }
            public bool taxNumber { get; set; }
            public bool gender { get; set; }
            public bool motherName { get; set; }
            public bool birthPlace { get; set; }
            public bool birthDate { get; set; }
            public bool zipCode { get; set; }
            public bool settlement { get; set; }
            public bool address { get; set; }
            public bool phone { get; set; }
            public bool jobPhone { get; set; }
            public bool eMail { get; set; }
        }
    }
}
