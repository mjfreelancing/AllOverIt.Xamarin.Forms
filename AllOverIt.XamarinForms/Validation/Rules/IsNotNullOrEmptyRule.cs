namespace AllOverIt.XamarinForms.Validation.Rules
{
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