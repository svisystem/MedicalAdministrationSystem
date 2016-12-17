using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public abstract class FormValidate : NotifyPropertyChanged
    {
        protected internal bool Validate(object item) =>
            !item.GetType().GetProperties().Where(p => p.Name != "IsChanged").Any(p => !(bool)p.GetValue(item));
        protected internal bool Validation(object current, object userName, object password) =>
            Validate(current) && Validate2(userName) && Validate2(password) && Validate3(userName, password);
        private bool Validate2(object item) =>
            item.GetType().GetProperties().Where(p => p.Name != "IsChanged").All(p => p.GetValue(item) == null) ? true :
            item.GetType().GetProperties().Where(p => p.Name != "IsChanged").Any(p => p.GetValue(item) == null || !(bool)p.GetValue(item)) ? false : true;
        private bool Validate3(object userName, object password) =>
            userName.GetType().GetProperties().Where(p => p.Name != "IsChanged").All(p => p.GetValue(userName) == null) &&
            password.GetType().GetProperties().Where(p => p.Name != "IsChanged").All(p => p.GetValue(password) == null) ? false : true;
    }
}
