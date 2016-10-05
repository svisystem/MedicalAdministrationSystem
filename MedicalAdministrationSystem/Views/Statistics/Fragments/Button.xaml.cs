using MedicalAdministrationSystem.Models.Statistics;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Fragments
{
    public partial class Button : UserControl
    {
        private StatisticsM.Step Item { get; set; }
        private int Id;
        public Button(StatisticsM.Step Item, int Id, string Title)
        {
            this.Item = Item;
            this.Id = Id;
            InitializeComponent();
            block.Text = Title;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Item.Answer = Id;
        }
    }
}
