using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class ExaminationEdit : ViewExtender
    {
        protected internal ExaminationEditVM ExaminationEditVM { get; set; }
        private ExaminationEditValid examinationEditValid { get; set; }
        public ExaminationEdit(bool imported, int ID)
        {
            Start(imported, ID);
        }
        private async void Start(bool imported, int ID)
        {
            await Loading.Show();
            examinationEditValid = new ExaminationEditValid();
            ExaminationEditVM = new ExaminationEditVM(imported, ID);
            this.DataContext = ExaminationEditVM;
            InitializeComponent();
            button = Save;
            ExaminationEditVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => examinationEditValid.Validate(examinationEditValid)), SetEnabledSave, delegate { });
            validatorClass = examinationEditValid;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            examinationName.Validate += examinationName_Validate;
            examinationDate.Validate += examinationDate_Validate;
        }
        protected internal bool Dirty()
        {
            return ExaminationEditVM.VMDirty();
        }
        protected internal void examinationName_Validate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false, null);
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            }
        }
        private void examinationDate_Validate(object sender, ValidationEventArgs e)
        {
            examinationEditValid.examinationDate = false;
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            }
            else if (e.Value as DateTime? < new DateTime(1900, 1, 1))
                e.SetError("A mező tartalma a megadott értékeken kívülre mutat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (e.Value as DateTime? > DateTime.Now)
                e.SetError("Nem vehetünk fel vizsgálatot a jövőbe", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                examinationEditValid.examinationDate = true;
            }
        }
        private void SetEnabledSave(bool enabled)
        {
            button.IsEnabled = enabled;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            ExaminationEditVM.ExecuteMethod();
        }
        private class ExaminationEditValid : FormValidate
        {
            public bool examinationName { get; set; }
            public bool examinationDate { get; set; }
        }
    }
}
