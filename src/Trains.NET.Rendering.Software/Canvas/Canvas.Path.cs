namespace Trains.NET.Rendering.Software
{
    internal partial class Canvas
    {
        public void DrawPath(SoftwarePath path, Pixel source, int strokeWidth)
        {
            Save();
            foreach (IPathElement element in path.GetPathElements())
            {
                element.Apply(this, source, strokeWidth);
            }
            Restore();
        }
    }
}
