namespace AllOverIt.XamarinForms.Validation
{
  /// <summary>
  /// Represents the state of an object as valid or not.
  /// </summary>
  public interface IValidity
  {
    bool IsValid { get; }
  }
}