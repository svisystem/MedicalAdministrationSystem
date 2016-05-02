using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Fragments;
using MedicalAdministrationSystem.ViewModels.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace MedicalAdministrationSystem.Views.Evidence
{
    public partial class EditEvidence : ViewExtender
    {
        protected internal EditEvidenceVM EditEvidenceVM { get; set; }
        private EditEvidenceValid editEvidenceValid { get; set; }
        public EditEvidence(bool imported, int ID)
        {
            Start(imported, ID);
        }
        private async void Start(bool imported, int ID)
        {
            await Loading.Show();
            editEvidenceValid = new EditEvidenceValid();
            EditEvidenceVM = new EditEvidenceVM(imported, ID, AddList, GetList, GetErased);
            this.DataContext = EditEvidenceVM;
            InitializeComponent();
            button = Save;
            EditEvidenceVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => editEvidenceValid.Validate(editEvidenceValid)), SetEnabledSave, delegate { });
            validatorClass = editEvidenceValid;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            date.Validate += date_Validate;
        }
        protected internal bool Dirty()
        {
            return EditEvidenceVM.VMDirty();
        }
        private void date_Validate(object sender, ValidationEventArgs e)
        {
            editEvidenceValid.date = false;
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
                editEvidenceValid.date = true;
            }
        }
        private void SetEnabledSave(bool enabled)
        {
            button.IsEnabled = enabled;
        }
        private void save(object sender, RoutedEventArgs e)
        {
            EditEvidenceVM.ExecuteQuestion();
        }
        private class EditEvidenceValid : FormValidate
        {
            public bool date { get; set; }
        }
        private void AddList(SelectedPatientM.ExaminationItem item)
        {
            associatedExaination.EmptyComboBoxEditVM.Add(item);
        }
        private ObservableCollection<SelectedPatientM.ExaminationItem> GetList()
        {
            return associatedExaination.ListProperty;
        }
        private List<EmptyComboBoxEditM.ErasedItem> GetErased()
        {
            return associatedExaination.ErasedProperty;
        }
    }
}
