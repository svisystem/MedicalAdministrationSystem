using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Statistics
{
    public class StatisticsM : NotifyPropertyChanged
    {
        private ObservableCollection<Step> _Steps = new ObservableCollection<Step>();
        public ObservableCollection<Step> Steps
        {
            get
            {
                return _Steps;
            }
            set
            {
                if (_Steps == value) return;
                _Steps = value;
                OnPropertyChanged("Steps");
            }
        }
        public class Step : NotifyPropertyChanged
        {
            private int _Id;
            private UserControl _Item;
            private object _Answer;
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
            public UserControl Item
            {
                get
                {
                    return _Item;
                }
                set
                {
                    if (_Item == value) return;
                    _Item = value;
                    OnPropertyChanged("Item");
                }
            }
            public object Answer
            {
                get
                {
                    return _Answer;
                }
                set
                {
                    if (_Answer == value) return;
                    _Answer = value;
                    OnPropertyChanged("Answer");
                }
            }
        }
    }
}
