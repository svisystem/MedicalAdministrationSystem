using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class NewButton : UserControl
    {
        public Enabler enabler { get; set; } = new Enabler();
        private Action NewItem { get; set; }
        public NewButton(Action NewItem)
        {
            this.NewItem = NewItem;
            InitializeComponent();
            enabler.Enabled = true;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            NewItem();
        }
        public class Enabler : NotifyPropertyChanged
        {
            private bool _Enabler;
            public bool Enabled
            {
                get
                {
                    return _Enabler;
                }
                set
                {
                    if (_Enabler == value) return;
                    _Enabler = value;
                    OnPropertyChanged("Enabled");
                }
            }
        }
    }
}
