using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Fragments;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
        private string State;
        protected internal bool Type; //true = examination, false = evidence
        protected internal DocumentControlVM(ref ContentControl content, ObservableCollection<DocumentControlM.ListElement> List)
        {
            local = content;
            DocumentControlM.List = List;
        }
        protected internal void New(int PatientId)
        {
            DocumentControlM.PatientId = PatientId;
            State = "New";
            Add(State);
            CollectionChange();
            Loaded();
        }
        protected internal void Edit(int PatientId)
        {
            DocumentControlM.PatientId = PatientId;
            State = "Edit";
            Add("New");
            CollectionChange();
            Loaded();
        }
        protected internal void ReadOnly()
        {
            State = "ReadOnly";
            CollectionChange();
            Loaded();
        }
        protected internal void Loaded()
        {
            foreach (DocumentControlM.ListElement row in DocumentControlM.List)
                row.AcceptChanges();
            DocumentControlM.AcceptChanges();
            Buttons();
        }
        protected internal async void Add(string type, string fileType = null, int? dbId = null, MemoryStream ms = null)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
             {
                 if (type == "New")
                     DocumentControlM.List.Add(new DocumentControlM.ListElement()
                     {
                         Button = new New(NewAdd)
                     });
                 else
                     DocumentControlM.List.Insert(Counter(), new DocumentControlM.ListElement()
                     {
                         DBId = dbId,
                         File = ms,
                         ButtonType = type,
                         FileType = fileType,
                         Button = new Views.Fragments.File((State == "ReadOnly"), type, BeforeShow, Erase)
                     });
                 CollectionChange();
                 Loaded();
             }));
        }
        private int Counter()
        {
            if (State != "ReadOnly") return DocumentControlM.List.Count > 0 ? DocumentControlM.List.Count - 1 : 0;
            return DocumentControlM.List.Count;
        }
        protected internal List<int> Erased()
        {
            return DocumentControlM.Erased;
        }
        private void NewAdd()
        {
            if (Validate())
            {
                if (local.Content != null)
                {
                    if (local.Content.GetType() == typeof(WordEditor))
                        (local.Content as WordEditor).WordEditorVM.CloseQuestion(() =>
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
                Dialog dialog = new Dialog(false, "Hiányzó adatok", () => { });
                dialog.content = new Views.Dialogs.TextBlock("Először töltse ki a vizsgálathoz szükséges adatokat");
                dialog.Start();
            }
        }
        private void DOCClick()
        {
            DocumentControlM.List.Insert(DocumentControlM.List.Count - 1, new DocumentControlM.ListElement()
            {
                Button = new Views.Fragments.File((State == "ReadOnly"), "doc", BeforeShow, Erase),
                ButtonType = "doc"
            });
            BeforeShow(DocumentControlM.List[DocumentControlM.List.Count - 2].Button, "doc");
            DocumentControlM.List[DocumentControlM.List.Count - 2].AcceptChanges();
            SetEnabledSave(false);
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
                        Button = new Views.Fragments.File((State == "ReadOnly"), "pdf", BeforeShow, Erase),
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
                        Button = new Views.Fragments.File((State == "ReadOnly"), "jpg", BeforeShow, Erase),
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
                (local.Content as WordEditor).WordEditorVM.CloseQuestion(() => Show(current, from), (local.Content as WordEditor).wordEditor.Modified);
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
                    if (State == "ReadOnly")
                        local.Content = new WordEditor(
                            DocumentControlM.List.Where(l => l.Button == current).Single(),
                            Close,
                            Type);
                    else local.Content = new WordEditor(
                        DocumentControlM.List.Where(l => l.Button == current).Single(),
                        DocumentControlM.PatientId,
                        Type ? GetName() : null,
                        GetCode(),
                        Close,
                        Type,
                        Buttons);
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
            Dialog dialog = new Dialog(true, "Dokumentum törlése", () => Ok(current), () => { }, true);
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
            if (DocumentControlM.List.Where(l => l.Button == current).Single().DBId != null)
                DocumentControlM.Erased.Add((int)DocumentControlM.List.Where(l => l.Button == current).Single().DBId);
            DocumentControlM.List.RemoveAt(DocumentControlM.List.Where(l => l.Button == current).Single().Id - 1);
        }
        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChange();
        }
        private void CollectionChange()
        {
            DocumentControlM.List.CollectionChanged -= CollectionChangedMethod;
            for (int i = 0; i < DocumentControlM.List.Count; i++)
                DocumentControlM.List[i].Id = i + 1;
            Buttons();
            DocumentControlM.List.CollectionChanged += CollectionChangedMethod;
        }
        private void Buttons()
        {
            if (State == "ReadOnly")
            {
                SetEnabledSave(false);
                SetReadOnlyFields(true);
            }
            else
            {
                if (DocumentControlM.List.Count > 1)
                {
                    SetEnabledSave(true);
                    SetReadOnlyFields(true);
                }

                else
                {
                    if (State == "New")
                    {
                        SetEnabledSave(false);
                        SetReadOnlyFields(false);
                    }
                    else
                    {
                        SetEnabledSave(false);
                        SetReadOnlyFields(true);
                    }
                }
            }
        }
        protected internal bool VMDirty()
        {
            if (DocumentControlM.List.Any(i => i.IsChanged)) return true;
            return false;
        }
    }
}
