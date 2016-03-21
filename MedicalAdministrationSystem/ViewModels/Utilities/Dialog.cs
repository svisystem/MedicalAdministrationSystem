using MahApps.Metro.Controls.Dialogs;
using MedicalAdministrationSystem.Views.Global;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public class Dialog
    {
        private bool important { get; set; }
        private Action OK { get; set; }
        private Action Cancel { get; set; }
        private bool decision { get; set; }
        private DialogBase DialogBase = new DialogBase();

        public Dialog(bool _important, string _title, Action _OK)
        {
            this.important = _important;
            DialogBase.title.Content = _title;
            this.OK = _OK;
            DialogBase.no.Visibility = Visibility.Collapsed;
        }
        public Dialog(bool _important, string _title, Action _OK, Action _Cancel, bool _decision)
        {
            this.important = _important;
            DialogBase.title.Content = _title;
            this.OK = _OK;
            this.Cancel = _Cancel;
            this.decision = _decision;
        }
        protected internal UserControl content
        {
            set
            {
                DialogBase.content.Content = value;
            }
        }
        protected internal async void Start()
        {
            if (important) DialogBase.icon.Source = new BitmapImage(new Uri("pack://application:,,,/MedicalAdministrationSystem;component/Icons/Warning.png"));
            else DialogBase.icon.Source = new BitmapImage(new Uri("pack://application:,,,/MedicalAdministrationSystem;component/Icons/Logo3.png"));

            if (decision) DialogBase.yes.Content = "Igen";
            else DialogBase.yes.Content = "OK";
            DialogBase.yes.Click += Okmethod;

            if (Cancel != null)
            {
                if (decision) DialogBase.no.Content = "Nem";
                else DialogBase.no.Content = "Mégse";
                DialogBase.no.Click += Cancelmethod;
            }
            
            DialogBase.yes.Focus();
            GlobalVM.MainWindow.Focusable = false;
            await GlobalVM.MainWindow.ShowMetroDialogAsync(DialogBase);
        }
        private async void Okmethod(object sender, EventArgs e)
        {
            await GlobalVM.MainWindow.HideMetroDialogAsync(DialogBase);
            GlobalVM.MainWindow.Focusable = true;
            OK();
        }

        private async void Cancelmethod(object sender, EventArgs e)
        {
            await GlobalVM.MainWindow.HideMetroDialogAsync(DialogBase);
            GlobalVM.MainWindow.Focusable = true;
            Cancel();
        }

        protected internal Button YesButton()
        {
            return DialogBase.yes;
        }
    }
}
