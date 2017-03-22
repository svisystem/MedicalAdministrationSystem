using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.Models.Statistics.Service;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics.Service
{
    public class ServiceQuestionVM : VMExtender
    {
        public ServiceQuestionM ServiceQuestionM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        protected internal ServiceQuestionVM(StatisticsM.Step Item, bool? All)
        {
            this.Item = Item;
            ServiceQuestionM = new ServiceQuestionM();
            FillQestions(All);
            Start();
        }
        List<string> Questions;
        private void FillQestions(bool? All)
        {
            string temp = All != null ? "ek" : null;
            string price = All != null ? "ai" : "á";
            if (All != null && (bool)All)
                Questions = new List<string>()
                {
                    "Kezelések árainak\nmódosulásai.",
                    "Elvégzett kezelések\nszámosságának alakulása.",
                    "Kezelések számosságának\nalakulása."
                };
            else Questions = new List<string>()
            {
                "A kiválasztott kezelés" + temp + "\nár" + price + "nak módosulásai.",
                "A kiválasztott kezelés" + temp + "ből\nadott időszakban mennyit végeztek el.",
            };
        }
        private async void Start()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < Questions.Count; i++)
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        ServiceQuestionM.Choices.Add(new ServiceQuestionM.Choice()
                        {
                            Id = i,
                            Answer = Questions[i],
                            Item = new Button(Item, i, Questions[i])
                        });
                    }));
            }, CancellationToken.None).ContinueWith(task =>
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                   await Loading.Hide())));
        }
        protected internal void EnabledChange(bool enabled)
        {
            foreach (ServiceQuestionM.Choice item in ServiceQuestionM.Choices)
                item.Item.IsEnabled = enabled;
        }
    }
}
