using AllOverIt.XamarinForms.Behaviors;
using AllOverIt.XamarinForms.Tests.Controls;
using FluentAssertions;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Behaviors
{
  // need to create controls non-parallel to prevent init issues
  [Collection(nameof(ControlCollection))]
  public class ClearListViewSelectedItemBehaviorFixture : AllOverItFixtureBase
  {
    public ClearListViewSelectedItemBehaviorFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void Should_Attach_To_ListView()
    {
      var listView = new ListView();

      var behavior = new ClearListViewSelectedItemBehavior();

      listView.Behaviors.Add(behavior);

      behavior.AssociatedObject.Should().Be(listView);
    }

    [Fact]
    public void Should_Detach_From_ListView()
    {
      var listView = new ListView();

      var behavior = new ClearListViewSelectedItemBehavior();

      listView.Behaviors.Add(behavior);

      behavior.AssociatedObject.Should().Be(listView);

      listView.Behaviors.Clear();

      behavior.AssociatedObject.Should().BeNull();
    }

    [Fact]
    public void Should_Change_BindingContext()
    {
      var listView = new ListView();
      var behavior = new ClearListViewSelectedItemBehavior();

      listView.Behaviors.Add(behavior);

      var prevBindingContext = behavior.BindingContext;

      var dataSource = CreateMany<string>(3);

      listView.BindingContext = dataSource;

      behavior.BindingContext.Should().NotBeSameAs(prevBindingContext);
      behavior.BindingContext.Should().BeSameAs(dataSource);
    }
    
    [Fact]
    public void Should_Deselect_A_Selected_Item()
    {
      var dataSource = CreateMany<string>(3);
      var selectedItem = string.Empty;
      var expected = dataSource[1];

      void OnSelectedItem(object sender, SelectedItemChangedEventArgs args)
      {
        if (args.SelectedItem != null)
        {
          selectedItem = $"{args.SelectedItem}";
        }
      }

      var listView = new ListView(ListViewCachingStrategy.RecycleElement)
      {
        ItemsSource = dataSource
      };

      listView.ItemSelected += OnSelectedItem;

      try
      {
        var behavior = new ClearListViewSelectedItemBehavior();

        listView.Behaviors.Add(behavior);
        listView.SelectedItem = expected;
      }
      finally
      {
        listView.ItemSelected -= OnSelectedItem;
        listView.Behaviors.Clear();
      }

      // asserts the items was selected
      selectedItem.Should().Be(expected);

      // asserts the behavior de-selected the item
      listView.SelectedItem.Should().BeNull();
    }
  }
}