using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Settings;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Settings
{
    public partial class Services : ViewExtender
    {
        protected internal ServicesVM ServicesVM { get; set; }
        public Services()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            ServicesVM = new ServicesVM(view_Loaded);
            this.DataContext = ServicesVM;
            InitializeComponent();
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            name.Validate += NonMaskedNotNullValidateForString;
            vat.Validate += MaskedNotNullValidateForNumber;
            price.Validate += MaskedNotNullValidateForNumber;
        }
        protected internal new void NonMaskedNotNullValidateForString(object sender, ValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            ForceBindingWithoutEnabledCheck(sender, e);
            save.IsEnabled = ServicesVM.ButtonValidate();
        }
        protected internal void MaskedNotNullValidateForNumber(object sender, ValidationEventArgs e)
        {
            if (e.Value == null) e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            ForceBindingWithoutEnabledCheck(sender, e);
            save.IsEnabled = ServicesVM.ButtonValidate();
        }
        protected internal new void TextChanged(object sender, RoutedEventArgs e)
        {
            (sender as dynamic).DoValidate();
        }
        private void Erase(object sender, RoutedEventArgs e)
        {
            ServicesVM.ServiceEraseMethod();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            ServicesVM.ExecuteMethod();
        }
        private void newLine(object sender, RoutedEventArgs e)
        {
            ServicesVM.NewLine();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            ServicesVM.Refresh();
        }
        protected internal bool Dirty()
        {
            return ServicesVM.VMDirty();
        }
        private async void view_Loaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(() =>
                 view.BestFitColumns()), DispatcherPriority.Loaded);
            ServicesVM.SetStartValue();
            await Loading.Hide();
        }
    }
}
