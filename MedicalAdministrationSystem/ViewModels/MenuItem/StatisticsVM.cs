using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class StatisticsVM : VMExtender
    {
        public StatisticsVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();
        }
        protected internal async void StatisticsLoad()
        {
           await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new Views.Statistics.Statistics()), new Views.Global.StockVerticalMenuItem());
        }
    }
}
