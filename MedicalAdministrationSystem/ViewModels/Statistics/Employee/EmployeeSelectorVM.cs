using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.Models.Statistics.Employee;
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

namespace MedicalAdministrationSystem.ViewModels.Statistics.Employee
{
    public class EmployeeSelectorVM : VMExtender
    {
        public EmployeeSelectorM EmployeeSelectorM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        protected internal EmployeeSelectorVM(StatisticsM.Step Item)
        {
            this.Item = Item;
            EmployeeSelectorM = new EmployeeSelectorM();
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
                         EmployeeSelectorM.Employees.Add(new EmployeeSelectorM.Employee()
                         {
                             Id = 0,
                             Name = "Összes kijelölése",
                             Enabled = false,
                             Button = new CheckButton(SwitchAllFunctionality, true)
                         });
                     }));

                    me = new medicalEntities();
                    me.Database.Connection.Open();

                    foreach (object employee in me.userdata.OrderBy(u => u.NameUD).Select(u => new { u.IdUD, u.NameUD }).ToList())
                    {
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                         {
                             EmployeeSelectorM.Employees.Add(new EmployeeSelectorM.Employee()
                             {
                                 Id = (int)employee.GetType().GetProperty("IdUD", BindingFlags.Instance | BindingFlags.Public |
                                     BindingFlags.NonPublic).GetValue(employee),
                                 Name = (string)employee.GetType().GetProperty("NameUD", BindingFlags.Instance | BindingFlags.Public |
                                     BindingFlags.NonPublic).GetValue(employee),
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
                        foreach (EmployeeSelectorM.Employee employee in EmployeeSelectorM.Employees)
                            (employee.Button as CheckButton).SetItem(employee);
                        if (workingConn) await Loading.Hide();
                        else ConnectionMessage();
                    })));
        }
        bool AllSwitcher;
        private void SwitchAllFunctionality(int Id)
        {
            if (Id != 0)
                if (EmployeeSelectorM.Employees.Where(e => e.Id != 0).Any(e => (bool)e.Enabled) && EmployeeSelectorM.Employees.Where(e => e.Id != 0).Any(e => !(bool)e.Enabled))
                    EmployeeSelectorM.Employees[0].Enabled = null;
                else EmployeeSelectorM.Employees[0].Enabled = AllSwitcher = EmployeeSelectorM.Employees.Where(e => e.Id != 0).Any(e => (bool)e.Enabled);
            else
            {
                EmployeeSelectorM.Employees.Where(e => e.Id == Id).Single().Enabled = AllSwitcher = !AllSwitcher;
                foreach (EmployeeSelectorM.Employee item in EmployeeSelectorM.Employees)
                    item.Enabled = AllSwitcher;
            }
            EmployeeSelectorM.ButtonEnabled = EmployeeSelectorM.Employees.Any(emp => emp.Enabled == true);
        }
        protected internal void Execute()
        {
            if (EmployeeSelectorM.Employees[0].Enabled == true) Item.Answer = new List<int>();
            else Item.Answer = EmployeeSelectorM.Employees.Where(e => e.Id != 0 && (bool)e.Enabled).Select(e => e.Id).ToList();
        }
    }
}
