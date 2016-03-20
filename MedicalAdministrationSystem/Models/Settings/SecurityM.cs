using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class SecurityM : NotifyPropertyChanged
    {
        private string _RegSecurityUser;
        private string _RegSecurityPass;
        private string _RegSecurityPassSalt;
        private string _CurrSecurityUser;
        private string _NewSecurityUser;
        private string _CurrSecurityPass;
        private string _NewSecurityPass;
        private string _ConfSecurityPass;
        public string RegSecurityUser
        {
            get
            {
                return _RegSecurityUser;
            }
            set
            {
                if (_RegSecurityUser == value) return;
                _RegSecurityUser = value;
                OnPropertyChanged("RegSecurityUser");
            }
        }
        public string RegSecurityPass
        {
            get
            {
                return _RegSecurityPass;
            }
            set
            {
                if (_RegSecurityPass == value) return;
                _RegSecurityPass = value;
                OnPropertyChanged("RegSecurityPass");
            }
        }
        public string RegSecurityPassSalt
        {
            get
            {
                return _RegSecurityPassSalt;
            }
            set
            {
                if (_RegSecurityPassSalt == value) return;
                _RegSecurityPassSalt = value;
                OnPropertyChanged("RegSecurityPassSalt");
            }
        }
        public string CurrSecurityUser
        {
            get
            {
                return _CurrSecurityUser;
            }
            set
            {
                if (_CurrSecurityUser == value) return;
                _CurrSecurityUser = value;
                OnPropertyChanged("CurrSecurityUser");
            }
        }
        public string NewSecurityUser
        {
            get
            {
                return _NewSecurityUser;
            }
            set
            {
                if (_NewSecurityUser == value) return;
                _NewSecurityUser = value;
                OnPropertyChanged("NewSecurityUser");
            }
        }
        public string CurrSecurityPass
        {
            get
            {
                return _CurrSecurityPass;
            }
            set
            {
                if (_CurrSecurityPass == value) return;
                _CurrSecurityPass = value;
                OnPropertyChanged("CurrSecurityPass");
            }
        }
        public string NewSecurityPass
        {
            get
            {
                return _NewSecurityPass;
            }
            set
            {
                if (_NewSecurityPass == value) return;
                _NewSecurityPass = value;
                OnPropertyChanged("NewSecurityPass");
            }
        }
        public string ConfSecurityPass
        {
            get
            {
                return _ConfSecurityPass;
            }
            set
            {
                if (_ConfSecurityPass == value) return;
                _ConfSecurityPass = value;
                OnPropertyChanged("ConfSecurityPass");
            }
        }
    }
}

