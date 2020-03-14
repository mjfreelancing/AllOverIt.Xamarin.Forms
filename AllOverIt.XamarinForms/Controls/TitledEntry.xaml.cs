using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllOverIt.XamarinForms.Controls
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class TitledEntry : ContentView
  {
    public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(nameof(TitleText), typeof(string), typeof(TitledEntry), default(string), propertyChanged: OnTitleTextPropertyChanged);
    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(nameof(TitleTextColor), typeof(Color), typeof(TitledEntry), default(Color), propertyChanged: OnTitleTextColorPropertyChanged);
    public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleText), typeof(int), typeof(TitledEntry), default(int), propertyChanged: OnTitleFontSizePropertyChanged);
    public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.Create(nameof(TitleFontFamily), typeof(string), typeof(TitledEntry), default(string), propertyChanged: OnTitleFontFamilyPropertyChanged);
    public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes), typeof(TitledEntry), default(FontAttributes), propertyChanged: OnTitleFontAttributesPropertyChanged);

    // ?? forward properties of the same name as 'Entry' since this is the primary purpose of the control
    // ... but this has implications because 'IsEnabled' could be for the composite or the Entry, or both
    // go with new properties for now :-)
    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(TitledEntry), default(string), BindingMode.TwoWay, propertyChanged: OnEntryTextPropertyChanged);
    public static readonly BindableProperty EntryTextColorProperty = BindableProperty.Create(nameof(EntryTextColor), typeof(Color), typeof(TitledEntry), default(Color), propertyChanged: OnEntryTextColorPropertyChanged);
    public static readonly BindableProperty EntryFontSizeProperty = BindableProperty.Create(nameof(EntryText), typeof(int), typeof(TitledEntry), default(int), propertyChanged: OnEntryFontSizePropertyChanged);
    public static readonly BindableProperty EntryFontFamilyProperty = BindableProperty.Create(nameof(EntryFontFamily), typeof(string), typeof(TitledEntry), default(string), propertyChanged: OnEntryFontFamilyPropertyChanged);
    public static readonly BindableProperty EntryFontAttributesProperty = BindableProperty.Create(nameof(EntryFontAttributes), typeof(FontAttributes), typeof(TitledEntry), default(FontAttributes), propertyChanged: OnEntryFontAttributesPropertyChanged);
    public static readonly BindableProperty EntryIsEnabledProperty = BindableProperty.Create(nameof(EntryIsEnabled), typeof(bool), typeof(TitledEntry), true, BindingMode.TwoWay, propertyChanged: OnEntryIsEnabledPropertyChanged);

    #region Title properties

    public string TitleText
    {
      get => (string)GetValue(TitleTextProperty);
      set => SetValue(TitleTextProperty, value);
    }

    public Color TitleTextColor
    {
      get => (Color)GetValue(TitleTextColorProperty);
      set => SetValue(TitleTextColorProperty, value);
    }

    public int TitleFontSize
    {
      get => (int)GetValue(TitleFontSizeProperty);
      set => SetValue(TitleFontSizeProperty, value);
    }

    public string TitleFontFamily
    {
      get => (string)GetValue(TitleFontFamilyProperty);
      set => SetValue(TitleFontFamilyProperty, value);
    }

    public FontAttributes TitleFontAttributes
    {
      get => (FontAttributes)GetValue(TitleFontAttributesProperty);
      set => SetValue(TitleFontAttributesProperty, value);
    }

    #endregion

    #region Entry properties

    public string EntryText
    {
      get => (string)GetValue(EntryTextProperty);
      set => SetValue(EntryTextProperty, value);
    }

    public Color EntryTextColor
    {
      get => (Color)GetValue(EntryTextColorProperty);
      set => SetValue(EntryTextColorProperty, value);
    }

    public int EntryFontSize
    {
      get => (int)GetValue(EntryFontSizeProperty);
      set => SetValue(EntryFontSizeProperty, value);
    }

    public string EntryFontFamily
    {
      get => (string)GetValue(EntryFontFamilyProperty);
      set => SetValue(EntryFontFamilyProperty, value);
    }

    public FontAttributes EntryFontAttributes
    {
      get => (FontAttributes)GetValue(EntryFontAttributesProperty);
      set => SetValue(EntryFontAttributesProperty, value);
    }

    public bool EntryIsEnabled
    {
      get => (bool)GetValue(EntryIsEnabledProperty);
      set => SetValue(EntryIsEnabledProperty, value);
    }

    #endregion

    public event EventHandler<FocusEventArgs> OnEntryFocused;


    public TitledEntry()
    {
      InitializeComponent();
    }

    protected override void OnParentSet()
    {
      base.OnParentSet();

      EntryTitle.Text = TitleText;
      EntryTitle.TextColor = TitleTextColor;
      EntryTitle.FontSize = TitleFontSize;
      EntryTitle.FontFamily = TitleFontFamily;
      EntryTitle.FontAttributes = TitleFontAttributes;

      InputEntry.Text = EntryText;
      InputEntry.TextColor = EntryTextColor;
      InputEntry.FontSize = EntryFontSize;
      InputEntry.FontFamily = EntryFontFamily;
      InputEntry.FontAttributes = EntryFontAttributes;
      InputEntry.IsEnabled = EntryIsEnabled;

      InputEntry.TextChanged += OnTextChanged;

      InputEntry.Focused += (sender, args) =>
      {
        OnEntryFocused?.Invoke(sender, args);
      };
    }

    // ?? if we should propagate any of the VisualElement properties, such as IsEnabled, to the Entry control
    // or have a different 'EntryIsEnabled' property
    //protected override void OnPropertyChanged(string propertyName = null)
    //{
    //  base.OnPropertyChanged(propertyName);

    //  if (propertyName == nameof(IsEnabled))
    //  {
    //    InputEntry.IsEnabled = IsEnabled;
    //  }
    //}

    private void OnTextChanged(object sender, TextChangedEventArgs args)
    {
      EntryText = args.NewTextValue;
    }

    #region Title PropertyChanged handlers

    private static void OnTitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<string>(bindable, newValue, (entry, value) => { entry.EntryTitle.Text = value; });
    }

    private static void OnTitleTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<Color>(bindable, newValue, (entry, value) => { entry.EntryTitle.TextColor = value; });
    }

    private static void OnTitleFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<int>(bindable, newValue, (entry, value) => { entry.EntryTitle.FontSize = value; });
    }

    private static void OnTitleFontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<string>(bindable, newValue, (entry, value) => { entry.EntryTitle.FontFamily = value; });
    }

    private static void OnTitleFontAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<FontAttributes>(bindable, newValue, (entry, value) => { entry.EntryTitle.FontAttributes = value; });
    }

    #endregion

    #region Entry PropertyChanged handlers

    private static void OnEntryTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<string>(bindable, newValue, (entry, value) => { entry.InputEntry.Text = value; });
    }

    private static void OnEntryTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<Color>(bindable, newValue, (entry, value) => { entry.InputEntry.TextColor = value; });
    }

    private static void OnEntryFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<int>(bindable, newValue, (entry, value) => { entry.InputEntry.FontSize = value; });
    }

    private static void OnEntryFontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<string>(bindable, newValue, (entry, value) => { entry.InputEntry.FontFamily = value; });
    }

    private static void OnEntryFontAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<FontAttributes>(bindable, newValue, (entry, value) => { entry.InputEntry.FontAttributes = value; });
    }

    private static void OnEntryIsEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      SetControlProperty<bool>(bindable, newValue, (entry, value) => { entry.InputEntry.IsEnabled = value; });
    }

    #endregion

    private static void SetControlProperty<TPropertyType>(BindableObject bindable, object newValue, Action<TitledEntry, TPropertyType> propertyAssigner)
    {
      if (!(bindable is TitledEntry entry) || !(newValue is TPropertyType propertyValue))
      {
        return;
      }

      propertyAssigner.Invoke(entry, propertyValue);
    }

  }
}