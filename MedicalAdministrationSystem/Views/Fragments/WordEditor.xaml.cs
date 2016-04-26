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
        protected internal WordEditorVM WordEditorVM = new WordEditorVM();
        private string name;
        private string Code;
        private Action close;
        private bool Type;
        private bool ReadOnly;
        private Action Change;
        public WordEditor(DocumentControlM.ListElement element, int PatientId, string name, string Code, Action close, bool Type, Action Change)
        {
            ReadOnly = false;
            WordEditorVM.PatientId = PatientId;
            this.element = element;
            this.DataContext = this;
            this.name = name;
            this.Code = Code;
            this.close = close;
            this.Type = Type;
            this.Change = Change;
            Start();
        }
        public WordEditor(DocumentControlM.ListElement element, Action close, bool Type)
        {
            ReadOnly = true;
            this.element = element;
            this.DataContext = this;
            this.close = close;
            this.Type = Type;
            Start();
        }
        private void Start()
        {
            InitializeComponent();
            wordEditor.Options.HorizontalRuler.Visibility = RichEditRulerVisibility.Hidden;
            wordEditor.Options.VerticalRuler.Visibility = RichEditRulerVisibility.Hidden;
            if (ReadOnly)
            {
                biFileNew.IsEnabled = false;
                biFileOpen.IsEnabled = false;
                biFileSave.IsEnabled = false;
            }
            if (element.File != null)
            {
                wordEditor.LoadDocument(new MemoryStream((element.File as MemoryStream).ToArray()), WordEditorVM.DocFormat(element.FileType));
                if (!ReadOnly) wordEditor.Options.Authentication.UserName = "User1";
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
            WordEditorVM.NewDataQuestion(async () =>
            {
                await WordEditorVM.ExaminationPage(wordEditor, name, Code);
                wordEditor.Options.Authentication.UserName = "User1";
                SaveMethod();
            }, element);
        }
        private void Load(object sender, ItemClickEventArgs e)
        {
            WordEditorVM.NewDataQuestion(() =>
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
            Change();
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
