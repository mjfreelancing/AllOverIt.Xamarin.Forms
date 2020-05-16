using AllOverIt.XamarinForms.Controls;
using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Mvvm
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
      public void Should_Set_Title_Text()
      {
        var expected = Create<string>();

        var subject = new TitledEntry { TitleText = expected };

        subject.TitleText.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Text_Color()
      {
        var expected = Create<Color>();

        var subject = new TitledEntry { TitleTextColor = expected };

        subject.TitleTextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Font_Size()
      {
        var expected = Create<double>();

        var subject = new TitledEntry { TitleFontSize = expected };

        subject.TitleFontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Font_Family()
      {
        var expected = Create<string>();

        var subject = new TitledEntry { TitleFontFamily = expected };

        subject.TitleFontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Title_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = new TitledEntry { TitleFontAttributes = expected };

        subject.TitleFontAttributes.Should().Be(expected);
      }
    }

    public class EntryProperties : TitledEntryFixture
    {
      [Fact]
      public void Should_Set_Entry_Text()
      {
        var expected = Create<string>();

        var subject = new TitledEntry { EntryText = expected };

        subject.EntryText.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Text_Color()
      {
        var expected = Create<Color>();

        var subject = new TitledEntry { EntryTextColor = expected };

        subject.EntryTextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Placeholder()
      {
        var expected = Create<string>();

        var subject = new TitledEntry { EntryPlaceholder = expected };

        subject.EntryPlaceholder.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Placeholder_Color()
      {
        var expected = Create<Color>();

        var subject = new TitledEntry { EntryPlaceholderColor = expected };

        subject.EntryPlaceholderColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Font_Size()
      {
        var expected = Create<double>();

        var subject = new TitledEntry { EntryFontSize = expected };

        subject.EntryFontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Font_Family()
      {
        var expected = Create<string>();

        var subject = new TitledEntry { EntryFontFamily = expected };

        subject.EntryFontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = new TitledEntry { EntryFontAttributes = expected };

        subject.EntryFontAttributes.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Is_Text_Prediction()
      {
        var expected = Create<bool>();

        var subject = new TitledEntry { EntryIsTextPredictionEnabled = expected };

        subject.EntryIsTextPredictionEnabled.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Entry_Is_Enabled()
      {
        var expected = Create<bool>();

        var subject = new TitledEntry { EntryIsEnabled = expected };

        subject.EntryIsEnabled.Should().Be(expected);
      }

      [Fact]
      public void Should_Focus_Entry()
      {
        var subject = new TitledEntry();

        SetupInputEntryFocus(subject);

        subject.EntryFocus();

        subject.EntryIsFocused.Should().BeTrue();
      }

      [Fact]
      public void Should_Unfocus_Entry()
      {
        var subject = new TitledEntry();

        SetupInputEntryFocus(subject);

        subject.EntryFocus();
        subject.EntryIsFocused.Should().BeTrue();

        subject.EntryUnfocus();
        subject.EntryIsFocused.Should().BeFalse();
      }

      [Fact]
      public void Should_Set_Entry_Is_Password()
      {
        var expected = Create<bool>();

        var subject = new TitledEntry { EntryIsPassword = expected };

        subject.EntryIsPassword.Should().Be(expected);
      }
    }

    public class EntryEvents : TitledEntryFixture
    {
      [Fact]
      public void Should_Add_EntryFocused()
      {
        var actual = false;
        var subject = new TitledEntry();

        SetupInputEntryFocus(subject);
        SetupInputEntryOnParentSet(subject);

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
        var subject = new TitledEntry();

        SetupInputEntryFocus(subject);
        SetupInputEntryOnParentSet(subject);

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
        var subject = new TitledEntry();

        SetupInputEntryFocus(subject);
        SetupInputEntryOnParentSet(subject);

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
        var subject = new TitledEntry();

        SetupInputEntryFocus(subject);
        SetupInputEntryOnParentSet(subject); 
        
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
        var subject = new TitledEntry();

        SetupInputEntryOnParentSet(subject);

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
        var subject = new TitledEntry();

        SetupInputEntryOnParentSet(subject);

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

    private static void SetupInputEntryFocus(TitledEntry titledEntry)
    {
      // get the Entry control defined in the XAML
      var entryFieldInfo = typeof(TitledEntry)
        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
        .Single(item => item.Name == "InputEntry");

      var inputEntry = (Entry)entryFieldInfo.GetValue(titledEntry);

      EntryUtilities.ConfigureEntryFocus(inputEntry);
    }

    private static void SetupInputEntryOnParentSet(TitledEntry titledEntry)
    {
      // need to invoke OnParentSet so the event handlers are setup
      var onParentSet = typeof(TitledEntry).GetMethod("OnParentSet", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

      onParentSet.Invoke(titledEntry, null);
    }
  }
}