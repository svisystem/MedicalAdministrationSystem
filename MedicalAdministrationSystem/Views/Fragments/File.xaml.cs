using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class File : UserControl
    {
        private string Type { get; set; }
        private Action<ContentControl, string> show { get; set; }
        private Action<ContentControl, string> erase { get; set; }
        public File(string Type, Action<ContentControl, string> Show, Action<ContentControl, string> Erase)
        {
            this.Type = Type;
            this.show = Show;
            this.erase = Erase;
            InitializeComponent();
            Icon();
        }

        private void Show(object sender, RoutedEventArgs e)
        {
            show(this, Type);
        }

        private void Erase(object sender, RoutedEventArgs e)
        {
            erase(this, Type);
        }
        private void Dispose()
        {
            //TODO
        }
        private void Icon()
        {
            icon.Source = new BitmapImage(new Uri("pack://application:,,,/MedicalAdministrationSystem;component/Icons/" + Type + ".png"));
        }
    }
}
