using AllOverIt.XamarinForms.Logging;
using System;

namespace Behaviors
{
  public class BehaviorLogger : LoggerBase
  {
    public BehaviorLogger() : base("BehaviorDemo")
    {
    }

    protected override void LogMessage(string level, string message)
    {
      Console.WriteLine($"{Tag} - [{level}] - {message}");
    }
  }
}