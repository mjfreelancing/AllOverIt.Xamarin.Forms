using AllOverIt.Extensions;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Tests
{
  internal static class EntryUtilities
  {
    public static void ConfigureEntryFocus(Entry entry)
    {
      entry.FocusChangeRequested += (sender, args) =>
      {
        var setValueMethod = typeof(BindableObject).GetMethodInfo(
          "SetValue",
          new[] { typeof(BindableProperty), typeof(object), typeof(bool), typeof(bool) });

        setValueMethod.Invoke(entry, new object[] { VisualElement.IsFocusedProperty, args.Focus, false, false });

        args.Result = true;
      };
    }
  }
}