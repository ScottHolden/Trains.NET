namespace Trains.NET.Rendering
{
    public class BenchRenderer : IBenchRenderer
    {
        private const float BenchWidthRatio = 1.0f / 4.0f;
        private const float BenchHeightRatio = 1.0f / 8.0f;
        private const float SlatBackInsetRatio = 0.95f;
        private const float LegSlatInsetRatio = 0.95f;
        private const float LegWidthHeightRatio = 1.0f / 3.0f;

        private const float BackRatio = 1.0f / 5;
        private const int Slats = 3;
        private const float SlatGapRatio = 0.5f;

        private readonly ITrackParameters _parameters;

        private readonly PaintBrush _slatPaint;
        private readonly PaintBrush _backPaint;
        private readonly PaintBrush _legPaint;

        public BenchRenderer(ITrackParameters parameters)
        {
            _parameters = parameters;
            _slatPaint = new PaintBrush
            {
                Color = new Color("#7F6000"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
            _backPaint = new PaintBrush
            {
                Color = new Color("#BF9000"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
            _legPaint = new PaintBrush
            {
                Color = new Color("#7F7F7F"),
                Style = PaintStyle.Fill,
                IsAntialias = true
            };
        }
        public void DrawBench(ICanvas canvas, int seatsWide)
        {
            if (seatsWide < 1) return;

            float width = BenchWidthRatio * seatsWide * _parameters.CellSize;
            float slatWidth = width * SlatBackInsetRatio;
            float height = BenchHeightRatio * _parameters.CellSize;

            float backHeight = height * BackRatio;
            float remaining = height - backHeight;

            float slatHeight = remaining / (Slats + (Slats - 1) * SlatGapRatio);
            float gapHeight = (remaining - slatHeight * Slats) / (Slats - 1);

            float legs = seatsWide + 1;
            float legWidth = LegWidthHeightRatio * height;
            float legArea = slatWidth * LegSlatInsetRatio - legWidth;
            float legSteps = legArea / (legs - 1);
            float legXOffset = -legArea / 2.0f - legWidth / 2.0f;

            // Draw legs
            for (int i=0; i< legs; i++)
            {
                float x = legXOffset + i * legSteps;
                canvas.DrawRect(x, 0, legWidth, height, _legPaint);
            }

            // Draw slats
            for (int i=0; i < Slats; i++)
            {
                float y = backHeight + (slatHeight + gapHeight) * i;
                canvas.DrawRect(-slatWidth / 2.0f, y, slatWidth, slatHeight, _slatPaint);
            }

            // Draw back
            canvas.DrawRect(-width / 2.0f, 0, width, backHeight, _backPaint);
        }
    }
}
