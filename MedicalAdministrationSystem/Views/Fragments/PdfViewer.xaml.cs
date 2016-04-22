using DevExpress.Xpf.DocumentViewer;
using System;
using System.IO;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class PdfViewer : UserControl
    {
        protected internal Action close;
        private BufferedStream bs;
        private MemoryStream memstream;
        public PdfViewer(Action close, MemoryStream ms)
        {
            this.DataContext = this;
            this.close = close;
            InitializeComponent();
            memstream = new MemoryStream(ms.ToArray());
            bs = new BufferedStream(memstream);
            pdfViewer.DocumentSource = bs;
        }

        private void Close(object sender, System.Windows.RoutedEventArgs e)
        {
            pdfViewer.DocumentSource = null;
            close();
        }
        private void SpecifiedZoom(double factor)
        {
            pdfViewer.ZoomMode = ZoomMode.Custom;
            pdfViewer.ZoomFactor = factor;
        }

        private void SpecifiedZoom(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            pdfViewer.ZoomMode = ZoomMode.Custom;
            pdfViewer.ZoomFactor = Convert.ToDouble(sender.GetType().GetProperty("CommandParameter").GetValue(sender));
        }

        private void ActualSize(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            pdfViewer.ZoomMode = ZoomMode.ActualSize;
        }

        private void FullPage(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            pdfViewer.ZoomMode = ZoomMode.PageLevel;
        }

        private void FitWidth(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            pdfViewer.ZoomMode = ZoomMode.FitToWidth;
        }

        private void Layout(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            int id = Convert.ToInt32(sender.GetType().GetProperty("CommandParameter").GetValue(sender));
            if (id == 1)
                pdfViewer.PageLayout = DevExpress.Pdf.PdfPageLayout.SinglePage;
            else if (id == 2)
                pdfViewer.PageLayout = DevExpress.Pdf.PdfPageLayout.OneColumn;
            else if (id == 3)
                pdfViewer.PageLayout = DevExpress.Pdf.PdfPageLayout.TwoPageLeft;
            else if (id == 4)
                pdfViewer.PageLayout = DevExpress.Pdf.PdfPageLayout.TwoColumnLeft;
        }
    }
}
