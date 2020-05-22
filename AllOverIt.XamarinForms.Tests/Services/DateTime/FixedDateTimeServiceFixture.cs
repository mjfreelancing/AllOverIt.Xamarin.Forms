using AllOverIt.XamarinForms.Services.DateTime;
using FluentAssertions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Services.DateTime
{
  public class FixedDateTimeServiceFixture : AllOverItFixtureBase
  {
    [Fact]
    public void Should_Return_Current_Date()
    {
      var expected = System.DateTime.Now.Date;
      var subject = new FixedDateTimeService(expected);

      var actual = subject.CurrentDate;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Return_Current_Time()
    {
      var expected = System.DateTime.Now.Date;
      var subject = new FixedDateTimeService(expected);

      var actual = subject.CurrentDateTime;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Return_Current_Time_Utc()
    {
      var expected = System.DateTime.Now.Date;
      var subject = new FixedDateTimeService(expected);

      var actual = subject.CurrentDateTimeUtc;

      actual.Should().Be(expected.ToUniversalTime());
    }
  }
}