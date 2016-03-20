using System.Collections.Generic;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Users
{
    public class RegistrationM : NotifyPropertyChanged
    {
        private string _Username;
        private List<string> _ExistUsers;
        private string _Password;
        private string _Confirm;
        private string _PassSalt;
        private List<string> _PriviledgesList;
        private string _PriviledgesSelected;
        private int _PriviledgesID;

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
        public string Confirm
        {
            get
            {
                return _Confirm;
            }
            set
            {
                if (_Confirm == value) return;
                _Confirm = value;
                OnPropertyChanged("Confirm");
            }
        }
        public string PassSalt
        {
            get
            {
                return _PassSalt;
            }
            set
            {
                if (_PassSalt == value) return;
                _PassSalt = value;
                OnPropertyChanged("PassSalt");
            }
        }
        public List<string> PriviledgesList
        {
            get
            {
                return _PriviledgesList;
            }
            set
            {
                if (_PriviledgesList == value) return;
                _PriviledgesList = value;
                OnPropertyChanged("PriviledgesList");
            }
        }
        public string PriviledgesSelected
        {
            get
            {
                return _PriviledgesSelected;
            }
            set
            {
                if (_PriviledgesSelected == value) return;
                _PriviledgesSelected = value;
                OnPropertyChanged("PriviledgesSelected");
            }
        }
        public int PriviledgesID
        {
            get
            {
                return _PriviledgesID;
            }
            set
            {
                if (_PriviledgesID == value) return;
                _PriviledgesID = value;
                OnPropertyChanged("PriviledgesID");
            }
        }
    }
}
