using AllOverIt.XamarinForms.Converters;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Converters
{
  public class ColorOpacityConverterFixture : AllOverItFixtureBase
  {
    private readonly IList<(object value, object opacity, Color expected)> _expectations = new List<(object, object, Color)>
    {
      (Color.Red, null, Color.Red),
      (Color.Red, 1.0d, Color.Red),
      (Color.Red, 0.5d, Color.FromRgba(1, 0, 0, 0.5)),
      (Color.Red, "0.75", Color.FromRgba(1, 0, 0, 0.75)),
      ("#112233", null, Color.FromHex("#112233")),
      ("#112233", 1.0d, Color.FromHex("#112233")),
      ("#112233", 0.5d, ConvertToColor("#112233", 0.5d)),
      ("#112233", "0.75", ConvertToColor("#112233", 0.75d))
    };

    public class Convert : ColorOpacityConverterFixture
    {
      [Theory]
      [InlineData(0)]
      [InlineData(1)]
      [InlineData(2)]
      [InlineData(3)]
      [InlineData(4)]
      [InlineData(5)]
      [InlineData(6)]
      [InlineData(7)]
      public void Should_Convert_Color_And_Opacity(int index)
      {
        var converter = new ColorOpacityConverter();

        var (value, opacity, expected) = _expectations[index];

        var actual = converter.Convert(value, typeof(Color), opacity, null);

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Throw_When_Unsupported_Color()
      {
        Invoking(() =>
          {
            var converter = new ColorOpacityConverter();

            converter.Convert(Create<int>(), typeof(Color), null, null);
          })
          .Should()
          .Throw<NotSupportedException>();
      }

      [Fact]
      public void Should_Throw_When_Unsupported_Opacity()
      {
        Invoking(() =>
          {
            var converter = new ColorOpacityConverter();

            converter.Convert(Create<Color>(), typeof(Color), Create<int>(), null);
          })
          .Should()
          .Throw<NotSupportedException>();
      }
    }

    public class ConvertBack : ColorOpacityConverterFixture
    {
      [Fact]
      public void Should_Throw_When_Convert_Back()
      {
        Invoking(() =>
          {
            var converter = new ColorOpacityConverter();
            converter.ConvertBack(null, null, null, null);
          })
          .Should()
          .Throw<NotImplementedException>();
      }
    }

    private static Color ConvertToColor(string hex, double alpha)
    {
      var color = Color.FromHex(hex);
      return Color.FromRgba(color.R, color.G, color.B, alpha);
    }
  }
}