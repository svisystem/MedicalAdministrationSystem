using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public abstract class ChartBase : UserControl
    {
        protected internal Action<DateTime> SelectedData { get; set; }
        protected internal abstract void SetVisualRange();
    }
}
