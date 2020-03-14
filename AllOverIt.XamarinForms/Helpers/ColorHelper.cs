using System;
using Xamarin.Forms;

namespace AllOverIt.XamarinForms.Helpers
{
  public static class ColorHelper
  {
    public static Color GetHighestContrastColor(Color backgroundColor, Color color1, Color color2)
    {
      var color1Contrast = ColorHelper.GetContrastRatio(color1, backgroundColor);
      var color2Contrast = ColorHelper.GetContrastRatio(color2, backgroundColor);

     return color1Contrast > color2Contrast
        ? color1
        : color2;
    }

    // if the foreground color contains a non-zero alpha that will be applied against the background color
    // before determining the contrast ratio.
    public static double GetContrastRatio(Color foregroundColor, Color backgroundColor)
    {
      // https://medium.muz.li/the-science-of-color-contrast-an-expert-designers-guide-33e84c41d156

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
      return Math.Round((luminance1 + 0.05) / (luminance2 + 0.05), 2);
    }

    public static double GetRelativeLuminance(Color color)
    {
      // https://www.w3.org/TR/WCAG20/#relativeluminancedef

      double GetComponent(double value)
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

    // combines two opaque colors, with the overlay having a specified opacity (0-1)
    public static Color CombineColors(string baseColorHex, string overlayColorHex, double overlayOpacity)
    {
      var baseColor = Color.FromHex(baseColorHex);
      var overlayColor = Color.FromHex(overlayColorHex);

      var combinedRed = (int)Math.Ceiling((1 - overlayOpacity) * GetRed(baseColor) + overlayOpacity * GetRed(overlayColor));
      var combinedGreen = (int)Math.Ceiling((1 - overlayOpacity) * GetGreen(baseColor) + overlayOpacity * GetGreen(overlayColor));
      var combinedBlue = (int)Math.Ceiling((1 - overlayOpacity) * GetBlue(baseColor) + overlayOpacity * GetBlue(overlayColor));

      return Color.FromRgb(combinedRed, combinedGreen, combinedBlue);
    }

    public static Color CombineColors(Color baseColor, Color overlayColor, double overlayOpacity)
    {
      var combinedRed = (int)Math.Ceiling((1 - overlayOpacity) * GetRed(baseColor) + overlayOpacity * GetRed(overlayColor));
      var combinedGreen = (int)Math.Ceiling((1 - overlayOpacity) * GetGreen(baseColor) + overlayOpacity * GetGreen(overlayColor));
      var combinedBlue = (int)Math.Ceiling((1 - overlayOpacity) * GetBlue(baseColor) + overlayOpacity * GetBlue(overlayColor));

      return Color.FromRgb(combinedRed, combinedGreen, combinedBlue);
    }

    // opacity = 0-1
    public static Color FromHexWithOpacity(string colorHex, double opacity)
    {
      var color = Color.FromHex(colorHex);
      return FromColorWithOpacity(color, opacity);
    }

    public static Color FromColorWithOpacity(Color color, double opacity)
    {
      return Color.FromRgba(color.R, color.G, color.B, opacity);
    }

    public static int GetRed(Color color)
    {
      return (int)(color.R * 255);
    }

    public static int GetGreen(Color color)
    {
      return (int)(color.G * 255);
    }

    public static int GetBlue(Color color)
    {
      return (int)(color.B * 255);
    }
  }
}