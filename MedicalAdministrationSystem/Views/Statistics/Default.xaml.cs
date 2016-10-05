using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics
{
    public partial class Default : UserControl
    {
        protected internal DefaultVM DefaultVM { get; set; }
        public Default(StatisticsM.Step Item)
        {
            Task.Run(async () => await Loading.Show());
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
