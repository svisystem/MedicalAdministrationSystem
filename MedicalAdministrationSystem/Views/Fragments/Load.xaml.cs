using System;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class Load : UserControl
    {
        private Action DOC;
        private Action PDF;
        private Action JPG;
        public Load(Action DOCClick, Action PDFClick, Action JPGClick)
        {
            this.DOC = DOCClick;
            this.PDF = PDFClick;
            this.JPG = JPGClick;
            InitializeComponent();
        }
        private void DOCClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DOC();
        }

        private void PDFClick(object sender, System.Windows.RoutedEventArgs e)
        {
            PDF();
        }

        private void JPGClick(object sender, System.Windows.RoutedEventArgs e)
        {
            JPG();
        }
    }
}
