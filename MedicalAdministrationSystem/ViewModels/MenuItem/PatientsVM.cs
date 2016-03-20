﻿using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using MedicalAdministrationSystem.Views.Patients;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class PatientsVM : VMExtender
    {
        private StockVerticalMenuItem newPatient { get; set; }
        private StockVerticalMenuItem patientList { get; set; }
        private StockVerticalMenuItem patientDetails { get; set; }
        public PatientsVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();

            newPatient = new StockVerticalMenuItem();
            patientList = new StockVerticalMenuItem();
            patientDetails = new StockVerticalMenuItem();

            newPatient.button.Content = "Új páciens felvétele";
            patientList.button.Content = "Páciensek listája";
            patientDetails.button.Content = "Kiválasztott páciens\nadatai";

            GlobalVM.StockLayout.verticalMenu.Children.Add(newPatient);
            GlobalVM.StockLayout.verticalMenu.Children.Add(patientList);
            GlobalVM.StockLayout.verticalMenu.Children.Add(patientDetails);

            newPatient.button.Click += NewPatientView;
            patientList.button.Click += PatientListView;
            patientDetails.button.Click += PatientDetailsView;

            Selected();
            CheckUserData();
        }

        protected internal void Selected()
        {
            if (GlobalVM.StockLayout.headerContent.Content == null)
                patientDetails.button.IsEnabled = false;
            else patientDetails.button.IsEnabled = true;
        }
        protected internal void CheckUserData()
        {
            if (GlobalVM.GlobalM.UserID.Equals(0))
            {
                dialog = new Dialog(true, "Felhasználó adatai", OkMethod);
                dialog.content = new Views.Dialogs.TextBlock("Ön még nem töltötte ki a saját adatait\n" +
                    "A páciensek kezelése során szükségesek ezek az adatok\n" +
                    "Amíg ezeket nem tölti ki nincs lehetőség betegellátásra\n" +
                    "Most átirányítjuk az adatlapjára");
                dialog.Start();
            }
        }
        private void OkMethod()
        {
            MenuButtonsEnabled mbe = new MenuButtonsEnabled();
            mbe.modifier = true;
            mbe.LoadItem(GlobalVM.StockLayout.usersTBI);
        }
        private void CancelMethod()
        {
            newPatient.button.Foreground = new SolidColorBrush(Colors.Black);
            Back();
        }
        private void NewPatientView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, NewPatientLoad, Back);
        }
        private void PatientListView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, PatientListLoad, Back);
        }
        private void PatientDetailsView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, PatientDetailsLoad, Back);
        }
        private void Check(StockVerticalMenuItem select, Action OK, Action No)
        {
            earlierItem = currentItem;
            if (!currentItem.Equals(select))
            {
                Utilities.Loading.Show();
                currentItem = select;
                new FormChecking(OK, No, true);
            }
        }
        private void Back()
        {
            currentItem = earlierItem;
            currentItem.button_Click(currentItem.button, new RoutedEventArgs(Button.ClickEvent));
        }
        protected internal void NewPatientLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate () { return new PatientDetails(true); }), newPatient);
        }
        protected internal void PatientListLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate () { return new PatientList(Selected); }), patientList);
        }
        protected internal void PatientDetailsLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate () { return new PatientDetails(false); }), patientDetails);
        }
    }
}