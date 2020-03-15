namespace AllOverIt.XamarinForms.Behaviors.Base
{
  public interface IEventCommand
  {
    bool Execute(object sender, object parameter);
  }
}