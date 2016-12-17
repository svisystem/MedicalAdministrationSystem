using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.Models.Statistics.Employee;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics.Employee
{
    public class EmployeeQuestionVM : VMExtender
    {
        public EmployeeQuestionM EmployeeQuestionM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        protected internal EmployeeQuestionVM(StatisticsM.Step Item, bool? All)
        {
            this.Item = Item;
            EmployeeQuestionM = new EmployeeQuestionM();
            FillQestions(All);
            Start();
        }
        List<string> Questions;
        private void FillQestions(bool? All)
        {
            string temp = All != null ? "ak" : null;
            if (All != null && (bool)All)
                Questions = new List<string>()
                {
                    "Az adott alkalmazotthoz\nmennyi páciens tartozik.",
                    "Az adott alkalmazott\nmennyi kezelést végzett el.",
                    "Az adott alkalmazott\nmennyi pácienst látott el.",
                    "Az adott alkalmazottnak\nmennyi munkaórája volt.",
                    "Az adott alkalmazott\nmennyit értékesített."
                };
            else Questions = new List<string>()
            {
                "A kiválasztott alkalmazott" + temp + "hoz\nmikor, mennyi páciens tartozott.",
                "A kiválasztott alkalmazott" + temp + " melyik\nkezelésből mennyit végzett el.",
                "A kiválasztott alkalmazott" + temp + " mennyi\npácienst lát" + (All != null ? "tak" : "ott") + " el.",
                "A kiválasztott alkalmazott" + temp + "nak\nmennyi munkaórája van.",
                "A kiválasztott alkalmazott" + temp + "\nmennyit értékesített" + (All != null ? "ek" : null) + "."
            };
        }
        private async void Start()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < Questions.Count; i++)
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        EmployeeQuestionM.Choices.Add(new EmployeeQuestionM.Choice()
                        {
                            Id = i,
                            Answer = Questions[i],
                            Item = new Button(Item, i, Questions[i])
                        });
                    }));
            }, CancellationToken.None).ContinueWith(task =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                    await Loading.Hide()
                ));
            });
        }
        protected internal void EnabledChange(bool enabled)
        {
            foreach (EmployeeQuestionM.Choice item in EmployeeQuestionM.Choices)
                item.Item.IsEnabled = enabled;
        }
    }
}
