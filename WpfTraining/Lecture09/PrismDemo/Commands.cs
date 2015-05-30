using Microsoft.Practices.Prism.Commands;

namespace PrismDemo
{
    public static class Commands
    {
        public static readonly CompositeCommand CloseCommand = new CompositeCommand();

        public static readonly CompositeCommand SaveCommand = new CompositeCommand(true);

        public static readonly CompositeCommand SaveAsCommand = new CompositeCommand(true);
    }
}
