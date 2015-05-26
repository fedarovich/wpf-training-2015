using DemoMvvmApp.ViewModels;

namespace DemoMvvmApp.Events
{
    public class DocumentClosedEvent
    {
        private readonly DocumentViewModel document;

        public DocumentClosedEvent(DocumentViewModel document)
        {
            this.document = document;
        }

        public DocumentViewModel Document
        {
            get { return document; }
        }
    }
}
