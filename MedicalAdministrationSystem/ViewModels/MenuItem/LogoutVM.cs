using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.Reflection;
using System.Windows;

namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class LogoutVM
    {
        private Action cancel;
        protected internal async void Click(Action Cancel)
        {
            cancel = Cancel;
            await Loading.Show();
            Dialog dialog = new Dialog(false, "Kijelentkezés", () => OkMethod(false), CancelMethod, true);
            dialog.content = new TextBlock("Biztos benne hogy szeretne kijelentkezni az alkalmazásból?");
            dialog.Start();
        }
        protected internal async void OkMethod(bool registrate)
        {
            GlobalVM.GlobalM.AccountID = null;
            GlobalVM.GlobalM.UserID = null;
            GlobalVM.GlobalM.CompanyId = null;
            GlobalVM.GlobalM.Secure = false;
            priviledges pr = new priviledges();

            foreach (PropertyInfo value in pr.GetType().GetProperties())
                if (value.PropertyType == typeof(bool))
                    pr.GetType().GetProperty(value.Name, BindingFlags.Instance | BindingFlags.Public |
                        BindingFlags.NonPublic).SetValue(pr, false);

            MenuButtonsEnabled mbe = new MenuButtonsEnabled(pr);
            mbe.SingleChange(GlobalVM.StockLayout.usersTBI, Visibility.Visible);
            mbe.SingleChange(GlobalVM.StockLayout.helpTBI, Visibility.Visible);
            if (registrate)
            {
                mbe.modifier = false;
                await mbe.LoadItem(GlobalVM.StockLayout.usersTBI);
            }
            else mbe.LoadFirst();
            await Loading.Hide();
        }
        private async void CancelMethod()
        {
            cancel();
            await Loading.Hide();
        }
    }
}
