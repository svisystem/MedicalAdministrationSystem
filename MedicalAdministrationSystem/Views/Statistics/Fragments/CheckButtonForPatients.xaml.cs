using System;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Statistics.Fragments
{
    public partial class CheckButtonForPatients : UserControl
    {
        private object Item { get; set; }
        private Action<int> SwitchAllFunctionality { get; set; }
        public CheckButtonForPatients(Action<int> SwitchAllFunctionality, bool? AllSelector = null)
        {
            InitializeComponent();
            this.SwitchAllFunctionality = SwitchAllFunctionality;
            if (AllSelector != null)
            {
                name.FontStyle = FontStyles.Italic;
                checkEdit.IsThreeState = true;
            }
        }
        protected internal void SetItem(object Item)
        {
            this.Item = Item;
            this.DataContext = Item;
        }
        private void ClickEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SwitchAllFunctionality((Item as dynamic).Id);
        }
    }
}
