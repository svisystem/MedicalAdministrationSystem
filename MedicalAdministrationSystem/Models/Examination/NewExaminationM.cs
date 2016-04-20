using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Examination
{
    public class NewExaminationM : NotifyPropertyChanged
    {
        private ObservableCollection<DocumentControlM.ListElement> _ExaminationList = new ObservableCollection<DocumentControlM.ListElement>();
        private List<string> _Treats;
        private string _SelectedTreat;
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
        public List<string> Treats
        {
            get
            {
                return _Treats;
            }
            set
            {
                if (_Treats == value) return;
                _Treats = value;
                OnPropertyChanged("Treats");
            }
        }
        public string SelectedTreat
        {
            get
            {
                return _SelectedTreat;
            }
            set
            {
                if (_SelectedTreat == value) return;
                _SelectedTreat = value;
                OnPropertyChanged("SelectedTreat");
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
