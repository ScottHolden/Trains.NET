using System;
using System.IO;
using SkiaSharp;

namespace Trains.NET.Rendering.Skia
{
    public class DrawOut : IDrawOut
    {
        private readonly int _size;
        private readonly ITreeRenderer _tree;
        private readonly ITrackRenderer _track;
        private readonly ITrainRenderer _train;

        public DrawOut(ITrackParameters trackParameters, ITreeRenderer tree, ITrackRenderer track, ITrainRenderer train)
        {
            _size = trackParameters.CellSize;
            _tree = tree;
            _track = track;
            _train = train;
        }

        public void Draw(string name, Action<ICanvas> x)
        {
            using (SKBitmap bitmap = new SKBitmap(_size, _size, SKImageInfo.PlatformColorType, SKAlphaType.Premul))
            {
                using (ICanvas canvas = new SKCanvasWrapper(new SKCanvas(bitmap)))
                {
                    canvas.Save();
                    x(canvas);
                    canvas.Restore();
                }
                using (Stream s = File.OpenWrite(name + ".png"))
                {
                    bitmap.Encode(s, SKEncodedImageFormat.Png, 100);
                }
            }
        }
        public void Save()
        {
            for(int i=0; i< 3; i++)
            {
                Draw("tree" + i, x => _tree.Render(x, i));
            }
            foreach (Engine.TrackDirection direction in (Engine.TrackDirection[])Enum.GetValues(typeof(Engine.TrackDirection)))
            {
                Draw("track" + direction, x => _track.Render(x, new Engine.Track(null) { Direction = direction }));
            }
            DrawTrain("trainUp", Engine.TrackDirection.Vertical, 270);
            DrawTrain("trainDown", Engine.TrackDirection.Vertical, 90);
            DrawTrain("trainLeft", Engine.TrackDirection.Horizontal, 180);
            DrawTrain("trainRight", Engine.TrackDirection.Horizontal, 0);
        }
        public void DrawTrain(string name, Engine.TrackDirection trackDirection, float angle) =>
            Draw(name, x => {
                x.Save();
                _track.Render(x, new Engine.Track(null) { Direction = trackDirection });
                x.Restore();
                var train = new Engine.Train();
                train.SetAngle(angle);
                _train.Render(x, train);
            });
    }
    public interface IDrawOut
    {
        void Save();
    }
}
