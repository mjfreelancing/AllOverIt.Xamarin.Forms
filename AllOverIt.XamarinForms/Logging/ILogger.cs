namespace AllOverIt.XamarinForms.Logging
{
  /// <summary>
  /// Describes a logger.
  /// </summary>
  public interface ILogger
  {
    /// <summary>
    /// Process a debug level message.
    /// </summary>
    /// <param name="message">The message to process.</param>
    void Debug(string message);

    /// <summary>
    /// Process an information level message.
    /// </summary>
    /// <param name="message">The message to process.</param>
    void Info(string message);

    /// <summary>
    /// Process a warning level message.
    /// </summary>
    /// <param name="message">The message to process.</param>
    void Warn(string message);

    /// <summary>
    /// Process an error level message.
    /// </summary>
    /// <param name="message">The message to process.</param>
    void Error(string message);
  }
}