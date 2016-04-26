using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class NewExamination : ViewExtender
    {
        protected internal NewExaminationVM NewExaminationVM { get; set; }
        private NewExaminationValid newExaminationValid { get; set; }
        public NewExamination()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            newExaminationValid = new NewExaminationValid();
            NewExaminationVM = new NewExaminationVM();
            this.DataContext = NewExaminationVM;
            InitializeComponent();
            button = Save;
            NewExaminationVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => newExaminationValid.Validate(newExaminationValid)), SetEnabledSave, SetReadOnlyFields);
            validatorClass = newExaminationValid;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            examinationName.Validate += examinationName_Validate;
            examinationDate.Validate += examinationDate_Validate;
        }
        protected internal bool Dirty()
        {
            return NewExaminationVM.VMDirty();
        }
        protected internal void examinationName_Validate(object sender, ValidationEventArgs e)
        {
            newExaminationValid.examinationName = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else if (!NewExaminationVM.ExaminationCheck(e.Value as string))
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                newExaminationValid.examinationName = true;
            }
            ForceBindingWithoutEnabledCheck(sender, e);
        }
        private void examinationDate_Validate(object sender, ValidationEventArgs e)
        {
            newExaminationValid.examinationDate = false;
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
                newExaminationValid.examinationDate = true;
            }
        }
        private void Spin(object sender, SpinEventArgs e)
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
        }
        private void save(object sender, RoutedEventArgs e)
        {
            NewExaminationVM.ExecuteMethod();
        }
        private class NewExaminationValid : FormValidate
        {
            public bool examinationName { get; set; }
            public bool examinationDate { get; set; }
        }
    }
}
