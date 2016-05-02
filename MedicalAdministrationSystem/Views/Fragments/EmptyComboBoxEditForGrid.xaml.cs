using DevExpress.Xpf.Grid;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Fragments;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class EmptyComboBoxEditForGrid : ViewExtender
    {
        public bool ImportEnable { get; set; } = false;
        public bool ExportEnable { get; set; } = true;
        public string Count { get; set; }
        protected internal ObservableCollection<SelectedPatientM.ExaminationItem> List
        {
            get { return EmptyComboBoxEditVM.ListPropertyVM; }
            set { EmptyComboBoxEditVM.ListPropertyVM = value; }
        }
        protected internal EmptyComboBoxEditVM EmptyComboBoxEditVM { get; set; }
        public EmptyComboBoxEditForGrid()
        {
            EmptyComboBoxEditVM = new EmptyComboBoxEditVM();
            this.DataContext = EmptyComboBoxEditVM;
            InitializeComponent();
        }
        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => ((GridControl)sender).InvalidateMeasure()), DispatcherPriority.Render);
        }
        private void Import(object sender, RoutedEventArgs e)
        {
            if (EmptyComboBoxEditVM.ImportCount()) associatedExaminations.IsPopupOpen = false;
            EmptyComboBoxEditVM.Import();
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            EmptyComboBoxEditVM.Export();
        }
    }
}
