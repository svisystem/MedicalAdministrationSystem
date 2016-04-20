using DevExpress.Xpf.Bars;
using DevExpress.Xpf.RichEdit;
using DevExpress.Xpf.RichEdit.Menu;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Fragments;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.IO;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class WordEditor : UserControl
    {
        private DocumentControlM.ListElement element;
        protected internal WordEditorVM WordEditorVM { get; set; }
        private string name;
        private string Code;
        private Action close;
        private bool Type;
        private bool ReadOnly;
        public WordEditor(DocumentControlM.ListElement element, int PatientId, string name, string Code, Action close, bool Type, bool ReadOnly)
        {
            WordEditorVM = new WordEditorVM(PatientId);
            this.element = element;
            this.DataContext = this;
            this.name = name;
            this.Code = Code;
            this.close = close;
            this.Type = Type;
            this.ReadOnly = ReadOnly;
            InitializeComponent();
            wordEditor.Options.HorizontalRuler.Visibility = RichEditRulerVisibility.Hidden;
            wordEditor.Options.VerticalRuler.Visibility = RichEditRulerVisibility.Hidden;
            Start();
        }
        private void Start()
        {
            if (element.File != null)
            {
                wordEditor.LoadDocument(new MemoryStream((element.File as MemoryStream).ToArray()), WordEditorVM.DocFormat(element.FileType));
                wordEditor.Options.Authentication.UserName = "User1";
                if (ReadOnly) wordEditor.Document.Protect("admin1");
            }
            else wordEditor.Document.Protect("admin");
            wordEditor.Modified = false;
            wordEditor.ClearUndo();
        }
        private void Close(object sender, ItemClickEventArgs e)
        {
            WordEditorVM.CloseQuestion(close, wordEditor.Modified);
        }
        private void New(object sender, ItemClickEventArgs e)
        {
            WordEditorVM.NewDataQuestion(async delegate
            {
                await WordEditorVM.ExaminationPage(wordEditor, name, Code);
                wordEditor.Options.Authentication.UserName = "User1";
            }, element);
        }
        private void Load(object sender, ItemClickEventArgs e)
        {
            WordEditorVM.NewDataQuestion(delegate
            {
                wordEditor.LoadDocument();
                wordEditor.Document.Protect("admin");
                element.FileType = Path.GetExtension(wordEditor.Options.DocumentSaveOptions.CurrentFileName);
                SaveMethod();
            }, element);
        }

        private void Save(object sender, ItemClickEventArgs e)
        {
            WordEditorVM.NewDataQuestion(SaveMethod, element);
        }
        private void SaveMethod()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (string.IsNullOrEmpty(element.FileType)) element.FileType = ".docx";
                wordEditor.SaveDocument(ms, WordEditorVM.DocFormat(element.FileType));
                element.File = ms;
            }
            wordEditor.Modified = false;
        }

        private void biFilePrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            new DocumentGenerator().WordPrint(wordEditor.RtfText);
        }

        private void wordEditor_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            for (int i = e.Menu.ItemLinks.Count - 1; i >= 0; i--)
                if (e.Menu.ItemLinks[i] is BarItemLink)
                {
                    RichEditMenuItem item = ((BarItemLink)((e.Menu.ItemLinks[i]))).Item as RichEditMenuItem;
                    if (item != null)
                        if ((RichEditCommandId.NewComment == ((RichEditUICommand)item.Command).CommandId) ||
                           (RichEditCommandId.DeleteOneComment == ((RichEditUICommand)item.Command).CommandId))
                            e.Menu.ItemLinks.Remove(e.Menu.ItemLinks[i]);
                }
        }
    }
}
