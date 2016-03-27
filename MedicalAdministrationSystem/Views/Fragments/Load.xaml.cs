using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.Views.Global;
using MedicalAdministrationSystem.ViewModels;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class Load : UserControl
    {
        private ObservableCollection<ImportExaminationM.ListElement> List { get; set; }
        private ContentControl content { get; set; }
        public Load(ContentControl content, ObservableCollection<ImportExaminationM.ListElement> List)
        {
            this.content = content;
            this.List = List;
            InitializeComponent();
        }

        private void DOCClick(object sender, RoutedEventArgs e)
        {
            List.Insert(List.Count - 1, new ImportExaminationM.ListElement()
            {
                Button = new File("doc", Show, Erase),
            });
        }

        private void PDFClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "PDF fájlok (*.pdf)|*.pdf",
                Title = "Fájl tallózása"
            };
            if (ofd.ShowDialog() == true)
            {
                content.Content = null;
                List.Insert(List.Count - 1, new ImportExaminationM.ListElement()
                {
                    Button = new File("pdf", Show, Erase),
                    File = ofd.OpenFile(),
                    Type = "pdf"
                });
            }
        }

        private void JPGClick(object sender, RoutedEventArgs e)
        {
            List.Insert(List.Count - 1, new ImportExaminationM.ListElement()
            {
                Button = new File("jpg", Show, Erase),
            });
        }
        private async void Show(ContentControl current, string from)
        {
            if (from == "pdf")
            {
                if (Check(current, from))
                {
                    await ViewModels.Utilities.Loading.Show();
                    await Task.Factory.StartNew(() =>
                    {
                        PdfViewer pdfViewer = new PdfViewer(content);
                        pdfViewer.pdfViewer.DocumentSource = List.Where(l => l.Button == current).Select(l => l.File).Single();
                        return pdfViewer;
                    }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(task =>
                    {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate
                        {
                            content.Content = task.Result as PdfViewer;
                        }));
                        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                    });
                }
            }
            else if (from == "doc")
            {
                if (Check(current, from))
                    await ViewModels.Utilities.Loading.Show();
                await Task.Factory.StartNew(() =>
                {
                    WordEditor wordEditor = new WordEditor();
                    //pdfViewer.pdfViewer.DocumentSource = List.Where(l => l.Button == current).Select(l => l.File).Single();
                    return wordEditor;
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(task =>
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate
                    {
                        content.Content = task.Result as WordEditor;
                    }));
                    SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                });
            }
            ViewModels.Utilities.Loading.Hide();
        }
        private void Erase(ContentControl current, string from)
        {
            if (Check(current, from)) List.RemoveAt(List.Where(l => l.Button == current).Select(l => l.Id).Single());
        }
        private bool Check(ContentControl current, string from)
        {
            if (content.Content != null && content.Content.GetType() == typeof(PdfViewer))
            {
                if ((content.Content as PdfViewer).pdfViewer.DocumentSource == List.Where(l => l.Button == current).Select(l => l.File).Single())
                    return false;
            }
            //else if (content.Content != null && content.Content.GetType() == typeof(WordEditor))
            //{
            //    if ((content.Content as WordEditor).pdfViewer.DocumentSource == List.Where(l => l.Button == current).Select(l => l.File).Single())
            //        return false;
            //}
            //    else if (from == "jpg")
            //{ }
            return true;
        }
    }
}
