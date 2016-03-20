using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class ConnectionM : NotifyPropertyChanged
    {
        private string _HostName;
        private string _PortNumber;
        private string _DatabaseName;
        private string _UserId;
        private string _Password;
        public string HostName
        {
            get
            {
                return _HostName;
            }
            set
            {
                if (_HostName == value) return;
                _HostName = value;
                OnPropertyChanged("HostName");
            }
        }
        public string PortNumber
        {
            get
            {
                return _PortNumber;
            }
            set
            {
                if (_PortNumber == value) return;
                _PortNumber = value;
                OnPropertyChanged("PortNumber");
            }
        }
        public string DatabaseName
        {
            get
            {
                return _DatabaseName;
            }
            set
            {
                if (_DatabaseName == value) return;
                _DatabaseName = value;
                OnPropertyChanged("DatabaseName");
            }
        }
        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                if (_UserId == value) return;
                _UserId = value;
                OnPropertyChanged("UserId");
            }
        }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (_Password == value) return;
                _Password = value;
                OnPropertyChanged("Password");
            }
        }
    }
}
