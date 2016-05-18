using DevExpress.Xpf.Scheduler;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Schedule;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private BackgroundWorker Loading { get; set; }
        protected internal ScheduleVM()
        {
            ScheduleM = new ScheduleM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
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
        }
        List<ScheduleM.Doctor> temp = new List<ScheduleM.Doctor>();
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            temp.Clear();
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
                await Utilities.Loading.Hide();
            }
            else ConnectionMessage();
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
    }
}
