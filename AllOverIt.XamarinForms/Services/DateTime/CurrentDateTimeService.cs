namespace AllOverIt.XamarinForms.Services.DateTime
{
  //[AutoDependency(false, true)]
  public class CurrentDateTimeService : IDateTimeService
  {
    public System.DateTime CurrentDate => System.DateTime.Now.Date;
  }
}