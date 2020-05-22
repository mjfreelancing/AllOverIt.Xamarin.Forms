using AllOverIt.XamarinForms.Services.DateTime;
using FluentAssertions;
using System;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Services.DateTime
{
  public class CurrentDateTimeServiceFixture : AllOverItFixtureBase
  {
    [Fact]
    public void Should_Return_Current_Date()
    {
      var expected = System.DateTime.Now.Date;
      var subject = new CurrentDateTimeService();

      var actual = subject.CurrentDate;

      // cater for the rare chance of testing on the stroke of midnight
      if (actual != expected)
      {
        expected = System.DateTime.Now.Date;
      }

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Return_Current_Time()
    {
      var expected = System.DateTime.Now;
      var subject = new CurrentDateTimeService();

      var actual = subject.CurrentDateTime;

      actual.Should().BeCloseTo(expected, TimeSpan.FromMilliseconds(250));
    }

    [Fact]
    public void Should_Return_Current_Time_Utc()
    {
      var expected = System.DateTime.UtcNow;
      var subject = new CurrentDateTimeService();

      var actual = subject.CurrentDateTimeUtc;

      actual.Should().BeCloseTo(expected, TimeSpan.FromMilliseconds(250));
    }
  }
}