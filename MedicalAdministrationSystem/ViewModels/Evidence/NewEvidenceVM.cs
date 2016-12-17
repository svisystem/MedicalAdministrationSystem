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
    public class NewEvidenceVM : VMExtender
    {
        public NewEvidenceM NewEvidenceM { get; set; }
        private DocumentControlVM DocumentControlVM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private SelectedPatient SelectedPatient { get; set; }
        private Func<ObservableCollection<SelectedPatientM.ExaminationItem>> GetList { get; set; }
        protected internal NewEvidenceVM(Func<ObservableCollection<SelectedPatientM.ExaminationItem>> GetList)
        {
            this.GetList = GetList;
            Start();
        }
        private async void Start()
        {
            NewEvidenceM = new NewEvidenceM();
            SelectedPatient = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient);
            NewEvidenceM.PatientId = SelectedPatient.SelectedPatientVM.Id();
            NewEvidenceM.Code = await new Codes().Generate((int)GlobalVM.GlobalM.UserID, NewEvidenceM.PatientId);
            NewEvidenceM.Date = DateTime.Now;
        }
        protected internal void ParameterPassingAfterLoad(ref ContentControl content, Func<bool> Validate, Action<bool> SetEnabledSave, Action<bool> SetReadOnlyFields)
        {
            DocumentControlVM = new DocumentControlVM(ref content, NewEvidenceM.EvidenceList)
            {
                Validate = Validate,
                SetEnabledSave = SetEnabledSave,
                SetReadOnlyFields = SetReadOnlyFields,
                GetCode = new Func<string>(() => NewEvidenceM.Code),
                Type = false
            };
            DocumentControlVM.New(NewEvidenceM.PatientId);
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

                evidencedata ed = new evidencedata()
                {
                    PatientIdED = NewEvidenceM.PatientId,
                    UserDataIdED = (int)GlobalVM.GlobalM.UserID,
                    CodeED = NewEvidenceM.Code,
                    //Schedule
                    DateTimeED = (DateTime)NewEvidenceM.Date,
                    CompanyIdED = (int)GlobalVM.GlobalM.CompanyId
                };

                me.evidencedata.Add(ed);
                me.SaveChanges();

                int id = ed.IdED;

                for (int i = 0; i < NewEvidenceM.EvidenceList.Count - 1; i++)
                {
                    evidencedatadocuments edd = new evidencedatadocuments()
                    {
                        DataEDD = NewEvidenceM.EvidenceList[i].File.ToArray(),
                        TypeEDD = NewEvidenceM.EvidenceList[i].ButtonType,
                        FileTypeEDD = NewEvidenceM.EvidenceList[i].FileType
                    };
                    me.evidencedatadocuments.Add(edd);
                    me.SaveChanges();

                    int ide = edd.IdEDD;
                    me.evidencedatadocuments_st.Add(new evidencedatadocuments_st()
                    {
                        IdED = id,
                        IdEDD = ide
                    });
                    me.SaveChanges();
                }

                foreach (SelectedPatientM.ExaminationItem item in GetList())
                {
                    if (item.Imported)
                    {
                        me.importedexaminationeachevidence_st.Add(new importedexaminationeachevidence_st()
                        {
                            IdIEX = item.Id,
                            IdED = id
                        });
                    }
                    else me.examinationeachevidence_st.Add(new examinationeachevidence_st()
                    {
                        IdEX = item.Id,
                        IdED = id
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
            return GetList().Any(i => i.IsChanged) ? true : NewEvidenceM.EvidenceList.Any(i => i.IsChanged);
        }
    }
}
