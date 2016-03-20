using DevExpress.Xpf.Navigation;
using MedicalAdministrationSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public class MenuButtonsEnabled
    {
        private List<Item> menuItems { get; set; }
        protected internal bool? modifier = null;

        public MenuButtonsEnabled(priviledges_fx priviledges)
        {
            LoadList();
            foreach (Item item in menuItems)
            {
                if (Convert.ToBoolean(priviledges.GetType().GetProperty(item.DataBaseName, BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.NonPublic).GetValue(priviledges, null)))
                {
                    item.Tile.Visibility = Visibility.Visible;
                }
                else item.Tile.Visibility = Visibility.Collapsed;
            }
        }
        public MenuButtonsEnabled()
        {
            LoadList();
        }
        private void LoadList()
        {
            menuItems = new List<Item>()
            {
                new Item() { Tile = GlobalVM.StockLayout.scheduleTBI, DataBaseName = "ScheduleP" },
                new Item() { Tile = GlobalVM.StockLayout.patientsTBI, DataBaseName = "PatientP" },
                new Item() { Tile = GlobalVM.StockLayout.examinationTBI, DataBaseName = "ExaminationP" },
                new Item() { Tile = GlobalVM.StockLayout.labTBI, DataBaseName = "LabP" },
                new Item() { Tile = GlobalVM.StockLayout.evidenceTBI, DataBaseName = "EvidenceP" },
                new Item() { Tile = GlobalVM.StockLayout.prescriptionTBI, DataBaseName = "PrescriptionP" },
                new Item() { Tile = GlobalVM.StockLayout.billingTBI, DataBaseName = "BillingP" },
                new Item() { Tile = GlobalVM.StockLayout.statisticsTBI, DataBaseName = "StatisticP" },
                new Item() { Tile = GlobalVM.StockLayout.usersTBI, DataBaseName = "UsersP" },
                new Item() { Tile = GlobalVM.StockLayout.settingsTBI, DataBaseName = "SettingP" },
                new Item() { Tile = GlobalVM.StockLayout.helpTBI, DataBaseName = "HelpP" },
                new Item() { Tile = GlobalVM.StockLayout.logoutTBI, DataBaseName = "LogoutP" }
            };
        }
        protected internal void LoadFirst()
        {
            foreach (Item item in menuItems)
            {
                if (Visible(item.Tile) && Enabled(item.Tile))
                {
                    LoadItem(item.Tile);
                    break;
                }
            }
        }
        protected internal void ChangeEnable(TileBarItem tile, bool enabled)
        {
            if (Visible(tile)) tile.IsEnabled = enabled;
        }
        protected internal void SingleChange(TileBarItem item, Visibility vis)
        {
            item.Visibility = vis;
        }
        private bool Visible(TileBarItem item)
        {
            return item.Visibility == Visibility.Visible;
        }
        private bool Enabled(TileBarItem item)
        {
            return item.IsEnabled;
        }
        protected internal void LoadItem(TileBarItem item)
        {
            if (Visible(item) && Enabled(item))
            {
                Loading.Show();
                GlobalVM.StockLayout.ManualChange(item);
                switch (item.Name)
                {
                    case "scheduleTBI":
                        GlobalVM.StockLayout.ScheduleLoad();
                        break;
                    case "patientsTBI":
                        if (modifier == true) GlobalVM.StockLayout.PatiensLoad(true);
                        else GlobalVM.StockLayout.PatiensLoad();
                        break;
                    case "examinationTBI":
                        GlobalVM.StockLayout.ExaminationLoad();
                        break;
                    case "labTBI":
                        GlobalVM.StockLayout.LabLoad();
                        break;
                    case "evidenceTBI":
                        GlobalVM.StockLayout.EvidenceLoad();
                        break;
                    case "prescriptionTBI":
                        GlobalVM.StockLayout.PrescriptionLoad();
                        break;
                    case "billingTBI":
                        GlobalVM.StockLayout.BillingLoad();
                        break;
                    case "statisticsTBI":
                        GlobalVM.StockLayout.StatisticsLoad();
                        break;
                    case "usersTBI":
                        if (modifier != null) GlobalVM.StockLayout.UsersLoad((bool)modifier);
                        else GlobalVM.StockLayout.UsersLoad();
                        break;
                    case "settingsTBI":
                        GlobalVM.StockLayout.SettingsLoad();
                        break;
                    case "helpTBI":
                        GlobalVM.StockLayout.HelpLoad();
                        break;
                    case "logoutTBI":
                        GlobalVM.StockLayout.LogOutLoad();
                        break;
                }
                Loading.Hide();
            }
        }
        private class Item
        {
            protected internal TileBarItem Tile { get; set; }
            protected internal string DataBaseName { get; set; }
        }
    }
}