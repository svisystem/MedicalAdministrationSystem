using MedicalAdministrationSystem.ViewModels.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Settings
{
    public partial class Priviledges : UserControl
    {
        protected internal PriviledgesVM PriviledgesVM { get; set; }
        private int currentInt { get; set; }
        public Priviledges()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            PriviledgesVM = new PriviledgesVM(view_Loaded);
            this.DataContext = PriviledgesVM;
            InitializeComponent();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            grid.ClearSorting();
            PriviledgesVM.Refresh();
        }
        protected internal bool Dirty()
        {
            return PriviledgesVM.VMDirty();
        }

        private void newLine(object sender, RoutedEventArgs e)
        {
            PriviledgesVM.NewLine();
        }

        private void Erase(object sender, RoutedEventArgs e)
        {
            PriviledgesVM.PriviledgeEraseMethod();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            PriviledgesVM.ExecuteMethod();
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(delegate
             {
                 view.BestFitColumns();
             }), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
    }
}
