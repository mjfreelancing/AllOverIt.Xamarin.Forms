using System;

namespace AllOverIt.XamarinForms.Exceptions
{
  public interface IExceptionHandler
  {
    void Handle(Exception exception);
  }
}