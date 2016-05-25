using DevExpress.Xpf.Scheduler;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Schedule
{
    public class ScheduleM : NotifyPropertyChanged
    {
        public ObservableCollection<Doctor> _Doctors = new ObservableCollection<Doctor>();
        public ObservableCollection<Appointment> _Appointments = new ObservableCollection<Appointment>();
        public AppointmentLabelCollection _Labels = new AppointmentLabelCollection();
        public ObservableCollection<Patient> _Patients = new ObservableCollection<Patient>();
        public ObservableCollection<Doctor> Doctors
        {
            get
            {
                return _Doctors;
            }
            set
            {
                if (_Doctors == value) return;
                _Doctors = value;
                OnPropertyChanged("Doctors");
            }
        }
        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                return _Appointments;
            }
            set
            {
                if (_Appointments == value) return;
                _Appointments = value;
                OnPropertyChanged("Appointments");
            }
        }
        public AppointmentLabelCollection Labels
        {
            get
            {
                return _Labels;
            }
            set
            {
                if (_Labels == value) return;
                _Labels = value;
                OnPropertyChanged("Labels");
            }
        }
        public ObservableCollection<Patient> Patients
        {
            get
            {
                return _Patients;
            }
            set
            {
                if (_Patients == value) return;
                _Patients = value;
                OnPropertyChanged("Patients");
            }
        }
        public class Doctor : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
            private int _Color;
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
            public int Color
            {
                get
                {
                    return _Color;
                }
                set
                {
                    if (_Color == value) return;
                    _Color = value;
                    OnPropertyChanged("Color");
                }
            }
        }

        public class Appointment : NotifyPropertyChanged
        {
            private int _Id;
            private bool _StillNotVisited;
            private string _PatientName;
            private string _PatientTajNumber;
            private DateTime _StartTime;
            private DateTime _EndTime;
            private int? _DoctorId;
            private string _Notes;
            private int _Label;
            private bool _StoreInDB;
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
            public bool StillNotVisited
            {
                get
                {
                    return _StillNotVisited;
                }
                set
                {
                    if (_StillNotVisited == value) return;
                    _StillNotVisited = value;
                    OnPropertyChanged("StillNotVisited");
                }
            }
            public string PatientName
            {
                get
                {
                    return _PatientName;
                }
                set
                {
                    if (_PatientName == value) return;
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
            public string PatientTajNumber
            {
                get
                {
                    return _PatientTajNumber;
                }
                set
                {
                    if (_PatientTajNumber == value) return;
                    _PatientTajNumber = value;
                    OnPropertyChanged("PatientTajNumber");
                }
            }
            public DateTime StartTime
            {
                get
                {
                    return _StartTime;
                }
                set
                {
                    if (_StartTime == value) return;
                    _StartTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
            public DateTime EndTime
            {
                get
                {
                    return _EndTime;
                }
                set
                {
                    if (_EndTime == value) return;
                    _EndTime = value;
                    OnPropertyChanged("EndTime");
                }
            }
            public int? DoctorId
            {
                get
                {
                    return _DoctorId;
                }
                set
                {
                    if (_DoctorId == value) return;
                    _DoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
            public string Notes
            {
                get
                {
                    return _Notes;
                }
                set
                {
                    if (_Notes == value) return;
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
            public int Label
            {
                get
                {
                    return _Label;
                }
                set
                {
                    if (_Label == value) return;
                    _Label = value;
                    OnPropertyChanged("Label");
                }
            }
            public bool StoreInDB
            {
                get
                {
                    return _StoreInDB;
                }
                set
                {
                    if (_StoreInDB == value) return;
                    _StoreInDB = value;
                    OnPropertyChanged("StoreInDB");
                }
            }
        }
        public class Patient : NotifyPropertyChanged
        {
            private int _Id;
            private string _Name;
            private string _TajNumber;
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
            public string TajNumber
            {
                get
                {
                    return _TajNumber;
                }
                set
                {
                    if (_TajNumber == value) return;
                    _TajNumber = value;
                    OnPropertyChanged("TajNumber");
                }
            }
        }
    }
}
