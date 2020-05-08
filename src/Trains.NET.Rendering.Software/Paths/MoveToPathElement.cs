namespace Trains.NET.Rendering.Software
{
    internal class MoveToPathElement : PointPathElement
    {
        public MoveToPathElement(float x, float y) : base(x, y)
        { }
        public override void Apply(Canvas canvas, Pixel source, int strokeWidth) => canvas.Translate(X, Y);
    }
}
