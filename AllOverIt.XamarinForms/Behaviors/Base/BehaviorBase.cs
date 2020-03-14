using System;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Behaviors.Base
{
  public abstract class BehaviorBase<TBindable> : Behavior<TBindable> where TBindable : BindableObject
  {
    public TBindable AssociatedObject { get; private set; }

    protected override void OnAttachedTo(TBindable bindable)
    {
      base.OnAttachedTo(bindable);

      AssociatedObject = bindable;

      if (bindable.BindingContext != null)
      {
        BindingContext = bindable.BindingContext;
      }

      bindable.BindingContextChanged += OnBindingContextChanged;
    }

    protected override void OnDetachingFrom(TBindable bindable)
    {
      base.OnDetachingFrom(bindable);

      bindable.BindingContextChanged -= OnBindingContextChanged;
      AssociatedObject = null;
    }

    protected override void OnBindingContextChanged()
    {
      base.OnBindingContextChanged();
      BindingContext = AssociatedObject.BindingContext;
    }

    private void OnBindingContextChanged(object sender, EventArgs args)
    {
      OnBindingContextChanged();
    }
  }
}