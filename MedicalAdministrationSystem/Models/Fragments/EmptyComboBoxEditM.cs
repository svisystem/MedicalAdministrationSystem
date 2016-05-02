using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Fragments
{
    public class EmptyComboBoxEditM : NotifyPropertyChanged
    {
        private ObservableCollection<SelectedPatientM.ExaminationItem> _List = new ObservableCollection<SelectedPatientM.ExaminationItem>();
        private SelectedPatientM.ExaminationItem _SelectedItem;
        private List<ErasedItem> _Erased = new List<ErasedItem>();
        public ObservableCollection<SelectedPatientM.ExaminationItem> List
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
        public SelectedPatientM.ExaminationItem SelectedItem
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
        public class ErasedItem : NotifyPropertyChanged
        {
            private int _Id;
            private bool _Imported;
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
        }
    }
}
