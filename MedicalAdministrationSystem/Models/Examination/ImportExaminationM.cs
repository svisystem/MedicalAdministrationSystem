using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Examination
{
    public class ImportExaminationM : NotifyPropertyChanged
    {
        private ObservableCollection<ListElement> _ExaminationList = new ObservableCollection<ListElement>();
        private ListElement _Selected;
        private string _ExaminationName;
        private DateTime? _ExaminationDate;

        public ObservableCollection<ListElement> ExaminationList
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
        public ListElement Selected
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
        public class ListElement : NotifyPropertyChanged
        {
            private int _Id;
            private UserControl _Button;
            private Stream _File;
            private string _Type;

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
            public Stream File
            {
                get
                {
                    return _File;
                }
                set
                {
                    if (_File == value) return;
                    _File = value;
                    OnPropertyChanged("File");
                }
            }
            public string Type
            {
                get
                {
                    return _Type;
                }
                set
                {
                    if (_Type == value) return;
                    _Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }
    }
}
