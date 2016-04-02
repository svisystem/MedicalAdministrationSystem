using MedicalAdministrationSystem.Models.Examination;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class New : UserControl
    {
        private ContentControl content;
        private ObservableCollection<ImportExaminationM.ListElement> List { get; set; }
        public New(ref ContentControl content, ObservableCollection<ImportExaminationM.ListElement> List)
        {
            this.content = content;
            this.List = List;
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (content.Content != null && content.Content.GetType() == typeof(WordEditor))
                (content.Content as WordEditor).CloseQuestion(delegate { content.Content = new Load(ref content, List); });
            else content.Content = new Load(ref content, List);
        }

        private void AddWithEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Add(this, new RoutedEventArgs());
        }
    }
}
