using AllOverIt.Fixture;
using AllOverIt.XamarinForms.Tests.Scaffolding;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Tests
{
  public class AllOverItFixtureBase : AoiFixtureBase
  {
    internal static string GetExpectedArgumentNullExceptionMessage(string name)
    {
      return $"Value cannot be null. (Parameter '{name}')";
    }

    internal static string GetExpectedArgumentEmptyExceptionMessage(string name)
    {
      return $"The argument cannot be empty (Parameter '{name}')";
    }

    // Based on: https://brianlagunas.com/mocking-and-unit-testing-the-xamarin-forms-application-class/
    //
    // An alternative is to use Xamarin.Forms.Mocks
    // http://jonathanpeppers.com/Blog/mocking-xamarin-forms
    // https://github.com/jonathanpeppers/Xamarin.Forms.Mocks/
    //
    protected static void InitFormsFixture()
    {
      Device.Info = new MockDeviceInfo();
      Device.PlatformServices = new MockPlatformServices();

      DependencyService.Register<MockResourcesProvider>();
      DependencyService.Register<MockDeserializer>();

      Application.Current = new Application();
    }
  }
}