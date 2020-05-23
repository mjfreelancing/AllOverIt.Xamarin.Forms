namespace AllOverIt.XamarinForms.Services.DateTime
{
  public interface IDateTimeService
  {
    /// <summary>
    /// Returns the current local date.
    /// </summary>
    System.DateTime CurrentDate { get; }

    /// <summary>
    /// Returns the current local date and time.
    /// </summary>
    System.DateTime CurrentDateTime { get; }

    /// <summary>
    /// Returns the current UTC date and time.
    /// </summary>
    System.DateTime CurrentDateTimeUtc { get; }
  }
}