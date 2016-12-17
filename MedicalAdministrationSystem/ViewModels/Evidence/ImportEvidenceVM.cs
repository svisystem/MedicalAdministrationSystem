using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Evidence
{
    public class ImportEvidenceVM : VMExtender
    {
        public ImportEvidenceM ImportEvidenceM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        private Func<ObservableCollection<SelectedPatientM.ExaminationItem>> GetList { get; set; }
        protected internal ImportEvidenceVM(Func<ObservableCollection<SelectedPatientM.ExaminationItem>> GetList)
        {
            this.GetList = GetList;
            Start();
        }
        private async void Start()
        {
            ImportEvidenceM = new ImportEvidenceM();
            SelectedPatient = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient);
            ImportEvidenceM.PatientId = SelectedPatient.SelectedPatientVM.Id();
            ImportEvidenceM.Code = await new Codes().Generate((int)GlobalVM.GlobalM.UserID, ImportEvidenceM.PatientId);
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, ImportEvidenceM.EvidenceList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                GetCode = new Func<string>(() => ImportEvidenceM.Code),
                Type = false
            };
            DocumentControlVM.New(ImportEvidenceM.PatientId);
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
        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteComplete);
            Execute.RunWorkerAsync();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new MedicalModel();
                me.Database.Connection.Open();

                importedevidencedata ied = new importedevidencedata()
                {
                    PatientIED = ImportEvidenceM.PatientId,
                    UserDataIdIED = (int)GlobalVM.GlobalM.UserID,
                    CodeIED = ImportEvidenceM.Code,
                    DateTimeIED = (DateTime)ImportEvidenceM.Date,
                    CompanyIdIED = (int)GlobalVM.GlobalM.CompanyId
                };

                me.importedevidencedata.Add(ied);
                me.SaveChanges();

                int id = ied.IdIED;

                for (int i = 0; i < ImportEvidenceM.EvidenceList.Count - 1; i++)
                {
                    evidencedatadocuments edd = new evidencedatadocuments()
                    {
                        DataEDD = ImportEvidenceM.EvidenceList[i].File.ToArray(),
                        TypeEDD = ImportEvidenceM.EvidenceList[i].ButtonType,
                        FileTypeEDD = ImportEvidenceM.EvidenceList[i].FileType
                    };
                    me.evidencedatadocuments.Add(edd);
                    me.SaveChanges();

                    int ide = edd.IdEDD;
                    me.importedevidencedatadocuments_st.Add(new importedevidencedatadocuments_st()
                    {
                        IdIED = id,
                        IdEDD = ide
                    });
                    me.SaveChanges();
                }

                foreach (SelectedPatientM.ExaminationItem item in GetList())
                {
                    if (item.Imported)
                    {
                        me.importedexaminationeachimportedevidence_st.Add(new importedexaminationeachimportedevidence_st()
                        {
                            IdIEX = item.Id,
                            IdIED = id
                        });
                    }
                    else me.examinationeachimportedevidence_st.Add(new examinationeachimportedevidence_st()
                    {
                        IdEX = item.Id,
                        IdIED = id
                    });
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
            return GetList().Any(i => i.IsChanged) ? true : ImportEvidenceM.EvidenceList.Any(i => i.IsChanged);
        }
    }
}
