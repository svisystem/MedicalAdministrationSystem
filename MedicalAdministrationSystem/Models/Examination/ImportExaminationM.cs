using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Examination
{
    public class ImportExaminationM : NotifyPropertyChanged
    {
        private ObservableCollection<object> _ExaminationList = new ObservableCollection<object>();

        public ObservableCollection<object> ExaminationList
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
    }
}
