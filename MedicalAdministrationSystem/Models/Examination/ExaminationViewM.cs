using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Examination
{
    public class ExaminationViewM : NotifyPropertyChanged
    {
        private ObservableCollection<DocumentControlM.ListElement> _ExaminationList = new ObservableCollection<DocumentControlM.ListElement>();
        private bool _Imported;
        private int _Id;
        private string _ExaminationName;
        private string _ExaminationCode;
        private int _PatientId;
        private DateTime? _ExaminationDate;
        public ObservableCollection<DocumentControlM.ListElement> ExaminationList
        {
            get
            {
                return _ExaminationList;
            }
            set
            {
                if (_ExaminationList == value) return;
                _ExaminationList = value;
                OnPropertyChanged("ExaminationList");
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
        public string ExaminationName
        {
            get
            {
                return _ExaminationName;
            }
            set
            {
                if (_ExaminationName == value) return;
                _ExaminationName = value;
                OnPropertyChanged("ExaminationName");
            }
        }
        public string ExaminationCode
        {
            get
            {
                return _ExaminationCode;
            }
            set
            {
                if (_ExaminationCode == value) return;
                _ExaminationCode = value;
                OnPropertyChanged("ExaminationCode");
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
        public DateTime? ExaminationDate
        {
            get
            {
                return _ExaminationDate;
            }
            set
            {
                if (_ExaminationDate == value) return;
                _ExaminationDate = value;
                OnPropertyChanged("ExaminationDate");
            }
        }
    }
}
