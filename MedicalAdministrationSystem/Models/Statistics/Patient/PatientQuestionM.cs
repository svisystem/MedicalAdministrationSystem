using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Statistics.Patient
{
    public class PatientQuestionM : NotifyPropertyChanged
    {
        private ObservableCollection<Choice> _Choices = new ObservableCollection<Choice>();
        public ObservableCollection<Choice> Choices
        {
            get
            {
                return _Choices;
            }
            set
            {
                if (_Choices == value) return;
                _Choices = value;
                OnPropertyChanged("Answers");
            }
        }
        public class Choice : NotifyPropertyChanged
        {
            private int _Id;
            private UserControl _Item;
            private string _Answer;
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
            public string Answer
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
