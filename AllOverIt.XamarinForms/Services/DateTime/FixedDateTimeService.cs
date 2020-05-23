namespace AllOverIt.XamarinForms.Services.DateTime
{
  /// <summary>
  /// An implementation of <see cref="IDateTimeService"/> for use in unit tests where date and time values need to be predictable.
  /// </summary>
  public class FixedDateTimeService : IDateTimeService
  {
    /// <summary>
    /// The date provided at the time of construction.
    /// </summary>
    public System.DateTime CurrentDate => CurrentDateTime.Date;

    /// <summary>
    /// The date and time provided at the time of construction.
    /// </summary>
    public System.DateTime CurrentDateTime { get; }

    /// <summary>
    /// The date and time provided at the time of construction, as UTC.
    /// </summary>
    public System.DateTime CurrentDateTimeUtc => CurrentDateTime.ToUniversalTime();

    /// <summary>
    /// Initializes the instance with a fixed date and time
    /// </summary>
    /// <param name="fixedDateTime">The fixed date and time to be used.</param>
    public FixedDateTimeService(System.DateTime fixedDateTime)
    {
      CurrentDateTime = fixedDateTime;
    }
  }
}