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
        public ServicesM.SelectedRow SelectedRow { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private BackgroundWorker EraseBackground { get; set; }
        private medicalEntities me { get; set; }
        private bool workingConn { get; set; }
        private ObservableCollection<ServicesM.Service> temp { get; set; }
        private bool modified { get; set; }
        private Action Loaded { get; set; }
        protected internal ServicesVM(Action Loaded)
        {
            this.Loaded = Loaded;
            ServicesM = new ServicesM();
            SelectedRow = new ServicesM.SelectedRow();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            temp = new ObservableCollection<ServicesM.Service>();
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();
                foreach (treatmentdata row in me.treatmentdata.Where(a => a.DeletedTD == false).ToList())
                    temp.Add(new ServicesM.Service
                    {
                        ID = row.IdTD,
                        Name = row.NameTD,
                        Vat = row.VatTD,
                        Price = row.PriceTD,
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
                ServicesM.Services = temp;
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
                            me.treatmentdata.Remove(me.treatmentdata.Where(a => a.IdTD == service).Single());
                            me.SaveChanges();
                        }
                        catch { }
                for (int i = 0; i < ServicesM.Services.Count; i++)
                {
                    int temp = ServicesM.Services[i].ID;
                    try
                    {
                        treatmentdata tr = new treatmentdata();
                        if (ServicesM.Services[i].New)
                        {
                            tr.NameTD = ServicesM.Services[i].Name;
                            tr.VatTD = ServicesM.Services[i].Vat;
                            tr.PriceTD = ServicesM.Services[i].Price;
                            tr.DetailsTD = ServicesM.Services[i].Details;
                            me.treatmentdata.Add(tr);
                            me.SaveChanges();
                        }
                        else
                        {
                            tr = me.treatmentdata.Where(a => a.IdTD == temp).Single();
                            if (!ServicesM.Services[i].Name.Equals(tr.NameTD))
                                tr.NameTD = ServicesM.Services[i].Name;
                            if (!ServicesM.Services[i].Vat.Equals(tr.VatTD))
                                tr.VatTD = ServicesM.Services[i].Vat;
                            if (!ServicesM.Services[i].Price.Equals(tr.PriceTD))
                                tr.PriceTD = ServicesM.Services[i].Price;
                            if (!ServicesM.Services[i].Details.Equals(tr.DetailsTD))
                                tr.DetailsTD = ServicesM.Services[i].Details;
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

                dialog = new Dialog(false, "Módosítások mentése", Utilities.Loading.Hide);
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
            new FormChecking(Loading.RunWorkerAsync, Dummy, true);
        }
        private void Dummy() { }
        protected internal void NewLine()
        {
            ServicesM.Services.Add(new ServicesM.Service() { New = true });
            modified = true;
        }
        protected internal void ServiceEraseMethod()
        {
            dialog = new Dialog(true, "Szolgáltatás törlése", Erase, Dummy, true);
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
            if (!SelectedRow.Selected.New) ServicesM.Erased.Add(SelectedRow.Selected.ID);
        }
        private void EraseBackgroundComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            ServicesM.Services.Remove(SelectedRow.Selected);
        }
    }
}
