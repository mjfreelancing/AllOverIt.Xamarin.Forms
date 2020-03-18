using AllOverIt.XamarinForms.Behaviors.Base;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public class ClearListViewSelectedItemBehavior : BehaviorBase<ListView>
  {
    protected override void OnAttachedTo(ListView bindable)
    {
      base.OnAttachedTo(bindable);

      bindable.ItemSelected += ListView_ItemSelected;
    }

    protected override void OnDetachingFrom(ListView bindable)
    {
      bindable.ItemSelected -= ListView_ItemSelected;

      base.OnDetachingFrom(bindable);
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      AssociatedObject.SelectedItem = null;
    }
  }
}