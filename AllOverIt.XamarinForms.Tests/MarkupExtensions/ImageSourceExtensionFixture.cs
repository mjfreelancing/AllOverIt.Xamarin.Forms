using AllOverIt.XamarinForms.MarkupExtensions;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.MarkupExtensions
{
  public class ImageSourceExtensionFixture : AllOverItFixtureBase
  {
    [Fact]
    public void Should_Have_ContentProperty_Attribute()
    {
      var actual = typeof(ImageSourceExtension).GetCustomAttribute(typeof(ContentPropertyAttribute));

      actual.Should().NotBeNull();
    }

    public class ProvideValue : ImageSourceExtensionFixture
    {
      [Fact]
      public void Should_Return_Null_When_Source_Null()
      {
        var subject = new ImageSourceExtension();

        var actual = subject.ProvideValue(A.Fake<IServiceProvider>());

        subject.Source.Should().BeNull();
        actual.Should().BeNull();
      }

      [Fact]
      public void Should_Return_Null_When_Source_Empty()
      {
        var subject = new ImageSourceExtension {Source = string.Empty};

        var actual = subject.ProvideValue(A.Fake<IServiceProvider>());

        actual.Should().BeNull();
      }

      [Fact]
      public void Should_Throw_When_ResolvingType_Null()
      {
        var subject = new ImageSourceExtension { Source = Create<string>() };

        Invoking(() =>
          {
            subject.ProvideValue(A.Fake<IServiceProvider>());
          })
          .Should()
          .Throw<ArgumentNullException>();
      }

      [Fact]
      public void Should_Try_Resolving_Image_Source()
      {
        // valid values, but npt pointing to anything valid
        var subject = new ImageSourceExtension { Source = Create<string>(), ResolvingType = GetType()};
        
        var imageSource = subject.ProvideValue(A.Fake<IServiceProvider>()) as StreamImageSource;

        imageSource.Should().NotBeNull();
      }
    }
  }
}