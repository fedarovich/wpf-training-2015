﻿using System.Windows;
using System.Windows.Media;

namespace DemoControls
{
    public class RenderedGrid : FrameworkElement
    {
        public static readonly DependencyProperty HorizontalIntervalProperty = DependencyProperty.Register(
            "HorizontalInterval", typeof(double), typeof(RenderedGrid), 
            new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.AffectsRender),
            value => (double)value >= 2);

        public static readonly DependencyProperty VerticalIntervalProperty = DependencyProperty.Register(
            "VerticalInterval", typeof(double), typeof(RenderedGrid),
            new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.AffectsRender),
            value => (double)value >= 2);

        public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register(
            "GridLineBrush", typeof(Brush), typeof(RenderedGrid),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public double HorizontalInterval
        {
            get { return (double)GetValue(HorizontalIntervalProperty); }
            set { SetValue(HorizontalIntervalProperty, value); }
        }

        public double VerticalInterval
        {
            get { return (double)GetValue(VerticalIntervalProperty); }
            set { SetValue(VerticalIntervalProperty, value); }
        }

        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        static RenderedGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RenderedGrid),
                new FrameworkPropertyMetadata(typeof(RenderedGrid)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // Calculate current DPI.
            Matrix m = PresentationSource.FromVisual(this)
                .CompositionTarget.TransformToDevice;
            double dpiFactor = 1 / m.M11;
            
            // Let the pen width be one PHYSICAL pixel
            var pen = new Pen(GridLineBrush, 1 * dpiFactor);

            // Prepare guidelines for lines
            #region guidelines

            double halfPenThickness = pen.Thickness / 2.0;
            var guidelines = new GuidelineSet();
            for (double x = 0; x < RenderSize.Width; x += HorizontalInterval)
            {
                guidelines.GuidelinesX.Add(x + halfPenThickness);
            }
            for (double y = 0; y < RenderSize.Height; y += VerticalInterval)
            {
                guidelines.GuidelinesY.Add(y + halfPenThickness);
            }
            drawingContext.PushGuidelineSet(guidelines);

            #endregion

            // Draw Lines
            for (double x = 0; x < RenderSize.Width; x += HorizontalInterval)
            {
                drawingContext.DrawLine(pen, new Point(x, 0), new Point(x, RenderSize.Height));
            }
            for (double y = 0; y < RenderSize.Height; y += VerticalInterval)
            {
                drawingContext.DrawLine(pen, new Point(0, y), new Point(RenderSize.Width, y));
            }
        }
    }
}
