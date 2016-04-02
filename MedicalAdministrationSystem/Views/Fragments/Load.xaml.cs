using DevExpress.XtraRichEdit;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class Load : UserControl
    {
        private ObservableCollection<ImportExaminationM.ListElement> List { get; set; }
        private ContentControl content { get; set; }
        public Load(ref ContentControl content, ObservableCollection<ImportExaminationM.ListElement> List)
        {
            this.content = content;
            this.List = List;
            InitializeComponent();
        }

        private void DOCClick(object sender, RoutedEventArgs e)
        {
            List.Insert(List.Count - 1, new ImportExaminationM.ListElement()
            {
                Button = new File("doc", BeforeShow, Erase),
                ButtonType = "doc"
            });
            BeforeShow(List[List.Count - 2].Button, "doc");
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
                    Button = new File("pdf", BeforeShow, Erase),
                    File = ofd.OpenFile(),
                    ButtonType = "pdf"
                });
            }
        }
        private void JPGClick(object sender, RoutedEventArgs e)
        {
            List.Insert(List.Count - 1, new ImportExaminationM.ListElement()
            {
                Button = new File("jpg", BeforeShow, Erase),
            });
        }
        int Id;
        private void BeforeShow(ContentControl current, string from)
        {
            if (content.Content != null && content.Content.GetType() == typeof(WordEditor))
                (content.Content as WordEditor).CloseQuestion(delegate { Show(current, from); });
            else Show(current, from);
        }
        private async void Show(ContentControl current, string from)
        {
            if (from == "pdf")
            {
                if (Check(current))
                {
                    await Loading.Show();
                    PdfViewer pdfViewer = new PdfViewer(content);
                    pdfViewer.pdfViewer.DocumentSource = List.Where(l => l.Button == current).Select(l => l.File).Single();
                    content.Content = pdfViewer;
                }
            }
            else if (from == "doc")
                if (Check(current))
                {
                    await Loading.Show();

                    WordEditor wordEditor = new WordEditor(content,
                        List.Where(l => l.Button == current).Single(), DocFormat);
                    if (List.Where(l => l.Button == current).Select(l => l.File).Single() != null)
                        wordEditor.wordEditor.LoadDocument(List.Where(l => l.Button == current).Select(l => l.File).Single(),
                            DocFormat(List.Where(l => l.Button == current).Select(l => l.FileType).Single()));
                    content.Content = wordEditor;
                }
            Id = List.Where(l => l.Button == current).Select(l => l.Id).Single();
            await Loading.Hide();
        }
        private DocumentFormat DocFormat(string FileType)
        {
            if (FileType == "rtf") return DocumentFormat.Rtf;
            else if (FileType == "txt") return DocumentFormat.PlainText;
            else if (FileType == "htm") return DocumentFormat.Html;
            else if (FileType == "html") return DocumentFormat.Html;
            else if (FileType == "mht") return DocumentFormat.Mht;
            else if (FileType == "docx") return DocumentFormat.OpenXml;
            else if (FileType == "odt") return DocumentFormat.OpenDocument;
            else if (FileType == "xml") return DocumentFormat.WordML;
            else if (FileType == "epub") return DocumentFormat.ePub;
            else if (FileType == "doc") return DocumentFormat.Doc;
            else return DocumentFormat.Undefined;
        }
        private void Erase(ContentControl current)
        {
            Action func = delegate { Ok(current); };
            Dialog dialog = new Dialog(true, "Dokumentum törlése", func, Dummy, true);
            dialog.content = new Dialogs.TextBlock("Biztosan eltávolítja a kiválasztott dokumentumot?\n" +
                "A dokumentum törlése csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private void Ok(ContentControl current)
        {
            if (!Check(current)) content.Content = null;
            List.RemoveAt(List.Where(l => l.Button == current).Select(l => l.Id).Single() - 1);
        }
        private void Dummy() { }
        private bool Check(ContentControl current)
        {
            if (Id == List.Where(l => l.Button == current).Select(l => l.Id).Single())
                return false;
            return true;
        }
    }
}
