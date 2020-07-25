using Trains.NET.Engine;
using Trains.NET.Instrumentation;

namespace Trains.NET.Rendering
{
    [Order(1000)]
    internal class DiagnosticsRenderer : ILayerRenderer
    {
        private const int TextSize = 16;
        private readonly PaintBrush _paint = new PaintBrush
        {
            Color = Colors.Black,
            TextSize = TextSize,
            TextAlign = TextAlign.Left,
        };
        private readonly PaintBrush _boxPaint = new PaintBrush
        {
            Color = new Color("#DDFFFFFF"),
            Style = PaintStyle.Fill
        };

        public bool Enabled { get; set; }

        public string Name => "Diagnostics";

        public DiagnosticsRenderer()
        {
        }

        public void Render(ICanvas canvas, int width, int height)
        {
            int i = 1;

            canvas.DrawRect(0, 0, TextSize * 20, TextSize * 30, _boxPaint);

            foreach((string name, IStat stat) in InstrumentationBag.Stats)
            {
                if (stat.ShouldShow())
                {
                    canvas.DrawText(name + ": " + stat.GetDescription(), 0, i++ * 25, _paint);
                }
            }
        }
    }
}
