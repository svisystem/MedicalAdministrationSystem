using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Settings
{
    public partial class Connection : ViewExtender
    {
        protected internal ConnectionVM ConnectionVM { get; set; }
        private ConnectionValid connectionValid { get; set; }
        public Connection()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            connectionValid = new ConnectionValid();
            ConnectionVM = new ConnectionVM();
            this.DataContext = ConnectionVM;
            InitializeComponent();
            validatorClass = connectionValid;
            button = modify;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            hostName.Validate += ConnectionValidate;
            portNumber.Validate += portNumber_Validate;
            databaseName.Validate += ConnectionValidate;
            userId.Validate += ConnectionValidate;
            password.Validate += ConnectionValidate;
        }
        private void portNumber_Validate(object sender, ValidationEventArgs e)
        {
            connectionValid.portNumber = false;
            if (string.IsNullOrEmpty(e.Value as string)) e.SetError("A mező kitöltése kötelező", 
                DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                else if (e.Value.ToString().Where(char.IsDigit).Count() != e.Value.ToString().Length)
                e.SetError("A mező csak számokat tartalmazhat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                connectionValid.portNumber = true;
            }
            ForceBinding(sender, e);
        }
        private void ConnectionValidate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false, null);
            if (string.IsNullOrEmpty(e.Value as string)) e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (e.Value.ToString().Contains("=") ||
                e.Value.ToString().Contains(";") ||
                e.Value.ToString().Contains("\""))
                e.SetError("A mező nem tartalmazhat \"=\" és \";\" karaktereket, illetve idézőjeleket",
                    DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            }
            ForceBinding(sender, e);
        }
        private void SecurityDatasChangeExecute(object sender, RoutedEventArgs e)
        {
            ConnectionVM.ExecuteMethod();
        }
        private void SecurityDatasChangeWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SecurityDatasChangeExecute(this, new RoutedEventArgs());
        }
        protected internal bool Dirty()
        {
            return ConnectionVM.VMDirty();
        }
        private class ConnectionValid : FormValidate
        {
            public bool hostName { get; set; }
            public bool portNumber { get; set; }
            public bool databaseName { get; set; }
            public bool userId { get; set; }
            public bool password { get; set; }
        }
    }
}
