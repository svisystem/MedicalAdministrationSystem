using MedicalAdministrationSystem.ViewModels.Utilities;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MedicalAdministrationSystem.Views.Fragments
{
    public partial class PictureViewer : UserControl
    {
        private Point start;
        private Point origin;
        protected internal Action close;
        private MemoryStream stream;
        public PictureViewer(MemoryStream stream, Action close)
        {
            this.DataContext = this;
            this.close = close;
            this.stream = stream;
            InitializeComponent();
            this.image.Source = BitmapFrame.Create(new MemoryStream(stream.ToArray()));
        }

        private void clipBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoom = e.Delta > 0 ? .2 : -.2;
            double zoomc = zoom * imageScaleTransform.ScaleX;
            if (!(e.Delta > 0) && (imageScaleTransform.ScaleX < .4 || imageScaleTransform.ScaleY < .4))
                return;

            Point relative = e.GetPosition(transformGrid);
            double abosuluteX;
            double abosuluteY;

            abosuluteX = relative.X * imageScaleTransform.ScaleX + imageTranslateTransform.X;
            abosuluteY = relative.Y * imageScaleTransform.ScaleY + imageTranslateTransform.Y;

            imageScaleTransform.ScaleX += zoomc;
            imageScaleTransform.ScaleY += zoomc;

            imageTranslateTransform.X = abosuluteX - relative.X * imageScaleTransform.ScaleX;
            imageTranslateTransform.Y = abosuluteY - relative.Y * imageScaleTransform.ScaleY;
        }

        private void mainImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(this);
            origin = new Point(imageTranslateTransform.X, imageTranslateTransform.Y);
            this.Cursor = Cursors.Hand;
            transformGrid.CaptureMouse();
        }

        private void mainImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            transformGrid.ReleaseMouseCapture();
            this.Cursor = Cursors.Arrow;
        }

        private void mainImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (transformGrid.IsMouseCaptured)
            {
                Vector v = start - e.GetPosition(this);
                imageTranslateTransform.X = origin.X - v.X;
                imageTranslateTransform.Y = origin.Y - v.Y;
            }
        }
        public void Reset()
        {
            // reset zoom
            imageScaleTransform.ScaleX = 1.0;
            imageScaleTransform.ScaleY = 1.0;

            // reset pan
            imageTranslateTransform.X = 0.0;
            imageTranslateTransform.Y = 0.0;
        }
        private void DefaultView()
        {
            transformGrid.Width = innerdock.ActualWidth - 20;
            transformGrid.Height = innerdock.ActualHeight - 20;
        }
        private void defaultView(object sender, RoutedEventArgs e)
        {
            Reset();
            DefaultView();
        }
        private void bPrint_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new DocumentGenerator().PicturePrint(stream);
        }

        private void bExport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "JPEG fájlok (*.jpg)|*.jpg|Bitmap képek (*.bmp)|*.bmp|PNG fájlok (*.png)|*.png|GIF fájlok (*.gif)|*.gif",
                Title = "Kép mentése",
                DefaultExt = "jpg"
            };
            if ((bool)sfd.ShowDialog() && sfd.OverwritePrompt)
            {
                using (var fileStream = new FileStream(sfd.FileName, FileMode.Create))
                {
                    BitmapEncoder encoder = SelectEncoder(Path.GetExtension(sfd.SafeFileName));
                    encoder.Frames.Add(BitmapFrame.Create(new MemoryStream(stream.ToArray())));
                    encoder.Save(fileStream);
                }
            }
        }
        private BitmapEncoder SelectEncoder(string extension)
        {
            if (extension.Equals(".jpg") || extension.Equals(".jpeg")) return new JpegBitmapEncoder();
            if (extension.Equals(".bmp")) return new BmpBitmapEncoder();
            if (extension.Equals(".png")) return new PngBitmapEncoder();
            if (extension.Equals(".gif")) return new GifBitmapEncoder();
            return new JpegBitmapEncoder();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            close();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultView();
        }
        private void innerdock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.Width = (sender as DockPanel).ActualWidth + 1 > transformGrid.ActualWidth ? (sender as DockPanel).ActualWidth + 1 : transformGrid.ActualWidth;
            grid.Height = (sender as DockPanel).ActualHeight + 1 > transformGrid.ActualHeight ? (sender as DockPanel).ActualHeight + 1 : transformGrid.ActualHeight;
        }
    }
}
