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
        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModels.Utilities.Loading.Show();
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.SingleChange(GlobalVM.StockLayout.usersTBI, Visibility.Visible);
            mbe.LoadItem(GlobalVM.StockLayout.usersTBI);
            await ViewModels.Utilities.Loading.Hide();
        }
    }
}
