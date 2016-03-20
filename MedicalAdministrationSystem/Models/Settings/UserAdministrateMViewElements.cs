using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class UserAdministrateMViewElements : NotifyPropertyChanged
    {
        private List<accountdata> _UserDatas;
        private ObservableCollection<UserRow> _Users = new ObservableCollection<UserRow>();
        private ObservableCollection<int> _Erased = new ObservableCollection<int>();
        private List<string> _Priviledges;
        public ObservableCollection<UserRow> Users
        {
            get
            {
                return _Users;
            }
            set
            {
                if (_Users == value) return;
                _Users = value;
                OnPropertyChanged("Users");
            }
        }
        public List<accountdata> UserDatas
        {
            get
            {
                return _UserDatas;
            }
            set
            {
                if (_UserDatas == value) return;
                _UserDatas = value;
                OnPropertyChanged("UserDatas");
            }
        }
        public ObservableCollection<int> Erased
        {
            get
            {
                return _Erased;
            }
            set
            {
                if (_Erased == value) return;
                _Erased = value;
                OnPropertyChanged("Erased");
            }
        }
        public List<string> Priviledges
        {
            get
            {
                return _Priviledges;
            }
            set
            {
                if (_Priviledges == value) return;
                _Priviledges = value;
                OnPropertyChanged("Priviledges");
            }
        }
        public class UserRow : NotifyPropertyChanged
        {
            private DateTime _RegistrationDate;
            private string _UserName;
            private string _Priviledge;
            private bool _Verified;
            private bool _PassModified;
            private bool _Enabled;
            public DateTime RegistrationDate
            {
                get
                {
                    return _RegistrationDate;
                }
                set
                {
                    if (_RegistrationDate == value) return;
                    _RegistrationDate = value;
                    OnPropertyChanged("RegistrationDate");
                }
            }
            public string UserName
            {
                get
                {
                    return _UserName;
                }
                set
                {
                    if (_UserName == value) return;
                    _UserName = value;
                    OnPropertyChanged("UserName");
                }
            }
            public string Priviledge
            {
                get
                {
                    return _Priviledge;
                }
                set
                {
                    if (_Priviledge == value) return;
                    _Priviledge = value;
                    OnPropertyChanged("Priviledge");
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
            public bool PassModified
            {
                get
                {
                    return _PassModified;
                }
                set
                {
                    if (_PassModified == value) return;
                    _PassModified = value;
                    OnPropertyChanged("PassModified");
                }
            }
            public bool Enabled
            {
                get
                {
                    return _Enabled;
                }
                set
                {
                    if (_Enabled == value) return;
                    _Enabled = value;
                    OnPropertyChanged("Enabled");
                }
            }
        }
    }
}
