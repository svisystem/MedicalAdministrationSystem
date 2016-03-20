using MedicalAdministrationSystem.ViewModels.Utilities;
using System;

namespace MedicalAdministrationSystem.Models.Users
{
    public class DetailsModifyMViewElements : NotifyPropertyChanged
    {
        private string _UserName;
        private string _BirthName;
        private string _JobTitle;
        private int? _SealNumber;
        private string _TajNumber;
        private long? _TaxNumber;
        private string _GenderSelected;
        private string _MotherName;
        private string _BirthPlaceSelected;
        private DateTime? _BirthDate;
        private int? _ZipCodeSelected;
        private string _SettlementSelected;
        private string _Address;
        private string _Phone;
        private string _JobPhone;
        private string _Email;

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
        public string JobTitle
        {
            get
            {
                return _JobTitle;
            }
            set
            {
                if (_JobTitle == value) return;
                _JobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }
        public int? SealNumber
        {
            get
            {
                return _SealNumber;
            }
            set
            {
                if (_SealNumber == value) return;
                _SealNumber = value;
                OnPropertyChanged("SealNumber");
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
        public long? TaxNumber
        {
            get
            {
                return _TaxNumber;
            }
            set
            {
                if (_TaxNumber == value) return;
                _TaxNumber = value;
                OnPropertyChanged("TaxNumber");
            }
        }
        public string GenderSelected
        {
            get
            {
                return _GenderSelected;
            }
            set
            {
                if (_GenderSelected == value) return;
                _GenderSelected = value;
                OnPropertyChanged("GenderSelected");
            }
        }
        public string MotherName
        {
            get
            {
                return _MotherName;
            }
            set
            {
                if (_MotherName == value) return;
                _MotherName = value;
                OnPropertyChanged("MotherName");
            }
        }
        public string BirthPlaceSelected
        {
            get
            {
                return _BirthPlaceSelected;
            }
            set
            {
                if (_BirthPlaceSelected == value) return;
                _BirthPlaceSelected = value;
                OnPropertyChanged("BirthPlaceSelected");
            }
        }
        public DateTime? BirthDate
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
        public int? ZipCodeSelected
        {
            get
            {
                return _ZipCodeSelected;
            }
            set
            {
                if (_ZipCodeSelected == value) return;
                _ZipCodeSelected = value;
                OnPropertyChanged("ZipCodeSelected");
            }
        }
        public string SettlementSelected
        {
            get
            {
                return _SettlementSelected;
            }
            set
            {
                if (_SettlementSelected == value) return;
                _SettlementSelected = value;
                OnPropertyChanged("SettlementSelected");
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
        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                if (_Phone == value) return;
                _Phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public string JobPhone
        {
            get
            {
                return _JobPhone;
            }
            set
            {
                if (_JobPhone == value) return;
                _JobPhone = value;
                OnPropertyChanged("JobPhone");
            }
        }
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                if (_Email == value) return;
                _Email = value;
                OnPropertyChanged("Email");
            }
        }
    }
}
