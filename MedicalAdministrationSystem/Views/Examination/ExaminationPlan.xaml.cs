using MedicalAdministrationSystem.ViewModels.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class ExaminationPlan : UserControl
    {
        protected internal ExaminationPlanVM ExaminationPlanVM { get; set; }
        public ExaminationPlan()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            ExaminationPlanVM = new ExaminationPlanVM(view_Loaded);
            this.DataContext = ExaminationPlanVM;
            InitializeComponent();
            word.Content = ExaminationPlanVM.WordEditor;
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            //ExaminationPlanVM.Refresh();
        }
        protected internal bool Dirty()
        {
            return ExaminationPlanVM.VMDirty();
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(() =>
                 view.BestFitColumns()), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
        private void Print(object sender, RoutedEventArgs e)
        {
            ExaminationPlanVM.Print();
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            ExaminationPlanVM.Add();
        }
    }
}
