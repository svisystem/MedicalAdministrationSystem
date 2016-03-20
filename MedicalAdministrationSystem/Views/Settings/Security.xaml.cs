using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Settings
{
    public partial class Security : ViewExtender
    {
        protected internal SecurityVM SecurityVM { get; set; }
        private InputValid inputValid { get; set; }
        private SecurityValid securityValid { get; set; }
        private EmptyValid emptyValid { get; set; }
        public Security(Action SecurityLoad)
        {
            Loading.Show();
            SecurityVM = new SecurityVM(SecurityLoad);
            inputValid = new InputValid();
            securityValid = new SecurityValid();
            emptyValid = new EmptyValid();
            this.DataContext = SecurityVM;
            InitializeComponent();
            validatorClass = inputValid;
            button = modify;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            currSecurityUser.Validate += currSecurityUser_Validate;
            newSecurityUser.Validate += newSecurityUser_Validate;
            currSecurityPass.Validate += currSecurityPass_Validate;
            newSecurityPass.Validate += newSecurityPass_Validate;
            confSecurityPass.Validate += confSecurityPass_Validate;
        }
        private void currSecurityUser_Validate(object sender, ValidationEventArgs e)
        {
            newSecurityUser.IsEnabled = false;
            inputValid.currUser = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A felhasználónevet nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (e.Value.ToString().Length < 6)
                e.SetError("A felhasználónévnek legalább 6 karaktert kell tartalmaznia", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (SecurityVM.RegSecurityUserCompare(e.Value.ToString()))
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                newSecurityUser.IsEnabled = true;
                inputValid.currUser = true;
            }
            else
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            modify.IsEnabled = securityValid.Validation(inputValid, emptyValid, securityValid);
        }
        private void newSecurityUser_Validate(object sender, ValidationEventArgs e)
        {
            emptyValid.newUser = false;
            securityValid.newUser = false;
            e.IsValid = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("Amennyiben üresen hagyja ezt a mezőt, mentés után a felhasználónév nem fog változni",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                emptyValid.newUser = true;
            }
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (e.Value.ToString().Length < 6)
                    e.ErrorContent = "A felhasználónévnek legalább 6 karaktert kell tartalmaznia";
                else if (SecurityVM.RegSecurityUserCompare(e.Value.ToString()))
                    e.ErrorContent = "Az új felhasználónév nem egyezhet meg a régivel";
                else if (SecurityVM.PasswordMatch(e.Value.ToString())) e.ErrorContent = "Az új felhasználónév nem egyezhet meg az aktuális jelszóval";
                else if (e.Value.ToString().Equals(newSecurityPass.EditValue))
                {
                    e.ErrorContent = "Az új felhasználónév nem egyezhet meg az új jelszóval";
                }
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    securityValid.newUser = true;
                }
            }
            CallOtherValidateMethod(newSecurityUser, newSecurityPass);
            modify.IsEnabled = securityValid.Validation(inputValid, emptyValid, securityValid);
        }
        private void currSecurityPass_Validate(object sender, ValidationEventArgs e)
        {
            newSecurityPass.IsEnabled = false;
            inputValid.currPass = false;
            e.IsValid = false;
            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            if (string.IsNullOrEmpty(e.Value as string))
                e.ErrorContent = "A jelszót nem lehet üresen hagyni";
            else if (e.Value.ToString().Length < 6)
                e.ErrorContent = "A jelszónak legalább 6 karakteresnek kell lennie";
            else if (e.Value.ToString().Where(char.IsLower).Count().Equals(0))
                e.ErrorContent = "A jelszónak legalább 1 kisbetűt kell tartalmaznia";
            else if (e.Value.ToString().Where(char.IsUpper).Count().Equals(0))
                e.ErrorContent = "A jelszónak legalább 1 nagybetűt kell tartalmaznia";
            else if (e.Value.ToString().Where(char.IsDigit).Count().Equals(0))
                e.ErrorContent = "A jelszónak legalább 1 számot kell tartalmaznia";
            else if (SecurityVM.PasswordMatch(e.Value.ToString()))
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                newSecurityPass.IsEnabled = true;
                inputValid.currPass = true;
            }
            else e.ErrorContent = "A beírt jelszó nem egyezik meg a biztonsági profil jelszavával";

            newSecurityPass.DoValidate();
            modify.IsEnabled = securityValid.Validation(inputValid, emptyValid, securityValid);
        }
        private void newSecurityPass_Validate(object sender, ValidationEventArgs e)
        {
            e.IsValid = false;
            confSecurityPass.IsEnabled = false;
            emptyValid.confPass = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information;
                e.ErrorContent = "Amennyiben üresen hagyja ezt a mezőt, mentés után a jelszó nem fog változni";
                emptyValid.confPass = true;
            }
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (e.Value.ToString().Length < 6) e.ErrorContent = "A jelszónak legalább 6 karakteresnek kell lennie";
                else if (e.Value.ToString().Where(char.IsLower).Count().Equals(0)) e.ErrorContent = "A jelszónak legalább 1 kisbetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsUpper).Count().Equals(0)) e.ErrorContent = "A jelszónak legalább 1 nagybetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsDigit).Count().Equals(0)) e.ErrorContent = "A jelszónak legalább 1 számot kell tartalmaznia";
                else if (SecurityVM.PasswordMatch(e.Value.ToString())) e.ErrorContent = "Az új jelszó nem egyezhet meg az aktuálissal";
                else if (SecurityVM.RegSecurityUserCompare(e.Value.ToString())) e.ErrorContent = "Az új jelszó nem egyezhet meg az aktuális felhasználónévvel";
                else if (e.Value.ToString().Equals(newSecurityUser.EditValue))
                {
                    e.ErrorContent = "Az új jelszó nem egyezhet meg az új felhasználónévvel";
                }
                else
                {
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1;
                    e.ErrorContent = "A mező tartalma megfelelő";
                    confSecurityPass.IsEnabled = true;
                }
            }
            CallOtherValidateMethod(newSecurityPass, newSecurityUser);
            confSecurityPass.DoValidate();
            modify.IsEnabled = securityValid.Validation(inputValid, emptyValid, securityValid);
        }
        private void confSecurityPass_Validate(object sender, ValidationEventArgs e)
        {
            securityValid.confPass = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("Írja be újra választott jelszavát", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else if (SecurityVM.SecurityM.NewSecurityPass.Equals(e.Value.ToString()))
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                securityValid.confPass = true;
            }
            else e.SetError("A jelszavaknak meg kell egyezniük", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            modify.IsEnabled = securityValid.Validation(inputValid, emptyValid, securityValid);
        }
        private void SecurityDatasChangeExecute(object sender, RoutedEventArgs e)
        {
            SecurityVM.ExecuteMethod();
        }
        private void SecurityDatasChangeWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SecurityDatasChangeExecute(this, new RoutedEventArgs());
        }
        protected internal bool Dirty()
        {
            return SecurityVM.VMDirty();
        }
        private ButtonEdit caller;
        protected internal void CallOtherValidateMethod(ButtonEdit who, ButtonEdit whom)
        {
            if (caller == null || who != caller)
            {
                caller = whom;
                whom.DoValidate();
            }
            else caller = null;
        }
        private class SecurityValid : FormValidate
        {
            public bool newUser { get; set; }
            public bool confPass { get; set; }
        }
        private class EmptyValid
        {
            public bool newUser { get; set; }
            public bool confPass { get; set; }
        }
        private class InputValid
        {
            public bool currUser { get; set; }
            public bool currPass { get; set; }
        }
    }
}
