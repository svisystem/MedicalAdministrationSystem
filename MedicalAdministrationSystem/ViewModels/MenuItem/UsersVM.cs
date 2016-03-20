using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Global;
using MedicalAdministrationSystem.Views.Users;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class UsersVM : VMExtender
    {
        private StockVerticalMenuItem registration { get; set; }
        private StockVerticalMenuItem login { get; set; }
        private StockVerticalMenuItem passChange { get; set; }
        private StockVerticalMenuItem detailsModify { get; set; }
        private StockVerticalMenuItem surgeryTime { get; set; }
        private StockVerticalMenuItem userDelete { get; set; }
        private bool workingConn { get; set; }
        protected internal bool fromPatient { get; set; } = false;
        public UsersVM()
        {
            GlobalVM.StockLayout.verticalMenu.Children.Clear();

            registration = new StockVerticalMenuItem();
            login = new StockVerticalMenuItem();
            passChange = new StockVerticalMenuItem();
            detailsModify = new StockVerticalMenuItem();
            surgeryTime = new StockVerticalMenuItem();
            userDelete = new StockVerticalMenuItem();

            registration.button.Content = "Regisztráció";
            login.button.Content = "Bejelentkezés";
            passChange.button.Content = "Jelszó módosítása";
            detailsModify.button.Content = "Adatok módosítása";
            surgeryTime.button.Content = "Rendelési idő";
            userDelete.button.Content = "Felhasználói fiók\ntörlése";

            GlobalVM.StockLayout.verticalMenu.Children.Add(registration);
            GlobalVM.StockLayout.verticalMenu.Children.Add(login);
            GlobalVM.StockLayout.verticalMenu.Children.Add(passChange);
            GlobalVM.StockLayout.verticalMenu.Children.Add(detailsModify);
            GlobalVM.StockLayout.verticalMenu.Children.Add(surgeryTime);
            GlobalVM.StockLayout.verticalMenu.Children.Add(userDelete);

            if (GlobalVM.GlobalM.AccountName == null)
            {
                registration.button.Click += RegistrationView;
                login.button.Click += LoginView;
                passChange.button.IsEnabled = false;
                detailsModify.button.IsEnabled = false;
                surgeryTime.button.IsEnabled = false;
                userDelete.button.IsEnabled = false;
            }
            else
            {
                registration.button.Click += RegistrateWhenLogin;
                login.button.IsEnabled = false;
                passChange.button.Click += PassChangeView;
                detailsModify.button.Click += DetailsModifyView;
                surgeryTime.button.Click += SurgeryTimeView;
                userDelete.button.Click += UserDeleteView;
            }
        }
        private void RegistrationView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, RegistrationLoad, Back);
        }
        private void LoginView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, LoginLoad, Back);
        }
        private void PassChangeView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, PassChangeLoad, Back);
        }
        private void DetailsModifyView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, DetailsModifyLoad, Back);
        }
        private void SurgeryTimeView(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, SurgeryTimeLoad, Back);
        }
        private void UserDeleteView(object sender, EventArgs e)
        {
            earlierItem = currentItem;
            dialog = new Dialog(true, "Felhasználói fiók törlése", DeleteMethod, UserDeleteCancel, true);
            dialog.content = new Views.Dialogs.TextBlock("Biztos benne hogy törölni szeretné felhasználói fiókját?\n" +
                "A módosítás végrehajtása után nem visszavonható!");
            dialog.Start();
        }
        private void DeleteMethod()
        {
            Utilities.Loading.Show();
            BackgroundWorker Execute = new BackgroundWorker();
            Execute.DoWork += new DoWorkEventHandler(ExecuteDoWork);
            Execute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExecuteCompleted);
            Execute.RunWorkerAsync();
        }
        private void UserDeleteCancel()
        {
            userDelete.button.Foreground = new SolidColorBrush(Colors.Black);
            Back();
            Utilities.Loading.Hide();
        }
        private void ExecuteDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (medicalEntities me = new medicalEntities())
                {
                    me.Database.Connection.Open();
                    me.accountdata.Where(a => a.IdAD == GlobalVM.GlobalM.AccountID).Single().DeletedAD = true;
                    me.SaveChanges();
                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
        }
        private void ExecuteCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Utilities.Loading.Hide();
            if (workingConn)
            {
                dialog = new Dialog(false, "Felhasználói fiók törlése", LogOut);
                dialog.content = new Views.Dialogs.TextBlock("Sikeresen törölte felhasználói fiókját");
                dialog.Start();
            }
            else ConnectionMessage();
        }
        private void LogOut()
        {
            LogoutVM logout = new LogoutVM();
            logout.OkMethod();
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
        private void RegistrateWhenLogin(object sender, EventArgs e)
        {
            Check((sender as Button).Parent as StockVerticalMenuItem, Question, Back);
        }
        private void Question()
        {
            dialog = new Dialog(false, "Bejelentkezett felhasználó nem regisztrálhat újra", OkMethod, CancelMethod, true);
            dialog.content = new Views.Dialogs.TextBlock("Amennyiben több felhasználó hasznája az alkalmazást ugyanazon a gépen," +
                " a regisztrációhoz előbb ki kell jelentkeznie az aktuális felhasználónak\n" +
                "Szeretne most kijelentkezni az alkalmazásból?");
            dialog.Start();
        }
        private void OkMethod()
        {
            LogoutVM logout = new LogoutVM();
            logout.OkMethod();
        }
        private void CancelMethod()
        {
            registration.button.Foreground = new SolidColorBrush(Colors.Black);
            Back();
        }
        private void Back()
        {
            currentItem = earlierItem;
            currentItem.button_Click(currentItem.button, new RoutedEventArgs(Button.ClickEvent));
        }
        protected internal void RegistrationLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate () { return new Registration(); }), registration);
        }
        protected internal void LoginLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate () { return new Login(); }), login);
        }
        protected internal void PassChangeLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate () { return new PassChange(); }), passChange);
        }
        protected internal void DetailsModifyLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate ()
            {
                if (!fromPatient) return new DetailsModify();
                else return new DetailsModify(fromPatient);
            }), detailsModify);
        }
        protected internal void SurgeryTimeLoad()
        {
            Utilities.Loading.Show();
            ViewLoad(new Func<UserControl>(delegate () { return new SurgeryTime(); }), surgeryTime);
        }
    }
}
