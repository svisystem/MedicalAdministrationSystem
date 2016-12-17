﻿using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

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
                me = new MedicalModel();
                me.Database.Connection.Open();
                ServicesM.Services.Clear();
                foreach (servicesdata row in me.servicesdata.Where(a => a.DeletedTD == null).ToList())
                    ServicesM.Services.Add(new ServicesM.Service
                    {
                        ID = row.IdTD,
                        Name = row.NameTD,
                        Vat = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == row.IdTD).OrderByDescending(pfs => pfs.IdPFS).FirstOrDefault().VatPFS,
                        Price = me.pricesforeachservice.Where(pfs => pfs.ServiceDataIdPFS == row.IdTD).OrderByDescending(pfs => pfs.IdPFS).FirstOrDefault().PricePFS,
                        Details = row.DetailsTD,
                        Valid = true,
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
                me = new MedicalModel();
                me.Database.Connection.Open();
                if (ServicesM.Erased.Count != 0)
                    foreach (int service in ServicesM.Erased)
                        try
                        {
                            me.servicesdata.Where(a => a.IdTD == service).Single().DeletedTD = DateTime.Now;
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
                            pfs.VatPFS = (int)ServicesM.Services[i].Vat;
                            pfs.PricePFS = (int)ServicesM.Services[i].Price;
                            tr.DetailsTD = ServicesM.Services[i].Details;
                            me.servicesdata.Add(tr);
                            me.SaveChanges();
                            pfs.ServiceDataIdPFS = tr.IdTD;
                            pfs.WhenChangedPFS = DateTime.Now;
                            me.pricesforeachservice.Add(pfs);
                            me.SaveChanges();
                            ServicesM.Services[i].ID = tr.IdTD;
                            ServicesM.Services[i].New = false;
                        }
                        else
                        {
                            int temp = ServicesM.Services[i].ID;
                            tr = me.servicesdata.Where(a => a.IdTD == temp).Single();
                            pfs = me.pricesforeachservice.Where(pf => pf.ServiceDataIdPFS == temp).OrderByDescending(pf => pf.IdPFS).FirstOrDefault();
                            if (!ServicesM.Services[i].Name.Equals(tr.NameTD))
                                tr.NameTD = ServicesM.Services[i].Name;
                            if (string.IsNullOrEmpty(ServicesM.Services[i].Details) || (ServicesM.Services[i].Details != tr.DetailsTD))
                                tr.DetailsTD = ServicesM.Services[i].Details;
                            if (!ServicesM.Services[i].Vat.Equals(pfs.VatPFS) || !ServicesM.Services[i].Price.Equals(pfs.PricePFS))
                            {
                                pfs = new pricesforeachservice()
                                {
                                    PricePFS = (int)ServicesM.Services[i].Price,
                                    VatPFS = (int)ServicesM.Services[i].Vat,
                                    ServiceDataIdPFS = temp,
                                    WhenChangedPFS = DateTime.Now
                                };
                                me.pricesforeachservice.Add(pfs);
                            }
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
        protected internal bool VMDirty() => !modified ? ServicesM.Services.Any(s => s.IsChanged) : true;
        protected internal async void Refresh()
        {
            await Utilities.Loading.Show();
            new FormChecking(Loading.RunWorkerAsync, () => { }, true);
        }
        protected internal void NewLine()
        {
            ServicesM.Services.Add(new ServicesM.Service() { New = true });
            ServicesM.Selected = ServicesM.Services.Last();
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
            ServicesM.Selected = ServicesM.Services.Last();
        }
        protected internal void SetStartValue()
        {
            if (ServicesM.Services.Count > 0) ServicesM.Selected = ServicesM.Services.First();
        }
        protected internal bool ButtonValidate()
        {
            if (ServicesM.Selected != null)
            {
                ServicesM.Selected.Valid = ServicesM.Selected.Name != null &&
                    ServicesM.Selected.Vat != null && ServicesM.Selected.Price != null;
                ServicesM.Services.Single(s => s.ID == ServicesM.Selected.ID).Valid = ServicesM.Selected.Valid;
            }
            return !ServicesM.Services.Any(s => !s.Valid);
        }
    }
}
