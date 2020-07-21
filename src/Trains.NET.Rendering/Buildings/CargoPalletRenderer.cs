using System;
using System.Collections.Generic;

namespace Trains.NET.Rendering.Buildings
{
    public class CargoPalletRenderer : ICargoPalletRenderer
    {
        private const float PalletSizeRatio = 0.5f;
        private const int SeedOffset = 1337;
        private const int MaxStyles = 100;

        private readonly Dictionary<int, IBitmap> _cachedPallets = new Dictionary<int, IBitmap>();
        private readonly ITrackParameters _parameters;
        private readonly IBitmapFactory _bitmapFactory;
        private readonly PaintBrush _palletPaint;
        private readonly PaintBrush _barrelPaint;
        private readonly PaintBrush[] _boxPaints;

        public CargoPalletRenderer(ITrackParameters parameters, IBitmapFactory bitmapFactory)
        {
            _parameters = parameters;
            _bitmapFactory = bitmapFactory;
            _palletPaint = new PaintBrush
            {
                Color = new Color("#7F6000"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
            _barrelPaint = new PaintBrush
            {
                Color = new Color("#3B3838"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
            _boxPaints = new[] {
                new PaintBrush
                {
                    Color = new Color("#BF9000"),
                    Style = PaintStyle.Fill,
                    IsAntialias = true
                },
                new PaintBrush
                {
                    Color = new Color("#E6AF00"),
                    Style = PaintStyle.Fill,
                    IsAntialias = true
                },new PaintBrush
                {
                    Color = new Color("#FFC305"),
                    Style = PaintStyle.Fill,
                    IsAntialias = true
                },
                new PaintBrush
                {
                    Color = new Color("#FFD966"),
                    Style = PaintStyle.Fill,
                    IsAntialias = true
                }
            };
        }
       
        private void DrawPallet(ICanvas canvas, int seed)
        {
            var r = new Random(SeedOffset + seed % MaxStyles);
            float palletSize = PalletSizeRatio * _parameters.CellSize;
            float halfPalletSize = palletSize / 2.0f;
            float baseBoxSize = halfPalletSize * 0.6f;
            float palletAngle = r.NextFloat(-6, 6);
            int boxes = r.Next(1, 5);
            canvas.Save();
            canvas.RotateDegrees(palletAngle);
            canvas.DrawRect(-halfPalletSize, -halfPalletSize, palletSize, palletSize, _palletPaint);
            float stackStraightness = r.NextFloat(0, 10);
            int maxStackHeight = _boxPaints.Length;
            canvas.RotateDegrees(40 + stackStraightness);
            for (int i = 0; i < boxes; i++)
            {
                int type = r.Next(1, 11);
                if (type == 8)
                {
                    canvas.DrawCircle(0, -halfPalletSize / 2, baseBoxSize * 0.6f, _barrelPaint);
                }
                else if (type < 8 || i < 1)
                {
                    int currentBoxSize = r.Next(maxStackHeight - 1, maxStackHeight);
                    canvas.Save();
                    canvas.Translate(0, -halfPalletSize / 2);
                    while (currentBoxSize > 0)
                    {
                        float widthRatio = r.NextFloat(0.8f, 1.0f);
                        float boxRotation = r.NextFloat(0, 90);
                        canvas.RotateDegrees(boxRotation);
                        int boxIndex = (maxStackHeight - currentBoxSize);
                        float currentSize = baseBoxSize / (boxIndex * 0.75f);
                        canvas.DrawRect(-currentSize / 2, -currentSize / 2, currentSize * widthRatio, currentSize, _boxPaints[boxIndex % _boxPaints.Length]);
                        currentBoxSize -= r.Next(1, 2);
                    }
                    canvas.Restore();
                }
                
                canvas.RotateDegrees(360.0f / boxes);
            }
            canvas.Restore();
        }

        public void Render(ICanvas canvas, int seed)
        {
            int index = seed % MaxStyles;
            if (!_cachedPallets.TryGetValue(index, out IBitmap cachedBitmap))
            {
                cachedBitmap = _bitmapFactory.CreateBitmap(_parameters.CellSize, _parameters.CellSize);
                ICanvas cachedCanvas = _bitmapFactory.CreateCanvas(cachedBitmap);

                cachedCanvas.Translate(_parameters.CellSize / 2, _parameters.CellSize / 2);
                DrawPallet(cachedCanvas, index);

                _cachedPallets.Add(index, cachedBitmap);
            }

            //canvas.DrawBitmap(cachedBitmap, 0, 0);
            DrawPallet(canvas, index);
        }
    }
}
