using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MedicalAdministrationSystem.Views.Statistics
{
    public partial class Interval : ViewExtender
    {
        private TimeSpan Duration;
        public IntervalEnabled intervalEnabled { get; set; } = new IntervalEnabled();
        private IntervalValid intervalValid { get; set; } = new IntervalValid();
        protected internal IntervalVM IntervalVM { get; set; }
        private Action<int> DeleteItems { get; set; }
        public Interval(StatisticsM.Step Item, Action<int> DeleteItems)
        {
            Start(Item, DeleteItems);
        }
        private async void Start(StatisticsM.Step Item, Action<int> DeleteItems)
        {
            await Loading.Show();
            IntervalVM = new IntervalVM(Item, CreateEnabled);
            this.DeleteItems = DeleteItems;
            this.DataContext = IntervalVM;
            InitializeComponent();
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            fixDateEdit.Validate += fixDateEdit_Validate;
            intervalStart.Validate += intervalStart_Validate;
            intervalEnd.Validate += intervalEnd_Validate;
            intervalEnabled.PropertyChanged += IntervalEnabled_PropertyChanged;
        }
        private void IntervalEnabled_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FixDate")
            {
                fixDateEdit.IsReadOnly = !(bool)sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
                fixDateErase.IsEnabled = fixDatePicker.IsEnabled = !fixDateEdit.IsReadOnly;
                if (fixDateEdit.IsReadOnly) fixDateEdit.Clear();
                else IntervalVM.ScalesEnabler(false);
                fixDateEdit.DoValidate();
            }
            else if (e.PropertyName == "IntervalDate")
            {
                intervalStart.IsReadOnly = intervalEnd.IsReadOnly = !(bool)sender.GetType().GetProperty(e.PropertyName).GetValue(sender);
                intervalStartErase.IsEnabled = intervalEndErase.IsEnabled = intervalStartPicker.IsEnabled = intervalEndPicker.IsEnabled = !intervalStart.IsReadOnly;
                if (intervalStart.IsReadOnly)
                {
                    intervalStart.Clear();
                    intervalEnd.Clear();
                }
                else IntervalVM.ScalesEnabler();
                intervalStart.DoValidate();
                intervalEnd.DoValidate();
            }
            else if (e.PropertyName == "Continuous" && (bool)sender.GetType().GetProperty(e.PropertyName).GetValue(sender)) IntervalVM.ScalesEnabler(true);
        }
        private void intervalStart_Validate(object sender, ValidationEventArgs e)
        {
            intervalValid.intervalStart = false;
            if (!intervalEnabled.IntervalDate)
            {
                e.SetError("A mező nincs engedélyezve", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                intervalValid.intervalStart = true;
            }
            else if (e.Value == null)
            {
                e.SetError("A mezőt nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                (sender as DateEdit).EditValue = e.Value;
                IntervalVM.ScalesEnabler(false);
            }
            else if (e.Value as DateTime? > DateTime.Now)
            {
                e.SetError("Nem vehetünk fel időpontot a jövőbe", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                IntervalVM.ScalesEnabler(false);
            }
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                intervalValid.intervalStart = true;
                if (intervalValid.intervalEnd) intervalEnd.EditValue = (DateTime)(sender as DateEdit).EditValue + Duration;
                else intervalEnd.DoValidate();
                IntervalVM.ScalesEnabler();
            }
            CreateEnabled();
        }
        private void intervalEnd_Validate(object sender, ValidationEventArgs e)
        {
            intervalValid.intervalEnd = false;
            if (!intervalEnabled.IntervalDate)
            {
                e.SetError("A mező nincs engedélyezve", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                intervalValid.intervalEnd = true;
            }
            else if (e.Value == null)
            {
                e.SetError("A mezőt nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                (sender as DateEdit).EditValue = e.Value;
                IntervalVM.ScalesEnabler(false);
            }
            else if (e.Value as DateTime? < (DateTime?)intervalStart.EditValue)
            {
                e.SetError("Nem lehet az időpont vége korábban mint a kezdete", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                IntervalVM.ScalesEnabler(false);
            }
            else if (e.Value as DateTime? > DateTime.Now)
            {
                e.SetError("Nem vehetünk fel időpontot a jövőbe", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                IntervalVM.ScalesEnabler(false);
            }
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                intervalValid.intervalEnd = true;
                if (intervalStart.EditValue != null) Duration = (DateTime)intervalEnd.EditValue - (DateTime)intervalStart.EditValue;
                IntervalVM.ScalesEnabler();
            }
            CreateEnabled();
        }
        private void fixDateEdit_Validate(object sender, ValidationEventArgs e)
        {
            intervalValid.fixDateEdit = false;
            if (!intervalEnabled.FixDate)
            {
                e.SetError("A mező nincs engedélyezve", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
                intervalValid.fixDateEdit = true;
            }
            else if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                (sender as DateEdit).EditValue = e.Value;
            }
            else if (e.Value as DateTime? > DateTime.Now)
                e.SetError("Nem vehetünk fel időpontot a jövőbe", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                intervalValid.fixDateEdit = true;
            }
            CreateEnabled();
        }
        private void ClickEvent(object sender, EditValueChangedEventArgs e)
        {
            List<PropertyInfo> prop = intervalEnabled.GetType().GetProperties().Where(p => p.Name != "IsChanged" && p.Name != GetSenderName(sender) && (bool)p.GetValue(intervalEnabled)).ToList();
            if ((bool)(sender as CheckEdit).IsChecked)
                foreach (PropertyInfo item in prop)
                    item.SetValue(intervalEnabled, false);
            else if (prop.Count == 0) IntervalVM.ScalesEnabler(false);
            CreateEnabled();
        }

        private void DeleteClick(object sender, RoutedEventArgs e) => DeleteItems(IntervalVM.Item.Id);
        private void CreateEnabled()
        {
            create.IsEnabled = intervalEnabled.FixDate ? intervalValid.Validate(intervalValid) :
                intervalEnabled.IntervalDate || intervalEnabled.Continuous ? intervalValid.Validate(intervalValid) &&
                IntervalVM.SelectedScaleNotNull() : false;
        }
        public class IntervalEnabled : FormValidate
        {
            private bool _FixDate;
            private bool _IntervalDate;
            private bool _Continuous;
            public bool FixDate
            {
                get
                {
                    return _FixDate;
                }
                set
                {
                    if (_FixDate == value) return;
                    _FixDate = value;
                    OnPropertyChanged("FixDate");
                }
            }
            public bool IntervalDate
            {
                get
                {
                    return _IntervalDate;
                }
                set
                {
                    if (_IntervalDate == value) return;
                    _IntervalDate = value;
                    OnPropertyChanged("IntervalDate");
                }
            }
            public bool Continuous
            {
                get
                {
                    return _Continuous;
                }
                set
                {
                    if (_Continuous == value) return;
                    _Continuous = value;
                    OnPropertyChanged("Continuous");
                }
            }
        }
        private class IntervalValid : FormValidate
        {
            public bool fixDateEdit { get; set; }
            public bool intervalStart { get; set; }
            public bool intervalEnd { get; set; }
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            IntervalVM.Create(intervalEnabled.GetType().GetProperties().Where(p => (bool)p.GetValue(intervalEnabled) && p.Name != "IsChanged").Single().Name);
        }

        private void PopupOpening(object sender, OpenPopupEventArgs e)
        {
            if ((sender as DateEdit).IsReadOnly)
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    ((DateEdit)sender).CancelPopup()));
        }
    }
}
