using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOverIt.XamarinForms.Commands
{
  public interface IAsyncCommand : ICommand
  {
    Task ExecuteAsync();
    bool CanExecute();
  }

  public interface IAsyncCommand<in TType> : ICommand
  {
    Task ExecuteAsync(TType parameter);
    bool CanExecute(TType parameter);
  }
}