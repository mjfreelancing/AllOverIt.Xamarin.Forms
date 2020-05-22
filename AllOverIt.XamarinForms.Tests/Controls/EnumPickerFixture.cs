using AllOverIt.XamarinForms.Controls;
using AllOverIt.XamarinForms.Converters;
using FluentAssertions;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Controls
{
  // need to create controls non-parallel to prevent init issues
  [Collection(nameof(ControlCollection))]
  public class EnumPickerFixture : AllOverItFixtureBase
  {
    private enum DummyEnum
    {
      FirstValue,
      SecondValue,
      ThirdValue
    }

    [Fact]
    public void Should_Set_Enum_Type()
    {
      var expected = typeof(FontAttributes);    // just choosing a random enum type

      var subject = new EnumPicker { EnumType = expected };

      subject.EnumType.Should().Be(expected);
    }

    [Fact]
    public void Should_Set_Display_Converter()
    {
      var expected = new EnumToStringConverter<FontAttributes>();    // just choosing a random converter

      var subject = new EnumPicker { DisplayItemConverter = expected };

      subject.DisplayItemConverter.Should().BeSameAs(expected);
    }

    [Fact]
    public void Should_Set_ItemSource_When_No_Converter()
    {
      var enumType = typeof(FontAttributes);
      var subject = new EnumPicker { EnumType = enumType };

      var expected = Enum.GetNames(enumType).ToList();

      subject.ItemsSource.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Should_Set_ItemSource_When_Setting_EnumType_Before_Converter()
    {
      var enumType = typeof(DummyEnum);

      var converter = new EnumToStringConverter<DummyEnum>
      {
        SplitWords = true
      };

      var subject = new EnumPicker { EnumType = enumType };

      // deliberately setting this after the enum type
      subject.DisplayItemConverter = converter;

      var expected = Enum.GetNames(enumType)
        .Select(item => (string) converter.Convert(item, typeof(string), null, CultureInfo.InvariantCulture))
        .ToList();

      subject.ItemsSource.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Should_Set_ItemSource_When_Setting_EnumType_After_Converter()
    {
      var enumType = typeof(DummyEnum);

      var converter = new EnumToStringConverter<DummyEnum>
      {
        SplitWords = true
      };

      var subject = new EnumPicker { DisplayItemConverter = converter };

      // deliberately setting this after the converter
      subject.EnumType = enumType;

      var expected = Enum.GetNames(enumType)
        .Select(item => (string)converter.Convert(item, typeof(string), null, CultureInfo.InvariantCulture))
        .ToList();

      subject.ItemsSource.Should().BeEquivalentTo(expected);
    }





    [Fact]
    public void Should_Set_Set_ItemSource_Null()
    {
      var enumType = typeof(DummyEnum);
      var subject = new EnumPicker { EnumType = enumType };

      subject.ItemsSource.Should().NotBeEmpty();

      subject.EnumType = null;

      subject.ItemsSource.Should().BeNull();
    }

    [Fact]
    public void Should_Throw_When_Not_Enum_Type()
    {
      Invoking(() =>
        {
          new EnumPicker {EnumType = typeof(string)};
        })
        .Should()
        .Throw<ArgumentOutOfRangeException>()
        .WithMessage("The EnumType property must be an Enum type (Parameter 'EnumType')");
    }
  }
}