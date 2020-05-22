using AllOverIt.XamarinForms.Validation;
using AllOverIt.XamarinForms.Validation.Rules;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Validation
{
  public class ValidatableObjectFixture : AllOverItFixtureBase
  {
    public class Constructor : ValidatableObjectFixture
    {
      [Fact]
      public void Should_Have_Default_No_errors()
      {
        var subject = new ValidatableObject<int>();

        var actual = subject.Errors;

        actual.Should().BeEmpty();
      }

      [Fact]
      public void Should_Have_Default_Is_Valid_True()
      {
        var subject = new ValidatableObject<int>();

        var actual = subject.IsValid;

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Have_Default_First_Error_Null()
      {
        var subject = new ValidatableObject<int>();

        var actual = subject.FirstError;

        actual.Should().BeNull();
      }

      [Fact]
      public void Should_Have_Default_Value()
      {
        var subject = new ValidatableObject<int>();

        var actual = subject.Value;

        actual.Should().Be(default);
      }

      [Fact]
      public void Should_Default_Validate_True()
      {
        var subject = new ValidatableObject<int>();

        var actual = subject.Validate();

        actual.Should().BeTrue();
      }
    }

    public class AddValidations : ValidatableObjectFixture
    {
      [Theory]
      [InlineData(1, true)]
      [InlineData(10, false)]
      public void Should_Add_Validations(int increment, bool expected)
      {
        var subject = new ValidatableObject<int>();
        var value = Create<int>();

        subject.AddValidations
        (
          new IsGreaterThanRule<int>(value),
          new IsLessThanRule<int>(value + 3)
        );

        subject.Value = value + increment;

        var actual = subject.IsValid;

        actual.Should().Be(expected);
      }
    }

    public class Validate : ValidatableObjectFixture
    {
      [Theory]
      [InlineData(1, true)]
      [InlineData(10, false)]
      public void Should_Validate_Using_Validate(int increment, bool expected)
      {
        var subject = new ValidatableObject<int>();
        var value = Create<int>();

        subject.AddValidations
        (
          new IsGreaterThanRule<int>(value),
          new IsLessThanRule<int>(value + 3)
        );

        subject.Value = value + increment;

        var actual = subject.Validate();

        actual.Should().Be(expected);
      }
    }

    public class SetValue : ValidatableObjectFixture
    {
      private class DummyValidatable : ValidatableObject<int>
      {
      }

      [Fact]
      public void Should_Set_Value()
      {
        var subject = new DummyValidatable();

        var value = Create<int>();
        subject.Value = value;

        var actual = subject.Value;

        actual.Should().Be(value);
      }

      [Fact]
      public void Should_Raise_Property_Change_Event()
      {
        var subject = new DummyValidatable();
        string actual = null;

        subject.PropertyChanged += (sender, args) =>
        {
          actual = args.PropertyName;
        };

        subject.Value = Create<int>();

        actual.Should().Be(nameof(DummyValidatable.Value));
      }

      [Fact]
      public void Should_Not_Raise_Property_Change_Event()
      {
        var subject = new DummyValidatable();
        string actual = null;

        subject.PropertyChanged += (sender, args) =>
        {
          actual = args.PropertyName;
        };

        var value = Create<int>();
        subject.Value = value;
        subject.Value = value;
        actual = null;

        actual.Should().BeNull();
      }

      [Fact]
      public void Should_Invoke_Action()
      {
        var subject = new ValidatableObject<int>();
        var value = Create<int>();
        var errorsPropertyChangeRaised = false;

        subject.PropertyChanged += (sender, args) =>
        {
          // A change notification for Errors occurs via the Action on SetValue
          if (args.PropertyName == nameof(ValidatableObject<int>.Errors))
          {
            errorsPropertyChangeRaised = true;
          }
        };

        subject.AddValidations(new IsGreaterThanRule<int>(value));

        subject.Value = value - 1;

        errorsPropertyChangeRaised.Should().BeTrue();
      }
    }
  }
}