using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public class MainChart : UserControl
    {
        protected internal Action<DateTime> SelectedData { get; set; }
    }
}
