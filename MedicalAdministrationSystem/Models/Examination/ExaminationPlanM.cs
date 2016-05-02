using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Examination
{
    public class ExaminationPlanM : NotifyPropertyChanged
    {
        private ObservableCollection<Service> _Services = new ObservableCollection<Service>();
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
            private int _Vat;
            private int _Price;
            private string _Details;
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
        }
    }
}
