namespace Trains.NET.Rendering.Software
{
    internal partial class Canvas
    {
        public void DrawLine(Point end, Pixel source, int strokeWidth) => DrawLine(Point.Zero, end, source, strokeWidth);
        public void DrawLine(Point start, Point end, Pixel source, int strokeWidth)
        {
            // TODO: Line width! Only support 1 width at the moment!
            DrawLine(start, end, source);
        }
        public void DrawLine(Point start, Point end, Pixel source) =>
            FillNormalisedLine(_currentTransform.NormalisePoint(start),
                                _currentTransform.NormalisePoint(end),
                                source);

        private void FillNormalisedLine(NormalisedPoint normalisedStart, NormalisedPoint normalisedEnd, Pixel source)
        {
            float steps = normalisedStart.DistanceTo(normalisedEnd);

            for (float i = 0; i <= steps; i += PixelDensity)
            {
                NormalisedPoint drawPoint = normalisedStart.MapBetween(normalisedEnd, i, 0, steps);

                DrawNormalisedPoint(drawPoint, source);
            }
        }
    }
}
