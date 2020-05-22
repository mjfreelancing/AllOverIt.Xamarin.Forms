using System;

namespace AllOverIt.XamarinForms.Validation.Rules
{
  public class IsGreaterThanRule<TValue> : ValidationRuleBase<TValue>
    where TValue : IComparable
  {
    private readonly TValue _value;

    public IsGreaterThanRule(TValue value)
      : this(value, $"Value must be greater than {value}")
    {
    }

    public IsGreaterThanRule(TValue value, string validationMessage)
      : base(validationMessage)
    {
      _value = value;
    }

    public override bool IsSatisfiedBy(TValue value)
    {
      return value.CompareTo(_value) > 0;
    }
  }
}