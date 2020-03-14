using System;

namespace AllOverIt.XamarinForms.Logging
{
  public interface ILogger
    : IDisposable
  {
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
  }
}