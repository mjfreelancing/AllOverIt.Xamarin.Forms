using System;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Helpers
{
  public static class ColorHelper
  {
    /// <summary>
    /// Determines which of two colors provides the greatest contrast ratio in relation to a background color.
    /// </summary>
    /// <param name="backgroundColor">The background color.</param>
    /// <param name="color1">The first color. If the color contains a non-zero alpha that will be applied before determining
    /// the contrast ratio.</param>
    /// <param name="color2">The second color. If the color contains a non-zero alpha that will be applied before determining
    /// the contrast ratio.</param>
    /// <returns>The color that provides the greatest contrast ratio in relation to the background color.</returns>
    public static Color GetHighestContrastColor(Color backgroundColor, Color color1, Color color2)
    {
      var color1Contrast = GetContrastRatio(color1, backgroundColor);
      var color2Contrast = GetContrastRatio(color2, backgroundColor);

     return color1Contrast > color2Contrast
        ? color1
        : color2;
    }

    /// <summary>
    /// Calculates the contrast ratio between a foreground and background color.
    /// </summary>
    /// <param name="foregroundColor">The foreground color. If the foreground color contains a non-zero alpha that will be
    /// applied before determining the contrast ratio.</param>
    /// <param name="backgroundColor">The background color.  The alpha channel is ignored.</param>
    /// <returns>The contrast ratio between a foreground and background color.</returns>
    public static double GetContrastRatio(Color foregroundColor, Color backgroundColor)
    {
      // https://medium.muz.li/the-science-of-color-contrast-an-expert-designers-guide-33e84c41d156

      // Also, the basic formula for the color when c0 overlays opaque c1 with alpha a0 is
      // r0 1 = (1 - a0)·r1 + a0·r0   (same for green and blue channels)

      var combinedColor = CombineColors(backgroundColor, foregroundColor, foregroundColor.A);

      var lighter = combinedColor.Luminosity > backgroundColor.Luminosity
        ? combinedColor
        : backgroundColor;

      var darker = lighter == combinedColor
        ? backgroundColor
        : combinedColor;

      var luminance1 = GetRelativeLuminance(lighter);
      var luminance2 = GetRelativeLuminance(darker);

      // luminance1 must be lighter than luminance2
      return (luminance1 + 0.05) / (luminance2 + 0.05);
    }

    /// <summary>
    /// Gets the relative brightness of any point in the sRGB color space, normalized to 0 for darkest black and 1 for lightest white.
    /// </summary>
    /// <param name="color">The color to calculate the relative luminance (brightness). The alpha channel is ignored.</param>
    /// <returns>The relative luminance of the provided color in the sRGB color space.</returns>
    public static double GetRelativeLuminance(Color color)
    {
      // https://www.w3.org/TR/WCAG20/#relativeluminancedef

      static double GetComponent(double value)
      {
        return value <= 0.03928
          ? value / 12.92
          : Math.Pow((value + 0.055) / 1.055, 2.4);
      }

      var red = GetComponent(color.R);
      var green = GetComponent(color.G);
      var blue = GetComponent(color.B);

      return 0.2126 * red + 0.7152 * green + 0.0722 * blue;
    }

    /// <summary>
    /// Combines two opaque colors, with the overlay having a specified opacity (0-1).
    /// </summary>
    /// <param name="baseColorHex">The base color represented as an RGB hex code. The alpha channel is ignored.</param>
    /// <param name="overlayColorHex">The overlay color represented as an RGB hex code. The alpha channel is ignored.</param>
    /// <param name="overlayOpacity">The opacity of the overlay color to be applied. Must have a value between 0 and 1.</param>
    /// <returns>A <see cref="Color"/> that represents the combined base and overlay colors. The resulting <see cref="Color"/> has an alpha value of 1.</returns>
    public static Color CombineColors(string baseColorHex, string overlayColorHex, double overlayOpacity)
    {
      if (overlayOpacity < 0.0d || overlayOpacity > 1.0d)
      {
        throw new ArgumentOutOfRangeException(nameof(overlayOpacity), "The opacity must have a value between 0 and 1");
      }

      var baseColor = Color.FromHex(baseColorHex);
      var overlayColor = Color.FromHex(overlayColorHex);

      return CombineColors(baseColor, overlayColor, overlayOpacity);
    }

    /// <summary>
    /// Combines two opaque colors, with the overlay having a specified opacity (0-1).
    /// </summary>
    /// <param name="baseColor">The base color. The alpha channel is ignored.</param>
    /// <param name="overlayColor">The overlay color. The alpha channel is ignored.</param>
    /// <param name="overlayOpacity">The opacity of the overlay color to be applied. Must have a value between 0 and 1.</param>
    /// <returns>A <see cref="Color"/> that represents the combined base and overlay colors. The resulting <see cref="Color"/> has an alpha value of 1.</returns>
    public static Color CombineColors(Color baseColor, Color overlayColor, double overlayOpacity)
    {
      if (overlayOpacity < 0.0d || overlayOpacity > 1.0d)
      {
        throw new ArgumentOutOfRangeException(nameof(overlayOpacity), "The opacity must have a value between 0 and 1");
      }

      var combinedRed = (int)Math.Ceiling((1 - overlayOpacity) * GetRed(baseColor) + overlayOpacity * GetRed(overlayColor));
      var combinedGreen = (int)Math.Ceiling((1 - overlayOpacity) * GetGreen(baseColor) + overlayOpacity * GetGreen(overlayColor));
      var combinedBlue = (int)Math.Ceiling((1 - overlayOpacity) * GetBlue(baseColor) + overlayOpacity * GetBlue(overlayColor));

      return Color.FromRgb(combinedRed, combinedGreen, combinedBlue);
    }

    /// <summary>
    /// Creates a new <see cref="Color"/> from a color represented as an RGB string and a specified opacity (alpha).
    /// </summary>
    /// <param name="colorHex">The color represented as an RGB string. If the string contains an alpha value it is ignored.</param>
    /// <param name="opacity">The opacity to be applied. Must have a value between 0 and 1.</param>
    /// <returns>A new <see cref="Color"/> with a specified opacity (alpha).</returns>
    public static Color FromHexWithOpacity(string colorHex, double opacity)
    {
      var color = Color.FromHex(colorHex);
      return FromColorWithOpacity(color, opacity);
    }

    /// <summary>
    /// Creates a new <see cref="Color"/> from a color and a specified opacity (alpha).
    /// </summary>
    /// <param name="color">The source color. If the color contains an alpha value it is ignored.</param>
    /// <param name="opacity">The opacity to be applied. Must have a value between 0 and 1.</param>
    /// <returns>A new <see cref="Color"/> with a specified opacity (alpha).</returns>
    public static Color FromColorWithOpacity(Color color, double opacity)
    {
      return Color.FromRgba(color.R, color.G, color.B, opacity);
    }

    /// <summary>
    /// Gets the red channel from a <see cref="Color"/> as value between 0 and 255.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns>The red channel as a value between 0 and 255.</returns>
    public static int GetRed(Color color)
    {
      return (int)(color.R * 255);
    }

    /// <summary>
    /// Gets the green channel from a <see cref="Color"/> as value between 0 and 255.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns>The green channel as a value between 0 and 255.</returns>
    public static int GetGreen(Color color)
    {
      return (int)(color.G * 255);
    }

    /// <summary>
    /// Gets the blue channel from a <see cref="Color"/> as value between 0 and 255.
    /// </summary>
    /// <param name="color">The source color.</param>
    /// <returns>The blue channel as a value between 0 and 255.</returns>
    public static int GetBlue(Color color)
    {
      return (int)(color.B * 255);
    }
  }
}