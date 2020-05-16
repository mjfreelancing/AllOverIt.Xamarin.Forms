using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AllOverIt.XamarinForms.Logging
{
  public enum LogLevel
  {
    Debug,
    Info,
    Warn,
    Error
  }
  public abstract class LoggerBase : ILogger
  {
    private readonly BlockingCollection<(LogLevel Level, string Message)> _logMessages;

    protected string Tag { get; }

    protected LoggerBase(string tag)
    {
      Tag = tag;

      _logMessages = new BlockingCollection<(LogLevel, string)>();

      // don't bother disposing of tasks : https://devblogs.microsoft.com/pfxteam/do-i-need-to-dispose-of-tasks/
      Task.Factory.StartNew(() =>
      {
        foreach (var (level, message) in _logMessages.GetConsumingEnumerable())
        {
          LogMessage(level, message);
        }
      }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public void Debug(string message) => DoLog(LogLevel.Debug, message);
    public void Info(string message) => DoLog(LogLevel.Info, message);
    public void Warn(string message) => DoLog(LogLevel.Warn, message);
    public void Error(string message) => DoLog(LogLevel.Error, message);

    public void Dispose()
    {
      Dispose(true);
    }

    protected abstract void LogMessage(LogLevel level, string message);

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && !_logMessages.IsAddingCompleted)
      {
        _logMessages.CompleteAdding();
      }
    }

    private void DoLog(LogLevel level, string message)
    {
      var logMessage = (level, message);

      _logMessages.Add(logMessage);
    }
  }
}