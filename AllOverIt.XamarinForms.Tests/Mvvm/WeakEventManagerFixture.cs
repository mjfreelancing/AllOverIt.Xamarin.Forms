using AllOverIt.XamarinForms.Mvvm;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Mvvm
{
  public class WeakEventManagerFixture : AllOverItFixtureBase
  {
    public class DummyStringArgs : EventArgs
    {
      public string Actual { get; set; }
      public string Expected { get; set; }
    }

    public class AddEventHandler_EventArgs : WeakEventManagerFixture
    {
      public EventHandler<DummyStringArgs> FixtureStringArgsHandler;

      [Fact]
      public void Should_Throw_When_Handler_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
          {
            subject.AddEventHandler((EventHandler<EventArgs>) null);

          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("handler"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
          {
            EventHandler<EventArgs> handler = (sender, args) => { };

            subject.AddEventHandler(handler, null);
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Empty()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
          {
            EventHandler<EventArgs> handler = (sender, args) => { };

            subject.AddEventHandler(handler, string.Empty);
          })
          .Should()
          .Throw<ArgumentException>()
          .WithMessage(GetExpectedArgumentEmptyExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Add_Handler()
      {
        var stringArgs = new DummyStringArgs { Expected = Create<string>() };

        FixtureStringArgsHandler += (sender, args) =>
        {
          args.Actual = args.Expected;
        };

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureStringArgsHandler);
        subject.HandleEvent(this, stringArgs, nameof(Should_Add_Handler));

        stringArgs.Actual.Should().Be(stringArgs.Expected);
      }

      [Fact]
      public void Should_Add_Static_Handler()
      {
        var stringArgs = new DummyStringArgs { Expected = Create<string>() };

        FixtureStringArgsHandler += StaticHandler;

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureStringArgsHandler);
        subject.HandleEvent(this, stringArgs, nameof(Should_Add_Static_Handler));

        stringArgs.Actual.Should().Be(stringArgs.Expected);
      }

      private static void StaticHandler(object sender, DummyStringArgs args)
      {
        args.Actual = args.Expected;
      }
    }

    public class AddEventHandler : WeakEventManagerFixture
    {
      public EventHandler FixtureEventArgsHandler;

      [Fact]
      public void Should_Throw_When_Handler_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
        {
          subject.AddEventHandler((EventHandler)null);

        })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("handler"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
        {
          EventHandler handler = (sender, args) => { };

          subject.AddEventHandler(handler, null);
        })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Empty()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
        {
          EventHandler handler = (sender, args) => { };

          subject.AddEventHandler(handler, string.Empty);
        })
          .Should()
          .Throw<ArgumentException>()
          .WithMessage(GetExpectedArgumentEmptyExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Add_Handler()
      {
        var expectedArgs = new EventArgs();
        EventArgs actualArgs = null;

        FixtureEventArgsHandler += (sender, args) =>
        {
          actualArgs = args;
        };

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureEventArgsHandler);
        subject.HandleEvent(this, expectedArgs, nameof(Should_Add_Handler));

        actualArgs.Should().BeSameAs(expectedArgs);
      }
    }

    public class HandleEvent : WeakEventManagerFixture
    {
      public EventHandler<DummyStringArgs> FixtureStringArgsHandler;
      
      [Fact]
      public void Should_Not_Throw_When_Sender_Null()
      {
        Invoking(() =>
          {
            var subject = new WeakEventManager();

            subject.HandleEvent(null, new { }, Create<string>());
          })
          .Should()
          .NotThrow();
      }

      [Fact]
      public void Should_Not_Throw_When_Args_Null()
      {
        Invoking(() =>
          {
            var subject = new WeakEventManager();

            subject.HandleEvent(this, null, Create<string>());
          })
          .Should()
          .NotThrow();
      }

      [Fact]
      public void Should_Throw_When_EventName_Null()
      {
        Invoking(() =>
          {
            var subject = new WeakEventManager();

            subject.HandleEvent(this, new { }, null);
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Empty()
      {
        Invoking(() =>
          {
            var subject = new WeakEventManager();

            subject.HandleEvent(this, new { }, string.Empty);
          })
          .Should()
          .Throw<ArgumentException>()
          .WithMessage(GetExpectedArgumentEmptyExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Handle_Instance_Handler()
      {
        var expectedArgs = new DummyStringArgs();
        DummyStringArgs actualArgs = null;

        FixtureStringArgsHandler += (sender, args) =>
        {
          actualArgs = args;
        };

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureStringArgsHandler);
        subject.HandleEvent(this, expectedArgs, nameof(Should_Handle_Instance_Handler));

        actualArgs.Should().BeSameAs(expectedArgs);
      }

      [Fact]
      public void Should_Handle_Static_Handler()
      {
        var stringArgs = new DummyStringArgs { Expected = Create<string>() };

        FixtureStringArgsHandler += StaticHandler;

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureStringArgsHandler);
        subject.HandleEvent(this, stringArgs, nameof(Should_Handle_Static_Handler));

        stringArgs.Actual.Should().Be(stringArgs.Expected);
      }




      private class FixtureDisposableClass : IDisposable
      {
        public void DisposableStaticHandler(object sender, DummyStringArgs args)
        {
          args.Actual = args.Expected;
        }

        public void Dispose()
        {
        }
      }

      [Fact]
      public void Should_Remove_Garbage_Collected_Handler()
      {
        // -------------------------------------------------------------
        // this test code shows WeakReference isn't working in the tests
        // -------------------------------------------------------------

        //var strongRef = new StringBuilder();
        //var weakRef = new WeakReference(strongRef);
        //strongRef = null;

        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        //GC.Collect();

        //var target = weakRef.Target;

        //target.Should().BeNull();

        // --------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------

        // -------------------------------------------------
        // if the above ever succeeds then here is the test:
        // -------------------------------------------------

        //var stringArgs = new DummyStringArgs { Expected = Create<string>() };
        //var subject = new WeakEventManager();

        //using (var disposable = new FixtureDisposableClass())
        //{
        //  subject.AddEventHandler<DummyStringArgs>(disposable.DisposableStaticHandler);
        //  subject.HandleEvent(this, stringArgs, nameof(Should_Remove_Garbage_Collected_Handler));

        //  // first prove it is called
        //  stringArgs.Actual.Should().Be(stringArgs.Expected);
        //}

        //GC.Collect();
        //GC.WaitForPendingFinalizers();

        //// the handler should now be disposed
        //stringArgs = new DummyStringArgs { Expected = Create<string>() };
        //subject.HandleEvent(this, stringArgs, nameof(Should_Remove_Garbage_Collected_Handler));

        //stringArgs.Actual.Should().BeNull();
      }

      private static void StaticHandler(object sender, DummyStringArgs args)
      {
        args.Actual = args.Expected;
      }
    }


    public class RemoveEventHandler_EventArgs : WeakEventManagerFixture
    {
      public EventHandler<DummyStringArgs> FixtureStringArgsHandler1;
      public EventHandler<DummyStringArgs> FixtureStringArgsHandler2;

      [Fact]
      public void Should_Throw_When_Handler_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
          {
            subject.RemoveEventHandler((EventHandler<EventArgs>)null);

          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("handler"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
          {
            EventHandler<EventArgs> handler = (sender, args) => { };

            subject.RemoveEventHandler(handler, null);
          })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Empty()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
          {
            EventHandler<EventArgs> handler = (sender, args) => { };

            subject.RemoveEventHandler(handler, string.Empty);
          })
          .Should()
          .Throw<ArgumentException>()
          .WithMessage(GetExpectedArgumentEmptyExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Remove_Handler()
      {
        var stringArgs = new DummyStringArgs { Expected = Create<string>() };

        FixtureStringArgsHandler1 += (sender, args) =>
        {
          args.Actual = args.Expected;
        };

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureStringArgsHandler1);
        subject.HandleEvent(this, stringArgs, nameof(Should_Remove_Handler));

        // assert it has been added
        stringArgs.Actual.Should().Be(stringArgs.Expected);

        stringArgs.Actual = null;
        subject.RemoveEventHandler(FixtureStringArgsHandler1);

        subject.HandleEvent(this, stringArgs, nameof(Should_Remove_Handler));

          // and now confirm it isn't called again
        stringArgs.Actual.Should().BeNull();
      }

      [Fact]
      public void Should_Not_Throw_If_Handler_Not_Registered()
      {
        FixtureStringArgsHandler1 += (sender, args) =>
        {
        };

        var subject = new WeakEventManager();

        Invoking(() =>
          {
            subject.RemoveEventHandler(FixtureStringArgsHandler1);
            subject.RemoveEventHandler(FixtureStringArgsHandler1);
          })
          .Should()
          .NotThrow();
      }

      [Fact]
      public void Should_Remove_Expected_Handler()
      {
        var eventArgs = new DummyStringArgs();
        var handlerNames = new List<string>();

        FixtureStringArgsHandler1 += (sender, args) =>
        {
          handlerNames.Add(nameof(FixtureStringArgsHandler1));
        };

        FixtureStringArgsHandler2 += (sender, args) =>
        {
          handlerNames.Add(nameof(FixtureStringArgsHandler2));
        };

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureStringArgsHandler1);
        subject.AddEventHandler(FixtureStringArgsHandler2);

        subject.HandleEvent(this, eventArgs, nameof(Should_Remove_Expected_Handler));

        // assert they both get invoked
        handlerNames.Should().BeEquivalentTo(nameof(FixtureStringArgsHandler1), nameof(FixtureStringArgsHandler2));

        handlerNames.Clear();
        subject.RemoveEventHandler(FixtureStringArgsHandler1);

        subject.HandleEvent(this, eventArgs, nameof(Should_Remove_Expected_Handler));

        // and now confirm FixtureArgsHandler1 isn't called again
        handlerNames.Should().BeEquivalentTo(nameof(FixtureStringArgsHandler2));
      }
    }

    public class RemoveEventHandler : WeakEventManagerFixture
    {
      public EventHandler FixtureArgsHandler1;
      public EventHandler FixtureArgsHandler2;

      [Fact]
      public void Should_Throw_When_Handler_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
        {
          subject.RemoveEventHandler((EventHandler)null);

        })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("handler"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Null()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
        {
          EventHandler handler = (sender, args) => { };

          subject.RemoveEventHandler(handler, null);
        })
          .Should()
          .Throw<ArgumentNullException>()
          .WithMessage(GetExpectedArgumentNullExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Throw_When_EventName_Empty()
      {
        var subject = new WeakEventManager();

        Invoking(() =>
        {
          EventHandler handler = (sender, args) => { };

          subject.RemoveEventHandler(handler, string.Empty);
        })
          .Should()
          .Throw<ArgumentException>()
          .WithMessage(GetExpectedArgumentEmptyExceptionMessage("eventName"));
      }

      [Fact]
      public void Should_Remove_Handler()
      {
        var expected = new EventArgs();
        EventArgs actual = null;

        FixtureArgsHandler1 += (sender, args) =>
        {
          actual = args;
        };

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureArgsHandler1);
        subject.HandleEvent(this, expected, nameof(Should_Remove_Handler));

        // assert it has been added
        actual.Should().BeSameAs(expected);

        actual = null;
        subject.RemoveEventHandler(FixtureArgsHandler1);

        subject.HandleEvent(this, expected, nameof(Should_Remove_Handler));

        // and now confirm it isn't called again
        actual.Should().BeNull();
      }

      [Fact]
      public void Should_Not_Throw_If_Handler_Not_Registered()
      {
        FixtureArgsHandler1 += (sender, args) =>
        {
        };

        var subject = new WeakEventManager();

        Invoking(() =>
        {
          subject.RemoveEventHandler(FixtureArgsHandler1);
          subject.RemoveEventHandler(FixtureArgsHandler1);
        })
          .Should()
          .NotThrow();
      }

      [Fact]
      public void Should_Remove_Expected_Handler()
      {
        var eventArgs = new EventArgs();
        var handlerNames = new List<string>();

        FixtureArgsHandler1 += (sender, args) =>
        {
          handlerNames.Add(nameof(FixtureArgsHandler1));
        };

        FixtureArgsHandler2 += (sender, args) =>
        {
          handlerNames.Add(nameof(FixtureArgsHandler2));
        };

        var subject = new WeakEventManager();

        subject.AddEventHandler(FixtureArgsHandler1);
        subject.AddEventHandler(FixtureArgsHandler2);

        subject.HandleEvent(this, eventArgs, nameof(Should_Remove_Expected_Handler));

        // assert they both get invoked
        handlerNames.Should().BeEquivalentTo(nameof(FixtureArgsHandler1), nameof(FixtureArgsHandler2));

        handlerNames.Clear();
        subject.RemoveEventHandler(FixtureArgsHandler1);

        subject.HandleEvent(this, eventArgs, nameof(Should_Remove_Expected_Handler));

        // and now confirm FixtureArgsHandler1 isn't called again
        handlerNames.Should().BeEquivalentTo(nameof(FixtureArgsHandler2));
      }
    }
  }
}