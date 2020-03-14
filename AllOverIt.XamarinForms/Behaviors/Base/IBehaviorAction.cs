using System.Threading.Tasks;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  public interface IBehaviorAction
  {
    Task<bool> Execute(object sender, object parameter);
  }
}