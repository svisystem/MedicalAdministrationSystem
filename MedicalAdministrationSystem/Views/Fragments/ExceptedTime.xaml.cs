using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class ExceptedTime : ViewExtender
    {
        protected internal bool Enabler
        {
            set
            {
                Disable(value);
            }
        }
        public SurgeryTimeM.Exception Data { get; set; }
        private ExceptedTimeValid exceptedTimeValid { get; set; }
        private Action Valid { get; set; }
        private Action<int> Delete { get; set; }
        private DateTime RegistrateDate { get; set; }
        private Func<bool, int, DateTime?, DateTime?, bool> Between { get; set; }
        public ExceptedTime(SurgeryTimeM.Exception data, Action Valid, Action<int> Delete, Func<bool, int, DateTime?, DateTime?, bool> Between, DateTime RegistrateDate, bool enable = true)
        {
            this.Data = data;
            this.Valid = Valid;
            this.Delete = Delete;
            this.Between = Between;
            this.RegistrateDate = RegistrateDate;
            this.DataContext = this;
            exceptedTimeValid = new ExceptedTimeValid();
            InitializeComponent();
            ConnectValidators();
            ButtonImage(image);
            Enabler = enable;
        }
        private void Disable(bool enable)
        {
            image.IsEnabled = startDateDrop.IsEnabled = startDateErase.IsEnabled = finishDateDrop.IsEnabled = finishDateErase.IsEnabled = delete.IsEnabled = enable;
            startDate.IsReadOnly = finishDate.IsReadOnly = !enable;
        }
        private void ConnectValidators()
        {
            startDate.Validate += StartTime_Validate;
            finishDate.Validate += FinishTime_Validate;
        }
        bool fromStart = true;
        bool fromFinish = true;
        private void Include(object sender, System.Windows.RoutedEventArgs e)
        {
            Data.Included = !Data.Included;
            ButtonImage(sender as Button);
            startDate.DoValidate();
            finishDate.DoValidate();
        }
        private void StartTime_Validate(object sender, ValidationEventArgs e)
        {
            exceptedTimeValid.Start = false;
            if (e.Value == null)
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if ((DateTime)e.Value < RegistrateDate)
                e.SetError("Nem lehet felvenni időpontot a profil regisztrációjánál korábbra", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (finishDate.EditValue != null && (DateTime)e.Value > (DateTime)finishDate.EditValue)
                e.SetError("A kezdő időpont nem lehet nagyobb a befejezés időpontjánál", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (Between(Data.Included, Data.Id, (DateTime)e.Value, Data.FinishDateTime))
                e.SetError("Ebben az időtartamban már lett felvéve ilyen irányú kivétel", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                exceptedTimeValid.Start = true;
            }
            if (!fromFinish)
            {
                fromStart = true;
                finishDate.DoValidate();
            }
            startDate.EditValue = e.Value;
            fromFinish = false;
            Validate();
        }
        private bool Compare(DateTime first, DateTime second)
        {
            return first.Date == second.Date;
        }
        private void FinishTime_Validate(object sender, ValidationEventArgs e)
        {
            exceptedTimeValid.Finish = false;
            if (e.Value == null)
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (startDate.EditValue != null && (DateTime)e.Value < (DateTime)startDate.EditValue)
                e.SetError("A kezdő időpont nem lehet nagyobb a befejezés időpontjánál", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (Between(Data.Included, Data.Id, Data.StartDateTime, (DateTime)e.Value))
                e.SetError("Ebben az időtartamban már lett felvéve ilyen irányú kivétel", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (startDate.EditValue != null && Data.Included && !Compare((DateTime)startDate.EditValue, (DateTime)e.Value))
                e.SetError("Amennyiben bejelöltük hogy ezt az időintervallumot a munkaórába\nbeleértve értjük, nem lehet több napnyi intervalumot megjelölni.", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                exceptedTimeValid.Finish = true;
            }
            if (!fromStart)
            {
                fromFinish = true;
                startDate.DoValidate();
            }
            finishDate.EditValue = e.Value;
            fromStart = false;
            Validate();
        }
        private void ButtonImage(Button button)
        {
            if (Data.Included)
            {
                button.Content = this.Resources["plus"];
                button.ToolTip = "Beleértve\nA heti rendszerességtől eltérően, ebben az intervallumban is rendel";
            }
            else
            {
                button.Content = this.Resources["minus"];
                button.ToolTip = "Kivéve\nA heti rendszerességtől eltérően, ebben az intervallumban nem rendel";
            }
        }
        private void Validate()
        {
            Data.Valid = exceptedTimeValid.Validate(exceptedTimeValid);
            Valid();
        }
        public class ExceptedTimeValid : FormValidate
        {
            public bool Start { get; set; }
            public bool Finish { get; set; }

        }

        private void DeleteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Delete(Data.Id);
        }
    }
}
