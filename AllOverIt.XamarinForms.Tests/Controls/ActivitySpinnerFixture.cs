using AllOverIt.XamarinForms.Controls;
using FluentAssertions;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Controls
{
  // need to create controls non-parallel to prevent init issues
  [Collection(nameof(ControlCollection))]
  public class ActivitySpinnerFixture : AllOverItFixtureBase
  {
    public ActivitySpinnerFixture()
    {
      InitFormsFixture();
    }

    public class ActivityIndicatorProperties : ActivitySpinnerFixture
    {
      [Fact]
      public void Should_Get_Indicator_Color()
      {
        var expected = Create<Color>();

        var subject = new ActivitySpinner { IndicatorColor = expected };

        subject.IndicatorColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Indicator_Color()
      {
        var expected = Create<Color>();

        var subject = new ActivitySpinner { IndicatorColor = expected };

        var spinner = ReflectionHelpers.GetNonPublicFieldFromControl<ActivityIndicator>(subject, "Spinner");

        spinner.Color.Should().Be(expected);
      }
    }

    public class LabelProperties : ActivitySpinnerFixture
    {
      [Fact]
      public void Should_Get_Spinner_Text()
      {
        var expected = Create<string>();

        var subject = new ActivitySpinner { Text = expected };

        subject.Text.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Spinner_Text()
      {
        var expected = Create<string>();

        var subject = new ActivitySpinner { Text = expected };

        var label = ReflectionHelpers.GetNonPublicFieldFromControl<Label>(subject, "SpinnerText");

        label.Text.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Spinner_Text_Color()
      {
        var expected = Create<Color>();

        var subject = new ActivitySpinner { TextColor = expected };

        subject.TextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Spinner_Text_Color()
      {
        var expected = Create<Color>();

        var subject = new ActivitySpinner { TextColor = expected };

        var label = ReflectionHelpers.GetNonPublicFieldFromControl<Label>(subject, "SpinnerText");

        label.TextColor.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Spinner_Text_Font_Family()
      {
        var expected = Create<string>();

        var subject = new ActivitySpinner { TextFontFamily = expected };

        subject.TextFontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Spinner_Text_Font_Family()
      {
        var expected = Create<string>();

        var subject = new ActivitySpinner { TextFontFamily = expected };

        var label = ReflectionHelpers.GetNonPublicFieldFromControl<Label>(subject, "SpinnerText");

        label.FontFamily.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Spinner_Text_Font_Size()
      {
        var expected = Create<double>();

        var subject = new ActivitySpinner { TextFontSize = expected };

        subject.TextFontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Spinner_Text_Font_Size()
      {
        var expected = Create<double>();

        var subject = new ActivitySpinner { TextFontSize = expected };

        var label = ReflectionHelpers.GetNonPublicFieldFromControl<Label>(subject, "SpinnerText");

        label.FontSize.Should().Be(expected);
      }

      [Fact]
      public void Should_Get_Spinner_Text_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = new ActivitySpinner { TextFontAttributes = expected };

        subject.TextFontAttributes.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Spinner_Text_Font_Attributes()
      {
        var expected = Create<FontAttributes>();

        var subject = new ActivitySpinner { TextFontAttributes = expected };

        var label = ReflectionHelpers.GetNonPublicFieldFromControl<Label>(subject, "SpinnerText");

        label.FontAttributes.Should().Be(expected);
      }
    }

    public class ActivitySpinnerProperties : ActivitySpinnerFixture
    {
      [Fact]
      public void Should_Get_Default_Fade_Duration()
      {
        var subject = new ActivitySpinner();

        subject.FadeDuration.Should().Be(400);
      }

      [Fact]
      public void Should_Set_Fade_Duration()
      {
        var expected = Create<uint>();

        var subject = new ActivitySpinner { FadeDuration = expected };

        subject.FadeDuration.Should().Be(expected);
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Set_IsBusy(bool expected)
      {
        var subject = new ActivitySpinner { IsBusy = expected};

        subject.IsBusy.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Spinner_Visibility()
      {
        var subject = new ActivitySpinner { IsBusy = true };

        subject.IsBusy.Should().BeTrue();
        subject.IsVisible.Should().BeTrue();

        var spinner = ReflectionHelpers.GetNonPublicFieldFromControl<ActivityIndicator>(subject, "Spinner");

        spinner.IsRunning.Should().BeTrue();
      }

      [Fact]
      public void Should_Toggle_Spinner_Visibility()
      {
        var subject = new ActivitySpinner { IsBusy = true};
        subject.IsBusy = false;

        subject.IsBusy.Should().BeFalse();
        subject.IsVisible.Should().BeFalse();

        var spinner = ReflectionHelpers.GetNonPublicFieldFromControl<ActivityIndicator>(subject, "Spinner");

        spinner.IsRunning.Should().BeFalse();
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Set_Spinner_Text_Visibility(bool expected)
      {
        var text = expected ? Create<string>() : string.Empty;

        var subject = new ActivitySpinner { Text = text };

        var label = ReflectionHelpers.GetNonPublicFieldFromControl<Label>(subject, "SpinnerText");

        label.IsVisible.Should().Be(expected);
      }
    }
  }
}