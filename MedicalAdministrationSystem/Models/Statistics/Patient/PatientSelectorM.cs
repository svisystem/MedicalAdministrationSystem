using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Statistics.Patient
{
    public class PatientSelectorM : NotifyPropertyChanged
    {
        private bool _ButtonEnabled;
        private ObservableCollection<Patient> _Patients = new ObservableCollection<Patient>();
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
        public ObservableCollection<Patient> Patients
        {
            get
            {
                return _Patients;
            }
            set
            {
                if (_Patients == value) return;
                _Patients = value;
                OnPropertyChanged("Patients");
            }
        }
        public class Patient : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
            private string _Taj;
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
            public string Taj
            {
                get
                {
                    return _Taj;
                }
                set
                {
                    if (_Taj == value) return;
                    _Taj = value;
                    OnPropertyChanged("Taj");
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
