using MedicalAdministrationSystem.Views.Fragments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class HelpVM
    {
        protected internal async void Start()
        {
            await Task.Factory.StartNew(() =>
             {
                 if (GlobalVM.HelpWindow == null)
                 {
                     GlobalVM.HelpWindow = new Views.Global.StockWindow();
                     GlobalVM.HelpWindow.Title = "Felhasználói kézikönyv";
                     GlobalVM.HelpWindow.MinWidth = GlobalVM.HelpWindow.Width = 650;
                     GlobalVM.HelpWindow.MinHeight = GlobalVM.HelpWindow.Height = 750;
                     GlobalVM.HelpWindow.Content = new PdfViewer(new MemoryStream(UsersGuide.Resource.UsersGuide), SearchFor);
                     GlobalVM.HelpWindow.Closed += Close;
                     GlobalVM.HelpWindow.Show();
                 }
                 else
                 {
                     GlobalVM.HelpWindow.Focus();
                     SearchFor();
                 }
             }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void Close(object sender, EventArgs e)
        {
            (GlobalVM.HelpWindow.Content as PdfViewer).ClearStream();
            GlobalVM.HelpWindow = null;
        }
        private void SearchFor()
        {
            GlobalVM.HelpWindow.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
                (GlobalVM.HelpWindow.Content as PdfViewer).pdfViewer.Navigate(
                    (GlobalVM.HelpWindow.Content as PdfViewer).pdfViewer.Document.CreateOutlines().FirstOrDefault(x => x.Title.Contains(ActualView())))));
        }
        private string ActualView() => Views.FirstOrDefault(v => v.Key == GlobalVM.StockLayout.actualContent.Content.GetType()).Value;

        private Dictionary<Type, string> Views = new Dictionary<Type, string>()
        {
            { typeof(Views.Schedule.Schedule), "1. Előjegyzés" },
            { typeof(Views.Patients.PatientDetails), "2.1. Új Páciens Felvétele" },
            { typeof(Views.Patients.PatientList), "2.2. Páciensek Listája" },
            { typeof(Views.Examination.Examinations), "3.1. Eddigi Viszgálati Eredmények" },
            { typeof(Views.Examination.NewExamination), "3.2. Új Vizsgálat és Vizsgálati Anyagok Importálása" },
            { typeof(Views.Examination.ImportExamination), "3.2. Új Vizsgálat és Vizsgálati Anyagok Importálása" },
            { typeof(Views.Examination.ExaminationView), "3.3. Vizsgálatok Megtekintése és Szerkesztése" },
            { typeof(Views.Examination.ExaminationEdit), "3.3. Vizsgálatok Megtekintése és Szerkesztése" },
            { typeof(Views.Examination.ExaminationPlan), "3.4. Kezelései Terv" },
            { typeof(Views.Evidence.Evidences), "4.1. Eddigi Státuszok" },
            { typeof(Views.Evidence.NewEvidence), "4.2. új Státusz, Státusz Importálása" },
            { typeof(Views.Evidence.ImportEvidence), "4.2. új Státusz, Státusz Importálása" },
            { typeof(Views.Evidence.ViewEvidence), "4.3. Státusz Megtekintése, Szerkesztése" },
            { typeof(Views.Evidence.EditEvidence), "4.3. Státusz Megtekintése, Szerkesztése" },
            { typeof(Views.Billing.CompanyData), "5.1. Cégadatok" },
            { typeof(Views.Billing.Bills), "5.2. Számlák" },
            { typeof(Views.Billing.CreateBill), "5.3. Számla Kiállítása" },
            { typeof(Views.Billing.ViewBill), "5.4. Számla Megtekintése" },
            { typeof(Views.Statistics.Statistics), "6.1. Lekérdezés Paraméterezése" },
            { typeof(Views.Statistics.Chart), "6.2. Grafikon Elemzése" },
            { typeof(Views.Users.Registration), "7.1. Regisztráció" },
            { typeof(Views.Users.Login), "7.2. Bejelentkezés" },
            { typeof(Views.Users.PassChange), "7.3. Jelszó Módosítása" },
            { typeof(Views.Users.DetailsModify), "7.4. Adatok Módosítása" },
            { typeof(Views.Users.SurgeryTime), "7.5. Rendelési Idő" },
            { typeof(Views.Settings.Services), "8.1. Szolgáltatások Kezelése" },
            { typeof(Views.Settings.UserAdministrate), "8.2. Felhasználók" },
            { typeof(Views.Settings.Priviledges), "8.3. Jogosultságok" },
            { typeof(Views.Settings.FacilityData), "8.4. Intézmény Adatai" },
            { typeof(Views.Settings.Connection), "8.5. Adatbázis Kapcsolat Beállításai" },
            { typeof(Views.Settings.Security), "8.6. Biztonsági Profil" }
        };
    }
}
