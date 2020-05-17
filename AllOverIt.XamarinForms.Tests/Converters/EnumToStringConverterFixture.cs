using AllOverIt.XamarinForms.Converters;
using FluentAssertions;
using System.Globalization;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Converters
{
  public class EnumToStringConverterFixture : AllOverItFixtureBase
  {
    public enum DummyEnum
    {
      FirstValue,
      SecondValue
    }

    public class Convert : EnumToStringConverterFixture
    {
      [Theory]
      [InlineData(DummyEnum.FirstValue, "First Value")]
      [InlineData(DummyEnum.SecondValue, "Second Value")]
      public void Should_Convert_Split_Words(DummyEnum enumValue, string expected)
      {
        var converter = new EnumToStringConverter<DummyEnum> {SplitWords = true};

        var actual = converter.Convert(enumValue, typeof(string), null, CultureInfo.InvariantCulture);

        actual.Should().Be(expected);
      }

      [Theory]
      [InlineData(DummyEnum.FirstValue, "FirstValue")]
      [InlineData(DummyEnum.SecondValue, "SecondValue")]
      public void Should_Convert_No_Split_Words(DummyEnum enumValue, string expected)
      {
        var converter = new EnumToStringConverter<DummyEnum> { SplitWords = false };

        var actual = converter.Convert(enumValue, typeof(string), null, CultureInfo.InvariantCulture);

        actual.Should().Be(expected);
      }
    }

    public class ConvertBack : EnumToStringConverterFixture
    {
      [Theory]
      [InlineData("FirstValue", DummyEnum.FirstValue)]
      [InlineData("Second Value", DummyEnum.SecondValue)]
      public void Should_Convert_Back(string enumValue, DummyEnum expected)
      {
        var converter = new EnumToStringConverter<DummyEnum> { SplitWords = Create<bool>() };

        var actual = converter.ConvertBack(enumValue, typeof(DummyEnum), null, CultureInfo.InvariantCulture);

        actual.Should().Be(expected);
      }
    }
  }
}