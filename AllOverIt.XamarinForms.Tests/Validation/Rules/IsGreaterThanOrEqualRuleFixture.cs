using AllOverIt.XamarinForms.Validation.Rules;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Validation.Rules
{
  public class IsGreaterThanOrEqualRuleFixture : AllOverItFixtureBase
  {
    [Fact]
    public void Should_Set_Validation_Message()
    {
      var value = Create<int>();

      var subject = new IsGreaterThanOrEqualRule<int>(value);

      subject.ValidationMessage.Should().Be($"Value must be greater than or equal to {value}");
    }

    [Fact]
    public void Should_Set_Custom_Validation_Message()
    {
      var value = Create<int>();
      var expected = $"{Create<string>()}{value}";

      var subject = new IsGreaterThanOrEqualRule<int>(value, expected);

      subject.ValidationMessage.Should().Be(expected);
    }

    [Theory]
    [InlineData(10, true)]
    [InlineData(15, true)]
    [InlineData(20, false)]
    public void Should_Satisfy_Rule(int value, bool expected)
    {
      var subject = new IsGreaterThanOrEqualRule<int>(value);

      var actual = subject.IsSatisfiedBy(15);

      actual.Should().Be(expected);
    }
  }
}