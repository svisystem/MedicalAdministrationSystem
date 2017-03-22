using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.UI;
using MedicalAdministrationSystem.Models.Schedule;
using System;
using System.Collections.ObjectModel;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public class OwnAppointmentFormViewModel : AppointmentFormViewModel
    {
        public ObservableCollection<ScheduleM.Patient> Patients
        {
            get;
            set;
        }
        public Action<bool> RegistrateEnabled { get; set; }
        public Action<bool> CollectionGetChanges { get; set; }
        public static OwnAppointmentFormViewModel Create(SchedulerControl control, DevExpress.XtraScheduler.Appointment apt,
            ObservableCollection<ScheduleM.Patient> Patients, Action<bool> RegistrateEnabled, Action<bool> CollectionGetChanges)
        {
            return ViewModelSource.Create(() => new OwnAppointmentFormViewModel(control, apt)
            {
                Patients = Patients,
                RegistrateEnabled = RegistrateEnabled,
                CollectionGetChanges = CollectionGetChanges
            });
        }

        public OwnAppointmentFormViewModel(SchedulerControl control, DevExpress.XtraScheduler.Appointment apt)
            : base(control, apt)
        { }
        protected override bool CloseChangedAppointment()
        {
            return true;
        }
    }
}
