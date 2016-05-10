using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Billing
{
    public partial class CompanyData : ViewExtender
    {
        protected internal CompanyDataVM CompanyDataVM { get; set; }
        private CompanyDataValid companyDataValid { get; set; }
        public CompanyData()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            EditorLocalizer.Active = new Localizer();
            companyDataValid = new CompanyDataValid();
            InitializeComponent();
            CompanyDataVM = new CompanyDataVM(view.MoveFirstRow);
            this.DataContext = CompanyDataVM;
            validatorClass = companyDataValid;
            button = save;
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
            companyDataValid.zipCode = false;
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CompanyDataVM.ItemSourceSearcher("zipCode", null);
            }
            else if (!e.Value.ToString().Where(char.IsNumber).Count().Equals(e.Value.ToString().Length))
            {
                e.SetError("A mező csak számokat tartalmazhat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CompanyDataVM.ItemSourceSearcher("zipCode", "false");
            }
            else if (!CompanyDataVM.ListChecker(e.Value.ToString(), typeof(zipcode_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CompanyDataVM.ItemSourceSearcher("zipCode", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                companyDataValid.zipCode = true;
                CompanyDataVM.ItemSourceSearcher("zipCode", e.Value.ToString());
            }
            ForceBinding(sender, e);
        }
        private void zipCodeErase(object sender, RoutedEventArgs e)
        {
            zipCode.Clear();
            CompanyDataVM.ItemSourceSearcher("zipCode", null);
        }
        private void settlement_Validate(object sender, ValidationEventArgs e)
        {
            companyDataValid.settlement = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("A mező kitöltése nem kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CompanyDataVM.ItemSourceSearcher("settlement", null);
            }
            else if (!CompanyDataVM.ListChecker(e.Value.ToString(), typeof(settlement_fx)))
            {
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CompanyDataVM.ItemSourceSearcher("settlement", "false");
            }
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                companyDataValid.settlement = true;
                CompanyDataVM.ItemSourceSearcher("settlement", e.Value.ToString());

            }
            ForceBinding(sender, e);
        }
        private void settlementErase(object sender, RoutedEventArgs e)
        {
            settlement.Clear();
            CompanyDataVM.ItemSourceSearcher("settlement", null);
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            CompanyDataVM.Save();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            CompanyDataVM.Refresh();
        }
        private void SaveWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Save(this, new RoutedEventArgs());
        }
        private void view_BeforeLayoutRefresh(object sender, DevExpress.Xpf.Core.CancelRoutedEventArgs e)
        {
            e.Cancel = !CompanyDataVM.Validate();
        }
        private void newLine(object sender, RoutedEventArgs e)
        {
            CompanyDataVM.NewLine(view.MoveLastRow);
        }
        private void Erase(object sender, RoutedEventArgs e)
        {
            CompanyDataVM.EraseMethod();
        }
        protected internal bool Dirty()
        {
            return CompanyDataVM.VMDirty();
        }
        private class CompanyDataValid : FormValidate
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