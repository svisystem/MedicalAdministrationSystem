using MedicalAdministrationSystem.ViewModels.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Settings
{
    public partial class Services : UserControl
    {
        protected internal ServicesVM ServicesVM { get; set; }
        public Services()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            ServicesVM = new ServicesVM(view_Loaded);
            this.DataContext = ServicesVM;
            InitializeComponent();
        }
        private void Erase(object sender, RoutedEventArgs e)
        {
            ServicesVM.ServiceEraseMethod();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            ServicesVM.ExecuteMethod();
        }
        private void newLine(object sender, RoutedEventArgs e)
        {
            ServicesVM.NewLine();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            ServicesVM.Refresh();
        }
        protected internal bool Dirty()
        {
            return ServicesVM.VMDirty();
        }
        private void vatErase(object sender, RoutedEventArgs e)
        {
            vat.Text = "0";
        }
        private void priceErase(object sender, RoutedEventArgs e)
        {
            price.Text = "0";
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
