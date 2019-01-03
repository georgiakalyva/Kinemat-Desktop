using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace Kinemat.UI.Controls
{
    /// <summary>
    /// Interaction logic for BookPage.xaml
    /// </summary>
    partial class BookPage : ContentControl
    {
        public BookPage()
        {
            InitializeComponent();
        }

        private const int animationDuration = 500;

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            ApplyParameters(new PageParameters(this.RenderSize));
        }

        void anim_Completed(object sender, EventArgs e)
        {
            //Application.Current.MainWindow.Title += "C";

            ApplyParameters(new PageParameters(this.RenderSize));

            if (Status == PageStatus.TurnAnimation)
            {
                Status = PageStatus.None;
                RaiseEvent(new RoutedEventArgs(BookPage.PageTurnedEvent, this));
            }
            else
                Status = PageStatus.None;
        }

        void anim_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            //Application.Current.MainWindow.Title += "I";
            PageParameters? parameters = ComputePage(this, CornerPoint, origin);
            _cornerPoint = CornerPoint;
            if (parameters != null)
                ApplyParameters(parameters.Value);
        }

        internal CornerOrigin origin = CornerOrigin.BottomRight;
        private const double gripSize = 30;
        private PageStatus _status = PageStatus.None;
        internal Action<PageStatus> SetStatus = null;
        internal Read<PageStatus> GetStatus = null;

        public PageStatus Status
        {
            private get
            {
                if (GetStatus != null)
                    return GetStatus();
                else
                    return _status;
            }
            set
            {
                if (SetStatus != null)
                    SetStatus(value);
                else
                    _status = value;
                gridShadow.Visibility = value == PageStatus.None ? Visibility.Hidden : Visibility.Visible;
                canvasReflection.Visibility = value == PageStatus.None ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private Point _cornerPoint;

        private Point CornerPoint
        {
            get { return (Point)GetValue(BookPage.CornerPointProperty); }
            set { SetValue(BookPage.CornerPointProperty, value); }
        }

        private void ApplyParameters(PageParameters parameters)
        {
            pageReflection.Opacity = parameters.Page0ShadowOpacity;

            rectangleRotate.Angle = parameters.Page1RotateAngle;
            rectangleRotate.CenterX = parameters.Page1RotateCenterX;
            rectangleRotate.CenterY = parameters.Page1RotateCenterY;
            rectangleTranslate.X = parameters.Page1TranslateX;
            rectangleTranslate.Y = parameters.Page1TranslateY;
            clippingFigure.Figures.Clear();
            clippingFigure.Figures.Add(parameters.Page1ClippingFigure);

            RectangleGeometry rg = (RectangleGeometry)clippingPage0.Geometry1;
            rg.Rect = new Rect(parameters.RenderSize);
            PathGeometry pg = (PathGeometry)clippingPage0.Geometry2;
            pg.Figures.Clear();
            pg.Figures.Add(parameters.Page2ClippingFigure);

            pageReflection.StartPoint = parameters.Page1ReflectionStartPoint;
            pageReflection.EndPoint = parameters.Page1ReflectionEndPoint;

            pageShadow.StartPoint = parameters.Page0ShadowStartPoint;
            pageShadow.EndPoint = parameters.Page0ShadowEndPoint;
        }

        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            if ((Status == PageStatus.DropAnimation) || (Status == PageStatus.TurnAnimation))
                return;

            //Application.Current.MainWindow.Title += "M";

            UIElement source = sender as UIElement;
            Point p = args.GetPosition(source);

            if (!(sender as UIElement).IsMouseCaptured)
            {
                CornerOrigin? tmp = GetCorner(source, p);

                if (tmp.HasValue)
                    origin = tmp.Value;
                else
                {
                    if (Status == PageStatus.DraggingWithoutCapture)
                    {
                        DropPage(ComputeAnimationDuration(source, p, origin));
                    }
                    return;
                }
                Status = PageStatus.DraggingWithoutCapture;
            }

            PageParameters? parameters = ComputePage(source, p, origin);
            _cornerPoint = p;
            if (parameters != null)
                ApplyParameters(parameters.Value);
        }

        private static int ComputeAnimationDuration(UIElement source, Point p, CornerOrigin origin)
        {
            double ratio = ComputeProgressRatio(source, p, origin);

            return Convert.ToInt32(animationDuration * (ratio / 2 + 0.5));
        }

        private static double ComputeProgressRatio(UIElement source, Point p, CornerOrigin origin)
        {
            if ((origin == CornerOrigin.BottomLeft) || (origin == CornerOrigin.TopLeft))
                return p.X / source.RenderSize.Width;
            else
                return (source.RenderSize.Width - p.X) / source.RenderSize.Width;
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            //Application.Current.MainWindow.Title += "D";

            //UIElement source = sender as UIElement;
            //Point p = args.GetPosition(source);

            //if (GetCorner(source, p).HasValue)
            //    TurnPage(animationDuration);
        }

        private CornerOrigin? GetCorner(UIElement source, Point position)
        {
            CornerOrigin? result = null;

            Rect topLeftRectangle = new Rect(0, 0, gripSize, gripSize);
            Rect topRightRectangle = new Rect(source.RenderSize.Width - gripSize, 0, gripSize, gripSize);
            Rect bottomLeftRectangle = new Rect(0, source.RenderSize.Height - gripSize, gripSize, gripSize);
            Rect bottomRightRectangle = new Rect(source.RenderSize.Width - gripSize, source.RenderSize.Height - gripSize, gripSize, gripSize);

            if (IsTopLeftCornerEnabled && topLeftRectangle.Contains(position))
                result = CornerOrigin.TopLeft;
            else if (IsTopRightCornerEnabled && topRightRectangle.Contains(position))
                result = CornerOrigin.TopRight;
            else if (IsBottomLeftCornerEnabled && bottomLeftRectangle.Contains(position))
                result = CornerOrigin.BottomLeft;
            else if (IsBottomRightCornerEnabled && bottomRightRectangle.Contains(position))
                result = CornerOrigin.BottomRight;

            return result;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs args)
        {
            if ((Status == PageStatus.DropAnimation) || (Status == PageStatus.TurnAnimation))
                return;

            UIElement source = sender as UIElement;
            Point p = args.GetPosition(source);

            CornerOrigin? tmp = GetCorner(source, p);

            if (tmp.HasValue)
            {
                origin = tmp.Value;
                this.CaptureMouse();
            }
            else
                return;

            Status = PageStatus.Dragging;
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            if (this.IsMouseCaptured)
            {
                Status = PageStatus.None;

                UIElement source = sender as UIElement;
                Point p = args.GetPosition(source);

                if (IsOnNextPage(args.GetPosition(this), this, origin))
                    TurnPage(animationDuration);
                else
                    DropPage(ComputeAnimationDuration(source, p, origin));

                this.ReleaseMouseCapture();
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs args)
        {
            if (Status == PageStatus.DraggingWithoutCapture)
            {
                //DropPage(ComputeAnimationDuration(source, p));
                DropPage(animationDuration);
            }
        }
        private Point OriginToPoint(UIElement source, CornerOrigin origin)
        {
            switch (origin)
            {
                case CornerOrigin.BottomLeft:
                    return new Point(0, source.RenderSize.Height);
                case CornerOrigin.BottomRight:
                    return new Point(source.RenderSize.Width, source.RenderSize.Height);
                case CornerOrigin.TopRight:
                    return new Point(source.RenderSize.Width, 0);
                default:
                    return new Point(0, 0);
            }
        }
        private Point OriginToOppositePoint(UIElement source, CornerOrigin origin)
        {
            switch (origin)
            {
                case CornerOrigin.BottomLeft:
                    return new Point(source.RenderSize.Width * 2, source.RenderSize.Height);
                case CornerOrigin.BottomRight:
                    return new Point(-source.RenderSize.Width, source.RenderSize.Height);
                case CornerOrigin.TopRight:
                    return new Point(-source.RenderSize.Width, 0);
                default:
                    return new Point(source.RenderSize.Width * 2, 0);
            }
        }
        private bool IsOnNextPage(Point p, UIElement source, CornerOrigin origin)
        {
            switch (origin)
            {
                case CornerOrigin.BottomLeft:
                case CornerOrigin.TopLeft:
                    return p.X > source.RenderSize.Width;
                default:
                    return p.X < 0;
            }
        }

        private void DropPage(int duration)
        {
            Status = PageStatus.DropAnimation;

            UIElement source = this as UIElement;
            CornerPoint = _cornerPoint;

            this.BeginAnimation(BookPage.CornerPointProperty, null);
            PointAnimation anim =
                new PointAnimation(
                    OriginToPoint(this, origin),
                    new Duration(TimeSpan.FromMilliseconds(duration)));
            anim.AccelerationRatio = 0.6;

            anim.CurrentTimeInvalidated += new EventHandler(anim_CurrentTimeInvalidated);
            anim.Completed += new EventHandler(anim_Completed);
            this.BeginAnimation(BookPage.CornerPointProperty, anim);
        }

        public void TurnPage()
        {
            TurnPage(animationDuration);
        }

        private void TurnPage(int duration)
        {
            Status = PageStatus.TurnAnimation;

            UIElement source = this as UIElement;
            CornerPoint = _cornerPoint;

            this.BeginAnimation(BookPage.CornerPointProperty, null);
            PointAnimation anim =
                new PointAnimation(
                    OriginToOppositePoint(this, origin),
                    new Duration(TimeSpan.FromMilliseconds(duration)));
            anim.AccelerationRatio = 0.6;

            anim.CurrentTimeInvalidated += new EventHandler(anim_CurrentTimeInvalidated);
            anim.Completed += new EventHandler(anim_Completed);
            this.BeginAnimation(BookPage.CornerPointProperty, anim);
        }

        public void AutoTurnPage(CornerOrigin fromCorner, int duration)
        {
            if (Status != PageStatus.None)
                return;

            Status = PageStatus.TurnAnimation;

            UIElement source = this as UIElement;

            this.BeginAnimation(BookPage.CornerPointProperty, null);

            Point startPoint = OriginToPoint(this, fromCorner);
            Point endPoint = OriginToOppositePoint(this, fromCorner);

            CornerPoint = startPoint;
            origin = fromCorner;

            BezierSegment bs =
                new BezierSegment(startPoint, new Point(endPoint.X + (startPoint.X - endPoint.X) / 3, 250), endPoint, true);

            PathGeometry path = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = startPoint;
            figure.Segments.Add(bs);
            figure.IsClosed = false;
            path.Figures.Add(figure);

            PointAnimationUsingPath anim =
                new PointAnimationUsingPath();
            anim.PathGeometry = path;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
            anim.AccelerationRatio = 0.6;

            anim.CurrentTimeInvalidated += new EventHandler(anim_CurrentTimeInvalidated);
            anim.Completed += new EventHandler(anim_Completed);
            this.BeginAnimation(BookPage.CornerPointProperty, anim);
        }

        public bool IsTopLeftCornerEnabled
        {
            get { return (bool)GetValue(BookPage.IsTopLeftCornerEnabledProperty); }
            set { SetValue(BookPage.IsTopLeftCornerEnabledProperty, value); }
        }
        public bool IsTopRightCornerEnabled
        {
            get { return (bool)GetValue(BookPage.IsTopRightCornerEnabledProperty); }
            set { SetValue(BookPage.IsTopRightCornerEnabledProperty, value); }
        }
        public bool IsBottomLeftCornerEnabled
        {
            get { return (bool)GetValue(BookPage.IsBottomLeftCornerEnabledProperty); }
            set { SetValue(BookPage.IsBottomLeftCornerEnabledProperty, value); }
        }
        public bool IsBottomRightCornerEnabled
        {
            get { return (bool)GetValue(BookPage.IsBottomRightCornerEnabledProperty); }
            set { SetValue(BookPage.IsBottomRightCornerEnabledProperty, value); }
        }

        public static DependencyProperty CornerPointProperty;
        public static DependencyProperty IsTopLeftCornerEnabledProperty;
        public static DependencyProperty IsTopRightCornerEnabledProperty;
        public static DependencyProperty IsBottomLeftCornerEnabledProperty;
        public static DependencyProperty IsBottomRightCornerEnabledProperty;

        public static readonly RoutedEvent PageTurnedEvent;

        public event RoutedEventHandler PageTurned
        {
            add
            {
                base.AddHandler(PageTurnedEvent, value);
            }
            remove
            {
                base.RemoveHandler(PageTurnedEvent, value);
            }
        }

        static BookPage()
        {
            PageTurnedEvent = EventManager.RegisterRoutedEvent("PageTurned", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BookPage));
            CornerPointProperty = DependencyProperty.Register("CornerPoint", typeof(Point), typeof(BookPage));
            IsTopLeftCornerEnabledProperty = DependencyProperty.Register("IsTopLeftCornerEnabled", typeof(bool), typeof(BookPage), new PropertyMetadata(true));
            IsTopRightCornerEnabledProperty = DependencyProperty.Register("IsTopRightCornerEnabled", typeof(bool), typeof(BookPage), new PropertyMetadata(true));
            IsBottomLeftCornerEnabledProperty = DependencyProperty.Register("IsBottomLeftCornerEnabled", typeof(bool), typeof(BookPage), new PropertyMetadata(true));
            IsBottomRightCornerEnabledProperty = DependencyProperty.Register("IsBottomRightCornerEnabled", typeof(bool), typeof(BookPage), new PropertyMetadata(true));
        }

        #region Compute
        private static PageParameters ResetPage(UIElement source, CornerOrigin origin)
        {
            PageParameters _parameters = new PageParameters(source.RenderSize);
            _parameters.Page0ShadowOpacity = 0;
            _parameters.Page1ClippingFigure = new PathFigure();
            _parameters.Page1ReflectionStartPoint = new Point(0, 0);
            _parameters.Page1ReflectionEndPoint = new Point(0, 0);
            _parameters.Page1RotateAngle = 0;
            _parameters.Page1RotateCenterX = 0;
            _parameters.Page1RotateCenterY = 0;
            _parameters.Page1TranslateX = 0;
            _parameters.Page1TranslateY = 0;
            _parameters.Page2ClippingFigure = new PathFigure();
            _parameters.Page0ShadowStartPoint = new Point(0, 0);
            _parameters.Page0ShadowEndPoint = new Point(0, 0);

            return _parameters;
        }

        private static double epsilon = 0.001;

        private static PageParameters? ComputePage(UIElement source, Point p, CornerOrigin origin)
        {
            CheckParams(ref source, ref p, origin);

            PageParameters _parameters = new PageParameters(source.RenderSize);

            double ratio = ComputeProgressRatio(source, p, origin);
            if (ratio > 1.5)
                ratio = (2 - ratio) / 0.5;
            else
                ratio = 1;
            _parameters.Page0ShadowOpacity = ratio;

            double xc = source.RenderSize.Width;
            double yc = source.RenderSize.Height;

            switch (origin)
            {
                case CornerOrigin.TopLeft:
                    p.X = xc - p.X;
                    p.Y = yc - p.Y;
                    break;
                case CornerOrigin.TopRight:
                    p.Y = yc - p.Y;
                    break;
                case CornerOrigin.BottomLeft:
                    p.X = xc - p.X;
                    break;
            }

            if (p.X >= xc)
                return null;

            // x = a * y + b
            double a = -(p.Y - yc) / (p.X - xc);
            double b = (xc + p.X) / 2 - a * ((yc + p.Y) / 2);

            double h1 = (xc - b) / a;
            double l1 = a * yc + b;

            double angle = (Math.Atan((xc - p.X) / (h1 - p.Y))) * 180 / Math.PI;
            if ((a < 0) && (p.Y < h1))
                angle = angle - 180;

            switch (origin)
            {
                case CornerOrigin.BottomRight:
                    _parameters.Page1RotateAngle = -angle;
                    _parameters.Page1RotateCenterX = p.X;
                    _parameters.Page1RotateCenterY = p.Y;
                    _parameters.Page1TranslateX = p.X;
                    _parameters.Page1TranslateY = p.Y - yc;
                    break;
                case CornerOrigin.TopLeft:
                    _parameters.Page1RotateAngle = -angle;
                    _parameters.Page1RotateCenterX = xc - p.X;
                    _parameters.Page1RotateCenterY = yc - p.Y;
                    _parameters.Page1TranslateX = -p.X;
                    _parameters.Page1TranslateY = yc - p.Y;
                    break;
                case CornerOrigin.TopRight:
                    _parameters.Page1RotateAngle = angle;
                    _parameters.Page1RotateCenterX = p.X;
                    _parameters.Page1RotateCenterY = yc - p.Y;
                    _parameters.Page1TranslateX = p.X;
                    _parameters.Page1TranslateY = yc - p.Y;
                    break;
                case CornerOrigin.BottomLeft:
                    _parameters.Page1RotateAngle = angle;
                    _parameters.Page1RotateCenterX = xc - p.X;
                    _parameters.Page1RotateCenterY = p.Y;
                    _parameters.Page1TranslateX = -p.X;
                    _parameters.Page1TranslateY = p.Y - yc;
                    break;
            }

            switch (origin)
            {
                case CornerOrigin.BottomRight:
                    if (angle < 0)
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(0, yc);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc - l1, yc), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(0, h1), false));
                    }
                    else
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(0, 0);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc - b, 0), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(0, h1), false));
                    }
                    break;
                case CornerOrigin.TopLeft:
                    if (angle < 0)
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(xc, 0);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(l1, 0), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc, yc - h1), false));
                    }
                    else
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(xc, yc);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(b, yc), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc, yc - h1), false));
                    }
                    break;
                case CornerOrigin.BottomLeft:
                    if (angle < 0)
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(xc, yc);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(l1, yc), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc, h1), false));
                    }
                    else
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(xc, 0);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(b, 0), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc, h1), false));
                    }
                    break;
                case CornerOrigin.TopRight:
                    if (angle < 0)
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(0, 0);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc - l1, 0), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(0, yc - h1), false));
                    }
                    else
                    {
                        _parameters.Page1ClippingFigure.StartPoint = new Point(0, yc);
                        _parameters.Page1ClippingFigure.Segments.Clear();
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(xc - b, yc), false));
                        _parameters.Page1ClippingFigure.Segments.Add(new LineSegment(new Point(0, yc - h1), false));
                    }
                    break;
            }

            _parameters.Page2ClippingFigure.StartPoint = new Point(xc - _parameters.Page1ClippingFigure.StartPoint.X, _parameters.Page1ClippingFigure.StartPoint.Y);

            _parameters.Page2ClippingFigure.Segments = _parameters.Page1ClippingFigure.Segments.Clone();
            ((LineSegment)_parameters.Page2ClippingFigure.Segments[0]).Point
                = new Point(xc - ((LineSegment)_parameters.Page2ClippingFigure.Segments[0]).Point.X,
                    ((LineSegment)_parameters.Page2ClippingFigure.Segments[0]).Point.Y);
            ((LineSegment)_parameters.Page2ClippingFigure.Segments[1]).Point
                = new Point(xc - ((LineSegment)_parameters.Page2ClippingFigure.Segments[1]).Point.X,
                    ((LineSegment)_parameters.Page2ClippingFigure.Segments[1]).Point.Y);

            Point refletStartPoint;
            Point refletEndPoint;

            CornerOrigin oppositeOrigin = CornerOrigin.TopLeft;
            switch (origin)
            {
                case CornerOrigin.BottomLeft:
                    oppositeOrigin = CornerOrigin.BottomRight;
                    break;
                case CornerOrigin.BottomRight:
                    oppositeOrigin = CornerOrigin.BottomLeft;
                    break;
                case CornerOrigin.TopLeft:
                    oppositeOrigin = CornerOrigin.TopRight;
                    break;
                case CornerOrigin.TopRight:
                    oppositeOrigin = CornerOrigin.TopLeft;
                    break;
            }

            LinearGradientHelper.ComputePointsFromTop(xc, yc, xc - l1, yc - h1, 20, 20,
                oppositeOrigin,
                out refletStartPoint,
                out refletEndPoint);

            _parameters.Page1ReflectionStartPoint = refletStartPoint;
            _parameters.Page1ReflectionEndPoint = refletEndPoint; //new Point(1, 1 / Math.Tan((90 - angleClipping) * Math.PI / 180));

            Point startPoint;
            Point endPoint;

            double d = Math.Sqrt(Math.Pow(p.X - xc, 2) + Math.Pow(p.Y - yc, 2));

            double r1 = d / 10;
            double r2 = d / 10;

            LinearGradientHelper.ComputePoints(xc, yc, xc - l1, yc - h1, r1, r2,
                origin, out startPoint, out endPoint);

            _parameters.Page0ShadowStartPoint = startPoint;
            _parameters.Page0ShadowEndPoint = endPoint;

            return _parameters;
        }

        private static void CheckParams(ref UIElement source, ref Point p, CornerOrigin origin)
        {
            switch (origin)
            {
                case CornerOrigin.TopRight:
                    if (origin == CornerOrigin.TopRight)
                    {
                        if (p.Y == 0)
                            p.Y = epsilon;
                        if (p.X == 0)
                            p.X = epsilon;
                        if (p.Y > 0)
                        {
                            double d = Math.Sqrt(p.X * p.X + p.Y * p.Y);
                            if (d > source.RenderSize.Width)
                            {
                                double x = p.X * source.RenderSize.Width / d;
                                double y = p.Y * source.RenderSize.Width / d;
                                p.X = x;
                                p.Y = y;
                            }
                        }
                        else
                        {
                            double d = Math.Sqrt(p.X * p.X + p.Y * p.Y);
                            double R = Math.Sqrt(source.RenderSize.Width * source.RenderSize.Width + source.RenderSize.Height * source.RenderSize.Height);

                            double a = (p.Y * p.Y) / (p.X * p.X) + 1;
                            double b = -2 * p.Y * source.RenderSize.Height / p.X;
                            double c = source.RenderSize.Height * source.RenderSize.Height - R * R;
                            double delta = b * b - 4 * a * c;

                            double x = 0;
                            if (p.X > 0)
                                x = (-b + Math.Sqrt(delta)) / (2 * a);
                            else
                                x = (-b - Math.Sqrt(delta)) / (2 * a);

                            double y = p.Y * x / p.X;

                            if (Math.Abs(x) < Math.Abs(p.X))
                            {
                                p.X = x;
                                p.Y = y;
                            }
                        }
                        if (source.RenderSize.Width - p.X == p.Y)
                            p.X += epsilon;
                    }
                    break;

                case CornerOrigin.TopLeft:
                    p.X = source.RenderSize.Width - p.X;
                    CheckParams(ref source, ref p, CornerOrigin.TopRight);
                    p.X = source.RenderSize.Width - p.X;
                    break;
                case CornerOrigin.BottomRight:
                    p.Y = source.RenderSize.Height - p.Y;
                    CheckParams(ref source, ref p, CornerOrigin.TopRight);
                    p.Y = source.RenderSize.Height - p.Y;
                    break;
                case CornerOrigin.BottomLeft:
                    p.Y = source.RenderSize.Height - p.Y;
                    p.X = source.RenderSize.Width - p.X;
                    CheckParams(ref source, ref p, CornerOrigin.TopRight);
                    p.Y = source.RenderSize.Height - p.Y;
                    p.X = source.RenderSize.Width - p.X;
                    break;
            }
        }
        #endregion
    }

    public delegate T Read<T>();
}

