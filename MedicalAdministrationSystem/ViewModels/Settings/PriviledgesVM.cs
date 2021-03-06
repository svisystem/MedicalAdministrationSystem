﻿using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Settings
{
    public class PriviledgesVM : VMExtender
    {
        public PriviledgesM PriviledgesM { get; set; }
        public PriviledgeSelectedRow PriviledgeSelectedRow { get; set; }
        private BackgroundWorker Loading { get; set; }
        private BackgroundWorker Execute { get; set; }
        private BackgroundWorker EraseBackground { get; set; }
        private bool modified { get; set; }
        private Action Loaded { get; set; }
        protected internal PriviledgesVM(Action Loaded)
        {
            this.Loaded = Loaded;
            PriviledgesM = new PriviledgesM();
            PriviledgeSelectedRow = new PriviledgeSelectedRow();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        protected internal async void Refresh()
        {
            await Utilities.Loading.Show();
            new FormChecking(Loading.RunWorkerAsync, () => { }, true);
        }
        private async void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new MedicalModel(ConfigurationManager.Connect()))
                {
                    await me.Database.Connection.OpenAsync();
                    PriviledgesM.PriviledgesList = me.priviledges.ToList();
                    PriviledgesM.Priviledges.Clear();
                    foreach (priviledges row in PriviledgesM.PriviledgesList)
                        PriviledgesM.Priviledges.Add(new Priviledge
                        {
                            IdP = row.IdP,
                            NameP = row.NameP,
                            ScheduleP = row.ScheduleP,
                            PatientP = row.PatientP,
                            ExaminationP = row.ExaminationP,
                            LabP = row.LabP,
                            EvidenceP = row.EvidenceP,
                            PrescriptionP = row.PrescriptionP,
                            BillingP = row.BillingP,
                            StatisticP = row.StatisticP,
                            SettingP = row.SettingP,
                            AllSeeP = row.AllSeeP,
                            IsDoctorP = row.IsDoctorP,
                            IncludeScheduleP = row.IncludeScheduleP,
                            JustImportDocumentsP = row.JustImportDocumentsP,
                            New = false,
                            Enabled = true
                        });
                }
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
                FindMyself();
                foreach (object row in PriviledgesM.Priviledges)
                    (row as Priviledge).AcceptChanges();
                PriviledgesM.AcceptChanges();
                Loaded();
                modified = false;
            }
            else ConnectionMessage();
        }
        private void FindMyself()
        {
            PriviledgesM.Priviledges.Where(p => p.NameP == PriviledgesM.PriviledgesList.Where
            (l => l.IdP.Equals(GlobalVM.GlobalM.PriviledgeID)).Select(l => l.NameP).Single()).Single().Enabled = false;
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
                    if (PriviledgesM.Erased.Count != 0)
                    {
                        foreach (int priviledge in PriviledgesM.Erased)
                        {
                            try
                            {
                                me.priviledges.Remove(me.priviledges.Where(a => a.IdP == priviledge).Single());
                                await me.SaveChangesAsync();
                            }
                            catch { }
                        }
                    }
                    for (int i = 0; i < PriviledgesM.Priviledges.Count; i++)
                    {
                        int temp = PriviledgesM.Priviledges[i].IdP;
                        try
                        {
                            priviledges pr = new priviledges();
                            if (PriviledgesM.Priviledges[i].New)
                            {
                                pr.NameP = PriviledgesM.Priviledges[i].NameP;
                                pr.ScheduleP = PriviledgesM.Priviledges[i].ScheduleP;
                                pr.PatientP = PriviledgesM.Priviledges[i].PatientP;
                                pr.ExaminationP = PriviledgesM.Priviledges[i].ExaminationP;
                                pr.LabP = PriviledgesM.Priviledges[i].LabP;
                                pr.EvidenceP = PriviledgesM.Priviledges[i].EvidenceP;
                                pr.PrescriptionP = PriviledgesM.Priviledges[i].PrescriptionP;
                                pr.BillingP = PriviledgesM.Priviledges[i].BillingP;
                                pr.StatisticP = PriviledgesM.Priviledges[i].StatisticP;
                                pr.SettingP = PriviledgesM.Priviledges[i].SettingP;
                                pr.AllSeeP = PriviledgesM.Priviledges[i].AllSeeP;
                                pr.IsDoctorP = PriviledgesM.Priviledges[i].IsDoctorP;
                                pr.IncludeScheduleP = PriviledgesM.Priviledges[i].IncludeScheduleP;
                                pr.JustImportDocumentsP = PriviledgesM.Priviledges[i].JustImportDocumentsP;
                                me.priviledges.Add(pr);
                                await me.SaveChangesAsync();
                            }
                            else
                            {
                                pr = me.priviledges.Where(a => a.IdP == temp).Single();
                                if (!PriviledgesM.Priviledges[i].NameP.Equals(pr.NameP))
                                {
                                    pr.NameP = PriviledgesM.Priviledges[i].NameP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].ScheduleP.Equals(pr.ScheduleP))
                                {
                                    pr.ScheduleP = PriviledgesM.Priviledges[i].ScheduleP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].PatientP.Equals(pr.PatientP))
                                {
                                    pr.PatientP = PriviledgesM.Priviledges[i].PatientP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].ExaminationP.Equals(pr.ExaminationP))
                                {
                                    pr.ExaminationP = PriviledgesM.Priviledges[i].ExaminationP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].LabP.Equals(pr.LabP))
                                {
                                    pr.LabP = PriviledgesM.Priviledges[i].LabP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].EvidenceP.Equals(pr.EvidenceP))
                                {
                                    pr.EvidenceP = PriviledgesM.Priviledges[i].EvidenceP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].PrescriptionP.Equals(pr.PrescriptionP))
                                {
                                    pr.PrescriptionP = PriviledgesM.Priviledges[i].PrescriptionP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].BillingP.Equals(pr.BillingP))
                                {
                                    pr.BillingP = PriviledgesM.Priviledges[i].BillingP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].StatisticP.Equals(pr.StatisticP))
                                {
                                    pr.StatisticP = PriviledgesM.Priviledges[i].StatisticP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].SettingP.Equals(pr.SettingP))
                                {
                                    pr.SettingP = PriviledgesM.Priviledges[i].SettingP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].AllSeeP.Equals(pr.AllSeeP))
                                {
                                    pr.AllSeeP = PriviledgesM.Priviledges[i].AllSeeP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].IsDoctorP.Equals(pr.IsDoctorP))
                                {
                                    pr.IsDoctorP = PriviledgesM.Priviledges[i].IsDoctorP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].IncludeScheduleP.Equals(pr.IncludeScheduleP))
                                {
                                    pr.IncludeScheduleP = PriviledgesM.Priviledges[i].IncludeScheduleP;
                                    await me.SaveChangesAsync();
                                }
                                if (!PriviledgesM.Priviledges[i].JustImportDocumentsP.Equals(pr.JustImportDocumentsP))
                                {
                                    pr.JustImportDocumentsP = PriviledgesM.Priviledges[i].JustImportDocumentsP;
                                    await me.SaveChangesAsync();
                                }
                            }
                        }
                        catch { }
                    }
                    await me.SaveChangesAsync();
                    PriviledgesM.Erased.Clear();
                }
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                foreach (object row in PriviledgesM.Priviledges)
                {
                    (row as Priviledge).AcceptChanges();
                }
                PriviledgesM.AcceptChanges();
                modified = false;

                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        protected internal bool VMDirty()
        {
            if (PriviledgesM.IsChanged) return true;
            if (PriviledgesM.Priviledges.Any(p => p.IsChanged)) return true;
            return modified;
        }
        protected internal void NewLine()
        {
            PriviledgesM.Priviledges.Add(new Priviledge() { New = true, Enabled = true });
        }
        protected internal void PriviledgeEraseMethod()
        {
            dialog = new Dialog(true, "Jogosultsági profil törlése", Erase, () => { }, true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott jogosultsági profilt?\n" +
                "A jogosultsági profil törlése csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private async void Erase()
        {
            await Utilities.Loading.Show();
            EraseBackground = new BackgroundWorker();
            EraseBackground.DoWork += new DoWorkEventHandler(EraseBackgroundDoWork);
            EraseBackground.RunWorkerCompleted += new RunWorkerCompletedEventHandler(EraseBackgroundComplete);
            EraseBackground.RunWorkerAsync();
        }
        int listtemp;
        private async void EraseBackgroundDoWork(object sender, DoWorkEventArgs e)
        {
            if (!PriviledgeSelectedRow.Selected.New)
            {
                int temp = PriviledgesM.PriviledgesList.Where(b => b.IdP == PriviledgeSelectedRow.Selected.IdP).Select(b => b.IdP).Single();
                try
                {
                    using (me = new MedicalModel(ConfigurationManager.Connect()))
                    {
                        await me.Database.Connection.OpenAsync();
                        listtemp = me.accountdata.Where(a => a.PriviledgesIdAD == temp).Count();
                    }
                    workingConn = true;
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                }
                if (listtemp == 0)
                {
                    PriviledgesM.Erased.Add(PriviledgesM.Priviledges.Where(a => a.IdP == PriviledgeSelectedRow.Selected.IdP).Select(a => a.IdP).Single());
                }
            }
        }
        private async void EraseBackgroundComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            modified = true;
            if (listtemp == 0) PriviledgesM.Priviledges.Remove(PriviledgeSelectedRow.Selected);
            else
            {
                dialog = new Dialog(true, "Jogosultsági profil törlése", Loaded);
                dialog.content = new TextBlock("A jogosultsági profil törlése nem sikerült\n" +
                    "Valószinűleg az adatbázis függőségei miatt, kérjük ezeket előbb szüntesse meg");
                dialog.Start();
            }
            await Utilities.Loading.Hide();
        }
    }
}
