using System;
using System.Runtime.InteropServices;

namespace Trains.NET.Rendering.Software
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Pixel : IEquatable<Pixel>
    {
        public byte Alpha { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public Pixel(byte red, byte green, byte blue) : this(255, red, green, blue)
        {
        }
        public Pixel(byte alpha, byte red, byte green, byte blue)
        {
            this.Alpha = alpha;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
        public override bool Equals(object? obj) => obj is Pixel pixel && Equals(pixel);
        public bool Equals(Pixel other) => this.Alpha == other.Alpha &&
                                           this.Red == other.Red &&
                                           this.Green == other.Green &&
                                           this.Blue == other.Blue;
        public override int GetHashCode()
        {
            int hashCode = -1096401256;
            hashCode = hashCode * -1521134295 + this.Alpha.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Red.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Green.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Blue.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Pixel left, Pixel right) => left.Equals(right);
        public static bool operator !=(Pixel left, Pixel right) => !(left == right);
    }
}
