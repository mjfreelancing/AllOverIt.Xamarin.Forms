using System.Threading.Tasks;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  public abstract class BehaviorAction
    : BindableObject, IBehaviorAction
  {
    public abstract Task<bool> Execute(object sender, object parameter);
  }
}