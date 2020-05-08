namespace Trains.NET.Rendering.Software
{
    internal class LineToPathElement : PointPathElement
    {
        public LineToPathElement(float x, float y) : base(x, y)
        { }
        public override void Apply(Canvas canvas, Pixel source, int strokeWidth) 
        {
            canvas.DrawLine(new Point(X, Y), source, strokeWidth);
            canvas.Translate(X, Y);
        }
    }
}
