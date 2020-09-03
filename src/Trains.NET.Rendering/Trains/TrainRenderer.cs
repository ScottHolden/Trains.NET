using Trains.NET.Engine;
using Trains.NET.Rendering.Trains;

namespace Trains.NET.Rendering
{
    public class TrainRenderer : IRenderer<Train>
    {
        private readonly ITrainParameters _trainParameters;
        private readonly ITrainPainter _trainPainter;

        public TrainRenderer(ITrainParameters trainParameters, ITrainPainter trainPainter)
        {
            _trainParameters = trainParameters;
            _trainPainter = trainPainter;
        }

        public void Render(ICanvas canvas, Train train)
        {
            TrainPalette? palette = _trainPainter.GetPalette(train);

            SetupCanvasToDrawTrain(canvas, train);

            var outline = new PaintBrush
            {
                Color = palette.OutlineColor,
                Style = PaintStyle.Stroke,
                StrokeWidth = _trainParameters.StrokeWidth
            };

            var smokeStack = new PaintBrush
            {
                Color = palette.OutlineColor,
                Style = PaintStyle.Fill,
                StrokeWidth = _trainParameters.StrokeWidth
            };

            float startPos = -((_trainParameters.HeadWidth + _trainParameters.RearWidth) / 2);

            canvas.GradientRect(startPos,
                            -(_trainParameters.RearHeight / 2),
                            _trainParameters.RearWidth,
                            _trainParameters.RearHeight,

                            palette.RearSectionStartColor, palette.RearSectionEndColor);

            canvas.GradientRect(startPos + _trainParameters.RearWidth,
                            -(_trainParameters.HeadHeight / 2),
                            _trainParameters.HeadWidth,
                            _trainParameters.HeadHeight,

                            palette.FrontSectionStartColor, palette.FrontSectionEndColor);

            canvas.DrawRect(startPos,
                            -(_trainParameters.RearHeight / 2),
                            _trainParameters.RearWidth,
                            _trainParameters.RearHeight,
                            outline);

            canvas.DrawRect(startPos + _trainParameters.RearWidth,
                            -(_trainParameters.HeadHeight / 2),
                            _trainParameters.HeadWidth,
                            _trainParameters.HeadHeight,
                            outline);

            canvas.DrawCircle(startPos + _trainParameters.RearWidth + _trainParameters.HeadWidth - _trainParameters.SmokeStackOffset, 0, _trainParameters.SmokeStackRadius, smokeStack);

            Cat(canvas, palette.FrontSectionEndColor);
        }

        private static void Cat(ICanvas canvas, Color color)
        {
            var a = new PaintBrush
            {
                Style = PaintStyle.Fill,
                Color = color
            };
            var b = new PaintBrush
            {
                Style = PaintStyle.Fill,
                Color = Colors.VeryDarkGray
            };
            var c = new PaintBrush
            {
                Style = PaintStyle.Stroke,
                Color = Colors.VeryDarkGray,
                StrokeWidth = 3
            };

            canvas.DrawCircle(41.35f, 0, 30, a);
            canvas.DrawCircle(36.55f, -10, 5, b);
            canvas.DrawCircle(36.55f, 10, 5, b);
            canvas.DrawLine(53.75f, 5, 43.75f, 40, c);
            canvas.DrawLine(53.75f, 5, 53.75f, 40, c);
            canvas.DrawLine(53.75f, 5, 63.75f, 40, c);
            canvas.DrawLine(53.75f, -5, 43.75f, -40, c);
            canvas.DrawLine(53.75f, -5, 53.75f, -40, c);
            canvas.DrawLine(53.75f, -5, 63.75f, -40, c);
        }

        public static void SetupCanvasToDrawTrain(ICanvas canvas, IMovable train)
        {
            float x = 100 * train.RelativeLeft;
            float y = 100 * train.RelativeTop;
            canvas.Translate(x, y);
            canvas.RotateDegrees(train.Angle);
        }
    }
}
