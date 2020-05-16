namespace AllOverIt.XamarinForms.Services.DateTime
{
  public class CurrentDateTimeService : IDateTimeService
  {
    public System.DateTime CurrentDate => System.DateTime.Now.Date;
    public System.DateTime CurrentDateTime => System.DateTime.Now;
  }
}