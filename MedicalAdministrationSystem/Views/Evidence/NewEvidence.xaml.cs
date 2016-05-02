using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace MedicalAdministrationSystem.Views.Evidence
{
    public partial class NewEvidence : ViewExtender
    {
        protected internal NewEvidenceVM NewEvidenceVM { get; set; }
        private NewEvidenceValid newEvidenceValid { get; set; }
        public NewEvidence()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            newEvidenceValid = new NewEvidenceValid();
            NewEvidenceVM = new NewEvidenceVM(GetList);
            this.DataContext = NewEvidenceVM;
            InitializeComponent();
            button = Save;
            NewEvidenceVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => newEvidenceValid.Validate(newEvidenceValid)), SetEnabledSave, new Action<bool>(delegate (bool x) { }));
            validatorClass = newEvidenceValid;
            ConnectValidators();
            await Loading.Hide();
        }
        private void ConnectValidators()
        {
            date.Validate += date_Validate;
        }
        protected internal bool Dirty()
        {
            return NewEvidenceVM.VMDirty();
        }
        private void date_Validate(object sender, ValidationEventArgs e)
        {
            newEvidenceValid.date = false;
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
                newEvidenceValid.date = true;
            }
        }
        private void SetEnabledSave(bool enabled)
        {
            button.IsEnabled = enabled;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            NewEvidenceVM.ExecuteQuestion();
        }
        private ObservableCollection<SelectedPatientM.ExaminationItem> GetList()
        {
            return associatedExaination.ListProperty;
        }
        private class NewEvidenceValid : FormValidate
        {
            public bool date { get; set; }
        }
    }
}
