using DevExpress.Xpf.Editors;
using MedicalAdministrationSystem.ViewModels.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.Views.Billing
{
    public partial class CreateBill : ViewExtender
    {
        protected internal CreateBillVM CreateBillVM { get; set; }
        public CreateBillValid createBillValid { get; set; }
        public CreateBill()
        {
            Start();
        }
        private async void Start()
        {
            await Loading.Show();
            createBillValid = new CreateBillValid();
            CreateBillVM = new CreateBillVM(ViewLoaded);
            this.DataContext = CreateBillVM;
            InitializeComponent();
            validatorClass = createBillValid;
            ConnectValidators();
        }
        private void ConnectValidators()
        {
            companyChooser.Validate += companyChooser_Validate;
        }
        private void companyChooser_Validate(object sender, ValidationEventArgs e)
        {
            createBillValid.CompanyValid = false;
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("Válassza ki a céget", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else if (!CreateBillVM.CompanyCheck(e.Value.ToString()))
                e.SetError("A mező tartalma nem megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                createBillValid.CompanyValid = true;
            }
            ForceBindingWithoutEnabledCheck(sender, e);
            AddEnabler();
        }
        private void Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((sender as Button).Name == "person")
            {
                if (!createBillValid.PersonEnable && createBillValid.CompanyEnable) createBillValid.CompanyEnable = false;
                createBillValid.PersonEnable = !createBillValid.PersonEnable;
            }
            else
            {
                if (!createBillValid.CompanyEnable && createBillValid.PersonEnable) createBillValid.PersonEnable = false;
                createBillValid.CompanyEnable = !createBillValid.CompanyEnable;
            }
            AddEnabler();
        }
        private void AddEnabler()
        {
            if (createBillValid.PersonEnable) createBillValid.AddValid = true;
            else if (createBillValid.CompanyEnable && createBillValid.CompanyValid) createBillValid.AddValid = true;
            else createBillValid.AddValid = false;
            CreateEnabler();
            CreateBillVM.From(createBillValid.PersonEnable);
        }

        private void Add(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateBillVM.AddBillingItem();
            CreateEnabler();
        }

        private void Increase(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateBillVM.ChangeValue(true);
        }

        private void Decrease(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateBillVM.ChangeValue(false);
        }

        private void Erase(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateBillVM.Erase();
            CreateEnabler();
        }

        private void Create(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateBillVM.ExecuteMethod();
        }
        private void CreateEnabler()
        {
            createBillValid.Create = createBillValid.AddValid && CreateBillVM.BillingCount();
        }
        protected internal bool Dirty()
        {
            return CreateBillVM.BillingCount();
        }
        private async void ViewLoaded()
        {
            await this.Dispatcher.BeginInvoke(new Action(() =>
            {
                billingView.BestFitColumns();
                servicesGrid.SortBy(servicesGrid.Columns[0], DevExpress.Data.ColumnSortOrder.Ascending);
            }), DispatcherPriority.Loaded);
            await Loading.Hide();
        }
        public class CreateBillValid : FormValidate
        {
            private bool _PersonEnable;
            private bool _CompanyEnable;
            private bool _CompanyValid;
            private bool _AddValid;
            private bool _Create;
            public bool PersonEnable
            {
                get
                {
                    return _PersonEnable;
                }
                set
                {
                    if (_PersonEnable == value) return;
                    _PersonEnable = value;
                    OnPropertyChanged("PersonEnable");
                }
            }
            public bool CompanyEnable
            {
                get
                {
                    return _CompanyEnable;
                }
                set
                {
                    if (_CompanyEnable == value) return;
                    _CompanyEnable = value;
                    OnPropertyChanged("CompanyEnable");
                }
            }
            public bool CompanyValid
            {
                get
                {
                    return _CompanyValid;
                }
                set
                {
                    if (_CompanyValid == value) return;
                    _CompanyValid = value;
                    OnPropertyChanged("CompanyValid");
                }
            }
            public bool AddValid
            {
                get
                {
                    return _AddValid;
                }
                set
                {
                    if (_AddValid == value) return;
                    _AddValid = value;
                    OnPropertyChanged("AddValid");
                }
            }
            public bool Create
            {
                get
                {
                    return _Create;
                }
                set
                {
                    if (_Create == value) return;
                    _Create = value;
                    OnPropertyChanged("Create");
                }
            }
        }
    }
}
