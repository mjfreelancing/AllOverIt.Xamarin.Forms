using AllOverIt.XamarinForms.Mvvm;
using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public class ControlsPageViewModel : ViewModelBase
  {
    private bool _lookBusy;

    public bool LookBusy
    {
      get => _lookBusy;
      set => SetProperty(ref _lookBusy, value);
    }

    public ICommand EntryFocusedCommand { get; }
    public ICommand EntryUnfocusedCommand { get; }
    public ICommand EntryTextChangedCommand { get; }

    public ControlsPageViewModel()
    {
      EntryFocusedCommand = new Command<FocusEventArgs>(OnEntryFocused);
      EntryUnfocusedCommand = new Command<FocusEventArgs>(OnEntryUnfocused);
      EntryTextChangedCommand = new Command<TextChangedEventArgs>(OnEntryTextChanged);
    }

    // ControlsPage has a behaviour that calls this command when the EntryFocused event is raised
    private void OnEntryFocused(FocusEventArgs obj)
    {
      // not doing anything in this demo
    }

    // ControlsPage has a behaviour that calls this command when the EntryUnfocused event is raised
    private void OnEntryUnfocused(FocusEventArgs obj)
    {
      // not doing anything in this demo
    }

    // ControlsPage has a behaviour that calls this command when the EntryTextChanged event is raised
    private void OnEntryTextChanged(TextChangedEventArgs obj)
    {
      var currentText = obj.NewTextValue.ToLower();

      LookBusy = currentText == "alloverit";
    }
  }
}