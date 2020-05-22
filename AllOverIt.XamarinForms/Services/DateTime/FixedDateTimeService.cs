namespace AllOverIt.XamarinForms.Services.DateTime
{
  // for use in unit tests
  public class FixedDateTimeService : IDateTimeService
  {
    public System.DateTime CurrentDate => CurrentDateTime.Date;
    public System.DateTime CurrentDateTime { get; }
    public System.DateTime CurrentDateTimeUtc => CurrentDateTime.ToUniversalTime();

    public FixedDateTimeService(System.DateTime fixedDateTime)
    {
      CurrentDateTime = fixedDateTime;
    }
  }
}