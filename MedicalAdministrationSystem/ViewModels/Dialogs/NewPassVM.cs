using MedicalAdministrationSystem.Models.Dialogs;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.ViewModels.Dialogs
{
    public class NewPassVM : FormValidate
    {
        public NewPassM NewPassM { get; set; }
        protected internal NewPassVM()
        {
            NewPassM = new NewPassM();
        }
        protected internal string Pass()
        {
            return NewPassM.NewPassConfirm;
        }
    }
}
