using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Users;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Fragments;
using System;
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
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                me = new medicalEntities();
                me.Database.Connection.Open();

                foreach (usersschedule item in me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).ToList())
                {
                    temp[item.DayOfWeekUS - 1] = true;
                    SurgeryTimeM.GetType().GetProperty("Start" + DayOfWeek(item.DayOfWeekUS)).SetValue(SurgeryTimeM, item.StartTimeUS);
                    SurgeryTimeM.GetType().GetProperty("Finish" + DayOfWeek(item.DayOfWeekUS)).SetValue(SurgeryTimeM, item.FinishTimeUS);
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
                    SurgeryTimeM.ExceptionsButton.Insert(SurgeryTimeM.ExceptionsButton.Count - 1, new ExceptedTime(item, Valid, DeleteItem));

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

                if (SurgeryTimeM.Monday)
                {
                    if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 1))
                    {
                        usersschedule us = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 1).Single();
                        us.StartTimeUS = (DateTime)SurgeryTimeM.StartMonday;
                        us.FinishTimeUS = (DateTime)SurgeryTimeM.FinishMonday;
                    }
                    else me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 1,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartMonday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishMonday
                    });
                }
                else if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 1))
                    me.usersschedule.Remove(me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 1).FirstOrDefault());

                if (SurgeryTimeM.Tuesday)
                {
                    if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 2))
                    {
                        usersschedule us = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 2).Single();
                        us.StartTimeUS = (DateTime)SurgeryTimeM.StartTuesday;
                        us.FinishTimeUS = (DateTime)SurgeryTimeM.FinishTuesday;
                    }
                    else me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 2,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartTuesday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishTuesday
                    });
                }
                else if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 2))
                    me.usersschedule.Remove(me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 2).FirstOrDefault());

                if (SurgeryTimeM.Wednesday)
                {
                    if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 3))
                    {
                        usersschedule us = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 3).Single();
                        us.StartTimeUS = (DateTime)SurgeryTimeM.StartWednesday;
                        us.FinishTimeUS = (DateTime)SurgeryTimeM.FinishWednesday;
                    }
                    else me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 3,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartWednesday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishWednesday
                    });
                }
                else if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 3))
                    me.usersschedule.Remove(me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 3).FirstOrDefault());

                if (SurgeryTimeM.Thursday)
                {
                    if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 4))
                    {
                        usersschedule us = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 4).Single();
                        us.StartTimeUS = (DateTime)SurgeryTimeM.StartThursday;
                        us.FinishTimeUS = (DateTime)SurgeryTimeM.FinishThursday;
                    }
                    else me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 4,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartThursday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishThursday
                    });
                }
                else if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 4))
                    me.usersschedule.Remove(me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 4).FirstOrDefault());

                if (SurgeryTimeM.Friday)
                {
                    if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 5))
                    {
                        usersschedule us = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 5).Single();
                        us.StartTimeUS = (DateTime)SurgeryTimeM.StartFriday;
                        us.FinishTimeUS = (DateTime)SurgeryTimeM.FinishFriday;
                    }
                    else me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 5,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartFriday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishFriday
                    });
                }
                else if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 5))
                    me.usersschedule.Remove(me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 5).FirstOrDefault());

                if (SurgeryTimeM.Saturday)
                {
                    if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 6))
                    {
                        usersschedule us = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 6).Single();
                        us.StartTimeUS = (DateTime)SurgeryTimeM.StartSaturday;
                        us.FinishTimeUS = (DateTime)SurgeryTimeM.FinishSaturday;
                    }
                    else me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 6,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartSaturday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishSaturday
                    });
                }
                else if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 6))
                    me.usersschedule.Remove(me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 6).FirstOrDefault());

                if (SurgeryTimeM.Sunday)
                {
                    if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 7))
                    {
                        usersschedule us = me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 7).Single();
                        us.StartTimeUS = (DateTime)SurgeryTimeM.StartSunday;
                        us.FinishTimeUS = (DateTime)SurgeryTimeM.FinishSunday;
                    }
                    else me.usersschedule.Add(new usersschedule()
                    {
                        DayOfWeekUS = 7,
                        UserDataIdUS = (int)GlobalVM.GlobalM.UserID,
                        StartTimeUS = (DateTime)SurgeryTimeM.StartSunday,
                        FinishTimeUS = (DateTime)SurgeryTimeM.FinishSunday
                    });
                }
                else if (me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID).Any(u => u.DayOfWeekUS == 7))
                    me.usersschedule.Remove(me.usersschedule.Where(u => u.UserDataIdUS == GlobalVM.GlobalM.UserID && u.DayOfWeekUS == 7).FirstOrDefault());

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
            SurgeryTimeM.ExceptionsButton.Insert(SurgeryTimeM.ExceptionsButton.Count - 1, new ExceptedTime(SurgeryTimeM.Exceptions.Last(), Valid, DeleteItem));
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
    }
}
