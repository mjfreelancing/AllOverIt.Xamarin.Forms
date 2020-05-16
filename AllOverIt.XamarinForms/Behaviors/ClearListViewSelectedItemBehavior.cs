using AllOverIt.XamarinForms.Behaviors.Base;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  /// <summary>
  /// Clears the selected item of a <see cref="ListView"/> after an item has been selected
  /// </summary>
  /// <remarks>This behavior can be attached via a style.</remarks>
  public class ClearListViewSelectedItemBehavior : AttachableBehavior<ClearListViewSelectedItemBehavior, ListView>
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

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
    {
      AssociatedObject.SelectedItem = null;
    }
  }
}