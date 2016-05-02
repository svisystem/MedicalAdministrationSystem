using DevExpress.Xpf.Grid;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.Models.Fragments;
using MedicalAdministrationSystem.ViewModels.Fragments;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class EmptyComboBoxEdit : ViewExtender
    {
        public bool ImportEnable { get; set; } = true;
        public bool ExportEnable { get; set; } = true;
        public bool EraseEnabled { get; set; } = true;
        protected internal ObservableCollection<SelectedPatientM.ExaminationItem> ListProperty
        {
            get { return EmptyComboBoxEditVM.ListPropertyVM; }
            set { EmptyComboBoxEditVM.ListPropertyVM = value; }
        }
        protected internal List<EmptyComboBoxEditM.ErasedItem> ErasedProperty
        {
            get { return EmptyComboBoxEditVM.ErasedProperty; }
            set { EmptyComboBoxEditVM.ErasedProperty = value; }
        }
        protected internal EmptyComboBoxEditVM EmptyComboBoxEditVM { get; set; }
        public EmptyComboBoxEdit()
        {
            EmptyComboBoxEditVM = new EmptyComboBoxEditVM();
            this.DataContext = EmptyComboBoxEditVM;
            InitializeComponent();
        }
        private void EraseItem(object sender, RoutedEventArgs e)
        {
            EmptyComboBoxEditVM.EraseItem();
        }
        private void EraseAll(object sender, RoutedEventArgs e)
        {
            EmptyComboBoxEditVM.EraseAll();
        }
        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => ((GridControl)sender).InvalidateMeasure()), System.Windows.Threading.DispatcherPriority.Render);
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
