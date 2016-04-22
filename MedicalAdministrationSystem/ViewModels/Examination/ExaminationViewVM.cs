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
    public class ExaminationViewVM : VMExtender
    {
        public ExaminationViewM ExaminationViewM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private Action ParameterPassingFromView { get; set; }
        protected internal ExaminationViewVM(bool imported, int ID, Action ParameterPassingFromView)
        {
            this.ParameterPassingFromView = ParameterPassingFromView;
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
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();

                    if (ExaminationViewM.Imported)
                    {
                        foreach (DocumentControlM.ListElement item in me.examinationdatadocuments.Where(ex => me.importedexaminationdata_st.Where
                        (iex => iex.IdIEX == ExaminationViewM.Id).Select(iex => iex.IdEXD).ToList().Any(c => c == ex.IdEXD)).ToList()
                            .Select(ex => new DocumentControlM.ListElement
                            {
                                DBId = ex.IdEXD,
                                File = new MemoryStream(ex.DataEXD),
                                ButtonType = ex.TypeEXD,
                                FileType = ex.FileTypeEXD
                            }))
                            ExaminationViewM.ExaminationList.Add(item);

                        ExaminationViewM.ExaminationCode = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationViewM.Id).Single().CodeIEX;
                        ExaminationViewM.ExaminationName = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationViewM.Id).Single().NameIEX;
                        ExaminationViewM.ExaminationDate = me.importedexaminationdata.Where(iex => iex.IdIEX == ExaminationViewM.Id).Single().DateTimeIEX;
                    }

                    else
                    {
                        foreach (DocumentControlM.ListElement item in me.examinationdatadocuments.Where(ex => me.examinationdata_st.Where
                        (iex => iex.IdEX == ExaminationViewM.Id).Select(iex => iex.IdEXD).ToList().Any(c => c == ex.IdEXD)).ToList()
                            .Select(ex => new DocumentControlM.ListElement
                            {
                                DBId = ex.IdEXD,
                                File = new MemoryStream(ex.DataEXD),
                                ButtonType = ex.TypeEXD,
                                FileType = ex.FileTypeEXD
                            }))
                            ExaminationViewM.ExaminationList.Add(item);

                        ExaminationViewM.ExaminationCode = me.examinationdata.Where(iex => iex.IdEX == ExaminationViewM.Id).Single().CodeEX;
                        ExaminationViewM.ExaminationName = me.treatmentdata.Where(t => t.IdTD == me.examinationdata.
                        Where(iex => iex.IdEX == ExaminationViewM.Id).FirstOrDefault().TreatmentIdEX).Single().NameTD;
                        ExaminationViewM.ExaminationDate = me.examinationdata.Where(iex => iex.IdEX == ExaminationViewM.Id).Single().DateTimeEX;
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
            if (workingConn) ParameterPassingFromView();
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
                GetName = new Func<string>(() => { return ExaminationViewM.ExaminationName; }),
                GetCode = new Func<string>(() => { return ExaminationViewM.ExaminationCode; }),
                ReadOnly = true,
                Type = true
            };
            DocumentControlVM.Start(ExaminationViewM.PatientId);
        }
        protected internal bool VMDirty()
        {
            if (ExaminationViewM.IsChanged) return true;
            if (ExaminationViewM.ExaminationList.Any(i => i.IsChanged)) return true;
            return false;
        }
    }
}
