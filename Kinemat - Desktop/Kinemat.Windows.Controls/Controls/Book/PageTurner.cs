// Type: Telerik.Windows.Controls.Book.PageTurner
// Assembly: Telerik.Windows.Controls.Navigation, Version=2013.1.403.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// Assembly location: W:\Developer\Telerik\Binaries\WPF45\Telerik.Windows.Controls.Navigation.dll

using System;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Kinemat.Windows.Controls;

namespace Kinemat.Windows.Controls.Book
{
  internal class PageTurner
  {
    private RadBook book;
    private Point mouseEnd;
    private Point mouseStart;
    private Point currentMousePossition;
    private DateTime beginTime;
    private DateTime endTime;
    private IEasingFunction easingFunction;
    private DispatcherTimer timer;
    private bool attachHandlerRequired;
    private EventHandler<PageTurnEventArgs> pageTurned;
    private EventHandler<PageTurnEventArgs> pageTurning;
    private EventHandler<IsAvailableChangedEventArgs> isAvailableChanged;

    public bool IsAvailable { get; set; }

    public Point CurrentMousePosition
    {
      get
      {
        return this.currentMousePossition;
      }
      set
      {
        this.currentMousePossition = value;
      }
    }

    internal event EventHandler<PageTurnEventArgs> PageTurned
    {
      add
      {
        EventHandler<PageTurnEventArgs> eventHandler = this.pageTurned;
        EventHandler<PageTurnEventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<PageTurnEventArgs>>(ref this.pageTurned, comparand + value, comparand);
        }
        while (eventHandler != comparand);
      }
      remove
      {
        EventHandler<PageTurnEventArgs> eventHandler = this.pageTurned;
        EventHandler<PageTurnEventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<PageTurnEventArgs>>(ref this.pageTurned, comparand - value, comparand);
        }
        while (eventHandler != comparand);
      }
    }

    internal event EventHandler<PageTurnEventArgs> PageTurning
    {
      add
      {
        EventHandler<PageTurnEventArgs> eventHandler = this.pageTurning;
        EventHandler<PageTurnEventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<PageTurnEventArgs>>(ref this.pageTurning, comparand + value, comparand);
        }
        while (eventHandler != comparand);
      }
      remove
      {
        EventHandler<PageTurnEventArgs> eventHandler = this.pageTurning;
        EventHandler<PageTurnEventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<PageTurnEventArgs>>(ref this.pageTurning, comparand - value, comparand);
        }
        while (eventHandler != comparand);
      }
    }

    internal event EventHandler<IsAvailableChangedEventArgs> IsAvailableChanged
    {
      add
      {
        EventHandler<IsAvailableChangedEventArgs> eventHandler = this.isAvailableChanged;
        EventHandler<IsAvailableChangedEventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<IsAvailableChangedEventArgs>>(ref this.isAvailableChanged, comparand + value, comparand);
        }
        while (eventHandler != comparand);
      }
      remove
      {
        EventHandler<IsAvailableChangedEventArgs> eventHandler = this.isAvailableChanged;
        EventHandler<IsAvailableChangedEventArgs> comparand;
        do
        {
          comparand = eventHandler;
          eventHandler = Interlocked.CompareExchange<EventHandler<IsAvailableChangedEventArgs>>(ref this.isAvailableChanged, comparand - value, comparand);
        }
        while (eventHandler != comparand);
      }
    }

    public PageTurner(RadBook book)
    {
      PageTurner pageTurner = this;
      CircleEase circleEase1 = new CircleEase();
      circleEase1.EasingMode = EasingMode.EaseOut;
      CircleEase circleEase2 = circleEase1;
      pageTurner.easingFunction = (IEasingFunction) circleEase2;
      this.timer = new DispatcherTimer()
      {
        Interval = TimeSpan.FromMilliseconds(1.0)
      };
      // ISSUE: explicit constructor call
      // base.\u002Ector();
      this.book = book;
      this.IsAvailable = true;
      this.timer.Tick += new EventHandler(this.Timer_Tick);
    }

    public PageTurner(RadBook book, IEasingFunction easingFunction)
      : this(book)
    {
      this.easingFunction = easingFunction;
    }

    public void Stop()
    {
      this.timer.Tick -= new EventHandler(this.Timer_Tick);
      this.IsAvailable = true;
      this.endTime = DateTime.Now;
      this.attachHandlerRequired = true;
      this.timer.Stop();
    }

    public Point GetCornerPoint(FoldPosition corner, RelativeTo relativeTo)
    {
      Point point;
      switch (corner)
      {
        case FoldPosition.TopLeft:
          point = relativeTo == RelativeTo.Book ? new Point(-1.0 * (this.book.ActualWidth / 2.0), 0.0) : new Point(0.0, 0.0);
          break;
        case FoldPosition.TopRight:
          point = relativeTo == RelativeTo.Book ? new Point(this.book.ActualWidth, 0.0) : new Point(this.book.ActualWidth / 2.0, 0.0);
          break;
        case FoldPosition.BottomRight:
          point = relativeTo == RelativeTo.Book ? new Point(this.book.ActualWidth, this.book.ActualHeight) : new Point(this.book.ActualWidth / 2.0, this.book.ActualHeight);
          break;
        case FoldPosition.BottomLeft:
          point = relativeTo == RelativeTo.Book ? new Point(-1.0 * (this.book.ActualWidth / 2.0), this.book.ActualHeight) : new Point(0.0, this.book.ActualHeight);
          break;
        default:
          point = new Point();
          break;
      }
      return point;
    }

    public void TurnPage(Point from, Point to)
    {
      this.mouseStart = from;
      this.mouseEnd = to;
      this.beginTime = DateTime.Now;
      this.endTime = this.beginTime + this.book.FlipDuration;
      this.IsAvailable = false;
      if (this.attachHandlerRequired)
      {
        this.timer.Tick += new EventHandler(this.Timer_Tick);
        this.attachHandlerRequired = false;
      }
      this.timer.Start();
    }

    public void TurnPage(FoldPosition from, FoldPosition to, RelativeTo relativeTo)
    {
      this.TurnPage(this.GetCornerPoint(from, relativeTo), this.GetCornerPoint(to, relativeTo));
    }

    public void TurnPage(FoldPosition from, FoldPosition to, RelativeTo cornerFromRelativeness, RelativeTo cornerToRelativeness)
    {
      this.TurnPage(this.GetCornerPoint(from, cornerFromRelativeness), this.GetCornerPoint(to, cornerToRelativeness));
    }

    public void TurnPage(Point from, FoldPosition to, RelativeTo relativeTo)
    {
      Point cornerPoint = this.GetCornerPoint(to, relativeTo);
      this.TurnPage(from, cornerPoint);
    }

    public void TurnPage(FoldPosition from, Point to, RelativeTo relativeTo)
    {
      this.TurnPage(this.GetCornerPoint(from, relativeTo), to);
    }

    public void TurnPage(Point to)
    {
      this.TurnPage(this.currentMousePossition, to);
    }

    public void TurnPage(FoldPosition to, RelativeTo relativeTo)
    {
      this.TurnPage(this.GetCornerPoint(to, relativeTo));
    }

    private static double Clamp(double number, double min, double max)
    {
      number = number > max ? max : number;
      number = number < min ? min : number;
      return number;
    }

    private static Point GetDelta(Point start, Point end)
    {
      return new Point(end.X - start.X, end.Y - start.Y);
    }

    private double GetNormalizedTime()
    {
      double num1 = (double) this.beginTime.Ticks;
      double num2 = (double) DateTime.Now.Ticks;
      double num3 = (double) this.endTime.Ticks;
      return PageTurner.Clamp((num2 - num1) / (num3 - num1), 0.0, 1.0);
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      double num = this.easingFunction.Ease(this.GetNormalizedTime());
      Point delta = PageTurner.GetDelta(this.mouseStart, this.mouseEnd);
      this.currentMousePossition.X = this.mouseStart.X + delta.X * num;
      this.currentMousePossition.Y = this.mouseStart.Y + delta.Y * num;
      if (this.pageTurning != null)
        this.pageTurning((object) this, new PageTurnEventArgs(this.currentMousePossition));
      if (this.endTime.Subtract(DateTime.Now).TotalMilliseconds < 0.0)
      {
        this.timer.Stop();
        this.IsAvailable = true;
        if (this.pageTurned != null)
          this.pageTurned((object) this, new PageTurnEventArgs(this.currentMousePossition));
        if (this.isAvailableChanged == null)
          return;
        this.isAvailableChanged((object) this, new IsAvailableChangedEventArgs(true));
      }
      else
        this.timer.Start();
    }
  }
}
