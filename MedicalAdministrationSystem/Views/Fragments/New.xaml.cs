using System;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class New : UserControl
    {
        private Action Add;
        public New(Action Add)
        {
            this.Add = Add;
            InitializeComponent();
        }
        private void NewAdd(object sender, RoutedEventArgs e)
        {
            Add();
        }
    }
}
