using System.Collections.Generic;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class UserAdministrateMDataSet : NotifyPropertyChanged
    {
        private List<priviledges> _PriviledgesList;
        private UserAdministrateMViewElements.UserRow _SelectedRow;
        
        public List<priviledges> PriviledgesList
        {
            get
            {
                return _PriviledgesList;
            }
            set
            {
                if (_PriviledgesList == value) return;
                _PriviledgesList = value;
                OnPropertyChanged("PriviledgesList");
            }
        }
        public UserAdministrateMViewElements.UserRow SelectedRow
        {
            get
            {
                return _SelectedRow;
            }
            set
            {
                if (_SelectedRow == value) return;
                _SelectedRow = value;
                OnPropertyChanged("SelectedRow");
            }
        }
    }
}
