using AllOverIt.Helpers;
using AllOverIt.XamarinForms.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllOverIt.XamarinForms.Controls
{
  // Inspired by https://progrunning.net/creating-custom-xamarin-forms-loading-indicator-with-animations/

  /// <summary>
  /// An activity indicator with a label.
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ActivitySpinner
  {
    private static readonly SemaphoreSlim AnimationSemaphore = new SemaphoreSlim(1);
    private const uint DefaultFadeDuration = 400;

    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(ActivitySpinner), default(bool), propertyChanged: IsBusyPropertyChanged);

    /// <summary>
    /// The activity indicator and label are visible when indicating a busy state.
    /// </summary>
    public bool IsBusy
    {
      get => (bool)GetValue(IsBusyProperty);
      set => SetValue(IsBusyProperty, value);
    }

    public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(nameof(IndicatorColor), typeof(Color), typeof(ActivitySpinner), default(Color));

    /// <summary>
    /// The color of the activity indicator.
    /// </summary>
    public Color IndicatorColor
    {
      get => (Color)GetValue(IndicatorColorProperty);
      set => SetValue(IndicatorColorProperty, value);
    }
    
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ActivitySpinner), default(string), propertyChanged: TextPropertyChanged);

    /// <summary>
    /// The label text.
    /// </summary>
    public string Text
    {
      get => (string)GetValue(TextProperty);
      set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ActivitySpinner), default(Color));

    /// <summary>
    /// The label text color.
    /// </summary>
    public Color TextColor
    {
      get => (Color)GetValue(TextColorProperty);
      set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty TextFontFamilyProperty = BindableProperty.Create(nameof(TextFontFamily), typeof(string), typeof(ActivitySpinner), default(string));

    /// <summary>
    /// The label text font family.
    /// </summary>
    public string TextFontFamily
    {
      get => (string)GetValue(TextFontFamilyProperty);
      set => SetValue(TextFontFamilyProperty, value);
    }

    public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(ActivitySpinner), default(double));

    /// <summary>
    /// The label text font size.
    /// </summary>
    public double TextFontSize
    {
      get => (double)GetValue(TextFontSizeProperty);
      set => SetValue(TextFontSizeProperty, value);
    }
    
    public static readonly BindableProperty TextFontAttributesProperty = BindableProperty.Create(nameof(TextFontAttributes), typeof(FontAttributes), typeof(ActivitySpinner), default(FontAttributes));

    /// <summary>
    /// The label text font attributes.
    /// </summary>
    public FontAttributes TextFontAttributes
    {
      get => (FontAttributes)GetValue(TextFontAttributesProperty);
      set => SetValue(TextFontAttributesProperty, value);
    }
    
    public static readonly BindableProperty FadeDurationProperty = BindableProperty.Create(nameof(FadeDuration), typeof(uint), typeof(ActivitySpinner), DefaultFadeDuration);

    /// <summary>
    /// The fade duration applied when toggling the control's visibility.
    /// </summary>
    public uint FadeDuration
    {
      get => (uint)GetValue(FadeDurationProperty);
      set => SetValue(FadeDurationProperty, value);
    }

    /// <summary>
    /// Initializes the control.
    /// </summary>
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

    private static async void IsBusyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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

        if (activitySpinner.IsBusy)
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