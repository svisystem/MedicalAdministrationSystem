using DevExpress.Xpf.Navigation;
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
        protected async internal void PatiensLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            PatientsVM patients = new PatientsVM(earlierItem);
            patients.PatientListLoad();
            currentItem = patientsTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void PatiensLoad(bool modifier)
        {
            await ViewModels.Utilities.Loading.Show();
            PatientsVM patients = new PatientsVM(earlierItem);
            if (modifier) patients.PatientDetailsLoad();
            else patients.PatientListLoad();
            currentItem = patientsTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void EvidenceLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            //TODO
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void ExaminationLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            ExaminationVM examination = new ExaminationVM();
            examination.ExaminationsLoad();
            currentItem = examinationTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void LabLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            //TODO
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void PrescriptionLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            //TODO
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void BillingLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            BillingVM billing = new BillingVM();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void StatisticsLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            //TODO
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void UsersLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            UsersVM users = new UsersVM();
            if (GlobalVM.GlobalM.AccountID == null) users.LoginLoad();
            else users.DetailsModifyLoad();
            currentItem = usersTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void UsersLoad(bool modifier)
        {
            await ViewModels.Utilities.Loading.Show();
            UsersVM users = new UsersVM();
            if (modifier) users.fromPatient = modifier;
            if (!modifier) users.RegistrationLoad();
            else
            {
                if (GlobalVM.GlobalM.AccountID == null) users.LoginLoad();
                else users.DetailsModifyLoad();
            }
            currentItem = usersTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void SettingsLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            SettingsVM settings = new SettingsVM();
            if (GlobalVM.GlobalM.Secure) settings.ConnectionLoad();
            else settings.UsersLoad();
            currentItem = settingsTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void HelpLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            //TODO
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected async internal void LogOutLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            LogoutVM logout = new LogoutVM();
            logout.Click(Back);
            currentItem = logoutTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        private async void Check(TileBarItem select, Action OK, Action No)
        {
            earlierItem = currentItem;
            if (!currentItem.Equals(select))
            {
                await ViewModels.Utilities.Loading.Show();
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
