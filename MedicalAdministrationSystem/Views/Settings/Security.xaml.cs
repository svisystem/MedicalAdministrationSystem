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
        private CurrentValid currentValid { get; set; }
        private UserNameValid userNameValid { get; set; }
        private PasswordValid passwordValid { get; set; }
        public Security(Action SecurityLoad)
        {
            Start(SecurityLoad);   
        }
        private async void Start(Action SecurityLoad)
        {
            await Loading.Show();
            SecurityVM = new SecurityVM(SecurityLoad);
            currentValid = new CurrentValid();
            userNameValid = new UserNameValid();
            passwordValid = new PasswordValid();
            this.DataContext = SecurityVM;
            InitializeComponent();
            button = modify;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            currSecurityUser.Validate += currSecurityUser_Validate;
            newSecurityUser.Validate += newSecurityUser_Validate;
            confSecurityUser.Validate += confSecurityUser_Validate;
            currSecurityPass.Validate += currSecurityPass_Validate;
            newSecurityPass.Validate += newSecurityPass_Validate;
            confSecurityPass.Validate += confSecurityPass_Validate;
        }
        private void currSecurityUser_Validate(object sender, ValidationEventArgs e)
        {
            newSecurityUser.IsEnabled = newSecurityPass.IsEnabled = false;
            currentValid.currUser = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A felhasználónevet nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (e.Value.ToString().Length < 6)
                e.SetError("A felhasználónévnek legalább 6 karaktert kell tartalmaznia", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (SecurityVM.RegSecurityUserCompare(e.Value.ToString()))
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                currentValid.currUser = true;
            }
            else
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            modify.IsEnabled = currentValid.Validation(currentValid, userNameValid, passwordValid);
            newSecurityUser.IsEnabled = newSecurityPass.IsEnabled = currentValid.Validate(currentValid);
        }
        private void newSecurityUser_Validate(object sender, ValidationEventArgs e)
        {
            userNameValid.newUser = false;
            e.IsValid = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("Amennyiben üresen hagyja ezt a mezőt, mentés után a felhasználónév nem fog változni",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                userNameValid.newUser = null;
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
                    e.ErrorContent = "Az új felhasználónév nem egyezhet meg az új jelszóval";
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    userNameValid.newUser = true;
                }
            }
            CallOtherValidateMethod(newSecurityUser, newSecurityPass);
            confSecurityUser.DoValidate();
            modify.IsEnabled = currentValid.Validation(currentValid, userNameValid, passwordValid);
        }
        private void confSecurityUser_Validate(object sender, ValidationEventArgs e)
        {
            userNameValid.confUser = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("Írja be újra a választott felhasználónevet", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                userNameValid.confUser = null;
            }
            else if (e.Value.ToString().Equals(newSecurityUser.EditValue))
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                userNameValid.confUser = true;
            }
            else e.SetError("A jelszavaknak meg kell egyezniük", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            modify.IsEnabled = currentValid.Validation(currentValid, userNameValid, passwordValid);
        }
        private void currSecurityPass_Validate(object sender, ValidationEventArgs e)
        {
            newSecurityUser.IsEnabled = newSecurityPass.IsEnabled = false;
            currentValid.currPass = false;
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
                currentValid.currPass = true;
            }
            else e.ErrorContent = "A beírt jelszó nem egyezik meg a biztonsági profil jelszavával";
            modify.IsEnabled = currentValid.Validation(currentValid, userNameValid, passwordValid);
            newSecurityUser.IsEnabled = newSecurityPass.IsEnabled = currentValid.Validate(currentValid);
        }
        private void newSecurityPass_Validate(object sender, ValidationEventArgs e)
        {
            e.IsValid = false;
            passwordValid.newPass = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("Amennyiben üresen hagyja ezt a mezőt, mentés után a jelszó nem fog változni",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                passwordValid.newPass = null;
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
                    e.ErrorContent = "Az új jelszó nem egyezhet meg az új felhasználónévvel";
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    passwordValid.newPass = true;
                }
            }
            CallOtherValidateMethod(newSecurityPass, newSecurityUser);
            confSecurityPass.DoValidate();
            modify.IsEnabled = currentValid.Validation(currentValid, userNameValid, passwordValid);
        }
        private void confSecurityPass_Validate(object sender, ValidationEventArgs e)
        {
            passwordValid.confPass = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.SetError("Írja be újra a választott jelszót", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                passwordValid.confPass = null;
            }
            else if (e.Value.ToString().Equals(newSecurityPass.EditValue))
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                passwordValid.confPass = true;
            }
            else e.SetError("A jelszavaknak meg kell egyezniük", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            modify.IsEnabled = currentValid.Validation(currentValid, userNameValid, passwordValid);
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
        private class UserNameValid
        {
            public bool? newUser { get; set; }
            public bool? confUser { get; set; }
        }
        private class PasswordValid
        {
            public bool? newPass { get; set; }
            public bool? confPass { get; set; }
        }
        private class CurrentValid : FormValidate
        {
            public bool currUser { get; set; }
            public bool currPass { get; set; }
        }
    }
}
