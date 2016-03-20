using MedicalAdministrationSystem.Views.Dialogs;
using System;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    class FormChecking
    {
        private dynamic actualContent { get; set; }
        private Action OK { get; set; }
        private Action No { get; set; }
        protected internal FormChecking(Action _OK, Action _No, bool autoStart)
        {
            this.OK = _OK;
            this.No = _No;
            actualContent = GlobalVM.StockLayout.actualContent.Content;
            if (autoStart) Question();
        }
        protected internal void Question()
        {
            if (actualContent.Dirty())
            {
                Dialog dialog = new Dialog(false, "El nem menetett változások lehetnek az adott oldalon", OkMethod, CancelMethod, true);
                dialog.content = new TextBlock("Amennyiben elnavigál erről az oldalról, az azon végrehajtott változások nem lesznek elmentve\n" +
                    "Biztosan elnavigál erről az oldalról?");
                dialog.Start();
            }
            else OkMethod();
        }
        protected internal void SpecialQuestion()
        {
            if (actualContent.Dirty())
            {
                Dialog dialog = new Dialog(true, "El nem menetett változások lehetnek az adott oldalon", OkMethod, CancelMethod, true);
                dialog.content = new TextBlock("Amennyiben kitörli a páciens kijelölését, az adott oldalon elveszhetnek a módosított adatok\n" +
                    "Amennyiben nem szeretné hogy elveszítsen ezeket, kérjük mentse el azokat\n\n" +
                    "Biztosan eltávolítja a páciens kijelölését?");
                dialog.Start();
            }
            else OkMethod();
        }
        private void OkMethod()
        {
            OK();
            Loading.Hide();
        }
        private void CancelMethod()
        {
            No();
            Loading.Hide();
        }
    }
}
