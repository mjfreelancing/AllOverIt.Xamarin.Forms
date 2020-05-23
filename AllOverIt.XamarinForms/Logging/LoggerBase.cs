using AllOverIt.Helpers;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AllOverIt.XamarinForms.Logging
{
  /// <summary>
  /// A base logger class that processes messages using a <see cref="BlockingCollection{T}"/>.
  /// </summary>
  public abstract class LoggerBase : ILogger, IDisposable
  {
    private readonly BlockingCollection<(LogLevel Level, string Message)> _logMessages;
    private readonly Task _loggingTask;

    /// <summary>
    /// A user-defined tag that can be associated with the logger.
    /// </summary>
    protected string Tag { get; }

    protected LoggerBase()
    {
      _logMessages = new BlockingCollection<(LogLevel, string)>();
      _loggingTask = CreateLoggingTask();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="tag">A tag to identify the logger</param>
    protected LoggerBase(string tag)
      : this()
    {
      Tag = tag.WhenNotNull(nameof(tag));
    }

    /// <summary>
    /// Processing a logging message with a Debug level.
    /// </summary>
    /// <param name="message">The message to process.</param>
    public void Debug(string message) => DoLog(LogLevel.Debug, message);

    /// <summary>
    /// Processing a logging message with a Info level.
    /// </summary>
    /// <param name="message">The message to process.</param>
    public void Info(string message) => DoLog(LogLevel.Info, message);

    /// <summary>
    /// Processing a logging message with a Warn level.
    /// </summary>
    /// <param name="message">The message to process.</param>
    public void Warn(string message) => DoLog(LogLevel.Warn, message);

    /// <summary>
    /// Processing a logging message with a Error level.
    /// </summary>
    /// <param name="message">The message to process.</param>
    public void Error(string message) => DoLog(LogLevel.Error, message);

    /// <summary>
    /// Flushes all pending log messages at the time of disposal.
    /// </summary>
    public void Dispose() => Dispose(true);

    /// <summary>
    /// Override to process the log messages as required.
    /// </summary>
    /// <param name="level">The log level of the message.</param>
    /// <param name="message">The content of the message.</param>
    protected abstract void LogMessage(LogLevel level, string message);

    /// <summary>
    /// Flushes all pending log messages at the time of disposal.
    /// </summary>
    /// <param name="disposing">Indicates if the class is being disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing && !_logMessages.IsAddingCompleted)
      {
        _logMessages.CompleteAdding();

        // todo: change class to use IAsyncEnumerable once available and await this task
        // don't bother disposing of tasks : https://devblogs.microsoft.com/pfxteam/do-i-need-to-dispose-of-tasks/
        _loggingTask.GetAwaiter().GetResult();
      }
    }

    private Task CreateLoggingTask()
    {
      return Task.Factory.StartNew(() =>
      {
        foreach (var (level, message) in _logMessages.GetConsumingEnumerable())
        {
          LogMessage(level, message);
        }
      }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    private void DoLog(LogLevel level, string message)
    {
      var logMessage = (level, message);

      _logMessages.Add(logMessage);
    }
  }
}