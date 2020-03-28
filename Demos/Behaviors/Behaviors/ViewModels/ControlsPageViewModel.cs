using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public class ControlsPageViewModel : BehaviorViewModelBase
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
      Logger.Debug($"Initialising {nameof(ControlsPageViewModel)}");

      EntryFocusedCommand = new Command<FocusEventArgs>(OnEntryFocused);
      EntryUnfocusedCommand = new Command<FocusEventArgs>(OnEntryUnfocused);
      EntryTextChangedCommand = new Command<TextChangedEventArgs>(OnEntryTextChanged);

      Logger.Debug($"{nameof(ControlsPageViewModel)} is initialised");
    }

    // ControlsPage has a behaviour that calls this command when the EntryFocused event is raised
    private void OnEntryFocused(FocusEventArgs args)
    {
      Logger.Debug("An Entry control has received focus");
    }

    // ControlsPage has a behaviour that calls this command when the EntryUnfocused event is raised
    private void OnEntryUnfocused(FocusEventArgs args)
    {
      Logger.Debug("An Entry control has lost focus");
    }

    // ControlsPage has a behaviour that calls this command when the EntryTextChanged event is raised
    private void OnEntryTextChanged(TextChangedEventArgs args)
    {
      var currentText = args.NewTextValue.ToLower();

      LookBusy = currentText == "alloverit";

      Logger.Debug($"Current Entry text = '{currentText}', looking busy = {_lookBusy}");
    }
  }
}