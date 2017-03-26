using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Examination
{
    public class ExaminationEditVM : VMExtender
    {
        public ExaminationEditM ExaminationEditM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal ExaminationEditVM(bool imported, int ID)
        {
            Start(imported, ID);
        }
        private void Start(bool imported, int ID)
        {
            ExaminationEditM = new ExaminationEditM();
            ExaminationEditM.Imported = imported;
            ExaminationEditM.Id = ID;
            ExaminationEditM.PatientId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).SelectedPatientVM.SelectedPatientM.Id;
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
        }
        private async void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    if (ExaminationEditM.Imported)
                    {
                        me.examinationdatadocuments.Where(ex => me.importedexaminationdatadocuments_st.Where
                        (iex => iex.IdIEX == ExaminationEditM.Id).Select(iex => iex.IdEXD).ToList().Any(c => c == ex.IdEXD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEXD, p.FileTypeEXD, p.IdEXD, new MemoryStream(p.DataEXD)));

                        ExaminationEditM.ExaminationCode = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationEditM.Id).Single().CodeIEX;
                        ExaminationEditM.ExaminationName = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationEditM.Id).Single().NameIEX;
                        ExaminationEditM.ExaminationDate = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationEditM.Id).Single().DateTimeIEX;
                    }

                    else
                    {
                        me.examinationdatadocuments.Where(ex => me.examinationdatadocuments_st.Where
                        (iex => iex.IdEX == ExaminationEditM.Id).Select(iex => iex.IdEXD).ToList().Any(c => c == ex.IdEXD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEXD, p.FileTypeEXD, p.IdEXD, new MemoryStream(p.DataEXD)));

                        ExaminationEditM.ExaminationCode = me.examinationdata.Where(iex => iex.IdEX == ExaminationEditM.Id).Single().CodeEX;
                        ExaminationEditM.ExaminationName = me.servicesdata.Where(t => t.IdTD == me.examinationdata.
                        Where(iex => iex.IdEX == ExaminationEditM.Id).FirstOrDefault().ServiceIdEX).Single().NameTD;
                        ExaminationEditM.ExaminationDate = me.examinationdata.Where(iex => iex.IdEX == ExaminationEditM.Id).Single().DateTimeEX;
                    }
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
                ExaminationEditM.AcceptChanges();
            else ConnectionMessage();
            await Utilities.Loading.Hide();
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, ExaminationEditM.ExaminationList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                GetName = new Func<string>(() => ExaminationEditM.ExaminationName),
                GetCode = new Func<string>(() => ExaminationEditM.ExaminationCode),
                Type = true
            };
            DocumentControlVM.Edit(ExaminationEditM.PatientId);
            Loading.RunWorkerAsync();
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
            dialog.content = new Views.Dialogs.TextBlock("Biztosan elmenti a vizsgálati anyagokon végrehajtott módosításokat?");
            dialog.Start();
        }
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();

                    foreach (DocumentControlM.ListElement item in ExaminationEditM.ExaminationList)
                    {
                        if (item.DBId != null)
                        {
                            examinationdatadocuments edd = me.examinationdatadocuments.Where(ed => ed.IdEXD == item.DBId).Single();
                            edd.DataEXD = edd.DataEXD != item.File.ToArray() ? item.File.ToArray() : edd.DataEXD;
                            edd.FileTypeEXD = edd.FileTypeEXD != item.FileType ? item.FileType : edd.FileTypeEXD;
                        }
                        else if (item.File != null)
                        {
                            examinationdatadocuments ed = new examinationdatadocuments()
                            {
                                DataEXD = item.File.ToArray(),
                                TypeEXD = item.ButtonType,
                                FileTypeEXD = item.FileType
                            };

                            me.examinationdatadocuments.Add(ed);
                            await me.SaveChangesAsync();
                            int ide = ed.IdEXD;

                            if (ExaminationEditM.Imported)
                            {
                                me.importedexaminationdatadocuments_st.Add(new importedexaminationdatadocuments_st()
                                {
                                    IdIEX = ExaminationEditM.Id,
                                    IdEXD = ide
                                });
                                await me.SaveChangesAsync();
                            }
                            else
                            {
                                me.examinationdatadocuments_st.Add(new examinationdatadocuments_st()
                                {
                                    IdEX = ExaminationEditM.Id,
                                    IdEXD = ide
                                });
                                await me.SaveChangesAsync();
                            }
                        }
                        await me.SaveChangesAsync();
                    }

                    foreach (int id in DocumentControlVM.Erased())
                    {
                        if (ExaminationEditM.Imported)
                            me.importedexaminationdatadocuments_st.Remove
                                (me.importedexaminationdatadocuments_st.Where(ex => ex.IdEXD == id).Single());

                        else me.examinationdatadocuments_st.Remove
                                (me.examinationdatadocuments_st.Where(ex => ex.IdEXD == id).Single());

                        me.examinationdatadocuments.Remove
                            (me.examinationdatadocuments.Where(ex => ex.IdEXD == id).Single());

                        await me.SaveChangesAsync();
                    }
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new Views.Dialogs.TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
                await new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.examinationTBI);
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty()
        {
            if (ExaminationEditM.IsChanged) return true;
            return DocumentControlVM.VMDirty();
        }
    }
}
