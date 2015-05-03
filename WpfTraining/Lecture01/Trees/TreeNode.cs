using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Trees
{
    public abstract class TreeNode : INotifyPropertyChanged
    {
        private bool isExpanded;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value.Equals(isExpanded)) return;
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        public abstract IEnumerable<TreeNode> Children { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
