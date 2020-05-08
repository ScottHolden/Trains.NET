using System;

namespace Trains.NET.Rendering.Software
{
    internal struct Point : IEquatable<Point>
    {
        public float X { get; }
        public float Y { get; }
        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        public Point AddX(float value) => new Point(this.X + value, this.Y);
        public Point AddY(float value) => new Point(this.X, this.Y + value);
        public Point Add(Point value) => new Point(this.X + value.X, this.Y + value.Y);
        public float DistanceTo(Point other) => (float)Math.Sqrt(Math.Pow(this.X - other.X, 2) + Math.Pow(this.Y - other.Y, 2));
        public Point MapBetween(Point other, float value, float min, float max) => new Point(
            (other.X - this.X) * ((value - min) / (max - min)) + this.X,
            (other.Y - this.Y) * ((value - min) / (max - min)) + this.Y
        );

        public override bool Equals(object? obj) => obj is Point point && Equals(point);
        public bool Equals(Point other) => this.X == other.X && this.Y == other.Y;
        public int ToIndex(int width) => (int)(this.Y * width + this.X);
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + this.X.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Y.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Point left, Point right) => left.Equals(right);
        public static bool operator !=(Point left, Point right) => !(left == right);

        public static readonly Point Zero = new Point(0, 0);
    }
}
