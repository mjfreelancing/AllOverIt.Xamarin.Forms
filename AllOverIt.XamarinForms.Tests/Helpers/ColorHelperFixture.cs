using AllOverIt.XamarinForms.Helpers;
using FluentAssertions;
using System;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Helpers
{
  public class ColorHelperFixture : AllOverItFixtureBase
  {
    public class GetHighestContrastColor: ColorHelperFixture
    {
      public static TheoryData<Color, Color, Color, Color> Data =>
        new TheoryData<Color, Color, Color, Color>
        {
          {Color.White, Color.Black, Color.White, Color.Black},
          {Color.Black, Color.Black, Color.White, Color.White},
          {Color.Red, Color.Green, Color.Blue, Color.Blue},
          {Color.Red, Color.Blue, Color.Green, Color.Blue},
          {Color.FromHex("#121212"), Color.FromHex("#222222"), Color.FromHex("#99333333"), Color.FromHex("#99333333")}, // (A)RGB
          {Color.FromHex("#121212"), Color.FromHex("#99222222"), Color.FromHex("#333333"), Color.FromHex("#333333")},
          {Color.FromHex("#121212"), Color.FromHex("#99222222"), Color.FromHex("#98222222"), Color.FromHex("#98222222")},
          {Color.FromHex("#1f1b24"), Color.FromHex("#99222222"), Color.FromHex("#98222222"), Color.FromHex("#98222222")},
          {Color.FromHex("#1f1b24"), Color.FromHex("#60ffffff"), Color.FromHex("#6200ee"), Color.FromHex("#60ffffff")}
        };

      [Theory]
      [MemberData(nameof(Data))]
      public void Should_Select_Greatest_Contrast_Color(Color backgroundColor, Color color1, Color color2, Color expected)
      {
        var actual = ColorHelper.GetHighestContrastColor(backgroundColor, color1, color2);

        actual.Should().Be(expected);
      }
    }

    public class GetContrastRatio : ColorHelperFixture
    {
      [Theory]
      [InlineData("#404040", "#A8A8A8", 4.36d)]
      [InlineData("#0000FF", "#FFFFFF", 8.59d)]
      [InlineData("#8A8AFF", "#FFFFFF", 2.93d)]
      public void Should_Calculate_Contrast_Ratio_With_No_Alpha(string foreground, string background, double expected)
      {
        var actual = Math.Round(ColorHelper.GetContrastRatio(Color.FromHex(foreground), Color.FromHex(background)), 2);

        actual.Should().Be(expected);
      }

      [Theory]
      [InlineData("#FF404040", "#A8A8A8", 4.36d)]           // format is (A)RGB
      [InlineData("#F0404040", "#A8A8A8", 3.91d)]
      [InlineData("#CC404040", "#A8A8A8", 3.14d)]
      [InlineData("#30404040", "#A8A8A8", 1.26d)]
      [InlineData("#0C404040", "#A8A8A8", 1.05d)]
      [InlineData("#FF0000FF", "#FFFFFF", 8.59d)]
      [InlineData("#990000FF", "#FFFFFF", 4.28d)]
      [InlineData("#FF8A8AFF", "#FFFFFF", 2.93d)]
      [InlineData("#CD8A8AFF", "#FFFFFF", 2.32d)]
      public void Should_Calculate_Contrast_Ratio_With_Alpha(string foreground, string background, double expected)
      {
        var actual = Math.Round(ColorHelper.GetContrastRatio(Color.FromHex(foreground), Color.FromHex(background)), 2);

        actual.Should().Be(expected);
      }
    }

    public class GetRelativeLuminance : ColorHelperFixture
    {
      [Theory]
      [InlineData("#404040", 0.05d)]           // format is (A)RGB
      [InlineData("#CA8AFF", 0.38d)]
      [InlineData("#CCCCCC", 0.60d)]
      [InlineData("#EEFFDD", 0.95d)]
      public void Should_Calculate_Relative_Luminance(string colorAsHex, double expected)
      {
        var actual = Math.Round(ColorHelper.GetRelativeLuminance(Color.FromHex(colorAsHex)), 2);

        actual.Should().Be(expected);
      }
    }

    public class CombineColors : ColorHelperFixture
    {
      public static TheoryData<string, string, double, string> Data =>
        new TheoryData<string, string, double, string>
        {
          {"#000000", "#FFFFFF", 1.0d, "#FFFFFFFF"}, // (A)RGB
          {"#FFFFFF", "#0000FF", 1.0d, "#FF0000FF"},
          {"#0000FF", "#00FF00", 0.5, "#FF008080"},
          {"#00FF00", "#0000FF", 0.5, "#FF008080"},
          {"#123456", "#654321", 0.8, "#FF55402C"},
          {"#C0C0EE", "#121212", 0.8, "#FF35353E"},
          {"#121212", "#C0C0EE", 0.8, "#FF9E9EC2"}
        };

      public class Using_String : CombineColors
      {
        [Theory]
        [MemberData(nameof(Data))]
        public void Should_Combine_Colors(string baseColor, string overlayColor, double opacity, string expected)
        {
          var actual = ColorHelper.CombineColors(baseColor, overlayColor, opacity).ToHex();

          actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(-0.1d)]
        [InlineData(1.1d)]
        public void Should_Throw_When_Opacity_Out_Of_Range(double opacity)
        {
          Invoking(() =>
            {
              ColorHelper.CombineColors("121212", "121212", opacity);
            })
            .Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("The opacity must have a value between 0 and 1 (Parameter 'overlayOpacity')");
        }
      }

      public class Using_Color : CombineColors
      {
        [Theory]
        [MemberData(nameof(Data))]
        public void Should_Combine_Colors(string baseColor, string overlayColor, double opacity, string expected)
        {
          var actual = ColorHelper.CombineColors(Color.FromHex(baseColor), Color.FromHex(overlayColor), opacity).ToHex();

          actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(-0.1d)]
        [InlineData(1.1d)]
        public void Should_Throw_When_Opacity_Out_Of_Range(double opacity)
        {
          Invoking(() =>
            {
              ColorHelper.CombineColors(Color.Red, Color.Red, opacity);
            })
            .Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("The opacity must have a value between 0 and 1 (Parameter 'overlayOpacity')");
        }
      }
    }

    public class FromHexWithOpacity : ColorHelperFixture
    {
      [Theory]
      [InlineData("#FF112233", 1.0d, "#FF112233")]
      [InlineData("#FF112233", 0.5d, "#7F112233")]
      [InlineData("#112233", 0.5d, "#7F112233")]
      [InlineData("#112233", 0.0d, "#00112233")]
      public void Should_Get_Color_With_Opacity(string color, double opacity, string expected)
      {
        var actual = ColorHelper.FromHexWithOpacity(color, opacity).ToHex();

        actual.Should().Be(expected);
      }
    }

    public class FromColorWithOpacity : ColorHelperFixture
    {
      [Theory]
      [InlineData("#FF112233", 1.0d, "#FF112233")]
      [InlineData("#FF112233", 0.5d, "#7F112233")]
      [InlineData("#112233", 0.5d, "#7F112233")]
      [InlineData("#112233", 0.0d, "#00112233")]
      public void Should_Get_Color_With_Opacity(string colorHex, double opacity, string expected)
      {
        var color = Color.FromHex(colorHex);

        var actual = ColorHelper.FromColorWithOpacity(color, opacity).ToHex();

        actual.Should().Be(expected);
      }
    }

    public class GetRed : ColorHelperFixture
    {
      [Theory]
      [InlineData("#FF112233", 17)]
      [InlineData("#FF223311", 34)]
      [InlineData("#112233", 17)]
      [InlineData("#002200", 0)]
      [InlineData("#000022", 0)]
      [InlineData("#220000", 34)]
      public void Should_Get_Red(string colorHex, int expected)
      {
        var color = Color.FromHex(colorHex);

        var actual = ColorHelper.GetRed(color);

        actual.Should().Be(expected);
      }
    }

    public class GetGreen : ColorHelperFixture
    {
      [Theory]
      [InlineData("#FF112233", 34)]
      [InlineData("#FF223311", 51)]
      [InlineData("#112233", 34)]
      [InlineData("#002200", 34)]
      [InlineData("#000022", 0)]
      [InlineData("#220000", 0)]
      public void Should_Get_Green(string colorHex, int expected)
      {
        var color = Color.FromHex(colorHex);

        var actual = ColorHelper.GetGreen(color);

        actual.Should().Be(expected);
      }
    }

    public class GetBlue : ColorHelperFixture
    {
      [Theory]
      [InlineData("#FF112233", 51)]
      [InlineData("#FF223311", 17)]
      [InlineData("#112233", 51)]
      [InlineData("#002200", 0)]
      [InlineData("#000022", 34)]
      [InlineData("#220000", 0)]
      public void Should_Get_Blue(string colorHex, int expected)
      {
        var color = Color.FromHex(colorHex);

        var actual = ColorHelper.GetBlue(color);

        actual.Should().Be(expected);
      }
    }
  }
}