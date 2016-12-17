using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Examination;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class ExaminationVM : VMExtender
    {
        private StockVerticalMenuItem examinations { get; set; }
        private StockVerticalMenuItem newExamination { get; set; }
        private StockVerticalMenuItem importExamination { get; set; }
        private StockVerticalMenuItem view { get; set; }
        private StockVerticalMenuItem edit { get; set; }
        private StockVerticalMenuItem examinationPlan { get; set; }
        public ExaminationVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();

            examinations = new StockVerticalMenuItem();
            newExamination = new StockVerticalMenuItem();
            importExamination = new StockVerticalMenuItem();
            view = new StockVerticalMenuItem();
            edit = new StockVerticalMenuItem();
            examinationPlan = new StockVerticalMenuItem();

            examinations.button.Content = "Eddigi vizsgálati\neredmények";
            newExamination.button.Content = "Új vizsgálat";
            importExamination.button.Content = "Vizsgálati anyagok\nimportálása";
            view.button.Content = "Vizsgálat\nmegtekintése";
            edit.button.Content = "Vizsgálat\nszerkesztése";
            examinationPlan.button.Content = "Kezelési terv";

            GlobalVM.StockLayout.verticalMenu.Children.Add(examinations);
            GlobalVM.StockLayout.verticalMenu.Children.Add(newExamination);
            GlobalVM.StockLayout.verticalMenu.Children.Add(importExamination);
            GlobalVM.StockLayout.verticalMenu.Children.Add(view);
            GlobalVM.StockLayout.verticalMenu.Children.Add(edit);
            GlobalVM.StockLayout.verticalMenu.Children.Add(examinationPlan);

            examinations.button.Click += ExaminationsView;
            newExamination.button.Click += NewExaminaionView;
            importExamination.button.Click += ImportExaminationView;
            view.button.Click += ViewView;
            edit.button.Click += EditView;
            examinationPlan.button.Click += ExaminationPlanView;

            view.IsEnabledTrigger = false;
            edit.IsEnabledTrigger = false;

            newExamination.IsEnabledTrigger = !GlobalVM.GlobalM.JustImportDocuments;
        }
        protected internal void SetBack()
        {
            if (earlierItem == view || earlierItem == edit) earlierItem.IsEnabledTrigger = false;
        }
        private void ExaminationsView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ExaminationsLoad, Back);
        }
        private void NewExaminaionView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, NewExaminationLoad, Back);
        }
        private void ImportExaminationView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ImportExaminationLoad, Back);
        }
        private void ViewView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, () => ViewLoad(false, 0), Back);
        }
        private void EditView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, () => EditLoad(false, 0), Back);
        }
        private void ExaminationPlanView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ExaminationPlanLoad, Back);
        }
        protected internal async void ExaminationsLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new Examinations();
            }), examinations);
        }
        protected internal async void NewExaminationLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new NewExamination();
            }), newExamination);
        }
        protected internal async void ImportExaminationLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new ImportExamination();
            }), importExamination);
        }
        protected internal async void ViewLoad(bool imported, int ID)
        {
            await Utilities.Loading.Show();
            view.IsEnabledTrigger = true;
            await ViewLoad(new Func<UserControl>(() => new ExaminationView(imported, ID)), view);
        }
        protected internal async void EditLoad(bool imported, int ID)
        {
            await Utilities.Loading.Show();
            edit.IsEnabledTrigger = true;
            await ViewLoad(new Func<UserControl>(() => new ExaminationEdit(imported, ID)), edit);
        }
        protected internal async void ExaminationPlanLoad()
        {
            await Utilities.Loading.Show();
            await ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new ExaminationPlan();
            }), examinationPlan);
        }
    }
}
