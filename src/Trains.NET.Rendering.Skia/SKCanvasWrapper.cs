using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Trains.NET.Rendering.Skia
{
    public class SKCanvasWrapper : ICanvas
    {
        private static readonly Dictionary<PaintBrush, SKPaint> s_paintCache = new();

        private readonly SkiaSharp.SKCanvas _canvas;

        public SKCanvasWrapper(SkiaSharp.SKCanvas canvas)
        {
            _canvas = canvas;
        }
        private static SKPaint GetSKPaint(PaintBrush paint)
        {
            if (!s_paintCache.TryGetValue(paint, out SKPaint skPaint))
            {
                skPaint = paint.ToSkia();
                s_paintCache.Add(paint, skPaint);
            }
            return skPaint;
        }

        public void Clear(Color color)
            => _canvas.Clear(color.ToSkia());

        public void ClipRect(Rectangle rect, bool antialias)
            => _canvas.ClipRect(rect.ToSkia(), antialias: antialias);

        public void Dispose()
        {
            ((IDisposable)_canvas).Dispose();
        }

        public void DrawImage(IImage image, int x, int y)
            => _canvas.DrawImage(image.ToSkia(), x, y);


        public void DrawCircle(float x, float y, float radius, PaintBrush paint)
            => _canvas.DrawCircle(x, y, radius, GetSKPaint(paint));

        public void DrawLine(float x1, float y1, float x2, float y2, PaintBrush paint)
            => _canvas.DrawLine(x1, y1, x2, y2, GetSKPaint(paint));

        public void DrawPath(IPath trackPath, PaintBrush paint)
            => _canvas.DrawPath(trackPath.ToSkia(), GetSKPaint(paint));

        public void DrawRect(float x, float y, float width, float height, PaintBrush paint)
            => _canvas.DrawRect(x, y, width, height, GetSKPaint(paint));

        public void DrawText(string text, float x, float y, PaintBrush paint)
            => _canvas.DrawText(text, x, y, GetSKPaint(paint));

        public void GradientRect(float x, float y, float width, float height, Color start, Color end)
        {
            var shader = SKShader.CreateLinearGradient(new SKPoint(x, y),
                                                       new SKPoint(x, y + height),
                                                       new SKColor[] { start.ToSkia(), end.ToSkia(), start.ToSkia() },
                                                       SKShaderTileMode.Clamp);
            using var paint = new SKPaint
            {
                Shader = shader
            };
            _canvas.DrawRect(x, y, width, height, paint);
        }

        public void Restore()
            => _canvas.Restore();

        public void Scale(float scaleX, float scaleY)
            => _canvas.Scale(scaleX, scaleY);

        public void RotateDegrees(float degrees, float x, float y)
            => _canvas.RotateDegrees(degrees, x, y);

        public void RotateDegrees(float degrees)
            => _canvas.RotateDegrees(degrees);

        public void Save()
            => _canvas.Save();

        public void Translate(float x, float y)
            => _canvas.Translate(x, y);

        public float MeasureText(string text, PaintBrush paint)
            => GetSKPaint(paint).MeasureText(text);

        public void DrawVertexTriangleList(List<(float X, float Y)> points, List<Color> colors)
        {
            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill
            };

            _canvas.DrawVertices(SKVertexMode.Triangles, 
                                 points.Select(x=>new SKPoint(x.X, x.Y)).ToArray(),
                                 colors.Select(x=>x.ToSkia()).ToArray(),
                                 paint);
        }

        public void DrawGradRect(int x, int y, int width, int height, Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
        {
            SKColor topLeftSkia = topLeft.ToSkia();
            SKColor topRightSkia = topRight.ToSkia();
            SKColor bottomLeftSkia = bottomLeft.ToSkia();
            SKColor bottomRightSkia = bottomRight.ToSkia();

            /*using var topLeftPaint = new SKPaint
            {
                Shader = SKShader.CreateSweepGradient(new SKPoint(x + width / 2, y + height / 2),
                                                       new SKColor[] { bottomRightSkia, bottomLeftSkia, topLeftSkia, topRightSkia, bottomRightSkia }, null, SKMatrix.CreateRotationDegrees(45, x + width / 2, y + height / 2)
                                                       )
            };*/
            //_canvas.RotateDegrees(45, x + width / 2, y + height / 2);
            //_canvas.DrawRect(x, y, width, height, topLeftPaint);


            /*
            using var topLeftPaint = new SKPaint
            {
                Color = topLeftSkia,
                Style = SKPaintStyle.Fill
            };
            _canvas.DrawRect(x, y, width / 2, height / 2, topLeftPaint);

            using var topRightPaint = new SKPaint
            {
                Color = topRightSkia,
                Style = SKPaintStyle.Fill
            };
            _canvas.DrawRect(x + width / 2, y, width / 2, height / 2, topRightPaint);

            using var bottomLeftPaint = new SKPaint
            {
                Color = bottomLeftSkia,
                Style = SKPaintStyle.Fill
            };
            _canvas.DrawRect(x, y + height / 2, width / 2, height / 2, bottomLeftPaint);

            using var bottomRightPaint = new SKPaint
            {
                Color = bottomRightSkia,
                Style = SKPaintStyle.Fill
            };
            _canvas.DrawRect(x + width / 2, y + height / 2, width / 2, height / 2, bottomRightPaint);
            */


            /*
            float radius = (float)Math.Sqrt(width * width + width * width);

            using var topLeftShader = SKShader.CreateRadialGradient(new SKPoint(x, y + height / 2),
                                                       width,
                                                       new SKColor[] { topLeftSkia, bottomRightSkia },
                                                       SKShaderTileMode.Clamp);

            using var topRightShader = SKShader.CreateRadialGradient(new SKPoint(x + width / 2, y),
                                                       width,
                                                       new SKColor[] { topRightSkia, bottomLeftSkia },
                                                       SKShaderTileMode.Clamp);

            using var bottomLeftShader = SKShader.CreateRadialGradient(new SKPoint(x + width / 2, y + height),
                                                       width,
                                                       new SKColor[] { bottomLeftSkia, topRightSkia },
                                                       SKShaderTileMode.Clamp);

            using var bottomRightShader = SKShader.CreateRadialGradient(new SKPoint(x + width, y + height / 2),
                                                       width,
                                                       new SKColor[] { bottomRightSkia, topLeftSkia },
                                                       SKShaderTileMode.Clamp);

            using var s1 = SKShader.CreateCompose(topLeftShader, topRightShader, SKBlendMode.Lighten);
            using var s2 = SKShader.CreateCompose(bottomLeftShader, bottomRightShader, SKBlendMode.Lighten);
            using var compShader = SKShader.CreateCompose(s1, s2, SKBlendMode.Lighten);

            using var paint = new SKPaint
            {
                Shader = compShader
            };
            _canvas.DrawRect(x, y, width, height, paint);
            */

            SKPoint topLeftPoint = new SKPoint(x, y);
            SKPoint topRightPoint = new SKPoint(x + width, y);
            SKPoint bottomLeftPoint = new SKPoint(x, y + height);
            SKPoint bottomRightPoint = new SKPoint(x + width, y + height);

            SKPoint centerPoint = new SKPoint(x + width / 2, y + height / 2);

            SKPoint[] triangles = new[]{
                topLeftPoint, centerPoint, topRightPoint,
                topRightPoint, centerPoint, bottomRightPoint,
                bottomRightPoint, centerPoint, bottomLeftPoint,
                bottomLeftPoint, centerPoint, topLeftPoint
            };

            SKColor centerSkia = new SKColor(
                (byte)((topLeftSkia.Red + topRightSkia.Red + bottomLeftSkia.Red + bottomRightSkia.Red) / 4.0f),
                (byte)((topLeftSkia.Green + topRightSkia.Green + bottomLeftSkia.Green + bottomRightSkia.Green) / 4.0f),
                (byte)((topLeftSkia.Blue + topRightSkia.Blue + bottomLeftSkia.Blue + bottomRightSkia.Blue) / 4.0f)
            );

            SKColor[] triangleColors = new[] {
                topLeftSkia, centerSkia, topRightSkia,
                topRightSkia, centerSkia, bottomRightSkia,
                bottomRightSkia, centerSkia, bottomLeftSkia,
                bottomLeftSkia, centerSkia, topLeftSkia
            };

            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill
            };

            _canvas.DrawVertices(SKVertexMode.Triangles, triangles, triangleColors, paint);
        }
    }
}
