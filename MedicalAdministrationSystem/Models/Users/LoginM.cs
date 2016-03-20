using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.Generic;

namespace MedicalAdministrationSystem.Models.Users
{
    public class LoginM : NotifyPropertyChanged
    {
        private string _Username;
        private List<string> _ExistUsers;
        private string _Password;
        private string _RegPass;
        private string _RegPassSalt;
        private Priviledges _PriviledgeList;
        private bool _Verified;

        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (_Username == value) return;
                _Username = value;
                OnPropertyChanged("Username");
            }
        }
        public List<string> ExistUsers
        {
            get
            {
                return _ExistUsers;
            }
            set
            {
                if (_ExistUsers == value) return;
                _ExistUsers = value;
                OnPropertyChanged("ExistUser");
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
        public Priviledges PriviledgeList
        {
            get
            {
                return _PriviledgeList;
            }
            set
            {
                if (_PriviledgeList == value) return;
                _PriviledgeList = value;
                OnPropertyChanged("PriviledgeList");
            }
        }
        public bool Verified
        {
            get
            {
                return _Verified;
            }
            set
            {
                if (_Verified == value) return;
                _Verified = value;
                OnPropertyChanged("Verified");
            }
        }
        public class Priviledges
        {
            public bool schedule { get; set; }
            public bool patient { get; set; }
            public bool examination { get; set; }
            public bool lab { get; set; }
            public bool evidence { get; set; }
            public bool prescription { get; set; }
            public bool billing { get; set; }
            public bool statistic { get; set; }
            public bool users { get; set; }
            public bool setting { get; set; }
            public bool help { get; set; }
            public bool logout { get; set; }
        }
    }
    
}
