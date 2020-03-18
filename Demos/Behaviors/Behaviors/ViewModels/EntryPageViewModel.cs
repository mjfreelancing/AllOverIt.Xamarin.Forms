using AllOverIt.XamarinForms.Mvvm;
using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public class EntryPageViewModel : ViewModelBase
  {
    public ICommand PageAppearingCommand { get; }
    public ICommand PageDisappearingCommand { get; }

    public double Amount { get; set; }

    public EntryPageViewModel()
    {
      PageAppearingCommand = new Command(OnPageAppearingCommand);
      PageDisappearingCommand = new Command(OnPageDisappearingCommand);
    }

    private void OnPageAppearingCommand()
    {
      // todo: bring in dialog service
      Application.Current.MainPage.DisplayAlert("Appearing Event", "The Entry page has appeared", "Ok");
    }

    private void OnPageDisappearingCommand()
    {
      // todo: bring in dialog service
      Application.Current.MainPage.DisplayAlert("Appearing Event", "The Entry page has disappeared", "Ok");
    }
  }
}