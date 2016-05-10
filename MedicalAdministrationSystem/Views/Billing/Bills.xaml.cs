using MedicalAdministrationSystem.ViewModels.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Billing
{
    public partial class Bills : UserControl
    {
        protected internal BillsVM BillsVM { get; set; }
        public Bills()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            BillsVM = new BillsVM(view_Loaded);
            this.DataContext = BillsVM;
            InitializeComponent();
        }
        protected internal bool Dirty()
        {
            return false;
        }
        private void view(object sender, RoutedEventArgs e)
        {
            BillsVM.View();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            //BillsVM.Loading.RunWorkerAsync();
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(() =>
            {
                viewer.BestFitColumns();
                grid.SortBy(grid.Columns[1], DevExpress.Data.ColumnSortOrder.Descending);
            }), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
    }
}
