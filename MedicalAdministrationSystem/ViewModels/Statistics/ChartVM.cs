using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Statistics.Queries;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Statistics.Fragments;
using MedicalAdministrationSystem.Views.Statistics.Graphs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Statistics
{
    public class ChartVM : VMExtender
    {
        public ChartM ChartM { get; set; }
        private Func<ObservableCollection<ChartM.Record>, ObservableCollection<ChartM.Record>,
            DateTime, string, ObservableCollection<ChartM.Legend>, Task> SetLayout
        { get; set; }
        protected internal Action<bool?, bool?> SetVisualRange { get; set; }
        private ObservableCollection<StatisticsM.Step> GetData { get; set; }
        private DateTime SelectedDate
        {
            get
            {
                return _localSelectedDate;
            }
            set
            {
                ChartM.SelectedDate = value.ToString("yyyy. MMMM d. (dddd)", new CultureInfo("hu-HU"));
                _localSelectedDate = value;
            }
        }
        private DateTime _localSelectedDate;
        protected internal Type Main { get; set; }
        protected internal Type Secondary { get; set; }
        protected internal ChartVM(Func<ObservableCollection<ChartM.Record>, ObservableCollection<ChartM.Record>, DateTime,
            string, ObservableCollection<ChartM.Legend>, Task> SetLayout, ObservableCollection<StatisticsM.Step> GetData)
        {
            ChartM = new ChartM();
            this.SetLayout = SetLayout;
            this.GetData = GetData;
            Start();
        }
        private async void Start()
        {
            await SelectQuery();
            await Loading.Hide();
        }
        private async Task SelectQuery()
        {
            await Task.Run(async () =>
            {
                if ((int)GetData[0].Answer == 0)
                {
                    EmployeeQueries employeeQueries = null;
                    if ((int)GetData[2].Answer == 0)
                    {
                        Main = typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Adott alkalmazottakhoz mennyi páciens tartozik";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = employeeQueries.GetStepInString();
                        return await employeeQueries.PatientBelong();
                    }
                    else if ((int)GetData[2].Answer == 1)
                    {
                        Main = ((List<int>)GetData[1].Answer).Count == 0 ? typeof(StackedLine) : typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Adott alkalmazottak mennyi vizsgálatot végeztek";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = employeeQueries.GetStepInString();
                        return await employeeQueries.ExaminationCount();
                    }
                    else if ((int)GetData[2].Answer == 2)
                    {
                        Main = ((List<int>)GetData[1].Answer).Count == 0 ? typeof(StackedLine) : typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Adott alkalmazottak mennyi pácienst láttak el";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = employeeQueries.GetStepInString();
                        return await employeeQueries.EvidenceCount();
                    }
                    else if ((int)GetData[2].Answer == 3)
                    {
                        Main = typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Adott alkalmazottak mennyi munkaórát dolgoztak";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            employeeQueries = new EmployeeQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = employeeQueries.GetStepInString();
                        return await employeeQueries.WorkingHours();
                    }
                    else //if ((int)GetData[2].Answer == 4)
                    {
                        Main = typeof(MultiLine);
                        Secondary = typeof(StackedBar);
                        FinanceQueries financeQueries = null;
                        ChartM.MainText = "Adott alkalmazottak mennyit értékesítettek";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = financeQueries.GetStepInString();
                        return await financeQueries.IncomePerEmployee();
                    }
                }
                else if (GetData[0].Answer as int? == 1)
                {
                    PatientQueries patientQueries = null;
                    if ((int)GetData[2].Answer == 0)
                    {
                        Main = ((List<int>)GetData[1].Answer).Count == 0 ? typeof(StackedLine) : typeof(StackedBar);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "A kiválasztott páiensekhez milyen eloszlásban tartoznak a státuszbejegyzések";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = patientQueries.GetStepInString();
                        return await patientQueries.StatusDistibution();
                    }
                    else if ((int)GetData[2].Answer == 1)
                    {
                        Main = typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Adott páciensekhez mennyi státuszbejegyzés tartozik";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = patientQueries.GetStepInString();
                        return await patientQueries.StatusPerPatient();
                    }
                    else if ((int)GetData[2].Answer == 2)
                    {
                        Main = typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Adott páciensek mennyi időt töltöttek az intézményben";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = patientQueries.GetStepInString();
                        return await patientQueries.TimePerPatient();
                    }
                    else if ((int)GetData[2].Answer == 3)
                    {
                        Main = typeof(StackedLine);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Új és meglévő páciensek eloszlása";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            patientQueries = new PatientQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = patientQueries.GetStepInString();
                        return await patientQueries.NewPatientPercentage();
                    }
                }
                else if (GetData[0].Answer as int? == 2)
                {
                    ServiceQueries serviceQueries = null;
                    if ((int)GetData[2].Answer == 0)
                    {
                        Main = typeof(MultiLine);
                        Secondary = typeof(StackedBar);
                        ChartM.MainText = "Kezelések árainak módosulásai";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = serviceQueries.GetStepInString();
                        return await serviceQueries.ServicesPrices();
                    }
                    else if ((int)GetData[2].Answer == 1)
                    {
                        Main = typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Adott kezelésekből mennyit végeztek el";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = serviceQueries.GetStepInString();
                        return await serviceQueries.ExecutedService();
                    }
                    else //if ((int)GetData[2].Answer == 2)
                    {
                        Main = typeof(Line);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Kezelések számosságának alakulása";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            serviceQueries = new ServiceQueries((List<int>)GetData[1].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = serviceQueries.GetStepInString();
                        return await serviceQueries.ServicesCount();
                    }
                }
                else //if (GetData[0].Answer as int? == 3)
                {
                    FinanceQueries financeQueries = null;
                    if ((int)GetData[1].Answer == 0)
                    {
                        Main = typeof(MultiLine);
                        Secondary = typeof(StackedBar);
                        ChartM.MainText = "Adott kezelésekből származó bevételek eloszlása";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[2].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[2].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[2].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = financeQueries.GetStepInString();
                        return await financeQueries.IncomePerService();
                    }
                    else if ((int)GetData[1].Answer == 1)
                    {
                        Main = typeof(StackedBar);
                        Secondary = typeof(Bar);
                        ChartM.MainText = "Mennyi volt összesítve a bevétel";

                        if ((int)((List<object>)GetData[2].Answer)[0] == 0)
                        {
                            financeQueries = new FinanceQueries(new List<int>(),
                                0,
                                (DateTime)((List<object>)GetData[2].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[2].Answer)[0] == 1)
                        {
                            financeQueries = new FinanceQueries(new List<int>(),
                                (int)((List<object>)GetData[2].Answer)[1],
                                (DateTime)((List<object>)GetData[2].Answer)[2],
                                (DateTime)((List<object>)GetData[2].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[2].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            financeQueries = new FinanceQueries(new List<int>(),
                                (int)((List<object>)GetData[2].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[2].Answer)[1]);
                        }
                        ChartM.Step = financeQueries.GetStepInString();
                        return await financeQueries.TotalIncome();
                    }
                    else //if ((int)GetData[1].Answer == 2)
                    {
                        Main = typeof(MultiLine);
                        Secondary = typeof(StackedBar);
                        ChartM.MainText = "Adott alkalmazottak mennyit értékesítettek";

                        if ((int)((List<object>)GetData[3].Answer)[0] == 0)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[2].Answer,
                                0,
                                (DateTime)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Adott időpontban";
                        }

                        else if ((int)((List<object>)GetData[3].Answer)[0] == 1)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[2].Answer,
                                (int)((List<object>)GetData[3].Answer)[1],
                                (DateTime)((List<object>)GetData[3].Answer)[2],
                                (DateTime)((List<object>)GetData[3].Answer)[3]);
                            ChartM.LesserText = "Adott intervallumban, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }

                        else //if ((int)((List<object>)GetData[3].Answer)[0] == 2)
                        {
                            financeQueries = new FinanceQueries((List<int>)GetData[2].Answer,
                                (int)((List<object>)GetData[3].Answer)[1]);
                            ChartM.LesserText = "Folyamatában, " + StepMeaning((int)((List<object>)GetData[3].Answer)[1]);
                        }
                        ChartM.Step = financeQueries.GetStepInString();
                        return await financeQueries.IncomePerEmployee();
                    }
                }
                return null;
            }, CancellationToken.None).ContinueWith(task =>
            {
                ChartM.FullRecords = task.Result;
                Duplicate();
            });
        }
        protected internal async void Duplicate()
        {
            await Task.Run(() =>
            {
                ObservableCollection<ChartM.Record> collection = new ObservableCollection<ChartM.Record>();
                foreach (ChartM.Record item in ChartM.FullRecords) collection.Add(item);
                return collection;
            }, CancellationToken.None).ContinueWith(async task =>
            {
                ChartM.ViewRecords = task.Result;
                await SetLayout(ChartM.ViewRecords, ChartM.SingleRecord, ChartM.FullRecords.OrderByDescending(r => r.Date).FirstOrDefault().Date.Date, ChartM.Step, ChartM.Legends);
            });
        }
        protected internal bool Continual() => ChartM.FullRecords.GroupBy(r => r.Date.Date).Count() > 1;
        protected internal async Task SetLegends(DateTime Date)
        {
            await Task.Run(async () =>
            {
                foreach (int item in ChartM.FullRecords.GroupBy(r => r.Id).OrderBy(r => r.Key).Select(r => r.Key).ToList())
                {
                    ChartM.Legends.Add(new ChartM.Legend()
                    {
                        Id = item,
                        Name = ChartM.FullRecords.Where(f => f.Id == item).FirstOrDefault().Name,
                        Visible = true
                    });
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                        ChartM.LegendsContainer.Add(new ColorCheckButton(ChartM.Legends.Last(), SearchForItem, new DevExpress.Xpf.Charts.PastelKitPalette()[ChartM.LegendsContainer.Count]))));
                }
            }, CancellationToken.None).ContinueWith(async task => await SelectSingleData(Date));
        }
        protected internal async Task SelectSingleData(DateTime When)
        {
            SelectedDate = When;
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                ChartM.SingleRecord.Clear()));
            foreach (ChartM.Record item in ChartM.FullRecords.Where(r => r.Date.Date == When.Date).ToList())
                if (ChartM.Legends.Where(l => l.Id == item.Id).Single().Visible)
                    if (!TypeCheck(Secondary, typeof(StackedBar)))
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                        ChartM.SingleRecord.Add(item)));
                    else
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                        {
                            ChartM.SingleRecord.Add(
                                new ChartM.Record()
                                {
                                    Date = item.Date,
                                    Id = item.Id,
                                    Name = "Bruttó ár",
                                    Value1 = item.Value1
                                });
                            ChartM.SingleRecord.Add(
                                new ChartM.Record()
                                {
                                    Date = item.Date,
                                    Id = item.Id,
                                    Name = "Áfa",
                                    Value1 = (int)item.Value2,
                                    Value2 = item.Value1
                                });
                        }));
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                SetVisualRange(false, TypeCheck(Secondary, typeof(Bar)))));
        }
        private async void SearchForItem(int Id, bool Visible)
        {
            await Loading.Show();
            await Task.Run(async () =>
            {
                if (Visible)
                {
                    ChartM.Record selected = ChartM.FullRecords.Where(vr => vr.Id == Id && vr.Date == SelectedDate.Date).FirstOrDefault();
                    if (selected != null)
                        if (TypeCheck(Main, typeof(MultiLine)))
                            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                            {
                                ChartM.SingleRecord.Add(
                                    new ChartM.Record
                                    {
                                        Date = selected.Date,
                                        Id = selected.Id,
                                        Name = "Bruttó ár",
                                        Value1 = selected.Value1
                                    });
                                ChartM.SingleRecord.Add(
                                    new ChartM.Record()
                                    {
                                        Date = selected.Date,
                                        Id = selected.Id,
                                        Name = "Áfa",
                                        Value1 = (int)selected.Value2,
                                        Value2 = selected.Value1
                                    });
                            }));
                        else await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                            ChartM.SingleRecord.Add(ChartM.FullRecords.Where(vr => vr.Id == Id && vr.Date.Date == SelectedDate.Date).Single())));
                    foreach (ChartM.Record item in ChartM.FullRecords.Where(fr => fr.Id == Id).ToList())
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                            ChartM.ViewRecords.Add(item)));
                }
                else //if (!Visible)
                {
                    if (TypeCheck(Main, typeof(MultiLine)))
                        foreach (ChartM.Record item in ChartM.SingleRecord.Where(sr => sr.Id == Id).ToList())
                            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                                ChartM.SingleRecord.Remove(item)));
                    else
                    {
                        ChartM.Record selected = ChartM.SingleRecord.Where(vr => vr.Id == Id).FirstOrDefault();
                        if (selected != null)
                            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => ChartM.SingleRecord.Remove(selected)));
                    }
                    foreach (ChartM.Record item in ChartM.ViewRecords.Where(fr => fr.Id == Id).ToList())
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => ChartM.ViewRecords.Remove(item)));
                }

            }, CancellationToken.None).ContinueWith(async task =>
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                {
                    SetVisualRange(ChartM.Legends.Any(l => l.Visible), null);
                    await Loading.Hide();
                })));
        }
        protected internal bool TypeCheck(Type type1, Type type2) => type1 == type2;
        private string StepMeaning(int Step)
        {
            if (Step == 1) return "napi bontásban";
            if (Step == 2) return "heti bontásban";
            if (Step == 3) return "havi bontásban";
            if (Step == 4) return "éves bontásban";
            return null;
        }
    }
}
