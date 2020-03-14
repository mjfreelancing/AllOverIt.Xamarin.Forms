using System.Collections.Generic;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Helpers
{
  public static class ResourceDictionaryHelper
  {
    /// <summary>
    /// Merges a provided <see cref="ResourceDictionary"/> into the current Application resources.
    /// </summary>
    /// <param name="resources">The resource dictionary to be merged.</param>
    public static void MergeIntoApplicationResources(ResourceDictionary resources)
    {
      MergeIntoApplicationResources((IDictionary<string, object>) resources);

      var currentResources = Application.Current.Resources;

      foreach (var themeMergedDictionary in resources.MergedDictionaries)
      {
        currentResources.MergedDictionaries.Add(themeMergedDictionary);
      }
    }

    /// <summary>
    /// Merges a provided (string, object) dictionary into the current Application resources.
    /// </summary>
    /// <param name="resources">The dictionary of resources to be merged.</param>
    private static void MergeIntoApplicationResources(IDictionary<string, object> resources)
    {
      var currentResources = Application.Current.Resources;

      foreach (var resource in resources)
      {
        currentResources[resource.Key] = resource.Value;
      }
    }
  }
}