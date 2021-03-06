﻿using DevExpress.Xpf.DocumentViewer;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class PdfViewer : UserControl
    {
        public Visible Bookmark { get; set; } = new Visible() { Visibility = Visibility.Collapsed };
        protected internal Action close;
        private MemoryStream memstream;
        public PdfViewer(Action close, MemoryStream ms)
        {
            this.DataContext = this;
            this.close = close;
            InitializeComponent();
            memstream = new MemoryStream(ms.ToArray());
            pdfViewer.DocumentSource = new BufferedStream(memstream);
            pdfViewer.ZoomMode = ZoomMode.FitToWidth;
        }
        Action Load;
        public PdfViewer(MemoryStream ms, Action Load)
        {
            this.Load = Load;
            this.DataContext = this;
            InitializeComponent();
            exit.Visibility = Visibility.Collapsed;
            memstream = new MemoryStream(ms.ToArray());
            pdfViewer.DocumentLoaded += DocumentLoaded;
            pdfViewer.DocumentSource = new BufferedStream(memstream);
            pdfViewer.ZoomMode = ZoomMode.FitToWidth;
        }
        private void DocumentLoaded(object sender, RoutedEventArgs e)
        {
            Load();
        }
        protected internal void ClearStream()
        {
            memstream.SetLength(0);
            (pdfViewer.DocumentSource as BufferedStream).SetLength(0);
            pdfViewer.DocumentSource = null;
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            ClearStream();
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

        private void bBookmark_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Bookmark.Visibility = Bookmark.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
        public class Visible : NotifyPropertyChanged
        {
            private Visibility _Visibility;
            public Visibility Visibility
            {
                get
                {
                    return _Visibility;
                }
                set
                {
                    if (value == _Visibility) return;
                    _Visibility = value;
                    OnPropertyChanged("Visibility");
                }
            }
        }
    }
}
