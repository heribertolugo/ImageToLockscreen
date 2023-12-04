using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageToLockscreen.Ui.Converters
{
    public sealed class DarkenColorConverter : IValueConverter
    {
        private static readonly int MinLightness = 1;
        private static readonly int MaxLightness = 10;
        private static readonly float MinLightnessCoef = 1f;
        private static readonly float MaxLightnessCoef = 0.4f;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int lightness;
            if (value == null) return new SolidColorBrush(Colors.Yellow);
            if (parameter == null || !int.TryParse(parameter.ToString(), out lightness)) lightness = 3;

            ColorBytes hexAsBytes = HexStringToByteArray(value.ToString());

            return new SolidColorBrush(ChangeLightness(hexAsBytes.Color, lightness));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static Color ChangeLightness(Color color, int lightness)
        {
            if (lightness < MinLightness)
                lightness = MinLightness;
            else if (lightness > MaxLightness)
                lightness = MaxLightness;

            float coef = MinLightnessCoef +
                (
                    (lightness - MinLightness) * ((MaxLightnessCoef - MinLightnessCoef) / (MaxLightness - MinLightness))
                );
            return Color.FromArgb(color.A, (byte)(color.R * coef), (byte)(color.G * coef), (byte)(color.B * coef));
        }

        private static ColorBytes HexStringToByteArray(string hex)
        {
            hex = hex.Replace("#", "");
            if (hex.Length % 2 == 1 && hex.Length != 3)
                throw new ArgumentException("Hex length is invalid");

            if (hex.Length == 3)
            {
                char[] vals = new char[6];
                int index = 0;

                foreach (char val in hex)
                {
                    vals[index++] = val;
                    vals[index++] = val;
                }
                hex = new string(vals);
            }

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; i++)
                arr[i] = (byte)((GetHexValue(hex[i << 1]) << 4) + (GetHexValue(hex[(i << 1) + 1])));

            return new ColorBytes(arr);
        }

        private static int GetHexValue(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }

    internal struct ColorBytes
    {
        public ColorBytes(byte[] bytes)
        {
            this.A = bytes[0];
            this.R = bytes[1];
            this.G = bytes[2];
            this.B = bytes[3];
            this.Color = Color.FromArgb(this.A, this.R, this.G, this.B);
        }

        public byte A { get; private set; }
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }
        public Color Color { get; private set; }
    }
}
