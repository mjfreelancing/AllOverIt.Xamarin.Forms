using AllOverIt.XamarinForms.Behaviors;
using AllOverIt.XamarinForms.Behaviors.Base;
using AllOverIt.XamarinForms.Tests.Controls;
using FluentAssertions;
using System.Linq;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Behaviors
{
  // need to create controls non-parallel to prevent init issues
  [Collection(nameof(ControlCollection))]
  public class NumericValidationBehaviorFixture : AllOverItFixtureBase
  {
    public NumericValidationBehaviorFixture()
    {
      InitFormsFixture();
    }

    [Fact]
    public void Should_Attach_To_Entry()
    {
      var entry = new Entry();

      var behavior = new NumericValidationBehavior();

      entry.Behaviors.Add(behavior);

      behavior.AssociatedObject.Should().Be(entry);
    }

    [Fact]
    public void Should_Detach_From_Entry()
    {
      var entry = new Entry();

      var behavior = new NumericValidationBehavior();

      entry.Behaviors.Add(behavior);

      behavior.AssociatedObject.Should().Be(entry);

      entry.Behaviors.Clear();

      behavior.AssociatedObject.Should().BeNull();
    }

    [Fact]
    public void Should_Get_Default_ValidTextColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var actual = behavior.ValidTextColor;
      
      actual.Should().Be(Color.Default);
    }

    [Fact]
    public void Should_Set_ValidTextColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var expected = Create<Color>();

      behavior.ValidTextColor = expected;

      var actual = behavior.ValidTextColor;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_ValidBackgroundColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var actual = behavior.ValidBackgroundColor;

      actual.Should().Be(Color.Default);
    }

    [Fact]
    public void Should_Set_ValidBackgroundColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var expected = Create<Color>();

      behavior.ValidBackgroundColor = expected;

      var actual = behavior.ValidBackgroundColor;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_InvalidTextColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var actual = behavior.InvalidTextColor;

      actual.Should().Be(Color.Red);
    }

    [Fact]
    public void Should_Set_InvalidTextColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var expected = Create<Color>();

      behavior.InvalidTextColor = expected;

      var actual = behavior.InvalidTextColor;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_InvalidBackgroundColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var actual = behavior.InvalidBackgroundColor;

      actual.Should().Be(Color.Default);
    }

    [Fact]
    public void Should_Set_InvalidBackgroundColor()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var expected = Create<Color>();

      behavior.InvalidBackgroundColor = expected;

      var actual = behavior.InvalidBackgroundColor;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_AllowDecimal()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var actual = behavior.AllowDecimal;

      actual.Should().Be(true);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Set_AllowDecimal(bool expected)
    {
      var behavior = GetBehaviorAttachedViaStyle();

      behavior.AllowDecimal = expected;

      var actual = behavior.AllowDecimal;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_AllowNegative()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var actual = behavior.AllowNegative;

      actual.Should().Be(true);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Set_AllowNegative(bool expected)
    {
      var behavior = GetBehaviorAttachedViaStyle();

      behavior.AllowNegative = expected;

      var actual = behavior.AllowNegative;

      actual.Should().Be(expected);
    }

    [Fact]
    public void Should_Get_Default_SanitizeInput()
    {
      var behavior = GetBehaviorAttachedViaStyle();

      var actual = behavior.SanitizeInput;

      actual.Should().Be(false);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Set_SanitizeInput(bool expected)
    {
      var behavior = GetBehaviorAttachedViaStyle();

      behavior.SanitizeInput = expected;

      var actual = behavior.SanitizeInput;

      actual.Should().Be(expected);
    }

    public class OnEntryTextChanged : NumericValidationBehaviorFixture
    {
      [Fact]
      public void Should_Not_Validate_When_Not_Focused()
      {
        var entry = new Entry();
        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        entry.Text = Create<string>();

        var actual = entry.TextColor;

        actual.Should().NotBe(behavior.InvalidTextColor);
      }

      [Fact]
      public void Should_Not_Validate_When_Focused_And_Empty_Text()
      {
        var entry = new Entry();

        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        entry.Text = Create<string>();

        entry.Focus();

        entry.IsFocused.Should().BeTrue();

        entry.Text = string.Empty;

        var actual = entry.TextColor;

        actual.Should().NotBe(behavior.InvalidTextColor);
      }

      [Fact]
      public void Should_Allow_Decimal()
      {
        var entry = new Entry();
        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        behavior.AllowDecimal = true;
        behavior.InvalidTextColor = CreateExcluding(entry.TextColor);
        behavior.InvalidBackgroundColor = CreateExcluding(entry.BackgroundColor);

        entry.Focus();

        entry.IsFocused.Should().BeTrue();

        entry.Text = "1.23";

        entry.TextColor.Should().NotBe(behavior.InvalidTextColor);
        entry.BackgroundColor.Should().NotBe(behavior.InvalidBackgroundColor);
      }

      [Fact]
      public void Should_Not_Allow_Decimal()
      {
        var entry = new Entry();
        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        behavior.AllowDecimal = false;
        behavior.InvalidTextColor = CreateExcluding(entry.TextColor);
        behavior.InvalidBackgroundColor = CreateExcluding(entry.BackgroundColor);

        entry.Focus();

        entry.IsFocused.Should().BeTrue();

        entry.Text = "1.23";

        entry.TextColor.Should().Be(behavior.InvalidTextColor);
        entry.BackgroundColor.Should().Be(behavior.InvalidBackgroundColor);
      }

      [Fact]
      public void Should_Allow_Negative()
      {
        var entry = new Entry();
        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        behavior.AllowNegative = true;
        behavior.InvalidTextColor = CreateExcluding(entry.TextColor);
        behavior.InvalidBackgroundColor = CreateExcluding(entry.BackgroundColor);

        entry.Focus();

        entry.IsFocused.Should().BeTrue();

        entry.Text = "-1.23";

        entry.TextColor.Should().NotBe(behavior.InvalidTextColor);
        entry.BackgroundColor.Should().NotBe(behavior.InvalidBackgroundColor);
      }

      [Fact]
      public void Should_Not_Allow_Negative()
      {
        var entry = new Entry();
        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        behavior.AllowNegative = false;
        behavior.InvalidTextColor = CreateExcluding(entry.TextColor);
        behavior.InvalidBackgroundColor = CreateExcluding(entry.BackgroundColor);

        entry.Focus();

        entry.IsFocused.Should().BeTrue();

        entry.Text = "-1.23";

        entry.TextColor.Should().Be(behavior.InvalidTextColor);
        entry.BackgroundColor.Should().Be(behavior.InvalidBackgroundColor);
      }

      [Fact]
      public void Should_Sanitize_Input()
      {
        var entry = new Entry();
        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        behavior.SanitizeInput = true;
        behavior.InvalidTextColor = CreateExcluding(entry.TextColor);
        behavior.InvalidBackgroundColor = CreateExcluding(entry.BackgroundColor);

        entry.Text = "1.23";

        entry.Focus();

        entry.IsFocused.Should().BeTrue();

        entry.Text = Create<string>();

        entry.Text.Should().Be("1.23");
        entry.TextColor.Should().NotBe(behavior.InvalidTextColor);
        entry.BackgroundColor.Should().NotBe(behavior.InvalidBackgroundColor);
      }

      [Fact]
      public void Should_Not_Sanitize_Input()
      {
        var entry = new Entry();
        var behavior = GetBehaviorAttachedViaStyle(entry);

        entry.IsFocused.Should().BeFalse();

        behavior.SanitizeInput = false;
        behavior.InvalidTextColor = CreateExcluding(entry.TextColor);
        behavior.InvalidBackgroundColor = CreateExcluding(entry.BackgroundColor);

        entry.Text = "1.23";

        entry.Focus();

        entry.IsFocused.Should().BeTrue();

        var newValue = Create<string>();
        entry.Text = newValue;

        entry.Text.Should().Be(newValue);
        entry.TextColor.Should().Be(behavior.InvalidTextColor);
        entry.BackgroundColor.Should().Be(behavior.InvalidBackgroundColor);
      }
    }

    private static NumericValidationBehavior GetBehaviorAttachedViaStyle(Entry entry = null)
    {
      var style = new Style(typeof(Entry));

      var styleSetter = new Setter
      {
        Property = AttachableBehavior<NumericValidationBehavior, Entry>.AttachBehaviorProperty,
        Value = true
      };

      style.Setters.Add(styleSetter);

      entry ??= new Entry();

      EntryUtilities.ConfigureEntryFocus(entry);

      entry.Style = style;

      return entry.Behaviors.Single() as NumericValidationBehavior;
    }
  }
}