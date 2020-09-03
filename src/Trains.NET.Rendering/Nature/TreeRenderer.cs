using System;
using Trains.NET.Engine;

namespace Trains.NET.Rendering
{
    public class TreeRenderer : ICachableRenderer<Tree>
    {
        private readonly float _maxTreeSize;
        private readonly float _minTreeSize;
        private readonly float _centerOffset;
        private readonly float _baseRadius;
        private readonly PaintBrush _baseTreeBrush;
        private readonly PaintBrush _topTreeBrush;

        // Change this if you don't like the majority of tree styles
        private const int SeedOffset = 1337;

        // Change this if you want more variance/styles
        private const int MaxStyles = 100;

        public TreeRenderer()
        {
            _centerOffset = 50.0f;
            _baseRadius = 25.0f;
            _minTreeSize = _baseRadius;
            _maxTreeSize = _baseRadius * 1.25f;
            _baseTreeBrush = new PaintBrush
            {
                Color = new Color("#1B633A"),
                Style = PaintStyle.Fill
            };
            _topTreeBrush = new PaintBrush
            {
                Color = new Color("#236A42"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
        }

        public string GetCacheKey(Tree item) => GetTrimmedTreeSeed(item).ToString();

        private static int GetTrimmedTreeSeed(Tree tree) => tree.Seed % MaxStyles;

        public void Render(ICanvas canvas, Tree tree)
        {
            canvas.Translate(_centerOffset, _centerOffset);

            // Let's make some repeatable numbers
            var r = new Random(SeedOffset + GetTrimmedTreeSeed(tree));
            int circleCount = r.Next(10, 20);

            // Draw a base fill
            canvas.DrawCircle(0, 0, _baseRadius, _baseTreeBrush);

            // Draw a few base circles
            float angleOffset = r.NextFloat(0, (float)(Math.PI / 2.0f));
            DrawTreeLayer(canvas, r, circleCount, angleOffset, 1.0f, _baseTreeBrush);

            // Draw a few top circles
            angleOffset = r.NextFloat(0, (float)(Math.PI / 2.0f));
            DrawTreeLayer(canvas, r, (int)(circleCount * 0.7), angleOffset, 0.5f, _topTreeBrush);

            Mouse(canvas);
        }

        private static void Mouse(ICanvas canvas)
        {
            var a = new PaintBrush
            {
                Color = new Color("#444444"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
            var b = new PaintBrush
            {
                Color = new Color("#777777"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
            var c = new PaintBrush
            {
                Color = new Color("#221122"),
                Style = PaintStyle.Stroke,
                StrokeWidth = 3,
                IsAntialias = true
            };
            canvas.DrawCircle(-25, -20, 20, a);
            canvas.DrawCircle(25, -20, 20, a);
            canvas.DrawCircle(0, 10, 30, b);
            canvas.DrawLine(-10, 20, -40, 20, c);
            canvas.DrawLine(-10, 20, -40, 0, c);
            canvas.DrawLine(-10, 20, -40, 40, c);
            canvas.DrawLine(10, 20, 40, 20, c);
            canvas.DrawLine(10, 20, 40, 0, c);
            canvas.DrawLine(10, 20, 40, 40, c);
            canvas.DrawCircle(0, 10, 10, a);
        }

        private void DrawTreeLayer(ICanvas canvas, Random r, int circleCount, float angleOffset, float scale, PaintBrush brush)
        {
            for (int i = 0; i < circleCount; i++)
            {
                float angle = angleOffset + (float)(Math.PI * 2.0 * i / circleCount);
                float offset = r.NextFloat(_minTreeSize, _maxTreeSize);
                float radius = r.NextFloat(10.0f, 15.0f);
                float x = (float)(scale * offset * Math.Cos(angle));
                float y = (float)(scale * offset * Math.Sin(angle));

                canvas.DrawCircle(x, y, radius, brush);
            }
        }
    }
}
