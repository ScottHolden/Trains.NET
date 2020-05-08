using System;

namespace Trains.NET.Rendering.Software
{
    /// <summary>
    /// This is exactly the same as a Point,
    ///  more of just a framework thing to differentiate between points that
    ///  haven't been normalised, and ones that are ready to use on the canvas array
    /// </summary>
    internal struct NormalisedPoint : IEquatable<NormalisedPoint>
    {
        public float X { get; }
        public float Y { get; }
        public NormalisedPoint(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public NormalisedPoint Add(Point value) => new NormalisedPoint(this.X + value.X, this.Y + value.Y);
        public NormalisedPoint Add(float x, float y) => new NormalisedPoint(this.X + x, this.Y + y);
        public float DistanceTo(NormalisedPoint other) => (float)Math.Sqrt(Math.Pow(this.X - other.X, 2) + Math.Pow(this.Y - other.Y, 2));
        public NormalisedPoint MapBetween(NormalisedPoint other, float value, float min, float max) => new NormalisedPoint(
            (other.X - this.X) * ((value - min) / (max - min)) + this.X,
            (other.Y - this.Y) * ((value - min) / (max - min)) + this.Y
        );

        public override bool Equals(object? obj) => obj is NormalisedPoint point && Equals(point);
        public bool Equals(NormalisedPoint other) => this.X == other.X && this.Y == other.Y;
        public int ToIndex(int width) => (int)(this.Y * width + this.X);
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + this.X.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Y.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(NormalisedPoint left, NormalisedPoint right) => left.Equals(right);
        public static bool operator !=(NormalisedPoint left, NormalisedPoint right) => !(left == right);
    }
}
