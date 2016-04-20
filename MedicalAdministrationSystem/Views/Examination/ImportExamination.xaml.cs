using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class ImportExamination : ViewExtender
    {
        protected internal ImportExaminationVM ImportExaminationVM { get; set; }
        private ImportExaminationValid importExaminationValid { get; set; }
        public ImportExamination()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            importExaminationValid = new ImportExaminationValid();
            ImportExaminationVM = new ImportExaminationVM();
            this.DataContext = ImportExaminationVM;
            InitializeComponent();
            ImportExaminationVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => importExaminationValid.Validate(importExaminationValid)), SetEnabledSave, SetReadOnlyFields);
            validatorClass = importExaminationValid;
            button = Save;
            ConnectValidators();
            await Loading.Hide();
        }
        private void ConnectValidators()
        {
            examinationName.Validate += examinationName_Validate;
            examinationDate.Validate += examinationDate_Validate;
        }
        protected internal bool Dirty()
        {
            return ImportExaminationVM.VMDirty();
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
            ForceBindingWithoutEnabledCheck(sender, e);
        }
        private void examinationDate_Validate(object sender, ValidationEventArgs e)
        {
            importExaminationValid.examinationDate = false;
            if (e.Value == null)
            {
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                (sender as DateEdit).EditValue = e.Value;
            }
            else if (e.Value as DateTime? < new DateTime(1900, 1, 1))
                e.SetError("A mező tartalma a megadott értékeken kívülre mutat", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (e.Value as DateTime? > DateTime.Now)
                e.SetError("Nem vehetünk fel vizsgálatot a jövőbe", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                (sender as DateEdit).EditValue = e.Value;
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                importExaminationValid.examinationDate = true;
            }
        }
        private void ExaminationTime_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }
        private void SetEnabledSave(bool enabled)
        {
            button.IsEnabled = enabled;
        }
        private void SetReadOnlyFields(bool enabled)
        {
            examinationName.IsReadOnly = enabled;
            examinationDate.IsReadOnly = enabled;
        }
        private void save(object sender, System.Windows.RoutedEventArgs e)
        {
            ImportExaminationVM.ExecuteMethod();
        }
        private class ImportExaminationValid : FormValidate
        {
            public bool examinationName { get; set; }
            public bool examinationDate { get; set; }
        }
    }
}
