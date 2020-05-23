using AllOverIt.Extensions;
using AllOverIt.XamarinForms.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Validation
{
  /// <summary>
  /// An observable object that is associated with one or more validation rules.
  /// </summary>
  /// <typeparam name="TType">The object type.</typeparam>
  public class ValidatableObject<TType> : ObservableObject, IValidatable
  {
    private readonly List<IValidationRule<TType>> _validations;
    private IReadOnlyList<string> _errors;
    private TType _value;
    private bool _isValid;

    /// <summary>
    /// Returns all validation errors reported when the object is not in a valid state.
    /// </summary>
    public IReadOnlyList<string> Errors
    {
      get => _errors;
      private set => SetValue(ref _errors, value, () =>
      {
        RaisePropertyChanged(nameof(FirstError));
      });
    }

    /// <summary>
    /// Returns the first error reported when the object is not in a valid state.
    /// </summary>
    public string FirstError => _errors.FirstOrDefault();

    /// <summary>
    /// The object's current value.
    /// </summary>
    public TType Value
    {
      get => _value;
      set => SetValue(ref _value, value);
    }

    /// <summary>
    /// Indicates if the object's current value is considered valid.
    /// </summary>
    public bool IsValid
    {
      get => _isValid;
      private set => SetValue(ref _isValid, value);
    }

    public ValidatableObject()
    {
      _errors = new List<string>();
      _validations = new List<IValidationRule<TType>>();
      _isValid = true;
    }

    /// <summary>
    /// Associates one or more validation rules with the object.
    /// </summary>
    /// <param name="rules">The rules to be associated with this object.</param>
    public void AddValidations(params IValidationRule<TType>[] rules)
    {
      _validations.AddRange(rules);
    }

    public bool Validate()
    {
      Errors = _validations
        .Where(rule => !rule.IsSatisfiedBy(_value))
        .Select(rule => rule.ValidationMessage)
        .AsReadOnlyList();

      IsValid = !Errors.Any();

      return IsValid;
    }

    private void SetValue<TPropertyType>(ref TPropertyType property, TPropertyType value, Action action = null, [CallerMemberName] string propertyName = null)
    {
      if (!EqualityComparer<TPropertyType>.Default.Equals(property, value))
      {
        property = value;

        if (propertyName == nameof(Value))
        {
          Validate();
        }

        RaisePropertyChanged(propertyName);

        action?.Invoke();
      }
    }
  }
}