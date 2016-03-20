using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Users
{
    public class PassChangeM : NotifyPropertyChanged
    {
        private string _CurrentPassword;
        private string _NewPassword;
        private string _NewPassSalt;
        private string _ConfirmPassword;
        private string _RegPass;
        private string _RegPassSalt;

        public string CurrentPassword
        {
            get
            {
                return _CurrentPassword;
            }
            set
            {
                if (_CurrentPassword == value) return;
                _CurrentPassword = value;
                OnPropertyChanged("CurrentPassword");
            }
        }
        public string NewPassword
        {
            get
            {
                return _NewPassword;
            }
            set
            {
                if (_NewPassword == value) return;
                _NewPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        public string NewPassSalt
        {
            get
            {
                return _NewPassSalt;
            }
            set
            {
                if (_NewPassSalt == value) return;
                _NewPassSalt = value;
                OnPropertyChanged("NewPassSalt");
            }
        }
        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }
            set
            {
                if (_ConfirmPassword == value) return;
                _ConfirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }
        public string RegPass
        {
            get
            {
                return _RegPass;
            }
            set
            {
                if (_RegPass == value) return;
                _RegPass = value;
                OnPropertyChanged("RegPass");
            }
        }
        public string RegPassSalt
        {
            get
            {
                return _RegPassSalt;
            }
            set
            {
                if (_RegPassSalt == value) return;
                _RegPassSalt = value;
                OnPropertyChanged("RegPassSalt");
            }
        }
    }
}
