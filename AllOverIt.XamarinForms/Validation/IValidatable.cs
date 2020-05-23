namespace AllOverIt.XamarinForms.Validation
{
  /// <summary>
  /// Represents an object that can be validated against one or more rules.
  /// </summary>
  public interface IValidatable : IValidity
  {
    /// <summary>
    /// Validates the object against all associated rules.
    /// </summary>
    /// <returns>True if all rules indicate the object is valid, otherwise false.</returns>
    bool Validate();
  }
}