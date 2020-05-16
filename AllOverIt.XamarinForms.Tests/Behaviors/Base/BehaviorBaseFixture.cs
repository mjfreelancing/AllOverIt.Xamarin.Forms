using AllOverIt.XamarinForms.Behaviors.Base;
using FluentAssertions;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Behaviors.Base
{
  public class BehaviorBaseFixture : AllOverItFixtureBase
  {
    private class DummyBehavior : BehaviorBase<ListView>
    {
    }

    public BehaviorBaseFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void AssociatedObject_Should_Be_Null()
    {
      var behavior = new DummyBehavior();

      behavior.AssociatedObject.Should().BeNull();
    }

    [Fact]
    public void Should_Attach_To_Bindable()
    {
      var listView = new ListView();
      var behavior = new DummyBehavior();

      listView.Behaviors.Add(behavior);

      behavior.AssociatedObject.Should().Be(listView);
    }

    [Fact]
    public void Should_Detach_From_Bindable()
    {
      var listView = new ListView();
      var behavior = new DummyBehavior();

      listView.Behaviors.Add(behavior);

      behavior.AssociatedObject.Should().Be(listView);

      listView.Behaviors.Clear();

      behavior.AssociatedObject.Should().BeNull();
    }

    [Fact]
    public void BindingContext_Should_Be_Null_When_Bindable_Has_No_BindingContext()
    {
      var listView = new ListView();
      var behavior = new DummyBehavior();

      listView.Behaviors.Add(behavior);

      behavior.BindingContext.Should().BeNull();
    }

    [Fact]
    public void BindingContext_Should_Be_Bindable_BindingContext()
    {
      var dataSource = CreateMany<string>();
      var listView = new ListView()
      {
        BindingContext = dataSource
      };

      var behavior = new DummyBehavior();

      listView.Behaviors.Add(behavior);

      behavior.BindingContext.Should().Be(dataSource);
    }

    [Fact]
    public void BindingContext_Should_Change_When_Bindable_BindingContext_Is_Added()
    {
      var listView = new ListView();
      var behavior = new DummyBehavior();

      listView.Behaviors.Add(behavior);

      behavior.BindingContext.Should().BeNull();

      var dataSource = CreateMany<string>();
      listView.BindingContext = dataSource;

      behavior.BindingContext.Should().Be(dataSource);
    }

    [Fact]
    public void BindingContext_Should_Change_When_Bindable_BindingContext_Is_Removed()
    {
      var dataSource = CreateMany<string>();

      var listView = new ListView
      {
        BindingContext = dataSource
      };

      var behavior = new DummyBehavior();

      listView.Behaviors.Add(behavior);

      behavior.BindingContext.Should().Be(dataSource);

      listView.BindingContext = null;

      behavior.BindingContext.Should().BeNull();
    }
  }
}