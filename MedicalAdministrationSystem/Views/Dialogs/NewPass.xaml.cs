using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Dialogs;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Dialogs
{
    public partial class NewPass : ViewExtender
    {
        protected internal NewPassVM NewPassVM { get; set; }
        private NewPassValid newPassValid = new NewPassValid();
        private string currentUser { get; set; }
        public NewPass(Button Ok, string CurrentUser)
        {
            this.currentUser = CurrentUser;
            NewPassVM = new NewPassVM();
            this.DataContext = NewPassVM;
            InitializeComponent();
            validatorClass = newPassValid;
            button = Ok;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            newPass.Validate += newPass_Validate;
            confPass.Validate += confPass_Validate;
        }
        private void newPass_Validate(object sender, ValidationEventArgs e)
        {
            newPassValid.newPass = false;
            confPass.IsEnabled = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A jelszót nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.IsValid = false;
                if (e.Value.ToString().Length < 6)
                    e.ErrorContent = "A jelszónak legalább 6 karakteresnek kell lennie";
                else if (e.Value.ToString().Where(char.IsLower).Count() == 0)
                    e.ErrorContent = "A jelszónak legalább 1 kisbetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsUpper).Count() == 0)
                    e.ErrorContent = "A jelszónak legalább 1 nagybetűt kell tartalmaznia";
                else if (e.Value.ToString().Where(char.IsDigit).Count() == 0)
                    e.ErrorContent = "A jelszónak legalább 1 számot kell tartalmaznia";
                else if (e.Value.ToString() == currentUser)
                    e.ErrorContent = "Az új jelszó nem egyezhet meg a felhasználónévvel";
                else
                {
                    e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                    confPass.IsEnabled = true;
                    newPassValid.newPass = true;
                    confPass.DoValidate();
                }
            }
            confPass.DoValidate();
            button.IsEnabled = newPassValid.Validate(newPassValid);
        }
        private void confPass_Validate(object sender, ValidationEventArgs e)
        {
            newPassValid.confPass = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("Írja be újra, választott jelszavát", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else if (!e.Value.Equals(newPass.EditValue))
                e.SetError("A jelszavaknak meg kell egyezniük", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                newPassValid.confPass = true;
            }
            button.IsEnabled = newPassValid.Validate(newPassValid);
        }
        class NewPassValid : FormValidate
        {
            public bool newPass { get; set; }
            public bool confPass { get; set; }
        }
    }
}
