using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Fragments;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Fragments
{
    public class EmptyComboBoxEditVM : VMExtender
    {
        protected internal ObservableCollection<SelectedPatientM.ExaminationItem> ListPropertyVM
        {
            get { return EmptyComboBoxEditM.List; }
            set { EmptyComboBoxEditM.List = value; }
        }
        protected internal List<EmptyComboBoxEditM.ErasedItem> ErasedProperty
        {
            get { return EmptyComboBoxEditM.Erased; }
            set { EmptyComboBoxEditM.Erased = value; }
        }
        private SelectedPatient SelectedPatient { get; set; }
        public EmptyComboBoxEditM EmptyComboBoxEditM { get; set; }
        protected internal EmptyComboBoxEditVM()
        {
            EmptyComboBoxEditM = new EmptyComboBoxEditM();
            SelectedPatient = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient);
        }
        protected internal void EraseItem()
        {
            EmptyComboBoxEditM.Erased.Add(new EmptyComboBoxEditM.ErasedItem()
            {
                Id = EmptyComboBoxEditM.SelectedItem.Id,
                Imported = EmptyComboBoxEditM.SelectedItem.Imported
            });
            EmptyComboBoxEditM.List.Remove(EmptyComboBoxEditM.SelectedItem);
        }
        protected internal void EraseAll()
        {
            EmptyComboBoxEditM.List.Clear();
        }
        protected internal bool ImportCount()
        {
            return EmptyComboBoxEditM.List.Count != 0 && SelectedPatient.SelectedPatientVM.GetList().Count != 0 ? true : false;
        }
        protected internal void Import()
        {
            if (SelectedPatient.SelectedPatientVM.GetList().Count != 0)
                if (EmptyComboBoxEditM.List.Count != 0)
                {
                    dialog = new Dialog(false, "Módosítás megerősítése", ImportProgress, () => { }, true);
                    dialog.content = new Views.Dialogs.TextBlock("A Státuszhoz rendelt vizsgálatok listája nem üres,\n" +
                        "Az újonan hozzárendelni kívánt elemekkel \"összefésüljük\", így az azonos hivatkozások nem lesznek duplán hozzárendelve\n" +
                        "Biztosan végrehajtja?");
                    dialog.Start();
                }
                else EmptyComboBoxEditM.List =
                        new ObservableCollection<SelectedPatientM.ExaminationItem>
                        (SelectedPatient.SelectedPatientVM.GetList().Select(a =>
                        new SelectedPatientM.ExaminationItem
                        {
                            Imported = a.Imported,
                            Id = a.Id,
                            Name = a.Name,
                            Code = a.Code,
                            Date = a.Date
                        }).ToList());
        }
        private void ImportProgress()
        {
            foreach (SelectedPatientM.ExaminationItem item in SelectedPatient.SelectedPatientVM.GetList())
            {
                if (!EmptyComboBoxEditM.List.Any(i => i.Imported == item.Imported && i.Id == item.Id)) EmptyComboBoxEditM.List.Add(
                    new SelectedPatientM.ExaminationItem
                    {
                        Imported = item.Imported,
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                        Date = item.Date
                    });
            }
        }
        protected internal void Export()
        {
            SelectedPatient.SelectedPatientVM.SetList(
                new ObservableCollection<SelectedPatientM.ExaminationItem>
                (EmptyComboBoxEditM.List.Select(a =>
                new SelectedPatientM.ExaminationItem
                {
                    Imported = a.Imported,
                    Id = a.Id,
                    Name = a.Name,
                    Code = a.Code,
                    Date = a.Date
                }).ToList()));
        }
        protected internal void Add(SelectedPatientM.ExaminationItem item)
        {

            EmptyComboBoxEditM.List.Add(item);
        }
    }
}