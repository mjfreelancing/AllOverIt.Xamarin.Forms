using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  [ContentProperty("Actions")]
  public class BehaviorActionsBase : AttachableBehavior<BehaviorActionsBase, VisualElement>
  {
    public static readonly BindableProperty ActionsProperty = BindableProperty.Create(nameof(Actions), typeof(BehaviorActionCollection), typeof(BehaviorActionsBase));
    public BehaviorActionCollection Actions => (BehaviorActionCollection)GetValue(ActionsProperty);

    public BehaviorActionsBase()
    {
      SetValue(ActionsProperty, new BehaviorActionCollection());
    }
  }
}