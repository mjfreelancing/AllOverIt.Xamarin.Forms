using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AllOverIt.XamarinForms.Tests.Scaffolding
{
  internal class MockResourcesProvider : ISystemResourcesProvider
  {
    public IResourceDictionary GetSystemResources()
    {
      var dictionary = new ResourceDictionary();

      AddStyle<Label>(dictionary, Device.Styles.BodyStyleKey);
      AddStyle<Label>(dictionary, Device.Styles.TitleStyleKey, setters => setters.Add(Label.FontSizeProperty, 50));
      AddStyle<Label>(dictionary, Device.Styles.SubtitleStyleKey, setters => setters.Add(Label.FontSizeProperty, 40));
      AddStyle<Label>(dictionary, Device.Styles.CaptionStyleKey, setters => setters.Add(Label.FontSizeProperty, 30));
      AddStyle<Label>(dictionary, Device.Styles.ListItemTextStyleKey, setters => setters.Add(Label.FontSizeProperty, 20));
      AddStyle<Label>(dictionary, Device.Styles.ListItemDetailTextStyleKey, setters => setters.Add(Label.FontSizeProperty, 10));

      return dictionary;
    }

    private static void AddStyle<TType>(ResourceDictionary dictionary, string deviceStyle, Action<IList<Setter>> styleSetter = null)
    {
      var style = new Style(typeof(TType));
      styleSetter?.Invoke(style.Setters);

      dictionary[deviceStyle] = style;
    }
  }
}