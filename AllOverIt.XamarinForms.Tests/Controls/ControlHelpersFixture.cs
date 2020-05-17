using AllOverIt.XamarinForms.Controls;
using FluentAssertions;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Controls
{
  public class ControlHelpersFixture : AllOverItFixtureBase
  {
    private class DummyPage : Page
    {
      private Entry DummyEntry;

      public DummyPage()
      {
        DummyEntry = new Entry();
      }

      public string EntryText => DummyEntry.Text;
    }

    public ControlHelpersFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void Should_Set_Property()
    {
      var expected = Create<string>();
      var subject = new Entry();

      ControlHelpers.SetProperty<Entry, string>(subject, expected, (control, propertyValue) =>
      {
        control.Text = propertyValue;
      });

      subject.Text.Should().Be(expected);
    }

    [Fact]
    public void Should_Not_Set_Property()
    {
      var expected = Create<string>();
      var subject = new Entry();

      // deliberately specifying the wrong control type so the property is not set
      ControlHelpers.SetProperty<Label, string>(subject, expected, (control, propertyValue) =>
      {
        control.Text = propertyValue;
      });

      subject.Text.Should().BeNull();
    }

    [Fact]
    public void Should_Get_Non_Public_Control()
    {
      var expected = Create<string>();
      var subject = new DummyPage();

      var entry = ControlHelpers.GetNonPublicFieldFromControl<Entry>(subject, "DummyEntry");
      entry.Text = expected;

      subject.EntryText.Should().Be(expected);
    }
  }
}