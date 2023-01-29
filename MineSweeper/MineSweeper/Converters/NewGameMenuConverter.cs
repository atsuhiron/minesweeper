using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Game;

namespace MineSweeper.Converters
{
    public class NewGameMenuConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return new object();

            if (values[0] is string fieldParameterPresetStr && values[1] is Grid mineGrid)
            {
                var preset = FieldParameter.ParsePresetFromString(fieldParameterPresetStr);
                return new NewGameMenuComplex(preset, mineGrid);
            }

            return new object();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is NewGameMenuComplex menuComplex)
            {
                return new object[] { menuComplex.FieldParameterPreset.ToString(), menuComplex.MineGrid };
            }
            return Array.Empty<object>();
        }
    }

    public class NewGameMenuComplex
    {
        public FieldParameterPreset FieldParameterPreset { get; init; }
        public Grid MineGrid { get; init; }

        public NewGameMenuComplex(FieldParameterPreset fieldParameterPreset, Grid mineGrid)
        {
            FieldParameterPreset = fieldParameterPreset;
            MineGrid = mineGrid;
        }
    }
}
