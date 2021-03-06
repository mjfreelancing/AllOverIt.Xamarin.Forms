﻿using System;

namespace AllOverIt.XamarinForms.Validation.Rules
{
  /// <summary>
  /// Represents a validation rule to check that a value is less than or equal to a known value.
  /// </summary>
  /// <typeparam name="TValue">The value to be compared to.</typeparam>
  public class IsLessThanOrEqualRule<TValue> : ValidationRuleBase<TValue>
    where TValue : IComparable
  {
    private readonly TValue _value;

    public IsLessThanOrEqualRule(TValue value)
      : this(value, $"Value must be less than or equal to {value}")
    {
    }

    public IsLessThanOrEqualRule(TValue value, string validationMessage)
      : base(validationMessage)
    {
      _value = value;
    }

    public override bool IsSatisfiedBy(TValue value)
    {
      return value.CompareTo(_value) <= 0;
    }
  }
}