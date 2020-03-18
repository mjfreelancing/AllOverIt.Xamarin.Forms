using AllOverIt.XamarinForms.Mvvm;
using System.Windows.Input;
using Xamarin.Forms;

namespace Behaviors.ViewModels
{
  public class ListViewPageViewModel : ViewModelBase
  {
    public ICommand ItemSelectedCommand { get; }        // see ListViewPage, used with behavior options 1 and 2
    public ICommand ItemSelectedTextCommand { get; }    // see ListViewPage, used with behavior option 3

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
      // used with behavior options 1 and 2
      ItemSelectedCommand = new Command<SelectedItemChangedEventArgs>(OnItemSelectedCommand);

      // used with behavior option 3
      ItemSelectedTextCommand = new Command<string>(OnItemSelectedTextCommand);
    }

    private void OnItemSelectedCommand(SelectedItemChangedEventArgs args)
    {
      SelectedItemText = (string)args.SelectedItem;
      SelectedItemIndex = args.SelectedItemIndex;
    }

    private void OnItemSelectedTextCommand(string selectedItemText)
    {
      SelectedItemText = selectedItemText;
      //SelectedItemIndex is not available when using behavior option 3
    }
  }
}