// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Reflection;
using Windows.UI;
using Color = Windows.UI.Color;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for colors.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Creates a <see cref="Color"/> from a XAML color string.
        /// Any format used in XAML should work.
        /// </summary>
        /// <param name="colorString">The XAML color string.</param>
        /// <returns>The created <see cref="Color"/>.</returns>
        public static Color ToColor(this string colorString)
        {
            if (string.IsNullOrEmpty(colorString))
            {
                ThrowArgumentException();
            }

            if (colorString[0] == '#')
            {
                switch (colorString.Length)
                {
                    case 9:
                        {
                            uint cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                            byte a = (byte)(cuint >> 24);
                            byte r = (byte)((cuint >> 16) & 0xff);
                            byte g = (byte)((cuint >> 8) & 0xff);
                            byte b = (byte)(cuint & 0xff);

                            return Color.FromArgb(a, r, g, b);
                        }

                    case 7:
                        {
                            uint cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                            byte r = (byte)((cuint >> 16) & 0xff);
                            byte g = (byte)((cuint >> 8) & 0xff);
                            byte b = (byte)(cuint & 0xff);

                            return Color.FromArgb(255, r, g, b);
                        }

                    case 5:
                        {
                            ushort cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                            byte a = (byte)(cuint >> 12);
                            byte r = (byte)((cuint >> 8) & 0xf);
                            byte g = (byte)((cuint >> 4) & 0xf);
                            byte b = (byte)(cuint & 0xf);
                            a = (byte)((a << 4) | a);
                            r = (byte)((r << 4) | r);
                            g = (byte)((g << 4) | g);
                            b = (byte)((b << 4) | b);

                            return Color.FromArgb(a, r, g, b);
                        }

                    case 4:
                        {
                            ushort cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                            byte r = (byte)((cuint >> 8) & 0xf);
                            byte g = (byte)((cuint >> 4) & 0xf);
                            byte b = (byte)(cuint & 0xf);
                            r = (byte)((r << 4) | r);
                            g = (byte)((g << 4) | g);
                            b = (byte)((b << 4) | b);

                            return Color.FromArgb(255, r, g, b);
                        }

                    default: return ThrowFormatException();
                }
            }

            if (colorString.Length > 3 && colorString[0] == 's' && colorString[1] == 'c' && colorString[2] == '#')
            {
                string[] values = colorString.Split(',');

                if (values.Length == 4)
                {
                    double scA = double.Parse(values[0].Substring(3), CultureInfo.InvariantCulture);
                    double scR = double.Parse(values[1], CultureInfo.InvariantCulture);
                    double scG = double.Parse(values[2], CultureInfo.InvariantCulture);
                    double scB = double.Parse(values[3], CultureInfo.InvariantCulture);

                    return Color.FromArgb((byte)(scA * 255), (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
                }

                if (values.Length == 3)
                {
                    double scR = double.Parse(values[0].Substring(3), CultureInfo.InvariantCulture);
                    double scG = double.Parse(values[1], CultureInfo.InvariantCulture);
                    double scB = double.Parse(values[2], CultureInfo.InvariantCulture);

                    return Color.FromArgb(255, (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
                }

                return ThrowFormatException();
            }

            PropertyInfo prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(colorString);

            return prop != null ? (Color)prop.GetValue(null) : ThrowFormatException();
            void ThrowArgumentException() => throw new ArgumentException("The parameter \"colorString\" must not be null or empty.");
            Color ThrowFormatException() => throw new FormatException("The parameter \"colorString\" is not a recognized Color format.");
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to a hexadecimal string representation.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The hexadecimal string representation of the color.</returns>
        public static string ToHex(this Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to a premultiplied Int32 - 4 byte ARGB structure.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The int representation of the color.</returns>
        public static int ToInt(this Color color)
        {
            int a = color.A + 1;
            int col = (color.A << 24) | ((byte)((color.R * a) >> 8) << 16) | ((byte)((color.G * a) >> 8) << 8) | (byte)((color.B * a) >> 8);
            return col;
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from the specified hue, saturation, lightness, and alpha values.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="lightness">0..1 range lightness</param>
        /// <param name="alpha">0..1 alpha</param>
        /// <returns>The created <see cref="Color"/>.</returns>
        public static Color FromHsl(double hue, double saturation, double lightness, double alpha = 1.0)
        {
            if (hue < 0 || hue > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue));
            }

            double chroma = (1 - Math.Abs((2 * lightness) - 1)) * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
            double m = lightness - (0.5 * chroma);
            double r1, g1, b1;

            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            byte r = (byte)(255 * (r1 + m));
            byte g = (byte)(255 * (g1 + m));
            byte b = (byte)(255 * (b1 + m));
            byte a = (byte)(255 * alpha);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from the specified hue, saturation, value, and alpha values.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="value">0..1 range value</param>
        /// <param name="alpha">0..1 alpha</param>
        /// <returns>The created <see cref="Color"/>.</returns>
        public static Color FromHsv(double hue, double saturation, double value, double alpha = 1.0)
        {
            if (hue < 0 || hue > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue));
            }

            double chroma = value * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
            double m = value - chroma;
            double r1, g1, b1;

            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            byte r = (byte)(255 * (r1 + m));
            byte g = (byte)(255 * (g1 + m));
            byte b = (byte)(255 * (b1 + m));
            byte a = (byte)(255 * alpha);

            return Color.FromArgb(a, r, g, b);
        }
    }
}