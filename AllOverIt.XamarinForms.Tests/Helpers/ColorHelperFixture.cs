using AllOverIt.XamarinForms.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Helpers
{
  public class ColorHelperFixture : AllOverItFixtureBase
  {
    public class GetHighestContrastColor : ColorHelperFixture
    {
      private readonly IList<(Color backgroundColor, Color color1, Color color2, Color expected)> _expectations = new List<(Color, Color, Color, Color)>
      {
        (Color.White, Color.Black, Color.White, Color.Black),
        (Color.Black, Color.Black, Color.White, Color.White),
        (Color.Red, Color.Green, Color.Blue, Color.Blue),
        (Color.Red, Color.Blue, Color.Green, Color.Blue),
        (Color.FromHex("#121212"), Color.FromHex("#222222"), Color.FromHex("#33333399"), Color.FromHex("#222222")),
        (Color.FromHex("#121212"), Color.FromHex("#22222299"), Color.FromHex("#333333"), Color.FromHex("#333333")),
        (Color.FromHex("#121212"), Color.FromHex("#22222299"), Color.FromHex("#22222298"), Color.FromHex("#22222298")),
        (Color.FromHex("#1f1b24"), Color.FromHex("#22222299"), Color.FromHex("#22222298"), Color.FromHex("#22222298")),
        (Color.FromHex("#1f1b24"), Color.FromHex("#ffffff60"), Color.FromHex("#6200ee"), Color.FromHex("#ffffff60"))
      };

      [Theory]
      [InlineData(0)]
      [InlineData(1)]
      [InlineData(2)]
      [InlineData(3)]
      [InlineData(4)]
      [InlineData(5)]
      [InlineData(6)]
      [InlineData(7)]
      [InlineData(8)]
      public void Should_Select_Greatest_Contrast_Color(int index)
      {
        var (backgroundColor, color1, color2, expected) = _expectations[index];

        var actual = ColorHelper.GetHighestContrastColor(backgroundColor, color1, color2);

        actual.Should().Be(expected);
      }
    }

    public class GetContrastRatio : ColorHelperFixture
    {

    }

    public class GetRelativeLuminance : ColorHelperFixture
    {

    }

    public class CombineColors_String : ColorHelperFixture
    {

    }

    public class CombineColors_Color : ColorHelperFixture
    {

    }

    public class FromHexWithOpacity : ColorHelperFixture
    {

    }

    public class FromColorWithOpacity : ColorHelperFixture
    {

    }

    public class GetRed : ColorHelperFixture
    {

    }

    public class GetGreen : ColorHelperFixture
    {

    }

    public class GetBlue : ColorHelperFixture
    {

    }
  }
}