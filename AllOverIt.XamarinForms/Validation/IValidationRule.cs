namespace AllOverIt.XamarinForms.Validation
{
  /// <summary>
  /// Represents a single validation rule.
  /// </summary>
  /// <typeparam name="TType">The object type.</typeparam>
  public interface IValidationRule<in TType>
  {
    /// <summary>
    /// The message provided when the object is not in a valid state.
    /// </summary>
    string ValidationMessage { get; }

    /// <summary>
    /// Validates the object based on the provided <see cref="value"/>.
    /// </summary>
    /// <param name="value">The value to validate the object against.</param>
    /// <returns>True of the object is considered true, otherwise false.</returns>
    bool IsSatisfiedBy(TType value);
  }
}