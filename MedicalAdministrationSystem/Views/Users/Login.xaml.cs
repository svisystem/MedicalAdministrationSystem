using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Users
{
    public partial class Login : ViewExtender
    {
        protected internal LoginVM LoginVM { get; set; }
        private LoginValid loginValid { get; set; } = new LoginValid();
        public Login()
        {
            Loading.Show();
            loginValid = new LoginValid();
            LoginVM = new LoginVM();
            this.DataContext = LoginVM.LoginM;
            InitializeComponent();
            validatorClass = loginValid;
            button = login;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            userName.Validate += userName_Validate;
            pass.Validate += pass_Validate;
        }
        private void userName_Validate(object sender, ValidationEventArgs e)
        {
            loginValid.userName = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A felhasználónevet nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else if (e.Value.ToString().Length < 6)
                e.SetError("A felhasználónévnek legalább 6 karaktert kell tartalmaznia", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                loginValid.userName = true;
                pass.DoValidate();
            }
            ForceBinding(sender, e);
        }
        private void pass_Validate(object sender, ValidationEventArgs e)
        {
            loginValid.pass = false;
            e.IsValid = false;
            if (string.IsNullOrEmpty(e.Value as string))
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information;
                e.ErrorContent = "A jelszót nem lehet üresen hagyni";
                loginValid.pass = false;
            }
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (e.Value.ToString().Length < 6) e.ErrorContent = "A jelszónak legalább 6 karakteresnek kell lennie";
                else if (e.Value.ToString().Where(char.IsLower).Count().Equals(0))
                    e.ErrorContent = "A jelszónak legalább 1 kisbetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsUpper).Count().Equals(0))
                    e.ErrorContent = "A jelszónak legalább 1 nagybetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsDigit).Count().Equals(0))
                    e.ErrorContent = "A jelszónak legalább 1 számot kell tartalmaznia";
                else if (e.Value.ToString().Equals(userName.EditValue))
                    e.ErrorContent = "A jelszó nem egyezhet meg a felhasználónévvel";
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    loginValid.pass = true;
                }
            }
            login.IsEnabled = loginValid.Validate(loginValid);
        }
        private void LoginExecute(object sender, RoutedEventArgs e)
        {
            LoginVM.ExecuteMethod();
        }
        private void LoginWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LoginExecute(this, new RoutedEventArgs());
        }
        protected internal bool Dirty()
        {
            return LoginVM.VMDirty();
        }
        private class LoginValid : FormValidate
        {
            public bool userName { get; set; }
            public bool pass { get; set; }
        }
    }
}