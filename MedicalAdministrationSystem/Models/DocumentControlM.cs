using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models
{
    public class DocumentControlM : NotifyPropertyChanged
    {
        public ObservableCollection<ListElement> _List;
        private ListElement _Selected;
        private int _PatientId;
        private List<int> _Erased = new List<int>();

        public ObservableCollection<ListElement> List
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
        public List<int> Erased
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
        public class ListElement : NotifyPropertyChanged
        {
            private int _Id;
            private int? _DBId;
            private UserControl _Button;
            private MemoryStream _File;
            private string _FileType;
            private string _ButtonType;

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
            public int? DBId
            {
                get
                {
                    return _DBId;
                }
                set
                {
                    if (_DBId == value) return;
                    _DBId = value;
                    OnPropertyChanged("DBId");
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
            public MemoryStream File
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
            public string FileType
            {
                get
                {
                    return _FileType;
                }
                set
                {
                    if (_FileType == value) return;
                    _FileType = value;
                    OnPropertyChanged("FileType");
                }
            }
            public string ButtonType
            {
                get
                {
                    return _ButtonType;
                }
                set
                {
                    if (_ButtonType == value) return;
                    _ButtonType = value;
                    OnPropertyChanged("ButtonType");
                }
            }
        }
    }
}
