using System;

namespace AllOverIt.XamarinForms.Validation.Rules
{
  public class IsLessThanOrEqualRule<TValue> : ValidationRuleBase<TValue>
    where TValue : IComparable
  {
    private readonly Func<TValue> _selector;

    public IsLessThanOrEqualRule(Func<TValue> selector, string validationMessage)
      : base(validationMessage)
    {
      _selector = selector;
    }

    public override bool IsSatisfiedBy(TValue value)
    {
      var modelValue = _selector.Invoke();
      return value.CompareTo(modelValue) <= 0;
    }
  }
}