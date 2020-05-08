using System;
using System.Collections.Generic;
using System.IO;

namespace Trains.NET.Rendering.Software
{
    internal class Canvas
    {
        private const int PixelDensity = 1;
        private Transform _currentTransform;
        private readonly Stack<Transform> _transformStack;
        private readonly int _width;
        private readonly int _height;
        private readonly Pixel[] _canvas;

        public Canvas(int width, int height)
        {
            _currentTransform = new Transform();
            _transformStack = new Stack<Transform>();
            _width = width;
            _height = height;
            _canvas = new Pixel[_width * _height];
        }

        public void DrawARGB(Stream output)
        {
            for(int i=0; i< _canvas.Length; i++)
            {
                output.WriteByte(_canvas[i].Alpha);
                output.WriteByte(_canvas[i].Red);
                output.WriteByte(_canvas[i].Green);
                output.WriteByte(_canvas[i].Blue);
            }
        }

        public void DrawText(Point point, string text, Pixel source, int textSize)
        {
            // TODO: Text
            // throw new NotImplementedException();
        }

        public void Clear(Pixel source)
        {
            // TODO: Implement Copy doubling method, this is too slow!
            for (int i = 0; i < _canvas.Length; i++)
            {
                _canvas[i] = source;
            }
        }
        public void DrawLine(Point start, Point end, Pixel source, int strokeWidth)
        {
            // TODO: Line width! Only support 1 width at the moment!
            DrawLine(start, end, source);
        }

        public void DrawCircle(Point point, float radius, Pixel source)
        {
            Point normalisedCenter = _currentTransform.NormalisePoint(point);

            float steps = 2.0f * (float)Math.PI * radius;

            for (float i = 0; i < steps; i += PixelDensity)
            {
                float angle = i / steps * 2.0f * (float)Math.PI;

                Point drawPoint = normalisedCenter.Add(radius * (float)Math.Cos(angle), radius * (float)Math.Sin(angle));

                DrawPoint(drawPoint, source);
            }
        }

        public void DrawLine(Point start, Point end, Pixel source) =>
            FillNormalisedLine(_currentTransform.NormalisePoint(start),
                                _currentTransform.NormalisePoint(end),
                                source);

        private void FillNormalisedLine(Point normalisedStart, Point normalisedEnd, Pixel source)
        {
            float steps = normalisedStart.DistanceTo(normalisedEnd);

            for (float i = 0; i < steps; i += PixelDensity)
            {
                Point drawPoint = normalisedStart.MapBetween(normalisedEnd, i, 0, steps);

                DrawPoint(drawPoint, source);
            }
        }

        private void DrawPoint(Point normalisedPoint, Pixel source) => _canvas[normalisedPoint.ToIndex(_width)] = source;

        public void DrawLine(Point end, Pixel source, int strokeWidth) => DrawLine(Point.Zero, end, source, strokeWidth);

        public void DrawRect(Point topLeft, Size size, Pixel source, int strokeWidth, bool drawStroke, bool drawFill)
        {
            if(drawStroke)
            {
                StrokeRect(topLeft, size, source, strokeWidth);
            }
            if (drawFill)
            {
                FillRect(topLeft, size, source);
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

            Point normalisedTopLeft = _currentTransform.NormalisePoint(topLeft);
            Point normalisedTopRight = _currentTransform.NormalisePoint(topRight);
            Point normalisedBottomLeft = _currentTransform.NormalisePoint(topLeft.AddY(size.Height));
            Point normalisedBottomRight = _currentTransform.NormalisePoint(topRight.AddY(size.Height));

            // Are these the same?
            float leftSteps = normalisedTopLeft.DistanceTo(normalisedBottomLeft);
            float rightSteps = normalisedTopRight.DistanceTo(normalisedBottomRight);
            float steps = Math.Max(leftSteps, rightSteps);

            for (float i = 0; i < steps; i += PixelDensity)
            {
                Point normalisedStart = normalisedTopLeft.MapBetween(normalisedBottomLeft, i, 0, steps);
                Point normalisedEnd = normalisedTopRight.MapBetween(normalisedBottomRight, i, 0, steps);

                FillNormalisedLine(normalisedStart, normalisedEnd, source);
            }
        }
        public void DrawPath(SoftwarePath path, Pixel source, int strokeWidth)
        {
            Save();
            foreach(IPathElement element in path.GetPathElements())
            {
                element.Apply(this, source, strokeWidth);
            }
            Restore();
        }

        public void Rotate(float radians) => _currentTransform = _currentTransform.Rotate(radians);
        public void Translate(float x, float y) => _currentTransform = _currentTransform.Translate(x, y);

        public void Save() => _transformStack.Push(_currentTransform.Clone());
        public void Restore()
        {
            if (_transformStack.Count < 1)
            {
                throw new Exception("Nothing saved");
            }
            _currentTransform = _transformStack.Pop();
        }
    }
}
