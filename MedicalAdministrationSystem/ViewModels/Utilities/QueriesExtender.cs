using System;
using System.Collections.Generic;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public class QueriesExtender : VMExtender
    {
        protected internal List<int> Members { get; set; }
        protected internal int Step { get; set; }
        protected internal DateTime? StartTime { get; set; }
        protected internal DateTime? FinishTime { get; set; }
        protected internal int DayOfWeek(DateTime day) => day.DayOfWeek == 0 ? 7 : (int)day.DayOfWeek;
        protected internal DateTime Correction(bool reverse, DateTime date)
        {
            if (Step == 0 || Step == 1) return reverse == true ? date.Date : date.Date.AddDays(1);
            if (Step == 2) return reverse == true ? date.AddDays(-(DayOfWeek(date) - 1)) : 
                    date.AddDays(8 - DayOfWeek(date));
            if (Step == 3) return reverse == true ? date.AddDays(-(date.Day - 1)) : 
                    new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1);
            if (Step == 4) return reverse == true ? date.AddMonths(-(date.Month - 1)).AddDays(-(date.Day - 1)) :
                     new DateTime(date.AddYears(1).Year, 1, 1);
            return new DateTime();
        }
        protected internal DateTime NextStep(DateTime date) => date.AddDays(EndStep(date));
        protected internal int EndStep(DateTime date)
        {
            if (Step == 2) return 7;
            if (Step == 3) return DateTime.DaysInMonth(date.Year, date.Month);
            if (Step == 4) return new DateTime(date.Year, 12, 31).DayOfYear;
            return 1;
        }
        protected internal bool CycleEnd(DateTime date)
        {
            if (Step == 2) return DayOfWeek(date) == EndStep(date);
            if (Step == 3) return date.Day == EndStep(date);
            if (Step == 4) return date.DayOfYear == EndStep(date);
            return true;
        }
        protected internal bool Compare(DateTime date1, DateTime date2)
        {
            if (Step != 0 || Step != 1) return date1.Date <= date2.Date && date1.Date.AddDays(EndStep(date1.Date)) > date2.Date;
            return date1.Date == date2.Date;
        }
        protected internal string GetStepInString()
        {
            if (Step == 0 || Step == 1) return "Day";
            if (Step == 2) return "Week";
            if (Step == 3) return "Month";
            if (Step == 4) return "Year";
            return null;
        }
    }
}
