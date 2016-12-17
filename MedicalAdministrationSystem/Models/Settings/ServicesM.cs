using System.Collections.Generic;
using System.Collections.ObjectModel;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class ServicesM : NotifyPropertyChanged
    {
        private ObservableCollection<Service> _Services = new ObservableCollection<Service>();
        private List<int> _Erased = new List<int>();
        private Service _Selected;
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
        public Service Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (_Selected == value) return;
                _Selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public class Service : NotifyPropertyChanged
        {
            private int _ID;
            private string _Name;
            private int? _Vat;
            private int? _Price;
            private string _Details;
            private bool _New;
            private bool _Valid;
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
            public int? Vat
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
            public int? Price
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
            public string Details
            {
                get
                {
                    return _Details;
                }
                set
                {
                    if (_Details == value) return;
                    _Details = value;
                    OnPropertyChanged("Details");
                }
            }

            public bool New
            {
                get
                {
                    return _New;
                }
                set
                {
                    if (_New == value) return;
                    _New = value;
                    OnPropertyChanged("New");
                }
            }
            public bool Valid
            {
                get
                {
                    return _Valid;
                }
                set
                {
                    if (_Valid == value) return;
                    _Valid = value;
                    OnPropertyChanged("Valid");
                }
            }
        }
    }
}
