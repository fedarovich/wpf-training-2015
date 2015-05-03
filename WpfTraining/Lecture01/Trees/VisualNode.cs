using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Trees
{
    public class VisualNode : TreeNode
    {
        private readonly DependencyObject element;

        public VisualNode(DependencyObject element)
        {
            this.element = element;
        }

        public string Name
        {
            get
            {
                string name = this.element.GetType().Name;
                var element = this.element as FrameworkElement;
                if (element != null && !string.IsNullOrEmpty(element.Name))
                {
                    name += " [Name = " + element.Name + "]";
                }
                return name;
            }
        }

        public override IEnumerable<TreeNode> Children
        {
            get
            {
                int count = VisualTreeHelper.GetChildrenCount(element);
                for (int i = 0; i < count; i++)
                {
                    yield return new VisualNode(VisualTreeHelper.GetChild(element, i));
                }
            }
        }

        public DependencyObject Element
        {
            get { return element; }
        }
    }
}
