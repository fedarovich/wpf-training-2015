using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;

// The ProvideMetadata assembly-level attribute indicates to designers
// that this assembly contains a class that provides an attribute table. 
[assembly: ProvideMetadata(typeof(DemoControls.Design.Metadata))]

namespace DemoControls.Design
{
    internal class Metadata : IProvideAttributeTable
    {
        public AttributeTable AttributeTable
        {
            get
            {
                var builder = new AttributeTableBuilder();

                builder.AddCustomAttributes(
                    typeof(TextView),
                    new ToolboxBrowsableAttribute(true));
                
                builder.AddCustomAttributes(
                    typeof(TextView),
                    "Text",
                    PropertyValueEditor.CreateEditorAttribute(typeof(TextEditor)));

                return builder.CreateTable();
            }
        }
    }
}
