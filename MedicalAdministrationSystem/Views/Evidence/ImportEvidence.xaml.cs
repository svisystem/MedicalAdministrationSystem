using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace MedicalAdministrationSystem.Views.Evidence
{
    public partial class ImportEvidence : ViewExtender
    {
        protected internal ImportEvidenceVM ImportEvidenceVM { get; set; }
        private ImportEvidenceValid importEvidenceValid { get; set; }
        public ImportEvidence()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            importEvidenceValid = new ImportEvidenceValid();
            ImportEvidenceVM = new ImportEvidenceVM(GetList);
            this.DataContext = ImportEvidenceVM;
            InitializeComponent();
            button = Save;
            ImportEvidenceVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => importEvidenceValid.Validate(importEvidenceValid)), SetEnabledSave, SetReadOnlyFields);
            validatorClass = importEvidenceValid;
            ConnectValidators();
            await Loading.Hide();
        }
        private void ConnectValidators()
        {
            date.Validate += date_Validate;
        }
        protected internal bool Dirty()
        {
            return ImportEvidenceVM.VMDirty();
        }
        private void date_Validate(object sender, ValidationEventArgs e)
        {
            importEvidenceValid.date = false;
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
                importEvidenceValid.date = true;
            }
        }
        private void SetEnabledSave(bool enabled)
        {
            button.IsEnabled = enabled;
        }
        private void SetReadOnlyFields(bool enabled)
        {
            date.IsReadOnly = enabled;
            dateClear.IsEnabled = !enabled;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            ImportEvidenceVM.ExecuteQuestion();
        }
        private ObservableCollection<SelectedPatientM.ExaminationItem> GetList()
        {
            return associatedExaination.ListProperty;
        }
        private class ImportEvidenceValid : FormValidate
        {
            public bool date { get; set; }
        }
    }
}
