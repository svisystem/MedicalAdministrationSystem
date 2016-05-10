using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Billing
{
    public class CompanyDataM : NotifyPropertyChanged
    {
        private List<zipcode_fx> _FullZipCodeList;
        private List<int> _ViewZipCodeList;
        private List<settlement_fx> _FullSettlementList;
        private List<string> _ViewSettlementList;
        private List<settlementzipcode_st> _SettlementZipSwitch;
        private ObservableCollection<Company> _Companies = new ObservableCollection<Company>();
        private Company _SelectedCompany;
        private List<int> _Erased = new List<int>();
        private Company _Earlier;
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
        public List<int> ViewZipCodeList
        {
            get
            {
                return _ViewZipCodeList;
            }
            set
            {
                if (_ViewZipCodeList == value) return;
                _ViewZipCodeList = value;
                OnPropertyChanged("ViewZipCodeList");
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
        public List<string> ViewSettlementList
        {
            get
            {
                return _ViewSettlementList;
            }
            set
            {
                if (_ViewSettlementList == value) return;
                _ViewSettlementList = value;
                OnPropertyChanged("ViewSettlementList");
            }
        }
        public List<settlementzipcode_st> SettlementZipSwitch
        {
            get
            {
                return _SettlementZipSwitch;
            }
            set
            {
                if (_SettlementZipSwitch == value) return;
                _SettlementZipSwitch = value;
                OnPropertyChanged("SettlementZipSwitch");
            }
        }
        public ObservableCollection<Company> Companies
        {
            get
            {
                return _Companies;
            }
            set
            {
                if (_Companies == value) return;
                _Companies = value;
                OnPropertyChanged("Companies");
            }
        }
        public Company SelectedCompany
        {
            get
            {
                return _SelectedCompany;
            }
            set
            {
                if (_SelectedCompany == value) return;
                _SelectedCompany = value;
                OnPropertyChanged("SelectedCompany");
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
        public Company Earlier
        {
            get
            {
                return _Earlier;
            }
            set
            {
                if (_Earlier == value) return;
                _Earlier = value;
                OnPropertyChanged("Earlier");
            }
        }
        public class Company : NotifyPropertyChanged
        {
            private int? _ID;
            private string _Name;
            private int _ZipCodeId;
            private int? _ViewZipCode;
            private int _SettlementId;
            private string _ViewSettlement;
            private string _Address;
            private string _TaxNumber;
            private string _RegistrationNumber;
            private string _InvoiceNumber;
            private string _Phone;
            private string _Email;
            private string _WebPage;
            public int? ID
            {
                get
                {
                    return _ID;
                }
                set
                {
                    if (_ID == value) return;
                    _ID = value;
                    OnPropertyChanged("ID");
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
            public int? ViewZipCode
            {
                get
                {
                    return _ViewZipCode;
                }
                set
                {
                    if (_ViewZipCode == value) return;
                    _ViewZipCode = value;
                    OnPropertyChanged("ViewZipCode");
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
            public string ViewSettlement
            {
                get
                {
                    return _ViewSettlement;
                }
                set
                {
                    if (_ViewSettlement == value) return;
                    _ViewSettlement = value;
                    OnPropertyChanged("ViewSettlement");
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
            public string TaxNumber
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
            public string RegistrationNumber
            {
                get
                {
                    return _RegistrationNumber;
                }
                set
                {
                    if (_RegistrationNumber == value) return;
                    _RegistrationNumber = value;
                    OnPropertyChanged("RegistrationNumber");
                }
            }
            public string InvoiceNumber
            {
                get
                {
                    return _InvoiceNumber;
                }
                set
                {
                    if (_InvoiceNumber == value) return;
                    _InvoiceNumber = value;
                    OnPropertyChanged("InvoiceNumber");
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
            public string WebPage
            {
                get
                {
                    return _WebPage;
                }
                set
                {
                    if (_WebPage == value) return;
                    _WebPage = value;
                    OnPropertyChanged("WebPage");
                }
            }
        }
    }
}
