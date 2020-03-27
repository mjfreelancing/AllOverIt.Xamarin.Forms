using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllOverIt.XamarinForms.MarkupExtensions
{
  /// <summary>
  /// Obtains an image resource from the calling assembly based on a string Source identifier.
  /// </summary>
  [ContentProperty(nameof(Source))]
  public class ImageSourceExtension : IMarkupExtension
  {
    /// <summary>
    /// The name of the image source to be retrieved.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// A type from the assembly in which to look up the image resource.
    /// </summary>
    public Type? ResolvingType { get; set; }

    /// <summary>
    /// Returns the object created from the markup extension.
    /// </summary>
    public object? ProvideValue(IServiceProvider serviceProvider)
    {
      return string.IsNullOrEmpty(Source)
        ? null
        : ImageSource.FromResource(Source, ResolvingType);
    }
  }
}