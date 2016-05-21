using DevExpress.Xpf.Scheduler;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Schedule;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace MedicalAdministrationSystem.ViewModels.Schedule
{
    public class ScheduleVM : VMExtender
    {
        protected internal ObservableCollection<ScheduleM.Patient> Patients
        {
            get
            {
                return ScheduleM.Patients;
            }
            private set { }
        }
        public ScheduleM ScheduleM { get; set; }
        protected internal BackgroundWorker Loading { get; set; }
        protected internal ScheduleVM()
        {
            ScheduleM = new ScheduleM();
            Loading = new BackgroundWorker();
            Loading.DoWork += LoadingModel;
            Loading.RunWorkerCompleted += LoadingModelComplete;
            Loading.RunWorkerAsync();

            AppointmentLabel label = ScheduleM.Labels.CreateNewLabel(1, "Előjegyezve", "Előjegyezve", (Color)ColorConverter.ConvertFromString("#FFF8D040"));
            ScheduleM.Labels.Add(label);
            label = ScheduleM.Labels.CreateNewLabel(2, "Megjelent", "Megjelent", (Color)ColorConverter.ConvertFromString("#FF97F26D"));
            ScheduleM.Labels.Add(label);
            label = ScheduleM.Labels.CreateNewLabel(3, "Nem jelent meg", "Nem jelent meg", (Color)ColorConverter.ConvertFromString("#FFFF6961"));
            ScheduleM.Labels.Add(label);
            label = ScheduleM.Labels.CreateNewLabel(4, "Lemondta", "Lemondta", (Color)ColorConverter.ConvertFromString("#FFFFCCEB"));
            ScheduleM.Labels.Add(label);
            label = ScheduleM.Labels.CreateNewLabel(5, "Új időpontot kért", "Új időpontot kért", (Color)ColorConverter.ConvertFromString("#FF9FD3F5"));
            ScheduleM.Labels.Add(label);

            EraseMethod.DoWork += EraseDoWork;
            EraseMethod.RunWorkerCompleted += EraseComplete;
            Create.DoWork += CreateDoWork;
            Create.RunWorkerCompleted += CreateComplete;
        }
        List<ScheduleM.Doctor> temp = new List<ScheduleM.Doctor>();
        ObservableCollection<ScheduleM.Appointment> tempApp = new ObservableCollection<ScheduleM.Appointment>();
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            ScheduleM.Appointments.CollectionChanged -= CollectionChangedMethod;
            temp.Clear();
            tempApp.Clear();

            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                foreach (ScheduleM.Doctor doctor in me.userdata.Where(a => !me.accountdata.Where(b => b.IdAD == a.AccountDataIdUD).FirstOrDefault().DeletedAD).ToList()
                    .Where(n => me.priviledges_fx.Where(p => p.IdP == me.accountdata.Where(b => b.IdAD == n.AccountDataIdUD).FirstOrDefault().PriviledgesIdAD).FirstOrDefault().IncludeScheduleP).ToList()
                    .Select(a => new ScheduleM.Doctor
                    {
                        Id = a.IdUD,
                        Name = a.NameUD,
                    }))
                    temp.Add(doctor);

                foreach (ScheduleM.Patient patient in me.patientdata.OrderBy(p => p.NamePD).Select(p => new ScheduleM.Patient()
                {
                    Id = p.IdPD,
                    Name = p.NamePD,
                    TajNumber = p.TAJNumberPD
                }))
                    ScheduleM.Patients.Add(patient);

                if (me.scheduledata.Count() != 0)
                    foreach (scheduledata appointment in me.scheduledata.ToList())
                    {
                        ScheduleM.Appointment app = new ScheduleM.Appointment()
                        {
                            StillNotVisited = appointment.StillNotVisitedSD,
                            DoctorId = appointment.DoctorIdSD != null ? (int)appointment.DoctorIdSD : 0,
                            EndTime = appointment.FinishSD,
                            Id = appointment.IdSD,
                            Label = appointment.StatusSD,
                            Notes = appointment.NotesSD,
                            PatientName = !appointment.StillNotVisitedSD ? ScheduleM.Patients.Where(p => p.Id == me.scheduleperson_st.
                            Where(sp => sp.IdSP == appointment.PatientIdSD).FirstOrDefault().ExistedIdSP).FirstOrDefault().Name :
                            me.newperson.Where(n => n.IdNP == me.scheduleperson_st.Where(sp => sp.IdSP == appointment.PatientIdSD).
                            FirstOrDefault().NewPersonIdSP).FirstOrDefault().PatientNameNP,
                            StartTime = appointment.StartSD,
                            StoreInDB = true,
                            PatientTajNumber = !appointment.StillNotVisitedSD ? ScheduleM.Patients.Where(p => p.Id == me.scheduleperson_st.
                            Where(sp => sp.IdSP == appointment.PatientIdSD).FirstOrDefault().ExistedIdSP).FirstOrDefault().TajNumber :
                            me.newperson.Where(n => n.IdNP == me.scheduleperson_st.Where(sp => sp.IdSP == appointment.PatientIdSD).
                            FirstOrDefault().NewPersonIdSP).FirstOrDefault().TAJNumberNP
                        };
                        tempApp.Add(app);
                    }

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                ColorFixer(false);
                foreach (ScheduleM.Appointment a in tempApp)
                    ScheduleM.Appointments.Add(a);
                //ScheduleM.Appointments.CollectionChanged += CollectionChangedMethod;
                await Utilities.Loading.Hide();
            }
            else ConnectionMessage();
        }
        protected async internal void Refresh()
        {
            await Utilities.Loading.Show();
            runOnce = false;
            ScheduleM.Appointments.Clear();
            ScheduleM.Doctors.Clear();
            ScheduleM.Patients.Clear();
            Loading.RunWorkerAsync();
        }
        private BackgroundWorker Create = new BackgroundWorker();
        int NewId;
        private void CreateDoWork(object sender, DoWorkEventArgs e)
        {
            ScheduleM.Appointment appointment =
                ScheduleM.Appointments.Where(s => !s.StoreInDB).Single();
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                newperson np = new newperson();
                int belong;
                if (appointment.StillNotVisited)
                {
                    np.PatientNameNP = appointment.PatientName;
                    np.TAJNumberNP = appointment.PatientTajNumber;
                    me.newperson.Add(np);
                    me.SaveChanges();

                    belong = np.IdNP;
                }
                else belong = ScheduleM.Patients.Where(p => p.TajNumber == appointment.PatientTajNumber).Single().Id;

                scheduleperson_st spst = new scheduleperson_st()
                {
                    WhereSP = appointment.StillNotVisited
                };
                if (appointment.StillNotVisited)
                {
                    spst.ExistedIdSP = null;
                    spst.NewPersonIdSP = belong;
                }
                else
                {
                    spst.ExistedIdSP = belong;
                    spst.NewPersonIdSP = null;
                }
                me.scheduleperson_st.Add(spst);
                me.SaveChanges();

                scheduledata sd = new scheduledata()
                {
                    StillNotVisitedSD = appointment.StillNotVisited,
                    StartSD = appointment.StartTime,
                    FinishSD = appointment.EndTime,
                    PatientIdSD = spst.IdSP,
                    NotesSD = appointment.Notes,
                    StatusSD = appointment.Label
                };
                if (appointment.DoctorId != 0) sd.DoctorIdSD = appointment.DoctorId;
                else sd.DoctorIdSD = null;

                me.scheduledata.Add(sd);
                me.SaveChanges();

                NewId = sd.IdSD;

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private async void CreateComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                ScheduleM.Appointments.Where(s => !s.StoreInDB).Single().Id = NewId;
                ScheduleM.Appointments.Where(s => !s.StoreInDB).Single().StoreInDB = true;
                await Utilities.Loading.Hide();
            }
            else ConnectionMessage();
        }
        private async void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            await Utilities.Loading.Show();
            Create.RunWorkerAsync();
        }
        bool runOnce;
        bool viewLoaded;
        protected internal void ColorFixer(bool from)
        {
            ScheduleM.Doctors.Clear();
            if (from) viewLoaded = true;
            if (!runOnce && workingConn && viewLoaded)
            {
                SchedulerColorSchemaCollection colorSchemas = (GlobalVM.StockLayout.actualContent.Content as Views.Schedule.Schedule).scheduler.GetResourceColorSchemasCopy();
                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i].Color = colorSchemas[i % colorSchemas.Count].Cell.ToArgb();
                    ScheduleM.Doctors.Add(temp[i]);
                }
                runOnce = true;
            }
        }
        protected internal int EraseInt;
        protected internal BackgroundWorker EraseMethod = new BackgroundWorker();
        private async void EraseDoWork(object sender, DoWorkEventArgs e)
        {
            await Utilities.Loading.Show();
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                me.scheduledata.Remove(me.scheduledata.Where(s => s.IdSD == EraseInt).FirstOrDefault());
                me.SaveChanges();

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private async void EraseComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn) await Utilities.Loading.Hide();
            else ConnectionMessage();
        }
    }
}
