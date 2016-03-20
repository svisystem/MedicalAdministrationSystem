﻿using DevExpress.Xpf.Navigation;
using MedicalAdministrationSystem.ViewModels;
using MedicalAdministrationSystem.ViewModels.MenuItem;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Global
{
    public partial class StockLayout : UserControl
    {
        private TileBarItem currentItem = new TileBarItem();
        private TileBarItem earlierItem { get; set; }
        public StockLayout()
        {
            this.DataContext = this;
            InitializeComponent();
        }
        private void scheduleTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, ScheduleLoad, Back);
        }
        private void patientsTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, PatiensLoad, Back);
        }
        private void evidenceTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, EvidenceLoad, Back);
        }
        private void examinationTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, ExaminationLoad, Back);
        }
        private void labTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, LabLoad, Back);
        }
        private void prescriptionTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, PrescriptionLoad, Back);
        }
        private void billingTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, BillingLoad, Back);
        }
        private void statisticsTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, StatisticsLoad, Back);
        }
        private void usersTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, UsersLoad, Back);
        }
        private void settingsTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, SettingsLoad, Back);
        }
        private void helpTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, HelpLoad, Back);
        }
        private void logoutTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, LogOutLoad, Back);
        }
        protected internal void ScheduleLoad()
        {
            //TODO
            SelectedPatient();
        }
        protected internal void PatiensLoad()
        {
            ViewModels.Utilities.Loading.Show();
            PatientsVM patients = new PatientsVM();
            patients.PatientListLoad();
            currentItem = patientsTBI;
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void PatiensLoad(bool modifier)
        {
            ViewModels.Utilities.Loading.Show();
            PatientsVM patients = new PatientsVM();
            if (modifier) patients.PatientDetailsLoad();
            else patients.PatientListLoad();
            currentItem = patientsTBI;
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void EvidenceLoad()
        {
            ViewModels.Utilities.Loading.Show();
            //TODO
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void ExaminationLoad()
        {
            ViewModels.Utilities.Loading.Show();
            //TODO
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void LabLoad()
        {
            ViewModels.Utilities.Loading.Show();
            //TODO
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void PrescriptionLoad()
        {
            ViewModels.Utilities.Loading.Show();
            //TODO
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void BillingLoad()
        {
            ViewModels.Utilities.Loading.Show();
            BillingVM billing = new BillingVM();
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void StatisticsLoad()
        {
            ViewModels.Utilities.Loading.Show();
            //TODO
            SelectedPatient();
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void UsersLoad()
        {
            ViewModels.Utilities.Loading.Show();
            UsersVM users = new UsersVM();
            if (GlobalVM.GlobalM.AccountName == null) users.LoginLoad();
            else users.DetailsModifyLoad();
            currentItem = usersTBI;
            SelectedPatient();
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void UsersLoad(bool modifier)
        {
            ViewModels.Utilities.Loading.Show();
            UsersVM users = new UsersVM();
            if (modifier) users.fromPatient = modifier;
            if (!modifier) users.RegistrationLoad();
            else
            {
                if (GlobalVM.GlobalM.AccountName == null) users.LoginLoad();
                else users.DetailsModifyLoad();
            }
            currentItem = usersTBI;
            SelectedPatient();
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void SettingsLoad()
        {
            ViewModels.Utilities.Loading.Show();
            SettingsVM settings = new SettingsVM();
            if (GlobalVM.GlobalM.Secure) settings.ConnectionLoad();
            else settings.UsersLoad();
            currentItem = settingsTBI;
            SelectedPatient();
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void HelpLoad()
        {
            ViewModels.Utilities.Loading.Show();
            //TODO
            SelectedPatient();
            ViewModels.Utilities.Loading.Hide();
        }
        protected internal void LogOutLoad()
        {
            ViewModels.Utilities.Loading.Show();
            LogoutVM logout = new LogoutVM();
            logout.Click(Back);
            currentItem = logoutTBI;
            ViewModels.Utilities.Loading.Hide();
        }
        private void Check(TileBarItem select, Action OK, Action No)
        {
            earlierItem = currentItem;
            if (!currentItem.Equals(select))
            {
                ViewModels.Utilities.Loading.Show();
                currentItem = select;
                new FormChecking(OK, No, true);
            }
        }
        protected internal void ManualChange(TileBarItem tile)
        {
            if (!currentItem.Equals(tile))
            {
                earlierItem = currentItem;
                currentItem = tile;
                GlobalVM.StockLayout.tileBar.SelectedItem = tile;
            }
        }
        protected internal void Back()
        {
            currentItem = earlierItem;
            GlobalVM.StockLayout.tileBar.SelectedItem = currentItem;
        }
        private void SelectedPatient()
        {
            if (headerContent.Content != null)
                (headerContent.Content as SelectedPatient).Dispose();
        }
    }
}