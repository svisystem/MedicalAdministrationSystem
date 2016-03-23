using MedicalAdministrationSystem.ViewModels.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class ImportExamination : ViewExtender
    {
        protected internal ImportExaminationVM ImportExaminationVM { get; set; }
        public ImportExamination()
        {
            Start();
        }
        private async void Start()
        {
            //await Loading.Show();
            ImportExaminationVM = new ImportExaminationVM();
            this.DataContext = ImportExaminationVM;
            InitializeComponent();
        }
        protected internal bool Dirty()
        {
            return ImportExaminationVM.VMDirty();
        }
        private void ExaminationTime_Spin(object sender, DevExpress.Xpf.Editors.SpinEventArgs e)
        {
            e.Handled = true;
        }
    }
}
