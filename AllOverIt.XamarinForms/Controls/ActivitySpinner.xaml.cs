using AllOverIt.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllOverIt.XamarinForms.Controls
{
  // Inspired by https://progrunning.net/creating-custom-xamarin-forms-loading-indicator-with-animations/
  //
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ActivitySpinner
  {
    private static readonly SemaphoreSlim AnimationSemaphore = new SemaphoreSlim(1);
    private const uint DefaultFadeDuration = 400;

    public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(ActivitySpinner), default(bool), propertyChanged: IsLoadingPropertyChanged);

    public bool IsLoading
    {
      get => (bool)GetValue(IsLoadingProperty);
      set => SetValue(IsLoadingProperty, value);
    }

    public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(nameof(IndicatorColor), typeof(Color), typeof(ActivitySpinner), default(Color));

    public Color IndicatorColor
    {
      get => (Color)GetValue(IndicatorColorProperty);
      set => SetValue(IndicatorColorProperty, value);
    }
    
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ActivitySpinner), default(string), propertyChanged: TextPropertyChanged);

    public string Text
    {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ActivitySpinner), default(Color));

    public Color TextColor
    {
      get => (Color)GetValue(TextColorProperty);
      set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty TextFontFamilyProperty = BindableProperty.Create(nameof(TextFontFamily), typeof(string), typeof(ActivitySpinner), default(string));

    public string TextFontFamily
    {
      get => (string)GetValue(TextFontFamilyProperty);
      set => SetValue(TextFontFamilyProperty, value);
    }

    public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(ActivitySpinner), default(double));

    public double TextFontSize
    {
      get => (double)GetValue(TextFontSizeProperty);
      set => SetValue(TextFontSizeProperty, value);
    }
    
    public static readonly BindableProperty TextFontAttributesProperty = BindableProperty.Create(nameof(TextFontAttributes), typeof(FontAttributes), typeof(ActivitySpinner), default(FontAttributes));

    public FontAttributes TextFontAttributes
    {
      get => (FontAttributes)GetValue(TextFontAttributesProperty);
      set => SetValue(TextFontAttributesProperty, value);
    }
    
    public static readonly BindableProperty FadeDurationProperty = BindableProperty.Create(nameof(FadeDuration), typeof(uint), typeof(ActivitySpinner), DefaultFadeDuration);

    public uint FadeDuration
    {
      get => (uint)GetValue(FadeDurationProperty);
      set => SetValue(FadeDurationProperty, value);
    }

    public ActivitySpinner()
    {
      InitializeComponent();

      SetActivityBindings();
      SetLabelBindings();
    }

    private void SetActivityBindings()
    {
      // bind properties from this control to the ActivityIndicator
      Spinner.BindingContext = this;
      Spinner.SetBinding(ActivityIndicator.ColorProperty, nameof(IndicatorColor));
    }

    private void SetLabelBindings()
    {
      // bind properties from this control to the Label
      SpinnerText.BindingContext = this;
      SpinnerText.SetBinding(Label.TextProperty, nameof(Text));
      SpinnerText.SetBinding(Label.TextColorProperty, nameof(TextColor));
      SpinnerText.SetBinding(Label.FontSizeProperty, nameof(TextFontSize));
      SpinnerText.SetBinding(Label.FontFamilyProperty, nameof(TextFontFamily));
      SpinnerText.SetBinding(Label.FontAttributesProperty, nameof(TextFontAttributes));
    }

    private static async void IsLoadingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var spinner = (bindable as ActivitySpinner).WhenNotNull(nameof(bindable));

      await SetLoadingVisibility(spinner);
    }

    private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      ControlHelpers.SetProperty<ActivitySpinner, string>(bindable, newValue,
        (activitySpinner, propertyValue) =>
        {
          activitySpinner.SpinnerText.Text = propertyValue;
          activitySpinner.SpinnerText.IsVisible = !string.IsNullOrWhiteSpace(propertyValue);
        });
    }

    private static async Task SetLoadingVisibility(ActivitySpinner activitySpinner)
    {
      try
      {
        ViewExtensions.CancelAnimations(activitySpinner);

        await AnimationSemaphore.WaitAsync();

        if (activitySpinner.IsLoading)
        {
          activitySpinner.Spinner.IsRunning = true;
          activitySpinner.IsVisible = true;

          await activitySpinner.FadeTo(1.0d, activitySpinner.FadeDuration, Easing.Linear);
        }
        else
        {
          await activitySpinner.FadeTo(0.0d, activitySpinner.FadeDuration, Easing.Linear);

          activitySpinner.IsVisible = false;
          activitySpinner.Spinner.IsRunning = false;
        }
      }
      finally
      {
        AnimationSemaphore.Release();
      }
    }
  }
}