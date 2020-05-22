using AllOverIt.Extensions;
using AllOverIt.XamarinForms.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AllOverIt.XamarinForms.Validation
{
  public class ValidatableObject<TType> : ObservableObject, IValidatable
  {
    private readonly List<IValidationRule<TType>> _validations;
    private IReadOnlyList<string> _errors;
    private TType _value;
    private bool _isValid;

    public IReadOnlyList<string> Errors
    {
      get => _errors;
      private set => SetValue(ref _errors, value, () =>
      {
        RaisePropertyChanged(nameof(FirstError));
      });
    }

    public string FirstError => _errors.FirstOrDefault();

    public TType Value
    {
      get => _value;
      set => SetValue(ref _value, value);
    }

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