using System;

namespace AllOverIt.XamarinForms.Validation
{
  public interface IValidatableRegistry : IValidity
  {
    bool ValidateAll();
    ValidatableObject<TType> CreateValidatableObject<TType>(Action<ValidatableObject<TType>> onPropertyChanged, params IValidationRule<TType>[] rules);
  }
}