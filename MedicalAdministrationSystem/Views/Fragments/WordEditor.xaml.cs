using DevExpress.Xpf.Bars;
using DevExpress.XtraRichEdit;
using MedicalAdministrationSystem.Models.Examination;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.IO;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class WordEditor : UserControl
    {
        private ContentControl content;
        private ImportExaminationM.ListElement element;
        private Func<string, DocumentFormat> DocFormat;
        public WordEditor(ContentControl content, ImportExaminationM.ListElement element, Func<string, DocumentFormat> DocFormat)
        {
            this.content = content;
            this.element = element;
            this.DocFormat = DocFormat;
            this.DataContext = this;
            InitializeComponent();
            wordEditor.Options.HorizontalRuler.Visibility = RichEditRulerVisibility.Hidden;
            wordEditor.Options.VerticalRuler.Visibility = RichEditRulerVisibility.Hidden;
        }

        private void Close(object sender, ItemClickEventArgs e)
        {
            CloseQuestion(delegate { content.Content = null; });
        }
        public void CloseQuestion(Action action)
        {
            if (wordEditor.Modified)
            {
                Dialog dialog = new Dialog(false, "El nem menetett változások lehetnek az adott oldalon", action, Dummy, true);
                dialog.content = new Dialogs.TextBlock("Amennyiben elnavigál erről az oldalról, az azon végrehajtott változások nem lesznek elmentve\n" +
                    "Biztosan elnavigál erről az oldalról?");
                dialog.Start();
            }
            else action();
        }
        private void Dummy() { }

        private void Load(object sender, ItemClickEventArgs e)
        {
            wordEditor.LoadDocument();
            SaveMethod();
        }

        private void Save(object sender, ItemClickEventArgs e)
        {
            SaveMethod();
        }
        private void SaveMethod()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (string.IsNullOrEmpty(element.FileType)) element.FileType = "docx";
                wordEditor.SaveDocument(ms, DocFormat(element.FileType));
                element.File = ms;
            }
            wordEditor.Modified = false;
        }
    }
}
