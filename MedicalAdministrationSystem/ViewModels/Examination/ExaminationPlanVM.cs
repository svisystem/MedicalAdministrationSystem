using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Fragments;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Examination
{
    public class ExaminationPlanVM : VMExtender
    {
        public ExaminationPlanM ExaminationPlanM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private Action Loaded { get; set; }
        protected internal WordEditor WordEditor { get; set; }
        protected internal ExaminationPlanVM(Action Loaded)
        {
            this.Loaded = Loaded;
            ExaminationPlanM = new ExaminationPlanM();
            WordEditor = new WordEditor(true);
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new MedicalModel(ConfigurationManager.Connect());
                me.Database.Connection.Open();
                foreach (servicesdata row in me.servicesdata.Where(a => a.DeletedTD == null).ToList())
                    ExaminationPlanM.Services.Add(new ExaminationPlanM.Service
                    {
                        ID = row.IdTD,
                        Name = row.NameTD,
                        Vat = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == row.IdTD).OrderByDescending(pfs => pfs.IdPFS).FirstOrDefault().VatPFS,
                        Price = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == row.IdTD).OrderByDescending(pfs => pfs.IdPFS).FirstOrDefault().PricePFS,
                        Details = row.DetailsTD
                    });
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                foreach (object row in ExaminationPlanM.Services)
                    (row as ExaminationPlanM.Service).AcceptChanges();
                ExaminationPlanM.AcceptChanges();
                WordEditor.ExaminationPlanStart();
                Loaded();
            }
            else ConnectionMessage();
        }
        protected internal async void Refresh()
        {
            await Utilities.Loading.Show();
            Loading.RunWorkerAsync();
        }
        protected internal void Print()
        {
            WordEditor.Print();
        }
        protected internal void Add()
        {
            WordEditor.ExaminationPlanItem(ExaminationPlanM.Selected.Name,
                ExaminationPlanM.Selected.Details != null ? ExaminationPlanM.Selected.Details : null,
                ExaminationPlanM.Selected.Price != 0 ? ExaminationPlanM.Selected.Price.ToString("##,#", new CultureInfo("hu-HU")) : null);
        }
        protected internal bool VMDirty()
        {
            return ExaminationPlanM.Services.Any(s => s.IsChanged);
        }
    }
}
