using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Users
{
    public partial class Registration : ViewExtender
    {
        protected internal RegistrationVM RegistrationVM { get; set; }
        private RegistrationValid registrationValid { get; set; }
        public Registration()
        {
            Loading.Show();
            registrationValid = new RegistrationValid();
            RegistrationVM = new RegistrationVM();
            this.DataContext = RegistrationVM.RegistrationM;
            InitializeComponent();
            validatorClass = registrationValid;
            button = registrate;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            userName.Validate += userName_Validate;
            pass.Validate += pass_Validate;
            confirm.Validate += confirmPass_Validate;
            confirm.Validate += confirmPass_Validate;
            priviledges.Validate += priviledges_Validate;
        }
        private void userName_Validate(object sender, ValidationEventArgs e)
        {
            e.IsValid = false;
            registrationValid.userName = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information;
                e.ErrorContent = "A felhasználónevet nem lehet üresen hagyni";
            }
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (e.Value.ToString().Length < 6)
                    e.ErrorContent = "A felhasználónévnek legalább 6 karaktert kell tartalmaznia";
                else if (RegistrationVM.UserCheck(e.Value.ToString()))
                    e.ErrorContent = "A felhasználónévnek egyedinek kell lennie";
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    registrationValid.userName = true;
                }
            }
            pass.DoValidate();
            ForceBinding(sender, e);
        }
        private void pass_Validate(object sender, ValidationEventArgs e)
        {
            e.IsValid = false;
            confirm.IsEnabled = false;
            registrationValid.pass = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information;
                e.ErrorContent = "A jelszót nem lehet üresen hagyni";
            }
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (e.Value.ToString().Length < 6)
                    e.ErrorContent = "A jelszónak legalább 6 karakteresnek kell lennie";
                else if (e.Value.ToString().Where(char.IsLower).Count() == 0)
                    e.ErrorContent = "A jelszónak legalább 1 kisbetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsUpper).Count() == 0)
                    e.ErrorContent = "A jelszónak legalább 1 nagybetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsDigit).Count() == 0)
                    e.ErrorContent = "A jelszónak legalább 1 számot kell tartalmaznia";
                else if (e.Value.ToString().Equals(userName.EditValue))
                    e.ErrorContent = "A jelszó nem egyezhet meg a felhasználónévvel";
                else
                {
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1;
                    e.IsValid = false;
                    e.ErrorContent = "A mező tartalma megfelelő";
                    confirm.IsEnabled = true;
                    registrationValid.pass = true;
                }
            }
            confirm.DoValidate();
            registrate.IsEnabled = registrationValid.Validate(registrationValid);
        }
        private void confirmPass_Validate(object sender, ValidationEventArgs e)
        {
            registrationValid.confirm = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("Írja be újra, választott jelszavát", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else if (!e.Value.ToString().Equals(pass.EditValue))
                e.SetError("A jelszavaknak meg kell egyezniük", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                registrationValid.confirm = true;
            }
            registrate.IsEnabled = registrationValid.Validate(registrationValid);
        }
        private void priviledges_Validate(object sender, ValidationEventArgs e)
        {
            registrationValid.priviledges = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("Válasszon jogosultságot", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else if (!RegistrationVM.PriviledgeCheck(e.Value.ToString()))
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                registrationValid.priviledges = true;
            }
            ForceBinding(sender, e);
        }
        private void RegistrationExecute(object sender, RoutedEventArgs e)
        {
            RegistrationVM.ExecuteMethod();
        }
        private void RegistrationWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) RegistrationExecute(this, new RoutedEventArgs());
        }
        protected internal bool Dirty()
        {
            return RegistrationVM.VMDirty();
        }
        private class RegistrationValid : FormValidate
        {
            public bool userName { get; set; }
            public bool pass { get; set; }
            public bool confirm { get; set; }
            public bool priviledges { get; set; }
        }
    }
}
