using MedicalAdministrationSystem.ViewModels.Users;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Users
{
    public partial class SurgeryTime : UserControl
    {
        protected internal SurgeryTimeVM SurgeryTimeVM { get; set; }
        public SurgeryTime()
        {
            InitializeComponent();
            SurgeryTimeVM = new SurgeryTimeVM();
            this.DataContext = SurgeryTimeVM.SurgeryTimeM;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            //userName.Validate += new ValidateEventHandler(userName_Validate);
            //pass.Validate += new ValidateEventHandler(pass_Validate);
        }
        protected internal bool Dirty()
        {
            return false;
        }
    }
}
