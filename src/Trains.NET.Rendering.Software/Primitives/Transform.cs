using System;

namespace Trains.NET.Rendering.Software
{
    internal struct Transform : IEquatable<Transform>
    {
        public float X { get; }
        public float Y { get; }
        public float Rotation { get; }

        public Transform(float x, float y, float rotation)
        {
            this.X = x;
            this.Y = y;
            this.Rotation = rotation;
        }

        public override bool Equals(object? obj) => obj is Transform transform &&
                                                    Equals(transform);

        public override int GetHashCode()
        {
            int hashCode = 281871178;
            hashCode = hashCode * -1521134295 + this.X.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Y.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Rotation.GetHashCode();
            return hashCode;
        }

        public Transform Clone() => new Transform(this.X, this.Y, this.Rotation);

        public Transform Translate(Point value) => Translate(value.X, value.Y);
        public Transform Translate(float x, float y)
        {
            if(this.Rotation == 0)
            {
                return new Transform(this.X + x, this.Y + y, this.Rotation);
            }

            float cos = (float)Math.Cos(this.Rotation);
            float sin = (float)Math.Sin(this.Rotation);

            float xRotated = x * cos - y * sin;
            float yRotated = x * sin + y * cos;

            return new Transform(this.X + xRotated, this.Y + yRotated, 0);
        }
        public Transform Rotate(float rotation) => new Transform(this.X, this.Y, rotation);

        public NormalisedPoint NormalisePoint(Point point) => Translate(point.X, point.Y).ToNormalisedPoint();

        public Point ToPoint() => new Point(this.X, this.Y);
        private NormalisedPoint ToNormalisedPoint() => new NormalisedPoint(this.X, this.Y);

        public bool Equals(Transform other) => this.X == other.X &&
                                                        this.Y == other.Y &&
                                                        this.Rotation == other.Rotation;

        public static bool operator ==(Transform left, Transform right) => left.Equals(right);

        public static bool operator !=(Transform left, Transform right) => !(left == right);
    }
}
