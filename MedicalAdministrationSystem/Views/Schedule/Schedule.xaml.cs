using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler;
using MedicalAdministrationSystem.ViewModels.Schedule;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Schedule
{
    public partial class Schedule : UserControl
    {
        protected internal ScheduleVM ScheduleVM { get; set; }
        public Schedule()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            ScheduleVM = new ScheduleVM(RegistrateEnabled);
            this.DataContext = ScheduleVM;
            InitializeComponent();
            scheduler.OptionsCustomization.AllowInplaceEditor = UsedAppointmentType.None;
            scheduler.Views.DayView.AppointmentDisplayOptions.StatusDisplayType = AppointmentStatusDisplayType.Never;
            scheduler.Views.DayView.WorkTime = TimeOfDayInterval.Day;
            scheduler.Views.WorkWeekView.AppointmentDisplayOptions.StatusDisplayType = AppointmentStatusDisplayType.Never;
            scheduler.Views.WorkWeekView.WorkTime = TimeOfDayInterval.Day;
            await Loading.Hide();
        }
        protected internal bool Dirty()
        {
            return false;
        }

        private void scheduler_PopupMenuShowing(object sender, SchedulerMenuEventArgs e)
        {
            e.Menu.ItemLinks.Clear();
        }

        private void ItemClick(object sender, ItemClickEventArgs e)
        {
            if ((sender as BarCheckItem).Name == "biSwitchToGroupByNone") resourceSelector.IsEnabled = false;
            else resourceSelector.IsEnabled = true;
        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            resourceSelector.IsEnabled = false;
            ScheduleVM.ColorFixer(true);
        }

        private void scheduler_EditAppointmentFormShowing(object sender, EditAppointmentFormEventArgs e)
        {
            ScheduleVM.CollectionGetChanges(true);
            e.ViewModel = OwnAppointmentFormViewModel.Create(sender as SchedulerControl, e.Appointment, ScheduleVM.Patients, RegistrateEnabled, ScheduleVM.CollectionGetChanges);
        }
        private void biDeleteAppointment_ItemClick(object sender, ItemClickEventArgs e)
        {
            ScheduleVM.EraseMethod(scheduler.SelectedAppointments.Select(sa => (int)sa.Id).ToList());
        }

        private void biRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ScheduleVM.Refresh();
        }

        private void biEditAppointment_IsEnabledChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && (bool)scheduler.SelectedAppointments[0].CustomFields["StillNotVisited"])
                biRegistratePatient.IsEnabled = true;
            else biRegistratePatient.IsEnabled = false;
        }
        private void RegistrateEnabled(bool enabled)
        {
            biRegistratePatient.IsEnabled = enabled;
        }

        private void scheduler_AppointmentDrag(object sender, AppointmentDragEventArgs e)
        {
            if (scheduler.SelectedAppointments.Any(sa => sa.Start < DateTime.Now)) e.Allow = false;
        }
        private void SchedulerStorage_AppointmentsChanged(object sender, PersistentObjectsEventArgs e)
        {
            ScheduleVM.Modified();
        }

        private void biRegistratePatient_ItemClick(object sender, ItemClickEventArgs e)
        {
            ScheduleVM.NewPatient((int)scheduler.SelectedAppointments[0].Id);
        }

        private void scheduler_SelectionChanged(object sender, EventArgs e)
        {
            if (scheduler.SelectedAppointments.Count == 1 && (bool)scheduler.SelectedAppointments[0].CustomFields["StillNotVisited"])
                biRegistratePatient.IsEnabled = true;
            else biRegistratePatient.IsEnabled = false;
        }
    }
}
