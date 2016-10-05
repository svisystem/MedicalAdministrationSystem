using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics
{
    public class DefaultVM : VMExtender
    {
        public DefaultM DefaultM { get; set; }
        protected internal BackgroundWorker Loading { get; set; }
        private StatisticsM.Step Item { get; set; }
        protected internal DefaultVM(StatisticsM.Step Item)
        {
            this.Item = Item;
            DefaultM = new DefaultM();
            Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private async void LoadingModel(object sender, DoWorkEventArgs e)
        {
            List<string> temp = new List<string>() { "Alkalmazottak", "Páciensek", "Kezelések", "Pénzügy" };
            for (int i = 0; i < temp.Count; i++)
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                {
                    DefaultM.Choices.Add(new DefaultM.Choice()
                    {
                        Id = i,
                        Answer = temp[i],
                        Item = new Button(Item, i, temp[i])
                    });
                }));
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            await Utilities.Loading.Hide();
        }
        protected internal void EnabledChange(bool enabled)
        {
            foreach (DefaultM.Choice item in DefaultM.Choices)
                item.Item.IsEnabled = enabled;
        }
    }
}
