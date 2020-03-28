using AllOverIt.XamarinForms.Logging;
using Xamarin.Forms;

namespace Behaviors
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      DependencyService.Register<ILogger, BehaviorLogger>();

      MainPage = new AppShell();
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
  }
}
