using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors
{
  public class ClearListViewSelectedItemBehavior : Behavior<ListView>
  {
    protected override void OnAttachedTo(ListView bindable)
    {
      base.OnAttachedTo(bindable);

      bindable.ItemSelected += ListView_ItemSelected;
    }

    protected override void OnDetachingFrom(ListView bindable)
    {
      base.OnDetachingFrom(bindable);

      bindable.ItemSelected -= ListView_ItemSelected;
    }

    private static void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      ((ListView)sender).SelectedItem = null;
    }
  }
}