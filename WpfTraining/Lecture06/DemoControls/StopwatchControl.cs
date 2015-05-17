using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace DemoControls
{
    [StyleTypedPropertyAttribute(Property = "ButtonStyle", StyleTargetType = typeof(ButtonBase))]
    public class StopwatchControl : Control
    {
        public static readonly DependencyProperty ElapsedProperty = DependencyProperty.Register(
            "Elapsed", typeof(TimeSpan), typeof(StopwatchControl), new PropertyMetadata(TimeSpan.Zero));

        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register(
            "IsRunning", typeof(bool), typeof(StopwatchControl),
            new PropertyMetadata(false, OnIsRunningChanged));

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(
            "ButtonStyle", typeof(Style), typeof(StopwatchControl), new PropertyMetadata(null));

        public static readonly RoutedCommand ResetCommand = new RoutedCommand("Reset", typeof(StopwatchControl));

        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly DispatcherTimer timer;

        public StopwatchControl()
        {
            timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(50)};
            timer.Tick += OnTimerTick;
        }

        static StopwatchControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(StopwatchControl), 
                new FrameworkPropertyMetadata(typeof(StopwatchControl)));

            CommandManager.RegisterClassCommandBinding(
                typeof(StopwatchControl),
                new CommandBinding(ResetCommand, OnResetCommandExecuted));
        }

        public TimeSpan Elapsed
        {
            get { return (TimeSpan)GetValue(ElapsedProperty); }
            set { SetValue(ElapsedProperty, value); }
        }

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        
        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Reset()
        {
            IsRunning = false;
            stopwatch.Reset();
            Elapsed = stopwatch.Elapsed;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Elapsed = stopwatch.Elapsed;
        }

        private static void OnResetCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((StopwatchControl)sender).Reset();
        }

        private static void OnIsRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StopwatchControl)d).OnIsRunningChanged(e);
        }

        protected virtual void OnIsRunningChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsRunning)
            {
                stopwatch.Start();
                timer.Start();
                Elapsed = stopwatch.Elapsed;
            }
            else
            {
                timer.Stop();
                stopwatch.Stop();
                Elapsed = stopwatch.Elapsed;
            }
        }
    }
}
