using DevExpress.Xpf.Charts;
using MedicalAdministrationSystem.Models.Statistics;
using MedicalAdministrationSystem.ViewModels.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MedicalAdministrationSystem.Views.Statistics.Graphs
{
    public partial class Pie : UserControl
    {
        public Rotation Rotate { get; set; } = new Rotation();
        const int clickDelta = 200;

        DateTime mouseDownTime;
        bool rotate;
        Point startPosition;

        public ObservableCollection<ChartM.Record> Data { get; set; }
        public Pie(ObservableCollection<ChartM.Record> Data)
        {
            this.Data = Data;
            this.DataContext = this;
            InitializeComponent();
            SeriesLabel.TextPattern = "{V},\n{VP:P0}";
        }

        private void CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            e.DrawOptions.Color = ChartControl.Palette[(e.SeriesPoint.Tag as ChartM.Record).Id];
        }

        bool IsClick(DateTime mouseUpTime)
        {
            return (mouseUpTime - mouseDownTime).TotalMilliseconds < clickDelta;
        }
        double CalcAngle(Point p1, Point p2)
        {
            Point center = new Point(ChartControl.Diagram.ActualWidth / 2, ChartControl.ActualHeight / 2);
            Point relativeP1 = new Point(p1.X - center.X, p1.Y - center.Y);
            Point relativeP2 = new Point(p2.X - center.X, p2.Y - center.Y);
            double angleP1Radian = Math.Atan2(relativeP1.X, relativeP1.Y);
            double angleP2Radian = Math.Atan2(relativeP2.X, relativeP2.Y);
            double angle = (angleP2Radian - angleP1Radian) * 180 / (Math.PI * 2);
            if (angle > 90)
                angle = 180 - angle;
            else if (angle < -90)
                angle = 180 + angle;
            return angle;
        }
        void chart_MouseUp(object sender, MouseButtonEventArgs e) =>
            AnimationEnd(sender, e.GetPosition(sender as ChartControl));
        void AnimationEnd(object sender, Point e)
        {
            ChartHitInfo hitInfo = (sender as ChartControl).CalcHitInfo(e);
            rotate = false;
            if (hitInfo == null || hitInfo.SeriesPoint == null || !IsClick(DateTime.Now))
                return;
            double distance = PieSeries.GetExplodedDistance(hitInfo.SeriesPoint);
            Storyboard storyBoard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300));
            animation.To = distance > 0 ? 0 : 0.3;
            storyBoard.Children.Add(animation);
            Storyboard.SetTarget(animation, hitInfo.SeriesPoint);
            Storyboard.SetTargetProperty(animation, new PropertyPath(PieSeries.ExplodedDistanceProperty));
            storyBoard.Begin();
        }
        void chart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownTime = DateTime.Now;
            Point position = e.GetPosition(ChartControl);
            ChartHitInfo hitInfo = ChartControl.CalcHitInfo(position);
            if (hitInfo != null && hitInfo.SeriesPoint != null)
            {
                rotate = true;
                startPosition = position;
            }
        }
        void chart_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(ChartControl);
            ChartHitInfo hitInfo = ChartControl.CalcHitInfo(position);
            if (!hitInfo.InDiagram || hitInfo == null)
                AnimationEnd(sender, position);
            if (rotate && !IsClick(DateTime.Now))
            {
                PieSeries2D series = ChartControl.Diagram.Series[0] as PieSeries2D;
                double angleDelta = CalcAngle(startPosition, position);
                if (Math.Abs(Rotate.Value + angleDelta) < 360)
                    Rotate.Value += angleDelta;
                else if (Rotate.Value + angleDelta > 360)
                    Rotate.Value = -360;
                else
                    Rotate.Value = 360;
                startPosition = position;
            }
        }
        void ChartsDemoModule_ModuleAppear(object sender, RoutedEventArgs e)
        {
            ChartControl.Animate();
        }
        void rblSweepDirection_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (ChartControl != null)
                ChartControl.Animate();
        }
        void chart_QueryChartCursor(object sender, QueryChartCursorEventArgs e)
        {
            ChartHitInfo hitInfo = ChartControl.CalcHitInfo(e.Position);
            if (hitInfo != null && hitInfo.SeriesPoint != null)
                e.Cursor = Cursors.Hand;
        }
        public class Rotation : NotifyPropertyChanged
        {
            private double _Value;

            public double Value
            {
                get
                {
                    return _Value;
                }
                set
                {
                    if (_Value == value) return;
                    else
                    {
                        _Value = value;
                        OnPropertyChanged("Value");
                    }
                }
            }
        }
    }
}
