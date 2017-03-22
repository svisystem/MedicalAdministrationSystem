using DevExpress.Xpf.Scheduler;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Schedule;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
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
        private Action<bool> RegistrateEnabled { get; set; }
        protected internal ScheduleVM(Action<bool> RegistrateEnabled)
        {
            this.RegistrateEnabled = RegistrateEnabled;
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

            Erase.DoWork += EraseDoWork;
            Erase.RunWorkerCompleted += EraseComplete;
            Create.DoWork += CreateDoWork;
            Create.RunWorkerCompleted += CreateComplete;
            Modify.DoWork += ModifyDoWork;
            Modify.RunWorkerCompleted += ModifyComplete;
        }
        List<ScheduleM.Doctor> tempdoc = new List<ScheduleM.Doctor>();
        ObservableCollection<ScheduleM.Appointment> tempApp = new ObservableCollection<ScheduleM.Appointment>();
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            tempdoc.Clear();
            tempApp.Clear();

            try
            {
                me = new MedicalModel(ConfigurationManager.Connect());
                me.Database.Connection.Open();

                foreach (ScheduleM.Doctor doctor in me.userdata.Where(a => !me.accountdata.FirstOrDefault(b => b.IdAD == a.AccountDataIdUD).DeletedAD).ToList()
                    .Where(n => me.priviledges.FirstOrDefault(p => p.IdP == me.accountdata.FirstOrDefault(b => b.IdAD == n.AccountDataIdUD).PriviledgesIdAD).IncludeScheduleP).ToList()
                    .Select(a => new ScheduleM.Doctor
                    {
                        Id = a.IdUD,
                        Name = a.NameUD,
                    }))
                    tempdoc.Add(doctor);

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
                            DoctorId = appointment.DoctorIdSD,
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
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                foreach (ScheduleM.Appointment a in tempApp)
                {
                    a.AcceptChanges();
                    ScheduleM.Appointments.Add(a);
                }
                ColorFixer(false);
                await Utilities.Loading.Hide();
            }
            else ConnectionMessage();
        }
        protected internal async void Refresh()
        {
            await Utilities.Loading.Show();
            ScheduleM.Appointments.Clear();
            ScheduleM.Doctors.Clear();
            ScheduleM.Patients.Clear();
            runOnce = false;
            Loading.RunWorkerAsync();
        }
        protected internal void Modified()
        {
            if (ScheduleM.Appointments.Any(s => s.IsChanged)) Modify.RunWorkerAsync();
        }
        private BackgroundWorker Modify = new BackgroundWorker();
        private void ModifyDoWork(object sender, DoWorkEventArgs e)
        {
            foreach (ScheduleM.Appointment appointment in
                ScheduleM.Appointments.Where(s => s.IsChanged && s.StoreInDB))
            {
                try
                {
                    me = new MedicalModel(ConfigurationManager.Connect());
                    me.Database.Connection.Open();

                    scheduledata dbAppointment = me.scheduledata.Where(s => s.IdSD == appointment.Id).Single();

                    if (dbAppointment.StillNotVisitedSD != appointment.StillNotVisited)
                    {
                        if (dbAppointment.StillNotVisitedSD)
                        {

                            scheduleperson_st spst = me.scheduleperson_st.Where(sp => sp.IdSP == dbAppointment.PatientIdSD).Single();
                            me.newperson.Remove(me.newperson.Where(np => np.IdNP == spst.NewPersonIdSP).Single());
                            spst.NewPersonIdSP = null;
                            spst.ExistedIdSP = ScheduleM.Patients.Where(p => p.TajNumber == appointment.PatientTajNumber).Single().Id;
                            dbAppointment.StillNotVisitedSD = false;
                            me.SaveChanges();
                        }
                        else
                        {
                            scheduleperson_st spst = me.scheduleperson_st.Where(sp => sp.IdSP == dbAppointment.PatientIdSD).Single();
                            spst.ExistedIdSP = null;

                            newperson np = new newperson();
                            dbAppointment.StillNotVisitedSD = true;
                            np.PatientNameNP = appointment.PatientName;
                            np.TAJNumberNP = appointment.PatientTajNumber;
                            me.newperson.Add(np);
                            me.SaveChanges();
                            spst.NewPersonIdSP = np.IdNP;
                            me.SaveChanges();
                        }
                        me.SaveChanges();
                    }
                    else if (dbAppointment.StillNotVisitedSD)
                    {
                        newperson newp = me.newperson.Where(per => per.IdNP == me.scheduleperson_st.Where(sp => sp.IdSP == dbAppointment.PatientIdSD).FirstOrDefault().NewPersonIdSP).Single();
                        if (newp.PatientNameNP != appointment.PatientName) newp.PatientNameNP = appointment.PatientName;
                        if (newp.TAJNumberNP != appointment.PatientTajNumber) newp.TAJNumberNP = appointment.PatientTajNumber;
                        me.SaveChanges();
                    }
                    else
                    {
                        scheduleperson_st spst = me.scheduleperson_st.Where(sp => sp.IdSP == dbAppointment.PatientIdSD).Single();
                        spst.ExistedIdSP = ScheduleM.Patients.Where(p => p.TajNumber == appointment.PatientTajNumber).Single().Id;
                        me.SaveChanges();
                    }

                    if (appointment.StillNotVisited != dbAppointment.StillNotVisitedSD) dbAppointment.StillNotVisitedSD = appointment.StillNotVisited;
                    if (appointment.StartTime != dbAppointment.StartSD) dbAppointment.StartSD = appointment.StartTime;
                    if (appointment.EndTime != dbAppointment.FinishSD) dbAppointment.FinishSD = appointment.EndTime;
                    if (appointment.DoctorId != dbAppointment.DoctorIdSD) dbAppointment.DoctorIdSD = appointment.DoctorId;
                    if (appointment.Notes != dbAppointment.NotesSD) dbAppointment.NotesSD = appointment.Notes;
                    if (appointment.Label != dbAppointment.StatusSD) dbAppointment.StatusSD = appointment.Label;
                    me.SaveChanges();

                    me.Database.Connection.Close();
                    workingConn = true;
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                }
            }
        }
        private void ModifyComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn) CollectionGetChanges(false);
            else ConnectionMessage();
        }
        private BackgroundWorker Create = new BackgroundWorker();
        int NewId;
        private void CreateDoWork(object sender, DoWorkEventArgs e)
        {
            ScheduleM.Appointment appointment =
                ScheduleM.Appointments.Where(s => !s.StoreInDB).Single();
            try
            {
                me = new MedicalModel(ConfigurationManager.Connect());
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
                else belong = ScheduleM.Patients.Single(p => p.TajNumber == appointment.PatientTajNumber).Id;

                scheduleperson_st spst;
                if (appointment.StillNotVisited)
                {
                    spst = new scheduleperson_st()
                    {
                        ExistedIdSP = null,
                        NewPersonIdSP = belong
                    };
                    me.scheduleperson_st.Add(spst);
                }
                else if (!me.scheduleperson_st.Any(sc => sc.ExistedIdSP == belong))
                {
                    spst = new scheduleperson_st()
                    {
                        ExistedIdSP = belong,
                        NewPersonIdSP = null
                    };
                    me.scheduleperson_st.Add(spst);
                }
                else spst = me.scheduleperson_st.Single(sc => sc.ExistedIdSP == belong);
                me.SaveChanges();

                scheduledata sd = new scheduledata()
                {
                    StillNotVisitedSD = appointment.StillNotVisited,
                    StartSD = appointment.StartTime,
                    FinishSD = appointment.EndTime,
                    PatientIdSD = spst.IdSP,
                    DoctorIdSD = appointment.DoctorId,
                    NotesSD = appointment.Notes,
                    StatusSD = appointment.Label
                };

                me.scheduledata.Add(sd);
                me.SaveChanges();

                NewId = sd.IdSD;

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                workingConn = false;
            }
        }
        private async void CreateComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                ScheduleM.Appointments.Where(s => !s.StoreInDB).Single().Id = NewId;
                ScheduleM.Appointments.Where(s => !s.StoreInDB).Single().StoreInDB = true;
                ScheduleM.Appointments.Where(s => s.Id == NewId).Single().AcceptChanges();
                CollectionGetChanges(false);
                RegistrateEnabled(false);
                await Utilities.Loading.Hide();
            }
            else ConnectionMessage();
        }
        protected internal void CollectionGetChanges(bool get)
        {
            if (get) ScheduleM.Appointments.CollectionChanged += CollectionChangedMethod;
            else ScheduleM.Appointments.CollectionChanged -= CollectionChangedMethod;
        }
        private async void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            await Utilities.Loading.Show();
            Create.RunWorkerAsync();
        }
        protected internal bool Load;
        private bool runOnce;
        protected internal void ColorFixer(bool from)
        {
            if (from) Load = true;
            if (Load && !runOnce && workingConn)
            {
                try
                {
                    SchedulerColorSchemaCollection colorSchemas =
                        (GlobalVM.StockLayout.actualContent.Content as Views.Schedule.Schedule).scheduler.GetResourceColorSchemasCopy();
                    for (int i = 0; i < tempdoc.Count; i++)
                    {
                        tempdoc[i].Color = colorSchemas[i % colorSchemas.Count].Cell.ToArgb();
                        ScheduleM.Doctors.Add(tempdoc[i]);
                    }
                    runOnce = true;
                }
                catch { }
            }
        }
        private List<int> EraseInt;
        private BackgroundWorker Erase = new BackgroundWorker();
        protected internal async void EraseMethod(List<int> list)
        {
            await Utilities.Loading.Show();
            dialog = new Dialog(true, "Időpont törlése", Erase.RunWorkerAsync, async () => await Utilities.Loading.Hide(), true);
            dialog.content = new TextBlock("Biztosan eltávolítja a kiválasztott bejegyzést?\n" +
                "Az eltávolítást a késöbbiekben nem lehet visszavonni");
            dialog.Start();
            EraseInt = list;
        }
        private void EraseDoWork(object sender, DoWorkEventArgs e)
        {
            foreach (int item in EraseInt)
                try
                {
                    me = new MedicalModel(ConfigurationManager.Connect());
                    me.Database.Connection.Open();

                    scheduledata sd = me.scheduledata.Where(s => s.IdSD == item).FirstOrDefault();
                    if (sd.StillNotVisitedSD) me.newperson.Remove(me.newperson.Where(np => np.IdNP == me.scheduleperson_st.
                        Where(sps => sps.IdSP == sd.PatientIdSD).FirstOrDefault().NewPersonIdSP).Single());
                    me.scheduleperson_st.Remove(me.scheduleperson_st.Where(sps => sps.IdSP == sd.PatientIdSD).Single());
                    me.scheduledata.Remove(sd);
                    me.SaveChanges();

                    me.Database.Connection.Close();
                    workingConn = true;
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    workingConn = false;
                }
        }
        private async void EraseComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                foreach (int Id in EraseInt)
                    ScheduleM.Appointments.Remove(ScheduleM.Appointments.Where(a => a.Id == Id).Single());
                await Utilities.Loading.Hide();
            }
            else ConnectionMessage();
        }
        protected internal async void NewPatient(int Id)
        {
            await new MenuButtonsEnabled()
            {
                modifier = true,
                Id = Id,
                Name = ScheduleM.Appointments.Where(a => a.Id == Id).Single().PatientName,
                Taj = ScheduleM.Appointments.Where(a => a.Id == Id).Single().PatientTajNumber,

            }.LoadItem(GlobalVM.StockLayout.patientsTBI);
        }
    }
}
