using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;

namespace MedicalAdministrationSystem.Views.Users
{
    public partial class SurgeryTime : ViewExtender
    {
        protected internal SurgeryTimeVM SurgeryTimeVM { get; set; }
        private SurgeryTimeValid surgeryTimeValid { get; set; }
        public SurgeryTimeEnabled surgeryTimeEnabled { get; set; } = new SurgeryTimeEnabled();
        public SurgeryTime()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            surgeryTimeValid = new SurgeryTimeValid();
            SurgeryTimeVM = new SurgeryTimeVM(SaveButtonValid);
            this.DataContext = SurgeryTimeVM;
            InitializeComponent();
            validatorClass = surgeryTimeValid;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            monStartTime.Validate += StartTime_Validate;
            monFinishTime.Validate += FinishTime_Validate;
            tueStartTime.Validate += StartTime_Validate;
            tueFinishTime.Validate += FinishTime_Validate;
            wedStartTime.Validate += StartTime_Validate;
            wedFinishTime.Validate += FinishTime_Validate;
            thuStartTime.Validate += StartTime_Validate;
            thuFinishTime.Validate += FinishTime_Validate;
            friStartTime.Validate += StartTime_Validate;
            friFinishTime.Validate += FinishTime_Validate;
            satStartTime.Validate += StartTime_Validate;
            satFinishTime.Validate += FinishTime_Validate;
            sunStartTime.Validate += StartTime_Validate;
            sunFinishTime.Validate += FinishTime_Validate;
        }

