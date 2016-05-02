using MedicalAdministrationSystem.ViewModels.Evidence;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Evidence
{
    public partial class Evidences : UserControl
    {
        protected internal EvidencesVM EvidencesVM { get; set; }
        public Evidences()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            EvidencesVM = new EvidencesVM(view_Loaded);
            this.DataContext = EvidencesVM;
            InitializeComponent();
        }
        protected internal bool Dirty()
        {
            return EvidencesVM.VMDirty();
        }
        private void modify(object sender, RoutedEventArgs e)
        {
            EvidencesVM.Edit();
        }
        private void erase(object sender, RoutedEventArgs e)
        {
            EvidencesVM.EraseMethod();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            EvidencesVM.ExecuteMethod();
        }
        private void view(object sender, RoutedEventArgs e)
        {
            EvidencesVM.View();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            EvidencesVM.Question();
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(() =>
            {
                viewer.BestFitColumns();
                grid.SortBy(grid.Columns[3], DevExpress.Data.ColumnSortOrder.Descending);
            }), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
    }
}
