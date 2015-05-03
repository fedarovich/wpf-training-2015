using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Trees
{
    public class LogicalNode : TreeNode
    {
        private readonly object element;

        public LogicalNode(object element)
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
                var dependencyObject = element as DependencyObject;
                return dependencyObject != null ?
                    LogicalTreeHelper.GetChildren(dependencyObject).Cast<object>().Select(x => new LogicalNode(x)) :
                    Enumerable.Empty<LogicalNode>();
            }
        }

        public object Element
        {
            get { return element; }
        }
    }
}
