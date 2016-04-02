using System;
using System.Reflection;
using System.Windows;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class LogoutVM
    {
        private Action cancel;
        protected internal async void Click(Action Cancel)
        {
            cancel = Cancel;
            await Loading.Show();
            Dialog dialog = new Dialog(false, "Kijelentkezés", OkMethod, CancelMethod, true);
            dialog.content = new TextBlock("Biztos benne hogy szeretne kijelentkezni az alkalmazásból?");
            dialog.Start();
        }
        protected internal async void OkMethod()
        {
            GlobalVM.GlobalM.AccountName = null;
            GlobalVM.GlobalM.AccountID = null;
            GlobalVM.GlobalM.UserID = 0;
            priviledges_fx pr = new priviledges_fx();
            foreach (PropertyInfo value in pr.GetType().GetProperties())
            {
                if (value.PropertyType == typeof(bool))
                    pr.GetType().GetProperty(value.Name, BindingFlags.Instance | BindingFlags.Public |
                        BindingFlags.NonPublic).SetValue(pr, false);
            }
            MenuButtonsEnabled mbe = new MenuButtonsEnabled(pr);
            mbe.SingleChange(GlobalVM.StockLayout.usersTBI, Visibility.Visible);
            mbe.LoadFirst();
            await Loading.Hide();
        }
        private async void CancelMethod()
        {
            cancel();
            await Loading.Hide();
        }
    }
}
