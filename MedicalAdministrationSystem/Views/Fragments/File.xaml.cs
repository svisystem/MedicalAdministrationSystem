using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class File : UserControl
    {
        private string Type;
        private Action<ContentControl, string> Show;
        private Action<ContentControl> Erase;
        public File(bool ReadOnly, string Type, Action<ContentControl, string> Show, Action<ContentControl> Erase)
        {
            this.Type = Type;
            this.Show = Show;
            this.Erase = Erase;
            InitializeComponent();
            if (ReadOnly) erase.Visibility = Visibility.Collapsed;
            Icon();
        }
        private void ShowClick(object sender, RoutedEventArgs e)
        {
            Show(this, Type);
        }
        private void EraseClick(object sender, RoutedEventArgs e)
        {
            Erase(this);
        }
        private void Icon()
        {
            icon.Source = new BitmapImage(new Uri("pack://application:,,,/MedicalAdministrationSystem;component/Icons/" + Type + ".png"));
        }
    }
}
