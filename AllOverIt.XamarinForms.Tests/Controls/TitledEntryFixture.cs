using AllOverIt.XamarinForms.Controls;
using FluentAssertions;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Controls
{
  public class TitledEntryFixture : AllOverItFixtureBase
  {
    public TitledEntryFixture()
    {
      InitFormsFixture();
    }

    public class TitleProperties : TitledEntryFixture
    {
      [Fact]
      public void Should_Get_Title_Text()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.TitleText = expected;

        subject.TitleText.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Text()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.TitleText = expected;

        var label = ControlHelpers.GetNonPublicFieldFromControl<Label>(subject, "EntryTitle");

        label.Text.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Title_Text_Color()
      {
        var expected = Create<Color>();

        var subject = CreatedTitledEntry();
        subject.TitleTextColor = expected;

        subject.TitleTextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Text_Color()
      {
        var expected = Create<Color>();

        var subject = CreatedTitledEntry();
        subject.TitleTextColor = expected;

        var label = ControlHelpers.GetNonPublicFieldFromControl<Label>(subject, "EntryTitle");

        label.TextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Title_Font_Size()
      {
        var expected = Create<double>();

        var subject = CreatedTitledEntry();
        subject.TitleFontSize = expected;

        subject.TitleFontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Font_Size()
      {
        var expected = Create<double>();

        var subject = CreatedTitledEntry();
        subject.TitleFontSize = expected;

        var label = ControlHelpers.GetNonPublicFieldFromControl<Label>(subject, "EntryTitle");

        label.FontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Title_Font_Family()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.TitleFontFamily = expected;

        subject.TitleFontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Font_Family()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.TitleFontFamily = expected;

        var label = ControlHelpers.GetNonPublicFieldFromControl<Label>(subject, "EntryTitle");

        label.FontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Title_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = CreatedTitledEntry();
        subject.TitleFontAttributes = expected;

        subject.TitleFontAttributes.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = CreatedTitledEntry();
        subject.TitleFontAttributes = expected;

        var label = ControlHelpers.GetNonPublicFieldFromControl<Label>(subject, "EntryTitle");

        label.FontAttributes.Should().Be(expected);
      }
    }

    public class EntryProperties : TitledEntryFixture
    {
      [Fact]
      public void Should_Get_Entry_Text()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.EntryText = expected;

        subject.EntryText.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Text()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.EntryText = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.Text.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Entry_Text_Color()
      {
        var expected = Create<Color>();

        var subject = CreatedTitledEntry();
        subject.EntryTextColor = expected;

        subject.EntryTextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Text_Color()
      {
        var expected = Create<Color>();

        var subject = CreatedTitledEntry();
        subject.EntryTextColor = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.TextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Entry_Placeholder()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.EntryPlaceholder = expected;

        subject.EntryPlaceholder.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Placeholder()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.EntryPlaceholder = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.Placeholder.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Entry_Placeholder_Color()
      {
        var expected = Create<Color>();

        var subject = CreatedTitledEntry();
        subject.EntryPlaceholderColor = expected;

        subject.EntryPlaceholderColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Placeholder_Color()
      {
        var expected = Create<Color>();

        var subject = CreatedTitledEntry();
        subject.EntryPlaceholderColor = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.PlaceholderColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Entry_Font_Size()
      {
        var expected = Create<double>();

        var subject = CreatedTitledEntry();
        subject.EntryFontSize = expected;

        subject.EntryFontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Font_Size()
      {
        var expected = Create<double>();

        var subject = CreatedTitledEntry();
        subject.EntryFontSize = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.FontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Entry_Font_Family()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.EntryFontFamily = expected;

        subject.EntryFontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Font_Family()
      {
        var expected = Create<string>();

        var subject = CreatedTitledEntry();
        subject.EntryFontFamily = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.FontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Entry_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = CreatedTitledEntry();
        subject.EntryFontAttributes = expected;

        subject.EntryFontAttributes.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = CreatedTitledEntry();
        subject.EntryFontAttributes = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.FontAttributes.Should().Be(expected);
      }

      //[Theory]
      //[InlineData(false)]
      //[InlineData(true)]
      //public void Should_Get_Entry_Is_Text_Prediction(bool expected)
      //{
      //  var subject = CreatedTitledEntry();
      //  subject.EntryIsTextPredictionEnabled = expected;

      //  subject.EntryIsTextPredictionEnabled.Should().Be(expected);
      //}

      // disabled for now - False is never set
      //[Theory]
      //[InlineData(false)]
      //[InlineData(true)]
      //public void Should_Set_Entry_Is_Text_Prediction(bool expected)
      //{
      //  var subject = CreatedTitledEntry();
      //  subject.EntryIsTextPredictionEnabled = expected;

      //  var entry = ControlHelpers.GetXamlControl<Entry>(subject, "InputEntry");

      //  entry.IsTextPredictionEnabled.Should().Be(expected);
      //}

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Get_Entry_Is_Enabled(bool expected)
      {
        var subject = CreatedTitledEntry();
        subject.EntryIsEnabled = expected;

        subject.EntryIsEnabled.Should().Be(expected);
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Set_Entry_Is_Enabled(bool expected)
      {
        var subject = CreatedTitledEntry();
        subject.EntryIsEnabled = expected;

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");

        entry.IsEnabled.Should().Be(expected);
      }

      [Fact]
      public void Should_Focus_Entry()
      {
        var subject = CreatedTitledEntry();

        subject.EntryFocus();

        subject.EntryIsFocused.Should().BeTrue();

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");
        entry.IsFocused.Should().BeTrue();
      }

      [Fact]
      public void Should_Unfocus_Entry()
      {
        var subject = CreatedTitledEntry();

        subject.EntryFocus();
        subject.EntryIsFocused.Should().BeTrue();

        subject.EntryUnfocus();
        subject.EntryIsFocused.Should().BeFalse();

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");
        entry.IsFocused.Should().BeFalse();
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Set_Entry_Is_Password(bool expected)
      {
        var subject = CreatedTitledEntry();
        subject.EntryIsPassword = expected;

        subject.EntryIsPassword.Should().Be(expected);

        var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "InputEntry");
        entry.IsPassword.Should().Be(expected);
      }
    }

    public class EntryEvents : TitledEntryFixture
    {
      [Fact]
      public void Should_Add_EntryFocused()
      {
        var actual = false;
        var subject = CreatedTitledEntry();

        subject.EntryFocused += (sender, args) =>
        {
          actual = true;
        };

        subject.EntryFocus();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Remove_EntryFocused()
      {
        var actual = false;
        var subject = CreatedTitledEntry();

        void EntryFocusedHandler(object sender, FocusEventArgs args)
        {
          actual = true;
        }

        subject.EntryFocused += EntryFocusedHandler;
        subject.EntryFocus();
        actual.Should().BeTrue();

        actual = false;
        subject.EntryFocused -= EntryFocusedHandler;

        subject.EntryFocus();
        actual.Should().BeFalse();
      }

      [Fact]
      public void Should_Add_EntryUnfocused()
      {
        var actual = false; 
        var subject = CreatedTitledEntry();

        subject.EntryUnfocused += (sender, args) =>
        {
          actual = true;
        };

        subject.EntryFocus();
        subject.EntryIsFocused.Should().BeTrue();

        subject.EntryUnfocus();
        subject.EntryIsFocused.Should().BeFalse();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Remove_EntryUnfocused()
      {
        var actual = false;
        var subject = CreatedTitledEntry();

        void EntryUnfocusedHandler(object sender, FocusEventArgs args)
        {
          actual = true;
        }

        subject.EntryUnfocused += EntryUnfocusedHandler;

        subject.EntryFocus();
        subject.EntryIsFocused.Should().BeTrue();

        subject.EntryUnfocus();
        subject.EntryIsFocused.Should().BeFalse();

        actual.Should().BeTrue();

        actual = false;
        subject.EntryUnfocused -= EntryUnfocusedHandler;

        subject.EntryFocus();
        subject.EntryIsFocused.Should().BeTrue();

        subject.EntryUnfocus();
        subject.EntryIsFocused.Should().BeFalse();

        actual.Should().BeFalse();
      }

      [Fact]
      public void Should_Add_EntryTextChanged()
      {
        var actual = false;
        var subject = CreatedTitledEntry();

        subject.EntryTextChanged += (sender, args) =>
        {
          actual = true;
        };

        subject.EntryText = Create<string>();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Remove_EntryTextChanged()
      {
        var actual = false;
        var subject = CreatedTitledEntry();

        void TextChangedHandler(object sender, TextChangedEventArgs args)
        {
          actual = true;
        }

        subject.EntryTextChanged += TextChangedHandler;

        subject.EntryText = Create<string>();
        actual.Should().BeTrue();

        actual = false;
        subject.EntryTextChanged -= TextChangedHandler;

        subject.EntryText = Create<string>();
        actual.Should().BeFalse();
      }
    }

    private static TitledEntry CreatedTitledEntry()
    {
      var titledEntry = new TitledEntry();
      SetupInputEntryOnParentSet(titledEntry);
      SetupInputEntryFocus(titledEntry);

      return titledEntry;
    }

    private static void SetupInputEntryFocus(TitledEntry titledEntry)
    {
      // get the Entry control defined in the XAML
      var inputEntry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(titledEntry, "InputEntry");

      EntryUtilities.ConfigureEntryFocus(inputEntry);
    }

    private static void SetupInputEntryOnParentSet(TitledEntry titledEntry)
    {
      // need to invoke OnParentSet so the event handlers are setup
      var onParentSet = typeof(TitledEntry).GetMethod("OnParentSet", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

      _ = onParentSet ?? throw new InvalidOperationException("OnParentSet not found");

      onParentSet.Invoke(titledEntry, null);
    }
  }
}