using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public class EntryPageViewModel : BehaviorViewModelBase
  {
    public ICommand PageAppearingCommand { get; }
    public ICommand PageDisappearingCommand { get; }

    public double Amount { get; set; }

    public EntryPageViewModel()
    {
      Logger.Debug($"Initialising {nameof(EntryPageViewModel)}");

      PageAppearingCommand = new Command(OnPageAppearingCommand);
      PageDisappearingCommand = new Command(OnPageDisappearingCommand);

      Logger.Debug($"Initialised {nameof(EntryPageViewModel)}");
    }

    private void OnPageAppearingCommand()
    {
      Logger.Debug("The EntryPage has appeared");

      // todo: bring in dialog service
      Application.Current.MainPage.DisplayAlert("Appearing Event", "The Entry page has appeared", "Ok");
    }

    private void OnPageDisappearingCommand()
    {
      Logger.Debug("The EntryPage is disappearing");

      // todo: bring in dialog service
      Application.Current.MainPage.DisplayAlert("Appearing Event", "The Entry page has disappeared", "Ok");
    }
  }
}