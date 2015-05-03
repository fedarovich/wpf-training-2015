namespace ImplicitDataTemplates
{
    public class CenteredItem
    {
        private static int counter;

        public CenteredItem()
        {
            Index = ++counter;
        }

        public int Index { get; private set; }
    }
}
