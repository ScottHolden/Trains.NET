﻿using System;
using System.Drawing;
using Comet.Skia;
using SkiaSharp;
using Trains.NET.Engine;
using Trains.NET.Engine.Tracks;
using Trains.NET.Rendering;
using Trains.NET.Rendering.Skia;

namespace Trains.NET.Comet
{
    internal class MiniMapDelegate : AbstractControlDelegate, IDisposable
    {
        private bool _redraw = true;
        private bool _shouldDraw;
        private float _dpi = 1.0f;
        private readonly ITrackLayout _trackLayout;
        private readonly IPixelMapper _pixelMapper;
        private readonly ITrackParameters _trackParameters;
        private readonly SKPaint _paint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Gray
        };
        private readonly SKPaint _viewPortPaint = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Green,
            StrokeWidth = 80
        };

        public MiniMapDelegate(ITrackLayout trackLayout, ITrackParameters trackParameters, IPixelMapper pixelMapper)
        {
            _trackLayout = trackLayout;
            _trackParameters = trackParameters;
            _pixelMapper = pixelMapper;

            _pixelMapper.ViewPortChanged += (s, e) => _redraw = true;
            _trackLayout.TracksChanged += (s, e) => _redraw = true;
        }

        public void FlagDraw() => _shouldDraw = true;

        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            if (!_shouldDraw) return;
            _shouldDraw = false;

            if (dirtyRect.IsEmpty) return;
            if (!_redraw) return;

            // Pull DPI from caller scale
            float newDpi = canvas.TotalMatrix.ScaleX;
            if (float.IsFinite(newDpi))
            {
                _dpi = newDpi;
            }
            canvas.RestoreToCount(-1);

            const int maxGridSize = PixelMapper.MaxGridSize;
            using var bitmap = new SKBitmap(maxGridSize, maxGridSize);

            using var tempCanvas = new SKCanvas(bitmap);
            tempCanvas.Clear(SKColor.Parse(Colors.VeryLightGray.HexCode));
            using var canvasWrapper = new SKCanvasWrapper(tempCanvas);

            foreach (Track track in _trackLayout)
            {
                (int x, int y) = _pixelMapper.CoordsToWorldPixels(track.Column, track.Row);
                tempCanvas.DrawRect(new SKRect(x, y, _trackParameters.CellSize + x, _trackParameters.CellSize + y), _paint);
            }

            tempCanvas.DrawRect(new SKRect(_pixelMapper.ViewPortX * -1, _pixelMapper.ViewPortY * -1, Math.Abs(_pixelMapper.ViewPortX) + _pixelMapper.ViewPortWidth, Math.Abs(_pixelMapper.ViewPortY) + _pixelMapper.ViewPortHeight), _viewPortPaint);

            canvas.DrawBitmap(bitmap, new SKRect(0, 0, maxGridSize, maxGridSize), new SKRect(0, 0, 100, 100));

            _redraw = false;
        }

        public override bool StartInteraction(PointF[] points)
        {
            DragInteraction(points);
            return true;
        }

        private (float x, float y) ConvertDPIScaledPointToRawPosition(PointF point) => (point.X * _dpi, point.Y * _dpi);

        public override void DragInteraction(PointF[] points)
        {
            (float mouseX, float mouseY) = ConvertDPIScaledPointToRawPosition(points[0]);

            float x = mouseX * (PixelMapper.MaxGridSize / 100);
            float y = mouseY * (PixelMapper.MaxGridSize / 100);

            x -= _pixelMapper.ViewPortWidth / 2;
            y -= _pixelMapper.ViewPortHeight / 2;

            _pixelMapper.SetViewPort((int)x, (int)y);
        }

        public void Dispose()
        {
            ((IDisposable)_paint).Dispose();
            ((IDisposable)_viewPortPaint).Dispose();
        }
    }
}
