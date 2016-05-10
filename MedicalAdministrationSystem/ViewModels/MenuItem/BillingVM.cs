using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Billing;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class BillingVM : VMExtender
    {
        private StockVerticalMenuItem bills { get; set; }
        private StockVerticalMenuItem companyData { get; set; }
        private StockVerticalMenuItem createBill { get; set; }
        private StockVerticalMenuItem viewBill { get; set; }
        public BillingVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();

            companyData = new StockVerticalMenuItem();
            bills = new StockVerticalMenuItem();
            createBill = new StockVerticalMenuItem();
            viewBill = new StockVerticalMenuItem();

            companyData.button.Content = "Cégadatok";
            bills.button.Content = "Számlák";
            createBill.button.Content = "Számla kiállítása";
            viewBill.button.Content = "Számla megtekintése";

            GlobalVM.StockLayout.verticalMenu.Children.Add(companyData);
            GlobalVM.StockLayout.verticalMenu.Children.Add(bills);
            GlobalVM.StockLayout.verticalMenu.Children.Add(createBill);
            GlobalVM.StockLayout.verticalMenu.Children.Add(viewBill);

            companyData.button.Click += CompanyDataView;
            bills.button.Click += BillsView;
            createBill.button.Click += CreateBillView;
            viewBill.button.Click += ViewBillView;

            viewBill.IsEnabledTrigger = false;

            createBill.IsEnabledTrigger = GlobalVM.StockLayout.headerContent.Visibility == System.Windows.Visibility.Collapsed ? false : true;
        }
        protected internal void SetBack()
        {
            if (earlierItem == viewBill) earlierItem.IsEnabledTrigger = false;
        }
        private void CompanyDataView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, CompanyDataLoad, Back);
        }
        private void BillsView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, BillsLoad, Back);
        }
        private void CreateBillView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, CreateBillLoad, Back);
        }
        private void ViewBillView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, () => ViewBillLoad(0), Back);
        }
        protected internal async void CompanyDataLoad()
        {
            await Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new CompanyData();
            }), companyData);
        }
        protected internal async void BillsLoad()
        {
            await Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new Bills();
            }), bills);
        }
        protected internal async void CreateBillLoad()
        {
            await Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(() =>
            {
                SetBack();
                return new CreateBill();
            }), createBill);
        }
        protected internal async void ViewBillLoad(int ID)
        {
            await Utilities.Loading.Show();
            viewBill.IsEnabledTrigger = true;
            ViewLoad(new Func<UserControl>(() => new ViewBill(ID)), viewBill);
        }
    }
}
