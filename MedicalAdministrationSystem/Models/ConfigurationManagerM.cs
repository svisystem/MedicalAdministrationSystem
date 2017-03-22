using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models
{
    public class ConfigurationManagerM : NotifyPropertyChanged
    {
        private string _Server;
        private string _PortNumber;
        private string _UserId;
        private string _Password;
        private string _Database;
        private string _SecurityUsername;
        private string _SecurityPassword;
        private string _SecurityPasswordSalt;
        private string _FacilityId;

        public string Server
        {
            get
            {
                return _Server;
            }
            set
            {
                if (_Server == value) return;
                _Server = value;
                OnPropertyChanged("Server");
            }
        }
        public string PortNumber
        {
            get
            {
                return _PortNumber;
            }
            set
            {
                if (_PortNumber == value) return;
                _PortNumber = value;
                OnPropertyChanged("PortNumber");
            }
        }
        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                if (_UserId == value) return;
                _UserId = value;
                OnPropertyChanged("UserId");
            }
        }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (_Password == value) return;
                _Password = value;
                OnPropertyChanged("Password");
            }
        }
        public string Database
        {
            get
            {
                return _Database;
            }
            set
            {
                if (_Database == value) return;
                _Database = value;
                OnPropertyChanged("Database");
            }
        }
        public string SecurityUsername
        {
            get
            {
                return _SecurityUsername;
            }
            set
            {
                if (_SecurityUsername == value) return;
                _SecurityUsername = value;
                OnPropertyChanged("SecurityUsername");
            }
        }
        public string SecurityPassword
        {
            get
            {
                return _SecurityPassword;
            }
            set
            {
                if (_SecurityPassword == value) return;
                _SecurityPassword = value;
                OnPropertyChanged("SecurityPassword");
            }
        }
        public string SecurityPasswordSalt
        {
            get
            {
                return _SecurityPasswordSalt;
            }
            set
            {
                if (_SecurityPasswordSalt == value) return;
                _SecurityPasswordSalt = value;
                OnPropertyChanged("SecurityPasswordSalt");
            }
        }
        public string FacilityId
        {
            get
            {
                return _FacilityId;
            }
            set
            {
                if (_FacilityId == value) return;
                _FacilityId = value;
                OnPropertyChanged("FacilityId");
            }
        }
    }
}
