﻿using System;
using System.Collections.Generic;

namespace Trains.NET.Rendering
{
    public interface ICanvas : IDisposable
    {
        public void DrawRect(float x, float y, float width, float height, PaintBrush paint);
        public void Save();
        public void Translate(float x, float y);
        public void DrawCircle(float x, float y, float radius, PaintBrush paint);
        public void Restore();
        public void DrawText(string text, float x, float y, PaintBrush paint);
        public void DrawLine(float x1, float y1, float x2, float y2, PaintBrush grid);
        public void ClipRect(Rectangle sKRect, bool antialias);
        public void RotateDegrees(float degrees, float x, float y);
        public void DrawPath(IPath trackPath, PaintBrush straightTrackPaint);
        public void RotateDegrees(float degrees);
        void Clear(Color color);
        void GradientRect(float x, float y, float width, float height, Color start, Color end);
        void Scale(float scaleX, float scaleY);
        void DrawImage(IImage cachedImage, int x, int y);
        float MeasureText(string text, PaintBrush paint);
        void DrawGradRect(int x, int y, int width, int height, Color topLeft, Color topRight, Color bottomLeft, Color bottomRight);
        void DrawVertexTriangleList(List<(float X, float Y)> points, List<Color> colors);
    }
}
