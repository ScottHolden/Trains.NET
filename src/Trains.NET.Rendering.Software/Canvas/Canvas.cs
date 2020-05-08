using System.Collections.Generic;

namespace Trains.NET.Rendering.Software
{
    
    internal partial class Canvas
    {
        private const int PixelDensity = 1;
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

        private void DrawNormalisedPoint(NormalisedPoint normalisedPoint, Pixel source) => _canvas[normalisedPoint.ToIndex(_width)] = source;
    }
}
