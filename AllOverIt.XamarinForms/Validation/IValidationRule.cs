namespace AllOverIt.XamarinForms.Validation
{
  public interface IValidationRule<in TType>
  {
    string ValidationMessage { get; }

    bool IsSatisfiedBy(TType value);
  }
}