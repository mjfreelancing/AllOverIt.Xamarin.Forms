using AllOverIt.XamarinForms.Logging;
using FluentAssertions;
using System;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Logging
{
  public class LoggerBaseFixture : AllOverItFixtureBase
  {
    public class DummyLogger : LoggerBase
    {
      private readonly Action<string> _action;

      public string LoggerTag => Tag;

      public DummyLogger(string tag, Action<string> action)
        : base(tag)
      {
        _action = action;
      }

      protected override void LogMessage(LogLevel level, string message)
      {
        _action.Invoke($"{level}{message}");
      }
    }

    [Fact]
    public void Should_Set_Tag()
    {
      var tag = Create<string>();

      var subject = new DummyLogger(tag, null);

      subject.LoggerTag.Should().Be(tag);
    }

    [Fact]
    public void Should_Log_Debug()
    {
      var tag = Create<string>();
      string actual = null;
      var expected = Create<string>();

      var subject = new DummyLogger(tag, log => actual = log);

      using (subject)
      {
        subject.Debug(expected);
      }

      actual.Should().Be($"Debug{expected}");
    }

    [Fact]
    public void Should_Log_Info()
    {
      var tag = Create<string>();
      string actual = null;
      var expected = Create<string>();

      var subject = new DummyLogger(tag, log => actual = log);

      using (subject)
      {
        subject.Info(expected);
      }

      actual.Should().Be($"Info{expected}");
    }

    [Fact]
    public void Should_Log_Warn()
    {
      var tag = Create<string>();
      string actual = null;
      var expected = Create<string>();

      var subject = new DummyLogger(tag, log => actual = log);

      using (subject)
      {
        subject.Warn(expected);
      }

      actual.Should().Be($"Warn{expected}");
    }

    [Fact]
    public void Should_Log_Error()
    {
      var tag = Create<string>();
      string actual = null;
      var expected = Create<string>();

      var subject = new DummyLogger(tag, log => actual = log);

      using (subject)
      {
        subject.Error(expected);
      }

      actual.Should().Be($"Error{expected}");
    }
  }
}