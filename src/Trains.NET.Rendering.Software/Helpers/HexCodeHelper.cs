using System;

namespace Trains.NET.Rendering.Software
{
    internal static class HexCodeHelper
    {
        public static Pixel ParseHexCode(string hexCodeString)
        {
            char[] hexCode = hexCodeString.ToUpperInvariant().TrimStart('#').Trim().ToCharArray();

            return hexCode.Length switch
            {
                // AARRGGBB
                8 => new Pixel(
                    HexToByte(hexCode, 0, 2),
                    HexToByte(hexCode, 2, 2),
                    HexToByte(hexCode, 4, 2),
                    HexToByte(hexCode, 6, 2)
                ),
                // RRGGBB
                6 => new Pixel(
                    HexToByte(hexCode, 0, 2),
                    HexToByte(hexCode, 2, 2),
                    HexToByte(hexCode, 4, 2)
                ),
                // ARGB
                4 => new Pixel(
                    ShorthandHexToByte(hexCode[0]),
                    ShorthandHexToByte(hexCode[1]),
                    ShorthandHexToByte(hexCode[2]),
                    ShorthandHexToByte(hexCode[3])
                ),
                // RGB
                3 => new Pixel(
                    ShorthandHexToByte(hexCode[0]),
                    ShorthandHexToByte(hexCode[1]),
                    ShorthandHexToByte(hexCode[2])
                ),
                _ => throw new Exception("Unknown hexCode format: " + hexCodeString)
            };

        }

        private static byte ShorthandHexToByte(char c)
        {
            byte value = HexToByte(c);
            return (byte)(value * 16 + value);
        }
        private static byte HexToByte(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return (byte)(c - '0');
            }
            if (c >= 'A' && c <= 'F')
            {
                return (byte)(10 + c - 'A');
            }

            throw new Exception("Bad hex character: " + c);
        }
        private static byte HexToByte(char[] c, int offset, int count)
        {
            byte value = 0;
            for (int i = 0; i < count; i++)
            {
                value *= 16;
                value += HexToByte(c[offset + i]);
            }
            return value;
        }
    }
}
