using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System.ComponentModel;
using System.Windows;

namespace MedicalAdministrationSystem.ViewModels
{
    public class GlobalVM
    {
        public static StockLayout StockLayout = new StockLayout();
        public static StockWindow MainWindow = new StockWindow();
        public static GlobalM GlobalM = new GlobalM();
        public static void StartUp()
        {
            DevExpress.Xpf.Core.DXGridDataController.DisableThreadingProblemsDetection = true;
            MainWindow = new StockWindow();
            MainWindow.MinWidth = 1024;
            MainWindow.MinHeight = 720;
            MainWindow.Content = StockLayout;
            MainWindow.Title = "Egyészségügyi Betegnyilvántartó Rendszer";
            MainWindow.Closing += ShutDown;
            MainWindow.Show();
        }
        private static void ShutDown(object sender, CancelEventArgs e)
        {
            if (GlobalM.AccountID != null)
            {
                Dialog dialog = new Dialog(true, "Kilépés", delegate { });
                dialog.content = new TextBlock("Ameddig be van jelentkezve az alkalmazásba nem javasolt bezárni azt\n\n" +
                    "Az alkalmazásból való kilépéshez előbb jelentkezzen ki");
                dialog.Start();
                e.Cancel = true;
            }
            else Application.Current.Shutdown();
        }
    }
}
