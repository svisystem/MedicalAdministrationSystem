using MahApps.Metro.Controls;
using MedicalAdministrationSystem.ViewModels;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Windows;

namespace MedicalAdministrationSystem.Views.Global
{
    public partial class StockWindow : MetroWindow
    {
        public StockWindow()
        {
            InitializeComponent();
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModels.Utilities.Loading.Show();
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.SingleChange(GlobalVM.StockLayout.usersTBI, Visibility.Visible);
            mbe.LoadItem(GlobalVM.StockLayout.usersTBI);
            ViewModels.Utilities.Loading.Hide();
        }
    }
}
