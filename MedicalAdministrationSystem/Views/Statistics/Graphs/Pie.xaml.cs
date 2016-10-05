using MedicalAdministrationSystem.Models.Statistics;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class Pie : UserControl
    {
        public ObservableCollection<ChartM.Record> Data { get; set; }
        public Pie(ObservableCollection<ChartM.Record> Data)
        {
            this.Data = Data;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
