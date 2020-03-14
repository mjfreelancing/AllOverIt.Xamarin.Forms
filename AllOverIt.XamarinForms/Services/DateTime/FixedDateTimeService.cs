namespace AllOverIt.XamarinForms.Services.DateTime
{
  // for use in unit tests
  public class FixedDateTimeService : IDateTimeService
  {
    public System.DateTime CurrentDate { get; }

    public FixedDateTimeService(System.DateTime fixedDate)
    {
      CurrentDate = fixedDate;
    }
  }
}