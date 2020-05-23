using System.Threading.Tasks;
using System.Windows.Input;

namespace AllOverIt.XamarinForms.Commands
{
  /// <summary>
  /// Describes an asynchronous command.
  /// </summary>
  public interface IAsyncCommand : ICommand
  {
    /// <summary>
    /// Determines if the command can be executed.
    /// </summary>
    bool CanExecute();

    /// <summary>
    /// The command handler to execute when the command is invoked.
    /// </summary>
    /// <returns>True if the command completes, otherwise false.</returns>
    Task ExecuteAsync();
  }

  /// <summary>
  /// Describes a strongly typed asynchronous command.
  /// </summary>
  public interface IAsyncCommand<in TType> : ICommand
  {
    /// <summary>
    /// Determines if the command can be executed with the provided parameter.
    /// </summary>
    /// <param name="parameter">The parameter that will be passed to the <see cref="ExecuteAsync"/> method.</param>
    bool CanExecute(TType parameter);

    /// <summary>
    /// The command handler to execute when the command is invoked.
    /// </summary>
    /// <param name="parameter">The parameter provided to the handler.</param>
    /// <returns>True if the command completes, otherwise false.</returns>
    Task ExecuteAsync(TType parameter);
  }
}