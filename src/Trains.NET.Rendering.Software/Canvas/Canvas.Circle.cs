using System;

namespace Trains.NET.Rendering.Software
{
    internal partial class Canvas
    {
        public void DrawCircle(Point point, float radius, Pixel pixel, int strokeWidth, bool drawStroke, bool drawFill)
        {
            if (drawFill)
            {
                FillCircle(point, radius, pixel);
            }
            if (drawStroke)
            {
                StrokeCircle(point, radius, pixel, strokeWidth);
            }
        }

        private void FillCircle(Point point, float radius, Pixel source) =>
            DrawNormalisedCircle(_currentTransform.NormalisePoint(point), 0, radius, source);

        private void StrokeCircle(Point point, float radius, Pixel source, int strokeWidth)
        {
            int innerStroke = strokeWidth / 2;
            int outerStroke = strokeWidth - innerStroke;

            DrawNormalisedCircle(_currentTransform.NormalisePoint(point), radius - innerStroke, radius + outerStroke, source);
        }

        private void DrawNormalisedCircle(NormalisedPoint normalisedCenter, float minRadius, float maxRadius, Pixel source)
        {
            float steps = 2.0f * (float)Math.PI * maxRadius;

            for (float i = 0; i < steps; i += PixelDensity)
            {
                float angle = i / steps * 2.0f * (float)Math.PI;

                NormalisedPoint innerPoint = normalisedCenter.Add(minRadius * (float)Math.Cos(angle), minRadius * (float)Math.Sin(angle));
                NormalisedPoint outerPoint = normalisedCenter.Add(maxRadius * (float)Math.Cos(angle), maxRadius * (float)Math.Sin(angle));

                FillNormalisedLine(innerPoint, outerPoint, source);
            }
        }

        public void DrawArc(Point point, float radius, Pixel source)
        {
            // TODO: Arc
        }
    }
}
