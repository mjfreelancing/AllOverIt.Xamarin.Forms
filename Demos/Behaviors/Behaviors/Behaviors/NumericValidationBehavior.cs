namespace Behaviors.Behaviors
{
  //public class NumericValidationBehavior : Behavior<Entry>
  //{
  //  protected override void OnAttachedTo(Entry entry)
  //  {
  //    base.OnAttachedTo(entry);
  //    entry.TextChanged += OnEntryTextChanged;
  //  }

  //  protected override void OnDetachingFrom(Entry entry)
  //  {
  //    entry.TextChanged -= OnEntryTextChanged;
  //    base.OnDetachingFrom(entry);
  //  }

  //  private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
  //  {
  //    var isValid = double.TryParse(args.NewTextValue, out _);

  //    ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
  //  }
  //}
}