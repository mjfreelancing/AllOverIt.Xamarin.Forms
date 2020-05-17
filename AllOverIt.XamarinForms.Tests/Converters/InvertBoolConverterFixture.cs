using AllOverIt.XamarinForms.Converters;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Converters
{
  public class InvertBoolConverterFixture : AllOverItFixtureBase
  {
    public class Convert : InvertBoolConverterFixture
    {
      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Convert_As_Inverted(bool value)
      {
        var expected = !value;

        var converter = new InvertBoolConverter();

        var actual = converter.Convert(value, typeof(bool), null, null);

        actual.Should().Be(expected);
      }
    }

    public class ConvertBack : InvertBoolConverterFixture
    {
      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_ConvertBack_As_Inverted(bool value)
      {
        var expected = !value;

        var converter = new InvertBoolConverter();

        var actual = converter.ConvertBack(value, typeof(bool), null, null);

        actual.Should().Be(expected);
      }
    }
  }
}