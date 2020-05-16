using AllOverIt.XamarinForms.Behaviors;
using FluentAssertions;
using System;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Behaviors.Base
{
  public class AttachableBehaviorFixture : AllOverItFixtureBase
  {
    public AttachableBehaviorFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void Should_Attach_Behavior()
    {
      var style = new Style(typeof(Entry));

      var setter = new Setter
      {
        Property = NumericValidationBehavior.AttachBehaviorProperty,
        Value = true
      };

      style.Setters.Add(setter);

      var entry = new Entry
      {
        Style = style
      };

      entry.Behaviors.Should().HaveCount(1);
      entry.Behaviors[0].Should().BeOfType<NumericValidationBehavior>();
    }

    [Fact]
    public void Should_Not_Attach_Behavior_To_Incorrect_Target()
    {
      var style = new Style(typeof(Entry));

      var setter = new Setter
      {
        Property = NumericValidationBehavior.AttachBehaviorProperty,
        Value = true
      };

      style.Setters.Add(setter);

      Invoking(() =>
        {
          var _ = new ListView(ListViewCachingStrategy.RecycleElement)
          {
            Style = style
          };
        })
        .Should()
        .Throw<ArgumentException>()
        .WithMessage($"Cannot attach behavior to target type {typeof(ListView)}, expected {typeof(Entry)}");
    }

    [Fact]
    public void Should_Not_Attach_Behavior()
    {
      var style = new Style(typeof(Entry));

      var setter = new Setter
      {
        Property = NumericValidationBehavior.AttachBehaviorProperty,
        Value = false
      };

      style.Setters.Add(setter);

      var entry = new Entry
      {
        Style = style
      };

      entry.Behaviors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Remove_Attached_Behavior_On_Style_Change()
    {
      var style1 = new Style(typeof(Entry));

      var setter = new Setter
      {
        Property = NumericValidationBehavior.AttachBehaviorProperty,
        Value = true
      };

      style1.Setters.Add(setter);

      var entry = new Entry
      {
        Style = style1
      };

      entry.Behaviors[0].Should().BeOfType<NumericValidationBehavior>();

      var style2 = new Style(typeof(Entry));
      setter.Value = false;

      style2.Setters.Add(setter);

      entry.Style = style2;
    }

    [Fact]
    public void Should_Replace_Attached_Behavior_On_Style_Change()
    {
      var style1 = new Style(typeof(Entry));

      var setter = new Setter
      {
        Property = NumericValidationBehavior.AttachBehaviorProperty,
        Value = true
      };

      style1.Setters.Add(setter);

      var entry = new Entry
      {
        Style = style1
      };

      var behavior1 = entry.Behaviors[0];

      var style2 = new Style(typeof(Entry));
      style2.Setters.Add(setter);

      entry.Style = style2;

      var behavior2 = entry.Behaviors[0];

      behavior1.Should().NotBe(behavior2);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Get_Attached_Behavior_status(bool expected)
    {
      var style = new Style(typeof(Entry));

      var setter = new Setter
      {
        Property = NumericValidationBehavior.AttachBehaviorProperty,
        Value = expected
      };

      style.Setters.Add(setter);

      var entry = new Entry
      {
        Style = style
      };

      var actual = NumericValidationBehavior.GetAttachBehavior(entry);

      actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Set_Attached_Behavior_Status(bool expected)
    {
      var entry = new Entry();

      NumericValidationBehavior.SetAttachBehavior(entry, expected);

      var actual = NumericValidationBehavior.GetAttachBehavior(entry);

      actual.Should().Be(expected);

      if (expected)
      {
        entry.Behaviors[0].Should().BeOfType<NumericValidationBehavior>();
      }
      else
      {
        entry.Behaviors.Should().BeEmpty();
      }
      
    }
  }
}