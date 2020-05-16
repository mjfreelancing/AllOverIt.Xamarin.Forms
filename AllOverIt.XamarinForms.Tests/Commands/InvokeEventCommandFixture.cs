using AllOverIt.XamarinForms.Commands;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Commands
{
  public class InvokeEventCommandFixture : AllOverItFixtureBase
  {
    private class DummyCommand : ICommand
    {
      public event EventHandler CanExecuteChanged = delegate { };
      public object Parameter { get; private set; }

      public bool CanExecute(object parameter)
      {
        return parameter != null;
      }

      public void Execute(object parameter)
      {
        Parameter = parameter;
      }
    }

    private class UpperConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        // default is to upper case
        // if parameter is a bool then selectively convert
        var upper = ((string)value).ToUpper();

        if (parameter is bool doConversion)
        {
          return doConversion 
            ? upper
            : value;
        }

        return upper;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        throw new NotImplementedException();
      }
    }

    public InvokeEventCommandFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void Should_Get_Default_Command()
    {
      var eventCommand = new InvokeEventCommand();

      var actual = eventCommand.Command;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_Command()
    {
      var eventCommand = new InvokeEventCommand();

      var expected = A.Fake<ICommand>();

      eventCommand.Command = expected;

      var actual = eventCommand.Command;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_CommandParameter()
    {
      var eventCommand = new InvokeEventCommand();

      var actual = eventCommand.CommandParameter;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_CommandParameter()
    {
      var eventCommand = new InvokeEventCommand();

      var expected = new { };

      eventCommand.CommandParameter = expected;

      var actual = eventCommand.CommandParameter;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_Converter()
    {
      var eventCommand = new InvokeEventCommand();

      var actual = eventCommand.Converter;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_Converter()
    {
      var eventCommand = new InvokeEventCommand();

      var expected = A.Fake<IValueConverter>();

      eventCommand.Converter = expected;

      var actual = eventCommand.Converter;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_ConverterParameter()
    {
      var eventCommand = new InvokeEventCommand();

      var actual = eventCommand.ConverterParameter;

      actual.Should().BeNull();
    }

    [Fact]
    public void Should_Set_ConverterParameter()
    {
      var eventCommand = new InvokeEventCommand();

      var expected = new { };

      eventCommand.ConverterParameter = expected;

      var actual = eventCommand.ConverterParameter;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Not_Execute_When_No_Command()
    {
      var eventCommand = new InvokeEventCommand();

      var actual = eventCommand.Execute(null, null);

      actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Should_Execute_When_Command_CanExecute(bool canExecute)
    {
      var eventCommand = new InvokeEventCommand
      {
        Command = new DummyCommand()
      };

      var value = canExecute ? Create<string>() : null;
      var actual = eventCommand.Execute(null, value);

      actual.Should().Be(canExecute);
    }

    [Fact]
    public void Should_Execute_With_Parameter()
    {
      var command = new DummyCommand();

      var eventCommand = new InvokeEventCommand
      {
        Command = command
      };

      var expected = Create<string>();
      var actual = eventCommand.Execute(null, expected);

      actual.Should().BeTrue();
      command.Parameter.Should().Be(expected);
    }

    [Fact]
    public void Should_Execute_With_CommandParameter()
    {
      var command = new DummyCommand();
      var expected = Create<string>();

      var eventCommand = new InvokeEventCommand
      {
        Command = command,
        CommandParameter = expected
      };

      // CommandParameter is passed to the command in preference to the Execute() parameter
      var actual = eventCommand.Execute(null, Create<string>());

      actual.Should().BeTrue();
      command.Parameter.Should().Be(expected);
      command.Parameter.Should().Be(eventCommand.CommandParameter);
    }

    [Fact]
    public void Should_Execute_With_Parameter_And_Converter()
    {
      var command = new DummyCommand();

      var eventCommand = new InvokeEventCommand
      {
        Command = command,
        Converter = new UpperConverter()
      };

      var expected = Create<string>().ToUpper();
      var actual = eventCommand.Execute(null, expected.ToLower());

      actual.Should().BeTrue();
      command.Parameter.Should().Be(expected);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Should_Execute_With_Parameter_And_Converter_Parameter(bool converterParameter)
    {
      var command = new DummyCommand();

      var eventCommand = new InvokeEventCommand
      {
        Command = command,
        Converter = new UpperConverter(),
        ConverterParameter = converterParameter
      };

      var expected = Create<string>().ToLower();

      if (converterParameter)
      {
        expected = expected.ToUpper();
      }

      var actual = eventCommand.Execute(null, expected);

      actual.Should().BeTrue();
      command.Parameter.Should().Be(expected);
    }
  }
}