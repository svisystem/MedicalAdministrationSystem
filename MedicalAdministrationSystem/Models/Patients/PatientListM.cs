using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Patients
{
    public class PatientListM : NotifyPropertyChanged
    {
        private ObservableCollection<Patient> _PatientList = new ObservableCollection<Patient>();
        private List<int> _Erased = new List<int>();
        private List<UserList> _FullUsersList;
        private List<UserList> _UserSelectionList;
        private Patient _SelectedRow;
        private List<zipcode_fx> _FullZipCodeList;
        private List<settlement_fx> _FullSettlementList;
        private UserList _SelectedUser;

        public ObservableCollection<Patient> PatientList
        {
            get
            {
                return _PatientList;
            }
            set
            {
                if (_PatientList == value) return;
                _PatientList = value;
                OnPropertyChanged("PatientList");
            }
        }
        public List<int> Erased
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
        public List<UserList> FullUsersList
        {
            get
            {
                return _FullUsersList;
            }
            set
            {
                if (_FullUsersList == value) return;
                _FullUsersList = value;
                OnPropertyChanged("FullUsersList");
            }
        }
        public List<UserList> UserSelectionList
        {
            get
            {
                return _UserSelectionList;
            }
            set
            {
                if (_UserSelectionList == value) return;
                _UserSelectionList = value;
                OnPropertyChanged("UserSelectionList");
            }
        }
        public Patient SelectedRow
        {
            get
            {
                return _SelectedRow;
            }
            set
            {
                if (_SelectedRow == value) return;
                _SelectedRow = value;
                OnPropertyChanged("SelectedRow");
            }
        }
        public List<zipcode_fx> FullZipCodeList
        {
            get
            {
                return _FullZipCodeList;
            }
            set
            {
                if (_FullZipCodeList == value) return;
                _FullZipCodeList = value;
                OnPropertyChanged("FullZipCodeList");
            }
        }
        public List<settlement_fx> FullSettlementList
        {
            get
            {
                return _FullSettlementList;
            }
            set
            {
                if (_FullSettlementList == value) return;
                _FullSettlementList = value;
                OnPropertyChanged("FullSettlementList");
            }
        }
        public UserList SelectedUser
        {
            get
            {
                return _SelectedUser;
            }
            set
            {
                if (_SelectedUser == value) return;
                _SelectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        public class Patient : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
            private string _BirthName;
            private int _BirthPlaceId;
            private string _BirthPlace;
            private DateTime _BirthDate;
            private string _TajNumber;
            private int _ZipCodeId;
            private int _ZipCode;
            private int _SettlementId;
            private string _Settlement;
            private string _Address;
            private List<int> _Belong = new List<int>();
            private List<UserList> _BelongUsers;
            public int Id
            {
                get
                {
                    return _Id;
                }
                set
                {
                    if (_Id == value) return;
                    _Id = value;
                    OnPropertyChanged("Id");
                }
            }
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    if (_Name == value) return;
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
            public string BirthName
            {
                get
                {
                    return _BirthName;
                }
                set
                {
                    if (_BirthName == value) return;
                    _BirthName = value;
                    OnPropertyChanged("BirthName");
                }
            }
            public int BirthPlaceId
            {
                get
                {
                    return _BirthPlaceId;
                }
                set
                {
                    if (_BirthPlaceId == value) return;
                    _BirthPlaceId = value;
                    OnPropertyChanged("BirthPlaceId");
                }
            }
            public string BirthPlace
            {
                get
                {
                    return _BirthPlace;
                }
                set
                {
                    if (_BirthPlace == value) return;
                    _BirthPlace = value;
                    OnPropertyChanged("BirthPlace");
                }
            }
            public DateTime BirthDate
            {
                get
                {
                    return _BirthDate;
                }
                set
                {
                    if (_BirthDate == value) return;
                    _BirthDate = value;
                    OnPropertyChanged("BirthDate");
                }
            }
            public string TajNumber
            {
                get
                {
                    return _TajNumber;
                }
                set
                {
                    if (_TajNumber == value) return;
                    _TajNumber = value;
                    OnPropertyChanged("TajNumber");
                }
            }
            public int ZipCodeId
            {
                get
                {
                    return _ZipCodeId;
                }
                set
                {
                    if (_ZipCodeId == value) return;
                    _ZipCodeId = value;
                    OnPropertyChanged("ZipCodeId");
                }
            }
            public int ZipCode
            {
                get
                {
                    return _ZipCode;
                }
                set
                {
                    if (_ZipCode == value) return;
                    _ZipCode = value;
                    OnPropertyChanged("ZipCode");
                }
            }
            public int SettlementId
            {
                get
                {
                    return _SettlementId;
                }
                set
                {
                    if (_SettlementId == value) return;
                    _SettlementId = value;
                    OnPropertyChanged("SettlementId");
                }
            }
            public string Settlement
            {
                get
                {
                    return _Settlement;
                }
                set
                {
                    if (_Settlement == value) return;
                    _Settlement = value;
                    OnPropertyChanged("Settlement");
                }
            }
            public string Address
            {
                get
                {
                    return _Address;
                }
                set
                {
                    if (_Address == value) return;
                    _Address = value;
                    OnPropertyChanged("Address");
                }
            }
            public List<int> Belong
            {
                get
                {
                    return _Belong;
                }
                set
                {
                    if (_Belong == value) return;
                    _Belong = value;
                    OnPropertyChanged("Belong");
                }
            }
            public List<UserList> BelongUsers
            {
                get
                {
                    return _BelongUsers;
                }
                set
                {
                    if (_BelongUsers == value) return;
                    _BelongUsers = value;
                    OnPropertyChanged("BelongUsers");
                }
            }
        }
        public class UserList : NotifyPropertyChanged
        {
            private bool _Belong;
            private int _Id;
            private string _Name;
            public bool Belong
            {
                get
                {
                    return _Belong;
                }
                set
                {
                    if (_Belong == value) return;
                    _Belong = value;
                    OnPropertyChanged("Belong");
                }
            }
            public int Id
            {
                get
                {
                    return _Id;
                }
                set
                {
                    if (_Id == value) return;
                    _Id = value;
                    OnPropertyChanged("Id");
                }
            }
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    if (_Name == value) return;
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }
}
