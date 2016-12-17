using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Evidence
{
    public class EvidencesM : NotifyPropertyChanged
    {
        private ObservableCollection<Evidence> _Evidences = new ObservableCollection<Evidence>();
        private Evidence _SelectedEvidence;
        private List<ErasedItem> _Erased = new List<ErasedItem>();
        private int _PatientId;
        public ObservableCollection<Evidence> Evidences
        {
            get
            {
                return _Evidences;
            }
            set
            {
                if (_Evidences == value) return;
                _Evidences = value;
                OnPropertyChanged("Evidences");
            }
        }
        public Evidence SelectedEvidence
        {
            get
            {
                return _SelectedEvidence;
            }
            set
            {
                if (_SelectedEvidence == value) return;
                _SelectedEvidence = value;
                OnPropertyChanged("SelectedEvidence");
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
        public class Evidence : NotifyPropertyChanged
        {
            private int _Id;
            private bool _Imported;
            private string _Code;
            private DateTime _Date;
            private string _DoctorName;
            private int _DocCount;
            private ObservableCollection<SelectedPatientM.ExaminationItem> _BelongDocs = new ObservableCollection<SelectedPatientM.ExaminationItem>();
            private bool _Scheduled;
            private UserControl _ComboBox;
            private bool _EditEvidence;
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
            public int DocCount
            {
                get
                {
                    return _DocCount;
                }
                set
                {
                    if (_DocCount == value) return;
                    _DocCount = value;
                    OnPropertyChanged("DocCount");
                }
            }
            public ObservableCollection<SelectedPatientM.ExaminationItem> BelongDocs
            {
                get
                {
                    return _BelongDocs;
                }
                set
                {
                    if (_BelongDocs == value) return;
                    _BelongDocs = value;
                    OnPropertyChanged("BelongDocs");
                }
            }
            public bool Scheduled
            {
                get
                {
                    return _Scheduled;
                }
                set
                {
                    if (_Scheduled == value) return;
                    _Scheduled = value;
                    OnPropertyChanged("Scheduled");
                }
            }
            public UserControl ComboBox
            {
                get
                {
                    return _ComboBox;
                }
                set
                {
                    if (_ComboBox == value) return;
                    _ComboBox = value;
                    OnPropertyChanged("ComboBox");
                }
            }
            public bool EditEvidence
            {
                get
                {
                    return _EditEvidence;
                }
                set
                {
                    if (_EditEvidence == value) return;
                    _EditEvidence = value;
                    OnPropertyChanged("EditEvidence");
                }
            }
        }
    }
}
