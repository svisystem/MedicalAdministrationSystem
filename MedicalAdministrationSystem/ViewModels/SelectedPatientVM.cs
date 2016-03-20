using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Patients;

namespace MedicalAdministrationSystem.ViewModels
{
    public class SelectedPatientVM
    {
        public SelectedPatientM SelectedPatientM { get; set; }
        protected internal SelectedPatientVM()
        {
            SelectedPatientM = new SelectedPatientM();
        }
        protected internal void Loaded(int id, string name)
        {
            SelectedPatientM.Id = id;
            SelectedPatientM.Name = name;
            MenuItemChanger(true);
        }
        protected internal void PatientDetailsModify()
        {
            if (GlobalVM.StockLayout.actualContent.Content.GetType() != typeof(PatientDetails))
            {
                new FormChecking(Ok, Dummy, true);
            }
            else if ((GlobalVM.StockLayout.actualContent.Content as PatientDetails).newView)
            {
                new FormChecking(Ok, Dummy, true);
            }
        }
        private void Ok()
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.modifier = true;
            mbe.LoadItem(GlobalVM.StockLayout.patientsTBI);
        }
        protected internal void Dispose()
        {
            GlobalVM.StockLayout.headerContent.Content = null;
            MenuItemChanger(false);
        }
        protected internal int Id()
        {
            return SelectedPatientM.Id;
        }
        private void MenuItemChanger(bool visible)
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.ChangeEnable(GlobalVM.StockLayout.examinationTBI, visible);
            mbe.ChangeEnable(GlobalVM.StockLayout.labTBI, visible);
            mbe.ChangeEnable(GlobalVM.StockLayout.evidenceTBI, visible);
            mbe.ChangeEnable(GlobalVM.StockLayout.prescriptionTBI, visible);
            mbe.ChangeEnable(GlobalVM.StockLayout.billingTBI, visible);
        }
        protected internal void Question()
        {
            FormChecking fc = new FormChecking(OkMethod, Dummy, false);
            fc.SpecialQuestion();
        }
        private void OkMethod()
        {
            Dispose();
            if (GlobalVM.StockLayout.actualContent.Content.GetType() != typeof(PatientList))
            {
                MenuButtonsEnabled mbe = new MenuButtonsEnabled();
                mbe.LoadItem(GlobalVM.StockLayout.patientsTBI);
            }
        }
        private void Dummy() { }
    }
}
