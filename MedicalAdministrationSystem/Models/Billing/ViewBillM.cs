using MedicalAdministrationSystem.ViewModels.Utilities;
using System.IO;

namespace MedicalAdministrationSystem.Models.Billing
{
    public class ViewBillM : NotifyPropertyChanged
    {
        private int _Id;
        private MemoryStream _Stream;
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
        public MemoryStream Stream
        {
            get
            {
                return _Stream;
            }
            set
            {
                if (_Stream == value) return;
                _Stream = value;
                OnPropertyChanged("Stream");
            }
        }
    }
}
