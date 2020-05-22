using AllOverIt.XamarinForms.Helpers;
using FluentAssertions;
using Xamarin.Forms;
using Xunit;

namespace AllOverIt.XamarinForms.Tests.Helpers
{
  [CollectionDefinition(nameof(MergeDictionaryCollection), DisableParallelization = true)]
  public class MergeDictionaryCollection { }

  public class ResourceDictionaryHelperFixture : AllOverItFixtureBase
  {
    public ResourceDictionaryHelperFixture()
    {
      InitFormsFixture();
    }

    [Collection(nameof(MergeDictionaryCollection))]
    public class MergeIntoApplicationResources_ResourceDictionary : ResourceDictionaryHelperFixture
    {
      [Fact]
      public void Should_Merge_Into_Application_Resources()
      {
        var resources = new ResourceDictionary { { Create<string>(), Create<int>() } };

        var currentResources = Application.Current.Resources;
        currentResources.Should().BeEmpty();

        ResourceDictionaryHelper.MergeIntoApplicationResources(resources);

        currentResources.Should().BeEquivalentTo(resources);
      }

      [Fact]
      public void Should_Merge_Merged_Dictionaries_Into_Application_Resources()
      {
        var resources = new ResourceDictionary();

        resources.MergedDictionaries.Add(new ResourceDictionary { { Create<string>(), Create<int>() } });
        resources.MergedDictionaries.Add(new ResourceDictionary { { Create<string>(), Create<int>() } });

        var currentResources = Application.Current.Resources;
        currentResources.MergedDictionaries.Should().BeEmpty();

        ResourceDictionaryHelper.MergeIntoApplicationResources(resources);

        currentResources.MergedDictionaries.Should().BeEquivalentTo(resources.MergedDictionaries);

      }
    }
  }
}