using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Evidence
{
    public class ViewEvidenceVM : VMExtender
    {
        public ViewEvidenceM ViewEvidenceM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        private Action<SelectedPatientM.ExaminationItem> AddList { get; set; }
        protected internal ViewEvidenceVM(bool imported, int ID, Action<SelectedPatientM.ExaminationItem> AddList)
        {
            this.AddList = AddList;
            Start(imported, ID);
        }
        private void Start(bool imported, int ID)
        {
            ViewEvidenceM = new ViewEvidenceM();
            SelectedPatient = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient);
            ViewEvidenceM.Imported = imported;
            ViewEvidenceM.Id = ID;
            ViewEvidenceM.PatientId = SelectedPatient.SelectedPatientVM.Id();
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

                    if (ViewEvidenceM.Imported)
                    {
                        me.evidencedatadocuments.Where(ex => me.importedevidencedatadocuments_st.Where
                        (iex => iex.IdIED == ViewEvidenceM.Id).Select(iex => iex.IdEDD).ToList().Any(c => c == ex.IdEDD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEDD, p.FileTypeEDD, p.IdEDD, new MemoryStream(p.DataEDD)));

                        ViewEvidenceM.Code = me.importedevidencedata.Where(iex => iex.IdIED == ViewEvidenceM.Id).Single().CodeIED;
                        ViewEvidenceM.Date = me.importedevidencedata.Where(iex => iex.IdIED == ViewEvidenceM.Id).Single().DateTimeIED;

                        foreach (SelectedPatientM.ExaminationItem item in me.examinationdata.Where(ed => ed.IdEX == me.examinationeachimportedevidence_st
                         .Where(eeie => eeie.IdIED == ViewEvidenceM.Id).Select(eeie => eeie.IdEX).FirstOrDefault()).ToList()
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
                         .Where(eeie => eeie.IdIED == ViewEvidenceM.Id).Select(eeie => eeie.IdIEX).FirstOrDefault()).ToList()
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
                        (iex => iex.IdED == ViewEvidenceM.Id).Select(iex => iex.IdEDD).ToList().Any(c => c == ex.IdEDD)).ToList().ForEach
                        (p => DocumentControlVM.Add(p.TypeEDD, p.FileTypeEDD, p.IdEDD, new MemoryStream(p.DataEDD)));

                        ViewEvidenceM.Code = me.evidencedata.Where(iex => iex.IdED == ViewEvidenceM.Id).Single().CodeED;
                        ViewEvidenceM.Date = me.evidencedata.Where(iex => iex.IdED == ViewEvidenceM.Id).Single().DateTimeED;

                        foreach (SelectedPatientM.ExaminationItem item in me.examinationdata.Where(ed => ed.IdEX == me.examinationeachevidence_st
                         .Where(eeie => eeie.IdED == ViewEvidenceM.Id).Select(eeie => eeie.IdEX).FirstOrDefault()).ToList()
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
                         .Where(eeie => eeie.IdED == ViewEvidenceM.Id).Select(eeie => eeie.IdIEX).FirstOrDefault()).ToList()
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
                ViewEvidenceM.AcceptChanges();
            else ConnectionMessage();
            await Utilities.Loading.Hide();
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, ViewEvidenceM.EvidenceList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                Type = false
            };
            DocumentControlVM.ReadOnly();
            Loading.RunWorkerAsync();
        }
        protected internal bool VMDirty()
        {
            if (ViewEvidenceM.IsChanged) return true;
            return DocumentControlVM.VMDirty();
        }
    }
}
