using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Fragments;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MedicalAdministrationSystem.ViewModels.Users
{
    public class SurgeryTimeVM : VMExtender
    {
        public SurgeryTimeM SurgeryTimeM { get; set; }
        private BackgroundWorker Loading { get; set; }
        private NewButton NewButton { get; set; }
        private Action SaveButtonValid { get; set; }
        private int Count = 0;
        protected internal SurgeryTimeVM(Action SaveButtonValid)
        {
            this.SaveButtonValid = SaveButtonValid;
            SurgeryTimeM = new SurgeryTimeM();
            NewButton = new NewButton(NewItem);
            Loading = new BackgroundWorker();
            Loading.DoWork += LoadingModel;
            Loading.RunWorkerCompleted += LoadingModelComplete;
            Loading.RunWorkerAsync();
        }
        bool[] temp = new bool[7];
        protected internal void AfterLoaded()
        {
            for (int i = 0; i < temp.Length; i++)
                if (temp[i]) SurgeryTimeM.GetType().GetProperty(DayOfWeek(i + 1)).SetValue(SurgeryTimeM, true);
            SurgeryTimeM.AcceptChanges();
        }
        List<SurgeryTimeM.Day> Actual = new List<SurgeryTimeM.Day>();
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                SurgeryTimeM.UserRegistrateDate = me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Single().RegistrateTimeAD;

                for (int i = 1; i <= 7; i++)
                {
                    usersschedule item = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == i).OrderByDescending(u => u.WhenCreateUS).FirstOrDefault();
                    if (item != null)
                    {
                        temp[item.DayOfWeekUS - 1] = true;
                        SurgeryTimeM.GetType().GetProperty("Start" + DayOfWeek(item.DayOfWeekUS)).SetValue(SurgeryTimeM, item.StartTimeUS);
                        SurgeryTimeM.GetType().GetProperty("Finish" + DayOfWeek(item.DayOfWeekUS)).SetValue(SurgeryTimeM, item.FinishTimeUS);
                        SetActual(i, item.StartTimeUS, item.FinishTimeUS);
                    }
                }

                foreach (SurgeryTimeM.Exception item in me.exceptedschedule.Where(ex => ex.UserDataIdES == GlobalVM.GlobalM.UserID).ToList().
                    Select(ex => new SurgeryTimeM.Exception()
                    {
                        DBId = ex.IdES,
                        Included = ex.IncludedES,
                        StartDateTime = ex.StartDateES,
                        FinishDateTime = ex.FinishDateED,
                        Valid = true
                    }))
                    SurgeryTimeM.Exceptions.Add(item);

                CollectionChange();

                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                SurgeryTimeM.ExceptionsButton.Add(NewButton);
                Count = SurgeryTimeM.Exceptions.Count;
                foreach (SurgeryTimeM.Exception item in SurgeryTimeM.Exceptions)
                    SurgeryTimeM.ExceptionsButton.Insert(SurgeryTimeM.ExceptionsButton.Count - 1,
                        new ExceptedTime(item, Valid, DeleteItem, Between, (DateTime)SurgeryTimeM.UserRegistrateDate, true));

                foreach (SurgeryTimeM.Exception row in SurgeryTimeM.Exceptions)
                    row.AcceptChanges();

                await Utilities.Loading.Hide();
            }
            else ConnectionMessage();
        }
        protected internal async void ExecuteMethod()
        {
            await Utilities.Loading.Show();
            dialog = new Dialog(false, "Változtatások mentése", () =>
            {
                BackgroundWorker Execute = new BackgroundWorker();
                Execute.DoWork += ExecuteDoWork;
                Execute.RunWorkerCompleted += ExecuteComplete;
                Execute.RunWorkerAsync();
            }, async () => await Utilities.Loading.Hide(), true);
            dialog.content = new TextBlock("Valóban menteni szeretné a Rendelési időben végrehajtott változtatásait?");
            dialog.Start();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                foreach (int id in SurgeryTimeM.Erased)
                    me.exceptedschedule.Remove(me.exceptedschedule.Where(ex => ex.IdES == id).FirstOrDefault());

                foreach (SurgeryTimeM.Exception item in SurgeryTimeM.Exceptions)
                {
                    if (item.DBId == null) me.exceptedschedule.Add(new exceptedschedule()
                    {
                        IncludedES = item.Included,
                        UserDataIdES = (int)GlobalVM.GlobalM.UserID,
                        StartDateES = (DateTime)item.StartDateTime,
                        FinishDateED = (DateTime)item.FinishDateTime
                    });
                    else
                    {
                        exceptedschedule es = me.exceptedschedule.Where(ex => ex.IdES == item.DBId).Single();
                        es.IncludedES = item.Included;
                        es.StartDateES = (DateTime)item.StartDateTime;
                        es.FinishDateED = (DateTime)item.FinishDateTime;
                    }
                }

                if (SurgeryTimeM.Monday && Changed(1))
                {
                    me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 1,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartMonday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishMonday,
                        WhenCreateUS = DateTime.Now
                    });
                    SetActual(1, (DateTime)SurgeryTimeM.StartMonday, (DateTime)SurgeryTimeM.FinishMonday);
                }
                if (SurgeryTimeM.Tuesday && Changed(2))
                {
                    me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 2,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartTuesday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishTuesday,
                        WhenCreateUS = DateTime.Now
                    });
                    SetActual(2, (DateTime)SurgeryTimeM.StartTuesday, (DateTime)SurgeryTimeM.FinishTuesday);
                }
                if (SurgeryTimeM.Wednesday && Changed(3))
                {
                    me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 3,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartWednesday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishWednesday,
                        WhenCreateUS = DateTime.Now
                    });
                    SetActual(3, (DateTime)SurgeryTimeM.StartWednesday, (DateTime)SurgeryTimeM.FinishWednesday);
                }
                if (SurgeryTimeM.Thursday && Changed(4))
                {
                    me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 4,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartThursday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishThursday,
                        WhenCreateUS = DateTime.Now
                    });
                    SetActual(4, (DateTime)SurgeryTimeM.StartThursday, (DateTime)SurgeryTimeM.FinishThursday);
                }
                if (SurgeryTimeM.Friday && Changed(5))
                {
                    me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 5,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartFriday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishFriday,
                        WhenCreateUS = DateTime.Now
                    });
                    SetActual(5, (DateTime)SurgeryTimeM.StartFriday, (DateTime)SurgeryTimeM.FinishFriday);
                }
                if (SurgeryTimeM.Saturday && Changed(6))
                {
                    me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 6,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartSaturday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishSaturday,
                        WhenCreateUS = DateTime.Now
                    });
                    SetActual(6, (DateTime)SurgeryTimeM.StartSaturday, (DateTime)SurgeryTimeM.FinishSaturday);
                }
                if (SurgeryTimeM.Sunday && Changed(7))
                {
                    me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 7,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartSunday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishSunday,
                        WhenCreateUS = DateTime.Now
                    });
                    SetActual(7, (DateTime)SurgeryTimeM.StartSunday, (DateTime)SurgeryTimeM.FinishSunday);
                }

                me.SaveChanges();
                me.Database.Connection.Close();
                workingConn = true;
            }
            catch
            {
                workingConn = false;
            }
        }
        private void ExecuteComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn)
            {
                SurgeryTimeM.Erased.Clear();
                foreach (SurgeryTimeM.Exception row in SurgeryTimeM.Exceptions)
                    row.AcceptChanges();
                SurgeryTimeM.AcceptChanges();
                Count = SurgeryTimeM.Exceptions.Count;

                dialog = new Dialog(false, "Módosítások mentése", async () => await Utilities.Loading.Hide());
                dialog.content = new TextBlock("A módosítások mentése sikeresen megtörtént");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        private void Valid()
        {
            NewButton.enabler.Enabled = Validate();
            SaveButtonValid();
        }
        protected internal bool Validate()
        {
            return !SurgeryTimeM.Exceptions.Any(ex => !ex.Valid);
        }
        private void NewItem()
        {
            SurgeryTimeM.Exceptions.Add(new SurgeryTimeM.Exception());
            CollectionChange();
            SurgeryTimeM.ExceptionsButton.Insert(SurgeryTimeM.ExceptionsButton.Count - 1,
                new ExceptedTime(SurgeryTimeM.Exceptions.Last(), Valid, DeleteItem, Between, (DateTime)SurgeryTimeM.UserRegistrateDate));
            Valid();
        }
        private void DeleteItem(int Id)
        {
            if (SurgeryTimeM.Exceptions[Id - 1].DBId != null)
                SurgeryTimeM.Erased.Add((int)SurgeryTimeM.Exceptions[Id - 1].DBId);
            SurgeryTimeM.ExceptionsButton.RemoveAt(Id - 1);
            SurgeryTimeM.Exceptions.RemoveAt(Id - 1);
            Valid();
        }
        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChange();
        }
        private void CollectionChange()
        {
            SurgeryTimeM.Exceptions.CollectionChanged -= CollectionChangedMethod;
            for (int i = 0; i < SurgeryTimeM.Exceptions.Count; i++)
                SurgeryTimeM.Exceptions[i].Id = i + 1;
            SurgeryTimeM.Exceptions.CollectionChanged += CollectionChangedMethod;
        }
        private string DayOfWeek(int number)
        {
            switch (number)
            {
                case 1: return "Monday";
                case 2: return "Tuesday";
                case 3: return "Wednesday";
                case 4: return "Thursday";
                case 5: return "Friday";
                case 6: return "Saturday";
                case 7: return "Sunday";
                default: return null;
            }
        }
        protected internal bool VMDirty()
        {
            if (Count != SurgeryTimeM.Exceptions.Count || SurgeryTimeM.Erased.Count != 0) return true;
            else return SurgeryTimeM.IsChanged;
        }
        private void SetActual(int DaysOfWeek, DateTime Start, DateTime End)
        {
            if (Actual.Any(a => a.DaysOfWeek == DaysOfWeek))
            {
                Actual.Where(a => a.DaysOfWeek == DaysOfWeek).Single().Start = Start;
                Actual.Where(a => a.DaysOfWeek == DaysOfWeek).Single().End = End;
            }
            else Actual.Add(new SurgeryTimeM.Day()
            {
                DaysOfWeek = DaysOfWeek,
                Start = Start,
                End = End
            });
        }
        private bool Changed(int DaysOfWeek)
        {
            if (Actual.Any(a => a.DaysOfWeek == DaysOfWeek))
                return Actual.Where(a => a.DaysOfWeek == DaysOfWeek).Single().Start != (DateTime)SurgeryTimeM.GetType().GetProperty("Start" + DayOfWeek(DaysOfWeek)).GetValue(SurgeryTimeM) ||
                Actual.Where(a => a.DaysOfWeek == DaysOfWeek).Single().End != (DateTime)SurgeryTimeM.GetType().GetProperty("Finish" + DayOfWeek(DaysOfWeek)).GetValue(SurgeryTimeM);
            else return true;
        }
        protected internal bool Between(bool Included, int Id, DateTime? Start, DateTime? End)
        {
            if (End == null)
                return SurgeryTimeM.Exceptions.Any(ex => ex.StartDateTime <= Start && ex.FinishDateTime >= Start && ex.Id != Id && ex.Included == Included);
            if (Start == null)
                return SurgeryTimeM.Exceptions.Any(ex => ex.StartDateTime <= End && ex.FinishDateTime >= End && ex.Id != Id && ex.Included == Included);
            return SurgeryTimeM.Exceptions.Any(ex => ex.StartDateTime <= Start && ex.FinishDateTime >= Start && ex.Id != Id && ex.Included == Included) ||
                SurgeryTimeM.Exceptions.Any(ex => ex.StartDateTime <= End && ex.FinishDateTime >= End && ex.Id != Id && ex.Included == Included) ||
                SurgeryTimeM.Exceptions.Any(ex => ex.StartDateTime >= Start && ex.FinishDateTime <= End && ex.Id != Id && ex.Included == Included);

        }
    }
}
