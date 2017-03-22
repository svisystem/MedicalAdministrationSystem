using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Users
{
    public partial class PassChange : ViewExtender
    {
        protected internal PassChangeVM PassChangeVM { get; set; }
        private PassChangeValid passChangeValid { get; set; }
        public PassChange()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            passChangeValid = new PassChangeValid();
            PassChangeVM = new PassChangeVM();
            this.DataContext = PassChangeVM.PassChangeM;
            InitializeComponent();
            validatorClass = passChangeValid;
            button = modify;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            currPass.Validate += currPass_Validate;
            newPass.Validate += newPass_Validate;
            confPass.Validate += confPass_Validate;
        }
        private void currPass_Validate(object sender, ValidationEventArgs e)
        {
            e.IsValid = false;
            newPass.IsEnabled = false;
            passChangeValid.currPass = false;
            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            if (string.IsNullOrEmpty(e.Value as string))
                e.ErrorContent = "A jelszót nem lehet üresen hagyni";
            else if (e.Value.ToString().Length < 6)
                e.ErrorContent = "A jelszónak legalább 6 karakteresnek kell lennie";
            else if (e.Value.ToString().Where(char.IsLower).Count() == 0)
                e.ErrorContent = "A jelszónak legalább 1 kisbetűt kell tartalmaznia";
            else if (e.Value.ToString().Where(char.IsUpper).Count() == 0)
                e.ErrorContent = "A jelszónak legalább 1 nagybetűt kell tartalmaznia";
            else if (e.Value.ToString().Where(char.IsDigit).Count() == 0)
                e.ErrorContent = "A jelszónak legalább 1 számot kell tartalmaznia";
            else if (!PassChangeVM.PasswordMatch())
                e.ErrorContent = "A beírt jelszó nem egyezik meg a bejelentkezett felhasználó profiljáéval";
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1;
                e.ErrorContent = "A mező tartalma megfelelő";
                newPass.IsEnabled = true;
                passChangeValid.currPass = true;
            }
            newPass.DoValidate();
            modify.IsEnabled = passChangeValid.Validate(passChangeValid);
        }
        private void newPass_Validate(object sender, ValidationEventArgs e)
        {
            e.IsValid = false;
            confPass.IsEnabled = false;
            passChangeValid.newPass = false;
            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            if (string.IsNullOrEmpty(e.Value as string))
                e.ErrorContent = "A jelszót nem lehet üresen hagyni";
            else if (e.Value.ToString().Length < 6)
                e.ErrorContent = "A jelszónak legalább 6 karakteresnek kell lennie";
            else if (e.Value.ToString().Where(char.IsLower).Count() == 0)
                e.ErrorContent = "A jelszónak legalább 1 kisbetűt kell tartalmaznia";
            else if (e.Value.ToString().Where(char.IsUpper).Count() == 0)
                e.ErrorContent = "A jelszónak legalább 1 nagybetűt kell tartalmaznia";
            else if (e.Value.ToString().Where(char.IsDigit).Count() == 0)
                e.ErrorContent = "A jelszónak legalább 1 számot kell tartalmaznia";
            else if (PassChangeVM.CurrentPassCompare(e.Value.ToString()))
                e.ErrorContent = "Az új jelszó nem egyezhet meg a régivel";
            else if (PassChangeVM.UserNameCompare(e.Value.ToString()))
                e.ErrorContent = "Az új jelszó nem egyezhet meg a felhasználónévvel";
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1;
                e.ErrorContent = "A mező tartalma megfelelő";
                confPass.IsEnabled = true;
                passChangeValid.newPass = true;
            }
            confPass.DoValidate();
            modify.IsEnabled = passChangeValid.Validate(passChangeValid);
        }
        private void confPass_Validate(object sender, ValidationEventArgs e)
        {
            passChangeValid.confPass = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("Írja be újra választott jelszavát", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else if (!e.Value.ToString().Equals(newPass.EditValue))
                e.SetError("A jelszavaknak meg kell egyezniük", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                passChangeValid.confPass = true;
            }
            modify.IsEnabled = passChangeValid.Validate(passChangeValid);
        }
        private void PassChangeExecute(object sender, RoutedEventArgs e)
        {
            PassChangeVM.ExecuteMethod();
        }
        private void PassChangeWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) PassChangeExecute(this, new RoutedEventArgs());
        }
        protected internal bool Dirty()
        {
            return PassChangeVM.VMDirty();
        }
        private class PassChangeValid : FormValidate
        {
            public bool currPass { get; set; }
            public bool newPass { get; set; }
            public bool confPass { get; set; }
        }
    }
}
