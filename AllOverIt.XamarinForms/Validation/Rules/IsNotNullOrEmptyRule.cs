namespace AllOverIt.XamarinForms.Validation.Rules
{
  /// <summary>
  /// Represents a validation rule to validate a string value is not null or whitespace.
  /// </summary>
  public class IsNotNullOrEmptyRule : ValidationRuleBase<string>
  {
    public IsNotNullOrEmptyRule() 
      : this("Value cannot be empty")
    {
    }

    public IsNotNullOrEmptyRule(string validationMessage)
      : base(validationMessage)
    {
    }

    public override bool IsSatisfiedBy(string value)
    {
      return !string.IsNullOrWhiteSpace(value);
    }
  }
}