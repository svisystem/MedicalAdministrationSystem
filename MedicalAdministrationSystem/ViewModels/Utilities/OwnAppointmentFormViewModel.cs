using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.UI;
using MedicalAdministrationSystem.Models.Schedule;
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
        public static OwnAppointmentFormViewModel Create(SchedulerControl control, DevExpress.XtraScheduler.Appointment apt, bool readOnly, bool showRecurrenceDialog, ObservableCollection<ScheduleM.Patient> Patients)
        {
            return ViewModelSource.Create(() => new OwnAppointmentFormViewModel(control, apt, readOnly, showRecurrenceDialog) { Patients = Patients });
        }

        public OwnAppointmentFormViewModel(SchedulerControl control, DevExpress.XtraScheduler.Appointment apt, bool readOnly, bool showRecurrenceDialog)
            : base(control, apt, readOnly, showRecurrenceDialog) {

        }
        protected override bool CloseChangedAppointment()
        {
            return true;
        }
    }
}
