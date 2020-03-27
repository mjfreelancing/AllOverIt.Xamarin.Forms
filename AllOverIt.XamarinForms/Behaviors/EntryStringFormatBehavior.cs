using AllOverIt.XamarinForms.Behaviors.Base;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public class EntryStringFormatBehavior : AttachableBehavior<EntryStringFormatBehavior, Entry>
  {
    public static readonly BindableProperty PathProperty = BindableProperty.Create(nameof(Path), typeof(string), typeof(EntryStringFormatBehavior));
    public string Path
    {
      get => (string)GetValue(PathProperty);
      set => SetValue(PathProperty, value);
    }

    public static readonly BindableProperty FocusedFormatProperty = BindableProperty.Create(nameof(FocusedFormat), typeof(string), typeof(EntryStringFormatBehavior));

    public string FocusedFormat
    {
      get => (string)GetValue(FocusedFormatProperty);
      set => SetValue(FocusedFormatProperty, value);
    }

    public static readonly BindableProperty UnfocusedFormatProperty = BindableProperty.Create(nameof(UnfocusedFormat), typeof(string), typeof(EntryStringFormatBehavior));

    public string UnfocusedFormat
    {
      get => (string)GetValue(UnfocusedFormatProperty);
      set => SetValue(UnfocusedFormatProperty, value);
    }

    protected override void OnAttachedTo(BindableObject bindable)
    {
      base.OnAttachedTo(bindable);

      AssociatedObject.Focused += OnFocus;
      AssociatedObject.Unfocused += OnUnfocus;

      SetFocusStringFormat(AssociatedObject);
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
      AssociatedObject.Focused -= OnFocus;
      AssociatedObject.Unfocused -= OnUnfocus;

      base.OnDetachingFrom(bindable);
    }

    private void OnFocus(object sender, FocusEventArgs args)
    {
      SetFocusStringFormat(AssociatedObject);
    }

    private void OnUnfocus(object sender, FocusEventArgs args)
    {
      SetFocusStringFormat(AssociatedObject);
    }

    private void SetFocusStringFormat(VisualElement entry)
    {
      var format = AssociatedObject.IsFocused ? FocusedFormat : UnfocusedFormat;
      entry.SetBinding(Entry.TextProperty, Path, stringFormat: format);
    }
  }
}