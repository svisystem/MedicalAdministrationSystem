using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Dialogs
{
    public class NewPassM : NotifyPropertyChanged
    {
        private string _NewPass;
        private string _NewPassConfirm;
        public string NewPass
        {
            get
            {
                return _NewPass;
            }
            set
            {
                if (_NewPass == value) return;
                _NewPass = value;
                OnPropertyChanged("NewPass");
            }
        }
        public string NewPassConfirm
        {
            get
            {
                return _NewPassConfirm;
            }
            set
            {
                if (_NewPassConfirm == value) return;
                _NewPassConfirm = value;
                OnPropertyChanged("NewPassConfirm");
            }
        }
    }
}
