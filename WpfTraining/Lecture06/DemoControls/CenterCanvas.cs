using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DemoControls
{
    public class CenterCanvas : Panel
    {
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.RegisterAttached(
            "CenterX", typeof (double), typeof (CenterCanvas),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.RegisterAttached(
            "CenterY", typeof(double), typeof(CenterCanvas),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsArrange));

        [AttachedPropertyBrowsableForChildren]
        [Category("Layout")]
        public static double GetCenterX(UIElement element)
        {
            return (double)element.GetValue(CenterXProperty);
        }

        public static void SetCenterX(UIElement element, double value)
        {
            element.SetValue(CenterXProperty, value);
        }

        [AttachedPropertyBrowsableForChildren]
        [Category("Layout")]
        public static double GetCenterY(UIElement element)
        {
            return (double)element.GetValue(CenterYProperty);
        }

        public static void SetCenterY(UIElement element, double value)
        {
            element.SetValue(CenterYProperty, value);
        }

        static CenterCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CenterCanvas),
                new FrameworkPropertyMetadata(typeof(CenterCanvas)));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
            }

            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                var point = new Point(
                    GetCenterX(child) - child.DesiredSize.Width / 2,
                    GetCenterY(child) - child.DesiredSize.Height / 2);
                var rect = new Rect(point, child.DesiredSize);
                child.Arrange(rect);
            }

            return finalSize;
        }
    }
}
