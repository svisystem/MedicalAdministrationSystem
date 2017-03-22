using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics;
using MedicalAdministrationSystem.Views.Statistics.Employee;
using MedicalAdministrationSystem.Views.Statistics.Finance;
using MedicalAdministrationSystem.Views.Statistics.Patient;
using MedicalAdministrationSystem.Views.Statistics.Service;
using System.Collections.Generic;

namespace MedicalAdministrationSystem.ViewModels.Statistics
{
    public class StatisticsVM : VMExtender
    {
        public StatisticsM StatisticsM { get; set; }
        protected internal StatisticsVM()
        {
            Start();
        }
        private async void Start()
        {
            StatisticsM = new StatisticsM();
            StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 0 });
            StatisticsM.Steps[0].Item = new Default(StatisticsM.Steps[0]);
            StatisticsM.Steps[0].PropertyChanged += DefaultChanged;
            await Loading.Hide();
        }
        private void DefaultChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[0].Item as Default).EnabledChange(StatisticsM.Steps[0].Answer != null ? false : true);
            if (StatisticsM.Steps[0].Answer as int? == 0)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 1 });
                StatisticsM.Steps[1].Item = new EmployeeSelector(StatisticsM.Steps[1], DeleteItems);
                StatisticsM.Steps[1].PropertyChanged += EmployeeSelectorChanged;
            }
            else if (StatisticsM.Steps[0].Answer as int? == 1)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 1 });
                StatisticsM.Steps[1].Item = new PatientSelector(StatisticsM.Steps[1], DeleteItems);
                StatisticsM.Steps[1].PropertyChanged += PatientSelectorChanged;
            }
            else if (StatisticsM.Steps[0].Answer as int? == 2)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 1 });
                StatisticsM.Steps[1].Item = new ServiceSelector(StatisticsM.Steps[1], DeleteItems);
                StatisticsM.Steps[1].PropertyChanged += ServiceSelectorChanged;
            }
            else if (StatisticsM.Steps[0].Answer as int? == 3)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 1 });
                StatisticsM.Steps[1].Item = new FinanceQuestion(StatisticsM.Steps[1], DeleteItems);
                StatisticsM.Steps[1].PropertyChanged += FinanceQuestionChanged;
            }
        }
        private void EmployeeSelectorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[StatisticsM.Steps.Count - 1].Item as EmployeeSelector).
                EnabledChange(StatisticsM.Steps[StatisticsM.Steps.Count - 1].Answer != null ? false : true);
            if (StatisticsM.Steps[StatisticsM.Steps.Count - 1].Answer != null)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = StatisticsM.Steps.Count });
                if (StatisticsM.Steps.Count == 3)
                {
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].Item = new EmployeeQuestion(StatisticsM.Steps[StatisticsM.Steps.Count - 1], DeleteItems, Type());
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].PropertyChanged += EmployeeQuestionChanged;
                }
                else
                {
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].Item = new Interval(StatisticsM.Steps[StatisticsM.Steps.Count - 1], DeleteItems);
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].PropertyChanged += IntervalChanged;
                }
            }
        }
        private void PatientSelectorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[1].Item as PatientSelector).EnabledChange(StatisticsM.Steps[1].Answer != null ? false : true);
            if (StatisticsM.Steps[1].Answer != null)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 2 });
                StatisticsM.Steps[2].Item = new PatientQuestion(StatisticsM.Steps[2], DeleteItems, Type());
                StatisticsM.Steps[2].PropertyChanged += PatientQuestionChanged;
            }
        }
        private void ServiceSelectorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[StatisticsM.Steps.Count - 1].Item as ServiceSelector).
                EnabledChange(StatisticsM.Steps[StatisticsM.Steps.Count - 1].Answer != null ? false : true);
            if (StatisticsM.Steps[StatisticsM.Steps.Count - 1].Answer != null)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = StatisticsM.Steps.Count });
                if (StatisticsM.Steps.Count == 3)
                {
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].Item = new ServiceQuestion(StatisticsM.Steps[StatisticsM.Steps.Count - 1], DeleteItems, Type());
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].PropertyChanged += ServiceQuestionChanged;
                }
                else
                {
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].Item = new Interval(StatisticsM.Steps[StatisticsM.Steps.Count - 1], DeleteItems);
                    StatisticsM.Steps[StatisticsM.Steps.Count - 1].PropertyChanged += IntervalChanged;
                }
            }
        }
        private void FinanceQuestionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[1].Item as FinanceQuestion).EnabledChange(StatisticsM.Steps[1].Answer != null ? false : true);
            if (StatisticsM.Steps[1].Answer != null)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 2 });
                if ((int)StatisticsM.Steps[1].Answer == 0)
                {
                    StatisticsM.Steps[2].Item = new ServiceSelector(StatisticsM.Steps[2], DeleteItems);
                    StatisticsM.Steps[2].PropertyChanged += ServiceSelectorChanged;
                }
                else if ((int)StatisticsM.Steps[1].Answer == 1)
                {
                    StatisticsM.Steps[2].Item = new Interval(StatisticsM.Steps[2], DeleteItems);
                    StatisticsM.Steps[2].PropertyChanged += IntervalChanged;
                }
                else if ((int)StatisticsM.Steps[1].Answer == 2)
                {
                    StatisticsM.Steps[2].Item = new EmployeeSelector(StatisticsM.Steps[2], DeleteItems);
                    StatisticsM.Steps[2].PropertyChanged += EmployeeSelectorChanged;
                }
            }
        }
        private void EmployeeQuestionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[2].Item as EmployeeQuestion).EnabledChange(StatisticsM.Steps[2].Answer != null ? false : true);
            if (StatisticsM.Steps[2].Answer != null)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 3 });
                StatisticsM.Steps[3].Item = new Interval(StatisticsM.Steps[3], DeleteItems);
                StatisticsM.Steps[3].PropertyChanged += IntervalChanged;
            }
        }
        private void PatientQuestionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[2].Item as PatientQuestion).EnabledChange(StatisticsM.Steps[2].Answer != null ? false : true);
            if (StatisticsM.Steps[2].Answer != null)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 3 });
                StatisticsM.Steps[3].Item = new Interval(StatisticsM.Steps[3], DeleteItems);
                StatisticsM.Steps[3].PropertyChanged += IntervalChanged;
            }
        }
        private void ServiceQuestionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (StatisticsM.Steps[2].Item as ServiceQuestion).EnabledChange(StatisticsM.Steps[2].Answer != null ? false : true);
            if (StatisticsM.Steps[2].Answer != null)
            {
                StatisticsM.Steps.Add(new StatisticsM.Step() { Id = 3 });
                StatisticsM.Steps[3].Item = new Interval(StatisticsM.Steps[3], DeleteItems);
                StatisticsM.Steps[3].PropertyChanged += IntervalChanged;
            }
        }
        private void IntervalChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            GlobalVM.StockLayout.actualContent.Content = new Chart(StatisticsM.Steps);
        }
        private bool? Type()
        {
            if ((StatisticsM.Steps[1].Answer as List<int>).Count == 0) return true;
            if ((StatisticsM.Steps[1].Answer as List<int>).Count > 1) return false;
            return null;
        }

        private void DeleteItems(int Id)
        {
            for (int i = 0; i < StatisticsM.Steps.Count; i++)
                if (StatisticsM.Steps[i].Id >= Id)
                {
                    StatisticsM.Steps.RemoveAt(i);
                    i--;
                }
            StatisticsM.Steps[StatisticsM.Steps.Count - 1].Answer = null;
        }
    }
}
