using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Billing
{
    public class BillsM : NotifyPropertyChanged
    {
        private ObservableCollection<Bill> _Bills = new ObservableCollection<Bill>();
        private Bill _SelectedBill;
        private int _PatientId;
        public ObservableCollection<Bill> Bills
        {
            get
            {
                return _Bills;
            }
            set
            {
                if (_Bills == value) return;
                _Bills = value;
                OnPropertyChanged("Bills");
            }
        }
        public Bill SelectedBill
        {
            get
            {
                return _SelectedBill;
            }
            set
            {
                if (_SelectedBill == value) return;
                _SelectedBill = value;
                OnPropertyChanged("SelectedBill");
            }
        }
        public int PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                if (_PatientId == value) return;
                _PatientId = value;
                OnPropertyChanged("PatientId");
            }
        }
        public class Bill : NotifyPropertyChanged
        {
            private int _Id;
            private bool _Personal;
            private string _DoctorName;
            private string _Patient;
            private string _Code;
            private string _BillingName;
            private DateTime _DateTime;
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
            public bool Personal
            {
                get
                {
                    return _Personal;
                }
                set
                {
                    if (_Personal == value) return;
                    _Personal = value;
                    OnPropertyChanged("Personal");
                }
            }
            public string DoctorName
            {
                get
                {
                    return _DoctorName;
                }
                set
                {
                    if (_DoctorName == value) return;
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
            public string Patient
            {
                get
                {
                    return _Patient;
                }
                set
                {
                    if (_Patient == value) return;
                    _Patient = value;
                    OnPropertyChanged("Patient");
                }
            }
            public string Code
            {
                get
                {
                    return _Code;
                }
                set
                {
                    if (_Code == value) return;
                    _Code = value;
                    OnPropertyChanged("Code");
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
            public DateTime DateTime
            {
                get
                {
                    return _DateTime;
                }
                set
                {
                    if (_DateTime == value) return;
                    _DateTime = value;
                    OnPropertyChanged("DateTime");
                }
            }
        }
    }
}
