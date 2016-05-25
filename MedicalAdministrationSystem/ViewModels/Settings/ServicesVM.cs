using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class ServicesVM : VMExtender
    {
        public ServicesM ServicesM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private BackgroundWorker EraseBackground { get; set; }
        private bool modified { get; set; }
        private Action Loaded { get; set; }
        protected internal ServicesVM(Action Loaded)
        {
            this.Loaded = Loaded;
            ServicesM = new ServicesM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();
                ServicesM.Services.Clear();
                foreach (servicesdata row in me.servicesdata.Where(a => a.DeletedTD == false).ToList())
                    ServicesM.Services.Add(new ServicesM.Service
                    {
                        ID = row.IdTD,
                        Name = row.NameTD,
                        Vat = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == row.IdTD).FirstOrDefault().VatPFS,
                        Price = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == row.IdTD).FirstOrDefault().PricePFS,
                        Details = row.DetailsTD,
                        New = false
                    });
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
                foreach (object row in ServicesM.Services)
                    (row as ServicesM.Service).AcceptChanges();
                ServicesM.AcceptChanges();
                modified = false;
                Loaded();
            }
            else ConnectionMessage();
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
                if (ServicesM.Erased.Count != 0)
                    foreach (int service in ServicesM.Erased)
                        try
                        {
                            me.servicesdata.Where(a => a.IdTD == service).Single().DeletedTD = true;
                            me.SaveChanges();
                        }
                        catch { }
                for (int i = 0; i < ServicesM.Services.Count; i++)
                {
                    try
                    {
                        servicesdata tr = new servicesdata();
                        pricesforeachservice pfs = new pricesforeachservice();
                        if (ServicesM.Services[i].New)
                        {
                            tr.NameTD = ServicesM.Services[i].Name;
                            pfs.VatPFS = ServicesM.Services[i].Vat;
                            pfs.PricePFS = ServicesM.Services[i].Price;
                            tr.DetailsTD = ServicesM.Services[i].Details;
                            me.servicesdata.Add(tr);
                            me.SaveChanges();
                            pfs.ServiceDataIdPFS = tr.IdTD;
                            me.pricesforeachservice.Add(pfs);
                            me.SaveChanges();
                            ServicesM.Services[i].ID = tr.IdTD;
                            ServicesM.Services[i].New = false;
                        }
                        else
                        {
                            int temp = ServicesM.Services[i].ID;
                            tr = me.servicesdata.Where(a => a.IdTD == temp).Single();
                            pfs = me.pricesforeachservice.Where(pf => pf.ServiceDataIdPFS == temp).LastOrDefault();
                            if (!ServicesM.Services[i].Name.Equals(tr.NameTD))
                                tr.NameTD = ServicesM.Services[i].Name;
                            if (!ServicesM.Services[i].Details.Equals(tr.DetailsTD))
                                tr.DetailsTD = ServicesM.Services[i].Details;
                            if (!ServicesM.Services[i].Vat.Equals(pfs.VatPFS) || !ServicesM.Services[i].Price.Equals(pfs.PricePFS))
                                pfs = new pricesforeachservice()
                                {
                                    PricePFS = ServicesM.Services[i].Price,
                                    VatPFS = ServicesM.Services[i].Vat,
                                    ServiceDataIdPFS = temp
                                };
                            me.pricesforeachservice.Add(pfs);
                            me.SaveChanges();
                        }
                    }
                    catch { }
                }
                me.SaveChanges();
                me.Database.Connection.Close();
                ServicesM.Erased.Clear();
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
                foreach (object row in ServicesM.Services)
                    (row as ServicesM.Service).AcceptChanges();
                ServicesM.AcceptChanges();
                modified = false;

                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty()
        {
            if (ServicesM.IsChanged) return true;
            if (ServicesM.Services.Any(s => s.IsChanged)) return true;
            return modified;
        }
        protected internal async void Refresh()
        {
            await Utilities.Loading.Show();
            new FormChecking(Loading.RunWorkerAsync, () => { }, true);
        }
        protected internal void NewLine()
        {
            ServicesM.Services.Add(new ServicesM.Service() { New = true });
            modified = true;
        }
        protected internal void ServiceEraseMethod()
        {
            dialog = new Dialog(true, "Szolgáltatás törlése", Erase, () => { }, true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott szolgáltatást?\n" +
                "A szolgáltatás törlése csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private void Erase()
        {
            EraseBackground = new BackgroundWorker();
            EraseBackground.DoWork += new DoWorkEventHandler(EraseBackgroundDoWork);
            EraseBackground.RunWorkerCompleted += new RunWorkerCompletedEventHandler(EraseBackgroundComplete);
            EraseBackground.RunWorkerAsync();
        }
        private void EraseBackgroundDoWork(object sender, DoWorkEventArgs e)
        {
            modified = true;
            if (!ServicesM.Selected.New) ServicesM.Erased.Add(ServicesM.Selected.ID);
        }
        private void EraseBackgroundComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            ServicesM.Services.Remove(ServicesM.Selected);
        }
    }
}
