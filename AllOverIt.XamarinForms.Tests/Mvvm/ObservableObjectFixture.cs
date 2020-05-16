using AllOverIt.XamarinForms.Mvvm;
using FluentAssertions;
using System;
using System.Linq.Expressions;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Mvvm
{
  public class ObservableObjectFixture : AllOverItFixtureBase
  {
    private class DummyObservable : ObservableObject
    {
      public string _value1;    // public for the test
      private string _value2;

      public bool ActionCalled { get; private set; }

      public string Value1
      {
        get => _value1;
        set => SetProperty(ref _value1, value);
      }

      public string Value2
      {
        get => _value2;
        set => SetProperty(ref _value2, value, () => { ActionCalled = true; });
      }

      public void RaiseChangeEvent(string propertyName)
      {
        RaisePropertyChanged(propertyName);
      }

      public void RaiseChangeEvent(Expression<Func<string>> expression)
      {
        RaisePropertyChanged(expression);
      }
    }

    public class RaisePropertyChangedFixture : ObservableObjectFixture
    {
      [Fact]
      public void Should_Raise_Property_Change_Notification()
      {
        var subject = new DummyObservable();
        string actual = null;

        subject.PropertyChanged += (sender, args) =>
        {
          actual = args.PropertyName;
        };

        var expected = Create<string>();
        subject.RaiseChangeEvent(expected);

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Not_Throw_When_No_Property_Change_Event()
      {
        Invoking(() =>
          {
            var subject = new DummyObservable();
            subject.RaiseChangeEvent(Create<string>());
          })
          .Should()
          .NotThrow();
      }
    }

    public class RaisePropertyChanged_Expression_Fixture : ObservableObjectFixture
    {
      [Fact]
      public void Should_Throw_If_Not_MemberExpression()
      {
        Invoking(() =>
          {
            var subject = new DummyObservable();

            subject.RaiseChangeEvent(() => "");
          })
          .Should()
          .Throw<ArgumentException>()
          .WithMessage("expression is expected to be a LambdaExpression containing a MemberExpression");
      }

      [Fact]
      public void Should_Raise_Property_Change_Notification()
      {
        var subject = new DummyObservable();
        string actual = null;

        subject.PropertyChanged += (sender, args) =>
        {
          actual = args.PropertyName;
        };

        subject.RaiseChangeEvent(() => subject.Value1);

        actual.Should().Be(nameof(DummyObservable.Value1));
      }

      [Fact]
      public void Should_Not_Throw_When_No_Property_Change_Event()
      {
        Invoking(() =>
          {
            var subject = new DummyObservable();

            subject.RaiseChangeEvent(() => subject.Value1);
          })
          .Should()
          .NotThrow();
      }
    }

    public class SetPropertyFixture : ObservableObjectFixture
    {
      [Fact]
      public void Should_Set_Value()
      {
        var expected = Create<string>();
        var subject = new DummyObservable { Value1 = expected };

        var actual = subject.Value1;

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Set_Backing_Store()
      {
        var expected = Create<string>();
        var subject = new DummyObservable { Value1 = expected };

        var actual = subject._value1;

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Raise_Property_Change_Notification()
      {
        var subject = new DummyObservable();
        var expected = nameof(DummyObservable.Value1);
        string actual = null;

        subject.PropertyChanged += (sender, args) =>
        {
          actual = args.PropertyName;
        };

        subject.Value1 = Create<string>();

        actual.Should().Be(expected);
      }

      [Fact]
      public void Should_Not_Raise_Property_Change_Notification()
      {
        var value = Create<string>();
        var subject = new DummyObservable { Value1 = value };
        string actual = null;

        subject.PropertyChanged += (sender, args) =>
        {
          actual = args.PropertyName;
        };

        subject.Value1 = value;

        actual.Should().BeNull();
      }

      [Fact]
      public void Should_Not_Raise_Property_Change_Action()
      {
        var subject = new DummyObservable();

        subject.PropertyChanged += (sender, args) => { };

        subject.Value1 = Create<string>();

        subject.ActionCalled.Should().BeFalse();
      }

      [Fact]
      public void Should_Raise_Property_Change_Action()
      {
        var subject = new DummyObservable();

        subject.PropertyChanged += (sender, args) => { };

        subject.ActionCalled.Should().BeFalse();

        subject.Value2 = Create<string>();

        subject.ActionCalled.Should().BeTrue();
      }
    }
  }
}