using MedicalAdministrationSystem.DataAccess;
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
    public class ExaminationViewVM : VMExtender
    {
        public ExaminationViewM ExaminationViewM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        protected internal ExaminationViewVM(bool imported, int ID)
        {
            Start(imported, ID);
        }
        private void Start(bool imported, int ID)
        {
            ExaminationViewM = new ExaminationViewM();
            ExaminationViewM.Imported = imported;
            ExaminationViewM.Id = ID;
            ExaminationViewM.PatientId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).SelectedPatientVM.SelectedPatientM.Id;
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    me.Database.Connection.Open();

                    if (ExaminationViewM.Imported)
                    {
                        me.examinationdatadocuments.Where(ex => me.importedexaminationdatadocuments_st.Where
                        (iex => iex.IdIEX == ExaminationViewM.Id).Select(iex => iex.IdEXD).ToList().Any(c => c == ex.IdEXD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEXD, p.FileTypeEXD, p.IdEXD, new MemoryStream(p.DataEXD)));

                        ExaminationViewM.ExaminationCode = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationViewM.Id).Single().CodeIEX;
                        ExaminationViewM.ExaminationName = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationViewM.Id).Single().NameIEX;
                        ExaminationViewM.ExaminationDate = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationViewM.Id).Single().DateTimeIEX;
                    }

                    else
                    {
                        me.examinationdatadocuments.Where(ex => me.examinationdatadocuments_st.Where
                        (iex => iex.IdEX == ExaminationViewM.Id).Select(iex => iex.IdEXD).ToList().Any(c => c == ex.IdEXD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEXD, p.FileTypeEXD, p.IdEXD, new MemoryStream(p.DataEXD)));

                        ExaminationViewM.ExaminationCode = me.examinationdata.Where(iex => iex.IdEX == ExaminationViewM.Id).Single().CodeEX;
                        ExaminationViewM.ExaminationName = me.servicesdata.Where(t => t.IdTD == me.examinationdata.
                        Where(iex => iex.IdEX == ExaminationViewM.Id).FirstOrDefault().ServiceIdEX).Single().NameTD;
                        ExaminationViewM.ExaminationDate = me.examinationdata.Where(iex => iex.IdEX == ExaminationViewM.Id).Single().DateTimeEX;
                    }
                    me.Database.Connection.Close();
                    workingConn = true;
                }
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
                ExaminationViewM.AcceptChanges();
            else ConnectionMessage();
            await Utilities.Loading.Hide();
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, ExaminationViewM.ExaminationList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                Type = true
            };
            DocumentControlVM.ReadOnly();
            Loading.RunWorkerAsync();
        }
        protected internal bool VMDirty()
        {
            if (ExaminationViewM.IsChanged) return true;
            return DocumentControlVM.VMDirty();
        }
    }
}
