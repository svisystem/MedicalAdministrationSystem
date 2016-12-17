using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MedicalAdministrationSystem.ViewModels.Fragments
{
    public class WordEditorVM : VMExtender
    {
        protected internal int PatientId;
        protected internal async Task TemplatePage(RichEditControl editor, string ExaminationName, string ExaminationCode, Action Save, bool Type)
        {
            await Loading.Show();
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    me = new MedicalModel();
                    workingConn = true;
                    me.Database.Connection.Open();
                    companydata cd = me.companydata.Where(a => a.IdCD == GlobalVM.GlobalM.CompanyId).Single();
                    userdata ud = me.userdata.Where(a => a.IdUD == GlobalVM.GlobalM.UserID).Single();
                    patientdata pd = me.patientdata.Where(a => a.IdPD == PatientId).Single();
                    return new DocumentGenerator().Template(
                        Type,
                        cd.NameCD,
                        me.zipcode_fx.Where(a => a.IdZC == cd.ZipCodeCD).Select(a => a.DataZC).Single().ToString(),
                        me.settlement_fx.Where(a => a.IdS == cd.SettlementCD).Select(a => a.DataS).Single(),
                        cd.AddressCD,
                        ud.NameUD,
                        (int)ud.SealNumberUD,
                        pd.NamePD,
                        pd.MotherNamePD,
                        pd.BirthDatePD,
                        pd.TAJNumberPD,
                        me.zipcode_fx.Where(a => a.IdZC == pd.ZipCodePD).Select(a => a.DataZC).Single().ToString(),
                        me.settlement_fx.Where(a => a.IdS == pd.SettlementPD).Select(a => a.DataS).Single(),
                        pd.AddressPD,
                        ExaminationName,
                        ExaminationCode);
                }
                catch
                {
                    workingConn = false;
                    return null;
                }
                finally
                {
                    me.Database.Connection.Close();
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(task =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(async () =>
                {
                    using (MemoryStream ms = new MemoryStream(task.Result.ToArray()))
                        editor.LoadDocument(ms, DocumentFormat.OpenXml);
                    Save();
                    await Loading.Hide();
                }));
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            });
        }
        protected internal void CloseQuestion(Action action, bool modified)
        {
            if (modified)
            {
                Dialog dialog = new Dialog(false, "El nem menetett változások lehetnek az adott oldalon", action, () => { }, true);
                dialog.content = new Views.Dialogs.TextBlock("Amennyiben elnavigál erről az oldalról, az azon végrehajtott változások nem lesznek elmentve\n\n" +
                    "Biztosan elnavigál erről az oldalról?");
                dialog.Start();
            }
            else action();
        }
        protected internal void NewDataQuestion(Action action, DocumentControlM.ListElement element)
        {
            if (element.File != null)
            {
                Dialog dialog = new Dialog(false, "El nem menetett változások lehetnek az adott oldalon", action, () => { }, true);
                dialog.content = new Views.Dialogs.TextBlock("Amennyiben felülrja a mostani dokumentumot, úgy az elveszik\n\n" +
                    "Biztosan betölti az új dokumentumot?");
                dialog.Start();
            }
            else action();
        }
        protected internal DocumentFormat DocFormat(string FileType)
        {
            if (FileType == ".rtf") return DocumentFormat.Rtf;
            else if (FileType == ".txt") return DocumentFormat.PlainText;
            else if (FileType == ".htm") return DocumentFormat.Html;
            else if (FileType == ".html") return DocumentFormat.Html;
            else if (FileType == ".mht") return DocumentFormat.Mht;
            else if (FileType == ".docx") return DocumentFormat.OpenXml;
            else if (FileType == ".odt") return DocumentFormat.OpenDocument;
            else if (FileType == ".xml") return DocumentFormat.WordML;
            else if (FileType == ".epub") return DocumentFormat.ePub;
            else if (FileType == ".doc") return DocumentFormat.Doc;
            else return DocumentFormat.Undefined;
        }
    }
}
