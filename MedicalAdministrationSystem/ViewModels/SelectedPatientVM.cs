using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Patients;
using System;

namespace MedicalAdministrationSystem.ViewModels
{
    public class SelectedPatientVM
    {
        public SelectedPatientM SelectedPatientM { get; set; }
        private Action Selected { get; set; }
        protected internal SelectedPatientVM(Action Selected)
        {
            SelectedPatientM = new SelectedPatientM();
            this.Selected = Selected;
        }
        protected internal void Loaded(int id, string name)
        {
            SelectedPatientM.Id = id;
            SelectedPatientM.Name = name;
            MenuItemChanger(true);
        }
        protected internal async void PatientDetailsModify()
        {
            if (GlobalVM.StockLayout.actualContent.Content.GetType() != typeof(PatientDetails))
            {
                await Loading.Show();
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
        protected internal async void Question()
        {
            if (GlobalVM.StockLayout.actualContent.Content.GetType() != typeof(PatientList))
            {
                await Loading.Show();
                FormChecking fc = new FormChecking(OkMethod, Dummy, false);
                fc.SpecialQuestion();
            }
            else
            {
                Dispose();
                Selected();
            }
        }
        private void OkMethod()
        {
            Dispose();
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.LoadItem(GlobalVM.StockLayout.patientsTBI);
            Selected();
        }
        private void Dummy() { }
    }
}
