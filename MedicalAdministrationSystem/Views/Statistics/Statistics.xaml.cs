using MedicalAdministrationSystem.ViewModels.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics
{
    public partial class Statistics : UserControl
    {
        protected internal StatisticsVM StatisticsVM { get; set; }
        public Statistics()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            StatisticsVM = new StatisticsVM();
            this.DataContext = StatisticsVM;
            InitializeComponent();
        }
        protected internal bool Dirty() => false;
    }
}
