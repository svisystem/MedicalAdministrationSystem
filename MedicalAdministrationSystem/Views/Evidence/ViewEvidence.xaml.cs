using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;

namespace MedicalAdministrationSystem.Views.Evidence
{
    public partial class ViewEvidence : ViewExtender
    {
        protected internal ViewEvidenceVM ViewEvidenceVM { get; set; }
        private ViewEvidenceValid viewEvidenceValid { get; set; }
        public ViewEvidence(bool imported, int ID)
        {
            Start(imported, ID);
        }
        private async void Start(bool imported, int ID)
        {
            await Loading.Show();
            viewEvidenceValid = new ViewEvidenceValid();
            ViewEvidenceVM = new ViewEvidenceVM(imported, ID, AddList);
            this.DataContext = ViewEvidenceVM;
            InitializeComponent();
            associatedExaination.EraseEnabled = false;
            associatedExaination.ImportEnable = false;
            ViewEvidenceVM.ParameterPassingAfterLoad(ref content, new Func<bool>(() => viewEvidenceValid.Validate(viewEvidenceValid)), delegate { }, delegate { });
            validatorClass = viewEvidenceValid;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            date.Validate += date_Validate;
        }
        protected internal bool Dirty()
        {
            return ViewEvidenceVM.VMDirty();
        }
        private void date_Validate(object sender, ValidationEventArgs e)
        {
            viewEvidenceValid.date = false;
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
                viewEvidenceValid.date = true;
            }
        }
        private class ViewEvidenceValid : FormValidate
        {
            public bool date { get; set; }
        }
        private void AddList(SelectedPatientM.ExaminationItem item)
        {
            associatedExaination.EmptyComboBoxEditVM.Add(item);
        }
    }
}
