using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Evidence
{
    public class EditEvidenceM : NotifyPropertyChanged
    {
        private ObservableCollection<DocumentControlM.ListElement> _EvidenceList = new ObservableCollection<DocumentControlM.ListElement>();
        private bool _Imported;
        private int _Id;
        private string _Code;
        private int _PatientId;
        private DateTime? _Date;
        public ObservableCollection<DocumentControlM.ListElement> EvidenceList
        {
            get
            {
                return _EvidenceList;
            }
            set
            {
                if (_EvidenceList == value) return;
                _EvidenceList = value;
                OnPropertyChanged("EvidenceList");
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
        public DateTime? Date
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
