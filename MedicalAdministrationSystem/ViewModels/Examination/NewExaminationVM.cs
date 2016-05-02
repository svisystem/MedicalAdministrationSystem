using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Examination
{
    public class NewExaminationVM : VMExtender
    {
        public NewExaminationM NewExaminationM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        protected internal NewExaminationVM()
        {
            Start();
        }
        private async void Start()
        {
            NewExaminationM = new NewExaminationM();
            NewExaminationM.PatientId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).SelectedPatientVM.Id();
            NewExaminationM.ExaminationCode = await new Codes().Generate((int)GlobalVM.GlobalM.UserID, NewExaminationM.PatientId);
            NewExaminationM.ExaminationDate = DateTime.Now;
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
                    NewExaminationM.Treats = me.treatmentdata.Where(t => t.DeletedTD == false)
                        .Select(t => t.NameTD).ToList();
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
                foreach (object row in NewExaminationM.ExaminationList)
                    (row as DocumentControlM.ListElement).AcceptChanges();
                NewExaminationM.AcceptChanges();
            }
            else ConnectionMessage();
            await Utilities.Loading.Hide();
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, NewExaminationM.ExaminationList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                GetName = new Func<string>(() => NewExaminationM.SelectedTreat),
                GetCode = new Func<string>(() => NewExaminationM.ExaminationCode),
                Type = true
            };
            DocumentControlVM.New(NewExaminationM.PatientId);
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

                examinationdata ed = new examinationdata()
                {
                    PatientIdEX = NewExaminationM.PatientId,
                    DoctorIdEX = (int)GlobalVM.GlobalM.UserID,
                    DateTimeEX = (DateTime)NewExaminationM.ExaminationDate,
                    TreatmentIdEX = me.treatmentdata.Where(t => t.NameTD == NewExaminationM.SelectedTreat).Single().IdTD,
                    CodeEX = NewExaminationM.ExaminationCode
                };
                me.examinationdata.Add(ed);
                me.SaveChanges();

                int id = ed.IdEX;

                for (int i = 0; i < NewExaminationM.ExaminationList.Count - 1; i++)
                {
                    examinationdatadocuments edd = new examinationdatadocuments()
                    {
                        DataEXD = NewExaminationM.ExaminationList[i].File.ToArray(),
                        TypeEXD = NewExaminationM.ExaminationList[i].ButtonType,
                        FileTypeEXD = NewExaminationM.ExaminationList[i].FileType
                    };
                    me.examinationdatadocuments.Add(edd);
                    me.SaveChanges();

                    int ide = edd.IdEXD;
                    me.examinationdatadocuments_st.Add(new examinationdatadocuments_st()
                    {
                        IdEX = id,
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
                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new Views.Dialogs.TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
                new MenuButtonsEnabled().LoadItem(GlobalVM.StockLayout.examinationTBI);
            }
            else ConnectionMessage();
        }
        protected internal bool ExaminationCheck(string selected)
        {
            return NewExaminationM.Treats.Any(l => l == selected);
        }
        protected internal bool VMDirty()
        {
            if (NewExaminationM.IsChanged) return true;
            return NewExaminationM.ExaminationList.Any(i => i.IsChanged);
        }
    }
}
