namespace AllOverIt.XamarinForms.Validation.Rules
{
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