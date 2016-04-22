using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Examination
{
    public class ImportExaminationVM : VMExtender
    {
        public ImportExaminationM ImportExaminationM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal ImportExaminationVM()
        {
            Start();
        }
        private async void Start()
        {
            ImportExaminationM = new ImportExaminationM();
            ImportExaminationM.PatientId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).SelectedPatientVM.SelectedPatientM.Id;
            ImportExaminationM.ExaminationCode = await new Codes().Generate((int)GlobalVM.GlobalM.UserID, ImportExaminationM.PatientId);
            await Task.Run(() =>
             {
                 foreach (object row in ImportExaminationM.ExaminationList)
                     (row as DocumentControlM.ListElement).AcceptChanges();
                 ImportExaminationM.AcceptChanges();
             });
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, ImportExaminationM.ExaminationList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                GetName = new Func<string>(() => { return ImportExaminationM.ExaminationName; }),
                GetCode = new Func<string>(() => { return ImportExaminationM.ExaminationCode; }),
                ReadOnly = false,
                Type = true
            };
            DocumentControlVM.Start(ImportExaminationM.PatientId);
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
                me = new medicalEntities();
                me.Database.Connection.Open();

                importedexaminationdata ied = new importedexaminationdata()
                {
                    PatientIdIEX = ImportExaminationM.PatientId,
                    DoctorIdIEX = (int)GlobalVM.GlobalM.UserID,
                    DateTimeIEX = (DateTime)ImportExaminationM.ExaminationDate,
                    NameIEX = ImportExaminationM.ExaminationName,
                    CodeIEX = ImportExaminationM.ExaminationCode
                };
                me.importedexaminationdata.Add(ied);
                me.SaveChanges();

                int id = ied.IdIEX;

                for (int i = 0; i < ImportExaminationM.ExaminationList.Count - 1; i++)
                {
                    examinationdatadocuments ed = new examinationdatadocuments()
                    {
                        DataEXD = ImportExaminationM.ExaminationList[i].File.ToArray(),
                        TypeEXD = ImportExaminationM.ExaminationList[i].ButtonType,
                        FileTypeEXD = ImportExaminationM.ExaminationList[i].FileType
                    };
                    me.examinationdatadocuments.Add(ed);
                    me.SaveChanges();

                    int ide = ed.IdEXD;
                    me.importedexaminationdata_st.Add(new importedexaminationdata_st()
                    {
                        IdIEX = id,
                        IdEXD = ide
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
                dialog = new Dialog(false, "Módosítások mentése", async delegate { await Utilities.Loading.Hide(); });
                dialog.content = new Views.Dialogs.TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
                new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.examinationTBI);
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty()
        {
            if (ImportExaminationM.IsChanged) return true;
            if (ImportExaminationM.ExaminationList.Any(i => i.IsChanged)) return true;
            return false;
        }
    }
}
