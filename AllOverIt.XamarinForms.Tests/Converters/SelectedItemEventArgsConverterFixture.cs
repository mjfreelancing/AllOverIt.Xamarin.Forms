using AllOverIt.XamarinForms.Converters;
using FluentAssertions;
using System;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Converters
{
  public class SelectedItemEventArgsConverterFixture : AllOverItFixtureBase
  {
    public class Convert : SelectedItemEventArgsConverterFixture
    {
      [Fact]
      public void Should_Return_Null_When_Null()
      {
        var converter = new SelectedItemEventArgsConverter();

        var actual = converter.Convert(null, typeof(bool), null, null);

        actual.Should().BeNull();
      }

      [Fact]
      public void Should_Return_Selected_Item()
      {
        var expected = Create<string>();
        var selectedArgs = new SelectedItemChangedEventArgs(expected, 0);

        var converter = new SelectedItemEventArgsConverter();

        var actual = (string)converter.Convert(selectedArgs, typeof(string), null, null);

        actual.Should().Be(expected);
      }
    }

    public class ConvertBack : SelectedItemEventArgsConverterFixture
    {
      [Fact]
      public void Should_Throw_When_Convert_Back()
      {
        Invoking(() =>
          {
            var converter = new SelectedItemEventArgsConverter();
            converter.ConvertBack(null, null, null, null);
          })
          .Should()
          .Throw<NotImplementedException>();
      }
    }
  }
}