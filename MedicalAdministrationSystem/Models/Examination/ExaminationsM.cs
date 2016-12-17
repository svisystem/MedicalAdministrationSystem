using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Examination
{
    public class ExaminationsM : NotifyPropertyChanged
    {
        private ObservableCollection<Examination> _Examinations = new ObservableCollection<Examination>();
        private Examination _SelectedExamination;
        private List<ErasedItem> _Erased = new List<ErasedItem>();
        private int _PatientId;
        public ObservableCollection<Examination> Examinations
        {
            get
            {
                return _Examinations;
            }
            set
            {
                if (_Examinations == value) return;
                _Examinations = value;
                OnPropertyChanged("Examinations");
            }
        }
        public Examination SelectedExamination
        {
            get
            {
                return _SelectedExamination;
            }
            set
            {
                if (_SelectedExamination == value) return;
                _SelectedExamination = value;
                OnPropertyChanged("SelectedExamination");
            }
        }
        public List<ErasedItem> Erased
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
        public class ErasedItem : NotifyPropertyChanged
        {
            private bool _Imported;
            private int _Id;
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
        }
        public class Examination : NotifyPropertyChanged
        {
            private int _Id;
            private bool _Imported;
            private string _Name;
            private string _Code;
            private DateTime _DateTime;
            private string _DoctorName;
            private int _DocumentCount;
            private bool _Editable;
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
            public int DocumentCount
            {
                get
                {
                    return _DocumentCount;
                }
                set
                {
                    if (_DocumentCount == value) return;
                    _DocumentCount = value;
                    OnPropertyChanged("DocumentCount");
                }
            }
            public bool Editable
            {
                get
                {
                    return _Editable;
                }
                set
                {
                    if (_Editable == value) return;
                    _Editable = value;
                    OnPropertyChanged("Editable");
                }
            }
        }
    }
}
