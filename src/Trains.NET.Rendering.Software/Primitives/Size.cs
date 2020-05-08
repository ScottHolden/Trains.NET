using System;

namespace Trains.NET.Rendering.Software
{
    internal struct Size : IEquatable<Size>
    {
        public float Width { get; }
        public float Height { get; }
        public Size(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public override bool Equals(object? obj) => obj is Size size && Equals(size);
        public bool Equals(Size other) => this.Width == other.Width && this.Height == other.Height;
        public override int GetHashCode()
        {
            int hashCode = 859600377;
            hashCode = hashCode * -1521134295 + this.Width.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Height.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Size left, Size right) => left.Equals(right);
        public static bool operator !=(Size left, Size right) => !(left == right);
    }
}
