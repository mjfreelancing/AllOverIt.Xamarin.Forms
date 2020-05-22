using AllOverIt.XamarinForms.Validation.Rules;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Validation.Rules
{
  public class ValidationRuleBaseFixture : AllOverItFixtureBase
  {
    private class DummyValidationRule : ValidationRuleBase<int>
    {
      public DummyValidationRule(string validationMessage) : base(validationMessage)
      {
      }

      public override bool IsSatisfiedBy(int value)
      {
        return value != 0;
      }
    }

    [Fact]
    public void Should_Set_Validation_Message()
    {
      var expected = Create<string>();

      var subject = new DummyValidationRule(expected);

      subject.ValidationMessage.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    public void Should_Satisfy_Rule(int value, bool expected)
    {
      var subject = new DummyValidationRule(Create<string>());

      var actual = subject.IsSatisfiedBy(value);

      actual.Should().Be(expected);
    }
  }
}