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
            Check(sender as TileBarItem, () => PatiensLoad(), Back);
        }
        private void examinationTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, ExaminationLoad, Back);
        }
        private void evidenceTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, EvidenceLoad, Back);
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
            HelpLoad();
        }
        private void logoutTBIClick(object sender, EventArgs e)
        {
            Check(sender as TileBarItem, LogOutLoad, Back);
        }
        protected internal async void ScheduleLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Collapsed;
            new ScheduleVM().ScheduleLoad();
            currentItem = scheduleTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void PatiensLoad(bool? modifier = null, string Name = null, string Taj = null, int? Id = null)
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            if (modifier != null) new PatientsVM(earlierItem).PatientDetailsLoad((bool)modifier, Name, Taj, Id);
            else new PatientsVM(earlierItem).PatientListLoad();
            currentItem = patientsTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void ExaminationLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            new ExaminationVM().ExaminationsLoad();
            currentItem = examinationTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void ExaminationLoad(bool modifier, bool imported, int ID)
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            if (modifier) new ExaminationVM().ViewLoad(imported, ID);
            else new ExaminationVM().EditLoad(imported, ID);
            currentItem = examinationTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void EvidenceLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            new EvidenceVM().EvidencesLoad();
            currentItem = evidenceTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void EvidenceLoad(bool modifier, bool imported, int ID)
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            if (modifier) new EvidenceVM().ViewEvidenceLoad(imported, ID);
            else new EvidenceVM().EditEvidenceLoad(imported, ID);
            currentItem = evidenceTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void LabLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            //TODO
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void PrescriptionLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            //TODO
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void BillingLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            new BillingVM().BillsLoad();
            currentItem = billingTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void BillingLoad(int ID)
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            new BillingVM().ViewBillLoad(ID);
            currentItem = billingTBI;
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void StatisticsLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Collapsed;
            new StatisticsVM().StatisticsLoad();
            currentItem = statisticsTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void UsersLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            if (GlobalVM.GlobalM.AccountID == null) new UsersVM().LoginLoad();
            else new UsersVM().DetailsModifyLoad();
            currentItem = usersTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void UsersLoad(bool modifier)
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            if (modifier) new UsersVM() { fromPatient = modifier }.DetailsModifyLoad();
            if (!modifier) new UsersVM().RegistrationLoad();
            currentItem = usersTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal async void SettingsLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            menu.Visibility = System.Windows.Visibility.Visible;
            if (GlobalVM.GlobalM.Secure) new SettingsVM().ConnectionLoad();
            else new SettingsVM().UsersLoad();
            currentItem = settingsTBI;
            SelectedPatient();
            await ViewModels.Utilities.Loading.Hide();
        }
        protected internal void HelpLoad()
        {
            earlierItem = currentItem;
            currentItem = helpTBI;
            new HelpVM().Start();
            Back();
        }
        protected internal async void LogOutLoad()
        {
            await ViewModels.Utilities.Loading.Show();
            new LogoutVM().Click(Back);
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
                tileBar.SelectedItem = tile;
            }
        }
        protected internal void Back()
        {
            currentItem = earlierItem;
            tileBar.SelectedItem = currentItem;
        }
        private void SelectedPatient()
        {
            if (headerContent.Content != null)
                (headerContent.Content as SelectedPatient).Dispose();
        }
        private async void StockLayoutLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModels.Utilities.Loading.Show();
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.SingleChange(usersTBI, System.Windows.Visibility.Visible);
            mbe.SingleChange(helpTBI, System.Windows.Visibility.Visible);
            await mbe.LoadItem(usersTBI);
            await ViewModels.Utilities.Loading.Hide();
        }

        private void tileBar_SelectionChanging(object sender, DevExpress.Xpf.WindowsUI.Base.SelectionChangingEventArgs e)
        {
            if (e.NewValue == helpTBI) e.Cancel = true;
        }
    }
}
