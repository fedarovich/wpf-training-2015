using MvvmLightDemo.ViewModel;

namespace MvvmLightDemo.Messages
{
    public class DocumentClosedMessage
    {
        private readonly DocumentViewModel document;

        public DocumentClosedMessage(DocumentViewModel document)
        {
            this.document = document;
        }

        public DocumentViewModel Document
        {
            get { return document; }
        }
    }
}
