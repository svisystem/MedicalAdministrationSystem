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
        protected internal bool Dirty() => UserAdministrateVM.VMDirty();
        private void Update(object sender, RoutedEventArgs e)
        {
            grid.ClearSorting();
            UserAdministrateVM.Refresh();
        }
        private void NewPass(object sender, RoutedEventArgs e)
        {
            UserAdministrateVM.NewPassMethod();
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(() =>
                 view.BestFitColumns()), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
    }
}
