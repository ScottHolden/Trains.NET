namespace Trains.NET.Rendering
{
    public class PaintBrush
    {
        public Color? Color { get; set; }
        public PaintStyle? Style { get; set; }
        public int? TextSize { get; set; }
        public TextAlign? TextAlign { get; set; }
        public int? StrokeWidth { get; set; }
        public bool? IsAntialias { get; set; }
    }
}
