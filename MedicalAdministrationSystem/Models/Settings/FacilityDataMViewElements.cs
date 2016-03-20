using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class FacilityDataMViewElements : NotifyPropertyChanged
    {
        private string _Name;
        private int? _ZipCode;
        private string _Settlement;
        private string _Address;
        private string _TaxNumber;
        private string _RegistrationNumber;
        private string _InvoiceNumber;
        private string _Phone;
        private string _Email;
        private string _WebPage;
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
        public int? ZipCode
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
