using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics
{
    public partial class Default : UserControl
    {
        protected internal DefaultVM DefaultVM { get; set; }
        public Default(StatisticsM.Step Item)
        {
            Start(Item);
        }
        private async void Start(StatisticsM.Step Item)
        {
            await Loading.Show();
            DefaultVM = new DefaultVM(Item);
            this.DataContext = DefaultVM;
            InitializeComponent();
        }
        protected internal void EnabledChange(bool enabled)
        {
            DefaultVM.EnabledChange(enabled);
        }
    }
}
