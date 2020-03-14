using AllOverIt.Extensions;
using AllOverIt.Fixture;
using FluentAssertions;
using AllOverIt.XamarinForms.Collections;
using System;
using System.Collections.Specialized;
using System.Linq;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Collections
{
  public class ObservableListFixture : AoiFixtureBase
  {
    public class Constructor_Default : ObservableListFixture
    {
      [Fact]
      public void Should_Be_Empty()
      {
        var actual = new ObservableList<string>();

        actual.Should().BeEmpty();
      }
    }

    public class Constructor_Collection : ObservableListFixture
    {
      [Fact]
      public void Should_Be_Empty()
      {
        var actual = new ObservableList<string>(new string[] { });

        actual.Should().BeEmpty();
      }

      [Fact]
      public void Should_Be_Initialized()
      {
        var expected = CreateMany<string>();
        var actual = new ObservableList<string>(expected);

        actual.Should().BeEquivalentTo(expected);
      }
    }

    public class AddRange : ObservableListFixture
    {
      private readonly ObservableList<string> _observable;

      public AddRange()
      {
        _observable = new ObservableList<string>();
      }

      [Fact]
      public void Should_Throw_When_Collection_Null()
      {
        Invoking(
            () => { _observable.AddRange(null, Create<NotifyCollectionChangedAction>()); }
          )
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage("Value cannot be null. (Parameter 'collection')");
      }

      [Fact]
      public void Should_Throw_When_Invalid_Mode()
      {
        var mode = CreateExcluding<NotifyCollectionChangedAction>(new[] { NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset });

        Invoking(
            () => { _observable.AddRange(CreateMany<string>(), mode); }
          )
          .Should()
          .Throw<ArgumentException>()
          .WithMessage($"{mode} is an invalid option for AddRange()");
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Add)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Allow_Reentry(NotifyCollectionChangedAction mode)
      {
        void OnCollectionChanged1(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        void OnCollectionChanged2(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        // requires at least two observers before the reentry error will be raised
        _observable.CollectionChanged += OnCollectionChanged1;
        _observable.CollectionChanged += OnCollectionChanged2;

        try
        {
          Invoking(
              () => { _observable.AddRange(CreateMany<string>(), mode); }
            )
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Cannot change ObservableCollection during a CollectionChanged event.");
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged1;
          _observable.CollectionChanged -= OnCollectionChanged2;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Add)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Be_Modified_When_Adding_Empty_Collection(NotifyCollectionChangedAction mode)
      {
        var itemsToAdd = CreateMany<string>();
        var currentCount = itemsToAdd.Count;

        _observable.AddRange(itemsToAdd);
        _observable.AddRange(new string[] { }, mode);

        _observable.Count.Should().Be(currentCount);
        _observable.Should().BeEquivalentTo(itemsToAdd);
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Add)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Throw_When_Adding_Empty_Collection(NotifyCollectionChangedAction mode)
      {
        Invoking(
            () => { _observable.AddRange(new string[] { }, mode); }
          )
          .Should()
          .NotThrow();
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Add)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Raise_Notification_When_Adding_Empty_Collection(NotifyCollectionChangedAction mode)
      {
        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationRaised = true;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.AddRange(new string[] { }, mode);

          notificationRaised.Should().BeFalse();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Theory]
      [InlineData(true, NotifyCollectionChangedAction.Add)]
      [InlineData(true, NotifyCollectionChangedAction.Reset)]
      [InlineData(false, NotifyCollectionChangedAction.Add)]
      [InlineData(false, NotifyCollectionChangedAction.Reset)]
      public void Should_Raise_Notification_When_Adding_Collection(bool startEmpty, NotifyCollectionChangedAction mode)
      {
        var initialItems = startEmpty
          ? Enumerable.Empty<string>()
          : CreateMany<string>();

        _observable.AddRange(initialItems);

        var expectedStart = _observable.Count;

        var itemsToAdd = CreateMany<string>();
        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationRaised = true;

          args.OldItems.Should().BeNull();

          if (mode == NotifyCollectionChangedAction.Add)
          {
            args.Action.Should().Be(NotifyCollectionChangedAction.Add);
            args.NewItems.Should().BeEquivalentTo(itemsToAdd);
            args.OldStartingIndex.Should().Be(-1);
            args.NewStartingIndex.Should().Be(expectedStart);
          }
          else
          {
            args.Action.Should().Be(NotifyCollectionChangedAction.Reset);
            args.NewItems.Should().BeNull();
            args.OldStartingIndex.Should().Be(-1);
            args.NewStartingIndex.Should().Be(-1);
          }
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.AddRange(itemsToAdd, mode);

          notificationRaised.Should().BeTrue();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Add)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Raise_Notification_Once(NotifyCollectionChangedAction mode)
      {
        var notificationCount = 0;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationCount++;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.AddRange(CreateMany<string>(), mode);

          notificationCount.Should().Be(1);
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Add)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Add_To_Collection(NotifyCollectionChangedAction mode)
      {
        _observable.AddRange(CreateMany<string>());

        var additionalItems = CreateMany<string>();
        var expected = _observable.Concat(additionalItems).AsReadOnlyList();

        _observable.AddRange(additionalItems, mode);

        _observable.Should().BeEquivalentTo(expected);
      }
    }

    public class RemoveRange : ObservableListFixture
    {
      private readonly ObservableList<string> _observable;

      public RemoveRange()
      {
        _observable = new ObservableList<string>();
      }

      [Fact]
      public void Should_Throw_When_Collection_Null()
      {
        Invoking(
            () => { _observable.RemoveRange(null, Create<NotifyCollectionChangedAction>()); }
          )
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage("Value cannot be null. (Parameter 'collection')");
      }

      [Fact]
      public void Should_Throw_When_Invalid_Mode()
      {
        var mode = CreateExcluding<NotifyCollectionChangedAction>(new[] { NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Reset });

        Invoking(
            () => { _observable.RemoveRange(CreateMany<string>(), mode); }
          )
          .Should()
          .Throw<ArgumentException>()
          .WithMessage($"{mode} is an invalid option for RemoveRange()");
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Allow_Reentry(NotifyCollectionChangedAction mode)
      {
        void OnCollectionChanged1(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        void OnCollectionChanged2(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        // requires at least two observers before the reentry error will be raised
        _observable.CollectionChanged += OnCollectionChanged1;
        _observable.CollectionChanged += OnCollectionChanged2;

        try
        {
          Invoking(
              () =>
              {
                var itemsToRemove = CreateMany<string>();
                _observable.AddRange(itemsToRemove);

                _observable.RemoveRange(itemsToRemove, mode);
              }
            )
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Cannot change ObservableCollection during a CollectionChanged event.");
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged1;
          _observable.CollectionChanged -= OnCollectionChanged2;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Be_Modified_When_Removing_Empty_Collection(NotifyCollectionChangedAction mode)
      {
        var itemsToAdd = CreateMany<string>();
        var currentCount = itemsToAdd.Count;

        _observable.AddRange(itemsToAdd);
        _observable.RemoveRange(new string[] { }, mode);

        _observable.Count.Should().Be(currentCount);
        _observable.Should().BeEquivalentTo(itemsToAdd);
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Throw_When_Removing_Empty_Collection(NotifyCollectionChangedAction mode)
      {
        Invoking(
            () => { _observable.RemoveRange(new string[] { }, mode); }
          )
          .Should()
          .NotThrow();
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Raise_Notification_When_Removing_Empty_Collection(NotifyCollectionChangedAction mode)
      {
        _observable.AddRange(CreateMany<string>());

        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationRaised = true;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.RemoveRange(new string[] { }, mode);

          notificationRaised.Should().BeFalse();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Raise_Notification_When_Removing_Collection(NotifyCollectionChangedAction mode)
      {
        var itemsToRemove = CreateMany<string>();

        _observable.AddRange(CreateMany<string>());
        _observable.AddRange(itemsToRemove);

        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationRaised = true;

          args.NewItems.Should().BeNull();

          if (mode == NotifyCollectionChangedAction.Remove)
          {
            args.Action.Should().Be(NotifyCollectionChangedAction.Remove);
            args.OldItems.Should().BeEquivalentTo(itemsToRemove);
            args.OldStartingIndex.Should().Be(-1);
            args.NewStartingIndex.Should().Be(-1);
          }
          else
          {
            args.Action.Should().Be(NotifyCollectionChangedAction.Reset);
            args.OldItems.Should().BeNull();
            args.OldStartingIndex.Should().Be(-1);
            args.NewStartingIndex.Should().Be(-1);
          }
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.RemoveRange(itemsToRemove, mode);

          notificationRaised.Should().BeTrue();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Not_Raise_Notification_When_Removing_Items_Not_In_The_Collection(NotifyCollectionChangedAction mode)
      {
        _observable.AddRange(CreateMany<string>());

        var itemsToRemove = CreateMany<string>();
        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationRaised = true;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.RemoveRange(itemsToRemove, mode);

          notificationRaised.Should().BeFalse();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Raise_Notification_Once(NotifyCollectionChangedAction mode)
      {
        var notificationCount = 0;
        var itemsToRemove = CreateMany<string>();

        _observable.AddRange(itemsToRemove);
        _observable.AddRange(CreateMany<string>());

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationCount++;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.RemoveRange(itemsToRemove, mode);

          notificationCount.Should().Be(1);
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Remove_From_Collection(NotifyCollectionChangedAction mode)
      {
        _observable.AddRange(CreateMany<string>());
        var expected = _observable.ToList();    // create a copy

        var additionalItems = CreateMany<string>();
        _observable.AddRange(additionalItems);

        _observable.Should().NotBeEquivalentTo(expected);

        _observable.RemoveRange(additionalItems, mode);

        _observable.Should().BeEquivalentTo(expected);
      }

      [Theory]
      [InlineData(NotifyCollectionChangedAction.Remove)]
      [InlineData(NotifyCollectionChangedAction.Reset)]
      public void Should_Remove_From_Collection_With_Duplicates(NotifyCollectionChangedAction mode)
      {
        var additionalItems = CreateMany<string>();
        _observable.AddRange(additionalItems);

        var expected = _observable.ToList();    // create a copy

        _observable.AddRange(additionalItems);

        _observable.Should().NotBeEquivalentTo(expected);

        _observable.RemoveRange(additionalItems, mode);

        _observable.Should().BeEquivalentTo(expected);
      }

      [Fact]
      public void Should_not_Mutate_Source_When_Source_Is_Present()
      {
        var itemsToAdd = CreateMany<string>();
        var itemsToRemove = itemsToAdd.ToList();          // want a mutable copy
        var itemCount = itemsToRemove.Count;

        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemCount);

        _observable.RemoveRange(itemsToRemove);

        itemsToRemove.Should().HaveCount(itemCount);
      }

      [Fact]
      public void Should_not_Mutate_Source_When_Source_Is_Not_Present()
      {
        // the strings are GUIDs so they will all be unique
        var itemsToAdd = CreateMany<string>();
        var itemsToRemove = CreateMany<string>().AsList();  // want a mutable version
        var itemCount = itemsToRemove.Count;

        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemCount);

        _observable.RemoveRange(itemsToRemove);

        itemsToRemove.Should().HaveCount(itemCount);
      }
    }

    public class Replace : ObservableListFixture
    {
      private readonly ObservableList<string> _observable;

      public Replace()
      {
        _observable = new ObservableList<string>();
      }

      [Fact]
      public void Should_Not_Throw_When_Replacing_Item()
      {
        Invoking(
            () => { _observable.Replace(Create<string>()); }
          )
          .Should()
          .NotThrow();
      }

      [Fact]
      public void Should_Replace_Collection()
      {
        var itemsToAdd = CreateMany<string>();
        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemsToAdd.Count);

        var newItem = Create<string>();
        _observable.Replace(newItem);

        _observable.Should().BeEquivalentTo(newItem);
      }

      [Fact]
      public void Should_Replace_Collection_When_New_Item_Is_Already_Present()
      {
        var itemsToAdd = CreateMany<string>();
        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemsToAdd.Count);

        var newItem = itemsToAdd.Last();
        _observable.Replace(newItem);

        _observable.Should().BeEquivalentTo(newItem);
      }

      [Fact]
      public void Should_Raise_Reset_Notification_When_Replacing()
      {
        _observable.AddRange(CreateMany<string>());

        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          args.Action.Should().Be(NotifyCollectionChangedAction.Reset);
          notificationRaised = true;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.Replace(Create<string>());

          notificationRaised.Should().BeTrue();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Fact]
      public void Should_Not_Allow_Reentry()
      {
        void OnCollectionChanged1(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        void OnCollectionChanged2(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        // requires at least two observers before the reentry error will be raised
        _observable.CollectionChanged += OnCollectionChanged1;
        _observable.CollectionChanged += OnCollectionChanged2;

        try
        {
          Invoking(
              () =>
              {
                _observable.Replace(Create<string>());
              }
            )
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Cannot change ObservableCollection during a CollectionChanged event.");
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged1;
          _observable.CollectionChanged -= OnCollectionChanged2;
        }
      }
    }
    
    public class ReplaceRange : ObservableListFixture
    {
      private readonly ObservableList<string> _observable;

      public ReplaceRange()
      {
        _observable = new ObservableList<string>();
      }

      [Fact]
      public void Should_Throw_When_Collection_Null()
      {
        Invoking(
            () => { _observable.ReplaceRange(null); }
          )
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage("Value cannot be null. (Parameter 'collection')");
      }

      [Fact]
      public void Should_Replace_Collection()
      {
        var itemsToAdd = CreateMany<string>();
        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemsToAdd.Count);

        var newItems = CreateMany<string>();
        _observable.ReplaceRange(newItems);

        _observable.Should().BeEquivalentTo(newItems);
      }

      [Fact]
      public void Should_Replace_Collection_When_New_Items_Are_Already_Present()
      {
        var itemsToAdd = CreateMany<string>();
        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemsToAdd.Count);

        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          args.Action.Should().Be(NotifyCollectionChangedAction.Reset);
          notificationRaised = true;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.ReplaceRange(itemsToAdd);

          _observable.Should().BeEquivalentTo(itemsToAdd);
          notificationRaised.Should().BeTrue();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Fact]
      public void Should_Raise_Reset_Notification_When_Replacing()
      {
        _observable.AddRange(CreateMany<string>());

        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          args.Action.Should().Be(NotifyCollectionChangedAction.Reset);
          notificationRaised = true;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.ReplaceRange(CreateMany<string>());

          notificationRaised.Should().BeTrue();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Fact]
      public void Should_Not_Allow_Reentry()
      {
        void OnCollectionChanged1(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        void OnCollectionChanged2(object sender, NotifyCollectionChangedEventArgs args)
        {
          _observable.AddRange(CreateMany<string>());
        }

        // requires at least two observers before the reentry error will be raised
        _observable.CollectionChanged += OnCollectionChanged1;
        _observable.CollectionChanged += OnCollectionChanged2;

        try
        {
          Invoking(
              () =>
              {
                _observable.ReplaceRange(CreateMany<string>());
              }
            )
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Cannot change ObservableCollection during a CollectionChanged event.");
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged1;
          _observable.CollectionChanged -= OnCollectionChanged2;
        }
      }

      // not raise when empty and replacing with empty
      [Fact]
      public void Should_Not_Raise_Notification_When_Replacing_Empty_With_Empty()
      {
        var notificationRaised = false;

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
          notificationRaised = true;
        }

        _observable.CollectionChanged += OnCollectionChanged;

        try
        {
          _observable.ReplaceRange(new string[] { });

          notificationRaised.Should().BeFalse();
        }
        finally
        {
          _observable.CollectionChanged -= OnCollectionChanged;
        }
      }

      [Fact]
      public void Should_not_Mutate_Source_When_Source_Is_Present()
      {
        var itemsToAdd = CreateMany<string>();
        var itemsToReplace = itemsToAdd.ToList();          // want a mutable copy
        var itemCount = itemsToReplace.Count;

        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemCount);

        _observable.ReplaceRange(itemsToReplace);

        itemsToReplace.Should().HaveCount(itemCount);
      }

      [Fact]
      public void Should_not_Mutate_Source_When_Source_Is_Not_Present()
      {
        // the strings are GUIDs so they will all be unique
        var itemsToAdd = CreateMany<string>();
        var itemsToReplace = CreateMany<string>().AsList();  // want a mutable version
        var itemCount = itemsToReplace.Count;

        _observable.AddRange(itemsToAdd);

        _observable.Should().HaveCount(itemCount);

        _observable.ReplaceRange(itemsToReplace);

        itemsToReplace.Should().HaveCount(itemCount);
      }
    }
  }
}
