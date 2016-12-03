using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.Models.Statistics.Service;
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

namespace MedicalAdministrationSystem.ViewModels.Statistics.Service
{
    public class ServiceSelectorVM : VMExtender
    {
        public ServiceSelectorM ServiceSelectorM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        protected internal ServiceSelectorVM(StatisticsM.Step Item)
        {
            this.Item = Item;
            ServiceSelectorM = new ServiceSelectorM();
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
                        ServiceSelectorM.Services.Add(new ServiceSelectorM.Service()
                        {
                            Id = 0,
                            Name = "Összes kijelölése",
                            Enabled = false,
                            Button = new CheckButton(SwitchAllFunctionality, true)
                        });
                    }));

                    me = new medicalEntities();
                    me.Database.Connection.Open();

                    foreach (object Service in me.servicesdata.OrderBy(u => u.NameTD).Select(u => new { u.IdTD, u.NameTD }).ToList())
                    {
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                        {
                            ServiceSelectorM.Services.Add(new ServiceSelectorM.Service()
                            {
                                Id = (int)Service.GetType().GetProperty("IdTD", BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic).GetValue(Service),
                                Name = (string)Service.GetType().GetProperty("NameTD", BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic).GetValue(Service),
                                Enabled = false,
                                Button = new CheckButton(SwitchAllFunctionality)
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
                        foreach (ServiceSelectorM.Service Service in ServiceSelectorM.Services)
                            (Service.Button as CheckButton).SetItem(Service);
                        if (workingConn) await Loading.Hide();
                        else ConnectionMessage();
                    })));
        }
        bool AllSwitcher;
        private void SwitchAllFunctionality(int Id)
        {
            if (Id != 0)
                if (ServiceSelectorM.Services.Where(e => e.Id != 0).Any(e => (bool)e.Enabled) && ServiceSelectorM.Services.Where(e => e.Id != 0).Any(e => !(bool)e.Enabled))
                    ServiceSelectorM.Services[0].Enabled = null;
                else ServiceSelectorM.Services[0].Enabled = AllSwitcher = ServiceSelectorM.Services.Where(e => e.Id != 0).Any(e => (bool)e.Enabled);
            else
            {
                ServiceSelectorM.Services.Where(e => e.Id == Id).Single().Enabled = AllSwitcher = !AllSwitcher;
                foreach (ServiceSelectorM.Service item in ServiceSelectorM.Services)
                    item.Enabled = AllSwitcher;
            }
            ServiceSelectorM.ButtonEnabled = ServiceSelectorM.Services.Where(emp => emp.Enabled != null).Any(emp => (bool)emp.Enabled);
        }
        protected internal void Execute()
        {
            if (ServiceSelectorM.Services[0].Enabled == true) Item.Answer = new List<int>();
            else Item.Answer = ServiceSelectorM.Services.Where(e => e.Id != 0 && (bool)e.Enabled).Select(e => e.Id).ToList();
        }
    }
}
