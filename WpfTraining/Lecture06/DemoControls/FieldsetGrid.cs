using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DemoControls
{
    public class FieldsetGrid : Panel
    {
        private const int ColumnCount = 2;

        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }

        public static readonly DependencyProperty HorizontalSpacingProperty = DependencyProperty.Register(
            "HorizontalSpacing", typeof(double), typeof(FieldsetGrid), new FrameworkPropertyMetadata(0d, 
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => (double)value >= 0);



        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }

        public static readonly DependencyProperty VerticalSpacingProperty = DependencyProperty.Register(
            "VerticalSpacing", typeof(double), typeof(FieldsetGrid), new FrameworkPropertyMetadata(0d,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => (double)value >= 0);



        static FieldsetGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FieldsetGrid),
                new FrameworkPropertyMetadata(typeof(FieldsetGrid)));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width = 0;
            double height = 0;

            // Measure each child.
            // The width of each column is defined by its widest element.
            // The height of each row is defined by its tallest element.
            int totalRowCount = (InternalChildren.Count + ColumnCount - 1) / ColumnCount;
            var columnWidths = new double[ColumnCount];
            for (int row = 0; row < totalRowCount; row++)
            {
                double rowHeight = 0;
                for (int column = 0; column < ColumnCount; column++)
                {
                    int index = row * ColumnCount + column;
                    if (index >= InternalChildren.Count)
                        break;

                    UIElement child = InternalChildren[index];
                    child.Measure(availableSize);
                    columnWidths[column] = Math.Max(columnWidths[column], child.DesiredSize.Width);
                    rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);
                }
                height += rowHeight;
            }

            width = columnWidths.Sum() + HorizontalSpacing * (ColumnCount - 1);
            height += VerticalSpacing * (totalRowCount - 1);
            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int totalRowCount = (InternalChildren.Count + ColumnCount - 1) / ColumnCount;

            // Calculate the size of each cell.
            
            // The height of each row is defined by its tallest element.
            // The size of the first column is defined by its widest element.
            var rowHeights = new double[totalRowCount];
            var columnWidths = new double[ColumnCount];
            for (int row = 0; row < totalRowCount; row++)
            {
                double rowHeight = 0;
                for (int column = 0; column < ColumnCount; column++)
                {
                    int index = row * ColumnCount + column;
                    if (index >= InternalChildren.Count)
                        break;

                    UIElement child = InternalChildren[index];
                    if (column == 0)
                    {
                        columnWidths[column] = Math.Max(columnWidths[column], child.DesiredSize.Width);
                    }
                    rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);
                }
                rowHeights[row] = rowHeight;
            }
            // The other columns share the remaining width in equal proportions.
            double remainingWidth = (finalSize.Width - columnWidths[0] - HorizontalSpacing * (ColumnCount - 1));
            double otherColumnWidth = remainingWidth / (ColumnCount - 1);
            for (int i = 1; i < columnWidths.Length; i++)
            {
                columnWidths[i] = otherColumnWidth;
            }

            // Perform arrange.
            double y = 0;
            for (int row = 0; row < totalRowCount; row++)
            {
                double x = 0;
                double cellHeight = rowHeights[row];
                for (int column = 0; column < ColumnCount; column++)
                {
                    int index = row * ColumnCount + column;
                    if (index >= InternalChildren.Count)
                        break;

                    UIElement child = InternalChildren[index];
                    double cellWidth = columnWidths[column];
                    Rect cellRect = new Rect(x, y, cellWidth, cellHeight);
                    child.Arrange(cellRect);
                    x += HorizontalSpacing + cellWidth;
                }
                y += VerticalSpacing + cellHeight;
            }

            var height = rowHeights.Sum() + (totalRowCount - 1) * VerticalSpacing;
            if (VerticalAlignment == VerticalAlignment.Stretch)
            {
                return new Size(
                    finalSize.Width,
                    Math.Max(height, finalSize.Height));
            }
            else
            {
                return new Size(finalSize.Width, height);
            }
        }
    }
}
