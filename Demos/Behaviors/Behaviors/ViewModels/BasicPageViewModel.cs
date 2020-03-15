using AllOverIt.XamarinForms.Mvvm;
using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public class BasicPageViewModel : ViewModelBase
  {
    public ICommand PageAppearingCommand { get; }

    public BasicPageViewModel()
    {
      PageAppearingCommand = new Command(OnPageAppearingCommand);
    }

    private void OnPageAppearingCommand()
    {
      // todo: bring in dialog service
      Application.Current.MainPage.DisplayAlert("Appearing Event", "This event was called in response to a page appearing", "Ok");
    }
  }
}