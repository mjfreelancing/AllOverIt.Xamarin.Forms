using AllOverIt.XamarinForms.Converters;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Converters
{
  public class ItemsSourceNullConverterFixture : AllOverItFixtureBase
  {
    public class Convert : ItemsSourceNullConverterFixture
    {
      [Fact]
      public void Should_Return_True_When_Null()
      {
        var converter = new ItemsSourceNullConverter();

        var actual = (bool)converter.Convert(null, typeof(bool), null, null);

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Return_False_When_Not_Null()
      {
        var converter = new ItemsSourceNullConverter();

        var actual = (bool)converter.Convert(Enumerable.Empty<string>(), typeof(bool), null, null);

        actual.Should().BeFalse();
      }
    }

    public class ConvertBack : ItemsSourceNullConverterFixture
    {
      [Fact]
      public void Should_Throw_When_Convert_Back()
      {
        Invoking(() =>
          {
            var converter = new ItemsSourceNullConverter();
            converter.ConvertBack(null, null, null, null);
          })
          .Should()
          .Throw<NotImplementedException>();
      }
    }
  }
}