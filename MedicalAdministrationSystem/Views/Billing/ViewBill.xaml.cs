using MedicalAdministrationSystem.ViewModels.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Billing
{
    public partial class ViewBill : UserControl
    {
        protected internal ViewBillVM ViewBillVM { get; set; }
        public ViewBill(int Id)
        {
            Start(Id);
        }
        private async void Start(int Id)
        {
            await Loading.Show();
            InitializeComponent();
            ViewBillVM = new ViewBillVM(Id, ref content);
            this.DataContext = ViewBillVM;
        }
        protected internal bool Dirty()
        {
            return false;
        }
    }
}
