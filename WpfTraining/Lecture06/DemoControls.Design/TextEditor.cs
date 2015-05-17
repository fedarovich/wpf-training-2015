using System.Windows;
using Microsoft.Windows.Design.PropertyEditing;

namespace DemoControls.Design
{
    public class TextEditor : DialogPropertyValueEditor
    {
        private readonly EditorResources editorResources = new EditorResources();

        public TextEditor()
        {
            InlineEditorTemplate = editorResources["TextInlineEditorTemplate"] as DataTemplate;
        }

        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            var dialog = new TextEditorDialog {Text = propertyValue.StringValue};
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                propertyValue.StringValue = dialog.Text;
            }
        }
    }
}
