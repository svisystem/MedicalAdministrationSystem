using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Fragments;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Evidence
{
    public class EvidencesVM : VMExtender
    {
        public EvidencesM EvidencesM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private Action Loaded { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        protected internal EvidencesVM(Action Loaded)
        {
            this.Loaded = Loaded;
            SelectedPatient = GlobalVM.StockLayout.headerContent.Content as SelectedPatient;
            EvidencesM = new EvidencesM();
            EvidencesM.PatientId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).SelectedPatientVM.SelectedPatientM.Id;
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            EvidencesM.Evidences.Clear();
            EvidencesM.Erased.Clear();
            try
            {
                me = new MedicalModel(ConfigurationManager.Connect());
                me.Database.Connection.Open();

                if (me.evidencedata.Where(ex => ex.PatientIdED == EvidencesM.PatientId).Count() != 0)
                    foreach (EvidencesM.Evidence item in me.evidencedata.Where(ex => ex.PatientIdED == EvidencesM.PatientId).ToList()
                        .Select(ex => new EvidencesM.Evidence
                        {
                            Id = ex.IdED,
                            Imported = false,
                            Code = ex.CodeED,
                            Date = ex.DateTimeED,
                            DoctorName = me.userdata.Where(u => u.IdUD == ex.UserDataIdED).FirstOrDefault().NameUD,
                            DocCount = me.evidencedatadocuments_st.Where(exd => exd.IdED == ex.IdED).Count(),
                            EditEvidence = !GlobalVM.GlobalM.JustImportDocuments
                        }))
                        EvidencesM.Evidences.Add(item);

                if (me.importedevidencedata.Where(ex => ex.PatientIED == EvidencesM.PatientId).Count() != 0)
                    foreach (EvidencesM.Evidence item in me.importedevidencedata.Where(ex => ex.PatientIED == EvidencesM.PatientId).ToList()
                        .Select(ex => new EvidencesM.Evidence
                        {
                            Id = ex.IdIED,
                            Imported = true,
                            Code = ex.CodeIED,
                            Date = ex.DateTimeIED,
                            DoctorName = me.userdata.Where(u => u.IdUD == ex.UserDataIdIED).FirstOrDefault().NameUD,
                            DocCount = me.importedevidencedatadocuments_st.Where(exd => exd.IdIED == ex.IdIED).Count(),
                            EditEvidence = true
                        }))
                        EvidencesM.Evidences.Add(item);

                foreach (EvidencesM.Evidence item in EvidencesM.Evidences)
                {
                    if (item.Imported)
                    {
                        foreach (SelectedPatientM.ExaminationItem element in me.examinationdata.Where(ed =>
                            me.examinationeachimportedevidence_st.Where(eeie => eeie.IdIED == item.Id)
                            .Select(eeie => eeie.IdEX).ToList().Any(c => c == ed.IdEX)).ToList().Select
                            (ed => new SelectedPatientM.ExaminationItem
                            {
                                Code = ed.CodeEX,
                                Date = ed.DateTimeEX,
                                Id = ed.IdEX,
                                Imported = false,
                                Name = me.servicesdata.Where(t => t.IdTD == ed.ServiceIdEX).FirstOrDefault().NameTD
                            }))
                            item.BelongDocs.Add(element);

                        foreach (SelectedPatientM.ExaminationItem element in me.importedexaminationdata.Where(ed =>
                            me.importedexaminationeachimportedevidence_st.Where(eeie => eeie.IdIED == item.Id)
                            .Select(eeie => eeie.IdIEX).ToList().Any(c => c == ed.IdIEX)).ToList().Select
                            (ed => new SelectedPatientM.ExaminationItem
                            {
                                Code = ed.CodeIEX,
                                Date = ed.DateTimeIEX,
                                Id = ed.IdIEX,
                                Imported = true,
                                Name = ed.NameIEX
                            }))
                            item.BelongDocs.Add(element);
                    }
                    else
                    {
                        foreach (SelectedPatientM.ExaminationItem element in me.examinationdata.Where(ed =>
                            me.examinationeachevidence_st.Where(eeie => eeie.IdED == item.Id)
                            .Select(eeie => eeie.IdEX).ToList().Any(c => c == ed.IdEX)).ToList().Select
                            (ed => new SelectedPatientM.ExaminationItem
                            {
                                Code = ed.CodeEX,
                                Date = ed.DateTimeEX,
                                Id = ed.IdEX,
                                Imported = false,
                                Name = me.servicesdata.Where(t => t.IdTD == ed.ServiceIdEX).FirstOrDefault().NameTD
                            }))
                            item.BelongDocs.Add(element);

                        foreach (SelectedPatientM.ExaminationItem element in me.importedexaminationdata.Where(ed =>
                            me.importedexaminationeachevidence_st.Where(eeie => eeie.IdED == item.Id)
                            .Select(eeie => eeie.IdIEX).ToList().Any(c => c == ed.IdIEX)).ToList().Select
                            (ed => new SelectedPatientM.ExaminationItem
                            {
                                Code = ed.CodeIEX,
                                Date = ed.DateTimeIEX,
                                Id = ed.IdIEX,
                                Imported = true,
                                Name = ed.NameIEX
                            }))
                            item.BelongDocs.Add(element);
                    }
                }
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                foreach (EvidencesM.Evidence row in EvidencesM.Evidences)
                {
                    row.ComboBox = new EmptyComboBoxEditForGrid()
                    {
                        List = row.BelongDocs,
                        Count = row.BelongDocs.Count().ToString()
                    };
                    row.AcceptChanges();
                }
                Loaded();
            }
            else ConnectionMessage();
        }
        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            BackgroundWorker Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteComplete);
            Execute.RunWorkerAsync();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new MedicalModel(ConfigurationManager.Connect());
                me.Database.Connection.Open();

                foreach (EvidencesM.ErasedItem item in EvidencesM.Erased)
                {
                    if (item.Imported)
                    {
                        me.examinationeachimportedevidence_st.RemoveRange
                            (me.examinationeachimportedevidence_st.Where(ex => ex.IdIED == item.Id).ToList());

                        me.importedexaminationeachimportedevidence_st.RemoveRange
                            (me.importedexaminationeachimportedevidence_st.Where(ex => ex.IdIED == item.Id).ToList());

                        me.evidencedatadocuments.RemoveRange
                            (me.evidencedatadocuments.Where(ex => me.importedevidencedatadocuments_st.Where
                            (ied => ied.IdIED == item.Id).Select(ied => ied.IdEDD).ToList().Any(c => c == ex.IdEDD)));

                        me.importedevidencedatadocuments_st.RemoveRange
                            (me.importedevidencedatadocuments_st.Where(ied => ied.IdIED == item.Id));

                        me.importedevidencedata.Remove
                            (me.importedevidencedata.Where(ex => ex.IdIED == item.Id).Single());
                    }

                    else
                    {
                        me.examinationeachevidence_st.RemoveRange
                            (me.examinationeachevidence_st.Where(ex => ex.IdED == item.Id).ToList());

                        me.importedexaminationeachevidence_st.RemoveRange
                            (me.importedexaminationeachevidence_st.Where(ex => ex.IdED == item.Id).ToList());

                        me.evidencedatadocuments.RemoveRange
                            (me.evidencedatadocuments.Where(ex => me.evidencedatadocuments_st.Where
                            (ied => ied.IdED == item.Id).Select(ied => ied.IdEDD).ToList().Any(c => c == ex.IdEDD)));

                        me.evidencedatadocuments_st.RemoveRange
                            (me.evidencedatadocuments_st.Where(ied => ied.IdED == item.Id));

                        me.evidencedata.Remove
                            (me.evidencedata.Where(ex => ex.IdED == item.Id).Single());
                    }
                }

                me.SaveChanges();
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                EvidencesM.Erased.Clear();
                foreach (EvidencesM.Evidence row in EvidencesM.Evidences)
                    row.AcceptChanges();
                EvidencesM.AcceptChanges();

                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        protected internal void EraseMethod()
        {
            dialog = new Dialog(true, "Vizsgálat törlése", Erase, () => { }, true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott Vizsgálathoz tartozó összes adatot?\n" +
                "A Vizsgálat törlése csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private void Erase()
        {
            EvidencesM.Erased.Add(new EvidencesM.ErasedItem()
            {
                Id = EvidencesM.SelectedEvidence.Id,
                Imported = EvidencesM.SelectedEvidence.Imported
            });
            EvidencesM.Evidences.Remove(EvidencesM.Evidences.Where(e => e.Id == EvidencesM.Erased.Last().Id).Single());
        }
        protected internal async void View()
        {
            await Utilities.Loading.Show();
            new FormChecking(() => OkMethod(true), () => { }, true);
        }
        protected internal async void Edit()
        {
            await Utilities.Loading.Show();
            new FormChecking(() => OkMethod(false), () => { }, true);
        }
        private async void OkMethod(bool which)
        {
            await new MenuButtonsEnabled()
            {
                modifier = which,
                Id = EvidencesM.SelectedEvidence.Id,
                imported = EvidencesM.SelectedEvidence.Imported
            }.LoadItem(GlobalVM.StockLayout.evidenceTBI);
        }
        protected internal async void Question()
        {
            await Utilities.Loading.Show();
            if (VMDirty())
            {
                dialog = new Dialog(true, "El nem menetett változások lehetnek az adott oldalon", Loading.RunWorkerAsync, async () => await Utilities.Loading.Hide(), true);
                dialog.content = new TextBlock("Amennyiben mentés nélkül frissíti a táblázatot, az Ön által végrehajtott változtatások nem kerülnek mentésre\n" +
                    "Biztosan frissíti a táblázatot?");
                dialog.Start();
            }
            else Loading.RunWorkerAsync();
        }
        protected internal bool VMDirty()
        {
            if (EvidencesM.Erased.Count != 0) return true;
            return EvidencesM.Evidences.Any(p => p.IsChanged);
        }
    }
}
