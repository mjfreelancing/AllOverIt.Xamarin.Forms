using AllOverIt.XamarinForms.Validation.Rules;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Validation.Rules
{
  public class IsNotNullOrEmptyRuleFixture : AllOverItFixtureBase
  {
    [Fact]
    public void Should_Set_Validation_Message()
    {
      var subject = new IsNotNullOrEmptyRule();

      subject.ValidationMessage.Should().Be("Value cannot be empty");
    }

    [Fact]
    public void Should_Set_Custom_Validation_Message()
    {
      var expected = Create<string>();
      var subject = new IsNotNullOrEmptyRule(expected);

      subject.ValidationMessage.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("xyz", true)]
    public void Should_Satisfy_Rule(string value, bool expected)
    {
      var subject = new IsNotNullOrEmptyRule();

      var actual = subject.IsSatisfiedBy(value);

      actual.Should().Be(expected);
    }
  }
}