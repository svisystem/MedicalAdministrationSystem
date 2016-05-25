using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Evidence;
using MedicalAdministrationSystem.Models.Fragments;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Evidence
{
    public class EditEvidenceVM : VMExtender
    {
        public EditEvidenceM EditEvidenceM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        private Action<SelectedPatientM.ExaminationItem> AddList { get; set; }
        private Func<ObservableCollection<SelectedPatientM.ExaminationItem>> GetList { get; set; }
        private Func<List<EmptyComboBoxEditM.ErasedItem>> GetErased { get; set; }
        private int BelongCount = 0;
        protected internal EditEvidenceVM(bool imported, int ID, Action<SelectedPatientM.ExaminationItem> AddList,
            Func<ObservableCollection<SelectedPatientM.ExaminationItem>> GetList,
            Func<List<EmptyComboBoxEditM.ErasedItem>> GetErased)
        {
            this.AddList = AddList;
            this.GetList = GetList;
            this.GetErased = GetErased;
            Start(imported, ID);
        }
        private void Start(bool imported, int ID)
        {
            EditEvidenceM = new EditEvidenceM();
            SelectedPatient = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient);
            EditEvidenceM.Imported = imported;
            EditEvidenceM.Id = ID;
            EditEvidenceM.PatientId = SelectedPatient.SelectedPatientVM.Id();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    if (EditEvidenceM.Imported)
                    {
                        me.evidencedatadocuments.Where(ex => me.importedevidencedatadocuments_st.Where
                        (iex => iex.IdIED == EditEvidenceM.Id).Select(iex => iex.IdEDD).ToList().Any(c => c == ex.IdEDD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEDD, p.FileTypeEDD, p.IdEDD, new MemoryStream(p.DataEDD)));

                        EditEvidenceM.Code = me.importedevidencedata.Where(iex => iex.IdIED == EditEvidenceM.Id).Single().CodeIED;
                        EditEvidenceM.Date = me.importedevidencedata.Where(iex => iex.IdIED == EditEvidenceM.Id).Single().DateTimeIED;

                        foreach (SelectedPatientM.ExaminationItem item in me.examinationdata.Where(ed => ed.IdEX == me.examinationeachimportedevidence_st
                         .Where(eeie => eeie.IdIED == EditEvidenceM.Id).Select(eeie => eeie.IdEX).FirstOrDefault()).ToList()
                            .Select(s => new SelectedPatientM.ExaminationItem()
                            {
                                Imported = false,
                                Id = s.IdEX,
                                Name = me.servicesdata.Where(t => t.IdTD == s.ServiceIdEX).Select(t => t.NameTD).FirstOrDefault(),
                                Code = s.CodeEX,
                                Date = s.DateTimeEX
                            }))
                            AddList(item);

                        foreach (SelectedPatientM.ExaminationItem item in me.importedexaminationdata.Where(ed => ed.IdIEX == me.importedexaminationeachimportedevidence_st
                         .Where(eeie => eeie.IdIED == EditEvidenceM.Id).Select(eeie => eeie.IdIEX).FirstOrDefault()).ToList()
                            .Select(s => new SelectedPatientM.ExaminationItem()
                            {
                                Imported = true,
                                Id = s.IdIEX,
                                Name = s.NameIEX,
                                Code = s.CodeIEX,
                                Date = s.DateTimeIEX
                            }))
                            AddList(item);
                    }

                    else
                    {
                        me.evidencedatadocuments.Where(ex => me.evidencedatadocuments_st.Where
                        (iex => iex.IdED == EditEvidenceM.Id).Select(iex => iex.IdEDD).ToList().Any(c => c == ex.IdEDD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEDD, p.FileTypeEDD, p.IdEDD, new MemoryStream(p.DataEDD)));

                        EditEvidenceM.Code = me.evidencedata.Where(iex => iex.IdED == EditEvidenceM.Id).Single().CodeED;
                        EditEvidenceM.Date = me.evidencedata.Where(iex => iex.IdED == EditEvidenceM.Id).Single().DateTimeED;

                        foreach (SelectedPatientM.ExaminationItem item in me.examinationdata.Where(ed => ed.IdEX == me.examinationeachevidence_st
                         .Where(eeie => eeie.IdED == EditEvidenceM.Id).Select(eeie => eeie.IdEX).FirstOrDefault()).ToList()
                            .Select(s => new SelectedPatientM.ExaminationItem()
                            {
                                Imported = false,
                                Id = s.IdEX,
                                Name = me.servicesdata.Where(t => t.IdTD == s.ServiceIdEX).Select(t => t.NameTD).FirstOrDefault(),
                                Code = s.CodeEX,
                                Date = s.DateTimeEX
                            }))
                            AddList(item);

                        foreach (SelectedPatientM.ExaminationItem item in me.importedexaminationdata.Where(ed => ed.IdIEX == me.importedexaminationeachevidence_st
                         .Where(eeie => eeie.IdED == EditEvidenceM.Id).Select(eeie => eeie.IdIEX).FirstOrDefault()).ToList()
                            .Select(s => new SelectedPatientM.ExaminationItem()
                            {
                                Imported = true,
                                Id = s.IdIEX,
                                Name = s.NameIEX,
                                Code = s.CodeIEX,
                                Date = s.DateTimeIEX
                            }))
                            AddList(item);
                    }
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                BelongCount = GetList().Count;
                EditEvidenceM.AcceptChanges();
            }
            else ConnectionMessage();
            await Utilities.Loading.Hide();
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, EditEvidenceM.EvidenceList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                GetCode = new Func<string>(() => EditEvidenceM.Code),
                Type = false
            };
            DocumentControlVM.Edit(EditEvidenceM.PatientId);
            Loading.RunWorkerAsync();
        }
        protected internal void ExecuteQuestion()
        {
            if (GetList().Count() == 0)
            {
                dialog = new Dialog(false, "Módosítások mentése", ExecuteMethod, () => { }, true);
                dialog.content = new Views.Dialogs.TextBlock("A Státusz bejegyzéshez nem rendelt vizsgálatokat, enélkül szeretné menteni?");
                dialog.Start();
            }
            else ExecuteMethod();
        }
        protected internal void ExecuteMethod()
        {
            dialog = new Dialog(false, "Módosítások mentése", async () =>
            {
                await Utilities.Loading.Show();
                Execute = new BackgroundWorker();
                Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
                Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteComplete);
                Execute.RunWorkerAsync();
            }, () => { }, true);
            dialog.content = new Views.Dialogs.TextBlock("Biztosan elmenti a Státuszban végrehajtott módosításokat?");
            dialog.Start();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                foreach (DocumentControlM.ListElement item in EditEvidenceM.EvidenceList)
                {
                    if (item.DBId != null)
                    {
                        evidencedatadocuments edd = me.evidencedatadocuments.Where(ed => ed.IdEDD == item.DBId).Single();
                        edd.DataEDD = edd.DataEDD != item.File.ToArray() ? item.File.ToArray() : edd.DataEDD;
                        edd.FileTypeEDD = edd.FileTypeEDD != item.FileType ? item.FileType : edd.FileTypeEDD;
                    }
                    else if (item.File != null)
                    {
                        evidencedatadocuments ed = new evidencedatadocuments()
                        {
                            DataEDD = item.File.ToArray(),
                            TypeEDD = item.ButtonType,
                            FileTypeEDD = item.FileType
                        };

                        me.evidencedatadocuments.Add(ed);
                        me.SaveChanges();
                        int ide = ed.IdEDD;

                        if (EditEvidenceM.Imported)
                        {
                            me.importedevidencedatadocuments_st.Add(new importedevidencedatadocuments_st()
                            {
                                IdIED = EditEvidenceM.Id,
                                IdEDD = ide
                            });
                            me.SaveChanges();
                        }
                        else
                        {
                            me.evidencedatadocuments_st.Add(new evidencedatadocuments_st()
                            {
                                IdED = EditEvidenceM.Id,
                                IdEDD = ide
                            });
                            me.SaveChanges();
                        }
                    }
                    me.SaveChanges();
                }

                foreach (int id in DocumentControlVM.Erased())
                {
                    if (EditEvidenceM.Imported)
                        me.importedevidencedatadocuments_st.Remove
                            (me.importedevidencedatadocuments_st.Where(ex => ex.IdEDD == id).Single());

                    else me.evidencedatadocuments_st.Remove
                            (me.evidencedatadocuments_st.Where(ex => ex.IdEDD == id).Single());

                    me.evidencedatadocuments.Remove
                        (me.evidencedatadocuments.Where(ex => ex.IdEDD == id).Single());

                    me.SaveChanges();
                }

                foreach (EmptyComboBoxEditM.ErasedItem erased in GetErased())
                {
                    if (EditEvidenceM.Imported)
                        if (erased.Imported) me.importedexaminationeachimportedevidence_st.Remove(
                            me.importedexaminationeachimportedevidence_st.Where(ieeie => ieeie.IdIEX == erased.Id).FirstOrDefault());
                        else me.examinationeachimportedevidence_st.Remove(
                            me.examinationeachimportedevidence_st.Where(eeie => eeie.IdEX == erased.Id).FirstOrDefault());
                    else if (erased.Imported) me.importedexaminationeachevidence_st.Remove(
                            me.importedexaminationeachevidence_st.Where(ieee => ieee.IdIEX == erased.Id).FirstOrDefault());
                    else me.examinationeachevidence_st.Remove(
                        me.examinationeachevidence_st.Where(eee => eee.IdEX == erased.Id).FirstOrDefault());
                    me.SaveChanges();
                }

                foreach (SelectedPatientM.ExaminationItem exist in GetList())
                {
                    if (EditEvidenceM.Imported)
                        if (exist.Imported)
                        {
                            if (!me.importedexaminationeachimportedevidence_st.Any(ieeie => ieeie.IdIEX == exist.Id))
                                me.importedexaminationeachimportedevidence_st.Add(new importedexaminationeachimportedevidence_st()
                                {
                                    IdIEX = exist.Id,
                                    IdIED = EditEvidenceM.Id
                                });
                        }
                        else
                        {
                            if (!me.examinationeachimportedevidence_st.Any(eeie => eeie.IdEX == exist.Id))
                                me.examinationeachimportedevidence_st.Add(new examinationeachimportedevidence_st()
                                {
                                    IdEX = exist.Id,
                                    IdIED = EditEvidenceM.Id
                                });
                        }
                    else
                    {
                        if (exist.Imported)
                        {
                            if (!me.importedexaminationeachevidence_st.Any(ieee => ieee.IdIEX == exist.Id))
                                me.importedexaminationeachevidence_st.Add(new importedexaminationeachevidence_st()
                                {
                                    IdIEX = exist.Id,
                                    IdED = EditEvidenceM.Id
                                });
                        }
                        else
                        {
                            if (!me.examinationeachevidence_st.Any(eee => eee.IdEX == exist.Id))
                                me.examinationeachevidence_st.Add(new examinationeachevidence_st()
                                {
                                    IdEX = exist.Id,
                                    IdED = EditEvidenceM.Id
                                });
                        }
                    }
                    me.SaveChanges();
                }

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new Views.Dialogs.TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
                new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.evidenceTBI);
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty()
        {
            if (GetErased().Count != 0 && BelongCount != GetList().Count) return true;
            return DocumentControlVM.VMDirty();
        }
    }
}
