using AllOverIt.XamarinForms.Behaviors.Base;
using System.Linq;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public class NumericValidationBehavior : AttachableBehavior<NumericValidationBehavior, Entry>
  {
    public static readonly BindableProperty ValidTextColorProperty = BindableProperty.Create(nameof(ValidTextColor), typeof(Color), typeof(NumericValidationBehavior), Color.Default);

    public Color ValidTextColor
    {
      get => (Color)GetValue(ValidTextColorProperty);
      set => SetValue(ValidTextColorProperty, value);
    }

    public static readonly BindableProperty ValidBackgroundColorProperty = BindableProperty.Create(nameof(ValidBackgroundColor), typeof(Color), typeof(NumericValidationBehavior), Color.Default);

    public Color ValidBackgroundColor
    {
      get => (Color)GetValue(ValidBackgroundColorProperty);
      set => SetValue(ValidBackgroundColorProperty, value);
    }

    // invalid colors only applied if the SanitizeInput property is false
    public static readonly BindableProperty InvalidTextColorProperty = BindableProperty.Create(nameof(InvalidTextColor), typeof(Color), typeof(NumericValidationBehavior), Color.Red);

    public Color InvalidTextColor
    {
      get => (Color)GetValue(InvalidTextColorProperty);
      set => SetValue(InvalidTextColorProperty, value);
    }

    public static readonly BindableProperty InvalidBackgroundColorProperty = BindableProperty.Create(nameof(InvalidBackgroundColor), typeof(Color), typeof(NumericValidationBehavior), Color.Default);

    public Color InvalidBackgroundColor
    {
      get => (Color)GetValue(InvalidBackgroundColorProperty);
      set => SetValue(InvalidBackgroundColorProperty, value);
    }

    // determines if the decimal (period) character input is allowed
    public static readonly BindableProperty AllowDecimalProperty = BindableProperty.Create(nameof(AllowDecimal), typeof(bool), typeof(NumericValidationBehavior), true);

    public bool AllowDecimal
    {
      get => (bool)GetValue(AllowDecimalProperty);
      set => SetValue(AllowDecimalProperty, value);
    }

    // determines if the negative (dash) character input is allowed
    public static readonly BindableProperty AllowNegativeProperty = BindableProperty.Create(nameof(AllowNegative), typeof(bool), typeof(NumericValidationBehavior), true);

    public bool AllowNegative
    {
      get => (bool)GetValue(AllowNegativeProperty);
      set => SetValue(AllowNegativeProperty, value);
    }

    // determines if the input should be sanitized if non-allowed characters are input, or if they are simply 'invalid' and hence the appearance is changed
    public static readonly BindableProperty SanitizeInputProperty = BindableProperty.Create(nameof(SanitizeInput), typeof(bool), typeof(NumericValidationBehavior), false);

    public bool SanitizeInput
    {
      get => (bool)GetValue(SanitizeInputProperty);
      set => SetValue(SanitizeInputProperty, value);
    }

    protected override void OnAttachedTo(BindableObject entry)
    {
      base.OnAttachedTo(entry);

      AssociatedObject.TextChanged += OnEntryTextChanged;

      // can't validate now because we don't have old/new values
    }

    protected override void OnDetachingFrom(BindableObject entry)
    {
      AssociatedObject.TextChanged -= OnEntryTextChanged;

      base.OnDetachingFrom(entry);
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
    {
      // only validate during data entry - in case the non-focused text is formatted
      if (!AssociatedObject.IsFocused || string.IsNullOrWhiteSpace(args.NewTextValue))
      {
        return;
      }

      var isValid = double.TryParse(args.NewTextValue, out _);

      if (isValid && !AllowDecimal)
      {
        // checking 'NewTexValue' rather than the converted value because 123. will evaluate to 123
        isValid = !args.NewTextValue.Contains('.');
      }

      if (isValid && !AllowNegative)
      {
        // checking 'NewTexValue' rather than the converted value because 123. will evaluate to 123
        isValid = !args.NewTextValue.Contains('-');
      }

      if (!isValid && SanitizeInput)
      {
        AssociatedObject.Text = args.OldTextValue;
      }

      AssociatedObject.TextColor = isValid ? ValidTextColor : InvalidTextColor;
      AssociatedObject.BackgroundColor = isValid ? ValidBackgroundColor : InvalidBackgroundColor;
    }
  }
}