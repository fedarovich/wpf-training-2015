using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace CustomMarkupExtensions
{
    /// <summary>
    /// Creates binding to the view model. The view model must be set as the DataContext of the view.
    /// </summary>
    public class ViewModelBindingExtension : MarkupExtension
    {
        #region property getters and setters

        /// <summary>
        /// Gets or sets the binding path.
        /// </summary>
        [ConstructorArgument("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the binding mode.
        /// Default value is <see cref="System.Windows.Data.BindingMode.Default"/>.
        /// </summary>
        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the binding update source trigger.
        /// Default value is <see cref="System.Windows.Data.UpdateSourceTrigger.Default"/>.
        /// </summary>
        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to raise the <see cref="Binding.SourceUpdatedEvent"/>
        /// when a value is transferred from the binding target to the binding source.
        /// Default value is <c>False</c>.
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to raise the <see cref="Binding.TargetUpdatedEvent"/>
        /// when a value is transferred from the binding target to the binding source.
        /// Default value is <c>False</c>.
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to raise the <see cref="Validation.ErrorEvent"/>
        /// attached event on the bound object.
        /// Default value is <c>False</c>.
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnValidationError { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to include the <see cref="DataErrorValidationRule"/>.
        /// Default value is <c>False</c>.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnDataErrors { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to include the <see cref="ExceptionValidationRule"/>.
        /// Default value is <c>False</c>.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnExceptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is async.
        /// Default value is <c>False</c>.
        /// </summary>
        [DefaultValue(false)]
        public bool IsAsync { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="BindingGroup"/> to which this binding belongs.
        /// Default value is empty string.
        /// </summary>
        [DefaultValue("")]
        public string BindingGroupName { get; set; }

        /// <summary>
        /// Gets or sets the converter to use.
        /// Default value is <c>null</c>.
        /// </summary>
        [DefaultValue(null)]
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the culture to which to evaluate the converter.
        /// Default value is <c>CultureInfo.CurrentCulture</c>.
        /// </summary>
        public CultureInfo ConverterCulture { get; set; }

        /// <summary>
        /// Gets or sets the parameter to pass to converter.
        /// Default value is <c>null</c>.
        /// </summary>
        [DefaultValue(null)]
        public object ConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets a string that specifies how to format the binding if it displays the bound value as a string.
        /// Default value is <c>null</c>.
        /// </summary>
        [DefaultValue(null)]
        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or sets the value to use when the binding is unable to return a value.
        /// Default value is <see cref="DependencyProperty.UnsetValue"/>.
        /// </summary>
        public object FallbackValue { get; set; }

        public RelativeSource RelativeSource { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBindingExtension"/> class.
        /// </summary>
        public ViewModelBindingExtension()
        {
            FallbackValue = DependencyProperty.UnsetValue;
            BindingGroupName = "";
            UpdateSourceTrigger = UpdateSourceTrigger.Default;
            Mode = BindingMode.Default;
            ConverterCulture = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBindingExtension"/> class.
        /// </summary>
        /// <param name="path">The binding path.</param>
        public ViewModelBindingExtension(string path)
            : this()
        {
            this.Path = path;
        }

        #endregion

        /// <summary>
        /// When implemented in a derived class, returns an object that is set as the value of the target property 
        /// for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var rootProvider = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));
            object rootObject = rootProvider.RootObject;
            if (rootObject == null)
                throw new InvalidOperationException("Root object is null.");

            bool isDesignMode = (Application.Current == null) || (Application.Current.MainWindow == null);
            if (!isDesignMode)
            {
                var element = rootObject as FrameworkElement;
                if (element == null)
                    throw new InvalidOperationException(
                        string.Format("Root object's '{0}' type is not of type FrameworkElement.",
                        rootObject.GetType()));
            }

            return ProvideValueHelper(rootObject).ProvideValue(serviceProvider);
        }

        /// <summary>
        /// Helper method for provide value to create binding object.
        /// </summary>
        /// <param name="rootObject">The root object.</param>
        /// <returns>The binding object. </returns>
        private Binding ProvideValueHelper(object rootObject)
        {
            string bindingPath = string.IsNullOrEmpty(this.Path) ? "DataContext" : "DataContext." + this.Path;
            return new Binding()
            {
                Path = new PropertyPath(bindingPath),
                Mode = this.Mode,
                UpdateSourceTrigger = this.UpdateSourceTrigger,
                NotifyOnSourceUpdated = this.NotifyOnSourceUpdated,
                NotifyOnTargetUpdated = this.NotifyOnTargetUpdated,
                NotifyOnValidationError = this.NotifyOnValidationError,
                ValidatesOnDataErrors = this.ValidatesOnDataErrors,
                ValidatesOnExceptions = this.ValidatesOnExceptions,
                IsAsync = this.IsAsync,
                BindingGroupName = this.BindingGroupName,
                Source = rootObject,
                Converter = this.Converter,
                ConverterCulture = this.ConverterCulture,
                ConverterParameter = this.ConverterParameter,
                StringFormat = this.StringFormat,
                FallbackValue = this.FallbackValue,
            };
        }
    }
}
