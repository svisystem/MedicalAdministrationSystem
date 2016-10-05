using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.Models.Statistics
{
    public class IntervalM : NotifyPropertyChanged
    {
        private ObservableCollection<Scale> _Scales = new ObservableCollection<Scale>();
        private Scale _SelectedScale;
        private DateTime? _FixDate;
        private DateTime? _IntervalStart;
        private DateTime? _IntervalEnd;
        public ObservableCollection<Scale> Scales
        {
            get
            {
                return _Scales;
            }
            set
            {
                if (_Scales == value) return;
                _Scales = value;
                OnPropertyChanged("Scales");
            }
        }
        public Scale SelectedScale
        {
            get
            {
                return _SelectedScale;
            }
            set
            {
                if (_SelectedScale == value) return;
                _SelectedScale = value;
                OnPropertyChanged("SelectedScale");
            }
        }
        public DateTime? FixDate
        {
            get
            {
                return _FixDate;
            }
            set
            {
                if (_FixDate == value) return;
                _FixDate = value;
                OnPropertyChanged("SpecifiedDate");
            }
        }
        public DateTime? IntervalStart
        {
            get
            {
                return _IntervalStart;
            }
            set
            {
                if (_IntervalStart == value) return;
                _IntervalStart = value;
                OnPropertyChanged("IntervalStart");
            }
        }
        public DateTime? IntervalEnd
        {
            get
            {
                return _IntervalEnd;
            }
            set
            {
                if (_IntervalEnd == value) return;
                _IntervalEnd = value;
                OnPropertyChanged("IntervalEnd");
            }
        }
        public class Scale : NotifyPropertyChanged
        {
            private int _Id;
            private bool _Enabled;
            private string _Title;
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
            public string Title
            {
                get
                {
                    return _Title;
                }
                set
                {
                    if (_Title == value) return;
                    _Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
    }
}
