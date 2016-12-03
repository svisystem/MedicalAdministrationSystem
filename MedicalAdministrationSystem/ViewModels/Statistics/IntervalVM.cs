using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalAdministrationSystem.ViewModels.Statistics
{
    public class IntervalVM : VMExtender
    {
        public IntervalM IntervalM { get; set; }
        protected internal StatisticsM.Step Item { get; set; }
        private Action CreateEnabled { get; set; }
        protected internal IntervalVM(StatisticsM.Step Item, Action CreateEnabled)
        {
            this.Item = Item;
            this.CreateEnabled = CreateEnabled;
            IntervalM = new IntervalM();
            IntervalM.PropertyChanged += SelectedScale_PropertyChanged;
            Start();
            Task.Run(async () => await Loading.Hide());
        }
        private void Start()
        {
            List<string> temp = new List<string>() { "Napi", "Heti", "Havi", "Éves" };
            for (int i = 0; i < temp.Count; i++)
                IntervalM.Scales.Add(new IntervalM.Scale()
                {
                    Id = i + 1,
                    Enabled = false,
                    Title = temp[i]
                });
        }
        protected internal void ScalesEnabler(bool? enabled = null)
        {
            if (enabled != null)
                foreach (IntervalM.Scale item in IntervalM.Scales) item.Enabled = (bool)enabled;
            else
            {
                DateTime temp;
                if (IntervalM.IntervalStart != null && IntervalM.IntervalEnd != null)
                {
                    temp = (DateTime)IntervalM.IntervalStart;
                    if (temp.AddYears(1) > IntervalM.IntervalEnd) IntervalM.Scales[3].Enabled = false;
                    else IntervalM.Scales[3].Enabled = true;
                    if (temp.AddMonths(1) > IntervalM.IntervalEnd) IntervalM.Scales[2].Enabled = false;
                    else IntervalM.Scales[2].Enabled = true;
                    if (temp.AddDays(7) > IntervalM.IntervalEnd) IntervalM.Scales[1].Enabled = false;
                    else IntervalM.Scales[1].Enabled = true;
                    if (temp.AddDays(1) > IntervalM.IntervalEnd) IntervalM.Scales[0].Enabled = false;
                    else IntervalM.Scales[0].Enabled = true;

                }
            }
            if (IntervalM.SelectedScale != null && !IntervalM.SelectedScale.Enabled) IntervalM.SelectedScale = null;
        }
        private void SelectedScale_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedScale") CreateEnabled();
        }
        protected internal bool SelectedScaleNotNull()
        {
            return IntervalM.SelectedScale != null;
        }
        private object Answer(string Name)
        {
            if (Name == "FixDate")
                return new List<object>
                {
                    0, IntervalM.FixDate
                };
            else if (Name == "IntervalDate")
                return new List<object>
                {
                    1,
                    IntervalM.SelectedScale.Id,
                    IntervalM.IntervalStart,
                    IntervalM.IntervalEnd
                };
            else if (Name == "Continuous")
                return new List<object>
                {
                    2, IntervalM.SelectedScale.Id
                };
            return null;
        }
        protected internal void Create(string Name) =>
            Item.Answer = Answer(Name);
    }
}
