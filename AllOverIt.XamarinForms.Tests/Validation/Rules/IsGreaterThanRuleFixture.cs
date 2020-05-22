using AllOverIt.XamarinForms.Validation.Rules;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Validation.Rules
{
  public class IsGreaterThanRuleFixture : AllOverItFixtureBase
  {
    [Fact]
    public void Should_Set_Validation_Message()
    {
      var value = Create<int>();

      var subject = new IsGreaterThanRule<int>(value);

      subject.ValidationMessage.Should().Be($"Value must be greater than {value}");
    }

    [Fact]
    public void Should_Set_Custom_Validation_Message()
    {
      var value = Create<int>();
      var expected = $"{Create<string>()}{value}";

      var subject = new IsGreaterThanRule<int>(value, expected);

      subject.ValidationMessage.Should().Be(expected);
    }

    [Theory]
    [InlineData(10, true)]
    [InlineData(15, false)]
    [InlineData(20, false)]
    public void Should_Satisfy_Rule(int value, bool expected)
    {
      var subject = new IsGreaterThanRule<int>(value);

      var actual = subject.IsSatisfiedBy(15);

      actual.Should().Be(expected);
    }
  }
}