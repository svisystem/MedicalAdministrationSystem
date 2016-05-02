using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Patients;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels
{
    public class SelectedPatientVM : VMExtender
    {
        public SelectedPatientM SelectedPatientM { get; set; }
        private Action Selected { get; set; }
        private Action Focused { get; set; }
        protected internal SelectedPatientVM(Action Selected, Action Focused)
        {
            SelectedPatientM = new SelectedPatientM();
            this.Selected = Selected;
            this.Focused = Focused;
        }
        protected internal void Loaded(int id, string name)
        {
            if (SelectedPatientM.Id != id)
            {
                SelectedPatientM.Id = id;
                SelectedPatientM.Name = name;
                MenuItemChanger(true);
                EraseAll();
            }
        }
        protected internal async void PatientDetailsModify()
        {
            if (GlobalVM.StockLayout.actualContent.Content.GetType() != typeof(PatientDetails))
            {
                await Loading.Show();
                new FormChecking(Ok, () => { }, true);
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
                FormChecking fc = new FormChecking(OkMethod, () => { }, false);
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
            new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.patientsTBI);
            Selected();
        }
        protected internal void Add(bool Imported, int Id, string Name, string Code, DateTime Date)
        {
            if (SelectedPatientM.List.Any(l => l.Imported == Imported && l.Id == Id))
            {
                dialog = new Dialog(false, "Egyezés", () => { });
                dialog.content = new Views.Dialogs.TextBlock("A hozzáadni kívánt vizsgálat már szerepel a kijelöltek között");
                dialog.Start();
            }
            else
            {
                SelectedPatientM.List.Add(new SelectedPatientM.ExaminationItem()
                {
                    Imported = Imported,
                    Id = Id,
                    Name = Name,
                    Code = Code,
                    Date = Date
                });
                Focused();
            }
        }
        protected internal void EraseItem()
        {
            SelectedPatientM.List.Remove(SelectedPatientM.SelectedItem);
        }
        protected internal void EraseAll()
        {
            SelectedPatientM.List.Clear();
        }
        protected internal int Count()
        {
            return SelectedPatientM.List.Count;
        }
        protected internal ObservableCollection<SelectedPatientM.ExaminationItem> GetList()
        {
            return SelectedPatientM.List;
        }
        protected internal void SetList(ObservableCollection<SelectedPatientM.ExaminationItem> outerList)
        {
            if (Count() != 0)
            {
                dialog = new Dialog(false, "Biztosan megváltoztatja a listát?", () =>
                {
                    SelectedPatientM.List = outerList;
                    Focused();
                }, () => { }, true);
                dialog.content = new Views.Dialogs.TextBlock("A kiválasztott vizsgálatok listája nem üres.\n" +
                    "Biztosan felülrja?");
                dialog.Start();
            }
            SelectedPatientM.List = outerList;
            Focused();
        }
    }
}
