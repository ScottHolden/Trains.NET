namespace Trains.NET.Rendering.Software
{
    internal partial class Canvas
    {
        public void DrawRect(Point topLeft, Size size, Pixel source, int strokeWidth, bool drawStroke, bool drawFill)
        {
            if (drawFill)
            {
                FillRect(topLeft, size, source);
            }
            if (drawStroke)
            {
                StrokeRect(topLeft, size, source, strokeWidth);
            }
        }

        private void StrokeRect(Point topLeft, Size size, Pixel source, int strokeWidth)
        {
            Point topRight = topLeft.AddX(size.Width);
            Point bottomLeft = topLeft.AddY(size.Height);
            Point bottomRight = topRight.AddY(size.Height);

            DrawLine(topLeft, topRight, source, strokeWidth);
            DrawLine(topLeft, bottomLeft, source, strokeWidth);

            DrawLine(topRight, bottomRight, source, strokeWidth);
            DrawLine(topLeft, bottomRight, source, strokeWidth);
        }
        private void FillRect(Point topLeft, Size size, Pixel source)
        {
            Point topRight = topLeft.AddX(size.Width);

            NormalisedPoint normalisedTopLeft = _currentTransform.NormalisePoint(topLeft);
            NormalisedPoint normalisedTopRight = _currentTransform.NormalisePoint(topRight);
            NormalisedPoint normalisedBottomLeft = _currentTransform.NormalisePoint(topLeft.AddY(size.Height));
            NormalisedPoint normalisedBottomRight = _currentTransform.NormalisePoint(topRight.AddY(size.Height));

            // TODO: Are these the same?
            float leftSteps = normalisedTopLeft.DistanceTo(normalisedBottomLeft);
            float rightSteps = normalisedTopRight.DistanceTo(normalisedBottomRight);
            float steps = System.Math.Max(leftSteps, rightSteps);

            for (float i = 0; i < steps; i += PixelDensity)
            {
                NormalisedPoint normalisedStart = normalisedTopLeft.MapBetween(normalisedBottomLeft, i, 0, steps);
                NormalisedPoint normalisedEnd = normalisedTopRight.MapBetween(normalisedBottomRight, i, 0, steps);

                FillNormalisedLine(normalisedStart, normalisedEnd, source);
            }
        }
    }
}
