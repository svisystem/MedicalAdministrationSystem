using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.Models.Statistics.Finance;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics.Finance
{
    public class FinanceQuestionVM : VMExtender
    {
        public FinanceQuestionM FinanceQuestionM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        protected internal FinanceQuestionVM(StatisticsM.Step Item)
        {
            this.Item = Item;
            FinanceQuestionM = new FinanceQuestionM();
            FillQestions();
            Start();
        }
        List<string> Questions;
        private void FillQestions() =>
            Questions = new List<string>()
            {
                "Mely kezelésekből mennyit\nértékesítettek.",
                "Mennyi volt az összbevétel.",
                "Mely alkalmazottak\nmennyit értékesítettek."
            };
        private async void Start() =>
            await Task.Run(async () =>
            {
                for (int i = 0; i < Questions.Count; i++)
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        FinanceQuestionM.Choices.Add(new FinanceQuestionM.Choice()
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
        protected internal void EnabledChange(bool enabled)
        {
            foreach (FinanceQuestionM.Choice item in FinanceQuestionM.Choices)
                item.Item.IsEnabled = enabled;
        }
    }        
}
