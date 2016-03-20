using System;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Patients
{
    public class PatientDetailsMViewElements : NotifyPropertyChanged
    {
        private string _UserName;
        private string _BirthName;
        private string _GenderSelected;
        private string _MotherName;
        private string _BirthPlaceSelected;
        private DateTime? _BirthDate;
        private string _TajNumber;
        private long? _TaxNumber;
        private int? _ZipCodeSelected;
        private string _SettlementSelected;
        private string _Address;
        private string _Phone;
        private string _MobilePhone;
        private string _Email;
        private string _BillingName;
        private int? _BillingZipCode;
        private string _BillingSettlement;
        private string _BillingAddress;
        private string _Notes;
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
        public string MobilePhone
        {
            get
            {
                return _MobilePhone;
            }
            set
            {
                if (_MobilePhone == value) return;
                _MobilePhone = value;
                OnPropertyChanged("MobilePhone");
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
        public string BillingName
        {
            get
            {
                return _BillingName;
            }
            set
            {
                if (_BillingName == value) return;
                _BillingName = value;
                OnPropertyChanged("BillingName");
            }
        }
        public int? BillingZipCode
        {
            get
            {
                return _BillingZipCode;
            }
            set
            {
                if (_BillingZipCode == value) return;
                _BillingZipCode = value;
                OnPropertyChanged("BillingZipCode");
            }
        }
        public string BillingSettlement
        {
            get
            {
                return _BillingSettlement;
            }
            set
            {
                if (_BillingSettlement == value) return;
                _BillingSettlement = value;
                OnPropertyChanged("BillingSettlement");
            }
        }
        public string BillingAddress
        {
            get
            {
                return _BillingAddress;
            }
            set
            {
                if (_BillingAddress == value) return;
                _BillingAddress = value;
                OnPropertyChanged("BillingAddress");
            }
        }
        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                if (_Notes == value) return;
                _Notes = value;
                OnPropertyChanged("Notes");
            }
        }
    }
}
