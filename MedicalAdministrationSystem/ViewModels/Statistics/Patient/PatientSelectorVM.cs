using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.Models.Statistics.Patient;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics.Patient
{
    public class PatientSelectorVM : VMExtender
    {
        public PatientSelectorM PatientSelectorM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        protected internal PatientSelectorVM(StatisticsM.Step Item)
        {
            this.Item = Item;
            PatientSelectorM = new PatientSelectorM();
            Start();
        }
        private async void Start()
        {
            await Task.Run(async () =>
            {
                try
                {
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        PatientSelectorM.Patients.Add(new PatientSelectorM.Patient()
                        {
                            Id = 0,
                            Name = "Összes kijelölése",
                            Enabled = false,
                            Button = new CheckButtonForPatients(SwitchAllFunctionality, true)
                        });
                    }));

                    me = new medicalEntities();
                    me.Database.Connection.Open();

                    foreach (object patient in me.patientdata.OrderBy(u => u.NamePD).Select(u => new { u.IdPD, u.NamePD, u.TAJNumberPD }).ToList())
                    {
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                        {
                            PatientSelectorM.Patients.Add(new PatientSelectorM.Patient()
                            {
                                Id = (int)patient.GetType().GetProperty("IdPD", BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic).GetValue(patient),
                                Name = (string)patient.GetType().GetProperty("NamePD", BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic).GetValue(patient),
                                Taj = (string)patient.GetType().GetProperty("TAJNumberPD", BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic).GetValue(patient),
                                Enabled = false,
                                Button = new CheckButtonForPatients(SwitchAllFunctionality)
                            });
                        }));
                    }

                    me.Database.Connection.Close();
                    workingConn = true;
                }
                catch
                {
                    workingConn = false;
                }
            }, CancellationToken.None).ContinueWith(task =>
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                {
                    foreach (PatientSelectorM.Patient patient in PatientSelectorM.Patients)
                        (patient.Button as CheckButtonForPatients).SetItem(patient);
                    if (workingConn) await Loading.Hide();
                    else ConnectionMessage();
                })));
        }
        bool AllSwitcher;
        private void SwitchAllFunctionality(int Id)
        {
            if (Id != 0)
                if (PatientSelectorM.Patients.Where(p => p.Id != 0).Any(p => (bool)p.Enabled) && PatientSelectorM.Patients.Where(p => p.Id != 0).Any(p => !(bool)p.Enabled))
                    PatientSelectorM.Patients[0].Enabled = null;
                else PatientSelectorM.Patients[0].Enabled = AllSwitcher = PatientSelectorM.Patients.Where(p => p.Id != 0).Any(p => (bool)p.Enabled);
            else
            {
                PatientSelectorM.Patients.Where(p => p.Id == Id).Single().Enabled = AllSwitcher = !AllSwitcher;
                foreach (PatientSelectorM.Patient item in PatientSelectorM.Patients)
                    item.Enabled = AllSwitcher;
            }
            PatientSelectorM.ButtonEnabled = PatientSelectorM.Patients.Where(p => p.Enabled != null).Any(p => (bool)p.Enabled);
        }
        protected internal void Execute()
        {
            if (PatientSelectorM.Patients[0].Enabled == true) Item.Answer = new List<int>();
            else Item.Answer = PatientSelectorM.Patients.Where(p => p.Id != 0 && (bool)p.Enabled).Select(p => p.Id).ToList();
        }
    }
}
