using System;

namespace AllOverIt.XamarinForms.Validation.Rules
{
  /// <summary>
  /// Represents a validation rule to check that a value is less than a known value.
  /// </summary>
  /// <typeparam name="TValue">The value to be compared to.</typeparam>
  public class IsLessThanRule<TValue> : ValidationRuleBase<TValue>
    where TValue : IComparable
  {
    private readonly TValue _value;

    public IsLessThanRule(TValue value)
      : this(value, $"Value must be less than {value}")
    {
    }

    public IsLessThanRule(TValue value, string validationMessage)
      : base(validationMessage)
    {
      _value = value;
    }

    public override bool IsSatisfiedBy(TValue value)
    {
      return value.CompareTo(_value) < 0;
    }
  }
}