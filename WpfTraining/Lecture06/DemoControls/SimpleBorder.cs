using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DemoControls
{
    public class SimpleBorder : Decorator
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Brush), typeof(SimpleBorder), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush", typeof(Brush), typeof(SimpleBorder),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            "BorderThickness", typeof(double), typeof(SimpleBorder),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender | 
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => (double)value >= 0d);

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        static SimpleBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SimpleBorder),
                new FrameworkPropertyMetadata(typeof(SimpleBorder)));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var totalBorder = 2 * BorderThickness;
            if (Child != null)
            {
                var size = new Size(
                    Math.Max(0, constraint.Width - totalBorder),
                    Math.Max(0, constraint.Height - totalBorder));

                Child.Measure(size);
                return new Size(
                    Child.DesiredSize.Width + totalBorder,
                    Child.DesiredSize.Height + totalBorder);
            }
            else
            {
                return new Size(totalBorder, totalBorder);
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (Child != null)
            {
                var totalBorder = 2 * BorderThickness;
                var childRect = new Rect(
                    BorderThickness,
                    BorderThickness,
                    Math.Max(0, arrangeSize.Width - totalBorder),
                    Math.Max(0, arrangeSize.Height - totalBorder));
                Child.Arrange(childRect);
            }

            return arrangeSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = (BorderThickness > 0 && BorderBrush != null)
                ? new Pen(BorderBrush, BorderThickness)
                : null;
            double halfBorder = BorderThickness / 2;
            var rect = new Rect(
                new Point(halfBorder, halfBorder), 
                new Point(RenderSize.Width - halfBorder, RenderSize.Height - halfBorder));
            drawingContext.DrawRectangle(Background, pen, rect);

            base.OnRender(drawingContext);
        }
    }
}
