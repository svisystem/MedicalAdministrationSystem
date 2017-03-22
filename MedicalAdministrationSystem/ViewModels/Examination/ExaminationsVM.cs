using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Examination
{
    public class ExaminationsVM : VMExtender
    {
        public ExaminationsM ExaminationsM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private System.Action Loaded { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        protected internal ExaminationsVM(System.Action Loaded)
        {
            this.Loaded = Loaded;
            SelectedPatient = GlobalVM.StockLayout.headerContent.Content as SelectedPatient;
            ExaminationsM = new ExaminationsM();
            ExaminationsM.PatientId = SelectedPatient.SelectedPatientVM.SelectedPatientM.Id;
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            ExaminationsM.Examinations.Clear();
            ExaminationsM.Erased.Clear();
            try
            {
                me = new MedicalModel(ConfigurationManager.Connect());
                me.Database.Connection.Open();

                if (SelectedPatient.SelectedPatientVM.Count() == 0)
                {
                    if (me.examinationdata.Where(ex => ex.PatientIdEX == ExaminationsM.PatientId).Count() != 0)
                        foreach (ExaminationsM.Examination item in me.examinationdata.Where(ex => ex.PatientIdEX == ExaminationsM.PatientId)
                            .Select(ex => new ExaminationsM.Examination
                            {
                                Id = ex.IdEX,
                                Imported = false,
                                Name = me.servicesdata.Where(t => t.IdTD == ex.ServiceIdEX).FirstOrDefault().NameTD,
                                Code = ex.CodeEX,
                                DateTime = ex.DateTimeEX,
                                DoctorName = me.userdata.Where(u => u.IdUD == ex.DoctorIdEX).FirstOrDefault().NameUD,
                                DocumentCount = me.examinationdatadocuments_st.Where(exd => exd.IdEX == ex.IdEX).Count(),
                                Editable = !GlobalVM.GlobalM.JustImportDocuments
                            }).ToList())
                            ExaminationsM.Examinations.Add(item);

                    if (me.importedexaminationdata.Where(ex => ex.PatientIdIEX == ExaminationsM.PatientId).Count() != 0)
                        foreach (ExaminationsM.Examination item in me.importedexaminationdata.Where(ex => ex.PatientIdIEX == ExaminationsM.PatientId)
                        .Select(ex => new ExaminationsM.Examination
                        {
                            Id = ex.IdIEX,
                            Imported = true,
                            Name = ex.NameIEX,
                            Code = ex.CodeIEX,
                            DateTime = ex.DateTimeIEX,
                            DoctorName = me.userdata.Where(u => u.IdUD == ex.DoctorIdIEX).FirstOrDefault().NameUD,
                            DocumentCount = me.importedexaminationdatadocuments_st.Where(exd => exd.IdIEX == ex.IdIEX).Count(),
                            Editable = true
                        }).ToList())
                            ExaminationsM.Examinations.Add(item);
                }
                else
                {
                    foreach (SelectedPatientM.ExaminationItem item in SelectedPatient.SelectedPatientVM.SelectedPatientM.List)
                    {
                        if (item.Imported)
                            ExaminationsM.Examinations.Add(me.importedexaminationdata.Where(ex => ex.IdIEX == item.Id)
                            .Select(ex => new ExaminationsM.Examination
                            {
                                Id = ex.IdIEX,
                                Imported = true,
                                Name = ex.NameIEX,
                                Code = ex.CodeIEX,
                                DateTime = ex.DateTimeIEX,
                                DoctorName = me.userdata.Where(u => u.IdUD == ex.DoctorIdIEX).FirstOrDefault().NameUD,
                                DocumentCount = me.importedexaminationdatadocuments_st.Where(exd => exd.IdIEX == ex.IdIEX).Count(),
                                Editable = true
                            }).Single());
                        else ExaminationsM.Examinations.Add(me.examinationdata.Where(ex => ex.IdEX == item.Id)
                            .Select(ex => new ExaminationsM.Examination
                            {
                                Id = ex.IdEX,
                                Imported = false,
                                Name = me.servicesdata.Where(t => t.IdTD == ex.ServiceIdEX).FirstOrDefault().NameTD,
                                Code = ex.CodeEX,
                                DateTime = ex.DateTimeEX,
                                DoctorName = me.userdata.Where(u => u.IdUD == ex.DoctorIdEX).FirstOrDefault().NameUD,
                                DocumentCount = me.examinationdatadocuments_st.Where(exd => exd.IdEX == ex.IdEX).Count(),
                                Editable = !GlobalVM.GlobalM.JustImportDocuments
                            }).Single());

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
                foreach (ExaminationsM.Examination row in ExaminationsM.Examinations)
                    row.AcceptChanges();
                ExaminationsM.AcceptChanges();
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

                foreach (ExaminationsM.ErasedItem item in ExaminationsM.Erased)
                {
                    if (item.Imported)
                    {
                        me.examinationdatadocuments.RemoveRange
                            (me.examinationdatadocuments.Where(ex => me.importedexaminationdatadocuments_st.Where
                            (ied => ied.IdIEX == item.Id).Select(ied => ied.IdEXD).ToList().Any(c => c == ex.IdEXD)));

                        me.importedexaminationdatadocuments_st.RemoveRange
                            (me.importedexaminationdatadocuments_st.Where(ex => ex.IdIEX == item.Id));

                        me.importedexaminationdata.Remove
                            (me.importedexaminationdata.Where(ex => ex.IdIEX == item.Id).Single());
                    }

                    else if (!item.Imported)
                    {
                        me.examinationdatadocuments.RemoveRange
                            (me.examinationdatadocuments.Where(ex => me.examinationdatadocuments_st.Where
                            (ied => ied.IdEX == item.Id).Select(ied => ied.IdEXD).ToList().Any(c => c == ex.IdEXD)));

                        me.examinationdatadocuments_st.RemoveRange
                            (me.examinationdatadocuments_st.Where(ex => ex.IdEX == item.Id));

                        me.examinationdata.Remove
                            (me.examinationdata.Where(ex => ex.IdEX == item.Id).Single());
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
                ExaminationsM.Erased.Clear();
                foreach (ExaminationsM.Examination row in ExaminationsM.Examinations)
                    row.AcceptChanges();
                ExaminationsM.AcceptChanges();

                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
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
                Id = ExaminationsM.SelectedExamination.Id,
                imported = ExaminationsM.SelectedExamination.Imported
            }.LoadItem(GlobalVM.StockLayout.examinationTBI);
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
        protected internal void ExaminationEraseMethod()
        {
            dialog = new Dialog(true, "Vizsgálat törlése", async () =>
            {
                await Utilities.Loading.Show();
                BackgroundWorker Erase = new BackgroundWorker();
                Erase.DoWork += new DoWorkEventHandler(EraseDoWork);
                Erase.RunWorkerCompleted += new RunWorkerCompletedEventHandler(EraseComplete);
                Erase.RunWorkerAsync();
            }, () => { }, true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott vizsgálatot a hozzátartozó összes adattal együtt?\n" +
                "A művelet csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private bool eraseable;
        private void EraseDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new MedicalModel(ConfigurationManager.Connect());
                me.Database.Connection.Open();
                eraseable = false;

                if (ExaminationsM.SelectedExamination.Imported)
                    eraseable = !me.importedexaminationeachevidence_st.Any(ieee => ieee.IdIEX == ExaminationsM.SelectedExamination.Id) ||
                        !me.importedexaminationeachimportedevidence_st.Any(ieee => ieee.IdIEX == ExaminationsM.SelectedExamination.Id);
                else eraseable = !me.examinationeachevidence_st.Any(ieee => ieee.IdEX == ExaminationsM.SelectedExamination.Id) ||
                        !me.examinationeachimportedevidence_st.Any(ieee => ieee.IdEX == ExaminationsM.SelectedExamination.Id);

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void EraseComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                if (eraseable)
                {
                    ExaminationsM.Erased.Add(new ExaminationsM.ErasedItem
                    {
                        Id = ExaminationsM.SelectedExamination.Id,
                        Imported = ExaminationsM.SelectedExamination.Imported
                    });
                    ExaminationsM.Examinations.Remove(ExaminationsM.Examinations.Where(ex => ex == ExaminationsM.SelectedExamination).Single());
                    await Utilities.Loading.Hide();
                }
                else
                {
                    dialog = new Dialog(true, "Nem lehet törölni a vizsgálatot", async () => await Utilities.Loading.Hide());
                    dialog.content = new TextBlock("Az adatbázis függőségei miatt nem lehet törölni a kívánt vizsgálatot\n" +
                        "Ehhez előbb ki kell törölni minden \"Státusz\"ban az erre a vizsglatra mutató hivatkozást");
                    dialog.Start();
                }
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty()
        {
            if (ExaminationsM.Erased.Count != 0) return true;
            return ExaminationsM.Examinations.Any(p => p.IsChanged);
        }
        protected internal void Select()
        {
            SelectedPatient.SelectedPatientVM.Add(
                ExaminationsM.SelectedExamination.Imported,
                ExaminationsM.SelectedExamination.Id,
                ExaminationsM.SelectedExamination.Name,
                ExaminationsM.SelectedExamination.Code,
                ExaminationsM.SelectedExamination.DateTime);
        }
    }
}
