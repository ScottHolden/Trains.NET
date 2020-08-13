﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SkiaSharp;
using Trains.NET.Engine;
using Trains.NET.Rendering;
using Trains.NET.Rendering.Skia;
using Trains.NET.Rendering.Trains;

namespace Trains.Emoji
{
    public class EmojiDrawer
    {
        private readonly int _size;
        private readonly IRenderer<Tree> _tree;
        private readonly IRenderer<Track> _track;
        private readonly (string color, IRenderer<Train> renderer)[] _trains;
        private const string FolderName = "EmojiOutput";

        public static void Main(string[] args) => new EmojiDrawer().Save(ShouldZip(args));

        private static bool ShouldZip(string[] args) => args.Any(x =>
                                                            x.Equals("-z", StringComparison.OrdinalIgnoreCase) ||
                                                            x.Equals("--zip", StringComparison.OrdinalIgnoreCase));

        public EmojiDrawer()
        {
            IGameParameters gameParameters = new GameParameters();
            ITrackParameters trackParameters = new TrackParameters();
            ITrainParameters trainParameters = new TrainParameters();
            IImageFactory imageFactory = new SKImageFactory();
            IPathFactory pathFactory = new SKPathFactory();
            ITrackPathBuilder trackPathBuilder = new TrackPathBuilder(gameParameters, trackParameters, pathFactory);

            _tree = new TreeRenderer(imageFactory, gameParameters);
            _track = new TrackRenderer(trackParameters, gameParameters, imageFactory, trackPathBuilder);

            _trains = typeof(ITrainPalette).Assembly.GetTypes()
                        .Where(x => !x.IsInterface && !x.IsAbstract && typeof(ITrainPalette).IsAssignableFrom(x) && x.GetConstructor(Type.EmptyTypes) != null)
                        .Select(x => (ITrainPalette)Activator.CreateInstance(x)!)
                        .Select(x => (x.GetType().Name, (IRenderer<Train>)new TrainRenderer(gameParameters, trainParameters, new TrainPainter(new OrderedList<ITrainPalette>(new object[] { x })))))
                        .ToArray();

            _size = gameParameters.CellSize;
        }

        public void Save(bool zip)
        {
            if (!Directory.Exists(FolderName))
            {
                Directory.CreateDirectory(FolderName);
            }
            for (int i = 0; i < 3; i++)
            {
                Draw("tree" + i, x => _tree.Render(x, new Tree()));
            }
            foreach (TrackDirection direction in (TrackDirection[])Enum.GetValues(typeof(TrackDirection)))
            {
                Draw("track" + direction, x => _track.Render(x, new Track() { Direction = direction }));
            }
            foreach ((TrackDirection direction, Track track)  in ((TrackDirection[])Enum.GetValues(typeof(TrackDirection)))
                                                    .Select(x => (Direction: x, Track: new Track() { Direction = x, AlternateState = true }))
                                                    .Where(x => x.Track.HasAlternateState()))
            {
                Draw("trackAlt" + direction, x => _track.Render(x, track));
            }
            foreach ((string color, IRenderer<Train> trainRenderer) in _trains)
            {
                DrawTrain($"train{color}Up", TrackDirection.Vertical, 270, trainRenderer);
                DrawTrain($"train{color}Down", TrackDirection.Vertical, 90, trainRenderer);
                DrawTrain($"train{color}Left", TrackDirection.Horizontal, 180, trainRenderer);
                DrawTrain($"train{color}Right", TrackDirection.Horizontal, 0, trainRenderer);
            }
            if (zip)
            {
                string zipFilename = FolderName + ".zip";
                if (File.Exists(zipFilename))
                {
                    File.Delete(zipFilename);
                }
                ZipFile.CreateFromDirectory(FolderName, zipFilename);
            }
        }

        public void Draw(string name, Action<ICanvas> x)
        {
            using var bitmap = new SKBitmap(_size, _size, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            using var skCanvas = new SKCanvas(bitmap);
            using (ICanvas canvas = new SKCanvasWrapper(skCanvas))
            {
                canvas.Save();
                x(canvas);
                canvas.Restore();
            }
            using Stream s = File.OpenWrite(FolderName + "\\" + name + ".png");
            bitmap.Encode(s, SKEncodedImageFormat.Png, 100);
        }

        public void DrawTrain(string name, TrackDirection trackDirection, float angle, IRenderer<Train> trainRenderer) =>
            Draw(name, x =>
            {
                x.Save();
                _track.Render(x, new Track() { Direction = trackDirection });
                x.Restore();
                var train = new Train();
                train.SetAngle(angle);
                trainRenderer.Render(x, train);
            });
    }
}
