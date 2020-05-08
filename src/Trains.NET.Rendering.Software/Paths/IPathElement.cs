namespace Trains.NET.Rendering.Software
{
    internal interface IPathElement
    {
        void Apply(Canvas canvas, Pixel source, int strokeWidth);
    }
}
