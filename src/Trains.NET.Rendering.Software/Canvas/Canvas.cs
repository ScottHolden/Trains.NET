using System.Collections.Generic;

namespace Trains.NET.Rendering.Software
{
    
    internal partial class Canvas
    {
        private const float PixelDensity = 1;
        private readonly int _width;
        private readonly int _height;
        private readonly Pixel[] _canvas;

        public Canvas(int width, int height)
        {
            _currentTransform = new Transform();
            _transformStack = new Stack<Transform>();
            _width = width;
            _height = height;
            _canvas = new Pixel[_width * _height];
        }

        public void Clear(Pixel source)
        {
            // TODO: Implement Copy doubling method, this is too slow!
            for (int i = 0; i < _canvas.Length; i++)
            {
                _canvas[i] = source;
            }
        }

        private void DrawNormalisedPoint(NormalisedPoint normalisedPoint, Pixel source)
        {
            int x = (int)normalisedPoint.X;
            int y = (int)normalisedPoint.Y;
            int index = y * _width + x;
            if (x >= 0 && x < _width &&
                y >= 0 && y < _height &&
                index < _canvas.Length)
            {
                _canvas[index] = source;
            }
        }
    }
}
