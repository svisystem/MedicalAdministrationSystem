using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Billing
{
    public class CreateBillM : NotifyPropertyChanged
    {
        private ObservableCollection<Service> _Services = new ObservableCollection<Service>();
        private ObservableCollection<PrintItem> _PrintList = new ObservableCollection<PrintItem>();
        private ObservableCollection<string> _CompaniesView = new ObservableCollection<string>();
        private List<Company> _Companies = new List<Company>();
        private Service _SelectedService;
        private PrintItem _SelectedPrintItem;
        private string _SelectedCompany;
        private string _Code;
        private int _Price;
        private bool _From;
        public ObservableCollection<Service> Services
        {
            get
            {
                return _Services;
            }
            set
            {
                if (_Services == value) return;
                _Services = value;
                OnPropertyChanged("Services");
            }
        }
        public ObservableCollection<PrintItem> PrintList
        {
            get
            {
                return _PrintList;
            }
            set
            {
                if (_PrintList == value) return;
                _PrintList = value;
                OnPropertyChanged("PrintList");
            }
        }
        public ObservableCollection<string> CompaniesView
        {
            get
            {
                return _CompaniesView;
            }
            set
            {
                if (_CompaniesView == value) return;
                _CompaniesView = value;
                OnPropertyChanged("CompaniesView");
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
        public Service SelectedService
        {
            get
            {
                return _SelectedService;
            }
            set
            {
                if (_SelectedService == value) return;
                _SelectedService = value;
                OnPropertyChanged("SelectedService");
            }
        }
        public PrintItem SelectedPrintItem
        {
            get
            {
                return _SelectedPrintItem;
            }
            set
            {
                if (_SelectedPrintItem == value) return;
                _SelectedPrintItem = value;
                OnPropertyChanged("SelectedPrintItem");
            }
        }
        public string SelectedCompany
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
        public int Price
        {
            get
            {
                return _Price;
            }
            set
            {
                if (_Price == value) return;
                _Price = value;
                OnPropertyChanged("Price");
            }
        }
        public bool From
        {
            get
            {
                return _From;
            }
            set
            {
                if (_From == value) return;
                _From = value;
                OnPropertyChanged("From");
            }
        }
        public class Service : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
            private int _Vat;
            private int _Price;
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
            public int Vat
            {
                get
                {
                    return _Vat;
                }
                set
                {
                    if (_Vat == value) return;
                    _Vat = value;
                    OnPropertyChanged("Vat");
                }
            }
            public int Price
            {
                get
                {
                    return _Price;
                }
                set
                {
                    if (_Price == value) return;
                    _Price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        public class PrintItem : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
            private int _Quantity;
            private int _QuantityPrice;
            private int _PriceWithoutVat;
            private int _Vat;
            private int _VatPrice;
            private int _PriceWithVat;
            private int _Price;
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
            public int Quantity
            {
                get
                {
                    return _Quantity;
                }
                set
                {
                    if (_Quantity == value) return;
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
            public int QuantityPrice
            {
                get
                {
                    return _QuantityPrice;
                }
                set
                {
                    if (_QuantityPrice == value) return;
                    _QuantityPrice = value;
                    OnPropertyChanged("QuantityPrice");
                }
            }
            public int PriceWithoutVat
            {
                get
                {
                    return _PriceWithoutVat;
                }
                set
                {
                    if (_PriceWithoutVat == value) return;
                    _PriceWithoutVat = value;
                    OnPropertyChanged("PriceWithoutVat");
                }
            }
            public int Vat
            {
                get
                {
                    return _Vat;
                }
                set
                {
                    if (_Vat == value) return;
                    _Vat = value;
                    OnPropertyChanged("Vat");
                }
            }
            public int VatPrice
            {
                get
                {
                    return _VatPrice;
                }
                set
                {
                    if (_VatPrice == value) return;
                    _VatPrice = value;
                    OnPropertyChanged("VatPrice");
                }
            }
            public int PriceWithVat
            {
                get
                {
                    return _PriceWithVat;
                }
                set
                {
                    if (_PriceWithVat == value) return;
                    _PriceWithVat = value;
                    OnPropertyChanged("PriceWithVat");
                }
            }
            public int Price
            {
                get
                {
                    return _Price;
                }
                set
                {
                    if (_Price == value) return;
                    _Price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        public class Company : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
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
        public class CompanyData : NotifyPropertyChanged
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
}
