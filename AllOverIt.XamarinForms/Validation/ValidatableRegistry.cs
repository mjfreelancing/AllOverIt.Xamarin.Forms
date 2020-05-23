using System;
using System.Collections.Generic;
using System.Linq;

namespace AllOverIt.XamarinForms.Validation
{
  /// <summary>
  /// Maintains a registry of objects that can be validated against the rules they are associated with.
  /// </summary>
  public class ValidatableRegistry : IValidatableRegistry
  {
    private readonly List<IValidatable> _validatables;

    public bool IsValid => _validatables.All(item => item.IsValid);

    public ValidatableRegistry()
    {
      _validatables = new List<IValidatable>();
    }

    public bool ValidateAll()
    {
      foreach (var validatable in _validatables)
      {
        validatable.Validate();
      }

      return IsValid;
    }

    public ValidatableObject<TType> CreateValidatableObject<TType>(Action<ValidatableObject<TType>> onPropertyChanged, params IValidationRule<TType>[] rules)
    {
      var property = new ValidatableObject<TType>();

      if (onPropertyChanged != null)
      {
        property.PropertyChanged += (sender, args) =>
        {
          if (args.PropertyName == nameof(property.Value))
          {
            onPropertyChanged.Invoke(property);
          }
        };
      }

      property.AddValidations(rules);

      _validatables.Add(property);

      return property;
    }
  }
}