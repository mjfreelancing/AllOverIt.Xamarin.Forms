using AllOverIt.XamarinForms.Behaviors;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Behaviors
{
  public class SelectedItemCommandBehaviorFixture : AllOverItFixtureBase
  {
    private class DummyCommand : ICommand
    {
      private readonly int _allowedValue;

      public event EventHandler CanExecuteChanged = delegate { };
      public object Parameter { get; private set; }

      public DummyCommand(int allowedValue)
      {
        _allowedValue = allowedValue;
      }

      public bool CanExecute(object parameter)
      {
        // cater for some tests using a converter
        if (parameter is SelectedItemChangedEventArgs eventArgs)
        {
          return _allowedValue == (int)eventArgs.SelectedItem;
        }

        return _allowedValue == (int)parameter;
      }

      public void Execute(object parameter)
      {
        // cater for some tests using a converter
        if (parameter is SelectedItemChangedEventArgs eventArgs)
        {
          Parameter = (int)eventArgs.SelectedItem;
        }
        else
        {
          Parameter = parameter;
        }
      }
    }

    private class MultiplierConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var selectedItem = (value as SelectedItemChangedEventArgs).SelectedItem;
        var multiplier = (int?) parameter ?? 2;

        return (int)selectedItem * multiplier;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }

    public SelectedItemCommandBehaviorFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void Should_Attach_Behavior()
    {
      var (listView, _) = CreateListViewWithAttachedBehavior();

      listView.Behaviors.Single().Should().BeOfType<SelectedItemCommandBehavior>();
    }

    [Fact]
    public void Should_Detach_Behavior()
    {
      var (listView, behavior) = CreateListViewWithAttachedBehavior();

      behavior.AssociatedObject.Should().Be(listView);

      listView.Behaviors.Clear();

      behavior.AssociatedObject.Should().BeNull();
    }

    [Fact]
    public void Should_Get_Default_Command()
    {
      var behavior = new SelectedItemCommandBehavior();

      var actual = behavior.Command;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_Command()
    {
      var behavior = new SelectedItemCommandBehavior();

      var expected = A.Fake<ICommand>();

      behavior.Command = expected;

      var actual = behavior.Command;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_Converter()
    {
      var behavior = new SelectedItemCommandBehavior();

      var actual = behavior.Converter;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_Converter()
    {
      var behavior = new SelectedItemCommandBehavior();

      var expected = A.Fake<IValueConverter>();

      behavior.Converter = expected;

      var actual = behavior.Converter;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_ConverterParameter()
    {
      var behavior = new SelectedItemCommandBehavior();

      var actual = behavior.ConverterParameter;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_ConverterParameter()
    {
      var behavior = new SelectedItemCommandBehavior();

      var expected = new { };

      behavior.ConverterParameter = expected;

      var actual = behavior.ConverterParameter;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Throw_When_No_Command_To_Execute()
    {
      var (listView, _) = CreateListViewWithAttachedBehavior();

      var dataSource = CreateMany<int>();
      listView.BindingContext = dataSource;

      Invoking(() =>
        {
          listView.SelectedItem = dataSource.First();
        })
        .Should()
        .Throw<InvalidOperationException>()
        .WithMessage("No Command has been provided");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Should_Allow_Execute_Command_When_Attached(bool allowExecute)
    {
      var (listView, behavior) = CreateListViewWithAttachedBehavior();

      var dataSource = CreateMany<int>();
      listView.BindingContext = dataSource;

      var compareTo = allowExecute ? dataSource.First() : dataSource.Last();
      var command = new DummyCommand(compareTo);

      behavior.Command = command;

      listView.SelectedItem = dataSource.First();

      var expected = allowExecute ? compareTo : (object) null;

      command.Parameter.Should().Be(expected);
    }

    [Fact]
    public void Should_Apply_Converter()
    {
      var (listView, behavior) = CreateListViewWithAttachedBehavior();

      var dataSource = CreateMany<int>();
      listView.BindingContext = dataSource;

      var expected = dataSource.First() * 2;

      var command = new DummyCommand(expected);

      behavior.Command = command;
      behavior.Converter = new MultiplierConverter();

      listView.SelectedItem = dataSource.First();

      command.Parameter.Should().Be(expected);
    }

    [Fact]
    public void Should_Apply_Converter_And_Parameter()
    {
      var (listView, behavior) = CreateListViewWithAttachedBehavior();

      var dataSource = CreateMany<int>();
      listView.BindingContext = dataSource;

      var expected = dataSource.First() * 3;

      var command = new DummyCommand(expected);

      behavior.Command = command;
      behavior.Converter = new MultiplierConverter();
      behavior.ConverterParameter = 3;

      listView.SelectedItem = dataSource.First();

      command.Parameter.Should().Be(expected);
    }

    private static (ListView listView, SelectedItemCommandBehavior behavior) CreateListViewWithAttachedBehavior()
    {
      var listView = new ListView();
      var behavior = new SelectedItemCommandBehavior();

      listView.Behaviors.Add(behavior);

      return (listView, behavior);
    }
  }
}