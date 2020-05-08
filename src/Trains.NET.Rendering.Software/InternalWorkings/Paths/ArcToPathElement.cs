namespace Trains.NET.Rendering.Software
{
    internal class ArcToPathElement : PointPathElement
    {
        private readonly bool _counterClockwise;
        private readonly float _radius;
        public ArcToPathElement(float x, float y, float radius, bool counterClockwise) : base(x, y)
        {
            _radius = radius;
            _counterClockwise = counterClockwise;
        }
        public override void Apply(Canvas canvas, Pixel source, int strokeWidth)
        {
            // TODO: Arc Implementation
            // throw new NotImplementedException();
            // Eat it!
        }
    }
}
