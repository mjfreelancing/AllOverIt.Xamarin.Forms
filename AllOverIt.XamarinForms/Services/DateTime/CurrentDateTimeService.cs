namespace AllOverIt.XamarinForms.Services.DateTime
{
  public class CurrentDateTimeService : IDateTimeService
  {
    /// <summary>
    /// Returns the current local date.
    /// </summary>
    public System.DateTime CurrentDate => System.DateTime.Now.Date;

    /// <summary>
    /// Returns the current local date and time.
    /// </summary>
    public System.DateTime CurrentDateTime => System.DateTime.Now;

    /// <summary>
    /// Returns the current UTC date and time.
    /// </summary>
    public System.DateTime CurrentDateTimeUtc => System.DateTime.UtcNow;
  }
}