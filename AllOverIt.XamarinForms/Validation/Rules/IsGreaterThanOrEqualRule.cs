using System;

namespace AllOverIt.XamarinForms.Validation.Rules
{
  /// <summary>
  /// Represents a validation rule to check that a value is greater than or equal to a known value.
  /// </summary>
  /// <typeparam name="TValue">The value to be compared to.</typeparam>
  public class IsGreaterThanOrEqualRule<TValue> : ValidationRuleBase<TValue>
    where TValue : IComparable
  {
    private readonly TValue _value;

    public IsGreaterThanOrEqualRule(TValue value)
      : this(value, $"Value must be greater than or equal to {value}")
    {
    }

    public IsGreaterThanOrEqualRule(TValue value, string validationMessage)
      : base(validationMessage)
    {
      _value = value;
    }

    public override bool IsSatisfiedBy(TValue value)
    {
      return value.CompareTo(_value) >= 0;
    }
  }
}