namespace AllOverIt.XamarinForms.Validation.Rules
{
  /// <summary>
  /// An abstract base class for all validation rules.
  /// </summary>
  /// <typeparam name="TValue">The value type.</typeparam>
  public abstract class ValidationRuleBase<TValue> : IValidationRule<TValue>
  {
    public string ValidationMessage { get; }

    protected ValidationRuleBase(string validationMessage)
    {
      ValidationMessage = validationMessage;
    }

    public abstract bool IsSatisfiedBy(TValue value);
  }
}