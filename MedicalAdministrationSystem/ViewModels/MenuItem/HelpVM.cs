namespace MedicalAdministrationSystem.ViewModels.MenuItem
{
    class HelpVM
    {
        protected internal void Start()
        {
            if (GlobalVM.HelpWindow == null)
            {
                GlobalVM.HelpWindow = new Views.Global.StockWindow();
                GlobalVM.HelpWindow.Title = "Felhasználói kézikönyv";
                GlobalVM.HelpWindow.MinWidth = GlobalVM.HelpWindow.Width = 600;
                GlobalVM.HelpWindow.MinHeight = GlobalVM.HelpWindow.Height = 720;
                GlobalVM.HelpWindow.Show();
            }
            else GlobalVM.HelpWindow.Focus();
        }
    }
}
