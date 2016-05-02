using MedicalAdministrationSystem.ViewModels.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class Examinations : UserControl
    {
        protected internal ExaminationsVM ExaminationsVM { get; set; }
        public Examinations()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            ExaminationsVM = new ExaminationsVM(view_Loaded);
            this.DataContext = ExaminationsVM;
            InitializeComponent();
        }
        protected internal bool Dirty()
        {
            return ExaminationsVM.VMDirty();
        }
        private void modify(object sender, RoutedEventArgs e)
        {
            ExaminationsVM.Edit();
        }
        private void erase(object sender, RoutedEventArgs e)
        {
            ExaminationsVM.ExaminationEraseMethod();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            ExaminationsVM.ExecuteMethod();
        }
        private void select(object sender, RoutedEventArgs e)
        {
            ExaminationsVM.Select();
        }
        private void view(object sender, RoutedEventArgs e)
        {
            ExaminationsVM.View();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            ExaminationsVM.Question();
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(() =>
            {
                viewer.BestFitColumns();
                grid.SortBy(grid.Columns[4], DevExpress.Data.ColumnSortOrder.Descending);
            }), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
    }
}
