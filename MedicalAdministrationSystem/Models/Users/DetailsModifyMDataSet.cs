using System.Collections.Generic;
using System.ComponentModel;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Users
{
    public class DetailsModifyMDataSet : NotifyPropertyChanged
    {
        private int _UserID;
        private List<gender_fx> _FullGenderList;
        private List<string> _ViewGenderList;
        private List<string> _ViewBirthPlaceList;
        private List<zipcode_fx> _FullZipCodeList;
        private List<int> _ViewZipCodeList;
        private List<settlement_fx> _FullSettlementList;
        private List<string> _ViewSettlementList;
        private List<settlementzipcode_st> _SettlementZipSwitch;
        
        public int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                if (_UserID == value) return;
                _UserID = value;
                OnPropertyChanged("UserID");
            }
        }
        public List<gender_fx> FullGenderList
        {
            get
            {
                return _FullGenderList;
            }
            set
            {
                if (_FullGenderList == value) return;
                _FullGenderList = value;
                OnPropertyChanged("FullGenderList");
            }
        }
        public List<string> ViewGenderList
        {
            get
            {
                return _ViewGenderList;
            }
            set
            {
                if (_ViewGenderList == value) return;
                _ViewGenderList = value;
                OnPropertyChanged("ViewGenderList");
            }
        }
        public List<string> ViewBirthPlaceList
        {
            get
            {
                return _ViewBirthPlaceList;
            }
            set
            {
                if (_ViewBirthPlaceList == value) return;
                _ViewBirthPlaceList = value;
                OnPropertyChanged("ViewBirthPlaceList");
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
    }
}
