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
  public class AsyncCommandFixture : AllOverItFixtureBase
  {
    public class Constructor_Execute : AsyncCommandFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Invoking(() =>
          {
            Func<Task> execute = null;

            _ = new AsyncCommand(execute);
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("execute"));
      }

      [Fact]
      public async Task Should_Execute()
      {
        var executed = false;

        Func<Task> execute = () =>
        {
          executed = true;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand(execute);

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Fact]
      public void Should_Default_CanExecute_True()
      {
        Func<Task> execute = () => Task.FromResult(true);

        var command = new AsyncCommand(execute);

        var actual = command.CanExecute();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Default_No_Exception_Handler()
      {
        var exception = new Exception(Create<string>());

        Awaiting(async () =>
          {
            Func<Task> execute = () => Task.FromException(exception);

            var command = new AsyncCommand(execute);

            await command.ExecuteAsync().ConfigureAwait(false);
          })
          .Should()
          .Throw<Exception>()
          .WithMessage(exception.Message);
      }

      [Fact]
      public async Task Should_Register_Changed_Event()
      {
        var executed = false;
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute);

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Fact]
      public async Task Should_Unregister_Changed_Event()
      {
        var executed = false;
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute);

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    // same tests as Constructor_Execute - can't test 'continueOnCapturedContext' argument
    public class Constructor_Execute_Context : AsyncCommandFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Invoking(() =>
          {
            Func<Task> execute = null;

            _ = new AsyncCommand(execute, Create<bool>());
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("execute"));
      }

      [Fact]
      public async Task Should_Execute()
      {
        var executed = false;

        Func<Task> execute = () =>
        {
          executed = true;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand(execute, Create<bool>());

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Fact]
      public void Should_Default_CanExecute_True()
      {
        Func<Task> execute = () => Task.FromResult(true);

        var command = new AsyncCommand(execute, Create<bool>());

        var actual = command.CanExecute();

        actual.Should().BeTrue();
      }

      [Fact]
      public void Should_Default_No_Exception_Handler()
      {
        var exception = new Exception(Create<string>());

        Awaiting(async () =>
        {
          Func<Task> execute = () => Task.FromException(exception);

          var command = new AsyncCommand(execute, Create<bool>());

          await command.ExecuteAsync().ConfigureAwait(false);
        })
          .Should()
          .Throw<Exception>()
          .WithMessage(exception.Message);
      }

      [Fact]
      public async Task Should_Register_Changed_Event()
      {
        var executed = false;
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute, Create<bool>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();
      }

      [Fact]
      public async Task Should_Unregister_Changed_Event()
      {
        var executed = false;
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute, Create<bool>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    public class Constructor_Execute_CanExecute_Exception_Handler : AsyncCommandFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Func<Task> execute = null;

        Invoking(() =>
          {
            _ = new AsyncCommand(execute, Create<bool>, Create<bool>(), A.Fake<IExceptionHandler>());
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
        var executed = false;

        Func<Task> execute = () =>
        {
          executed = true;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand(execute, () => canExecute, continueOnCapturedContext, A.Fake<IExceptionHandler>());

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().Be(canExecute);
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Throw_When_No_Exception_Handler(bool continueOnCapturedContext)
      {
        Invoking(() =>
          {
            Func<Task> execute = () => Task.FromResult(true);

            _ = new AsyncCommand(execute, () => true, continueOnCapturedContext, (IExceptionHandler)null);
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

        Func<Task> execute = () => Task.FromException(exception);

        var command = new AsyncCommand(execute, () => true, Create<bool>(), exceptionHandler);

        await command.ExecuteAsync();

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
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute, () => canExecute, continueOnCapturedContext, A.Fake<IExceptionHandler>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

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
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute, () => canExecute, continueOnCapturedContext, A.Fake<IExceptionHandler>());

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    public class Constructor_Execute_CanExecute_Action_Handler : AsyncCommandFixture
    {
      [Fact]
      public void Should_Throw_When_Execute_Null()
      {
        Func<Task> execute = null;

        Invoking(() =>
        {
          _ = new AsyncCommand(execute, Create<bool>, Create<bool>(), exception => { });
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
        var executed = false;

        Func<Task> execute = () =>
        {
          executed = true;
          return Task.FromResult(true);
        };

        var command = new AsyncCommand(execute, () => canExecute, continueOnCapturedContext, exception => { });

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().Be(canExecute);
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public void Should_Throw_When_No_Exception_Action_Handler(bool continueOnCapturedContext)
      {
        Invoking(() =>
          {
            Func<Task> execute = () => Task.FromResult(true);

            _ = new AsyncCommand(execute, () => true, continueOnCapturedContext, (Action<Exception>)null);
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

        Func<Task> execute = () => Task.FromException(exception);

        var command = new AsyncCommand(execute, () => true, Create<bool>(), exceptionHandler);

        await command.ExecuteAsync();

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
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute, () => canExecute, continueOnCapturedContext, exception => { });

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

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
        Func<Task> execute = () => Task.FromResult(true);

        void OnChangedHandler(object sender, EventArgs args)
        {
          executed = true;
        }

        var command = new AsyncCommand(execute, () => canExecute, continueOnCapturedContext, exception => { });

        command.CanExecuteChanged += OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeTrue();

        executed = false;
        command.CanExecuteChanged -= OnChangedHandler;

        await command.ExecuteAsync().ConfigureAwait(false);

        executed.Should().BeFalse();
      }
    }

    public class ICommand_CanExecute_Fixture : AsyncCommandFixture
    {
      [Fact]
      public void Should_Call_CanExecute()
      {
        Func<Task> execute = () => Task.FromResult(true);

        var actual = false;

        Func<bool> canExecute = () =>
        {
          actual = true;
          return Create<bool>();    // randomly invoke 'execute'
        };

        ICommand command = new AsyncCommand(execute, canExecute, Create<bool>(), exception => { });

        command.CanExecute(new { });

        actual.Should().BeTrue();
      }
    }

    public class ICommand_Execute_Fixture : AsyncCommandFixture
    {
      [Fact]
      public void Should_Call_Execute()
      {
        var executed = false;

        Func<Task> execute = () =>
        {
          executed = true;
          return Task.FromResult(true);
        };

        ICommand command = new AsyncCommand(execute, () => true, Create<bool>(), exception => { });

        command.Execute(new { });

        executed.Should().BeTrue();
      }

      [Fact]
      public void Should_Call_Exception_Handler()
      {
        var exception = new Exception(Create<string>());
        Exception capturedException = null;

        Func<Task> execute = () => Task.FromException(exception);

        ICommand command = new AsyncCommand(execute, () => true, Create<bool>(), ex =>
        {
          capturedException = ex;
        });

        command.Execute(new { });

        capturedException.Should().BeSameAs(exception);
      }

      //[Fact]
      //public async Task Should_Not_Throw_Exception()
      //{
      //  var message = Create<string>();
      //  var exception = new Exception(message);

      //  Func<Task> execute = () => Task.FromException(exception);

      //  // even though this constructor does not use an exception handler the
      //  // thrown exception will not be propagated (async void)
      //  ICommand command = new AsyncCommand(execute);

      //  // **** this would result in an unhandled exception that cannot be caught in a test ****
      //  command.Execute(new { });
      //}
    }
  }
}