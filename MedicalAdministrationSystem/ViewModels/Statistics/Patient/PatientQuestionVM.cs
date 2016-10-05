using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.Models.Statistics.Patient;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics.Patient
{
    public class PatientQuestionVM : VMExtender
    {
        public PatientQuestionM PatientQuestionM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        protected internal PatientQuestionVM(StatisticsM.Step Item, bool? All)
        {
            this.Item = Item;
            PatientQuestionM = new PatientQuestionM();
            FillQestions(All);
            Start();
        }
        List<string> Questions;
        private void FillQestions(bool? All)
        {
            string temp = All != null ? "ek" : null;
            if (All != null && (bool)All)
                Questions = new List<string>()
                {
                    "Mennyi új, illetve\nmeglévő páciensünk van.",
                    "Előjegyzés státuszainak\neloszlása.",
                    "Melyik pácienshez mennyi\nstátuszbejegyzés tartozik.",
                    "Ki mennyi időt töltött\naz intézményben."
                };
            else Questions = new List<string>()
            {
                "A kiválasztott páciens" + temp + "hez milyen\nelőjegyzési státuszból mennyi tartozik.",
                "A kiválasztott páciens" + temp + "hez\nmennyi státuszbejegyzés tartozik.",
                "A kiválasztott páciens" + temp + " mennyi\nidőt töltött" + temp + " az intézményben."
            };
        }
        private async void Start()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < Questions.Count; i++)
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        PatientQuestionM.Choices.Add(new PatientQuestionM.Choice()
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
            foreach (PatientQuestionM.Choice item in PatientQuestionM.Choices)
                item.Item.IsEnabled = enabled;
        }
    }
}