        private void ChangeDayEnabler(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckEdit ce = this.FindName(GetSenderName(sender) + "Check") as CheckEdit;
            ce.IsChecked = !ce.IsChecked;
            (this.FindName(GetSenderName(sender) + "StartTime") as DateEdit).DoValidate();
            (this.FindName(GetSenderName(sender) + "FinishTime") as DateEdit).DoValidate();
            SaveButtonValid();
        }
        bool fromStart = true;
        bool fromFinish = true;
        private void StartTime_Validate(object sender, ValidationEventArgs e)
        {
            surgeryTimeValid.GetType().GetProperty(GetSenderName(sender).Remove(8)).SetValue(surgeryTimeValid, false);
            if ((this.FindName(GetSenderName(sender).Remove(3) + "Check") as CheckEdit).IsChecked == false)
            {
                e.SetError("Amennyiben nincs engedélyezve az adott nap, nem kell kitölteni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                surgeryTimeValid.GetType().GetProperty(GetSenderName(sender).Remove(8)).SetValue(surgeryTimeValid, true);
            }
            else if (e.Value == null)
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if ((this.FindName(GetSenderName(sender).Remove(3) + "FinishTime") as DateEdit).EditValue != null &&
                ((DateTime)e.Value > (DateTime)((this.FindName(GetSenderName(sender).Remove(3) + "FinishTime") as DateEdit).EditValue)))
                e.SetError("A kezdő időpont nem lehet nagyobb a befejezés időpontjánál", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                surgeryTimeValid.GetType().GetProperty(GetSenderName(sender).Remove(8)).SetValue(surgeryTimeValid, true);

            }
            if (!fromFinish)
            {
                fromStart = true;
                (this.FindName(GetSenderName(sender).Remove(3) + "FinishTime") as DateEdit).DoValidate();
            } 
            (sender as DateEdit).EditValue = e.Value;
            fromFinish = false;
            SaveButtonValid();
        }
        private void FinishTime_Validate(object sender, ValidationEventArgs e)
        {
            surgeryTimeValid.GetType().GetProperty(GetSenderName(sender).Remove(9)).SetValue(surgeryTimeValid, false);
            if ((this.FindName(GetSenderName(sender).Remove(3) + "Check") as CheckEdit).IsChecked == false)
            {
                e.SetError("Amennyiben nincs engedélyezve az adott nap, nem kell kitölteni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                surgeryTimeValid.GetType().GetProperty(GetSenderName(sender).Remove(9)).SetValue(surgeryTimeValid, true);
            }
            else if (e.Value == null)
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if ((this.FindName(GetSenderName(sender).Remove(3) + "StartTime") as DateEdit).EditValue != null &&
                (DateTime)e.Value < (DateTime)(this.FindName(GetSenderName(sender).Remove(3) + "StartTime") as DateEdit).EditValue)
                e.SetError("A kezdő időpont nem lehet nagyobb a befejezés időpontjánál", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                surgeryTimeValid.GetType().GetProperty(GetSenderName(sender).Remove(9)).SetValue(surgeryTimeValid, true);
            }
            if (!fromStart)
            {
                fromFinish = true;
                (this.FindName(GetSenderName(sender).Remove(3) + "StartTime") as DateEdit).DoValidate();
            }
            (sender as DateEdit).EditValue = e.Value;
            fromStart = false;
            SaveButtonValid();
        }
        protected internal bool Dirty()
        {
            return SurgeryTimeVM.VMDirty();
        }

        private void save(object sender, System.Windows.RoutedEventArgs e)
        {
            SurgeryTimeVM.ExecuteMethod();
        }
        private void SaveButtonValid()
        {
            create.IsEnabled = SurgeryTimeVM.Validate() && surgeryTimeValid.Validate(surgeryTimeValid);
        }
        public class SurgeryTimeEnabled : FormValidate
        {
            private bool _Monday;
            private bool _Tuesday;
            private bool _Wednesday;
            private bool _Thursday;
            private bool _Friday;
            private bool _Saturday;
            private bool _Sunday;
            public bool Monday
            {
                get
                {
                    return _Monday;
                }
                set
                {
                    if (_Monday == value) return;
                    _Monday = value;
                    OnPropertyChanged("Monday");
                }
            }
            public bool Tuesday
            {
                get
                {
                    return _Tuesday;
                }
                set
                {
                    if (_Tuesday == value) return;
                    _Tuesday = value;
                    OnPropertyChanged("Tuesday");
                }
            }
            public bool Wednesday
            {
                get
                {
                    return _Wednesday;
                }
                set
                {
                    if (_Wednesday == value) return;
                    _Wednesday = value;
                    OnPropertyChanged("Wednesday");
                }
            }
            public bool Thursday
            {
                get
                {
                    return _Thursday;
                }
                set
                {
                    if (_Thursday == value) return;
                    _Thursday = value;
                    OnPropertyChanged("Thursday");
                }
            }
            public bool Friday
            {
                get
                {
                    return _Friday;
                }
                set
                {
                    if (_Friday == value) return;
                    _Friday = value;
                    OnPropertyChanged("Friday");
                }
            }
            public bool Saturday
            {
                get
                {
                    return _Saturday;
                }
                set
                {
                    if (_Saturday == value) return;
                    _Saturday = value;
                    OnPropertyChanged("Saturday");
                }
            }
            public bool Sunday
            {
                get
                {
                    return _Sunday;
                }
                set
                {
                    if (_Sunday == value) return;
                    _Sunday = value;
                    OnPropertyChanged("Sunday");
                }
            }
        }
        private class SurgeryTimeValid : FormValidate
        {

            public bool monStart { get; set; }
            public bool tueStart { get; set; }
            public bool wedStart { get; set; }
            public bool thuStart { get; set; }
            public bool friStart { get; set; }
            public bool satStart { get; set; }
            public bool sunStart { get; set; }

            public bool monFinish { get; set; }
            public bool tueFinish { get; set; }
            public bool wedFinish { get; set; }
            public bool thuFinish { get; set; }
            public bool friFinish { get; set; }
            public bool satFinish { get; set; }
            public bool sunFinish { get; set; }
        }

        private void ViewLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SurgeryTimeVM.AfterLoaded();
        }
    }
}
