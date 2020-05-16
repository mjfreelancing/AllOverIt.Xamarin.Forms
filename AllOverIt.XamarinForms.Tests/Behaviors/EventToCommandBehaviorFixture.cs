using AllOverIt.XamarinForms.Behaviors;
using AllOverIt.XamarinForms.Commands;
using FluentAssertions;
using System;
using System.Linq;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Behaviors
{
  public class EventToCommandBehaviorFixture : AllOverItFixtureBase
  {
    private class DummyCommand : EventCommandBase
    {
      public object ExecutingContext { get; private set; }
      public string Value { get; private set; }

      public override bool Execute(object sender, object parameter)
      {
        var args = (TextChangedEventArgs) parameter;

        ExecutingContext = BindingContext;
        Value = args.NewTextValue;

        return true;
      }
    }

    public EventToCommandBehaviorFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void Should_Attach_Behavior()
    {
      var entry = new Entry();
      var behavior = new EventToCommandBehavior();

      entry.Behaviors.Add(behavior);

      entry.Behaviors.Single().Should().BeOfType<EventToCommandBehavior>();
    }

    [Fact]
    public void Should_Detach_Behavior()
    {
      var (entry, behavior) = CreateEventWithAttachedBehavior();

      entry.Behaviors.Add(behavior);

      behavior.AssociatedObject.Should().Be(entry);

      entry.Behaviors.Clear();

      behavior.AssociatedObject.Should().BeNull();
    }

    [Fact]
    public void Should_Have_Empty_Commands()
    {
      var behavior = new EventToCommandBehavior();

      var actual = behavior.Commands;

      actual.Should().BeEmpty();
    }

    [Fact]
    public void Should_Get_Commands_From_Attached_Behavior()
    {
      var entry = new Entry();

      var behavior = new EventToCommandBehavior();

      entry.Behaviors.Add(behavior);

      var commands = behavior.Commands;

      commands.Should().NotBeNull();
    }

    [Fact]
    public void Should_Get_Default_EventName()
    {
      var behavior = new EventToCommandBehavior();

      var actual = behavior.EventName;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_EventName()
    {
      var behavior = new EventToCommandBehavior();

      var expected = Create<string>();

      behavior.EventName = expected;

      var actual = behavior.EventName;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Fail_To_Register_Unknown_Event()
    {
      var (_, behavior) = CreateEventWithAttachedBehavior();

      var eventName = Create<string>();

      Invoking(() =>
        {
          behavior.EventName = eventName;
        })
        .Should()
        .Throw<ArgumentException>()
        .WithMessage($"Cannot register the '{eventName}' event");
    }

    [Fact]
    public void Should_Register_Event()
    {
      var (entry, behavior) = CreateEventWithAttachedBehavior();

      behavior.EventName = nameof(Entry.TextChanged);

      var command = new DummyCommand();

      behavior.Commands.Add(command);

      var expected = Create<string>();

      entry.Text = expected;

      var actual = command.Value;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Unregister_Event()
    {
      var (entry, behavior) = CreateEventWithAttachedBehavior();

      behavior.EventName = nameof(Entry.TextChanged);

      var command = new DummyCommand();

      behavior.Commands.Add(command);

      entry.Behaviors.Clear();

      entry.Text = Create<string>();

      var actual = command.Value;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Have_Binding_Context_When_Executing_Command()
    {
      var (entry, behavior) = CreateEventWithAttachedBehavior();

      var dataSource = CreateMany<int>();
      entry.BindingContext = dataSource;

      behavior.EventName = nameof(Entry.TextChanged);

      var command = new DummyCommand();

      behavior.Commands.Add(command);

      entry.Text = Create<string>();

      var actual = command.ExecutingContext;

      actual.Should().Be(dataSource);
    }

    [Fact]
    public void Should_Change_Binding_Context()
    {
      var (entry, behavior) = CreateEventWithAttachedBehavior();

      var dataSource1 = CreateMany<int>();
      entry.BindingContext = dataSource1;

      behavior.EventName = nameof(Entry.TextChanged);

      var command = new DummyCommand();

      behavior.Commands.Add(command);

      entry.Text = Create<string>();

      var actual1 = command.ExecutingContext;

      actual1.Should().Be(dataSource1);

      var dataSource2 = CreateMany<int>();
      entry.BindingContext = dataSource2;

      entry.Text = Create<string>();

      var actual2 = command.ExecutingContext;

      actual2.Should().Be(dataSource2);
    }

    private static (Entry entry, EventToCommandBehavior behavior) CreateEventWithAttachedBehavior()
    {
      var entry = new Entry();
      var behavior = new EventToCommandBehavior();

      entry.Behaviors.Add(behavior);

      return (entry, behavior);
    }
  }
}