using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Statistics.Service
{
    public class ServiceSelectorM : NotifyPropertyChanged
    {
        private bool _ButtonEnabled;
        private ObservableCollection<Service> _Services = new ObservableCollection<Service>();
        public bool ButtonEnabled
        {
            get
            {
                return _ButtonEnabled;
            }
            set
            {
                if (_ButtonEnabled == value) return;
                _ButtonEnabled = value;
                OnPropertyChanged("ButtonEnabled");
            }
        }
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
        public class Service : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
            private bool? _Enabled;
            private UserControl _Button;
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
            public bool? Enabled
            {
                get
                {
                    return _Enabled;
                }
                set
                {
                    if (_Enabled == value) return;
                    _Enabled = value;
                    OnPropertyChanged("Enabled");
                }
            }
            public UserControl Button
            {
                get
                {
                    return _Button;
                }
                set
                {
                    if (_Button == value) return;
                    _Button = value;
                    OnPropertyChanged("Button");
                }
            }
        }
    }
}
