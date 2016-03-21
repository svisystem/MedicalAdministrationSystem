using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class ExaminationVM : VMExtender
    {
        private StockVerticalMenuItem examinations { get; set; }
        private StockVerticalMenuItem importExamination { get; set; }
        private StockVerticalMenuItem newExamination { get; set; }
        private StockVerticalMenuItem view { get; set; }
        private StockVerticalMenuItem examinationPlan { get; set; }
        public ExaminationVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();

            examinations = new StockVerticalMenuItem();
            importExamination = new StockVerticalMenuItem();
            newExamination = new StockVerticalMenuItem();
            view = new StockVerticalMenuItem();
            examinationPlan = new StockVerticalMenuItem();

            examinations.button.Content = "Eddigi vizsgálati\neredmények";
            importExamination.button.Content = "Vizsgálati anyagok\nimportálása";
            newExamination.button.Content = "Új vizsgálat";
            view.button.Content = "Megtekintés";
            examinationPlan.button.Content = "Kezelési terv";

            GlobalVM.StockLayout.verticalMenu.Children.Add(examinations);
            GlobalVM.StockLayout.verticalMenu.Children.Add(importExamination);
            GlobalVM.StockLayout.verticalMenu.Children.Add(newExamination);
            GlobalVM.StockLayout.verticalMenu.Children.Add(view);
            GlobalVM.StockLayout.verticalMenu.Children.Add(examinationPlan);

            examinations.button.Click += ExaminationsView;
            importExamination.button.Click += ImportExaminationView;
            newExamination.button.Click += NewExaminaionView;
            view.button.Click += ViewView;
            examinationPlan.button.Click += ExaminationPlanView;

            Selected();
        }
        protected internal void Selected()
        {
            if (GlobalVM.StockLayout.headerContent.Content == null)
                view.button.IsEnabled = false;
            else view.button.IsEnabled = true;
        }
        private void ExaminationsView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ExaminationsLoad, Back);
        }
        private void ImportExaminationView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ImportExamiationLoad, Back);
        }
        private void NewExaminaionView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, NewExaminationLoad, Back);
        }
        private void ViewView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ViewLoad, Back);
        }
        private void ExaminationPlanView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ExaminationPlanLoad, Back);
        }
        protected internal async void ExaminationsLoad()
        {
            await Utilities.Loading.Show();
            //ViewLoad(new Func<UserControl>(delegate () { return new Registration(); }), examinations);
        }
        protected internal async void ImportExamiationLoad()
        {
            await Utilities.Loading.Show();
            //ViewLoad(new Func<UserControl>(delegate () { return new Registration(); }), importExamination);
        }
        protected internal async void NewExaminationLoad()
        {
            await Utilities.Loading.Show();
            //ViewLoad(new Func<UserControl>(delegate () { return new Registration(); }), newExamination);
        }
        protected internal async void ViewLoad()
        {
            await Utilities.Loading.Show();
            //ViewLoad(new Func<UserControl>(delegate () { return new Registration(); }), view);
        }
        protected internal async void ExaminationPlanLoad()
        {
            await Utilities.Loading.Show();
            //ViewLoad(new Func<UserControl>(delegate () { return new Registration(); }), examinationPlan);
        }
    }
}
