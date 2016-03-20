using System.Collections.Generic;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class FacilityDataMDataSet : NotifyPropertyChanged
    {
        private List<zipcode_fx> _FullZipCodeList;
        private List<int> _ViewZipCodeList;
        private List<settlement_fx> _FullSettlementList;
        private List<string> _ViewSettlementList;
        private List<settlementzipcode_st> _SettlementZipSwitch;
        private List<Company> _Companies;
        private Company _SelectedCompany;

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
        public List<Company> Companies
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
        public class Company : NotifyPropertyChanged
        {
            private int _ID;
            private string _Name;
            public int ID
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
        }
    }
}
