using AllOverIt.XamarinForms.Logging;
using System;
using System.Collections.Generic;

namespace Behaviors
{
  public class BehaviorLogger : LoggerBase
  {
    private static IDictionary<LogLevel, string> _levelIdentifier = new Dictionary<LogLevel, string>
    {
      {LogLevel.Debug, "D" },
      {LogLevel.Info, "I" },
      {LogLevel.Warn, "W" },
      {LogLevel.Error, "E" }
    };

    public BehaviorLogger() : base("BehaviorDemo")
    {
    }

    protected override void LogMessage(LogLevel level, string message)
    {
      Console.WriteLine($"{Tag} - [{_levelIdentifier[level]} - {message}");
    }
  }
}