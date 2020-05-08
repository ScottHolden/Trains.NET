using System;
using System.Collections.Generic;
using System.IO;

namespace Trains.NET.Rendering.Software
{
    public class SoftwareCanvas : ICanvas
    {
        private const float PIFloat = (float)Math.PI;
        private readonly Canvas _canvas;
        private readonly Dictionary<string, Pixel> _pixelColorCache;

        private static class Defaults
        {
            public static readonly int StrokeWidth = 1;
            public static readonly PaintStyle PaintStyle = PaintStyle.Stroke;
            public static readonly Color Color = Colors.Black;
            public static readonly int TextSize = 10;
        }

        public SoftwareCanvas(int width, int height)
        {
            _canvas = new Canvas(width, height);
            _pixelColorCache = new Dictionary<string, Pixel>();
        }
        public void Clear(Color color) => _canvas.Clear(ColorToPixel(color));

        // TODO: Text alignment
        public void DrawText(string text, float x, float y, PaintBrush paint) =>
            _canvas.DrawText(new Point(x, y),
                                text,
                                ColorToPixel(paint.Color ?? Defaults.Color),
                                paint.TextSize ?? Defaults.TextSize);


        // TODO: Add gradients
        public void GradientRect(float x, float y, float width, float height, Color start, Color end) =>
            DrawRect(x, y, width, height, new PaintBrush {
                Color = start,
                Style = PaintStyle.Fill
            });
        public void DrawCircle(float x, float y, float radius, PaintBrush paint) =>
            _canvas.DrawCircle(new Point(x, y),
                                radius,
                                ColorToPixel(paint.Color ?? Defaults.Color),
                                paint.StrokeWidth ?? Defaults.StrokeWidth,
                                ShouldDrawStroke(paint.Style ?? Defaults.PaintStyle),
                                ShouldDrawFill(paint.Style ?? Defaults.PaintStyle));

        public void DrawLine(float x1, float y1, float x2, float y2, PaintBrush paint) =>
            _canvas.DrawLine(new Point(x1, y1), 
                                new Point(x2, y2),
                                ColorToPixel(paint.Color ?? Defaults.Color),
                                paint.StrokeWidth ?? Defaults.StrokeWidth);

        public void DrawPath(IPath trackPath, PaintBrush paint) =>
            _canvas.DrawPath((SoftwarePath)trackPath,
                                ColorToPixel(paint.Color ?? Defaults.Color),
                                paint.StrokeWidth ?? Defaults.StrokeWidth);
        public void DrawRect(float x, float y, float width, float height, PaintBrush paint) =>
            _canvas.DrawRect(new Point(x, y),
                        new Size(width, height),
                        ColorToPixel(paint.Color ?? Defaults.Color),
                        paint.StrokeWidth ?? Defaults.StrokeWidth,
                        ShouldDrawStroke(paint.Style ?? Defaults.PaintStyle),
                        ShouldDrawFill(paint.Style ?? Defaults.PaintStyle));

        public void RotateDegrees(float degrees, float x, float y)
        {
            Translate(x, y);
            RotateDegrees(degrees);
            Translate(-x, -y);
        }
        public void RotateDegrees(float degrees) => _canvas.Rotate(DegreesToRadians(degrees));
        public void Translate(float x, float y) => _canvas.Translate(x, y);
        public void Save() => _canvas.Save();
        public void Restore() => _canvas.Restore();

        public void DrawToStream(Stream stream, SoftwarePixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case SoftwarePixelFormat.ARGB_4Byte:
                    _canvas.DrawARGB(stream);
                    break;
                default:
                    throw new Exception("Color mode not supported");
            }
        }

        private float DegreesToRadians(float degrees) => degrees / (180.0f / PIFloat);

        private bool ShouldDrawStroke(PaintStyle style) => style switch
        {
            PaintStyle.Fill => false,
            PaintStyle.Stroke => true,
            _ => throw new NotImplementedException(),
        };

        private bool ShouldDrawFill(PaintStyle style) => style switch
        {
            PaintStyle.Fill => true,
            PaintStyle.Stroke => false,
            _ => throw new NotImplementedException(),
        };

        private Pixel ColorToPixel(Color color)
        {
            if (!_pixelColorCache.TryGetValue(color.HexCode, out Pixel pixel))
            {
                pixel = HexCodeHelper.ParseHexCode(color.HexCode);

                _pixelColorCache.Add(color.HexCode, pixel);
            }

            return pixel;
        }
    }
}
