using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Evidence;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels
{
    class EvidenceVM : VMExtender
    {
        private StockVerticalMenuItem evidences { get; set; }
        private StockVerticalMenuItem newEvidence { get; set; }
        private StockVerticalMenuItem importEvidence { get; set; }
        private StockVerticalMenuItem viewEvidence { get; set; }
        private StockVerticalMenuItem editEvidence { get; set; }
        public EvidenceVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();

            evidences = new StockVerticalMenuItem();
            newEvidence = new StockVerticalMenuItem();
            importEvidence = new StockVerticalMenuItem();
            viewEvidence = new StockVerticalMenuItem();
            editEvidence = new StockVerticalMenuItem();

            evidences.button.Content = "Eddigi státuszok";
            newEvidence.button.Content = "Új státusz";
            importEvidence.button.Content = "Státusz importálása";
            viewEvidence.button.Content = "Státusz megtekintése";
            editEvidence.button.Content = "Státusz szerkesztése";

            GlobalVM.StockLayout.verticalMenu.Children.Add(evidences);
            GlobalVM.StockLayout.verticalMenu.Children.Add(newEvidence);
            GlobalVM.StockLayout.verticalMenu.Children.Add(importEvidence);
            GlobalVM.StockLayout.verticalMenu.Children.Add(viewEvidence);
            GlobalVM.StockLayout.verticalMenu.Children.Add(editEvidence);

            evidences.button.Click += EvidencesView;
            newEvidence.button.Click += NewEvidenceView;
            importEvidence.button.Click += ImportEvidenceView;
            viewEvidence.button.Click += ViewEvidenceView;
            editEvidence.button.Click += EditEvidenceView;

            viewEvidence.IsEnabledTrigger = false;
            editEvidence.IsEnabledTrigger = false;
        }
        protected internal void SetBack()
        {
            if (earlierItem == viewEvidence || earlierItem == editEvidence) earlierItem.IsEnabledTrigger = false;
        }
        private void EvidencesView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, EvidencesLoad, Back);
        }
        private void NewEvidenceView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, NewEvidenceLoad, Back);
        }
        private void ImportEvidenceView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, ImportEvidenceLoad, Back);
        }
        private void ViewEvidenceView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, () => ViewEvidenceLoad(false, 0), Back);
        }
        private void EditEvidenceView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, () => EditEvidenceLoad(false, 0), Back);
        }
        protected internal async void EvidencesLoad()
        {
            await Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new Evidences();
            }), evidences);
        }
        protected internal async void NewEvidenceLoad()
        {
            await Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new NewEvidence();
            }), newEvidence);
        }
        protected internal async void ImportEvidenceLoad()
        {
            await Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new ImportEvidence();
            }), importEvidence);
        }
        protected internal async void ViewEvidenceLoad(bool imported, int ID)
        {
            await Utilities.Loading.Show();
            viewEvidence.IsEnabledTrigger = true;
            ViewLoad(new Func<UserControl>(() => new ViewEvidence(imported, ID)), viewEvidence);
        }
        protected internal async void EditEvidenceLoad(bool imported, int ID)
        {
            await Utilities.Loading.Show();
            editEvidence.IsEnabledTrigger = true;
            ViewLoad(new Func<UserControl>(() => new EditEvidence(imported, ID)), editEvidence);
        }
    }
}
