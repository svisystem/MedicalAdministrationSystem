using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
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
            ImportExaminationM.AcceptChanges();
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, ImportExaminationM.ExaminationList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                GetName = new Func<string>(() => ImportExaminationM.ExaminationName),
                GetCode = new Func<string>(() => ImportExaminationM.ExaminationCode),
                Type = true
            };
            DocumentControlVM.New(ImportExaminationM.PatientId);
        }
        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteComplete);
            Execute.RunWorkerAsync();
        }
        private async void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();

                    importedexaminationdata ied = new importedexaminationdata()
                    {
                        PatientIdIEX = ImportExaminationM.PatientId,
                        DoctorIdIEX = (int)GlobalVM.GlobalM.UserID,
                        DateTimeIEX = (DateTime)ImportExaminationM.ExaminationDate,
                        NameIEX = ImportExaminationM.ExaminationName,
                        CodeIEX = ImportExaminationM.ExaminationCode,
                        CompanyIdIEX = (int)GlobalVM.GlobalM.CompanyId
                    };
                    me.importedexaminationdata.Add(ied);
                    await me.SaveChangesAsync();

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
                        await me.SaveChangesAsync();

                        int ide = ed.IdEXD;
                        me.importedexaminationdatadocuments_st.Add(new importedexaminationdatadocuments_st()
                        {
                            IdIEX = id,
                            IdEXD = ide
                        });
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
            if (ImportExaminationM.IsChanged) return true;
            return DocumentControlVM.VMDirty();
        }
    }
}
