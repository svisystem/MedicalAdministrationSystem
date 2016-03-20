using System.Collections.Generic;
using System.Collections.ObjectModel;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;

namespace MedicalAdministrationSystem.Models.Settings
{
    public class PriviledgesM : NotifyPropertyChanged
    {
        private ObservableCollection<Priviledge> _Priviledges = new ObservableCollection<Priviledge>();
        private List<int> _Erased = new List<int>();
        private List<priviledges_fx> _PriviledgesList = new List<priviledges_fx>();
        public ObservableCollection<Priviledge> Priviledges
        {
            get
            {
                return _Priviledges;
            }
            set
            {
                if (_Priviledges == value) return;
                _Priviledges = value;
                OnPropertyChanged("Priviledges");
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
        
        public List<priviledges_fx> PriviledgesList
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
    }
    public class PriviledgeSelectedRow : NotifyPropertyChanged
    {

        private Priviledge _Selected;
        public Priviledge Selected
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
    }
    public class Priviledge : NotifyPropertyChanged
    {
        private int _IdP;
        private string _NameP;
        private bool _ScheduleP;
        private bool _PatientP;
        private bool _ExaminationP;
        private bool _LabP;
        private bool _EvidenceP;
        private bool _PrescriptionP;
        private bool _BillingP;
        private bool _StatisticP;
        private bool _SettingP;
        private bool _AllSeeP;
        private bool _New;
        private bool _Enabled;
        public int IdP
        {
            get
            {
                return _IdP;
            }
            set
            {
                if (_IdP == value) return;
                _IdP = value;
                OnPropertyChanged("IdP");
            }
        }
        public string NameP
        {
            get
            {
                return _NameP;
            }
            set
            {
                if (_NameP == value) return;
                _NameP = value;
                OnPropertyChanged("NameP");
            }
        }
        public bool ScheduleP
        {
            get
            {
                return _ScheduleP;
            }
            set
            {
                if (_ScheduleP == value) return;
                _ScheduleP = value;
                OnPropertyChanged("ScheduleP");
            }
        }
        public bool PatientP
        {
            get
            {
                return _PatientP;
            }
            set
            {
                if (_PatientP == value) return;
                _PatientP = value;
                OnPropertyChanged("PatientP");
            }
        }
        public bool ExaminationP
        {
            get
            {
                return _ExaminationP;
            }
            set
            {
                if (_ExaminationP == value) return;
                _ExaminationP = value;
                OnPropertyChanged("ExaminationP");
            }
        }
        public bool LabP
        {
            get
            {
                return _LabP;
            }
            set
            {
                if (_LabP == value) return;
                _LabP = value;
                OnPropertyChanged("LabP");
            }
        }
        public bool EvidenceP
        {
            get
            {
                return _EvidenceP;
            }
            set
            {
                if (_EvidenceP == value) return;
                _EvidenceP = value;
                OnPropertyChanged("EvidenceP");
            }
        }
        public bool PrescriptionP
        {
            get
            {
                return _PrescriptionP;
            }
            set
            {
                if (_PrescriptionP == value) return;
                _PrescriptionP = value;
                OnPropertyChanged("PrescriptionP");
            }
        }
        public bool BillingP
        {
            get
            {
                return _BillingP;
            }
            set
            {
                if (_BillingP == value) return;
                _BillingP = value;
                OnPropertyChanged("BillingP");
            }
        }
        public bool StatisticP
        {
            get
            {
                return _StatisticP;
            }
            set
            {
                if (_StatisticP == value) return;
                _StatisticP = value;
                OnPropertyChanged("StatisticP");
            }
        }
        public bool SettingP
        {
            get
            {
                return _SettingP;
            }
            set
            {
                if (_SettingP == value) return;
                _SettingP = value;
                OnPropertyChanged("SettingP");
            }
        }
        public bool AllSeeP
        {
            get
            {
                return _AllSeeP;
            }
            set
            {
                if (_AllSeeP == value) return;
                _AllSeeP = value;
                OnPropertyChanged("AllSeeP");
            }
        }
        public bool New
        {
            get
            {
                return _New;
            }
            set
            {
                if (_New == value) return;
                _New = value;
                OnPropertyChanged("New");
            }
        }
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                if (_Enabled == value) return;
                _Enabled = value;
                OnPropertyChanged("Enabled");
            }
        }
    }
}