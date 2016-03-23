using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicalAdministrationSystem.Views.Examination
{
    public partial class Examinations : UserControl
    {
        public Examinations()
        {
            InitializeComponent();
        }
        protected internal bool Dirty()
        {
            //TODO
            return false;
        }
        private void modify(object sender, RoutedEventArgs e)
        {

        }

        private void erase(object sender, RoutedEventArgs e)
        {

        }

        private void Update(object sender, RoutedEventArgs e)
        {

        }
    }
}
