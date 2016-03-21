using MedicalAdministrationSystem.ViewModels.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Settings
{
    public partial class UserAdministrate : UserControl
    {
        protected internal UserAdministrateVM UserAdministrateVM { get; set; }
        private int currentInt { get; set; }
        public UserAdministrate()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            UserAdministrateVM = new UserAdministrateVM(view_Loaded);
            this.DataContext = UserAdministrateVM;
            InitializeComponent();
        }
        private void UsersExecute(object sender, RoutedEventArgs e)
        {
            UserAdministrateVM.ExecuteMethod();
        }
        protected internal bool Dirty()
        {
            return UserAdministrateVM.VMDirty();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            grid.ClearSorting();
            UserAdministrateVM.Refresh();
        }
        private void Erase(object sender, RoutedEventArgs e)
        {
            UserAdministrateVM.UserEraseMethod();
        }
        private void NewPass(object sender, RoutedEventArgs e)
        {
            UserAdministrateVM.NewPassMethod();
        }
        private void view_Loaded()
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                view.BestFitColumns();
            }), DispatcherPriority.Loaded);
            Loading.Hide();
        }
    }
}
