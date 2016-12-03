using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Models.Statistics
{
    public class ChartM : NotifyPropertyChanged
    {
        private UserControl _MainContent;
        private UserControl _AssistContent;
        private UserControl _SecondaryAssistContent;
        private string _SelectedDate;
        private string _MainText;
        private string _LesserText;
        private string _Step;
        private ObservableCollection<Record> _FullRecords;
        private ObservableCollection<Record> _ViewRecords = new ObservableCollection<Record>();
        private ObservableCollection<Record> _SingleRecord = new ObservableCollection<Record>();
        private ObservableCollection<Legend> _Legends = new ObservableCollection<Legend>();
        private ObservableCollection<UserControl> _LegendsContainer = new ObservableCollection<UserControl>();
        public UserControl MainContent
        {
            get
            {
                return _MainContent;
            }
            set
            {
                if (_MainContent == value) return;
                _MainContent = value;
                OnPropertyChanged("MainContent");
            }
        }
        public UserControl AssistContent
        {
            get
            {
                return _AssistContent;
            }
            set
            {
                if (_AssistContent == value) return;
                _AssistContent = value;
                OnPropertyChanged("AssistContent");
            }
        }
        public UserControl SecondaryAssistContent
        {
            get
            {
                return _SecondaryAssistContent;
            }
            set
            {
                if (_SecondaryAssistContent == value) return;
                _SecondaryAssistContent = value;
                OnPropertyChanged("SecondaryAssistContent");
            }
        }
        public string SelectedDate
        {
            get
            {
                return _SelectedDate;
            }
            set
            {
                if (_SelectedDate == value) return;
                _SelectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }
        public string MainText
        {
            get
            {
                return _MainText;
            }
            set
            {
                if (_MainText == value) return;
                _MainText = value;
                OnPropertyChanged("MainText");
            }
        }
        public string LesserText
        {
            get
            {
                return _LesserText;
            }
            set
            {
                if (_LesserText == value) return;
                _LesserText = value;
                OnPropertyChanged("LesserText");
            }
        }
        public string Step
        {
            get
            {
                return _Step;
            }
            set
            {
                if (_Step == value) return;
                _Step = value;
                OnPropertyChanged("Step");
            }
        }
        public ObservableCollection<Record> FullRecords
        {
            get
            {
                return _FullRecords;
            }
            set
            {
                if (_FullRecords == value) return;
                _FullRecords = value;
                OnPropertyChanged("FullRecords");
            }
        }
        public ObservableCollection<Record> ViewRecords
        {
            get
            {
                return _ViewRecords;
            }
            set
            {
                if (_ViewRecords == value) return;
                _ViewRecords = value;
                OnPropertyChanged("ViewRecords");
            }
        }
        public ObservableCollection<Record> SingleRecord
        {
            get
            {
                return _SingleRecord;
            }
            set
            {
                if (_SingleRecord == value) return;
                _SingleRecord = value;
                OnPropertyChanged("SingleRecord");
            }
        }
        public ObservableCollection<Legend> Legends
        {
            get
            {
                return _Legends;
            }
            set
            {
                if (_Legends == value) return;
                _Legends = value;
                OnPropertyChanged("Legends");
            }
        }
        public ObservableCollection<UserControl> LegendsContainer
        {
            get
            {
                return _LegendsContainer;
            }
            set
            {
                if (_LegendsContainer == value) return;
                _LegendsContainer = value;
                OnPropertyChanged("LegendsContainer");
            }
        }
        public class Legend
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Visible { get; set; }
        }
        public class Record
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public int Value1 { get; set; }
            public int? Value2 { get; set; }
        }
    }
}
