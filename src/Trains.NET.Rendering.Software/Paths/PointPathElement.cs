namespace Trains.NET.Rendering.Software
{
    internal abstract class PointPathElement : IPathElement
    {
        protected readonly float X;
        protected readonly float Y;
        public PointPathElement(float x, float y)
        {
            X = x;
            Y = y;
        }
        public abstract void Apply(Canvas canvas, Pixel source, int strokeWidth);
    }
}
