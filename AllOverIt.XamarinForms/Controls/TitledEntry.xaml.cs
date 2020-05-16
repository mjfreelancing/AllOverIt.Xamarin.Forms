using AllOverIt.XamarinForms.Mvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AllOverIt.XamarinForms.Controls
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class TitledEntry : ContentView
  {
    private readonly WeakEventManager _eventManager = new WeakEventManager();

    #region Title properties

    public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(nameof(TitleText), typeof(string), typeof(TitledEntry), default(string));

    public string TitleText
    {
      get => (string)GetValue(TitleTextProperty);
      set => SetValue(TitleTextProperty, value);
    }

    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(nameof(TitleTextColor), typeof(Color), typeof(TitledEntry), default(Color));
    
    public Color TitleTextColor
    {
      get => (Color)GetValue(TitleTextColorProperty);
      set => SetValue(TitleTextColorProperty, value);
    }

    public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(TitledEntry), default(double));

    [TypeConverter(typeof(FontSizeConverter))]
    public double TitleFontSize
    {
      get => (double)GetValue(TitleFontSizeProperty);
      set => SetValue(TitleFontSizeProperty, value);
    }

    public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.Create(nameof(TitleFontFamily), typeof(string), typeof(TitledEntry), default(string));
    
    public string TitleFontFamily
    {
      get => (string)GetValue(TitleFontFamilyProperty);
      set => SetValue(TitleFontFamilyProperty, value);
    }

    public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes), typeof(TitledEntry), default(FontAttributes));

    public FontAttributes TitleFontAttributes
    {
      get => (FontAttributes)GetValue(TitleFontAttributesProperty);
      set => SetValue(TitleFontAttributesProperty, value);
    }

    #endregion

    #region Entry properties

    public static readonly BindableProperty EntryTextProperty = BindableProperty.Create(nameof(EntryText), typeof(string), typeof(TitledEntry), default(string), BindingMode.TwoWay);

    public string EntryText
    {
      get => (string)GetValue(EntryTextProperty);
      set => SetValue(EntryTextProperty, value);
    }

    public static readonly BindableProperty EntryTextColorProperty = BindableProperty.Create(nameof(EntryTextColor), typeof(Color), typeof(TitledEntry), default(Color));
    
    public Color EntryTextColor
    {
      get => (Color)GetValue(EntryTextColorProperty);
      set => SetValue(EntryTextColorProperty, value);
    }

    public static readonly BindableProperty EntryPlaceholderProperty = BindableProperty.Create(nameof(EntryPlaceholder), typeof(string), typeof(TitledEntry), default(string));

    public string EntryPlaceholder
    {
      get => (string)GetValue(EntryPlaceholderProperty);
      set => SetValue(EntryPlaceholderProperty, value);
    }

    public static readonly BindableProperty EntryPlaceholderColorProperty = BindableProperty.Create(nameof(EntryPlaceholderColor), typeof(Color), typeof(TitledEntry), default(Color));

    public Color EntryPlaceholderColor
    {
      get => (Color)GetValue(EntryPlaceholderColorProperty);
      set => SetValue(EntryPlaceholderColorProperty, value);
    }

    public static readonly BindableProperty EntryFontSizeProperty = BindableProperty.Create(nameof(EntryFontSize), typeof(double), typeof(TitledEntry), default(double));

    [TypeConverter(typeof(FontSizeConverter))]
    public double EntryFontSize
    {
      get => (double)GetValue(EntryFontSizeProperty);
      set => SetValue(EntryFontSizeProperty, value);
    }

    public static readonly BindableProperty EntryFontFamilyProperty = BindableProperty.Create(nameof(EntryFontFamily), typeof(string), typeof(TitledEntry), default(string));
    
    public string EntryFontFamily
    {
      get => (string)GetValue(EntryFontFamilyProperty);
      set => SetValue(EntryFontFamilyProperty, value);
    }

    public static readonly BindableProperty EntryFontAttributesProperty = BindableProperty.Create(nameof(EntryFontAttributes), typeof(FontAttributes), typeof(TitledEntry), default(FontAttributes));
    
    public FontAttributes EntryFontAttributes
    {
      get => (FontAttributes)GetValue(EntryFontAttributesProperty);
      set => SetValue(EntryFontAttributesProperty, value);
    }

    public static readonly BindableProperty EntryIsTextPredictionEnabledProperty = BindableProperty.Create(nameof(EntryIsTextPredictionEnabled), typeof(bool), typeof(TitledEntry), false);

    public bool EntryIsTextPredictionEnabled
    {
      get => (bool)GetValue(EntryIsTextPredictionEnabledProperty);
      set => SetValue(EntryIsTextPredictionEnabledProperty, value);
    }

    public static readonly BindableProperty EntryIsEnabledProperty = BindableProperty.Create(nameof(EntryIsEnabled), typeof(bool), typeof(TitledEntry), true);
    
    public bool EntryIsEnabled
    {
      get => (bool)GetValue(EntryIsEnabledProperty);
      set => SetValue(EntryIsEnabledProperty, value);
    }

    public bool EntryIsFocused => InputEntry.IsFocused;

    public static readonly BindableProperty EntryIsPasswordProperty = BindableProperty.Create(nameof(EntryIsPassword), typeof(bool), typeof(TitledEntry), false);

    public bool EntryIsPassword
    {
      get => (bool)GetValue(EntryIsPasswordProperty);
      set => SetValue(EntryIsPasswordProperty, value);
    }

    #endregion

    #region Entry events

    public event EventHandler<FocusEventArgs> EntryFocused
    {
      add => _eventManager.AddEventHandler(value);
      remove => _eventManager.RemoveEventHandler(value);
    }

    public event EventHandler<FocusEventArgs> EntryUnfocused
    {
      add => _eventManager.AddEventHandler(value);
      remove => _eventManager.RemoveEventHandler(value);
    }

    public event EventHandler<TextChangedEventArgs> EntryTextChanged
    {
      add => _eventManager.AddEventHandler(value);
      remove => _eventManager.RemoveEventHandler(value);
    }

    #endregion

    public TitledEntry()
    {
      InitializeComponent();
    }

    protected override void OnParentSet()
    {
      base.OnParentSet();

      SetTitleBindings();
      SetEntryBindings();
      SetEntryEventHandlers();
    }

    private void SetTitleBindings()
    {
      // bind properties from this control to the Label
      EntryTitle.BindingContext = this;
      EntryTitle.SetBinding(Label.TextProperty, nameof(TitleText));
      EntryTitle.SetBinding(Label.TextColorProperty, nameof(TitleTextColor));
      EntryTitle.SetBinding(Label.FontSizeProperty, nameof(TitleFontSize));
      EntryTitle.SetBinding(Label.FontFamilyProperty, nameof(TitleFontFamily));
      EntryTitle.SetBinding(Label.FontAttributesProperty, nameof(TitleFontAttributes));
    }

    private void SetEntryBindings()
    {
      // bind properties from this control to the Entry
      InputEntry.BindingContext = this;
      InputEntry.SetBinding(Entry.TextProperty, nameof(EntryText));
      InputEntry.SetBinding(Entry.TextColorProperty, nameof(EntryTextColor));
      InputEntry.SetBinding(Entry.FontSizeProperty, nameof(EntryFontSize));
      InputEntry.SetBinding(Entry.FontFamilyProperty, nameof(EntryFontFamily));
      InputEntry.SetBinding(Entry.FontAttributesProperty, nameof(EntryFontAttributes));
      InputEntry.SetBinding(Entry.IsEnabledProperty, nameof(EntryIsEnabled));
      InputEntry.SetBinding(Entry.PlaceholderProperty, nameof(EntryPlaceholder));
      InputEntry.SetBinding(Entry.PlaceholderColorProperty, nameof(EntryPlaceholderColor));
      InputEntry.SetBinding(Entry.IsTextPredictionEnabledProperty, nameof(EntryIsTextPredictionEnabled));
      InputEntry.SetBinding(Entry.IsPasswordProperty, nameof(EntryIsPassword));
    }

    private void SetEntryEventHandlers()
    {
      InputEntry.TextChanged += (sender, args) => OnEntryTextChanged(args);
      InputEntry.Focused += (sender, args) => OnEntryFocused(args);
      InputEntry.Unfocused += (sender, args) => OnEntryUnfocused(args);
    }

    private void OnEntryFocused(FocusEventArgs args)
    {
      _eventManager.HandleEvent(this, args, nameof(EntryFocused));
    }

    private void OnEntryUnfocused(FocusEventArgs args)
    {
      _eventManager.HandleEvent(this, args, nameof(EntryUnfocused));
    }

    private void OnEntryTextChanged(TextChangedEventArgs args)
    {
      EntryText = args.NewTextValue;

      _eventManager.HandleEvent(this, args, nameof(EntryTextChanged));
    }
  }
}