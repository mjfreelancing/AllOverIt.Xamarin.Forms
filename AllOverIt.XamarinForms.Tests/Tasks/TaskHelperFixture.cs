using AllOverIt.XamarinForms.Exceptions;
using AllOverIt.XamarinForms.Tasks;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Tasks
{
  // SetDefaultExceptionHandler() updates a static - tests need to run sequentially
  [CollectionDefinition(nameof(DefaultExceptionHandlerCollection), DisableParallelization = true)]
  public class DefaultExceptionHandlerCollection { }

  public class TaskHelperFixture : AllOverItFixtureBase
  {
    [Collection(nameof(DefaultExceptionHandlerCollection))]
    public class SetDefaultExceptionHandler : TaskHelperFixture
    {
      [Fact(Timeout = 5000)]
      public async Task Should_Set_Default_Handler()
      {
        var tcs = new TaskCompletionSource<bool>();
        Exception handledException = null;

        TaskHelper.SetDefaultExceptionHandler(exception =>
        {
          handledException = exception;

          Task.Run(() => tcs.TrySetResult(true));
        });

        var exception = new ArgumentException();

        TaskHelper.SafeFireAndForget(DelayThenThrow(exception));

        await tcs.Task.ConfigureAwait(false);

        handledException.Should().BeSameAs(exception);
      }
    }

    [Collection(nameof(DefaultExceptionHandlerCollection))]
    public class SafeFireAndForget : TaskHelperFixture
    {
      [Fact(Timeout = 5000)]
      public async Task Should_Run_To_Completion()
      {
        var expected = Create<bool>();
        var tcs = new TaskCompletionSource<bool>();
        Exception handledException = null;

        var task = Task.Run(async () =>
        {
          await Task.Delay(10).ConfigureAwait(false);

          return tcs.TrySetResult(expected);
        });

        TaskHelper.SafeFireAndForget(task, ex =>
        {
          handledException = ex;
        });

        await tcs.Task.ConfigureAwait(false);

        var actual = await task;

        actual.Should().Be(expected);
        handledException.Should().BeNull();
      }

      [Fact(Timeout = 5000)]
      public async Task Should_Execute_Default_Handler()
      {
        var tcs = new TaskCompletionSource<bool>();
        Exception handledException = null;

        TaskHelper.SetDefaultExceptionHandler(exception =>
        {
          handledException = exception;

          Task.Run(() => tcs.TrySetResult(true));
        });

        var exception = new ArgumentException();

        TaskHelper.SafeFireAndForget(DelayThenThrow(exception));

        await tcs.Task.ConfigureAwait(false);

        handledException.Should().BeSameAs(exception);
      }

      [Fact(Timeout = 5000)]
      public async Task Should_Execute_Action_Handler()
      {
        var tcs = new TaskCompletionSource<bool>();
        Exception handledException = null;

        var exception = new ArgumentException();

        TaskHelper.SafeFireAndForget(DelayThenThrow(exception), ex =>
        {
          handledException = ex;

          Task.Run(() => tcs.TrySetResult(true));
        });

        await tcs.Task.ConfigureAwait(false);

        handledException.Should().BeSameAs(exception);
      }

      [Fact(Timeout = 5000)]
      public async Task Should_Execute_Exception_Handler()
      {
        var tcs = new TaskCompletionSource<bool>();
        Exception handledException = null;

        var exception = new ArgumentException();

        var handlerFake = A.Fake<IExceptionHandler>();
        A.CallTo(() => handlerFake.Handle(exception)).Invokes(fake =>
        {
          handledException = exception;

          Task.Run(() => tcs.TrySetResult(true));
        });

        TaskHelper.SafeFireAndForget(DelayThenThrow(exception), handlerFake);

        await tcs.Task.ConfigureAwait(false);

        handledException.Should().BeSameAs(exception);
      }

      [Fact(Timeout = 5000)]
      public async Task Should_Execute_Default_And_Action_Handler()
      {
        var tcs1 = new TaskCompletionSource<bool>();
        var tcs2 = new TaskCompletionSource<bool>();
        Exception handledException1 = null;
        Exception handledException2 = null;

        TaskHelper.SetDefaultExceptionHandler(ex =>
        {
          handledException1 = ex;
          tcs1.TrySetResult(true);
        });

        var exception = new ArgumentException();

        TaskHelper.SafeFireAndForget(DelayThenThrow(exception), ex =>
        {
          handledException2 = ex;
          tcs2.TrySetResult(true);
        }, true);

        await Task.WhenAll(tcs1.Task, tcs2.Task).ConfigureAwait(false);

        handledException1.Should().BeSameAs(exception);
        handledException1.Should().BeSameAs(handledException2);
      }
    }

    private static async Task DelayThenThrow(ArgumentException exception)
    {
      await Task.Delay(10).ConfigureAwait(false);
      throw exception;
    }
  }
}