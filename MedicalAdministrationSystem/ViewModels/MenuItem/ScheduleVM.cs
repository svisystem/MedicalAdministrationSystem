using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class ScheduleVM : VMExtender
    {
        public ScheduleVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();
        }
        protected internal async void ScheduleLoad()
        {
            await Loading.Show();
            await ViewLoad(new Func<UserControl>(() => new Views.Schedule.Schedule()), new Views.Global.StockVerticalMenuItem());
        }
    }
}
