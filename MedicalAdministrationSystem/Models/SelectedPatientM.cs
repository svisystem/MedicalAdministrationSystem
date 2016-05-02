using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models
{
    public class SelectedPatientM : NotifyPropertyChanged
    {
        private int _Id;
        private string _Name;
        private ObservableCollection<ExaminationItem> _List = new ObservableCollection<ExaminationItem>();
        private ExaminationItem _SelectedItem;
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
        public ObservableCollection<ExaminationItem> List
        {
            get
            {
                return _List;
            }
            set
            {
                if (_List == value) return;
                _List = value;
                OnPropertyChanged("List");
            }
        }
        public ExaminationItem SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem == value) return;
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
        public class ExaminationItem : NotifyPropertyChanged
        {
            private bool _Imported;
            private int _Id;
            private string _Name;
            private string _Code;
            private DateTime _Date;

            public bool Imported
            {
                get
                {
                    return _Imported;
                }
                set
                {
                    if (_Imported == value) return;
                    _Imported = value;
                    OnPropertyChanged("Imported");
                }
            }
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
            public DateTime Date
            {
                get
                {
                    return _Date;
                }
                set
                {
                    if (_Date == value) return;
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
    }
}
