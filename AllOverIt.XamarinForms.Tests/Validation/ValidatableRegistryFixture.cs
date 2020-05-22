using AllOverIt.XamarinForms.Validation;
using AllOverIt.XamarinForms.Validation.Rules;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Validation
{
  public class ValidatableRegistryFixture : AllOverItFixtureBase
  {
    public class IsValid : ValidatableRegistryFixture
    {
      [Fact]
      public void Should_Default_IsValid_True()
      {
        var subject = new ValidatableRegistry();

        var actual = subject.IsValid;

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Validate_True()
      {
        var subject = new ValidatableRegistry();
        var value = Create<int>();

        var model1 = subject.CreateValidatableObject(null, new IsGreaterThanRule<int>(value));
        var model2 = subject.CreateValidatableObject(null, new IsLessThanRule<int>(value + 3));

        model1.Value = value + 1;
        model2.Value = value + 2;

        var actual = subject.IsValid;

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Validate_False()
      {
        var subject = new ValidatableRegistry();
        var value = Create<int>();

        var model1 = subject.CreateValidatableObject(null, new IsGreaterThanRule<int>(value));
        var model2 = subject.CreateValidatableObject(null, new IsLessThanRule<int>(value + 3));

        model1.Value = value - 1;   // false
        model2.Value = value + 2;   // true

        var actual = subject.IsValid;

        actual.Should().BeFalse();
      }
    }

    public class ValidateAll : ValidatableRegistryFixture
    {
      [Fact]
      public void Should_Default_IsValid_True()
      {
        var subject = new ValidatableRegistry();

        var actual = subject.ValidateAll();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Validate_True()
      {
        var subject = new ValidatableRegistry();
        var value = Create<int>();

        var model1 = subject.CreateValidatableObject(null, new IsGreaterThanRule<int>(value));
        var model2 = subject.CreateValidatableObject(null, new IsLessThanRule<int>(value + 3));

        model1.Value = value + 1;
        model2.Value = value + 2;

        var actual = subject.ValidateAll();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Validate_False()
      {
        var subject = new ValidatableRegistry();
        var value = Create<int>();

        var model1 = subject.CreateValidatableObject(null, new IsGreaterThanRule<int>(value));
        var model2 = subject.CreateValidatableObject(null, new IsLessThanRule<int>(value + 3));

        model1.Value = value - 1;   // false
        model2.Value = value + 2;   // true

        var actual = subject.ValidateAll();

        actual.Should().BeFalse();
      }
    }

    public class CreateValidatableObject : ValidatableRegistryFixture
    {
      [Fact]
      public void Should_Not_Throw_When_Action_Null()
      {
        var subject = new ValidatableRegistry();

        Invoking(() =>
          {
            subject.CreateValidatableObject(null, new IsGreaterThanRule<int>(Create<int>()));
          })
          .Should()
          .NotThrow();
      }

      [Fact]
      public void Should_Call_Action_When_Property_Changed()
      {
        var subject = new ValidatableRegistry();
        int actual = 0;

        var model1 = subject.CreateValidatableObject(model => actual = model.Value, new IsGreaterThanRule<int>(Create<int>()));

        model1.Value = Create<int>(); 

        actual.Should().Be(model1.Value);
      }

      [Fact]
      public void Should_Add_Validations()
      {
        var subject = new ValidatableRegistry();
        var value = Create<int>();

        var model1 = subject.CreateValidatableObject(null, new IsGreaterThanRule<int>(value));
        var model2 = subject.CreateValidatableObject(null, new IsLessThanRule<int>(value + 3));

        model1.Value = value + 1;
        model2.Value = value + 2;

        var actual = subject.ValidateAll();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Return_Validatable_Object()
      {
        var subject = new ValidatableRegistry();

        var model1 = subject.CreateValidatableObject(null, new IsGreaterThanRule<int>(Create<int>()));

        model1.Should().BeOfType<ValidatableObject<int>>();
      }
    }
  }
}