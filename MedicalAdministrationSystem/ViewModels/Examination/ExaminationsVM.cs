using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAdministrationSystem.ViewModels.Examination
{
    public class ExaminationsVM : VMExtender
    {
        public ExaminationsM ExaminationsM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private BackgroundWorker EraseBackground { get; set; }
        private System.Action Loaded { get; set; }
        protected internal ExaminationsVM(System.Action Loaded)
        {
            this.Loaded = Loaded;
            ExaminationsM = new ExaminationsM();
            ExaminationsM.PatientId = (GlobalVM.StockLayout.headerContent.Content as SelectedPatient).SelectedPatientVM.SelectedPatientM.Id;
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
                me = new medicalEntities();
                me.Database.Connection.Open();

                if (me.examinationdata.Where(ex => ex.PatientIdEX == ExaminationsM.PatientId).Count() != 0)
                    foreach (ExaminationsM.Examination item in me.examinationdata.Where(ex => ex.PatientIdEX == ExaminationsM.PatientId)
                        .Select(ex => new ExaminationsM.Examination
                        {
                            Id = ex.IdEX,
                            Imported = false,
                            Name = me.treatmentdata.Where(t => t.IdTD == ex.IdEX).FirstOrDefault().NameTD,
                            Code = ex.CodeEX,
                            DateTime = ex.DateTimeEX,
                            DoctorName = me.userdata.Where(u => u.IdUD == GlobalVM.GlobalM.UserID).FirstOrDefault().NameUD,
                            DocumentCount = me.examinationdata_st.Where(exd => exd.IdEX == ex.IdEX).Count(),
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
                        DoctorName = me.userdata.Where(u => u.IdUD == GlobalVM.GlobalM.UserID).FirstOrDefault().NameUD,
                        DocumentCount = me.importedexaminationdata_st.Where(exd => exd.IdIEX == ex.IdIEX).Count(),
                    }).ToList())
                        ExaminationsM.Examinations.Add(item);

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
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
                me = new medicalEntities();
                me.Database.Connection.Open();

                foreach (ExaminationsM.ErasedItem item in ExaminationsM.Erased)
                {
                    if (item.Imported)
                    {
                        me.examinationdatadocuments.RemoveRange
                            (me.examinationdatadocuments.Where(ex => me.importedexaminationdata_st.Where
                            (ied => ied.IdIEX == item.Id).Select(ied => ied.IdEXD).ToList().Any(c => c == ex.IdEXD)));

                        me.importedexaminationdata_st.RemoveRange
                            (me.importedexaminationdata_st.Where(ex => ex.IdIEX == item.Id));

                        me.importedexaminationdata.Remove
                            (me.importedexaminationdata.Where(ex => ex.IdIEX == item.Id).Single());
                    }

                    else if (!item.Imported)
                    {
                        me.examinationdatadocuments.RemoveRange
                            (me.examinationdatadocuments.Where(ex => me.examinationdata_st.Where
                            (ied => ied.IdEX == item.Id).Select(ied => ied.IdEXD).ToList().Any(c => c == ex.IdEXD)));

                        me.examinationdata_st.RemoveRange
                            (me.examinationdata_st.Where(ex => ex.IdEX == item.Id));

                        me.examinationdata.Remove
                            (me.examinationdata.Where(ex => ex.IdEX == item.Id).Single());
                    }
                }

                me.SaveChanges();
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
                ExaminationsM.Erased.Clear();
                foreach (ExaminationsM.Examination row in ExaminationsM.Examinations)
                    row.AcceptChanges();
                ExaminationsM.AcceptChanges();

                dialog = new Dialog(false, "Módosítások mentése", async delegate { await Utilities.Loading.Hide(); });
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        protected internal async void View()
        {
            await Utilities.Loading.Show();
            new FormChecking(delegate { OkMethod(true); }, delegate { }, true);
        }
        protected internal async void Edit()
        {
            await Utilities.Loading.Show();
            new FormChecking(delegate { OkMethod(false); }, delegate { }, true);
        }
        private void OkMethod(bool which)
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled()
            {
                modifier = which,
                ID = ExaminationsM.SelectedExamination.Id,
                imported = ExaminationsM.SelectedExamination.Imported
            };
            mbe.LoadItem(GlobalVM.StockLayout.examinationTBI);
        }
        protected internal async void Question()
        {
            await Utilities.Loading.Show();
            if (VMDirty())
            {
                dialog = new Dialog(true, "El nem menetett változások lehetnek az adott oldalon", Loading.RunWorkerAsync, async delegate { await Utilities.Loading.Hide(); }, true);
                dialog.content = new TextBlock("Amennyiben mentés nélkül frissíti a táblázatot, az Ön által végrehajtott változtatások nem kerülnek mentésre\n" +
                    "Biztosan frissíti a táblázatot?");
                dialog.Start();
            }
            else Loading.RunWorkerAsync();
        }
        protected internal void ExaminationEraseMethod()
        {
            dialog = new Dialog(true, "Vizsgálat törlése", Erase, delegate { }, true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott vizsgálatot a hozzátartozó összes adattal együtt?\n" +
                "A művelet csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private void Erase()
        {
            ExaminationsM.Erased.Add(new ExaminationsM.ErasedItem
            {
                Id = ExaminationsM.SelectedExamination.Id,
                Imported = ExaminationsM.SelectedExamination.Imported
            });
            ExaminationsM.Examinations.Remove(ExaminationsM.Examinations.Where(e => e == ExaminationsM.SelectedExamination).Single());
        }
        protected internal bool VMDirty()
        {
            if (ExaminationsM.Erased.Count != 0) return true;
            return ExaminationsM.Examinations.Any(p => p.IsChanged);
        }
    }
}
