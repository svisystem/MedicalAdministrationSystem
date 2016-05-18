﻿using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Scheduler;
using DevExpress.XtraScheduler;
using MedicalAdministrationSystem.ViewModels.Schedule;
using MedicalAdministrationSystem.ViewModels.Utilities;
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
            ScheduleVM = new ScheduleVM();
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
            e.ViewModel = OwnAppointmentFormViewModel.Create(sender as SchedulerControl, e.Appointment, e.ReadOnly, e.OpenRecurrenceDialog, ScheduleVM.Patients);
        }
    }
}