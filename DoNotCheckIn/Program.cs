using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Trains.NET.Engine;
using Trains.NET.Rendering;
using Trains.NET.Rendering.Software;
using Trains.NET.Rendering.TrainPalettes;

namespace Trains.NET
{
	class Program
	{
		static void Main(string[] args)
		{
            /*
            IGameBoard gameBoard = new GameBoard(null, null);
            ITrackParameters param = new TrackParameters();
            IPixelMapper pm = new PixelMapper(param);
            OrderedList<ITrainPalette> trainPalettes = new OrderedList<ITrainPalette>(new ITrainPalette[] {
                new Blue()
            });
            OrderedList <ILayerRenderer> layers = new OrderedList<ILayerRenderer>(new ILayerRenderer[] {
                new GridRenderer(param),
                new TrackLayoutRenderer(gameBoard, new TrackRenderer(param, new SoftwarePathFactory()), pm, param),
                new TrainsRenderer(gameBoard, new TrainRenderer(param, new TrainParameters(), trainPalettes), pm)
            });
            IGame game = new Game(gameBoard, layers, pm);

            gameBoard.AddTrack(1, 1);
            gameBoard.AddTrack(1, 2);
            gameBoard.AddTrack(2, 2);
            gameBoard.AddTrack(2, 3);
            gameBoard.AddTrack(2, 4);
            gameBoard.AddTrack(2, 5);
            */
            int width = 300;
            int height = 300;

            SoftwareCanvas sw = new SoftwareCanvas(width, height);

            sw.RotateDegrees(180, width / 2, height / 2);

            sw.DrawLine(15, 10, 285, 10, new PaintBrush { Color = Colors.DarkGreen });
            sw.DrawLine(10, 15, 10, 285, new PaintBrush { Color = Colors.DarkBlue });
            sw.DrawCircle(100, 100, 20, new PaintBrush { Color = Colors.DarkGreen, Style = PaintStyle.Stroke });
            sw.DrawCircle(200, 100, 20, new PaintBrush { Color = Colors.DarkBlue, Style = PaintStyle.Fill });
            sw.DrawRect(80, 180, 40, 40, new PaintBrush { Color = Colors.DarkBlue, Style = PaintStyle.Stroke });
            sw.DrawRect(180, 180, 40, 40, new PaintBrush { Color = Colors.DarkGreen, Style = PaintStyle.Fill });

            //game.Render(sw);

            using (MemoryStream ms = new MemoryStream())
            {
                sw.DrawToStream(ms, SoftwarePixelFormat.Argb32);
                ms.Flush();
                using (Stream s = File.OpenWrite("C:\\temp\\game.png"))
                {
                    Image.LoadPixelData<Argb32>(ms.ToArray(), width, height).SaveAsPng(s);
                }
            }
		}
	}
}
