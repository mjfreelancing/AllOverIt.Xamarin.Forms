﻿using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AllOverIt.XamarinForms.Logging
{
  public abstract class LoggerBase : ILogger
  {
    private readonly BlockingCollection<(string LevelTag, string Message)> _logMessages;

    protected string AppTag { get; }

    protected LoggerBase(string appTag)
    {
      AppTag = appTag;

      _logMessages = new BlockingCollection<(string, string)>();

      // don't bother disposing of tasks : https://devblogs.microsoft.com/pfxteam/do-i-need-to-dispose-of-tasks/
      Task.Factory.StartNew(() =>
      {
        foreach (var message in _logMessages.GetConsumingEnumerable())
        {
          LogMessage(message.LevelTag, message.Message);
        }
      }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public void Debug(string message) => DoLog("D", message);
    public void Info(string message) => DoLog("I", message);
    public void Warn(string message) => DoLog("W", message);
    public void Error(string message) => DoLog("E", message);

    public void Dispose()
    {
      Dispose(true);
    }

    protected abstract void LogMessage(string levelTag, string message);

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && !_logMessages.IsAddingCompleted)
      {
        _logMessages.CompleteAdding();
      }
    }

    private void DoLog(string levelTag, string message)
    {
      var logMessage = (levelTag, message);
      _logMessages.Add(logMessage);
    }
  }
}