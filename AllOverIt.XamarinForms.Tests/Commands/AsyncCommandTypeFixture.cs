using AllOverIt.XamarinForms.Commands;
using AllOverIt.XamarinForms.Exceptions;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Commands
{
  public class AsyncCommandTypeFixture : AllOverItFixtureBase
  {
    public class Constructor_Execute : AsyncCommandTypeFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Invoking(() =>
          {
            Func<string, Task> execute = null;

            _ = new AsyncCommand<string>(execute);
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("execute"));
      }

      [Fact]
      public async Task Should_Execute()
      {
        var expected = Create<string>();
        var actual = string.Empty;

        Func<string, Task> execute = param =>
        {
          actual = param;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand<string>(execute);

        await command.ExecuteAsync(expected).ConfigureAwait(false);

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Default_CanExecute_True()
      {
        Func<string, Task> execute = param => Task.FromResult(true);

        var command = new AsyncCommand<string>(execute);

        var actual = command.CanExecute(Create<string>());

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Default_No_Exception_Handler()
      {
        var exception = new Exception(Create<string>());

        Awaiting(async () =>
          {
            Func<string, Task> execute = param => Task.FromException(exception);

            var command = new AsyncCommand<string>(execute);

            await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);
          })
          .Should()
          .Throw<Exception>()
          .WithMessage(exception.Message);
      }

      [Fact]
      public async Task Should_Register_Changed_Event()
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute);

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Fact]
      public async Task Should_Unregister_Changed_Event()
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute);

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    //same tests as Constructor_Execute - can't test 'continueOnCapturedContext' argument
    public class Constructor_Execute_Context : AsyncCommandTypeFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Invoking(() =>
          {
            Func<string, Task> execute = null;

            _ = new AsyncCommand<string>(execute, Create<bool>());
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("execute"));
      }

      [Fact]
      public async Task Should_Execute()
      {
        var expected = Create<string>();
        var actual = string.Empty;

        Func<string, Task> execute = param =>
        {
          actual = param;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand<string>(execute, Create<bool>());

        await command.ExecuteAsync(expected).ConfigureAwait(false);

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Default_CanExecute_True()
      {
        Func<string, Task> execute = param => Task.FromResult(true);

        var command = new AsyncCommand<string>(execute, Create<bool>());

        var actual = command.CanExecute(Create<string>());

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Default_No_Exception_Handler()
      {
        var exception = new Exception(Create<string>());

        Awaiting(async () =>
          {
            Func<string, Task> execute = param => Task.FromException(exception);

            var command = new AsyncCommand<string>(execute, Create<bool>());

            await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);
          })
          .Should()
          .Throw<Exception>()
          .WithMessage(exception.Message);
      }

      [Fact]
      public async Task Should_Register_Changed_Event()
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute, Create<bool>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Fact]
      public async Task Should_Unregister_Changed_Event()
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute, Create<bool>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    public class Constructor_Execute_CanExecute_Exception_Handler : AsyncCommandTypeFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Func<string, Task> execute = null;

        Invoking(() =>
          {
            _ = new AsyncCommand<string>(execute, param => Create<bool>(), Create<bool>(), A.Fake<IExceptionHandler>());
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("execute"));
      }

      [Theory]
      [InlineData(false, false)]
      [InlineData(false, true)]
      [InlineData(true, false)]
      [InlineData(true, true)]
      public async Task Should_Execute(bool canExecute, bool continueOnCapturedContext)
      {
        var value = Create<string>();
        var actual = string.Empty;
        var expected = canExecute ? value : string.Empty;

        Func<string, Task> execute = param =>
        {
          actual = param;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand<string>(execute, param => canExecute, continueOnCapturedContext, A.Fake<IExceptionHandler>());

        await command.ExecuteAsync(value).ConfigureAwait(false);

        actual.Should().Be(expected);
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Throw_When_No_Exception_Handler(bool continueOnCapturedContext)
      {
        Invoking(() =>
          {
            Func<string, Task> execute = param => Task.FromResult(true);

            _ = new AsyncCommand<string>(execute, param => true, continueOnCapturedContext, (IExceptionHandler)null);
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("exceptionHandler"));
      }

      [Fact]
      public async Task Should_Execute_Exception_Handler()
      {
        var exceptionHandler = A.Fake<IExceptionHandler>();

        var exception = new Exception(Create<string>());

        Func<string, Task> execute = param => Task.FromException(exception);

        var command = new AsyncCommand<string>(execute, param => true, Create<bool>(), exceptionHandler);

        await command.ExecuteAsync(Create<string>());

        A.CallTo(() => exceptionHandler.Handle(exception))
          .MustHaveHappened();
      }

      [Theory]
      [InlineData(false, false)]
      [InlineData(false, true)]
      [InlineData(true, false)]
      [InlineData(true, true)]
      public async Task Should_Register_Changed_Event(bool canExecute, bool continueOnCapturedContext)
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute, param => canExecute, continueOnCapturedContext, A.Fake<IExceptionHandler>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Theory]
      [InlineData(false, false)]
      [InlineData(false, true)]
      [InlineData(true, false)]
      [InlineData(true, true)]
      public async Task Should_Unregister_Changed_Event(bool canExecute, bool continueOnCapturedContext)
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute, param => canExecute, continueOnCapturedContext, A.Fake<IExceptionHandler>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    public class Constructor_Execute_CanExecute_Action_Handler : AsyncCommandTypeFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Func<string, Task> execute = null;

        Invoking(() =>
          {
            _ = new AsyncCommand<string>(execute, param => Create<bool>(), Create<bool>(), exception => { });
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("execute"));
      }

      [Theory]
      [InlineData(false, false)]
      [InlineData(false, true)]
      [InlineData(true, false)]
      [InlineData(true, true)]
      public async Task Should_Execute(bool canExecute, bool continueOnCapturedContext)
      {
        var value = Create<string>();
        var expected = canExecute ? value : string.Empty;
        var actual = string.Empty;

        Func<string, Task> execute = param =>
        {
          actual = param;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand<string>(execute, param => canExecute, continueOnCapturedContext, exception => { });

        await command.ExecuteAsync(expected).ConfigureAwait(false);

        actual.Should().Be(expected);
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Throw_When_No_Exception_Action_Handler(bool continueOnCapturedContext)
      {
        Invoking(() =>
          {
            Func<string, Task> execute = param => Task.FromResult(true);

            _ = new AsyncCommand<string>(execute, param => true, continueOnCapturedContext, (Action<Exception>)null);
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("exceptionHandler"));
      }

      [Fact]
      public async Task Should_Execute_Exception_Action_Handler()
      {
        Exception capturedException = null;

        Action<Exception> exceptionHandler = ex =>
        {
          capturedException = ex;
        };

        var exception = new Exception(Create<string>());

        Func<string, Task> execute = param => Task.FromException(exception);

        var command = new AsyncCommand<string>(execute, param => true, Create<bool>(), exceptionHandler);

        await command.ExecuteAsync(Create<string>());

        capturedException.Should().BeSameAs(exception);
      }

      [Theory]
      [InlineData(false, false)]
      [InlineData(false, true)]
      [InlineData(true, false)]
      [InlineData(true, true)]
      public async Task Should_Register_Changed_Event(bool canExecute, bool continueOnCapturedContext)
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute, param => canExecute, continueOnCapturedContext, exception => { });

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Theory]
      [InlineData(false, false)]
      [InlineData(false, true)]
      [InlineData(true, false)]
      [InlineData(true, true)]
      public async Task Should_Unregister_Changed_Event(bool canExecute, bool continueOnCapturedContext)
      {
        var executed = false;
        Func<string, Task> execute = param => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand<string>(execute, param => canExecute, continueOnCapturedContext, exception => { });

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync(Create<string>()).ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    public class ICommand_CanExecute_Fixture : AsyncCommandTypeFixture
    {
      [Fact]
      public void Should_Call_CanExecute()
      {
        Func<string, Task> execute = param => Task.FromResult(true);

        var actual = false;

        Func<string, bool> canExecute = param =>
        {
          actual = true;
          return Create<bool>();    // randomly invoke 'execute'
        };

        ICommand command = new AsyncCommand<string>(execute, canExecute, Create<bool>(), exception => { });

        command.CanExecute(Create<string>());

        actual.Should().BeTrue();
      }
    }

    public class ICommand_Execute_Fixture : AsyncCommandTypeFixture
    {
      [Fact]
      public void Should_Call_Execute()
      {
        var expected = Create<string>();
        var actual = string.Empty;

        Func<string, Task> execute = param =>
        {
          actual = param;
          return Task.FromResult(true);
        };

        ICommand command = new AsyncCommand<string>(execute, param => true, Create<bool>(), exception => { });

        command.Execute(expected);

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Call_Exception_Handler()
      {
        var exception = new Exception(Create<string>());
        Exception capturedException = null;

        Func<string, Task> execute = param => Task.FromException(exception);

        ICommand command = new AsyncCommand<string>(execute, param => true, Create<bool>(), ex =>
        {
          capturedException = ex;
        });

        command.Execute(Create<string>());

        capturedException.Should().BeSameAs(exception);
      }

      //[Fact]
      //public async Task Should_Not_Throw_Exception()
      //{
      //  var message = Create<string>();
      //  var exception = new Exception(message);

      //  Func<string, Task> execute = param => Task.FromException(exception);

      //  // even though this constructor does not use an exception handler the
      //  // thrown exception will not be propagated (async void)
      //  ICommand command = new AsyncCommand<string>(execute);

      //  // **** this would result in an unhandled exception that cannot be caught in a test ****
      //  command.Execute(Create<string>());
      //}
    }
  }
}