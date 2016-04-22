using DevExpress.XtraRichEdit;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Fragments;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels
{
    public class DocumentControlVM : VMExtender
    {
        private OpenFileDialog ofd { get; set; }
        private ContentControl local;
        private DocumentControlM DocumentControlM = new DocumentControlM();
        protected internal Func<bool> Validate;
        protected internal Action<bool> SetEnabledSave;
        protected internal Action<bool> SetReadOnlyFields;
        protected internal Func<string> GetName;
        protected internal Func<string> GetCode;
        protected internal bool ReadOnly;
        protected internal bool Type; //true = examination, false = evidence
        protected internal DocumentControlVM(ref ContentControl content, ObservableCollection<DocumentControlM.ListElement> List)
        {
            local = content;
            DocumentControlM.List = List;
        }
        protected internal void Start(int PatientId)
        {
            DocumentControlM.PatientId = PatientId;

            if (!ReadOnly)
                DocumentControlM.List.Add(new DocumentControlM.ListElement()
                {
                    Button = new New(NewAdd)
                });
            else
            {
                foreach (DocumentControlM.ListElement item in DocumentControlM.List)
                {
                    item.Button = new Views.Fragments.File(item.ButtonType, BeforeShow, Erase);
                }
            }
            foreach (DocumentControlM.ListElement row in DocumentControlM.List)
                row.AcceptChanges();
            DocumentControlM.AcceptChanges();
            DocumentControlM.List.CollectionChanged += CollectionChangedMethod;
        }
        private void NewAdd()
        {
            if (Validate())
            {
                if (local.Content != null)
                {
                    if (local.Content.GetType() == typeof(WordEditor))
                        (local.Content as WordEditor).WordEditorVM.CloseQuestion(delegate
                        {
                            local.Content = new Load(DOCClick, PDFClick, JPGClick);
                            DocumentControlM.Selected = null;
                        }, (local.Content as WordEditor).wordEditor.Modified);
                    else if (local.Content.GetType() != typeof(Load))
                    {
                        if (local.Content.GetType() == typeof(PictureViewer)) (local.Content as PictureViewer).close();
                        else if (local.Content.GetType() == typeof(PdfViewer)) (local.Content as PdfViewer).close();
                        local.Content = new Load(DOCClick, PDFClick, JPGClick);
                        DocumentControlM.Selected = null;
                    }
                }
                else
                {
                    local.Content = new Load(DOCClick, PDFClick, JPGClick);
                    DocumentControlM.Selected = null;
                }
            }
            else
            {
                Dialog dialog = new Dialog(false, "Hiányzó adatok", delegate { });
                dialog.content = new Views.Dialogs.TextBlock("Először töltse ki a vizsgálathoz szükséges adatokat");
                dialog.Start();
            }
        }
        private void DOCClick()
        {
            DocumentControlM.List.Insert(DocumentControlM.List.Count - 1, new DocumentControlM.ListElement()
            {
                Button = new Views.Fragments.File("doc", BeforeShow, Erase),
                ButtonType = "doc"
            });
            BeforeShow(DocumentControlM.List[DocumentControlM.List.Count - 2].Button, "doc");
        }

        private void PDFClick()
        {
            ofd = new OpenFileDialog()
            {
                Filter = "PDF fájlok (*.pdf)|*.pdf",
                Title = "Fájl tallózása"
            };
            if (ofd.ShowDialog() == true)
            {
                local.Content = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    ofd.OpenFile().CopyTo(ms);
                    DocumentControlM.List.Insert(DocumentControlM.List.Count - 1, new DocumentControlM.ListElement()
                    {
                        Button = new Views.Fragments.File("pdf", BeforeShow, Erase),
                        File = ms,
                        ButtonType = "pdf"
                    });
                }
                BeforeShow(DocumentControlM.List[DocumentControlM.List.Count - 2].Button, "pdf");
            }
        }
        private void JPGClick()
        {
            ofd = new OpenFileDialog()
            {
                Filter = "Minden támogatott formátum|*.jpg;*.jpeg;*.bmp;*.png;*.gif" +
                "|JPEG fájlok (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                "|Bitmap képek (*.bmp)|*.bmp" +
                "|PNG fájlok (*.png)|*.png" +
                "|GIF fájlok (*.gif)|*.gif",
                Title = "Fájl tallózása",
            };
            if (ofd.ShowDialog() == true)
            {
                local.Content = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    ofd.OpenFile().CopyTo(ms);
                    DocumentControlM.List.Insert(DocumentControlM.List.Count - 1, new DocumentControlM.ListElement()
                    {
                        Button = new Views.Fragments.File("jpg", BeforeShow, Erase),
                        File = ms,
                        ButtonType = "jpg"
                    });
                }
                BeforeShow(DocumentControlM.List[DocumentControlM.List.Count - 2].Button, "jpg");
            }
        }
        protected internal void BeforeShow(ContentControl current, string from)
        {
            if (local.Content != null && local.Content.GetType() == typeof(WordEditor))
                (local.Content as WordEditor).WordEditorVM.CloseQuestion(delegate { Show(current, from); }, (local.Content as WordEditor).wordEditor.Modified);
            else Show(current, from);
        }
        private async void Show(ContentControl current, string from)
        {
            if (from == "pdf")
            {
                if (Check(current))
                {
                    await Loading.Show();
                    local.Content = null;
                    local.Content = new PdfViewer(Close, DocumentControlM.List.Where(l => l.Button == current).Single().File);
                    DocumentControlM.Selected = DocumentControlM.List.Where(l => l.Button == current).Single();
                }
            }
            else if (from == "doc")
            {
                if (Check(current))
                {
                    await Loading.Show();
                    local.Content = null;
                    local.Content = new WordEditor(
                        DocumentControlM.List.Where(l => l.Button == current).Single(),
                        DocumentControlM.PatientId,
                        GetName(),
                        GetCode(),
                        Close,
                        Type,
                        ReadOnly);
                    DocumentControlM.Selected = DocumentControlM.List.Where(l => l.Button == current).Single();
                }
            }
            else if (from == "jpg")
                if (Check(current))
                {
                    await Loading.Show();
                    local.Content = null;
                    local.Content = new PictureViewer(
                        DocumentControlM.List.Where(l => l.Button == current).Single().File as MemoryStream, Close);
                    DocumentControlM.Selected = DocumentControlM.List.Where(l => l.Button == current).Single();
                }
            await Loading.Hide();
        }
        private bool Check(ContentControl current)
        {
            if (DocumentControlM.Selected == null) return true;
            if (DocumentControlM.Selected.Id == DocumentControlM.List.Where(l => l.Button == current).Single().Id)
                return false;
            return true;
        }
        private void Close()
        {
            local.Content = null;
            DocumentControlM.Selected = null;
        }

        protected internal void Erase(ContentControl current)
        {
            Action func = delegate
            {
                Ok(current);
            };
            Dialog dialog = new Dialog(true, "Dokumentum törlése", func, delegate { }, true);
            dialog.content = new Views.Dialogs.TextBlock("Biztosan eltávolítja a kiválasztott dokumentumot?\n" +
                "A dokumentum törlése csak a \"Változtatások mentése\" gombra kattintva lesz véglegesítve");
            dialog.Start();
        }
        private void Ok(ContentControl current)
        {
            if (!Check(current))
            {
                local.Content = null;
                DocumentControlM.Selected = null;
            }
            DocumentControlM.List.RemoveAt(DocumentControlM.List.Where(l => l.Button == current).Select(l => l.Id).Single() - 1);
        }
        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            DocumentControlM.List.CollectionChanged -= CollectionChangedMethod;
            for (int i = 1; i < DocumentControlM.List.Count; i++)
            {
                DocumentControlM.List[i - 1].Id = i;
            }
            if (ReadOnly)
            {
                SetReadOnlyFields(false);
            }
            else if (!ReadOnly && DocumentControlM.List.Count > 1)
            {
                SetEnabledSave(true);
                SetReadOnlyFields(true);
            }
            else SetEnabledSave(false);
            DocumentControlM.List.CollectionChanged += CollectionChangedMethod;
        }
    }
}
