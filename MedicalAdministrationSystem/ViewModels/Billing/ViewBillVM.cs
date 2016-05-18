using MedicalAdministrationSystem.DataAccess;
using MedicalAdministrationSystem.Models.Billing;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Fragments;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Billing
{
    public class ViewBillVM : VMExtender
    {
        public ViewBillM ViewBillM { get; set; }
        private ContentControl content { get; set; }
        protected internal ViewBillVM(int Id, ref ContentControl content)
        {
            this.content = content;
            Start(Id);
        }
        private void Start(int Id)
        {
            ViewBillM = new ViewBillM();
            ViewBillM.Id = Id;
            BackgroundWorker Loading = new BackgroundWorker();
            Loading.DoWork += new DoWorkEventHandler(LoadingModel);
            Loading.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoadingModelComplete);
            Loading.RunWorkerAsync();
        }
        private void LoadingModel(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (me = new medicalEntities())
                {
                    me.Database.Connection.Open();

                    ViewBillM.Stream = new MemoryStream(me.billing.Where(b => b.IdB == ViewBillM.Id).Single().BillB);

                    me.Database.Connection.Close();
                    workingConn = true;
                }
            }
            catch
            {
                workingConn = false;
            }
        }
        private async void LoadingModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (workingConn) content.Content = new PdfViewer(ViewBillM.Stream);
            else ConnectionMessage();
            await Loading.Hide();
        }
    }
}
