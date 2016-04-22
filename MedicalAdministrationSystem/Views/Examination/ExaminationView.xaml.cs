using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class ExaminationView : ViewExtender
    {
        protected internal ExaminationViewVM ExaminationViewVM { get; set; }
        private ExaminationViewValid examinationViewValid { get; set; }
        public ExaminationView(bool imported, int ID)
        {
            Start(imported, ID);
        }
        private async void Start(bool imported, int ID)
        {
            await Loading.Show();
            examinationViewValid = new ExaminationViewValid();
            ExaminationViewVM = new ExaminationViewVM(imported, ID, delegate
            { ExaminationViewVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => examinationViewValid.Validate(examinationViewValid)), delegate { }, delegate { }); });
            this.DataContext = ExaminationViewVM;
            InitializeComponent();
            validatorClass = examinationViewValid;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            examinationName.Validate += examinationName_Validate;
            examinationDate.Validate += examinationDate_Validate;
        }
        protected internal bool Dirty()
        {
            return ExaminationViewVM.VMDirty();
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
            examinationViewValid.examinationDate = false;
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
                examinationViewValid.examinationDate = true;
            }
        }
        private void Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }
        private class ExaminationViewValid : FormValidate
        {
            public bool examinationName { get; set; }
            public bool examinationDate { get; set; }
        }
    }
}
