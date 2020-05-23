using AllOverIt.XamarinForms.Logging;
using AllOverIt.XamarinForms.Mvvm;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Mvvm
{
  public class ViewModelBaseFixture : AllOverItFixtureBase
  {
    private class DummyViewModel : ViewModelBase
    {
      private readonly IList<string> _propertyNamesNotified = new List<string>();

      public ILogger ViewModelLogger => Logger;
      public IEnumerable<string> PropertyNamesNotified => _propertyNamesNotified;

      public DummyViewModel()
      {
        PropertyChanged += (sender, args) =>
        {
          _propertyNamesNotified.Add(args.PropertyName);
        };
      }

      public DummyViewModel(ILogger logger)
        : base(logger)
      {
      }

      public DummyViewModel(Lazy<ILogger> logger)
        : base(logger)
      {
      }
    }

    public class Constructor : ViewModelBaseFixture
    {
      [Fact]
      public void Should_Return_Default_IsBusy_False()
      {
        var subject = new DummyViewModel();

        subject.IsBusy.Should().BeFalse();
      }

      [Fact]
      public void Should_Notify_IsBusy_Changed()
      {
        var subject = new DummyViewModel { IsBusy = true };

        subject.PropertyNamesNotified.Should().BeEquivalentTo(nameof(DummyViewModel.IsBusy));
      }

      [Fact]
      public void Should_Return_Default_Title_Empty()
      {
        var subject = new DummyViewModel();

        subject.Title.Should().BeEmpty();
      }

      [Fact]
      public void Should_Notify_Title_Changed()
      {
        var subject = new DummyViewModel { Title = Create<string>() };

        subject.PropertyNamesNotified.Should().BeEquivalentTo(nameof(DummyViewModel.Title));
      }

      [Fact]
      public void Should_Return_Default_Logger_Null()
      {
        var subject = new DummyViewModel();

        subject.ViewModelLogger.Should().BeNull();
      }

      [Fact]
      public void Should_Return_Logger()
      {
        var loggerFake = A.Fake<ILogger>();

        var subject = new DummyViewModel(loggerFake);

        subject.ViewModelLogger.Should().BeSameAs(loggerFake);
      }

      [Fact]
      public void Should_Return_Lazy_Logger()
      {
        var loggerFake = A.Fake<ILogger>();

        var subject = new DummyViewModel(new Lazy<ILogger>(() => loggerFake));

        subject.ViewModelLogger.Should().BeSameAs(loggerFake);
      }
    }

    public class RaisePropertyChanged : ViewModelBaseFixture
    {
      [Fact]
      public void Should_Raise_Property_Change_Notification()
      {
        var subject = new DummyViewModel { Title = Create<string>() };

        subject.PropertyNamesNotified.Should().BeEquivalentTo(nameof(DummyViewModel.Title));
      }

      [Fact]
      public void Should_Not_Raise_Property_Change_Notification()
      {
        var subject = new DummyViewModel();

        using (subject.CreateDeferredPropertyChangeScope())
        {
          subject.Title = Create<string>();
          subject.PropertyNamesNotified.Should().BeEmpty();
        }
      }
    }

    public class CreateDeferredPropertyChangeScope : ViewModelBaseFixture
    {
      [Fact]
      public void Should_Defer_Raise_Property_Change_Notification()
      {
        var subject = new DummyViewModel();

        using (subject.CreateDeferredPropertyChangeScope())
        {
          subject.Title = Create<string>();
          subject.IsBusy = true;

          subject.PropertyNamesNotified.Should().BeEmpty();
        }

        subject.PropertyNamesNotified.Should().BeEquivalentTo(
          nameof(DummyViewModel.Title),
          nameof(DummyViewModel.IsBusy)
        );
      }

      [Fact]
      public void Should_Throw_When_Defer_Notification_Is_Nested()
      {
        Invoking(() =>
          {
            var subject = new DummyViewModel();

            subject.CreateDeferredPropertyChangeScope();
            subject.CreateDeferredPropertyChangeScope();
          })
          .Should()
          .Throw<InvalidOperationException>()
          .WithMessage("Deferred property notification cannot be nested");
      }

      [Fact]
      public void Should_Raise_Property_Change_Notification_Once()
      {
        var subject = new DummyViewModel();

        using (subject.CreateDeferredPropertyChangeScope())
        {
          subject.Title = Create<string>();
          subject.Title = Create<string>();

          subject.PropertyNamesNotified.Should().BeEmpty();
        }

        subject.PropertyNamesNotified.Should().BeEquivalentTo(nameof(DummyViewModel.Title));
      }
    }
  }
}