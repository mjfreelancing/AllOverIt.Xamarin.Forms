using System;

namespace AllOverIt.XamarinForms.Validation
{
  /// <summary>
  /// Represents a registry of objects that can be validated against a set of rules.
  /// </summary>
  public interface IValidatableRegistry : IValidity
  {
    /// <summary>
    /// Explicitly validates all objects maintained by the registry.
    /// </summary>
    /// <returns>True if all objects are in a valid state, otherwise false.</returns>
    bool ValidateAll();

    /// <summary>
    /// Creates an object that can be automatically validated when its' value changes.
    /// </summary>
    /// <typeparam name="TType">The type of the object.</typeparam>
    /// <param name="onPropertyChanged">An action to invoke when the value of the object changes.</param>
    /// <param name="rules">One or more rules that must all return true for the object to be considered in a valid state.</param>
    /// <returns>An object that can be automatically validated when its' value changes. The registry maintains a strong reference to this object.</returns>
    ValidatableObject<TType> CreateValidatableObject<TType>(Action<ValidatableObject<TType>> onPropertyChanged, params IValidationRule<TType>[] rules);
  }
}