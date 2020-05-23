using System.Linq;
using System.Reflection;

namespace AllOverIt.XamarinForms.Tests
{
  public static class ReflectionHelpers
  {
    public static TElement GetNonPublicFieldFromControl<TElement>(object control, string name)
    {
      var fieldInfo = control.GetType()
        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
        .Single(item => item.Name == name);

      return (TElement)fieldInfo.GetValue(control);
    }
  }
}