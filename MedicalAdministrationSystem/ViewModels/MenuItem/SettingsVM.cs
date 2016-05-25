using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using MedicalAdministrationSystem.Views.Settings;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class SettingsVM : VMExtender
    {
        private StockVerticalMenuItem services { get; set; }
        private StockVerticalMenuItem users { get; set; }
        private StockVerticalMenuItem priviledges { get; set; }
        private StockVerticalMenuItem facilityData { get; set; }
        private StockVerticalMenuItem connection { get; set; }
        private StockVerticalMenuItem security { get; set; }
        public SettingsVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();

            services = new StockVerticalMenuItem();
            users = new StockVerticalMenuItem();
            priviledges = new StockVerticalMenuItem();
            facilityData = new StockVerticalMenuItem();
            connection = new StockVerticalMenuItem();
            security = new StockVerticalMenuItem();

            services.button.Content = "Szolgáltatások\nkezelése";
            users.button.Content = "Felhasználók";
            priviledges.button.Content = "Jogosultságok";
            facilityData.button.Content = "Intézmény adatai";
            connection.button.Content = "Adatbázis kapcsolat\nbeállítása";
            security.button.Content = "Biztonsági profil";

            GlobalVM.StockLayout.verticalMenu.Children.Add(services);
            GlobalVM.StockLayout.verticalMenu.Children.Add(users);
            GlobalVM.StockLayout.verticalMenu.Children.Add(priviledges);
            GlobalVM.StockLayout.verticalMenu.Children.Add(facilityData);
            GlobalVM.StockLayout.verticalMenu.Children.Add(connection);
            GlobalVM.StockLayout.verticalMenu.Children.Add(security);

            if (GlobalVM.GlobalM.Secure)
            {
                services.button.IsEnabled = false;
                users.button.IsEnabled = false;
                priviledges.button.IsEnabled = false;
                facilityData.button.IsEnabled = false;
                connection.button.Click += ConnectionView;
                security.button.IsEnabled = false;
            }
            else
            {
                services.button.Click += ServicesView;
                users.button.Click += UsersView;
                priviledges.button.Click += PriviledgesView;
                facilityData.button.Click += FacilityDataView;
                connection.button.Click += ConnectionView;
                security.button.Click += SecurityView;
            }
        }
        private void ServicesView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ServicesLoad, Back);
        }
        private void UsersView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, UsersLoad, Back);
        }
        private void PriviledgesView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, PriviledgesLoad, Back);
        }
        private void FacilityDataView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, FacilityDataLoad, Back);
        }
        private void ConnectionView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ConnectionLoad, Back);
        }
        private void SecurityView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, SecurityLoad, Back);
        }
        protected internal async void ServicesLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new Services()), services);
        }
        protected internal async void UsersLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new UserAdministrate()), users);
        }
        protected internal async void PriviledgesLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new Priviledges()), priviledges);
        }
        protected internal async void FacilityDataLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new FacilityData()), facilityData);
        }
        protected internal async void ConnectionLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new Connection()), connection);
        }
        protected internal async void SecurityLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new Security(SecurityLoad)), security);
        }
    }
}
