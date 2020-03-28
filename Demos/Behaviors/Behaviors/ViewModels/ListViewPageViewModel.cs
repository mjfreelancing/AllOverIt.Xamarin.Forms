using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public class ListViewPageViewModel : BehaviorViewModelBase
  {
    public ICommand SelectedItemCommand { get; }        // see ListViewPage, used with behavior options 1 and 2
    public ICommand SelectedItemTextCommand { get; }    // see ListViewPage, used with behavior option 3

    private string _selectedItemText;
    public string SelectedItemText
    {
      get => _selectedItemText;
      set => SetProperty(ref _selectedItemText, value);
    }

    private int _selectedItemIndex;
    public int SelectedItemIndex
    {
      get => _selectedItemIndex;
      set => SetProperty(ref _selectedItemIndex, value);
    }

    public ListViewPageViewModel()
    {
      Logger.Debug($"Initialising {nameof(ListViewPageViewModel)}");

      // used with behavior options 1 and 2
      SelectedItemCommand = new Command<SelectedItemChangedEventArgs>(OnSelectedItemCommand);

      // used with behavior option 3
      SelectedItemTextCommand = new Command<string>(OnSelectedItemTextCommand);
      
      Logger.Debug($"Initialised {nameof(ListViewPageViewModel)}");
    }

    private void OnSelectedItemCommand(SelectedItemChangedEventArgs args)
    {
      Logger.Debug($"The {nameof(OnSelectedItemCommand)} has been invoked");

      // ListViewPage includes a 'ClearListViewSelectedItemBehavior' - don't clear out the selected info when
      // this behavior clears the current selection.
      if (args.SelectedItem == null)
      {
        Logger.Debug("The list view selected item is now cleared");
        return;
      }

      SelectedItemText = (string)args.SelectedItem;
      SelectedItemIndex = args.SelectedItemIndex;

      Logger.Debug($"The list view selected item is '{SelectedItemText}' at index {SelectedItemIndex}");
    }

    private void OnSelectedItemTextCommand(string selectedItemText)
    {
      SelectedItemText = selectedItemText;
      //SelectedItemIndex is not available when using behavior option 3

      Logger.Debug($"The {nameof(OnSelectedItemTextCommand)} has been invoked");
    }
  }
}