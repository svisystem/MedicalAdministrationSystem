using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Settings
{
    public partial class FacilityData : ViewExtender
    {
        protected internal FacilityDataVM FacilityDataVM { get; set; }
        private FacilityDataValid facilityDataValid { get; set; }
        public FacilityData()
        {
            Loading.Show();
            EditorLocalizer.Active = new Localizer();
            facilityDataValid = new FacilityDataValid();
            FacilityDataVM = new FacilityDataVM();
            this.DataContext = FacilityDataVM;
            InitializeComponent();
            validatorClass = facilityDataValid;
            button = modify;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            name.Validate += NonMaskedNotNullValidateForString;
            taxNumber.Validate += MaskedNotNullValidateForString;
            registrationNumber.Validate += MaskedNullEnabledValidateForString;
            invoiceNumber.Validate += MaskedNullEnabledValidateForString;
            zipCode.Validate += zipCode_Validate;
            settlement.Validate += settlement_Validate;
            address.Validate += NonMaskedNotNullValidateForString;
            phone.Validate += MaskedNullEnabledValidateForNumber;
            eMail.Validate += MaskedNullEnabledValidateForString;
            webPage.Validate += MaskedNullEnabledValidateForString;
        }
        private void zipCode_Validate(object sender, ValidationEventArgs e)
        {
            facilityDataValid.zipCode = false;
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                FacilityDataVM.ItemSourceSearcher("zipCode", null);
            }
            else if (!e.Value.ToString().Where(char.IsNumber).Count().Equals(e.Value.ToString().Length))
            {
                e.SetError("A mező csak számokat tartalmazhat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                FacilityDataVM.ItemSourceSearcher("zipCode", "false");
            }
            else if (!FacilityDataVM.ListChecker(e.Value.ToString(), typeof(zipcode_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                FacilityDataVM.ItemSourceSearcher("zipCode", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                facilityDataValid.zipCode = true;
                FacilityDataVM.ItemSourceSearcher("zipCode", e.Value.ToString());
            }
            ForceBinding(sender, e);
        }
        private void zipCodeErase(object sender, RoutedEventArgs e)
        {
            zipCode.Clear();
            FacilityDataVM.ItemSourceSearcher("zipCode", null);
        }
        private void settlement_Validate(object sender, ValidationEventArgs e)
        {
            facilityDataValid.settlement = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("A mező kitöltése nem kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                FacilityDataVM.ItemSourceSearcher("settlement", null);
            }
            else if (!FacilityDataVM.ListChecker(e.Value.ToString(), typeof(settlement_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                FacilityDataVM.ItemSourceSearcher("settlement", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                facilityDataValid.settlement = true;
                FacilityDataVM.ItemSourceSearcher("settlement", e.Value.ToString());

            }
            ForceBinding(sender, e);
        }
        private void settlementErase(object sender, RoutedEventArgs e)
        {
            settlement.Clear();
            FacilityDataVM.ItemSourceSearcher("settlement", null);
        }
        private void FacilityDataChangeExecute(object sender, RoutedEventArgs e)
        {
            FacilityDataVM.ExecuteMethod();
        }
        private void FacilityDataChangeWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) FacilityDataChangeExecute(this, new RoutedEventArgs());
        }
        protected internal bool Dirty()
        {
            return FacilityDataVM.VMDirty();
        }
        private void selectedCompany_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }
        private void selectedCompany_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            FacilityDataVM.Question(false);
        }
        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            FacilityDataVM.Question(true);
        }
        private class FacilityDataValid : FormValidate
        {
            public bool name { get; set; }
            public bool zipCode { get; set; }
            public bool settlement { get; set; }
            public bool address { get; set; }
            public bool taxNumber { get; set; }
            public bool registrationNumber { get; set; }
            public bool invoiceNumber { get; set; }
            public bool phone { get; set; }
            public bool eMail { get; set; }
            public bool webPage { get; set; }
        }
    }
}
