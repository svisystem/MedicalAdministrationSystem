using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models
{
    public class SelectedPatientM : NotifyPropertyChanged
    {
        private int _Id;
        private string _Name;
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
    }
}
