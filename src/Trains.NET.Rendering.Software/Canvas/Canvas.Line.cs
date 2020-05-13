using System;

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
            /*
            float steps = normalisedStart.DistanceTo(normalisedEnd);

            for (float i = 0; i < steps; i += PixelDensity)
            {
                NormalisedPoint drawPoint = normalisedStart.MapBetween(normalisedEnd, i, 0, steps);

                DrawNormalisedPoint(drawPoint, source);
            }
            */

            int x0 = (int)normalisedStart.X;
            int y0 = (int)normalisedStart.Y;
            int x1 = (int)normalisedEnd.X;
            int y1 = (int)normalisedEnd.Y;

            var dx = Math.Abs(x1 - x0);
            var sx = x0 < x1 ? 1 : -1;
            var dy = -Math.Abs(y1 - y0);
            var sy = y0 < y1 ? 1 : -1;
            var err = dx + dy;
            while(true)
            {
                DrawNormalisedPoint(new NormalisedPoint(x0, y0), source);
                if (x0 == x1 && y0 == y1) break;
                var e2 = 2 * err;
                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }
                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }
    }
}
