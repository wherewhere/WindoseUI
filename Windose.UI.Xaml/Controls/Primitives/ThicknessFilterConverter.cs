using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Windose.UI.Xaml.Controls.Primitives
{
    public class ThicknessFilterConverter : DependencyObject, IValueConverter
    {
        public ThicknessFilterKind Filter { get; set; }

        public double Scale { get; set; } = 1.0;

        public Thickness Convert(Thickness thickness, ThicknessFilterKind filterKind)
        {
            Thickness result = new Thickness();

            if (filterKind.HasFlag(ThicknessFilterKind.Top))
            {
                result.Top = thickness.Top;
            }
            
            if (filterKind.HasFlag(ThicknessFilterKind.Right))
            {
                result.Right = thickness.Right;
            }
            
            if (filterKind.HasFlag(ThicknessFilterKind.Bottom))
            {
                result.Bottom = thickness.Bottom;
            }
            
            if (filterKind.HasFlag(ThicknessFilterKind.Left))
            {
                result.Left = thickness.Left;
            }

            return result;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Thickness thickness = (Thickness)value;

            double scale = Scale;
            if (!double.IsNaN(scale))
            {
                thickness.Top *= scale;
                thickness.Right *= scale;
                thickness.Bottom *= scale;
                thickness.Left *= scale;
            }

            ThicknessFilterKind filterType = Filter;

            return Convert(thickness, filterType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    [Flags]
    public enum ThicknessFilterKind
    {
        None = 0b0000,
        Top = 0b0001,
        Right = 0b0010,
        Bottom = 0b0100,
        Left = 0b1000
    }
}
